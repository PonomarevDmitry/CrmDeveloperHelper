
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): App
    /// (Russian - 1049): Приложение
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): Apps
    /// (Russian - 1049): Приложения
    /// 
    /// Description:
    /// (English - United States - 1033): To provide specific CRM UI context .For internal use only
    /// (Russian - 1049): Для определения конкретного контекста пользовательского интерфейса CRM. Только для внутреннего использования
    /// 
    /// PropertyName                          Value                CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     True                 False
    /// CanBePrimaryEntityInRelationship      True                 False
    /// CanBeRelatedEntityInRelationship      True                 False
    /// CanChangeHierarchicalRelationship     False                False
    /// CanChangeTrackingBeEnabled            False                False
    /// CanCreateAttributes                   False                False
    /// CanCreateCharts                       False                False
    /// CanCreateForms                        False                False
    /// CanCreateViews                        False                False
    /// CanEnableSyncToExternalSearchIndex    False                False
    /// CanModifyAdditionalSettings           False                True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 False
    /// CollectionSchemaName                  AppModules
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         appmodule
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          True
    /// IsAuditEnabled                        False                False
    /// IsAvailableOffline                    False
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False                False
    /// IsCustomEntity                        False
    /// IsCustomizable                        False                False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          True
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                True
    /// IsMappable                            False                False
    /// IsOfflineInMobileClient               False                True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             True
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              True                 False
    /// IsRenameable                          False                False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False                False
    /// IsVisibleInMobile                     False                False
    /// IsVisibleInMobileClient               True                 False
    /// LogicalCollectionName                 appmodules
    /// LogicalName                           appmodule
    /// ObjectTypeCode                        9006
    /// OwnershipType                         OrganizationOwned
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredAppModule
    /// SchemaName                            AppModule
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class AppModule
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "appmodule";

            public const string EntitySchemaName = "AppModule";

            public const string EntityPrimaryNameAttribute = "name";

            public const string EntityPrimaryIdAttribute = "appmoduleid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): AppModuleId
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier for entity instances
                ///     (Russian - 1049): Уникальный код для экземпляров сущности
                /// 
                /// SchemaName: AppModuleId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string appmoduleid = "appmoduleid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): App Module Unique Id
                ///     (Russian - 1049): Уникальный идентификатор модуля приложения
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the App Module used when synchronizing customizations for the Microsoft Dynamics 365 client for Outlook
                ///     (Russian - 1049): Уникальный идентификатор модуля приложения, используемого при синхронизации настроек для клиента Microsoft Dynamics 365 для Outlook.
                /// 
                /// SchemaName: AppModuleIdUnique
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string appmoduleidunique = "appmoduleidunique";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): App Module Version
                ///     (Russian - 1049): Версия модуля приложения
                /// 
                /// Description:
                /// 
                /// SchemaName: AppModuleVersion
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 48
                /// Format = VersionNumber    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string appmoduleversion = "appmoduleversion";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): App Module Xml Managed
                ///     (Russian - 1049): XML-код модуля приложения управляется
                /// 
                /// SchemaName: AppModuleXmlManaged
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string appmodulexmlmanaged = "appmodulexmlmanaged";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Client Type
                ///     (Russian - 1049): Тип клиента
                /// 
                /// Description:
                ///     (English - United States - 1033): Client Type such as desktop or MoCA
                ///     (Russian - 1049): Тип клиента, например настольный компьютер или MoCA
                /// 
                /// SchemaName: ClientType
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 1    MaxValue = 8
                /// Format = None
                ///</summary>
                public const string clienttype = "clienttype";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Component State
                ///     (Russian - 1049): Состояние компонента
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only
                ///     (Russian - 1049): Только для внутреннего использования
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
                /// Description:
                ///     (English - United States - 1033): Contains configuration XML
                ///     (Russian - 1049): Содержит XML-код конфигурации
                /// 
                /// SchemaName: ConfigXML
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string configxml = "configxml";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By
                ///     (Russian - 1049): Кем создано
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who created the record.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, создавшего запись.
                /// 
                /// SchemaName: CreatedBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = True
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
                ///     (Russian - 1049): Когда создано
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the record was created.
                ///     (Russian - 1049): Дата и время, когда была создана запись.
                /// 
                /// SchemaName: CreatedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateAndTime
                ///</summary>
                public const string createdon = "createdon";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By (Delegate)
                ///     (Russian - 1049): Кем создано (представитель)
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the delegate user who created the record.
                ///     (Russian - 1049): Уникальный идентификатор пользователя-представителя, создавшего запись.
                /// 
                /// SchemaName: CreatedOnBehalfBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = True
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
                ///     (English - United States - 1033): Description
                ///     (Russian - 1049): Описание
                /// 
                /// Description:
                ///     (English - United States - 1033): Description for entity
                ///     (Russian - 1049): Описание сущности
                /// 
                /// SchemaName: Description
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 2000
                /// Format = Text    ImeMode = Auto    IsLocalizable = True
                ///</summary>
                public const string description = "description";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Form Factor
                ///     (Russian - 1049): Форм-фактор
                /// 
                /// Description:
                /// 
                /// SchemaName: FormFactor
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 1    MaxValue = 8
                /// Format = None
                ///</summary>
                public const string formfactor = "formfactor";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Import Sequence Number
                ///     (Russian - 1049): Порядковый номер импорта
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the data import or data migration that created this record.
                ///     (Russian - 1049): Уникальный идентификатор операции импорта или переноса данных, в результате которой была создана эта запись.
                /// 
                /// SchemaName: ImportSequenceNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string importsequencenumber = "importsequencenumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Introduced Version
                ///     (Russian - 1049): Введенная версия
                /// 
                /// Description:
                ///     (English - United States - 1033): Version in which the similarity rule is introduced.
                ///     (Russian - 1049): Версия, в которой было введено правило подобия.
                /// 
                /// SchemaName: IntroducedVersion
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = VersionNumber    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string introducedversion = "introducedversion";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Default
                ///     (Russian - 1049): По умолчанию
                /// 
                /// Description:
                /// 
                /// SchemaName: IsDefault
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string isdefault = "isdefault";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): IsFeatured
                /// 
                /// Description:
                ///     (English - United States - 1033): Is Featured
                ///     (Russian - 1049): Является популярным
                /// 
                /// SchemaName: IsFeatured
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string isfeatured = "isfeatured";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): IsManaged
                /// 
                /// Description:
                ///     (English - United States - 1033): Is Managed
                ///     (Russian - 1049): Является управляемым
                /// 
                /// SchemaName: IsManaged
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: ApplicationRequired
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
                ///     (English - United States - 1033): Modified By
                ///     (Russian - 1049): Кем изменено
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who modified the record.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, изменившего запись.
                /// 
                /// SchemaName: ModifiedBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = True
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
                ///     (Russian - 1049): Когда изменено
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the record was modified.
                ///     (Russian - 1049): Дата и время изменения записи.
                /// 
                /// SchemaName: ModifiedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateAndTime
                ///</summary>
                public const string modifiedon = "modifiedon";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By (Delegate)
                ///     (Russian - 1049): Кем изменено (представитель)
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the delegate user who modified the record.
                ///     (Russian - 1049): Уникальный идентификатор пользователя-представителя, изменившего запись.
                /// 
                /// SchemaName: ModifiedOnBehalfBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = True
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
                ///     (Russian - 1049): Имя
                /// 
                /// Description:
                ///     (English - United States - 1033): Name of App Module
                ///     (Russian - 1049): Имя модуля приложения
                /// 
                /// SchemaName: Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = True
                ///</summary>
                public const string name = "name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Organization
                ///     (Russian - 1049): Организация
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the organization associated with the app.
                ///     (Russian - 1049): Уникальный идентификатор организации, связанной с приложением.
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
                ///     (English - United States - 1033): Record Created On
                ///     (Russian - 1049): Когда создана запись
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time that the record was migrated.
                ///     (Russian - 1049): Дата и время переноса записи.
                /// 
                /// SchemaName: OverriddenCreatedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateOnly
                ///</summary>
                public const string overriddencreatedon = "overriddencreatedon";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Overwrite Time
                ///     (Russian - 1049): Перезаписать время
                /// 
                /// Description:
                ///     (English - United States - 1033): Internal use only
                ///     (Russian - 1049): Только для внутреннего использования
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
                ///     (English - United States - 1033): Published On
                ///     (Russian - 1049): Дата публикации
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the record was published.
                ///     (Russian - 1049): Дата и время публикации записи.
                /// 
                /// SchemaName: PublishedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateAndTime
                ///</summary>
                public const string publishedon = "publishedon";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Publisher
                ///     (Russian - 1049): Издатель
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the publisher.
                ///     (Russian - 1049): Уникальный идентификатор издателя.
                /// 
                /// SchemaName: PublisherId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: publisher
                /// 
                ///     Target publisher    PrimaryIdAttribute publisherid    PrimaryNameAttribute friendlyname
                ///         DisplayName:
                ///         (English - United States - 1033): Publisher
                ///         (Russian - 1049): Издатель
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Publishers
                ///         (Russian - 1049): Издатели
                ///         
                ///         Description:
                ///         (English - United States - 1033): A publisher of a CRM solution.
                ///         (Russian - 1049): Издатель решения по управлению отношениями с клиентами (CRM).
                ///</summary>
                public const string publisherid = "publisherid";

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
                /// IsValidForCreate: False    IsValidForRead: False    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string supportingsolutionid = "supportingsolutionid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Unique Name
                ///     (Russian - 1049): Уникальное имя
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique Name of App Module
                ///     (Russian - 1049): Уникальное имя модуля приложения
                /// 
                /// SchemaName: UniqueName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string uniquename = "uniquename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): URL
                ///     (Russian - 1049): URL-адрес
                /// 
                /// Description:
                ///     (English - United States - 1033): Contains URL
                ///     (Russian - 1049): Содержит URL-адрес
                /// 
                /// SchemaName: URL
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string url = "url";

                ///<summary>
                /// SchemaName: VersionNumber
                /// BigIntAttributeMetadata    AttributeType: BigInt    AttributeTypeName: BigIntType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -9223372036854775808    MaxValue = 9223372036854775807
                ///</summary>
                public const string versionnumber = "versionnumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Web Resource Id
                ///     (Russian - 1049): Идентификатор веб-ресурса
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the Web Resource
                ///     (Russian - 1049): Уникальный идентификатор веб-ресурса
                /// 
                /// SchemaName: WebResourceId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string webresourceid = "webresourceid";
            }

            #endregion Attributes.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship lk_appmodule_createdby
                /// 
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     systemuser_appmodule_createdby
                /// ReferencingEntityNavigationPropertyName    appmodule_createdby
                /// IsCustomizable                             True                              False
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
                public static partial class lk_appmodule_createdby
                {
                    public const string Name = "lk_appmodule_createdby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_appmodule = "appmodule";

                    public const string ReferencingAttribute_createdby = "createdby";
                }

                ///<summary>
                /// N:1 - Relationship lk_appmodule_createdonbehalfby
                /// 
                /// PropertyName                               Value                                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     systemuser_appmodule_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    appmodule_createdonbehalfby
                /// IsCustomizable                             True                                      False
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
                public static partial class lk_appmodule_createdonbehalfby
                {
                    public const string Name = "lk_appmodule_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_appmodule = "appmodule";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_appmodule_modifiedby
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     systemuser_appmodule_modifiedby
                /// ReferencingEntityNavigationPropertyName    appmodule_modifiedby
                /// IsCustomizable                             True                               False
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
                public static partial class lk_appmodule_modifiedby
                {
                    public const string Name = "lk_appmodule_modifiedby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_appmodule = "appmodule";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";
                }

                ///<summary>
                /// N:1 - Relationship lk_appmodule_modifiedonbehalfby
                /// 
                /// PropertyName                               Value                                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     systemuser_appmodule_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    appmodule_modifiedonbehalfby
                /// IsCustomizable                             True                                       False
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
                public static partial class lk_appmodule_modifiedonbehalfby
                {
                    public const string Name = "lk_appmodule_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_appmodule = "appmodule";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship organization_appmodule
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_appmodule
                /// ReferencingEntityNavigationPropertyName    organization_appmodule_appmodule
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
                public static partial class organization_appmodule
                {
                    public const string Name = "organization_appmodule";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_appmodule = "appmodule";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// N:1 - Relationship publisher_appmodule
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     publisher_appmodule
                /// ReferencingEntityNavigationPropertyName    publisher_appmodule_appmodule
                /// IsCustomizable                             False                            False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                Restrict
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity publisher:
                ///     DisplayName:
                ///     (English - United States - 1033): Publisher
                ///     (Russian - 1049): Издатель
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Publishers
                ///     (Russian - 1049): Издатели
                ///     
                ///     Description:
                ///     (English - United States - 1033): A publisher of a CRM solution.
                ///     (Russian - 1049): Издатель решения по управлению отношениями с клиентами (CRM).
                ///</summary>
                public static partial class publisher_appmodule
                {
                    public const string Name = "publisher_appmodule";

                    public const string ReferencedEntity_publisher = "publisher";

                    public const string ReferencedAttribute_publisherid = "publisherid";

                    public const string ReferencedEntity_PrimaryNameAttribute_friendlyname = "friendlyname";

                    public const string ReferencingEntity_appmodule = "appmodule";

                    public const string ReferencingAttribute_publisherid = "publisherid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship appmodule_appmodulecomponent
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     appmodule_appmodulecomponent
                /// ReferencingEntityNavigationPropertyName    appmoduleid
                /// IsCustomizable                             False                           False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity appmodulecomponent:
                ///     DisplayName:
                ///     (English - United States - 1033): App Module Component
                ///     (Russian - 1049): Компонент модуля приложения
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): App Module Components
                ///     (Russian - 1049): Компоненты модуля приложения
                ///     
                ///     Description:
                ///     (English - United States - 1033): App Module Component entity
                ///     (Russian - 1049): Сущность компонента модуля приложения
                ///</summary>
                public static partial class appmodule_appmodulecomponent
                {
                    public const string Name = "appmodule_appmodulecomponent";

                    public const string ReferencedEntity_appmodule = "appmodule";

                    public const string ReferencedAttribute_appmoduleidunique = "appmoduleidunique";

                    public const string ReferencingEntity_appmodulecomponent = "appmodulecomponent";

                    public const string ReferencingAttribute_appmoduleidunique = "appmoduleidunique";
                }
            }

            #endregion Relationship OneToMany - 1:N.

            #region Relationship ManyToMany - N:N.

            public static partial class ManyToMany
            {
                ///<summary>
                /// N:N - Relationship appmoduleroles_association
                /// 
                /// PropertyName                                   Value                         CanBeChanged
                /// Entity1NavigationPropertyName                  appmoduleroles_association
                /// Entity2NavigationPropertyName                  appmoduleroles_association
                /// IsCustomizable                                 False                         False
                /// IsCustomRelationship                           False
                /// IsValidForAdvancedFind                         True
                /// RelationshipType                               ManyToManyRelationship
                /// SecurityTypes                                  None
                /// Entity1AssociatedMenuConfiguration.Behavior    DoNotDisplay
                /// Entity1AssociatedMenuConfiguration.Group       Details
                /// Entity1AssociatedMenuConfiguration.Order       null
                /// Entity2AssociatedMenuConfiguration.Behavior    DoNotDisplay
                /// Entity2AssociatedMenuConfiguration.Group       Details
                /// Entity2AssociatedMenuConfiguration.Order       null
                /// 
                /// Entity2LogicalName role:
                ///     DisplayName:
                ///     (English - United States - 1033): Security Role
                ///     (Russian - 1049): Роль безопасности
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Security Roles
                ///     (Russian - 1049): Роли безопасности
                ///     
                ///     Description:
                ///     (English - United States - 1033): Grouping of security privileges. Users are assigned roles that authorize their access to the Microsoft CRM system.
                ///     (Russian - 1049): Группа привилегий безопасности. Пользователям назначаются роли, которые контролируют их доступ к CRM-системе Microsoft.
                ///</summary>
                public static partial class appmoduleroles_association
                {
                    public const string Name = "appmoduleroles_association";

                    public const string IntersectEntity_appmoduleroles = "appmoduleroles";

                    public const string Entity1_appmodule = "appmodule";

                    public const string Entity1Attribute_appmoduleid = "appmoduleid";

                    public const string Entity2_role = "role";

                    public const string Entity2Attribute_roleid = "roleid";

                    public const string Entity2LogicalNamename = "name";
                }
            }

            #endregion Relationship ManyToMany - N:N.
        }
    }
}
