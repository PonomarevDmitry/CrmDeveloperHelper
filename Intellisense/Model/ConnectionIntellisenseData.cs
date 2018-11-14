using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    [DataContract]
    public class ConnectionIntellisenseData
    {
        private static ConcurrentDictionary<Guid, object> _cacheSyncObjectFile = new ConcurrentDictionary<Guid, object>();

        private static object GetFileSyncObject(Guid connectionId)
        {
            if (!_cacheSyncObjectFile.ContainsKey(connectionId))
            {
                _cacheSyncObjectFile.TryAdd(connectionId, new object());
            }

            return _cacheSyncObjectFile[connectionId];
        }

        private const int _savePeriodInMinutes = 5;

        private string FilePath { get; set; }

        public DateTime? NextSaveFileDate { get; set; }

        [DataMember]
        public Guid ConnectionId { get; set; }

        [DataMember]
        public ConcurrentDictionary<string, EntityIntellisenseData> Entities { get; private set; }

        public ConnectionIntellisenseData()
        {
            this.Entities = new ConcurrentDictionary<string, EntityIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
        }

        public void LoadFullData(IEnumerable<EntityMetadata> entityMetadataList)
        {
            foreach (var entityMetadata in entityMetadataList)
            {
                if (!this.Entities.ContainsKey(entityMetadata.LogicalName))
                {
                    this.Entities.TryAdd(entityMetadata.LogicalName, new EntityIntellisenseData());
                }

                this.Entities[entityMetadata.LogicalName].LoadData(entityMetadata);
            }

            SaveIntellisenseDataByTime();
        }

        public void LoadData(EntityMetadata entityMetadata)
        {
            if (entityMetadata == null)
            {
                return;
            }

            if (!this.Entities.ContainsKey(entityMetadata.LogicalName))
            {
                this.Entities.TryAdd(entityMetadata.LogicalName, new EntityIntellisenseData());
            }

            this.Entities[entityMetadata.LogicalName].LoadData(entityMetadata);

            SaveIntellisenseDataByTime();
        }

        [OnDeserializing]
        private void BeforeDeserialize(StreamingContext context)
        {
            if (Entities == null)
            {
                this.Entities = new ConcurrentDictionary<string, EntityIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
            }
        }

        private void SaveIntellisenseDataByTime()
        {
            bool saveData = !this.NextSaveFileDate.HasValue || this.NextSaveFileDate < DateTime.Now;

            if (saveData)
            {
                this.Save();
            }
        }

        public static ConnectionIntellisenseData Get(Guid connectionId)
        {
            var filePath = GetFilePath(connectionId);

            ConnectionIntellisenseData result = null;

            if (File.Exists(filePath))
            {
                lock (GetFileSyncObject(connectionId))
                {
                    DataContractSerializer ser = new DataContractSerializer(typeof(ConnectionIntellisenseData));

                    try
                    {
                        using (var sr = File.OpenRead(filePath))
                        {
                            result = ser.ReadObject(sr) as ConnectionIntellisenseData;
                            result.FilePath = filePath;
                            result.ConnectionId = connectionId;
                        }
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(ex);

                        FileOperations.CreateBackUpFile(filePath, ex);
                    }
                }
            }

            return result;
        }

        public void Save()
        {
            string filePath = GetFilePath(this.ConnectionId);

            if (!string.IsNullOrEmpty(FilePath))
            {
                filePath = FilePath;
            }

            this.Save(filePath);
        }

        private void Save(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (this.Entities.Any())
            {
                this.NextSaveFileDate = DateTime.Now.AddMinutes(_savePeriodInMinutes);

                byte[] fileBody = null;

                using (var memoryStream = new MemoryStream())
                {
                    try
                    {
                        DataContractSerializer ser = new DataContractSerializer(typeof(ConnectionIntellisenseData));

                        ser.WriteObject(memoryStream, this);

                        fileBody = memoryStream.ToArray();
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToLog(ex);

                        fileBody = null;
                    }
                }

                if (fileBody != null)
                {
                    lock (GetFileSyncObject(this.ConnectionId))
                    {
                        try
                        {
                            try
                            {

                            }
                            finally
                            {
                                File.WriteAllBytes(filePath, fileBody);
                            }
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToLog(ex);
                        }
                    }
                }
            }
        }

        private static string GetFilePath(Guid connectionId)
        {
            var fileName = string.Format("IntellisenseData.{0}.xml", connectionId.ToString());

            var filePath = FileOperations.GetConnectionIntellisenseDataFullFilePath(fileName);

            return filePath;
        }

        public void MergeDataFromDisk(ConnectionIntellisenseData data)
        {
            if (data == null)
            {
                return;
            }

            if (data.Entities != null)
            {
                if (this.Entities == null)
                {
                    this.Entities = new ConcurrentDictionary<string, EntityIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
                }

                foreach (var entityData in data.Entities.Values)
                {
                    if (!this.Entities.ContainsKey(entityData.EntityLogicalName))
                    {
                        this.Entities.TryAdd(entityData.EntityLogicalName, entityData);
                    }
                    else
                    {
                        this.Entities[entityData.EntityLogicalName].MergeDataFromDisk(entityData);
                    }
                }
            }
        }
    }
}