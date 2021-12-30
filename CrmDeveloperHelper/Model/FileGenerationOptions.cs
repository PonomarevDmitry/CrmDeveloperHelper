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

        private const int _defaultGenerateCommonSpaceCount = 4;

        private int _GenerateCommonSpaceCount = _defaultGenerateCommonSpaceCount;
        /// <summary>
        /// Количество пробелов для отступа в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public int GenerateCommonSpaceCount
        {
            get => _GenerateCommonSpaceCount;
            set
            {
                if (_GenerateCommonSpaceCount == value)
                {
                    return;
                }

                if (value <= 0)
                {
                    value = _defaultGenerateCommonSpaceCount;
                }

                this.OnPropertyChanging(nameof(GenerateCommonSpaceCount));
                this._GenerateCommonSpaceCount = value;
                this.OnPropertyChanged(nameof(GenerateCommonSpaceCount));
            }
        }

        private IndentType _GenerateCommonIndentType = IndentType.Spaces;
        /// <summary>
        /// Тип отступа в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public IndentType GenerateCommonIndentType
        {
            get => _GenerateCommonIndentType;
            set
            {
                if (_GenerateCommonIndentType == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(GenerateCommonIndentType));
                this._GenerateCommonIndentType = value;
                this.OnPropertyChanged(nameof(GenerateCommonIndentType));
            }
        }

        private bool _GenerateCommonAllDescriptions = true;
        /// <summary>
        /// Генерировать все описания (description) или только первое по приоритету в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateCommonAllDescriptions
        {
            get => _GenerateCommonAllDescriptions;
            set
            {
                this.OnPropertyChanging(nameof(GenerateCommonAllDescriptions));
                this._GenerateCommonAllDescriptions = value;
                this.OnPropertyChanged(nameof(GenerateCommonAllDescriptions));
            }
        }

        #region INotifyPropertyChanging, INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;

            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnPropertyChanging(string propertyName)
        {
            PropertyChangingEventHandler propertyChanging = this.PropertyChanging;

            if (propertyChanging != null)
            {
                propertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanging, INotifyPropertyChanged

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

            this.GenerateCommonSpaceCount = diskData.GenerateCommonSpaceCount;
            this.GenerateCommonIndentType = diskData.GenerateCommonIndentType;

            this.GenerateCommonAllDescriptions = diskData.GenerateCommonAllDescriptions;

            this.NamespaceClassesCSharp = diskData.NamespaceClassesCSharp;

            this.SolutionComponentWithManagedInfo = diskData.SolutionComponentWithManagedInfo;
            this.TypeConverterName = diskData.TypeConverterName;

            this.LoadFromDiskJavaScript(diskData);

            this.LoadFromDiskEntityProxyClass(diskData);
            this.LoadFromDiskEntitySchemaClass(diskData);

            this.LoadFromDiskGlobalOptionSetSchema(diskData);

            this.LoadFromDiskSdkMessageRequest(diskData);
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

        public string GetTabSpacer()
        {
            if (this.GenerateCommonSpaceCount <= 0)
            {
                this.GenerateCommonSpaceCount = _defaultGenerateCommonSpaceCount;
            }

            switch (this.GenerateCommonIndentType)
            {
                case IndentType.Tab:
                    return "\t";

                case IndentType.Spaces:
                default:
                    return new string(' ', this.GenerateCommonSpaceCount);
            }
        }
    }
}
