using Microsoft.Xrm.Sdk.Metadata;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    public class ConnectionIntellisenseData
    {
        public Guid ConnectionId { get; set; }

        public ConcurrentDictionary<string, EntityIntellisenseData> Entities { get; private set; }

        public ConnectionIntellisenseData()
        {
            this.Entities = new ConcurrentDictionary<string, EntityIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
        }

        public void LoadSomeData(IEnumerable<EntityMetadata> entityMetadataList)
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

        public void LoadFullData(IEnumerable<EntityMetadata> entityMetadataList)
        {
            var hashSet = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

            foreach (var entityMetadata in entityMetadataList)
            {
                hashSet.Add(entityMetadata.LogicalName);

                if (!this.Entities.ContainsKey(entityMetadata.LogicalName))
                {
                    this.Entities.TryAdd(entityMetadata.LogicalName, new EntityIntellisenseData());
                }

                this.Entities[entityMetadata.LogicalName].LoadData(entityMetadata);
            }

            foreach (var entityName in this.Entities.Keys.ToList())
            {
                if (!hashSet.Contains(entityName))
                {
                    this.Entities.TryRemove(entityName, out _);
                }
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

        public void GetDataFromDisk()
        {
            var directory = GetFolderPath(this.ConnectionId);

            if (this.Entities == null)
            {
                this.Entities = new ConcurrentDictionary<string, EntityIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
            }

            var directoryInfo = new DirectoryInfo(directory);

            if (directoryInfo.Exists)
            {
                var files = directoryInfo.GetFiles("*.xml");

                Parallel.ForEach(files, file =>
                {
                    var entityData = EntityIntellisenseData.Get(file.FullName);

                    if (entityData != null)
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
                });
            }
        }

        public void Save()
        {
            string directory = GetFolderPath(this.ConnectionId);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            this.Save(directory);
        }

        private void Save(string directory)
        {
            if (this.Entities.Any())
            {
                Parallel.ForEach(this.Entities.Values, entityData => entityData.Save(directory));
            }
        }

        private void SaveIntellisenseDataByTime()
        {
            string directory = GetFolderPath(this.ConnectionId);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            Parallel.ForEach(this.Entities.Values, entityData =>
            {
                bool saveData = !entityData.NextSaveFileDate.HasValue || entityData.NextSaveFileDate < DateTime.Now;

                if (saveData)
                {
                    entityData.Save(directory);
                }
            });
        }

        private static string GetFolderPath(Guid connectionId)
        {
            var folderName = string.Format("IntellisenseData.{0}", connectionId.ToString());

            var folderPath = FileOperations.GetConnectionIntellisenseDataFolderPath(folderName);

            return folderPath;
        }
    }
}