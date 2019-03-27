using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class CommonConfiguration : INotifyPropertyChanging, INotifyPropertyChanged
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
                this.OnPropertyChanged(nameof(CompareProgram));
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
                this.OnPropertyChanged(nameof(TextEditorProgram));
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
                this.OnPropertyChanged(nameof(CompareArgumentsThreeWayFormat));
            }
        }

        private bool _ExportRibbonXmlForm = true;
        /// <summary>
        /// Экспортировать риббон расположения Form
        /// </summary>
        [DataMember]
        public bool ExportRibbonXmlForm
        {
            get => _ExportRibbonXmlForm;
            set
            {
                this.OnPropertyChanging(nameof(ExportRibbonXmlForm));
                this._ExportRibbonXmlForm = value;
                this.OnPropertyChanged(nameof(ExportRibbonXmlForm));
            }
        }

        private bool _ExportRibbonXmlHomepageGrid = true;
        /// <summary>
        /// Экспортировать риббон расположения HomepageGrid 
        /// </summary>
        [DataMember]
        public bool ExportRibbonXmlHomepageGrid
        {
            get => _ExportRibbonXmlHomepageGrid;
            set
            {
                this.OnPropertyChanging(nameof(ExportRibbonXmlHomepageGrid));
                this._ExportRibbonXmlHomepageGrid = value;
                this.OnPropertyChanged(nameof(ExportRibbonXmlHomepageGrid));
            }
        }

        private bool _ExportRibbonXmlSubGrid = true;
        /// <summary>
        /// Экспортировать риббон расположения SubGrid
        /// </summary>
        [DataMember]
        public bool ExportRibbonXmlSubGrid
        {
            get => _ExportRibbonXmlSubGrid;
            set
            {
                this.OnPropertyChanging(nameof(ExportRibbonXmlSubGrid));
                this._ExportRibbonXmlSubGrid = value;
                this.OnPropertyChanged(nameof(ExportRibbonXmlSubGrid));
            }
        }

        private bool _ExportXmlAttributeOnNewLine;
        [DataMember]
        public bool ExportXmlAttributeOnNewLine
        {
            get => _ExportXmlAttributeOnNewLine;
            set
            {
                this.OnPropertyChanging(nameof(ExportXmlAttributeOnNewLine));
                this._ExportXmlAttributeOnNewLine = value;
                this.OnPropertyChanged(nameof(ExportXmlAttributeOnNewLine));
            }
        }

        private bool _GenerateAttributes = true;
        /// <summary>
        /// Генерировать атрибуты в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateAttributes
        {
            get => _GenerateAttributes;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributes));
                this._GenerateAttributes = value;
                this.OnPropertyChanged(nameof(GenerateAttributes));
            }
        }

        private bool _GenerateManyToOne = true;
        /// <summary>
        /// Генерировать связи Many To One (N:1) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateManyToOne
        {
            get => _GenerateManyToOne;
            set
            {
                this.OnPropertyChanging(nameof(GenerateManyToOne));
                this._GenerateManyToOne = value;
                this.OnPropertyChanged(nameof(GenerateManyToOne));
            }
        }

        private bool _GenerateOneToMany = true;
        /// <summary>
        /// Генерировать One To Many (1:N) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateOneToMany
        {
            get => _GenerateOneToMany;
            set
            {
                this.OnPropertyChanging(nameof(GenerateOneToMany));
                this._GenerateOneToMany = value;
                this.OnPropertyChanged(nameof(GenerateOneToMany));
            }
        }

        private bool _GenerateManyToMany = true;
        /// <summary>
        /// Генерировать Many To Many (N:N) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateManyToMany
        {
            get => _GenerateManyToMany;
            set
            {
                this.OnPropertyChanging(nameof(GenerateManyToMany));
                this._GenerateManyToMany = value;
                this.OnPropertyChanged(nameof(GenerateManyToMany));
            }
        }

        private bool _GenerateLocalOptionSet = true;
        /// <summary>
        /// Генерировать enum для значений Picklist-ов в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateLocalOptionSet
        {
            get => _GenerateLocalOptionSet;
            set
            {
                this.OnPropertyChanging(nameof(GenerateLocalOptionSet));
                this._GenerateLocalOptionSet = value;
                this.OnPropertyChanged(nameof(GenerateLocalOptionSet));
            }
        }

        private bool _GenerateGlobalOptionSet = false;
        [DataMember]
        public bool GenerateGlobalOptionSet
        {
            get => _GenerateGlobalOptionSet;
            set
            {
                this.OnPropertyChanging(nameof(GenerateGlobalOptionSet));
                this._GenerateGlobalOptionSet = value;
                this.OnPropertyChanged(nameof(GenerateGlobalOptionSet));
            }
        }

        private bool _GenerateStatus = true;
        /// <summary>
        /// Генерировать enum для значения Statecode и Statuscode в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateStatus
        {
            get => _GenerateStatus;
            set
            {
                this.OnPropertyChanging(nameof(GenerateStatus));
                this._GenerateStatus = value;
                this.OnPropertyChanged(nameof(GenerateStatus));
            }
        }

        private bool _GenerateKeys = true;
        /// <summary>
        /// Генерировать уникальные ключи
        /// </summary>
        [DataMember]
        public bool GenerateKeys
        {
            get => _GenerateKeys;
            set
            {
                this.OnPropertyChanging(nameof(GenerateKeys));
                this._GenerateKeys = value;
                this.OnPropertyChanged(nameof(GenerateKeys));
            }
        }

        private bool _GenerateIntoSchemaClass;
        [DataMember]
        public bool GenerateIntoSchemaClass
        {
            get => _GenerateIntoSchemaClass;
            set
            {
                this.OnPropertyChanging(nameof(GenerateIntoSchemaClass));
                this._GenerateIntoSchemaClass = value;
                this.OnPropertyChanged(nameof(GenerateIntoSchemaClass));
            }
        }

        private bool _AllDescriptions;
        /// <summary>
        /// Генерировать все описания (description) или только первое по приоритету в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool AllDescriptions
        {
            get => _AllDescriptions;
            set
            {
                this.OnPropertyChanging(nameof(AllDescriptions));
                this._AllDescriptions = value;
                this.OnPropertyChanged(nameof(AllDescriptions));
            }
        }

        private bool _EntityMetadaOptionSetDependentComponents;
        [DataMember]
        public bool EntityMetadaOptionSetDependentComponents
        {
            get => _EntityMetadaOptionSetDependentComponents;
            set
            {
                this.OnPropertyChanging(nameof(EntityMetadaOptionSetDependentComponents));
                this._EntityMetadaOptionSetDependentComponents = value;
                this.OnPropertyChanged(nameof(EntityMetadaOptionSetDependentComponents));
            }
        }

        private bool _AllDependentComponentsForAttributes;
        /// <summary>
        /// Отображать все зависимые элементы
        /// </summary>
        [DataMember]
        public bool AllDependentComponentsForAttributes
        {
            get => _AllDependentComponentsForAttributes;
            set
            {
                this.OnPropertyChanging(nameof(AllDependentComponentsForAttributes));
                this._AllDependentComponentsForAttributes = value;
                this.OnPropertyChanged(nameof(AllDependentComponentsForAttributes));
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

        private bool _GlobalOptionSetsWithDependentComponents;
        [DataMember]
        public bool GlobalOptionSetsWithDependentComponents
        {
            get => _GlobalOptionSetsWithDependentComponents;
            set
            {
                this.OnPropertyChanging(nameof(GlobalOptionSetsWithDependentComponents));
                this._GlobalOptionSetsWithDependentComponents = value;
                this.OnPropertyChanged(nameof(GlobalOptionSetsWithDependentComponents));
            }
        }

        private FileAction _DefaultFileAction = FileAction.None;
        [DataMember]
        [TypeConverter("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
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

        private int _SpaceCount = 4;
        /// <summary>
        /// Количество пробелов для отступа в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public int SpaceCount
        {
            get => _SpaceCount;
            set
            {
                if (_SpaceCount == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(SpaceCount));
                this._SpaceCount = value;
                this.OnPropertyChanged(nameof(SpaceCount));
            }
        }

        private IndentType _IndentType = IndentType.Spaces;
        /// <summary>
        /// Тип отступа в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public IndentType IndentType
        {
            get => _IndentType;
            set
            {
                if (_IndentType == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(IndentType));
                this._IndentType = value;
                this.OnPropertyChanged(nameof(IndentType));
            }
        }

        private ConstantType _ConstantType = ConstantType.Constant;
        /// <summary>
        /// Тип записей в файле с метаданными сущности. const или read only
        /// </summary>
        [DataMember]
        public ConstantType ConstantType
        {
            get => _ConstantType;
            set
            {
                if (_ConstantType == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(ConstantType));
                this._ConstantType = value;
                this.OnPropertyChanged(nameof(ConstantType));
            }
        }

        private OptionSetExportType _OptionSetExportType = OptionSetExportType.Enums;
        [DataMember]
        public OptionSetExportType OptionSetExportType
        {
            get => _OptionSetExportType;
            set
            {
                if (_OptionSetExportType == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(OptionSetExportType));
                this._OptionSetExportType = value;
                this.OnPropertyChanged(nameof(OptionSetExportType));
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

        private bool _SetXmlSchemasDuringExport = true;
        [DataMember]
        public bool SetXmlSchemasDuringExport
        {
            get => _SetXmlSchemasDuringExport;
            set
            {
                this.OnPropertyChanging(nameof(SetXmlSchemasDuringExport));
                this._SetXmlSchemasDuringExport = value;
                this.OnPropertyChanged(nameof(SetXmlSchemasDuringExport));
            }
        }

        private bool _SetIntellisenseContext = true;
        [DataMember]
        public bool SetIntellisenseContext
        {
            get => _SetIntellisenseContext;
            set
            {
                this.OnPropertyChanging(nameof(SetIntellisenseContext));
                this._SetIntellisenseContext = value;
                this.OnPropertyChanged(nameof(SetIntellisenseContext));
            }
        }

        private bool _SortRibbonCommandsAndRulesById = false;
        [DataMember]
        public bool SortRibbonCommandsAndRulesById
        {
            get => _SortRibbonCommandsAndRulesById;
            set
            {
                this.OnPropertyChanging(nameof(SortRibbonCommandsAndRulesById));
                this._SortRibbonCommandsAndRulesById = value;
                this.OnPropertyChanged(nameof(SortRibbonCommandsAndRulesById));
            }
        }

        private bool _SortFormXmlElements = false;
        [DataMember]
        public bool SortFormXmlElements
        {
            get => _SortFormXmlElements;
            set
            {
                this.OnPropertyChanging(nameof(SortFormXmlElements));
                this._SortFormXmlElements = value;
                this.OnPropertyChanged(nameof(SortFormXmlElements));
            }
        }

        private bool _SortXmlAttributes = false;
        [DataMember]
        public bool SortXmlAttributes
        {
            get => _SortXmlAttributes;
            set
            {
                this.OnPropertyChanging(nameof(SortXmlAttributes));
                this._SortXmlAttributes = value;
                this.OnPropertyChanged(nameof(SortXmlAttributes));
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

        [DataMember]
        public ObservableCollection<CrmSvcUtil> Utils { get; private set; }

        [DataMember]
        public ConcurrentDictionary<string, FileAction> FileActionsByExtensions { get; private set; }

        private const string defaultPluginConfigurationFileName = "Plugins Configuration";
        private const string defaultFormsEventsFileName = "System Forms Events";

        public CommonConfiguration()
        {
            this.PluginConfigurationFileName = defaultPluginConfigurationFileName;
            this.FormsEventsFileName = defaultFormsEventsFileName;

            this.Utils = new ObservableCollection<CrmSvcUtil>();
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
            this.TextEditorProgram = diskData.TextEditorProgram;
            this.CompareArgumentsFormat = diskData.CompareArgumentsFormat;
            this.CompareArgumentsThreeWayFormat = diskData.CompareArgumentsThreeWayFormat;
            this.ExportRibbonXmlForm = diskData.ExportRibbonXmlForm;
            this.ExportRibbonXmlHomepageGrid = diskData.ExportRibbonXmlHomepageGrid;
            this.ExportRibbonXmlSubGrid = diskData.ExportRibbonXmlSubGrid;
            this.ExportXmlAttributeOnNewLine = diskData.ExportXmlAttributeOnNewLine;
            this.GenerateAttributes = diskData.GenerateAttributes;
            this.GenerateManyToOne = diskData.GenerateManyToOne;
            this.GenerateOneToMany = diskData.GenerateOneToMany;
            this.GenerateManyToMany = diskData.GenerateManyToMany;
            this.GenerateLocalOptionSet = diskData.GenerateLocalOptionSet;
            this.GenerateGlobalOptionSet = diskData.GenerateGlobalOptionSet;
            this.GenerateStatus = diskData.GenerateStatus;
            this.GenerateKeys = diskData.GenerateKeys;
            this.GenerateIntoSchemaClass = diskData.GenerateIntoSchemaClass;
            this.AllDescriptions = diskData.AllDescriptions;
            this.EntityMetadaOptionSetDependentComponents = diskData.EntityMetadaOptionSetDependentComponents;
            this.AllDependentComponentsForAttributes = diskData.AllDependentComponentsForAttributes;
            this.FormsEventsOnlyWithFormLibraries = diskData.FormsEventsOnlyWithFormLibraries;
            this.GlobalOptionSetsWithDependentComponents = diskData.GlobalOptionSetsWithDependentComponents;
            this.DefaultFileAction = diskData.DefaultFileAction;
            this.ComponentsGroupBy = diskData.ComponentsGroupBy;
            this.SpaceCount = diskData.SpaceCount;
            this.IndentType = diskData.IndentType;
            this.ConstantType = diskData.ConstantType;
            this.OptionSetExportType = diskData.OptionSetExportType;
            this.PluginConfigurationFileName = diskData.PluginConfigurationFileName;
            this.FormsEventsFileName = diskData.FormsEventsFileName;
            this.SetXmlSchemasDuringExport = diskData.SetXmlSchemasDuringExport;
            this.SetIntellisenseContext = diskData.SetIntellisenseContext;
            this.SortRibbonCommandsAndRulesById = diskData.SortRibbonCommandsAndRulesById;
            this.SortXmlAttributes = diskData.SortXmlAttributes;

            this.SolutionComponentWithManagedInfo = diskData.SolutionComponentWithManagedInfo;
            this.SolutionComponentWithSolutionInfo = diskData.SolutionComponentWithSolutionInfo;
            this.SolutionComponentWithUrl = diskData.SolutionComponentWithUrl;

            this.Utils.Clear();
            foreach (var item in diskData.Utils)
            {
                this.Utils.Add(item);
            }

            this.FileActionsByExtensions.Clear();
            foreach (var key in diskData.FileActionsByExtensions.Keys)
            {
                this.FileActionsByExtensions.TryAdd(key, diskData.FileActionsByExtensions[key]);
            }
        }

        [OnDeserializing]
        private void BeforeDeserialize(StreamingContext context)
        {
            if (this.Utils == null)
            {
                this.Utils = new ObservableCollection<CrmSvcUtil>();
            }

            if (this.FileActionsByExtensions == null)
            {
                this.FileActionsByExtensions = new ConcurrentDictionary<string, FileAction>(StringComparer.InvariantCultureIgnoreCase);
            }
        }

        [OnDeserialized]
        private void AfterDeserialize(StreamingContext context)
        {
            var notExists = this.Utils.Where(e => !File.Exists(e.Path)).ToList();
            if (notExists.Any())
            {
                notExists.ForEach(e => this.Utils.Remove(e));
            }
        }

        /// <summary>
        /// Сохранение конфигурации в папку
        /// </summary>
        /// <param name="filePath"></param>
        private void Save(string filePath)
        {
            var notExists = this.Utils.Where(e => !File.Exists(e.Path)).ToList();
            if (notExists.Any())
            {
                notExists.ForEach(e => this.Utils.Remove(e));
            }

            foreach (var item in this.Utils)
            {
                FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(item.Path);
                item.Version = string.Format("{0}.{1}.{2}.{3}", versionInfo.ProductMajorPart, versionInfo.ProductMinorPart, versionInfo.ProductPrivatePart, versionInfo.ProductBuildPart);
            }

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

        public bool TextEditorProgramExists()
        {
            return !string.IsNullOrEmpty(this.TextEditorProgram) && File.Exists(this.TextEditorProgram);
        }

        public bool DifferenceProgramExists()
        {
            return !string.IsNullOrEmpty(this.CompareProgram) && File.Exists(this.CompareProgram) && !string.IsNullOrEmpty(this.CompareArgumentsFormat);
        }

        public bool DifferenceThreeWayAvaliable()
        {
            return !string.IsNullOrEmpty(this.CompareProgram) && File.Exists(this.CompareProgram) && !string.IsNullOrEmpty(this.CompareArgumentsThreeWayFormat);
        }

        public Microsoft.Crm.Sdk.Messages.RibbonLocationFilters GetRibbonLocationFilters()
        {
            Microsoft.Crm.Sdk.Messages.RibbonLocationFilters filter = Microsoft.Crm.Sdk.Messages.RibbonLocationFilters.All;

            if (this.ExportRibbonXmlForm || this.ExportRibbonXmlHomepageGrid || this.ExportRibbonXmlSubGrid)
            {
                filter = 0;

                if (this.ExportRibbonXmlForm)
                {
                    filter |= Microsoft.Crm.Sdk.Messages.RibbonLocationFilters.Form;
                }

                if (this.ExportRibbonXmlHomepageGrid)
                {
                    filter |= Microsoft.Crm.Sdk.Messages.RibbonLocationFilters.HomepageGrid;
                }

                if (this.ExportRibbonXmlSubGrid)
                {
                    filter |= Microsoft.Crm.Sdk.Messages.RibbonLocationFilters.SubGrid;
                }
            }

            return filter;
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