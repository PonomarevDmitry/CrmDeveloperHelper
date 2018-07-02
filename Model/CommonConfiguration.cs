using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
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

        private static object _syncObject = new object();

        private static object _syncObjectFile = new object();

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
            get
            {
                return _FolderForExport;
            }
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
            get
            {
                return _DoNotPropmPublishMessage;
            }
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
            get
            {
                return _ClearOutputWindowBeforeCRMOperation;
            }
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
            get
            {
                return _CompareProgram;
            }
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
            get
            {
                return _TextEditorProgram;
            }
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
            get
            {
                return _CompareArgumentsFormat;
            }
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
            get
            {
                return _CompareArgumentsThreeWayFormat;
            }
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

        private bool _ExportRibbonXmlForm;
        /// <summary>
        /// Экспортировать риббон расположения Form
        /// </summary>
        [DataMember]
        public bool ExportRibbonXmlForm
        {
            get
            {
                return _ExportRibbonXmlForm;
            }
            set
            {
                this.OnPropertyChanging(nameof(ExportRibbonXmlForm));
                this._ExportRibbonXmlForm = value;
                this.OnPropertyChanged(nameof(ExportRibbonXmlForm));
            }
        }

        private bool _ExportRibbonXmlHomepageGrid;
        /// <summary>
        /// Экспортировать риббон расположения HomepageGrid 
        /// </summary>
        [DataMember]
        public bool ExportRibbonXmlHomepageGrid
        {
            get
            {
                return _ExportRibbonXmlHomepageGrid;
            }
            set
            {
                this.OnPropertyChanging(nameof(ExportRibbonXmlHomepageGrid));
                this._ExportRibbonXmlHomepageGrid = value;
                this.OnPropertyChanged(nameof(ExportRibbonXmlHomepageGrid));
            }
        }

        private bool _ExportRibbonXmlSubGrid;
        /// <summary>
        /// Экспортировать риббон расположения SubGrid
        /// </summary>
        [DataMember]
        public bool ExportRibbonXmlSubGrid
        {
            get
            {
                return _ExportRibbonXmlSubGrid;
            }
            set
            {
                this.OnPropertyChanging(nameof(ExportRibbonXmlSubGrid));
                this._ExportRibbonXmlSubGrid = value;
                this.OnPropertyChanged(nameof(ExportRibbonXmlSubGrid));
            }
        }

        private bool _GenerateAttributes;
        /// <summary>
        /// Генерировать атрибуты в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateAttributes
        {
            get
            {
                return _GenerateAttributes;
            }
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributes));
                this._GenerateAttributes = value;
                this.OnPropertyChanged(nameof(GenerateAttributes));
            }
        }

        private bool _GenerateManyToOne;
        /// <summary>
        /// Генерировать связи Many To One (N:1) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateManyToOne
        {
            get
            {
                return _GenerateManyToOne;
            }
            set
            {
                this.OnPropertyChanging(nameof(GenerateManyToOne));
                this._GenerateManyToOne = value;
                this.OnPropertyChanged(nameof(GenerateManyToOne));
            }
        }

        private bool _GenerateOneToMany;
        /// <summary>
        /// Генерировать One To Many (1:N) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateOneToMany
        {
            get
            {
                return _GenerateOneToMany;
            }
            set
            {
                this.OnPropertyChanging(nameof(GenerateOneToMany));
                this._GenerateOneToMany = value;
                this.OnPropertyChanged(nameof(GenerateOneToMany));
            }
        }

        private bool _GenerateManyToMany;
        /// <summary>
        /// Генерировать Many To Many (N:N) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateManyToMany
        {
            get
            {
                return _GenerateManyToMany;
            }
            set
            {
                this.OnPropertyChanging(nameof(GenerateManyToMany));
                this._GenerateManyToMany = value;
                this.OnPropertyChanged(nameof(GenerateManyToMany));
            }
        }

        private bool _GenerateLocalOptionSet;
        /// <summary>
        /// Генерировать enum для значений Picklist-ов в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateLocalOptionSet
        {
            get
            {
                return _GenerateLocalOptionSet;
            }
            set
            {
                this.OnPropertyChanging(nameof(GenerateLocalOptionSet));
                this._GenerateLocalOptionSet = value;
                this.OnPropertyChanged(nameof(GenerateLocalOptionSet));
            }
        }

        private bool _GenerateGlobalOptionSet;
        [DataMember]
        public bool GenerateGlobalOptionSet
        {
            get
            {
                return _GenerateGlobalOptionSet;
            }
            set
            {
                this.OnPropertyChanging(nameof(GenerateGlobalOptionSet));
                this._GenerateGlobalOptionSet = value;
                this.OnPropertyChanged(nameof(GenerateGlobalOptionSet));
            }
        }

        private bool _GenerateStatus;
        /// <summary>
        /// Генерировать enum для значения Statecode и Statuscode в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateStatus
        {
            get
            {
                return _GenerateStatus;
            }
            set
            {
                this.OnPropertyChanging(nameof(GenerateStatus));
                this._GenerateStatus = value;
                this.OnPropertyChanged(nameof(GenerateStatus));
            }
        }

        private bool _GenerateKeys;
        /// <summary>
        /// Генерировать уникальные ключи
        /// </summary>
        [DataMember]
        public bool GenerateKeys
        {
            get
            {
                return _GenerateKeys;
            }
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
            get
            {
                return _GenerateIntoSchemaClass;
            }
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
            get
            {
                return _WithManagedInfo;
            }
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
            get
            {
                return _AllDescriptions;
            }
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
            get
            {
                return _EntityMetadaOptionSetDependentComponents;
            }
            set
            {
                this.OnPropertyChanging(nameof(EntityMetadaOptionSetDependentComponents));
                this._EntityMetadaOptionSetDependentComponents = value;
                this.OnPropertyChanged(nameof(EntityMetadaOptionSetDependentComponents));
            }
        }

        private bool _AllDependentComponents;
        /// <summary>
        /// Отображать все зависимые элементы
        /// </summary>
        [DataMember]
        public bool AllDependentComponents
        {
            get
            {
                return _AllDependentComponents;
            }
            set
            {
                this.OnPropertyChanging(nameof(AllDependentComponents));
                this._AllDependentComponents = value;
                this.OnPropertyChanged(nameof(AllDependentComponents));
            }
        }

        private bool _FormsEventsOnlyWithFormLibraries;
        [DataMember]
        public bool FormsEventsOnlyWithFormLibraries
        {
            get
            {
                return _FormsEventsOnlyWithFormLibraries;
            }
            set
            {
                this.OnPropertyChanging(nameof(FormsEventsOnlyWithFormLibraries));
                this._FormsEventsOnlyWithFormLibraries = value;
                this.OnPropertyChanged(nameof(FormsEventsOnlyWithFormLibraries));
            }
        }

        private bool _CreateGlobalOptionSetsWithDependentComponents;
        [DataMember]
        public bool CreateGlobalOptionSetsWithDependentComponents
        {
            get
            {
                return _CreateGlobalOptionSetsWithDependentComponents;
            }
            set
            {
                this.OnPropertyChanging(nameof(CreateGlobalOptionSetsWithDependentComponents));
                this._CreateGlobalOptionSetsWithDependentComponents = value;
                this.OnPropertyChanged(nameof(CreateGlobalOptionSetsWithDependentComponents));
            }
        }

        private FileAction _AfterCreateFileAction = FileAction.None;
        [DataMember]
        [TypeConverter("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
        public FileAction AfterCreateFileAction
        {
            get
            {
                return _AfterCreateFileAction;
            }
            set
            {
                if (_AfterCreateFileAction == value)
                {
                    return;
                }

                this.OnPropertyChanging(nameof(AfterCreateFileAction));
                this._AfterCreateFileAction = value;
                this.OnPropertyChanged(nameof(AfterCreateFileAction));
            }
        }

        private ComponentsGroupBy _ComponentsGroupBy = ComponentsGroupBy.RequiredComponents;
        [DataMember]
        public ComponentsGroupBy ComponentsGroupBy
        {
            get
            {
                return _ComponentsGroupBy;
            }
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
            get
            {
                return _SpaceCount;
            }
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
            get
            {
                return _IndentType;
            }
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
            get
            {
                return _ConstantType;
            }
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
            get
            {
                return _OptionSetExportType;
            }
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
            get
            {
                return _PluginConfigurationFileName;
            }
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
            get
            {
                return _FormsEventsFileName;
            }
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

        private bool _AllEnities;
        /// <summary>
        /// Отображать в списке все сущности, не только с отличиями
        /// </summary>
        [DataMember]
        public bool AllEnities
        {
            get
            {
                return _AllEnities;
            }
            set
            {
                this.OnPropertyChanging(nameof(AllEnities));
                this._AllEnities = value;
                this.OnPropertyChanged(nameof(AllEnities));
            }
        }

        private bool _ExportAutoNumberingSettings;
        /// <summary>
        /// Экспортировать в решении настройки AutoNumbering
        /// </summary>
        public bool ExportAutoNumberingSettings
        {
            get
            {
                return _ExportAutoNumberingSettings;
            }
            set
            {
                this.OnPropertyChanging(nameof(ExportAutoNumberingSettings));
                this._ExportAutoNumberingSettings = value;
                this.OnPropertyChanged(nameof(ExportAutoNumberingSettings));
            }
        }

        private bool _ExportCalendarSettings;
        /// <summary>
        /// Экспортировать в решении настройки Calendar
        /// </summary>
        [DataMember]
        public bool ExportCalendarSettings
        {
            get
            {
                return _ExportCalendarSettings;
            }
            set
            {
                this.OnPropertyChanging(nameof(ExportCalendarSettings));
                this._ExportCalendarSettings = value;
                this.OnPropertyChanged(nameof(ExportCalendarSettings));
            }
        }

        private bool _ExportCustomizationSettings;
        /// <summary>
        /// Экспортировать в решении настройки Customization
        /// </summary>
        [DataMember]
        public bool ExportCustomizationSettings
        {
            get
            {
                return _ExportCustomizationSettings;
            }
            set
            {
                this.OnPropertyChanging(nameof(ExportCustomizationSettings));
                this._ExportCustomizationSettings = value;
                this.OnPropertyChanged(nameof(ExportCustomizationSettings));
            }
        }

        private bool _ExportEmailTrackingSettings;
        /// <summary>
        /// Экспортировать в решении настройки EmailTracking
        /// </summary>
        [DataMember]
        public bool ExportEmailTrackingSettings
        {
            get
            {
                return _ExportEmailTrackingSettings;
            }
            set
            {
                this.OnPropertyChanging(nameof(ExportEmailTrackingSettings));
                this._ExportEmailTrackingSettings = value;
                this.OnPropertyChanged(nameof(ExportEmailTrackingSettings));
            }
        }

        private bool _ExportExternalApplications;
        /// <summary>
        /// Экспортировать в решении настройки ExternalApplications
        /// </summary>
        [DataMember]
        public bool ExportExternalApplications
        {
            get
            {
                return _ExportExternalApplications;
            }
            set
            {
                this.OnPropertyChanging(nameof(ExportExternalApplications));
                this._ExportExternalApplications = value;
                this.OnPropertyChanged(nameof(ExportExternalApplications));
            }
        }

        private bool _ExportGeneralSettings;
        /// <summary>
        /// Экспортировать в решении настройки General
        /// </summary>
        [DataMember]
        public bool ExportGeneralSettings
        {
            get
            {
                return _ExportGeneralSettings;
            }
            set
            {
                this.OnPropertyChanging(nameof(ExportGeneralSettings));
                this._ExportGeneralSettings = value;
                this.OnPropertyChanged(nameof(ExportGeneralSettings));
            }
        }

        private bool _ExportIsvConfig;
        /// <summary>
        /// Экспортировать в решении настройки IsvConfig
        /// </summary>
        [DataMember]
        public bool ExportIsvConfig
        {
            get
            {
                return _ExportIsvConfig;
            }
            set
            {
                this.OnPropertyChanging(nameof(ExportIsvConfig));
                this._ExportIsvConfig = value;
                this.OnPropertyChanged(nameof(ExportIsvConfig));
            }
        }

        private bool _ExportMarketingSettings;
        /// <summary>
        /// Экспортировать в решении настройки Marketing
        /// </summary>
        [DataMember]
        public bool ExportMarketingSettings
        {
            get
            {
                return _ExportMarketingSettings;
            }
            set
            {
                this.OnPropertyChanging(nameof(ExportMarketingSettings));
                this._ExportMarketingSettings = value;
                this.OnPropertyChanged(nameof(ExportMarketingSettings));
            }
        }

        private bool _ExportOutlookSynchronizationSettings;
        /// <summary>
        /// Экспортировать в решении настройки OutlookSynchronization
        /// </summary>
        [DataMember]
        public bool ExportOutlookSynchronizationSettings
        {
            get
            {
                return _ExportOutlookSynchronizationSettings;
            }
            set
            {
                this.OnPropertyChanging(nameof(ExportOutlookSynchronizationSettings));
                this._ExportOutlookSynchronizationSettings = value;
                this.OnPropertyChanged(nameof(ExportOutlookSynchronizationSettings));
            }
        }

        private bool _ExportRelationshipRoles;
        /// <summary>
        /// Экспортировать в решении настройки RelationshipRoles
        /// </summary>
        [DataMember]
        public bool ExportRelationshipRoles
        {
            get
            {
                return _ExportRelationshipRoles;
            }
            set
            {
                this.OnPropertyChanging(nameof(ExportRelationshipRoles));
                this._ExportRelationshipRoles = value;
                this.OnPropertyChanged(nameof(ExportRelationshipRoles));
            }
        }

        private bool _ExportSales;
        /// <summary>
        /// Экспортировать в решении настройки Sales
        /// </summary>
        [DataMember]
        public bool ExportSales
        {
            get
            {
                return _ExportSales;
            }
            set
            {
                this.OnPropertyChanging(nameof(ExportSales));
                this._ExportSales = value;
                this.OnPropertyChanged(nameof(ExportSales));
            }
        }

        private bool _Managed;
        [DataMember]
        public bool Managed
        {
            get
            {
                return _Managed;
            }
            set
            {
                this.OnPropertyChanging(nameof(Managed));
                this._Managed = value;
                this.OnPropertyChanged(nameof(Managed));
            }
        }

        [DataMember]
        public ObservableCollection<CrmSvcUtil> Utils { get; private set; }

        private const string defaultPluginConfigurationFileName = "Plugins Configuration";
        private const string defaultFormsEventsFileName = "System Forms Events";

        public CommonConfiguration()
        {
            this.PluginConfigurationFileName = defaultPluginConfigurationFileName;
            this.FormsEventsFileName = defaultFormsEventsFileName;

            this.Utils = new ObservableCollection<CrmSvcUtil>();
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
                            DTEHelper.WriteExceptionToLog(ex);

                            result = null;
                        }
                    }
                }

                result = result ?? new CommonConfiguration();

                result.Path = filePath;

                var notExists = result.Utils.Where(e => !File.Exists(e.Path)).ToList();
                if (notExists.Any())
                {
                    notExists.ForEach(e => result.Utils.Remove(e));
                }

                foreach (var item in result.Utils)
                {
                    FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(item.Path);
                    item.Version = string.Format("{0}.{1}.{2}.{3}", versionInfo.ProductMajorPart, versionInfo.ProductMinorPart, versionInfo.ProductPrivatePart, versionInfo.ProductBuildPart);
                }

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

            lock (_syncObjectFile)
            {
                try
                {
                    using (var stream = File.Create(filePath))
                    {
                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.Indent = true;
                        settings.Encoding = Encoding.UTF8;

                        using (XmlWriter xmlWriter = XmlWriter.Create(stream, settings))
                        {
                            ser.WriteObject(xmlWriter, this);
                        }
                    }
                }
                catch (Exception ex)
                {
                    DTEHelper.WriteExceptionToLog(ex);
                }
            }
        }

        public void Save()
        {
            Save(this.Path);
        }

        public bool TextEditorProgramExists() => !string.IsNullOrEmpty(this.TextEditorProgram) && File.Exists(this.TextEditorProgram);

        public bool DifferenceProgramExists() => !string.IsNullOrEmpty(this.CompareProgram) && File.Exists(this.CompareProgram) && !string.IsNullOrEmpty(this.CompareArgumentsFormat);

        public bool DifferenceThreeWayAvaliable() => !string.IsNullOrEmpty(this.CompareProgram) && File.Exists(this.CompareProgram) && !string.IsNullOrEmpty(this.CompareArgumentsThreeWayFormat);
    }
}