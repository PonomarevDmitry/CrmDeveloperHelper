using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public partial class FileGenerationOptions : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public FileGenerationOptions()
        {
            this.NamespaceClassesCSharp = nameof(NamespaceClassesCSharp);
            this.NamespaceGlobalOptionSetsCSharp = nameof(NamespaceGlobalOptionSetsCSharp);
        }

        public FileGenerationConfiguration Configuration { get; set; }

        public string SolutionFilePath { get; set; }

        private bool _SolutionComponentWithManagedInfo = false;
        [DataMember]
        public bool SolutionComponentWithManagedInfo
        {
            get => _SolutionComponentWithManagedInfo;
            set
            {
                this.OnPropertyChanging(nameof(SolutionComponentWithManagedInfo));
                this._SolutionComponentWithManagedInfo = value;
                this.OnPropertyChanged(nameof(SolutionComponentWithManagedInfo));
            }
        }

        private string _TypeConverterName;
        [DataMember]
        public string TypeConverterName
        {
            get => _TypeConverterName;
            set
            {
                this.OnPropertyChanging(nameof(TypeConverterName));

                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }
                else
                {
                    value = string.Empty;
                }

                this._TypeConverterName = value;
                this.OnPropertyChanged(nameof(TypeConverterName));
            }
        }

        private string _NamespaceClassesCSharp;
        /// <summary>
        /// Пространство имен для класса с метаданными сущности
        /// </summary>
        [DataMember]
        public string NamespaceClassesCSharp
        {
            get => _NamespaceClassesCSharp;
            set
            {
                this.OnPropertyChanging(nameof(NamespaceClassesCSharp));

                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }
                else
                {
                    value = string.Empty;
                }

                this._NamespaceClassesCSharp = value;
                this.OnPropertyChanged(nameof(NamespaceClassesCSharp));
            }
        }

        private string _NamespaceGlobalOptionSetsCSharp;
        [DataMember]
        public string NamespaceGlobalOptionSetsCSharp
        {
            get => _NamespaceGlobalOptionSetsCSharp;
            set
            {
                this.OnPropertyChanging(nameof(NamespaceGlobalOptionSetsCSharp));

                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }
                else
                {
                    value = string.Empty;
                }

                this._NamespaceGlobalOptionSetsCSharp = value;
                this.OnPropertyChanged(nameof(NamespaceGlobalOptionSetsCSharp));
            }
        }

        private string _NamespaceSdkMessagesCSharp;
        [DataMember]
        public string NamespaceSdkMessagesCSharp
        {
            get => _NamespaceSdkMessagesCSharp;
            set
            {
                this.OnPropertyChanging(nameof(NamespaceSdkMessagesCSharp));

                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }
                else
                {
                    value = string.Empty;
                }

                this._NamespaceSdkMessagesCSharp = value;
                this.OnPropertyChanged(nameof(NamespaceSdkMessagesCSharp));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        [OnDeserialized]
        private void AfterDeserialize(StreamingContext context)
        {
            if (this._GenerateCommonSpaceCount <= 0)
            {
                this._GenerateCommonSpaceCount = _defaultGenerateCommonSpaceCount;
            }
        }

        public void LoadFromDisk(FileGenerationOptions diskData)
        {
            if (diskData == null)
            {
                return;
            }

            this.NamespaceClassesCSharp = diskData.NamespaceClassesCSharp;
            this.NamespaceGlobalOptionSetsCSharp = diskData.NamespaceGlobalOptionSetsCSharp;
            this.NamespaceSdkMessagesCSharp = diskData.NamespaceSdkMessagesCSharp;

            this.SolutionComponentWithManagedInfo = diskData.SolutionComponentWithManagedInfo;
            this.TypeConverterName = diskData.TypeConverterName;

            this.LoadFromDiskEntitySchema(diskData);
            this.LoadFromDiskGlobalOptionSetSchema(diskData);

            this.LoadFromDiskProxyClass(diskData);
        }

        public FileGenerationOptions Clone()
        {
            var result = new FileGenerationOptions();

            result.LoadFromDisk(this);

            return result;
        }

        public static FileGenerationOptions LoadFromPath(string filePath)
        {
            FileGenerationOptions result = null;

            if (File.Exists(filePath))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(FileGenerationOptions));

                using (Mutex mutex = new Mutex(false, FileOperations.GetMutexName(filePath)))
                {
                    try
                    {
                        mutex.WaitOne();

                        using (var sr = File.OpenRead(filePath))
                        {
                            result = ser.ReadObject(sr) as FileGenerationOptions;
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

        public void Save(string filePath)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(FileGenerationOptions));

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
                    Helpers.DTEHelper.WriteExceptionToLog(ex);

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
    }
}
