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
using System.Xml;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [DataContract]
    public class CommonConfiguration : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private static CommonConfiguration _singleton;

        private static readonly object _syncObject = new object();

        private static readonly object _syncObjectFile = new object();

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

        private bool _WithManagedInfo;
        [DataMember]
        public bool WithManagedInfo
        {
            get => _WithManagedInfo;
            set
            {
                this.OnPropertyChanging(nameof(WithManagedInfo));
                this._WithManagedInfo = value;
                this.OnPropertyChanged(nameof(WithManagedInfo));
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

        private bool _ExportSolutionExportAutoNumberingSettings;
        /// <summary>
        /// Экспортировать в решении настройки AutoNumbering
        /// </summary>
        public bool ExportSolutionExportAutoNumberingSettings
        {
            get => _ExportSolutionExportAutoNumberingSettings;
            set
            {
                this.OnPropertyChanging(nameof(ExportSolutionExportAutoNumberingSettings));
                this._ExportSolutionExportAutoNumberingSettings = value;
                this.OnPropertyChanged(nameof(ExportSolutionExportAutoNumberingSettings));
            }
        }

        private bool _ExportSolutionExportCalendarSettings;
        /// <summary>
        /// Экспортировать в решении настройки Calendar
        /// </summary>
        [DataMember]
        public bool ExportSolutionExportCalendarSettings
        {
            get => _ExportSolutionExportCalendarSettings;
            set
            {
                this.OnPropertyChanging(nameof(ExportSolutionExportCalendarSettings));
                this._ExportSolutionExportCalendarSettings = value;
                this.OnPropertyChanged(nameof(ExportSolutionExportCalendarSettings));
            }
        }

        private bool _ExportSolutionExportCustomizationSettings;
        /// <summary>
        /// Экспортировать в решении настройки Customization
        /// </summary>
        [DataMember]
        public bool ExportSolutionExportCustomizationSettings
        {
            get => _ExportSolutionExportCustomizationSettings;
            set
            {
                this.OnPropertyChanging(nameof(ExportSolutionExportCustomizationSettings));
                this._ExportSolutionExportCustomizationSettings = value;
                this.OnPropertyChanged(nameof(ExportSolutionExportCustomizationSettings));
            }
        }

        private bool _ExportSolutionExportEmailTrackingSettings;
        /// <summary>
        /// Экспортировать в решении настройки EmailTracking
        /// </summary>
        [DataMember]
        public bool ExportSolutionExportEmailTrackingSettings
        {
            get => _ExportSolutionExportEmailTrackingSettings;
            set
            {
                this.OnPropertyChanging(nameof(ExportSolutionExportEmailTrackingSettings));
                this._ExportSolutionExportEmailTrackingSettings = value;
                this.OnPropertyChanged(nameof(ExportSolutionExportEmailTrackingSettings));
            }
        }

        private bool _ExportSolutionExportExternalApplications;
        /// <summary>
        /// Экспортировать в решении настройки ExternalApplications
        /// </summary>
        [DataMember]
        public bool ExportSolutionExportExternalApplications
        {
            get => _ExportSolutionExportExternalApplications;
            set
            {
                this.OnPropertyChanging(nameof(ExportSolutionExportExternalApplications));
                this._ExportSolutionExportExternalApplications = value;
                this.OnPropertyChanged(nameof(ExportSolutionExportExternalApplications));
            }
        }

        private bool _ExportSolutionExportGeneralSettings;
        /// <summary>
        /// Экспортировать в решении настройки General
        /// </summary>
        [DataMember]
        public bool ExportSolutionExportGeneralSettings
        {
            get => _ExportSolutionExportGeneralSettings;
            set
            {
                this.OnPropertyChanging(nameof(ExportSolutionExportGeneralSettings));
                this._ExportSolutionExportGeneralSettings = value;
                this.OnPropertyChanged(nameof(ExportSolutionExportGeneralSettings));
            }
        }

        private bool _ExportSolutionExportIsvConfig;
        /// <summary>
        /// Экспортировать в решении настройки IsvConfig
        /// </summary>
        [DataMember]
        public bool ExportSolutionExportIsvConfig
        {
            get => _ExportSolutionExportIsvConfig;
            set
            {
                this.OnPropertyChanging(nameof(ExportSolutionExportIsvConfig));
                this._ExportSolutionExportIsvConfig = value;
                this.OnPropertyChanged(nameof(ExportSolutionExportIsvConfig));
            }
        }

        private bool _ExportSolutionExportMarketingSettings;
        /// <summary>
        /// Экспортировать в решении настройки Marketing
        /// </summary>
        [DataMember]
        public bool ExportSolutionExportMarketingSettings
        {
            get => _ExportSolutionExportMarketingSettings;
            set
            {
                this.OnPropertyChanging(nameof(ExportSolutionExportMarketingSettings));
                this._ExportSolutionExportMarketingSettings = value;
                this.OnPropertyChanged(nameof(ExportSolutionExportMarketingSettings));
            }
        }

        private bool _ExportSolutionExportOutlookSynchronizationSettings;
        /// <summary>
        /// Экспортировать в решении настройки OutlookSynchronization
        /// </summary>
        [DataMember]
        public bool ExportSolutionExportOutlookSynchronizationSettings
        {
            get => _ExportSolutionExportOutlookSynchronizationSettings;
            set
            {
                this.OnPropertyChanging(nameof(ExportSolutionExportOutlookSynchronizationSettings));
                this._ExportSolutionExportOutlookSynchronizationSettings = value;
                this.OnPropertyChanged(nameof(ExportSolutionExportOutlookSynchronizationSettings));
            }
        }

        private bool _ExportSolutionExportRelationshipRoles;
        /// <summary>
        /// Экспортировать в решении настройки RelationshipRoles
        /// </summary>
        [DataMember]
        public bool ExportSolutionExportRelationshipRoles
        {
            get => _ExportSolutionExportRelationshipRoles;
            set
            {
                this.OnPropertyChanging(nameof(ExportSolutionExportRelationshipRoles));
                this._ExportSolutionExportRelationshipRoles = value;
                this.OnPropertyChanged(nameof(ExportSolutionExportRelationshipRoles));
            }
        }

        private bool _ExportSolutionExportSales;
        /// <summary>
        /// Экспортировать в решении настройки Sales
        /// </summary>
        [DataMember]
        public bool ExportSolutionExportSales
        {
            get => _ExportSolutionExportSales;
            set
            {
                this.OnPropertyChanging(nameof(ExportSolutionExportSales));
                this._ExportSolutionExportSales = value;
                this.OnPropertyChanged(nameof(ExportSolutionExportSales));
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

                string filePath = FileOperations.GetCommonConfigPath();

                CommonConfiguration result = null;

                if (File.Exists(filePath))
                {
                    lock (_syncObjectFile)
                    {
                        DataContractSerializer ser = new DataContractSerializer(typeof(CommonConfiguration));

                        try
                        {
                            using (var sr = File.OpenRead(filePath))
                            {
                                result = ser.ReadObject(sr) as CommonConfiguration;
                            }
                        }
                        catch (Exception ex)
                        {
                            DTEHelper.WriteExceptionToOutput(ex);

                            FileOperations.CreateBackUpFile(filePath, ex);

                            result = null;
                        }
                    }
                }

                result = result ?? new CommonConfiguration();

                result.Path = filePath;

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
                    }

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
                lock (_syncObjectFile)
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