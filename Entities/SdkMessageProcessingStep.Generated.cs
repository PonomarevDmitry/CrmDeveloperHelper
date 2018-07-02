
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): Sdk Message Processing Step
    /// (Russian - 1049): Шаг обработки сообщения SDK
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): Sdk Message Processing Steps
    /// (Russian - 1049): Шаги обработки сообщения SDK
    /// 
    /// Description:
    /// (English - United States - 1033): Stage in the execution pipeline that a plug-in is to execute.
    /// (Russian - 1049): Стадия конвейерной обработки, на которой выполняется запуск подключаемого модуля.
    /// 
    /// PropertyName                          Value                               CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False                               False
    /// CanBePrimaryEntityInRelationship      False                               False
    /// CanBeRelatedEntityInRelationship      False                               False
    /// CanChangeHierarchicalRelationship     False                               False
    /// CanChangeTrackingBeEnabled            False                               False
    /// CanCreateAttributes                   False                               False
    /// CanCreateCharts                       False                               False
    /// CanCreateForms                        False                               False
    /// CanCreateViews                        False                               False
    /// CanEnableSyncToExternalSearchIndex    False                               False
    /// CanModifyAdditionalSettings           False                               True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 False
    /// CollectionSchemaName                  SdkMessageProcessingSteps
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         sdkmessageprocessingsteps
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False                               False
    /// IsAvailableOffline                    True
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False                               False
    /// IsCustomEntity                        False
    /// IsCustomizable                        False                               False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                               False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                               False
    /// IsMappable                            False                               False
    /// IsOfflineInMobileClient               False                               True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False                               False
    /// IsRenameable                          False                               False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                True
    /// IsValidForQueue                       False                               False
    /// IsVisibleInMobile                     False                               False
    /// IsVisibleInMobileClient               False                               False
    /// LogicalCollectionName                 sdkmessageprocessingsteps
    /// LogicalName                           sdkmessageprocessingstep
    /// ObjectTypeCode                        4608
    /// OwnershipType                         OrganizationOwned
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredSdkMessageProcessingStep
    /// SchemaName                            SdkMessageProcessingStep
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class SdkMessageProcessingStep
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "sdkmessageprocessingstep";

            public const string EntitySchemaName = "SdkMessageProcessingStep";

            public const string EntityPrimaryNameAttribute = "name";

            public const string EntityPrimaryIdAttribute = "sdkmessageprocessingstepid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Asynchronous Automatic Delete
                ///     (Russian - 1049): Асинхронное автоматическое удаление
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the asynchronous system job is automatically deleted on completion.
                ///     (Russian - 1049): Указывает, будет ли асинхронное системное задание автоматически удаляться по выполнении.
                /// 
                /// SchemaName: AsyncAutoDelete
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
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
                public const string asyncautodelete = "asyncautodelete";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Intent
                ///     (Russian - 1049): Назначение
                /// 
                /// Description:
                ///     (English - United States - 1033): Identifies whether a SDK Message Processing Step type will be ReadOnly or Read Write. false - ReadWrite, true - ReadOnly 
                ///     (Russian - 1049): Указывает, будет ли тип шага обработки сообщения SDK доступен только для чтения либо для чтения и записи. false - для чтения и записи (ReadWrite), true - только для чтения (ReadOnly).
                /// 
                /// SchemaName: CanUseReadOnlyConnection
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
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
                public const string canusereadonlyconnection = "canusereadonlyconnection";

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
                ///     (English - United States - 1033): Configuration
                ///     (Russian - 1049): Настройка
                /// 
                /// Description:
                ///     (English - United States - 1033): Step-specific configuration for the plug-in type. Passed to the plug-in constructor at run time.
                ///     (Russian - 1049): Конфигурация типа подключаемого модуля для конкретного шага. Передается конструктору подключаемого модуля при запуске.
                /// 
                /// SchemaName: Configuration
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1073741823
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string configuration = "configuration";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By
                ///     (Russian - 1049): Создано
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who created the SDK message processing step.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, создавшего шаг обработки сообщения SDK.
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
                ///     (English - United States - 1033): Date and time when the SDK message processing step was created.
                ///     (Russian - 1049): Дата и время создания шага обработки сообщения SDK.
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who created the sdkmessageprocessingstep.
                ///     (Russian - 1049): Уникальный идентификатор делегата, создавшего шаг обработки сообщения SDK.
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
                /// Description:
                ///     (English - United States - 1033): Customization level of the SDK message processing step.
                ///     (Russian - 1049): Уровень настройки шага обработки сообщения SDK.
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
                ///     (English - United States - 1033): Description
                ///     (Russian - 1049): Описание
                /// 
                /// Description:
                ///     (English - United States - 1033): Description of the SDK message processing step.
                ///     (Russian - 1049): Описание шага обработки сообщения SDK.
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
                ///     (English - United States - 1033): Event Handler
                ///     (Russian - 1049): Обработчик событий
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the associated event handler.
                ///     (Russian - 1049): Уникальный идентификатор связанного обработчика событий.
                /// 
                /// SchemaName: EventHandler
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: plugintype,serviceendpoint
                /// 
                ///     Target plugintype    PrimaryIdAttribute plugintypeid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Plug-in Type
                ///         (Russian - 1049): Тип подключаемого модуля
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Plug-in Types
                ///         (Russian - 1049): Типы подключаемых модулей
                ///         
                ///         Description:
                ///         (English - United States - 1033): Type that inherits from the IPlugin interface and is contained within a plug-in assembly.
                ///         (Russian - 1049): Тип, производный от интерфейса IPlugin, и содержащийся в сборке подключаемого модуля.
                /// 
                ///     Target serviceendpoint    PrimaryIdAttribute serviceendpointid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Service Endpoint
                ///         (Russian - 1049): Конечная точка сервиса
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Service Endpoints
                ///         (Russian - 1049): Конечные точки сервиса
                ///         
                ///         Description:
                ///         (English - United States - 1033): Service endpoint that can be contacted.
                ///         (Russian - 1049): Конечная точка сервиса, к которой можно обратиться.
                ///</summary>
                public const string eventhandler = "eventhandler";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Filtering Attributes
                ///     (Russian - 1049): Атрибуты фильтрации
                /// 
                /// Description:
                ///     (English - United States - 1033): Comma-separated list of attributes. If at least one of these attributes is modified, the plug-in should execute.
                ///     (Russian - 1049): Список разделенных запятыми атрибутов. Запуск подключаемого модуля следует выполнить при изменении по крайней мере одного из этих атрибутов.
                /// 
                /// SchemaName: FilteringAttributes
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100000
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string filteringattributes = "filteringattributes";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Impersonating User
                ///     (Russian - 1049): Олицетворяющий пользователь
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user to impersonate context when step is executed.
                ///     (Russian - 1049): Уникальный идентификатор пользователя для олицетворения контекста при выполнении шага.
                /// 
                /// SchemaName: ImpersonatingUserId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
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
                public const string impersonatinguserid = "impersonatinguserid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Introduced Version
                ///     (Russian - 1049): Версия введения
                /// 
                /// Description:
                ///     (English - United States - 1033): Version in which the form is introduced.
                ///     (Russian - 1049): Версия, в которой была введена форма.
                /// 
                /// SchemaName: IntroducedVersion
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 48
                /// Format = VersionNumber    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string introducedversion = "introducedversion";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Invocation Source
                ///     (Russian - 1049): Источник вызова
                /// 
                /// Description:
                ///     (English - United States - 1033): Identifies if a plug-in should be executed from a parent pipeline, a child pipeline, or both.
                ///     (Russian - 1049): Указывает источник запуска подключаемого модуля: родительский канал, дочерний канал или оба.
                /// 
                /// SchemaName: InvocationSource
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet sdkmessageprocessingstep_invocationsource
                /// DefaultFormValue = 0
                ///</summary>
                public const string invocationsource = "invocationsource";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Customizable
                ///     (Russian - 1049): Настраиваемый
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether this component can be customized.
                ///     (Russian - 1049): Сведения, указывающие на возможность настройки этого компонента.
                /// 
                /// SchemaName: IsCustomizable
                /// ManagedPropertyAttributeMetadata    AttributeType: ManagedProperty    AttributeTypeName: ManagedPropertyType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string iscustomizable = "iscustomizable";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Hidden
                ///     (Russian - 1049): Скрытый
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether this component should be hidden.
                ///     (Russian - 1049): Сведения, указывающие на необходимость скрытия этого компонента.
                /// 
                /// SchemaName: IsHidden
                /// ManagedPropertyAttributeMetadata    AttributeType: ManagedProperty    AttributeTypeName: ManagedPropertyType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string ishidden = "ishidden";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): State
                ///     (Russian - 1049): Область
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether this component is managed.
                ///     (Russian - 1049): Сведения о том, является ли компонент управляемым.
                /// 
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
                ///     (English - United States - 1033): Execution Mode
                ///     (Russian - 1049): Режим выполнения
                /// 
                /// Description:
                ///     (English - United States - 1033): Run-time mode of execution, for example, synchronous or asynchronous.
                ///     (Russian - 1049): Режим запуска, например синхронный или асинхронный.
                /// 
                /// SchemaName: Mode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet sdkmessageprocessingstep_mode
                /// DefaultFormValue = 0
                ///</summary>
                public const string mode = "mode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By
                ///     (Russian - 1049): Изменено
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who last modified the SDK message processing step.
                ///     (Russian - 1049): Уникальный идентификатор последнего пользователя, изменившего шаг обработки сообщения SDK.
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
                ///     (English - United States - 1033): Date and time when the SDK message processing step was last modified.
                ///     (Russian - 1049): Дата и время последнего изменения шага обработки сообщения SDK.
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who last modified the sdkmessageprocessingstep.
                ///     (Russian - 1049): Уникальный идентификатор делегата, внесшего последнее изменение в шаг обработки сообщения SDK.
                /// 
                /// SchemaName: ModifiedOnBehalfBy
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
                public const string modifiedonbehalfby = "modifiedonbehalfby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Name
                ///     (Russian - 1049): Название
                /// 
                /// Description:
                ///     (English - United States - 1033): Name of SdkMessage processing step.
                ///     (Russian - 1049): Название шага обработки сообщения SDK.
                /// 
                /// SchemaName: Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string name = "name";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the organization with which the SDK message processing step is associated.
                ///     (Russian - 1049): Уникальный идентификатор организации, с которой связан шаг обработки сообщения SDK.
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
                ///     (English - United States - 1033): Plug-In Type
                ///     (Russian - 1049): Тип подключаемого модуля
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the plug-in type associated with the step.
                ///     (Russian - 1049): Уникальный идентификатор типа подключаемого модуля, связанного с шагом.
                /// 
                /// SchemaName: PluginTypeId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: sdkmessagefilter
                /// 
                ///     Target sdkmessagefilter    PrimaryIdAttribute sdkmessagefilterid
                ///         DisplayName:
                ///         (English - United States - 1033): Sdk Message Filter
                ///         (Russian - 1049): Фильтр сообщения SDK
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Sdk Message Filters
                ///         (Russian - 1049): Фильтры сообщений SDK
                ///         
                ///         Description:
                ///         (English - United States - 1033): Filter that defines which SDK messages are valid for each type of entity.
                ///         (Russian - 1049): Фильтр, определяющий, какие сообщения SDK подходят для каждого типа сущности.
                ///</summary>
                public const string plugintypeid = "plugintypeid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Execution Order
                ///     (Russian - 1049): Порядок выполнения
                /// 
                /// Description:
                ///     (English - United States - 1033): Processing order within the stage.
                ///     (Russian - 1049): Порядок обработки внутри этапа.
                /// 
                /// SchemaName: Rank
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string rank = "rank";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): SdkMessage Filter
                ///     (Russian - 1049): Фильтр сообщения SDK
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the SDK message filter.
                ///     (Russian - 1049): Уникальный идентификатор фильтра сообщения SDK.
                /// 
                /// SchemaName: SdkMessageFilterId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: sdkmessagefilter
                /// 
                ///     Target sdkmessagefilter    PrimaryIdAttribute sdkmessagefilterid
                ///         DisplayName:
                ///         (English - United States - 1033): Sdk Message Filter
                ///         (Russian - 1049): Фильтр сообщения SDK
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Sdk Message Filters
                ///         (Russian - 1049): Фильтры сообщений SDK
                ///         
                ///         Description:
                ///         (English - United States - 1033): Filter that defines which SDK messages are valid for each type of entity.
                ///         (Russian - 1049): Фильтр, определяющий, какие сообщения SDK подходят для каждого типа сущности.
                ///</summary>
                public const string sdkmessagefilterid = "sdkmessagefilterid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): SDK Message
                ///     (Russian - 1049): Сообщение SDK
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the SDK message.
                ///     (Russian - 1049): Уникальный идентификатор сообщения SDK.
                /// 
                /// SchemaName: SdkMessageId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: sdkmessage
                /// 
                ///     Target sdkmessage    PrimaryIdAttribute sdkmessageid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Sdk Message
                ///         (Russian - 1049): Сообщение SDK
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Sdk Messages
                ///         (Russian - 1049): Сообщения SDK
                ///         
                ///         Description:
                ///         (English - United States - 1033): Message that is supported by the SDK.
                ///         (Russian - 1049): Сообщение, поддерживаемое SDK.
                ///</summary>
                public const string sdkmessageid = "sdkmessageid";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the SDK message processing step entity.
                ///     (Russian - 1049): Уникальный идентификатор сущности шага обработки сообщения SDK.
                /// 
                /// SchemaName: SdkMessageProcessingStepId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string sdkmessageprocessingstepid = "sdkmessageprocessingstepid";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the SDK message processing step.
                ///     (Russian - 1049): Уникальный идентификатор шага обработки сообщения SDK.
                /// 
                /// SchemaName: SdkMessageProcessingStepIdUnique
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string sdkmessageprocessingstepidunique = "sdkmessageprocessingstepidunique";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): SDK Message Processing Step Secure Configuration
                ///     (Russian - 1049): Безопасная конфигурация шага обработки сообщения SDK
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the Sdk message processing step secure configuration.
                ///     (Russian - 1049): Уникальный идентификатор безопасной конфигурации шага обработки сообщения SDK.
                /// 
                /// SchemaName: SdkMessageProcessingStepSecureConfigId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: sdkmessageprocessingstepsecureconfig
                /// 
                ///     Target sdkmessageprocessingstepsecureconfig    PrimaryIdAttribute sdkmessageprocessingstepsecureconfigid
                ///         DisplayName:
                ///         (English - United States - 1033): Sdk Message Processing Step Secure Configuration
                ///         (Russian - 1049): Безопасная конфигурация шага обработки сообщения SDK
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Sdk Message Processing Step Secure Configurations
                ///         (Russian - 1049): Безопасные конфигурации шага обработки сообщения SDK
                ///         
                ///         Description:
                ///         (English - United States - 1033): Non-public custom configuration that is passed to a plug-in's constructor.
                ///         (Russian - 1049): Внутренняя настраиваемая конфигурация, передаваемая конструктору подключаемого модуля.
                ///</summary>
                public const string sdkmessageprocessingstepsecureconfigid = "sdkmessageprocessingstepsecureconfigid";

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
                ///     (English - United States - 1033): Execution Stage
                ///     (Russian - 1049): Этап выполнения
                /// 
                /// Description:
                ///     (English - United States - 1033): Stage in the execution pipeline that the SDK message processing step is in.
                ///     (Russian - 1049): Стадия конвейерной обработки, в которую входит шаг обработки сообщения SDK.
                /// 
                /// SchemaName: Stage
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet sdkmessageprocessingstep_stage
                /// DefaultFormValue = 10
                ///</summary>
                public const string stage = "stage";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Status
                ///     (Russian - 1049): Состояние
                /// 
                /// Description:
                ///     (English - United States - 1033): Status of the SDK message processing step.
                ///     (Russian - 1049): Состояние шага обработки сообщения SDK.
                /// 
                /// SchemaName: StateCode
                /// StateAttributeMetadata    AttributeType: State    AttributeTypeName: StateType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// DefaultFormValue = 0
                ///</summary>
                public const string statecode = "statecode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Status Reason
                ///     (Russian - 1049): Причина состояния
                /// 
                /// Description:
                ///     (English - United States - 1033): Reason for the status of the SDK message processing step.
                ///     (Russian - 1049): Причина состояния шага обработки сообщения SDK.
                /// 
                /// SchemaName: StatusCode
                /// StatusAttributeMetadata    AttributeType: Status    AttributeTypeName: StatusType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// DefaultFormValue = -1
                ///</summary>
                public const string statuscode = "statuscode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Deployment
                ///     (Russian - 1049): Развертывание
                /// 
                /// Description:
                ///     (English - United States - 1033): Deployment that the SDK message processing step should be executed on; server, client, or both.
                ///     (Russian - 1049): Развертывание, в котором должен быть выполнен шаг обработки сообщения SDK: сервер, клиент или оба.
                /// 
                /// SchemaName: SupportedDeployment
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet sdkmessageprocessingstep_supporteddeployment
                /// DefaultFormValue = 0
                ///</summary>
                public const string supporteddeployment = "supporteddeployment";

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
                /// Description:
                ///     (English - United States - 1033): Number that identifies a specific revision of the SDK message processing step. 
                ///     (Russian - 1049): Число, определяющее редакцию шага обработки сообщения SDK. 
                /// 
                /// SchemaName: VersionNumber
                /// BigIntAttributeMetadata    AttributeType: BigInt    AttributeTypeName: BigIntType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -9223372036854775808    MaxValue = 9223372036854775807
                ///</summary>
                public const string versionnumber = "versionnumber";
            }

            #endregion Attributes.

            #region OptionSets.

            public static partial class OptionSets
            {
                #region State and Status OptionSets.

                ///<summary>
                /// Attribute: statecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Status
                ///     (Russian - 1049): Состояние
                /// 
                /// Description:
                ///     (English - United States - 1033): Status of the SDK message processing step.
                ///     (Russian - 1049): Состояние шага обработки сообщения SDK.
                ///</summary>
                public enum statecode
                {
                    ///<summary>
                    /// Default statuscode: Enabled_1, 1
                    /// InvariantName: Enabled
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Enabled
                    ///     (Russian - 1049): Включено
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Enabled_0 = 0,

                    ///<summary>
                    /// Default statuscode: Disabled_2, 2
                    /// InvariantName: Disabled
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Disabled
                    ///     (Russian - 1049): Отключено
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Disabled_1 = 1,
                }

                ///<summary>
                /// Attribute: statuscode
                /// Value Format: Statecode_Statuscode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Status
                ///     (Russian - 1049): Состояние
                /// 
                /// Description:
                ///     (English - United States - 1033): Status of the SDK message processing step.
                ///     (Russian - 1049): Состояние шага обработки сообщения SDK.
                ///</summary>
                public enum statuscode
                {
                    ///<summary>
                    /// Linked Statecode: Enabled_0, 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Enabled
                    ///     (Russian - 1049): Включено
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Enabled_0_Enabled_1 = 1,

                    ///<summary>
                    /// Linked Statecode: Disabled_1, 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Disabled
                    ///     (Russian - 1049): Отключено
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Disabled_1_Disabled_2 = 2,
                }

                #endregion State and Status OptionSets.

                #region Picklist OptionSet OptionSets.

                ///<summary>
                /// Attribute: invocationsource
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Invocation Source
                ///     (Russian - 1049): Источник вызова
                /// 
                /// Description:
                ///     (English - United States - 1033): Identifies if a plug-in should be executed from a parent pipeline, a child pipeline, or both.
                ///     (Russian - 1049): Указывает источник запуска подключаемого модуля: родительский канал, дочерний канал или оба.
                /// 
                /// Local System  OptionSet sdkmessageprocessingstep_invocationsource
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Invocation Source
                ///     (Russian - 1049): Источник вызова
                /// 
                /// Description:
                ///     (English - United States - 1033): Identifies if a plug-in should be executed from a parent pipeline, a child pipeline, or both.
                ///     (Russian - 1049): Указывает источник запуска подключаемого модуля: родительский канал, дочерний канал или оба.
                ///</summary>
                public enum invocationsource
                {
                    ///<summary>
                    /// -1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Internal
                    ///     (Russian - 1049): Внутренний
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Internal_1 = -1,

                    ///<summary>
                    /// 0
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Parent
                    ///     (Russian - 1049): Родительский объект
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Parent_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Child
                    ///     (Russian - 1049): Дочерний объект
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Child_1 = 1,
                }

                ///<summary>
                /// Attribute: mode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Execution Mode
                ///     (Russian - 1049): Режим выполнения
                /// 
                /// Description:
                ///     (English - United States - 1033): Run-time mode of execution, for example, synchronous or asynchronous.
                ///     (Russian - 1049): Режим запуска, например синхронный или асинхронный.
                /// 
                /// Local System  OptionSet sdkmessageprocessingstep_mode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Mode
                ///     (Russian - 1049): Режим
                /// 
                /// Description:
                ///     (English - United States - 1033): Run-time mode of execution, for example, synchronous or asynchronous.
                ///     (Russian - 1049): Режим запуска, например синхронный или асинхронный.
                ///</summary>
                public enum mode
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Synchronous
                    ///     (Russian - 1049): Синхронный
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Synchronous_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Asynchronous
                    ///     (Russian - 1049): Асинхронный
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Asynchronous_1 = 1,
                }

                ///<summary>
                /// Attribute: stage
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Execution Stage
                ///     (Russian - 1049): Этап выполнения
                /// 
                /// Description:
                ///     (English - United States - 1033): Stage in the execution pipeline that the SDK message processing step is in.
                ///     (Russian - 1049): Стадия конвейерной обработки, в которую входит шаг обработки сообщения SDK.
                /// 
                /// Local System  OptionSet sdkmessageprocessingstep_stage
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Stage
                ///     (Russian - 1049): Этап
                /// 
                /// Description:
                ///     (English - United States - 1033): Stage in the execution pipeline that the SDK message processing step is in.
                ///     (Russian - 1049): Стадия конвейерной обработки, в которую входит шаг обработки сообщения SDK.
                ///</summary>
                public enum stage
                {
                    ///<summary>
                    /// 5
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Initial Pre-operation (For internal use only)
                    ///     (Russian - 1049): Начальная операция вне транзакции
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Initial_Pre_operation_For_internal_use_only_5 = 5,

                    ///<summary>
                    /// 10
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Pre-validation
                    ///     (Russian - 1049): Перед основной операцией вне транзакции
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Pre_validation_10 = 10,

                    ///<summary>
                    /// 15
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Internal Pre-operation Before External Plugins (For internal use only)
                    ///     (Russian - 1049): Внутренняя перед основной операцией перед внешними подключаемыми модулями внутри транзакции
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Internal_Pre_operation_Before_External_Plugins_For_internal_use_only_15 = 15,

                    ///<summary>
                    /// 20
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Pre-operation
                    ///     (Russian - 1049): Перед основной операцией внутри транзакции
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Pre_operation_20 = 20,

                    ///<summary>
                    /// 25
                    /// DisplayOrder: 5
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Internal Pre-operation After External Plugins (For internal use only)
                    ///     (Russian - 1049): Внутренняя перед основной операцией после внешних подключаемых модулей внутри транзакции
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Internal_Pre_operation_After_External_Plugins_For_internal_use_only_25 = 25,

                    ///<summary>
                    /// 30
                    /// DisplayOrder: 6
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Main Operation (For internal use only)
                    ///     (Russian - 1049): Основная операция
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Main_Operation_For_internal_use_only_30 = 30,

                    ///<summary>
                    /// 35
                    /// DisplayOrder: 7
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Internal Post-operation Before External Plugins (For internal use only)
                    ///     (Russian - 1049): Внутренняя после основной операции перед внешними подключаемыми модулями внутри транзакции
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Internal_Post_operation_Before_External_Plugins_For_internal_use_only_35 = 35,

                    ///<summary>
                    /// 40
                    /// DisplayOrder: 8
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Post-operation
                    ///     (Russian - 1049): После основной операции внутри транзакции
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Post_operation_40 = 40,

                    ///<summary>
                    /// 45
                    /// DisplayOrder: 9
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Internal Post-operation After External Plugins (For internal use only)
                    ///     (Russian - 1049): Внутренняя после основной операции после внешних подключаемых модулей внутри транзакции
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Internal_Post_operation_After_External_Plugins_For_internal_use_only_45 = 45,

                    ///<summary>
                    /// 50
                    /// DisplayOrder: 10
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Post-operation (Deprecated)
                    ///     (Russian - 1049): После основной операции вне транзакции
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Post_operation_Deprecated_50 = 50,

                    ///<summary>
                    /// 55
                    /// DisplayOrder: 11
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Final Post-operation (For internal use only)
                    ///     (Russian - 1049): Конечная операция вне транзакции
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Final_Post_operation_For_internal_use_only_55 = 55,
                }

                ///<summary>
                /// Attribute: supporteddeployment
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Deployment
                ///     (Russian - 1049): Развертывание
                /// 
                /// Description:
                ///     (English - United States - 1033): Deployment that the SDK message processing step should be executed on; server, client, or both.
                ///     (Russian - 1049): Развертывание, в котором должен быть выполнен шаг обработки сообщения SDK: сервер, клиент или оба.
                /// 
                /// Local System  OptionSet sdkmessageprocessingstep_supporteddeployment
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Supported Deployment
                ///     (Russian - 1049): Поддерживаемое развертывание
                /// 
                /// Description:
                ///     (English - United States - 1033): Deployment that the SDK message processing step should be executed on; server, client, or both.
                ///     (Russian - 1049): Развертывание, в котором должен быть выполнен шаг обработки сообщения SDK: сервер, клиент или оба.
                ///</summary>
                public enum supporteddeployment
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Server Only
                    ///     (Russian - 1049): Только сервер
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Server_Only_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Microsoft Dynamics 365 Client for Outlook Only
                    ///     (Russian - 1049): Только клиент Microsoft Dynamics 365 для Outlook
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Microsoft_Dynamics_365_Client_for_Outlook_Only_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Both
                    ///     (Russian - 1049): Оба
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Both_2 = 2,
                }

                #endregion Picklist OptionSets.
            }

            #endregion OptionSets.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship createdby_sdkmessageprocessingstep
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     createdby_sdkmessageprocessingstep
                /// ReferencingEntityNavigationPropertyName    createdby
                /// IsCustomizable                             False                                 False
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
                public static partial class createdby_sdkmessageprocessingstep
                {
                    public const string Name = "createdby_sdkmessageprocessingstep";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_sdkmessageprocessingstep = "sdkmessageprocessingstep";

                    public const string ReferencingAttribute_createdby = "createdby";
                }

                ///<summary>
                /// N:1 - Relationship impersonatinguserid_sdkmessageprocessingstep
                /// 
                /// PropertyName                               Value                                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     impersonatinguserid_sdkmessageprocessingstep
                /// ReferencingEntityNavigationPropertyName    impersonatinguserid
                /// IsCustomizable                             False                                           False
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
                public static partial class impersonatinguserid_sdkmessageprocessingstep
                {
                    public const string Name = "impersonatinguserid_sdkmessageprocessingstep";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_sdkmessageprocessingstep = "sdkmessageprocessingstep";

                    public const string ReferencingAttribute_impersonatinguserid = "impersonatinguserid";
                }

                ///<summary>
                /// N:1 - Relationship lk_sdkmessageprocessingstep_createdonbehalfby
                /// 
                /// PropertyName                               Value                                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_sdkmessageprocessingstep_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    createdonbehalfby
                /// IsCustomizable                             False                                            False
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
                public static partial class lk_sdkmessageprocessingstep_createdonbehalfby
                {
                    public const string Name = "lk_sdkmessageprocessingstep_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_sdkmessageprocessingstep = "sdkmessageprocessingstep";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_sdkmessageprocessingstep_modifiedonbehalfby
                /// 
                /// PropertyName                               Value                                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_sdkmessageprocessingstep_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    modifiedonbehalfby
                /// IsCustomizable                             False                                             False
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
                public static partial class lk_sdkmessageprocessingstep_modifiedonbehalfby
                {
                    public const string Name = "lk_sdkmessageprocessingstep_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_sdkmessageprocessingstep = "sdkmessageprocessingstep";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship modifiedby_sdkmessageprocessingstep
                /// 
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     modifiedby_sdkmessageprocessingstep
                /// ReferencingEntityNavigationPropertyName    modifiedby
                /// IsCustomizable                             False                                  False
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
                public static partial class modifiedby_sdkmessageprocessingstep
                {
                    public const string Name = "modifiedby_sdkmessageprocessingstep";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_sdkmessageprocessingstep = "sdkmessageprocessingstep";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";
                }

                ///<summary>
                /// N:1 - Relationship organization_sdkmessageprocessingstep
                /// 
                /// PropertyName                               Value                                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_sdkmessageprocessingstep
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                    False
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
                public static partial class organization_sdkmessageprocessingstep
                {
                    public const string Name = "organization_sdkmessageprocessingstep";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_sdkmessageprocessingstep = "sdkmessageprocessingstep";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// N:1 - Relationship plugintype_sdkmessageprocessingstep
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
                /// ReferencedEntity plugintype:
                ///     DisplayName:
                ///     (English - United States - 1033): Plug-in Type
                ///     (Russian - 1049): Тип подключаемого модуля
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Plug-in Types
                ///     (Russian - 1049): Типы подключаемых модулей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Type that inherits from the IPlugin interface and is contained within a plug-in assembly.
                ///     (Russian - 1049): Тип, производный от интерфейса IPlugin, и содержащийся в сборке подключаемого модуля.
                ///</summary>
                public static partial class plugintype_sdkmessageprocessingstep
                {
                    public const string Name = "plugintype_sdkmessageprocessingstep";

                    public const string ReferencedEntity_plugintype = "plugintype";

                    public const string ReferencedAttribute_plugintypeid = "plugintypeid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_sdkmessageprocessingstep = "sdkmessageprocessingstep";

                    public const string ReferencingAttribute_eventhandler = "eventhandler";
                }

                ///<summary>
                /// N:1 - Relationship plugintypeid_sdkmessageprocessingstep
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
                /// ReferencedEntity plugintype:
                ///     DisplayName:
                ///     (English - United States - 1033): Plug-in Type
                ///     (Russian - 1049): Тип подключаемого модуля
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Plug-in Types
                ///     (Russian - 1049): Типы подключаемых модулей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Type that inherits from the IPlugin interface and is contained within a plug-in assembly.
                ///     (Russian - 1049): Тип, производный от интерфейса IPlugin, и содержащийся в сборке подключаемого модуля.
                ///</summary>
                public static partial class plugintypeid_sdkmessageprocessingstep
                {
                    public const string Name = "plugintypeid_sdkmessageprocessingstep";

                    public const string ReferencedEntity_plugintype = "plugintype";

                    public const string ReferencedAttribute_plugintypeid = "plugintypeid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_sdkmessageprocessingstep = "sdkmessageprocessingstep";

                    public const string ReferencingAttribute_plugintypeid = "plugintypeid";
                }

                ///<summary>
                /// N:1 - Relationship sdkmessagefilterid_sdkmessageprocessingstep
                /// 
                /// PropertyName                               Value                                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sdkmessagefilterid_sdkmessageprocessingstep
                /// ReferencingEntityNavigationPropertyName    sdkmessagefilterid
                /// IsCustomizable                             False                                          False
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
                /// ReferencedEntity sdkmessagefilter:
                ///     DisplayName:
                ///     (English - United States - 1033): Sdk Message Filter
                ///     (Russian - 1049): Фильтр сообщения SDK
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Sdk Message Filters
                ///     (Russian - 1049): Фильтры сообщений SDK
                ///     
                ///     Description:
                ///     (English - United States - 1033): Filter that defines which SDK messages are valid for each type of entity.
                ///     (Russian - 1049): Фильтр, определяющий, какие сообщения SDK подходят для каждого типа сущности.
                ///</summary>
                public static partial class sdkmessagefilterid_sdkmessageprocessingstep
                {
                    public const string Name = "sdkmessagefilterid_sdkmessageprocessingstep";

                    public const string ReferencedEntity_sdkmessagefilter = "sdkmessagefilter";

                    public const string ReferencedAttribute_sdkmessagefilterid = "sdkmessagefilterid";

                    public const string ReferencingEntity_sdkmessageprocessingstep = "sdkmessageprocessingstep";

                    public const string ReferencingAttribute_sdkmessagefilterid = "sdkmessagefilterid";
                }

                ///<summary>
                /// N:1 - Relationship sdkmessageid_sdkmessageprocessingstep
                /// 
                /// PropertyName                               Value                                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sdkmessageid_sdkmessageprocessingstep
                /// ReferencingEntityNavigationPropertyName    sdkmessageid
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
                /// ReferencedEntity sdkmessage:
                ///     DisplayName:
                ///     (English - United States - 1033): Sdk Message
                ///     (Russian - 1049): Сообщение SDK
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Sdk Messages
                ///     (Russian - 1049): Сообщения SDK
                ///     
                ///     Description:
                ///     (English - United States - 1033): Message that is supported by the SDK.
                ///     (Russian - 1049): Сообщение, поддерживаемое SDK.
                ///</summary>
                public static partial class sdkmessageid_sdkmessageprocessingstep
                {
                    public const string Name = "sdkmessageid_sdkmessageprocessingstep";

                    public const string ReferencedEntity_sdkmessage = "sdkmessage";

                    public const string ReferencedAttribute_sdkmessageid = "sdkmessageid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_sdkmessageprocessingstep = "sdkmessageprocessingstep";

                    public const string ReferencingAttribute_sdkmessageid = "sdkmessageid";
                }

                ///<summary>
                /// N:1 - Relationship sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep
                /// 
                /// PropertyName                               Value                                                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep
                /// ReferencingEntityNavigationPropertyName    sdkmessageprocessingstepsecureconfigid
                /// IsCustomizable                             False                                                              False
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
                /// ReferencedEntity sdkmessageprocessingstepsecureconfig:
                ///     DisplayName:
                ///     (English - United States - 1033): Sdk Message Processing Step Secure Configuration
                ///     (Russian - 1049): Безопасная конфигурация шага обработки сообщения SDK
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Sdk Message Processing Step Secure Configurations
                ///     (Russian - 1049): Безопасные конфигурации шага обработки сообщения SDK
                ///     
                ///     Description:
                ///     (English - United States - 1033): Non-public custom configuration that is passed to a plug-in's constructor.
                ///     (Russian - 1049): Внутренняя настраиваемая конфигурация, передаваемая конструктору подключаемого модуля.
                ///</summary>
                public static partial class sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep
                {
                    public const string Name = "sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep";

                    public const string ReferencedEntity_sdkmessageprocessingstepsecureconfig = "sdkmessageprocessingstepsecureconfig";

                    public const string ReferencedAttribute_sdkmessageprocessingstepsecureconfigid = "sdkmessageprocessingstepsecureconfigid";

                    public const string ReferencingEntity_sdkmessageprocessingstep = "sdkmessageprocessingstep";

                    public const string ReferencingAttribute_sdkmessageprocessingstepsecureconfigid = "sdkmessageprocessingstepsecureconfigid";
                }

                ///<summary>
                /// N:1 - Relationship serviceendpoint_sdkmessageprocessingstep
                /// 
                /// PropertyName                               Value                                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     serviceendpoint_sdkmessageprocessingstep
                /// ReferencingEntityNavigationPropertyName    eventhandler_serviceendpoint
                /// IsCustomizable                             False                                       False
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
                /// ReferencedEntity serviceendpoint:
                ///     DisplayName:
                ///     (English - United States - 1033): Service Endpoint
                ///     (Russian - 1049): Конечная точка сервиса
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Service Endpoints
                ///     (Russian - 1049): Конечные точки сервиса
                ///     
                ///     Description:
                ///     (English - United States - 1033): Service endpoint that can be contacted.
                ///     (Russian - 1049): Конечная точка сервиса, к которой можно обратиться.
                ///</summary>
                public static partial class serviceendpoint_sdkmessageprocessingstep
                {
                    public const string Name = "serviceendpoint_sdkmessageprocessingstep";

                    public const string ReferencedEntity_serviceendpoint = "serviceendpoint";

                    public const string ReferencedAttribute_serviceendpointid = "serviceendpointid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_sdkmessageprocessingstep = "sdkmessageprocessingstep";

                    public const string ReferencingAttribute_eventhandler = "eventhandler";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship SdkMessageProcessingStep_AsyncOperations
                /// 
                /// PropertyName                               Value                                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     SdkMessageProcessingStep_AsyncOperations
                /// ReferencingEntityNavigationPropertyName    owningextensionid
                /// IsCustomizable                             False                                       False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
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
                /// ReferencingEntity asyncoperation:
                ///     DisplayName:
                ///     (English - United States - 1033): System Job
                ///     (Russian - 1049): Системное задание
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): System Jobs
                ///     (Russian - 1049): Системные задания
                ///     
                ///     Description:
                ///     (English - United States - 1033): Process whose execution can proceed independently or in the background.
                ///     (Russian - 1049): Процесс, который может выполняться независимо или в фоновом режиме.
                ///</summary>
                public static partial class sdkmessageprocessingstep_asyncoperations
                {
                    public const string Name = "SdkMessageProcessingStep_AsyncOperations";

                    public const string ReferencedEntity_sdkmessageprocessingstep = "sdkmessageprocessingstep";

                    public const string ReferencedAttribute_sdkmessageprocessingstepid = "sdkmessageprocessingstepid";

                    public const string ReferencingEntity_asyncoperation = "asyncoperation";

                    public const string ReferencingAttribute_owningextensionid = "owningextensionid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship sdkmessageprocessingstepid_sdkmessageprocessingstepimage
                /// 
                /// PropertyName                               Value                                                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sdkmessageprocessingstepid_sdkmessageprocessingstepimage
                /// ReferencingEntityNavigationPropertyName    sdkmessageprocessingstepid
                /// IsCustomizable                             False                                                       False
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
                /// ReferencingEntity sdkmessageprocessingstepimage:
                ///     DisplayName:
                ///     (English - United States - 1033): Sdk Message Processing Step Image
                ///     (Russian - 1049): Образ шага обработки сообщения SDK
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Sdk Message Processing Step Images
                ///     (Russian - 1049): Образы шагов обработки сообщения SDK
                ///     
                ///     Description:
                ///     (English - United States - 1033): Copy of an entity's attributes before or after the core system operation.
                ///     (Russian - 1049): Копирование атрибутов сущности перед операцией базовой системы или после нее.
                ///</summary>
                public static partial class sdkmessageprocessingstepid_sdkmessageprocessingstepimage
                {
                    public const string Name = "sdkmessageprocessingstepid_sdkmessageprocessingstepimage";

                    public const string ReferencedEntity_sdkmessageprocessingstep = "sdkmessageprocessingstep";

                    public const string ReferencedAttribute_sdkmessageprocessingstepid = "sdkmessageprocessingstepid";

                    public const string ReferencingEntity_sdkmessageprocessingstepimage = "sdkmessageprocessingstepimage";

                    public const string ReferencingAttribute_sdkmessageprocessingstepid = "sdkmessageprocessingstepid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_sdkmessageprocessingstep
                /// 
                /// PropertyName                               Value                                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_sdkmessageprocessingstep
                /// ReferencingEntityNavigationPropertyName    objectid_sdkmessageprocessingstep
                /// IsCustomizable                             False                                              False
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
                public static partial class userentityinstancedata_sdkmessageprocessingstep
                {
                    public const string Name = "userentityinstancedata_sdkmessageprocessingstep";

                    public const string ReferencedEntity_sdkmessageprocessingstep = "sdkmessageprocessingstep";

                    public const string ReferencedAttribute_sdkmessageprocessingstepid = "sdkmessageprocessingstepid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}
