using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Intellisense.Model
{
    [DataContract]
    public class ConnectionWebResourceIntellisenseData
    {
        private string FilePath { get; set; }

        [DataMember]
        public Guid ConnectionId { get; set; }

        [DataMember]
        public ConcurrentDictionary<string, WebResourceIntellisenseData> WebResourcesAll { get; private set; }

        public ConnectionWebResourceIntellisenseData()
        {
            CreateCollectionIfNeaded();
        }

        public ConnectionWebResourceIntellisenseData(Guid connectionId)
        {
            this.ConnectionId = connectionId;

            CreateCollectionIfNeaded();
        }

        [OnDeserializing]
        private void BeforeDeserialize(StreamingContext context)
        {
            CreateCollectionIfNeaded();
        }

        private void CreateCollectionIfNeaded()
        {
            if (WebResourcesAll == null)
            {
                this.WebResourcesAll = new ConcurrentDictionary<string, WebResourceIntellisenseData>(StringComparer.InvariantCultureIgnoreCase);
            }
        }

        public IEnumerable<WebResourceIntellisenseData> GetHtmlWebResources()
        {
            return WebResourcesAll.Values.Where(w => w.WebResourceType != null && w.WebResourceType.Value == (int)WebResource.Schema.OptionSets.webresourcetype.Webpage_HTML_1);
        }

        public IEnumerable<WebResourceIntellisenseData> GetJavaScriptWebResources()
        {
            return WebResourcesAll.Values.Where(w => w.WebResourceType != null && w.WebResourceType.Value == (int)WebResource.Schema.OptionSets.webresourcetype.Script_JScript_3);
        }

        private static ConnectionWebResourceIntellisenseData Get(Guid connectionId)
        {
            var filePath = GetFilePath(connectionId);

            ConnectionWebResourceIntellisenseData result = null;

            if (File.Exists(filePath))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(ConnectionWebResourceIntellisenseData));

                using (Mutex mutex = new Mutex(false, FileOperations.GetMutexName(filePath)))
                {
                    try
                    {
                        mutex.WaitOne();

                        using (var sr = File.OpenRead(filePath))
                        {
                            result = ser.ReadObject(sr) as ConnectionWebResourceIntellisenseData;
                            result.FilePath = filePath;
                            result.ConnectionId = connectionId;
                        }
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToLog(ex);

                        FileOperations.CreateBackUpFile(filePath, ex);
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
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
            byte[] fileBody = null;

            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    DataContractSerializer ser = new DataContractSerializer(typeof(ConnectionWebResourceIntellisenseData));

                    ser.WriteObject(memoryStream, this);

                    memoryStream.Seek(0, SeekOrigin.Begin);

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
                using (Mutex mutex = new Mutex(false, FileOperations.GetMutexName(filePath)))
                {
                    try
                    {
                        mutex.WaitOne();

                        try
                        {
                            using (var stream = File.Open(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                stream.Write(fileBody, 0, fileBody.Length);
                            }
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToLog(ex);
                        }
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
            }
        }

        private static string GetFilePath(Guid connectionId)
        {
            string folderPath = FileOperations.GetConnectionIntellisenseDataFolderPath(connectionId);

            var fileName = "WebResourceIntellisenseData.xml";

            var filePath = Path.Combine(folderPath, fileName);

            return filePath;
        }

        private void MergeDataFromDisk(ConnectionWebResourceIntellisenseData data)
        {
            if (data == null)
            {
                return;
            }

            CreateCollectionIfNeaded();

            foreach (var webResourceData in data.WebResourcesAll.Values)
            {
                if (!this.WebResourcesAll.ContainsKey(webResourceData.Name))
                {
                    this.WebResourcesAll.TryAdd(webResourceData.Name, webResourceData);
                }
                else
                {
                    this.WebResourcesAll[webResourceData.Name].MergeDataFromDisk(webResourceData);
                }
            }
        }

        internal void GetDataFromDisk()
        {
            var data = Get(this.ConnectionId);

            if (data != null)
            {
                this.MergeDataFromDisk(data);
            }
        }
    }
}