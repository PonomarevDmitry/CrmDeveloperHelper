using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class FileGenerationConfiguration
    {
        private static readonly object _syncObject = new object();

        private FileSystemWatcher _watcher = null;

        /// <summary>
        /// Путь к файлу конфигурации
        /// </summary>
        public string Path { get; private set; }

        [DataMember]
        public FileGenerationOptions DefaultFileGenerationOptions { get; private set; }

        [DataMember]
        public Dictionary<string, FileGenerationOptions> FileGenerationOptionsCollection { get; private set; }

        public FileGenerationConfiguration()
        {
            this.DefaultFileGenerationOptions = new FileGenerationOptions()
            {
                SolutionFilePath = string.Empty,
                Configuration = this,
            };
            this.FileGenerationOptionsCollection = new Dictionary<string, FileGenerationOptions>(StringComparer.InvariantCultureIgnoreCase);
        }

        [OnDeserializing]
        private void BeforeDeserialize(StreamingContext context)
        {

            {
                this.FileGenerationOptionsCollection = new Dictionary<string, FileGenerationOptions>(StringComparer.InvariantCultureIgnoreCase);
            }
        }

        [OnDeserialized]
        private void AfterDeserialize(StreamingContext context)
        {
            if (this.DefaultFileGenerationOptions == null)
            {
                this.DefaultFileGenerationOptions = new FileGenerationOptions()
                {
                    SolutionFilePath = string.Empty,
                };
            }

            this.DefaultFileGenerationOptions.Configuration = this;

            if (this.FileGenerationOptionsCollection != null)
            {
                foreach (var item in this.FileGenerationOptionsCollection)
                {
                    item.Value.SolutionFilePath = item.Key;
                    item.Value.Configuration = this;
                }
            }
        }

        public static FileGenerationOptions GetFileGenerationOptions()
        {
            var configuration = GetStatic();

            var solutionFilePath = CrmDeveloperHelperPackage.Singleton?.ApplicationObject?.Solution?.FullName;

            FileGenerationOptions result = null;

            if (!string.IsNullOrEmpty(solutionFilePath))
            {
                if (configuration.FileGenerationOptionsCollection.ContainsKey(solutionFilePath))
                {
                    result = configuration.FileGenerationOptionsCollection[solutionFilePath];
                }
                else
                {
                    result = configuration.DefaultFileGenerationOptions.Clone();

                    configuration.FileGenerationOptionsCollection.Add(solutionFilePath, result);
                }

                result.SolutionFilePath = solutionFilePath;
            }
            else
            {
                result = configuration.DefaultFileGenerationOptions;
            }

            result.Configuration = configuration;

            configuration.Save();

            return result;
        }

        public static FileGenerationConfiguration GetConfiguration()
        {
            return GetStatic();
        }

        private static FileGenerationConfiguration _singleton;

        private static FileGenerationConfiguration GetStatic()
        {
            lock (_syncObject)
            {
                if (_singleton == null)
                {
                    string filePath = FileOperations.GetFileGenerationConfigurationFilePath();

                    FileGenerationConfiguration localFile = GetFromPath(filePath);

                    _singleton = localFile ?? new FileGenerationConfiguration();
                    _singleton.Path = filePath;
                }

                return _singleton;
            }
        }

        private static FileGenerationConfiguration GetFromPath(string filePath)
        {
            FileGenerationConfiguration result = null;

            if (File.Exists(filePath))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(FileGenerationConfiguration));

                using (Mutex mutex = new Mutex(false, FileOperations.GetMutexName(filePath)))
                {
                    try
                    {
                        mutex.WaitOne();

                        using (var sr = File.OpenRead(filePath))
                        {
                            result = ser.ReadObject(sr) as FileGenerationConfiguration;
                        }
                    }
                    catch (Exception ex)
                    {
                        DTEHelper.WriteExceptionToOutput(null, ex);

                        FileOperations.CreateBackUpFile(filePath, ex);

                        result = null;
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
            }

            return result;
        }

        private void StartWatchFile()
        {
            if (_watcher != null)
            {
                return;
            }

            _watcher = new FileSystemWatcher()
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                Path = System.IO.Path.GetDirectoryName(this.Path),
                Filter = System.IO.Path.GetFileName(this.Path),
            };

            _watcher.Changed += _watcher_Changed;

            _watcher.EnableRaisingEvents = true;
        }

        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            var diskData = GetFromPath(this.Path);

            LoadFromDisk(diskData);
        }

        private void LoadFromDisk(FileGenerationConfiguration diskData)
        {
            this.FileGenerationOptionsCollection.Clear();

            foreach (var item in diskData.FileGenerationOptionsCollection)
            {
                this.FileGenerationOptionsCollection.Add(item.Key, item.Value);
            }

            this.DefaultFileGenerationOptions = diskData.DefaultFileGenerationOptions;
        }

        private void Save(string filePath)
        {
            this.Path = filePath;

            DataContractSerializer ser = new DataContractSerializer(typeof(FileGenerationConfiguration));

            byte[] fileBody = null;

            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    XmlWriterSettings settings = new XmlWriterSettings
                    {
                        Indent = true,
                        Encoding = Encoding.UTF8
                    };

                    using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, settings))
                    {
                        ser.WriteObject(xmlWriter, this);
                        xmlWriter.Flush();
                    }

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

                        if (_watcher != null)
                        {
                            _watcher.EnableRaisingEvents = false;
                        }

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

                        if (_watcher != null)
                        {
                            _watcher.EnableRaisingEvents = true;
                        }
                    }
                    finally
                    {
                        mutex.ReleaseMutex();
                    }
                }
            }
        }

        public void Save()
        {
            this.Save(this.Path);
        }

        public static void SaveConfiguration()
        {
            _singleton?.Save();
        }
    }
}
