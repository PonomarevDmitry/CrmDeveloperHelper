
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): Plug-in Type
    /// (Russian - 1049): Тип подключаемого модуля
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): Plug-in Types
    /// (Russian - 1049): Типы подключаемых модулей
    /// 
    /// Description:
    /// (English - United States - 1033): Type that inherits from the IPlugin interface and is contained within a plug-in assembly.
    /// (Russian - 1049): Тип, производный от интерфейса IPlugin, и содержащийся в сборке подключаемого модуля.
    /// 
    /// PropertyName                          Value                 CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False                 False
    /// CanBePrimaryEntityInRelationship      False                 False
    /// CanBeRelatedEntityInRelationship      False                 False
    /// CanChangeHierarchicalRelationship     False                 False
    /// CanChangeTrackingBeEnabled            False                 False
    /// CanCreateAttributes                   False                 False
    /// CanCreateCharts                       False                 False
    /// CanCreateForms                        False                 False
    /// CanCreateViews                        False                 False
    /// CanEnableSyncToExternalSearchIndex    False                 False
    /// CanModifyAdditionalSettings           False                 True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 False
    /// CollectionSchemaName                  PluginTypes
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         plugintypes
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False                 False
    /// IsAvailableOffline                    True
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False                 False
    /// IsCustomEntity                        False
    /// IsCustomizable                        False                 False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                 False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                 False
    /// IsMappable                            False                 False
    /// IsOfflineInMobileClient               False                 True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False                 False
    /// IsRenameable                          False                 False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                True
    /// IsValidForQueue                       False                 False
    /// IsVisibleInMobile                     False                 False
    /// IsVisibleInMobileClient               False                 False
    /// LogicalCollectionName                 plugintypes
    /// LogicalName                           plugintype
    /// ObjectTypeCode                        4602
    /// OwnershipType                         OrganizationOwned
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredPluginType
    /// SchemaName                            PluginType
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class PluginType
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "plugintype";

            public const string EntitySchemaName = "PluginType";

            public const string EntityPrimaryNameAttribute = "name";

            public const string EntityPrimaryIdAttribute = "plugintypeid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Assembly Name
                ///     (Russian - 1049): Название сборки
                /// 
                /// Description:
                ///     (English - United States - 1033): Full path name of the plug-in assembly.
                ///     (Russian - 1049): Полный путь к сборке подключаемого модуля.
                /// 
                /// SchemaName: AssemblyName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string assemblyname = "assemblyname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Component State
                ///     (Russian - 1049): Состояние компонента
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: ComponentState
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Global System  OptionSet componentstate
                /// DefaultFormValue = -1
                ///</summary>
                public const string componentstate = "componentstate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By
                ///     (Russian - 1049): Создано
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who created the plug-in type.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, создавшего тип подключаемого модуля.
                /// 
                /// SchemaName: CreatedBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///         (English - United States - 1033): User
                ///         (Russian - 1049): Пользователь
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///         (Russian - 1049): Пользователи
                ///         
                ///         Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///         (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public const string createdby = "createdby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created On
                ///     (Russian - 1049): Дата создания
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the plug-in type was created.
                ///     (Russian - 1049): Дата и время создания подключаемого модуля.
                /// 
                /// SchemaName: CreatedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateAndTime
                ///</summary>
                public const string createdon = "createdon";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By (Delegate)
                ///     (Russian - 1049): Кем создано (делегат)
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the delegate user who created the plugintype.
                ///     (Russian - 1049): Уникальный идентификатор пользователя-делегата, создавшего plugintype.
                /// 
                /// SchemaName: CreatedOnBehalfBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///         (English - United States - 1033): User
                ///         (Russian - 1049): Пользователь
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///         (Russian - 1049): Пользователи
                ///         
                ///         Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///         (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public const string createdonbehalfby = "createdonbehalfby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Culture
                ///     (Russian - 1049): Культура
                /// 
                /// Description:
                ///     (English - United States - 1033): Culture code for the plug-in assembly.
                ///     (Russian - 1049): Код культуры для сборки подключаемого модуля.
                /// 
                /// SchemaName: Culture
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 32
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string culture = "culture";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Customization level of the plug-in type.
                ///     (Russian - 1049): Уровень настройки типа подключаемого модуля.
                /// 
                /// SchemaName: CustomizationLevel
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -255    MaxValue = 255
                /// Format = None
                ///</summary>
                public const string customizationlevel = "customizationlevel";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Custom Workflow Activity Info
                ///     (Russian - 1049): Сведения о настраиваемом действии бизнес-процесса
                /// 
                /// Description:
                ///     (English - United States - 1033): Serialized Custom Activity Type information, including required arguments. For more information, see SandboxCustomActivityInfo.
                ///     (Russian - 1049): Сведения о сериализованном настраиваемом типе действия. Дополнительные сведения см. в описании SandboxCustomActivityInfo.
                /// 
                /// SchemaName: CustomWorkflowActivityInfo
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1048576
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string customworkflowactivityinfo = "customworkflowactivityinfo";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Description
                ///     (Russian - 1049): Описание
                /// 
                /// Description:
                ///     (English - United States - 1033): Description of the plug-in type.
                ///     (Russian - 1049): Описание типа подключаемого модуля.
                /// 
                /// SchemaName: Description
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string description = "description";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Display Name
                ///     (Russian - 1049): Отображаемое имя
                /// 
                /// Description:
                ///     (English - United States - 1033): User friendly name for the plug-in.
                ///     (Russian - 1049): Понятное имя пользователя подключаемого модуля.
                /// 
                /// SchemaName: FriendlyName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string friendlyname = "friendlyname";

                ///<summary>
                /// SchemaName: IsManaged
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Unmanaged
                ///     (Russian - 1049): Неуправляемый
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Managed
                ///     (Russian - 1049): Управляемый
                /// TrueOption = 1
                ///</summary>
                public const string ismanaged = "ismanaged";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Workflow Activity
                ///     (Russian - 1049): Является действием бизнес-процесса?
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates if the plug-in is a custom activity for workflows.
                ///     (Russian - 1049): Указывает, является ли подключаемый модуль настраиваемым действием для бизнес-процессов.
                /// 
                /// SchemaName: IsWorkflowActivity
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): No
                ///     (Russian - 1049): Нет
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Yes
                ///     (Russian - 1049): Да
                /// TrueOption = 1
                ///</summary>
                public const string isworkflowactivity = "isworkflowactivity";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Version major
                ///     (Russian - 1049): Основной номер версии
                /// 
                /// Description:
                ///     (English - United States - 1033): Major of the version number of the assembly for the plug-in type.
                ///     (Russian - 1049): Основной номер версии сборки типа подключаемого модуля.
                /// 
                /// SchemaName: Major
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 65534
                /// Format = None
                ///</summary>
                public const string major = "major";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Version minor
                ///     (Russian - 1049): Дополнительный номер версии
                /// 
                /// Description:
                ///     (English - United States - 1033): Minor of the version number of the assembly for the plug-in type.
                ///     (Russian - 1049): Дополнительный номер версии сборки типа подключаемого модуля.
                /// 
                /// SchemaName: Minor
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 65534
                /// Format = None
                ///</summary>
                public const string minor = "minor";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By
                ///     (Russian - 1049): Изменено
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who last modified the plug-in type.
                ///     (Russian - 1049): Уникальный идентификатор последнего пользователя, изменившего подключаемый модуль.
                /// 
                /// SchemaName: ModifiedBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///         (English - United States - 1033): User
                ///         (Russian - 1049): Пользователь
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///         (Russian - 1049): Пользователи
                ///         
                ///         Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///         (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public const string modifiedby = "modifiedby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified On
                ///     (Russian - 1049): Дата изменения
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the plug-in type was last modified.
                ///     (Russian - 1049): Дата и время последнего изменения подключаемого модуля.
                /// 
                /// SchemaName: ModifiedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateAndTime
                ///</summary>
                public const string modifiedon = "modifiedon";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By (Delegate)
                ///     (Russian - 1049): Кем изменено (делегат)
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the delegate user who last modified the plugintype.
                ///     (Russian - 1049): Уникальный идентификатор пользователя-делегата, последним изменившего plugintype.
                /// 
                /// SchemaName: ModifiedOnBehalfBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///         (English - United States - 1033): User
                ///         (Russian - 1049): Пользователь
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///         (Russian - 1049): Пользователи
                ///         
                ///         Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///         (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public const string modifiedonbehalfby = "modifiedonbehalfby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Name
                ///     (Russian - 1049): Название
                /// 
                /// Description:
                ///     (English - United States - 1033): Name of the plug-in type.
                ///     (Russian - 1049): Название типа подключаемого модуля.
                /// 
                /// SchemaName: Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string name = "name";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the organization with which the plug-in type is associated.
                ///     (Russian - 1049): Уникальный идентификатор организации, с которой связан подключаемый модуль.
                /// 
                /// SchemaName: OrganizationId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: organization
                /// 
                ///     Target organization    PrimaryIdAttribute organizationid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Organization
                ///         (Russian - 1049): Предприятие
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Organizations
                ///         (Russian - 1049): Предприятия
                ///         
                ///         Description:
                ///         (English - United States - 1033): Top level of the Microsoft Dynamics 365 business hierarchy. The organization can be a specific business, holding company, or corporation.
                ///         (Russian - 1049): Верхний уровень бизнес-иерархии Microsoft Dynamics 365. Организация может являться конкретной компанией, холдингом или корпорацией.
                ///</summary>
                public const string organizationid = "organizationid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Record Overwrite Time
                ///     (Russian - 1049): Время замены записи
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: OverwriteTime
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateOnly
                ///</summary>
                public const string overwritetime = "overwritetime";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Plugin Assembly
                ///     (Russian - 1049): Сборка подключаемого модуля
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the plug-in assembly that contains this plug-in type.
                ///     (Russian - 1049): Уникальный идентификатор сборки подключаемого модуля, включающей этот тип подключаемого модуля.
                /// 
                /// SchemaName: PluginAssemblyId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: pluginassembly
                /// 
                ///     Target pluginassembly    PrimaryIdAttribute pluginassemblyid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Plug-in Assembly
                ///         (Russian - 1049): Сборка подключаемого модуля
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Plug-in Assemblies
                ///         (Russian - 1049): Сборки подключаемых модулей
                ///         
                ///         Description:
                ///         (English - United States - 1033): Assembly that contains one or more plug-in types.
                ///         (Russian - 1049): Сборка, содержащая один или несколько типов подключаемых модулей.
                ///</summary>
                public const string pluginassemblyid = "pluginassemblyid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Plug-in Type
                ///     (Russian - 1049): Тип подключаемого модуля
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the plug-in type.
                ///     (Russian - 1049): Уникальный идентификатор подключаемого модуля.
                /// 
                /// SchemaName: PluginTypeId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string plugintypeid = "plugintypeid";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the plug-in type.
                ///     (Russian - 1049): Уникальный идентификатор подключаемого модуля.
                /// 
                /// SchemaName: PluginTypeIdUnique
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string plugintypeidunique = "plugintypeidunique";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Public Key Token
                ///     (Russian - 1049): Токен открытого ключа
                /// 
                /// Description:
                ///     (English - United States - 1033): Public key token of the assembly for the plug-in type.
                ///     (Russian - 1049): Токен общего ключа сборки для этого типа подключаемого модуля.
                /// 
                /// SchemaName: PublicKeyToken
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 32
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string publickeytoken = "publickeytoken";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Solution
                ///     (Russian - 1049): Решение
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the associated solution.
                ///     (Russian - 1049): Уникальный идентификатор связанного решения.
                /// 
                /// SchemaName: SolutionId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string solutionid = "solutionid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Solution
                ///     (Russian - 1049): Решение
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: SupportingSolutionId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: False    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string supportingsolutionid = "supportingsolutionid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Type Name
                ///     (Russian - 1049): Название типа
                /// 
                /// Description:
                ///     (English - United States - 1033): Fully qualified type name of the plug-in type.
                ///     (Russian - 1049): Полное имя типа подключаемого модуля.
                /// 
                /// SchemaName: TypeName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string typename = "typename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Version
                ///     (Russian - 1049): Версия
                /// 
                /// Description:
                ///     (English - United States - 1033): Version number of the assembly for the plug-in type.
                ///     (Russian - 1049): Номер версии сборки подключаемого модуля.
                /// 
                /// SchemaName: Version
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 48
                /// Format = VersionNumber    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string version = "version";

                ///<summary>
                /// SchemaName: VersionNumber
                /// BigIntAttributeMetadata    AttributeType: BigInt    AttributeTypeName: BigIntType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -9223372036854775808    MaxValue = 9223372036854775807
                ///</summary>
                public const string versionnumber = "versionnumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Workflow Activity Group Name
                ///     (Russian - 1049): Название группы действия бизнес-процесса
                /// 
                /// Description:
                ///     (English - United States - 1033): Group name of workflow custom activity.
                ///     (Russian - 1049): Название группы настраиваемого действия бизнес-процесса.
                /// 
                /// SchemaName: WorkflowActivityGroupName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string workflowactivitygroupname = "workflowactivitygroupname";
            }

            #endregion Attributes.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship createdby_plugintype
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     createdby_plugintype
                /// ReferencingEntityNavigationPropertyName    createdby
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                NoCascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity systemuser:
                ///     DisplayName:
                ///     (English - United States - 1033): User
                ///     (Russian - 1049): Пользователь
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Users
                ///     (Russian - 1049): Пользователи
                ///     
                ///     Description:
                ///     (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///     (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public static partial class createdby_plugintype
                {
                    public const string Name = "createdby_plugintype";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_plugintype = "plugintype";

                    public const string ReferencingAttribute_createdby = "createdby";
                }

                ///<summary>
                /// N:1 - Relationship lk_plugintype_createdonbehalfby
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_plugintype_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    createdonbehalfby
                /// IsCustomizable                             False                              False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                NoCascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity systemuser:
                ///     DisplayName:
                ///     (English - United States - 1033): User
                ///     (Russian - 1049): Пользователь
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Users
                ///     (Russian - 1049): Пользователи
                ///     
                ///     Description:
                ///     (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///     (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public static partial class lk_plugintype_createdonbehalfby
                {
                    public const string Name = "lk_plugintype_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_plugintype = "plugintype";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_plugintype_modifiedonbehalfby
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_plugintype_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    modifiedonbehalfby
                /// IsCustomizable                             False                               False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                NoCascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity systemuser:
                ///     DisplayName:
                ///     (English - United States - 1033): User
                ///     (Russian - 1049): Пользователь
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Users
                ///     (Russian - 1049): Пользователи
                ///     
                ///     Description:
                ///     (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///     (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public static partial class lk_plugintype_modifiedonbehalfby
                {
                    public const string Name = "lk_plugintype_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_plugintype = "plugintype";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship modifiedby_plugintype
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     modifiedby_plugintype
                /// ReferencingEntityNavigationPropertyName    modifiedby
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                NoCascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity systemuser:
                ///     DisplayName:
                ///     (English - United States - 1033): User
                ///     (Russian - 1049): Пользователь
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Users
                ///     (Russian - 1049): Пользователи
                ///     
                ///     Description:
                ///     (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///     (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public static partial class modifiedby_plugintype
                {
                    public const string Name = "modifiedby_plugintype";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_plugintype = "plugintype";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";
                }

                ///<summary>
                /// N:1 - Relationship organization_plugintype
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_plugintype
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                      False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                NoCascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity organization:
                ///     DisplayName:
                ///     (English - United States - 1033): Organization
                ///     (Russian - 1049): Предприятие
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Organizations
                ///     (Russian - 1049): Предприятия
                ///     
                ///     Description:
                ///     (English - United States - 1033): Top level of the Microsoft Dynamics 365 business hierarchy. The organization can be a specific business, holding company, or corporation.
                ///     (Russian - 1049): Верхний уровень бизнес-иерархии Microsoft Dynamics 365. Организация может являться конкретной компанией, холдингом или корпорацией.
                ///</summary>
                public static partial class organization_plugintype
                {
                    public const string Name = "organization_plugintype";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_plugintype = "plugintype";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// N:1 - Relationship pluginassembly_plugintype
                /// 
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     pluginassembly_plugintype
                /// ReferencingEntityNavigationPropertyName    pluginassemblyid
                /// IsCustomizable                             False                        False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity pluginassembly:
                ///     DisplayName:
                ///     (English - United States - 1033): Plug-in Assembly
                ///     (Russian - 1049): Сборка подключаемого модуля
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Plug-in Assemblies
                ///     (Russian - 1049): Сборки подключаемых модулей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Assembly that contains one or more plug-in types.
                ///     (Russian - 1049): Сборка, содержащая один или несколько типов подключаемых модулей.
                ///</summary>
                public static partial class pluginassembly_plugintype
                {
                    public const string Name = "pluginassembly_plugintype";

                    public const string ReferencedEntity_pluginassembly = "pluginassembly";

                    public const string ReferencedAttribute_pluginassemblyid = "pluginassemblyid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_plugintype = "plugintype";

                    public const string ReferencingAttribute_pluginassemblyid = "pluginassemblyid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship plugin_type_service
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     plugin_type_service
                /// ReferencingEntityNavigationPropertyName    strategyid
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Restrict
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity service:
                ///     DisplayName:
                ///     (English - United States - 1033): Service
                ///     (Russian - 1049): Сервис
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Services
                ///     (Russian - 1049): Сервисы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity that represents work done to satisfy a customer's need.
                ///     (Russian - 1049): Действие, представляющее работы, выполненные с целью удовлетворения потребностей клиента.
                ///</summary>
                public static partial class plugin_type_service
                {
                    public const string Name = "plugin_type_service";

                    public const string ReferencedEntity_plugintype = "plugintype";

                    public const string ReferencedAttribute_plugintypeid = "plugintypeid";

                    public const string ReferencingEntity_service = "service";

                    public const string ReferencingAttribute_strategyid = "strategyid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship plugintype_plugintypestatistic
                /// 
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     plugintype_plugintypestatistic
                /// ReferencingEntityNavigationPropertyName    plugintypeid
                /// IsCustomizable                             False                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity plugintypestatistic:
                ///     DisplayName:
                ///     (English - United States - 1033): Plug-in Type Statistic
                ///     (Russian - 1049): Статистика по типам подключаемых модулей
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Plug-in Type Statistics
                ///     (Russian - 1049): Статистики по типам подключаемых модулей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Plug-in type statistic.
                ///     (Russian - 1049): Статистика по типам подключаемых модулей.
                ///</summary>
                public static partial class plugintype_plugintypestatistic
                {
                    public const string Name = "plugintype_plugintypestatistic";

                    public const string ReferencedEntity_plugintype = "plugintype";

                    public const string ReferencedAttribute_plugintypeid = "plugintypeid";

                    public const string ReferencingEntity_plugintypestatistic = "plugintypestatistic";

                    public const string ReferencingAttribute_plugintypeid = "plugintypeid";
                }

                ///<summary>
                /// 1:N - Relationship plugintype_sdkmessageprocessingstep
                /// 
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     plugintype_sdkmessageprocessingstep
                /// ReferencingEntityNavigationPropertyName    eventhandler_plugintype
                /// IsCustomizable                             False                                  False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity sdkmessageprocessingstep:
                ///     DisplayName:
                ///     (English - United States - 1033): Sdk Message Processing Step
                ///     (Russian - 1049): Шаг обработки сообщения SDK
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Sdk Message Processing Steps
                ///     (Russian - 1049): Шаги обработки сообщения SDK
                ///     
                ///     Description:
                ///     (English - United States - 1033): Stage in the execution pipeline that a plug-in is to execute.
                ///     (Russian - 1049): Стадия конвейерной обработки, на которой выполняется запуск подключаемого модуля.
                ///</summary>
                public static partial class plugintype_sdkmessageprocessingstep
                {
                    public const string Name = "plugintype_sdkmessageprocessingstep";

                    public const string ReferencedEntity_plugintype = "plugintype";

                    public const string ReferencedAttribute_plugintypeid = "plugintypeid";

                    public const string ReferencingEntity_sdkmessageprocessingstep = "sdkmessageprocessingstep";

                    public const string ReferencingAttribute_eventhandler = "eventhandler";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship plugintypeid_sdkmessageprocessingstep
                /// 
                /// PropertyName                               Value                                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     plugintypeid_sdkmessageprocessingstep
                /// ReferencingEntityNavigationPropertyName    plugintypeid
                /// IsCustomizable                             False                                    False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Restrict
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity sdkmessageprocessingstep:
                ///     DisplayName:
                ///     (English - United States - 1033): Sdk Message Processing Step
                ///     (Russian - 1049): Шаг обработки сообщения SDK
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Sdk Message Processing Steps
                ///     (Russian - 1049): Шаги обработки сообщения SDK
                ///     
                ///     Description:
                ///     (English - United States - 1033): Stage in the execution pipeline that a plug-in is to execute.
                ///     (Russian - 1049): Стадия конвейерной обработки, на которой выполняется запуск подключаемого модуля.
                ///</summary>
                public static partial class plugintypeid_sdkmessageprocessingstep
                {
                    public const string Name = "plugintypeid_sdkmessageprocessingstep";

                    public const string ReferencedEntity_plugintype = "plugintype";

                    public const string ReferencedAttribute_plugintypeid = "plugintypeid";

                    public const string ReferencingEntity_sdkmessageprocessingstep = "sdkmessageprocessingstep";

                    public const string ReferencingAttribute_plugintypeid = "plugintypeid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_plugintype
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_plugintype
                /// ReferencingEntityNavigationPropertyName    objectid_plugintype
                /// IsCustomizable                             False                                False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity userentityinstancedata:
                ///     DisplayName:
                ///     (English - United States - 1033): User Entity Instance Data
                ///     (Russian - 1049): Данные экземпляра сущности пользователя
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Per User item instance data
                ///     (Russian - 1049): Данные экземпляра позиции "на пользователя"
                ///</summary>
                public static partial class userentityinstancedata_plugintype
                {
                    public const string Name = "userentityinstancedata_plugintype";

                    public const string ReferencedEntity_plugintype = "plugintype";

                    public const string ReferencedAttribute_plugintypeid = "plugintypeid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}
