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
                UpdateDifferenceProgramExists();
                UpdateDifferenceThreeWayAvaliable();
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
                UpdateTextEditorProgramExists();
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

        private bool _GenerateAttributesSchema = true;
        /// <summary>
        /// Генерировать атрибуты в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateAttributesSchema
        {
            get => _GenerateAttributesSchema;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributesSchema));
                this._GenerateAttributesSchema = value;
                this.OnPropertyChanged(nameof(GenerateAttributesSchema));
            }
        }

        private bool _GenerateManyToOneSchema = true;
        /// <summary>
        /// Генерировать связи Many To One (N:1) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateManyToOneSchema
        {
            get => _GenerateManyToOneSchema;
            set
            {
                this.OnPropertyChanging(nameof(GenerateManyToOneSchema));
                this._GenerateManyToOneSchema = value;
                this.OnPropertyChanged(nameof(GenerateManyToOneSchema));
            }
        }

        private bool _GenerateOneToManySchema = true;
        /// <summary>
        /// Генерировать One To Many (1:N) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateOneToManySchema
        {
            get => _GenerateOneToManySchema;
            set
            {
                this.OnPropertyChanging(nameof(GenerateOneToManySchema));
                this._GenerateOneToManySchema = value;
                this.OnPropertyChanged(nameof(GenerateOneToManySchema));
            }
        }

        private bool _GenerateManyToManySchema = true;
        /// <summary>
        /// Генерировать Many To Many (N:N) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateManyToManySchema
        {
            get => _GenerateManyToManySchema;
            set
            {
                this.OnPropertyChanging(nameof(GenerateManyToManySchema));
                this._GenerateManyToManySchema = value;
                this.OnPropertyChanged(nameof(GenerateManyToManySchema));
            }
        }

        private bool _GenerateLocalOptionSetSchema = true;
        /// <summary>
        /// Генерировать enum для значений Picklist-ов в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateLocalOptionSetSchema
        {
            get => _GenerateLocalOptionSetSchema;
            set
            {
                this.OnPropertyChanging(nameof(GenerateLocalOptionSetSchema));
                this._GenerateLocalOptionSetSchema = value;
                this.OnPropertyChanged(nameof(GenerateLocalOptionSetSchema));
            }
        }

        private bool _GenerateGlobalOptionSetSchema = false;
        [DataMember]
        public bool GenerateGlobalOptionSetSchema
        {
            get => _GenerateGlobalOptionSetSchema;
            set
            {
                this.OnPropertyChanging(nameof(GenerateGlobalOptionSetSchema));
                this._GenerateGlobalOptionSetSchema = value;
                this.OnPropertyChanged(nameof(GenerateGlobalOptionSetSchema));
            }
        }

        private bool _GenerateStatusOptionSetSchema = true;
        /// <summary>
        /// Генерировать enum для значения Statecode и Statuscode в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateStatusOptionSetSchema
        {
            get => _GenerateStatusOptionSetSchema;
            set
            {
                this.OnPropertyChanging(nameof(GenerateStatusOptionSetSchema));
                this._GenerateStatusOptionSetSchema = value;
                this.OnPropertyChanged(nameof(GenerateStatusOptionSetSchema));
            }
        }

        private bool _GenerateKeysSchema = true;
        /// <summary>
        /// Генерировать уникальные ключи
        /// </summary>
        [DataMember]
        public bool GenerateKeysSchema
        {
            get => _GenerateKeysSchema;
            set
            {
                this.OnPropertyChanging(nameof(GenerateKeysSchema));
                this._GenerateKeysSchema = value;
                this.OnPropertyChanged(nameof(GenerateKeysSchema));
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

        private bool _GenerateProxyClassesUseSchemaConstInCSharpAttributes = true;
        [DataMember]
        public bool GenerateProxyClassesUseSchemaConstInCSharpAttributes
        {
            get => _GenerateProxyClassesUseSchemaConstInCSharpAttributes;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesUseSchemaConstInCSharpAttributes));
                this._GenerateProxyClassesUseSchemaConstInCSharpAttributes = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesUseSchemaConstInCSharpAttributes));
            }
        }

        private bool _GenerateProxyClassesWithDebuggerNonUserCode = true;
        [DataMember]
        public bool GenerateProxyClassesWithDebuggerNonUserCode
        {
            get => _GenerateProxyClassesWithDebuggerNonUserCode;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesWithDebuggerNonUserCode));
                this._GenerateProxyClassesWithDebuggerNonUserCode = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesWithDebuggerNonUserCode));
            }
        }

        private bool _GenerateProxyClassesMakeAllPropertiesEditable = true;
        [DataMember]
        public bool GenerateProxyClassesMakeAllPropertiesEditable
        {
            get => _GenerateProxyClassesMakeAllPropertiesEditable;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesMakeAllPropertiesEditable));
                this._GenerateProxyClassesMakeAllPropertiesEditable = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesMakeAllPropertiesEditable));
            }
        }

        private bool _GenerateProxyClassesWithoutObsoleteAttribute = true;
        [DataMember]
        public bool GenerateProxyClassesWithoutObsoleteAttribute
        {
            get => _GenerateProxyClassesWithoutObsoleteAttribute;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesWithoutObsoleteAttribute));
                this._GenerateProxyClassesWithoutObsoleteAttribute = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesWithoutObsoleteAttribute));
            }
        }

        private bool _GenerateProxyClassesAddConstructorWithAnonymousTypeObject = true;
        [DataMember]
        public bool GenerateProxyClassesAddConstructorWithAnonymousTypeObject
        {
            get => _GenerateProxyClassesAddConstructorWithAnonymousTypeObject;
            set
            {
                this.OnPropertyChanging(nameof(GenerateProxyClassesAddConstructorWithAnonymousTypeObject));
                this._GenerateProxyClassesAddConstructorWithAnonymousTypeObject = value;
                this.OnPropertyChanged(nameof(GenerateProxyClassesAddConstructorWithAnonymousTypeObject));
            }
        }

        private bool _GenerateAttributesProxyClass = true;
        /// <summary>
        /// Генерировать атрибуты в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateAttributesProxyClass
        {
            get => _GenerateAttributesProxyClass;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributesProxyClass));
                this._GenerateAttributesProxyClass = value;
                this.OnPropertyChanged(nameof(GenerateAttributesProxyClass));
            }
        }

        private bool _GenerateAttributesProxyClassWithNameOf = true;
        [DataMember]
        public bool GenerateAttributesProxyClassWithNameOf
        {
            get => _GenerateAttributesProxyClassWithNameOf;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributesProxyClassWithNameOf));
                this._GenerateAttributesProxyClassWithNameOf = value;
                this.OnPropertyChanged(nameof(GenerateAttributesProxyClassWithNameOf));
            }
        }

        private ProxyClassAttributeEnums _GenerateAttributesProxyClassEnumsStateStatus = ProxyClassAttributeEnums.NotNeeded;
        [DataMember]
        public ProxyClassAttributeEnums GenerateAttributesProxyClassEnumsStateStatus
        {
            get => _GenerateAttributesProxyClassEnumsStateStatus;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributesProxyClassEnumsStateStatus));
                this._GenerateAttributesProxyClassEnumsStateStatus = value;
                this.OnPropertyChanged(nameof(GenerateAttributesProxyClassEnumsStateStatus));
            }
        }

        private ProxyClassAttributeEnums _GenerateAttributesProxyClassEnumsLocal = ProxyClassAttributeEnums.NotNeeded;
        [DataMember]
        public ProxyClassAttributeEnums GenerateAttributesProxyClassEnumsLocal
        {
            get => _GenerateAttributesProxyClassEnumsLocal;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributesProxyClassEnumsLocal));
                this._GenerateAttributesProxyClassEnumsLocal = value;
                this.OnPropertyChanged(nameof(GenerateAttributesProxyClassEnumsLocal));
            }
        }

        private ProxyClassAttributeEnums _GenerateAttributesProxyClassEnumsGlobal = ProxyClassAttributeEnums.NotNeeded;
        [DataMember]
        public ProxyClassAttributeEnums GenerateAttributesProxyClassEnumsGlobal
        {
            get => _GenerateAttributesProxyClassEnumsGlobal;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributesProxyClassEnumsGlobal));
                this._GenerateAttributesProxyClassEnumsGlobal = value;
                this.OnPropertyChanged(nameof(GenerateAttributesProxyClassEnumsGlobal));
            }
        }

        private bool _GenerateAttributesProxyClassEnumsUseSchemaStateStatusEnum = true;
        [DataMember]
        public bool GenerateAttributesProxyClassEnumsUseSchemaStateStatusEnum
        {
            get => _GenerateAttributesProxyClassEnumsUseSchemaStateStatusEnum;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributesProxyClassEnumsUseSchemaStateStatusEnum));
                this._GenerateAttributesProxyClassEnumsUseSchemaStateStatusEnum = value;
                this.OnPropertyChanged(nameof(GenerateAttributesProxyClassEnumsUseSchemaStateStatusEnum));
            }
        }

        private bool _GenerateAttributesProxyClassEnumsUseSchemaLocalEnum = true;
        [DataMember]
        public bool GenerateAttributesProxyClassEnumsUseSchemaLocalEnum
        {
            get => _GenerateAttributesProxyClassEnumsUseSchemaLocalEnum;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributesProxyClassEnumsUseSchemaLocalEnum));
                this._GenerateAttributesProxyClassEnumsUseSchemaLocalEnum = value;
                this.OnPropertyChanged(nameof(GenerateAttributesProxyClassEnumsUseSchemaLocalEnum));
            }
        }

        private ProxyClassAttributeEnumsGlobalOptionSetLocation _GenerateAttributesProxyClassEnumsUseSchemaGlobalEnum = ProxyClassAttributeEnumsGlobalOptionSetLocation.InGlobalOptionSetNamespace;
        [DataMember]
        public ProxyClassAttributeEnumsGlobalOptionSetLocation GenerateAttributesProxyClassEnumsUseSchemaGlobalEnum
        {
            get => _GenerateAttributesProxyClassEnumsUseSchemaGlobalEnum;
            set
            {
                this.OnPropertyChanging(nameof(GenerateAttributesProxyClassEnumsUseSchemaGlobalEnum));
                this._GenerateAttributesProxyClassEnumsUseSchemaGlobalEnum = value;
                this.OnPropertyChanged(nameof(GenerateAttributesProxyClassEnumsUseSchemaGlobalEnum));
            }
        }

        private bool _GenerateManyToOneProxyClass = false;
        /// <summary>
        /// Генерировать связи Many To One (N:1) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateManyToOneProxyClass
        {
            get => _GenerateManyToOneProxyClass;
            set
            {
                this.OnPropertyChanging(nameof(GenerateManyToOneProxyClass));
                this._GenerateManyToOneProxyClass = value;
                this.OnPropertyChanged(nameof(GenerateManyToOneProxyClass));
            }
        }

        private bool _GenerateOneToManyProxyClass = false;
        /// <summary>
        /// Генерировать One To Many (1:N) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateOneToManyProxyClass
        {
            get => _GenerateOneToManyProxyClass;
            set
            {
                this.OnPropertyChanging(nameof(GenerateOneToManyProxyClass));
                this._GenerateOneToManyProxyClass = value;
                this.OnPropertyChanged(nameof(GenerateOneToManyProxyClass));
            }
        }

        private bool _GenerateManyToManyProxyClass = true;
        /// <summary>
        /// Генерировать Many To Many (N:N) в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateManyToManyProxyClass
        {
            get => _GenerateManyToManyProxyClass;
            set
            {
                this.OnPropertyChanging(nameof(GenerateManyToManyProxyClass));
                this._GenerateManyToManyProxyClass = value;
                this.OnPropertyChanged(nameof(GenerateManyToManyProxyClass));
            }
        }

        private bool _GenerateLocalOptionSetProxyClass = true;
        /// <summary>
        /// Генерировать enum для значений Picklist-ов в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateLocalOptionSetProxyClass
        {
            get => _GenerateLocalOptionSetProxyClass;
            set
            {
                this.OnPropertyChanging(nameof(GenerateLocalOptionSetProxyClass));
                this._GenerateLocalOptionSetProxyClass = value;
                this.OnPropertyChanged(nameof(GenerateLocalOptionSetProxyClass));
            }
        }

        private bool _GenerateGlobalOptionSetProxyClass = false;
        [DataMember]
        public bool GenerateGlobalOptionSetProxyClass
        {
            get => _GenerateGlobalOptionSetProxyClass;
            set
            {
                this.OnPropertyChanging(nameof(GenerateGlobalOptionSetProxyClass));
                this._GenerateGlobalOptionSetProxyClass = value;
                this.OnPropertyChanged(nameof(GenerateGlobalOptionSetProxyClass));
            }
        }

        private bool _GenerateStatusOptionSetProxyClass = false;
        /// <summary>
        /// Генерировать enum для значения Statecode и Statuscode в файле с метаданными сущности
        /// </summary>
        [DataMember]
        public bool GenerateStatusOptionSetProxyClass
        {
            get => _GenerateStatusOptionSetProxyClass;
            set
            {
                this.OnPropertyChanging(nameof(GenerateStatusOptionSetProxyClass));
                this._GenerateStatusOptionSetProxyClass = value;
                this.OnPropertyChanged(nameof(GenerateStatusOptionSetProxyClass));
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
            this.TextEditorProgram = diskData.TextEditorProgram;
            this.CompareArgumentsFormat = diskData.CompareArgumentsFormat;
            this.CompareArgumentsThreeWayFormat = diskData.CompareArgumentsThreeWayFormat;
            this.ExportRibbonXmlForm = diskData.ExportRibbonXmlForm;
            this.ExportRibbonXmlHomepageGrid = diskData.ExportRibbonXmlHomepageGrid;
            this.ExportRibbonXmlSubGrid = diskData.ExportRibbonXmlSubGrid;
            this.ExportXmlAttributeOnNewLine = diskData.ExportXmlAttributeOnNewLine;

            this.GenerateAttributesSchema = diskData.GenerateAttributesSchema;
            this.GenerateManyToOneSchema = diskData.GenerateManyToOneSchema;
            this.GenerateOneToManySchema = diskData.GenerateOneToManySchema;
            this.GenerateManyToManySchema = diskData.GenerateManyToManySchema;
            this.GenerateLocalOptionSetSchema = diskData.GenerateLocalOptionSetSchema;
            this.GenerateGlobalOptionSetSchema = diskData.GenerateGlobalOptionSetSchema;
            this.GenerateStatusOptionSetSchema = diskData.GenerateStatusOptionSetSchema;
            this.GenerateKeysSchema = diskData.GenerateKeysSchema;
            this.GenerateIntoSchemaClass = diskData.GenerateIntoSchemaClass;

            this.GenerateAttributesProxyClass = diskData.GenerateAttributesProxyClass;
            this.GenerateManyToOneProxyClass = diskData.GenerateManyToOneProxyClass;
            this.GenerateOneToManyProxyClass = diskData.GenerateOneToManyProxyClass;
            this.GenerateManyToManyProxyClass = diskData.GenerateManyToManyProxyClass;
            this.GenerateLocalOptionSetProxyClass = diskData.GenerateLocalOptionSetProxyClass;
            this.GenerateGlobalOptionSetProxyClass = diskData.GenerateGlobalOptionSetProxyClass;
            this.GenerateStatusOptionSetProxyClass = diskData.GenerateStatusOptionSetProxyClass;

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