using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public partial class CommonConfiguration : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static CommonConfiguration _singleton;

        private static readonly object _syncObject = new object();

        private FileSystemWatcher _watcher = null;

        /// <summary>
        /// Путь к файлу конфигурации
        /// </summary>
        public string Path { get; set; }

        private string _FolderForExport;
        /// <summary>
        /// Каталог для копирования файлов
        /// </summary>
        [DataMember]
        public string FolderForExport
        {
            get => _FolderForExport;
            set
            {
                this.OnPropertyChanging(nameof(FolderForExport));

                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }

                this._FolderForExport = value;
                this.OnPropertyChanged(nameof(FolderForExport));
            }
        }

        private bool _DoNotPropmPublishMessage;
        /// <summary>
        /// Не показывать сообщение при публикации
        /// </summary>
        [DataMember]
        public bool DoNotPropmPublishMessage
        {
            get => _DoNotPropmPublishMessage;
            set
            {
                this.OnPropertyChanging(nameof(DoNotPropmPublishMessage));
                this._DoNotPropmPublishMessage = value;
                this.OnPropertyChanged(nameof(DoNotPropmPublishMessage));
            }
        }

        private bool _ClearOutputWindowBeforeCRMOperation;
        /// <summary>
        /// Очищать Output Window в начале каждой операции CRM
        /// </summary>
        [DataMember]
        public bool ClearOutputWindowBeforeCRMOperation
        {
            get => _ClearOutputWindowBeforeCRMOperation;
            set
            {
                this.OnPropertyChanging(nameof(ClearOutputWindowBeforeCRMOperation));
                this._ClearOutputWindowBeforeCRMOperation = value;
                this.OnPropertyChanged(nameof(ClearOutputWindowBeforeCRMOperation));
            }
        }

        private string _CompareProgram;
        /// <summary>
        /// Программа для сравнения файлов
        /// </summary>
        [DataMember]
        public string CompareProgram
        {
            get => _CompareProgram;
            set
            {
                this.OnPropertyChanging(nameof(CompareProgram));

                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }

                this._CompareProgram = value;
                UpdateDifferenceProgramExists();
                UpdateDifferenceThreeWayAvaliable();
                this.OnPropertyChanged(nameof(CompareProgram));
            }
        }

        private string _CompareArgumentsFormat;
        /// <summary>
        /// Формат аргументов для программы сравнения
        /// </summary>
        [DataMember]
        public string CompareArgumentsFormat
        {
            get => _CompareArgumentsFormat;
            set
            {
                this.OnPropertyChanging(nameof(CompareArgumentsFormat));

                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }

                this._CompareArgumentsFormat = value;
                UpdateDifferenceProgramExists();
                this.OnPropertyChanged(nameof(CompareArgumentsFormat));
            }
        }

        private string _CompareArgumentsThreeWayFormat;
        [DataMember]
        public string CompareArgumentsThreeWayFormat
        {
            get => _CompareArgumentsThreeWayFormat;
            set
            {
                this.OnPropertyChanging(nameof(CompareArgumentsThreeWayFormat));

                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }

                this._CompareArgumentsThreeWayFormat = value;
                UpdateDifferenceThreeWayAvaliable();
                this.OnPropertyChanged(nameof(CompareArgumentsThreeWayFormat));
            }
        }

        private string _TextEditorProgram;
        /// <summary>
        /// Программа - текстовый редактор
        /// </summary>
        [DataMember]
        public string TextEditorProgram
        {
            get => _TextEditorProgram;
            set
            {
                this.OnPropertyChanging(nameof(TextEditorProgram));

                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }

                this._TextEditorProgram = value;
                UpdateTextEditorProgramExists();
                this.OnPropertyChanged(nameof(TextEditorProgram));
            }
        }

        private bool _FormsEventsOnlyWithFormLibraries;
        [DataMember]
        public bool FormsEventsOnlyWithFormLibraries
        {
            get => _FormsEventsOnlyWithFormLibraries;
            set
            {
                this.OnPropertyChanging(nameof(FormsEventsOnlyWithFormLibraries));
                this._FormsEventsOnlyWithFormLibraries = value;
                this.OnPropertyChanged(nameof(FormsEventsOnlyWithFormLibraries));
            }
        }

        private bool _AttributesDependentComponentsAllComponents;
        /// <summary>
        /// Отображать все зависимые элементы
        /// </summary>
        [DataMember]
        public bool AttributesDependentComponentsAllComponents
        {
            get => _AttributesDependentComponentsAllComponents;
            set
            {
                this.OnPropertyChanging(nameof(AttributesDependentComponentsAllComponents));
                this._AttributesDependentComponentsAllComponents = value;
                this.OnPropertyChanged(nameof(AttributesDependentComponentsAllComponents));
            }
        }

        private ComponentsGroupBy _ComponentsGroupBy = ComponentsGroupBy.RequiredComponents;
        [DataMember]
        public ComponentsGroupBy ComponentsGroupBy
        {
            get => _ComponentsGroupBy;
            set
            {
                if (_ComponentsGroupBy == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ComponentsGroupBy));
                this._ComponentsGroupBy = value;
                this.OnPropertyChanged(nameof(ComponentsGroupBy));
            }
        }

        private string _PluginConfigurationFileName;
        /// <summary>
        /// Имя файла с описанием плагинов
        /// </summary>
        [DataMember]
        public string PluginConfigurationFileName
        {
            get => _PluginConfigurationFileName;
            set
            {
                this.OnPropertyChanging(nameof(PluginConfigurationFileName));

                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }

                this._PluginConfigurationFileName = value;
                this.OnPropertyChanged(nameof(PluginConfigurationFileName));
            }
        }

        private string _FormsEventsFileName;
        /// <summary>
        /// Имя файла с событиями форм
        /// </summary>
        [DataMember]
        public string FormsEventsFileName
        {
            get => _FormsEventsFileName;
            set
            {
                this.OnPropertyChanging(nameof(FormsEventsFileName));

                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                }

                this._FormsEventsFileName = value;
                this.OnPropertyChanged(nameof(FormsEventsFileName));
            }
        }

        private bool _SolutionComponentWithUrl = false;
        [DataMember]
        public bool SolutionComponentWithUrl
        {
            get => _SolutionComponentWithUrl;
            set
            {
                this.OnPropertyChanging(nameof(SolutionComponentWithUrl));
                this._SolutionComponentWithUrl = value;
                this.OnPropertyChanged(nameof(SolutionComponentWithUrl));
            }
        }

        private bool _SolutionComponentWithSolutionInfo = true;
        [DataMember]
        public bool SolutionComponentWithSolutionInfo
        {
            get => _SolutionComponentWithSolutionInfo;
            set
            {
                this.OnPropertyChanging(nameof(SolutionComponentWithSolutionInfo));
                this._SolutionComponentWithSolutionInfo = value;
                this.OnPropertyChanged(nameof(SolutionComponentWithSolutionInfo));
            }
        }

        private bool _SolutionComponentWithManagedInfo = true;
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

        private bool _OpenImportJobFormattedResultsInExcel = true;
        [DataMember]
        public bool OpenImportJobFormattedResultsInExcel
        {
            get => _OpenImportJobFormattedResultsInExcel;
            set
            {
                this.OnPropertyChanging(nameof(OpenImportJobFormattedResultsInExcel));
                this._OpenImportJobFormattedResultsInExcel = value;
                this.OnPropertyChanged(nameof(OpenImportJobFormattedResultsInExcel));
            }
        }

        private FileAction _DefaultFileAction = FileAction.None;
        [DataMember]
        public FileAction DefaultFileAction
        {
            get => _DefaultFileAction;
            set
            {
                if (_DefaultFileAction == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(DefaultFileAction));
                this._DefaultFileAction = value;
                this.OnPropertyChanged(nameof(DefaultFileAction));
            }
        }

        [DataMember]
        public ConcurrentDictionary<string, FileAction> FileActionsByExtensions { get; private set; }

        private const string defaultPluginConfigurationFileName = "Plugins Configuration";
        private const string defaultFormsEventsFileName = "System Forms Events";

        public CommonConfiguration()
        {
            this.PluginConfigurationFileName = defaultPluginConfigurationFileName;
            this.FormsEventsFileName = defaultFormsEventsFileName;

            this.FileActionsByExtensions = new ConcurrentDictionary<string, FileAction>(StringComparer.InvariantCultureIgnoreCase);
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

        /// <summary>
        /// Загрузка файла конфигурации
        /// </summary>
        /// <param name="dataPath"></param>
        /// <returns></returns>
        public static CommonConfiguration Get()
        {
            lock (_syncObject)
            {
                if (_singleton != null)
                {
                    return _singleton;
                }

                string filePath = FileOperations.GetCommonConfigFilePath();

                CommonConfiguration result = GetFromPath(filePath);

                result = result ?? new CommonConfiguration();

                result.Path = filePath;

                result.StartWatchFile();

                if (string.IsNullOrEmpty(result.PluginConfigurationFileName))
                {
                    result.PluginConfigurationFileName = defaultPluginConfigurationFileName;
                }

                if (string.IsNullOrEmpty(result.FormsEventsFileName))
                {
                    result.FormsEventsFileName = defaultFormsEventsFileName;
                }

                if (string.IsNullOrEmpty(result.FolderForExport))
                {
                    result.FolderForExport = FileOperations.GetDefaultFolderForExportFilePath();
                }

                _singleton = result;

                return _singleton;
            }
        }

        private static CommonConfiguration GetFromPath(string filePath)
        {
            CommonConfiguration result = null;

            if (File.Exists(filePath))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(CommonConfiguration));

                using (Mutex mutex = new Mutex(false, FileOperations.GetMutexName(filePath)))
                {
                    try
                    {
                        mutex.WaitOne();

                        using (var sr = File.OpenRead(filePath))
                        {
                            result = ser.ReadObject(sr) as CommonConfiguration;
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

        private void LoadFromDisk(CommonConfiguration diskData)
        {
            this.FolderForExport = diskData.FolderForExport;

            this.DoNotPropmPublishMessage = diskData.DoNotPropmPublishMessage;
            this.ClearOutputWindowBeforeCRMOperation = diskData.ClearOutputWindowBeforeCRMOperation;

            this.CompareProgram = diskData.CompareProgram;
            this.CompareArgumentsFormat = diskData.CompareArgumentsFormat;
            this.CompareArgumentsThreeWayFormat = diskData.CompareArgumentsThreeWayFormat;

            this.TextEditorProgram = diskData.TextEditorProgram;
            
            this.FormsEventsOnlyWithFormLibraries = diskData.FormsEventsOnlyWithFormLibraries;

            this.ComponentsGroupBy = diskData.ComponentsGroupBy;

            this.PluginConfigurationFileName = diskData.PluginConfigurationFileName;
            this.FormsEventsFileName = diskData.FormsEventsFileName;

            this.SolutionComponentWithManagedInfo = diskData.SolutionComponentWithManagedInfo;
            this.SolutionComponentWithSolutionInfo = diskData.SolutionComponentWithSolutionInfo;
            this.SolutionComponentWithUrl = diskData.SolutionComponentWithUrl;

            this.AttributesDependentComponentsAllComponents = diskData.AttributesDependentComponentsAllComponents;

            this.LoadFromDiskExportRibbon(diskData);

            this.LoadFromDiskXml(diskData);

            this.LoadFromDiskEntitySchema(diskData);
            this.LoadFromDiskGlobalOptionSetSchema(diskData);

            this.LoadFromDiskProxyClass(diskData);

            this.DefaultFileAction = diskData.DefaultFileAction;

            this.FileActionsByExtensions.Clear();
            foreach (var key in diskData.FileActionsByExtensions.Keys)
            {
                this.FileActionsByExtensions.TryAdd(key, diskData.FileActionsByExtensions[key]);
            }
        }

        [OnDeserializing]
        private void BeforeDeserialize(StreamingContext context)
        {
            if (this.FileActionsByExtensions == null)
            {
                this.FileActionsByExtensions = new ConcurrentDictionary<string, FileAction>(StringComparer.InvariantCultureIgnoreCase);
            }
        }

        [OnDeserialized]
        private void AfterDeserialize(StreamingContext context)
        {
            UpdateTextEditorProgramExists();
            UpdateDifferenceProgramExists();
            UpdateDifferenceThreeWayAvaliable();

            if (this._GenerateCommonSpaceCount <= 0)
            {
                this._GenerateCommonSpaceCount = _defaultGenerateCommonSpaceCount;
            }
        }

        /// <summary>
        /// Сохранение конфигурации в папку
        /// </summary>
        /// <param name="filePath"></param>
        private void Save(string filePath)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(CommonConfiguration));

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
            Save(this.Path);
        }

        private bool _TextEditorProgramExists = false;

        public bool TextEditorProgramExists() => _TextEditorProgramExists;

        private void UpdateTextEditorProgramExists()
        {
            _TextEditorProgramExists = !string.IsNullOrEmpty(this.TextEditorProgram) && File.Exists(this.TextEditorProgram);
        }

        private bool _DifferenceProgramExists = false;

        public bool DifferenceProgramExists() => _DifferenceProgramExists;

        private void UpdateDifferenceProgramExists()
        {
            _DifferenceProgramExists = !string.IsNullOrEmpty(this.CompareProgram) && File.Exists(this.CompareProgram) && !string.IsNullOrEmpty(this.CompareArgumentsFormat);
        }

        private bool _DifferenceThreeWayAvaliable = false;

        public bool DifferenceThreeWayAvaliable() => _DifferenceThreeWayAvaliable;

        private void UpdateDifferenceThreeWayAvaliable()
        {
            _DifferenceThreeWayAvaliable = !string.IsNullOrEmpty(this.CompareProgram) && File.Exists(this.CompareProgram) && !string.IsNullOrEmpty(this.CompareArgumentsThreeWayFormat);
        }

        public FileAction GetFileActionByExtension(string extension)
        {
            if (this.FileActionsByExtensions.ContainsKey(extension))
            {
                return this.FileActionsByExtensions[extension];
            }

            return this.DefaultFileAction;
        }
    }
}