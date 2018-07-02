
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): App Module Component
    /// (Russian - 1049): Компонент модуля приложения
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): App Module Components
    /// (Russian - 1049): Компоненты модуля приложения
    /// 
    /// Description:
    /// (English - United States - 1033): App Module Component entity
    /// (Russian - 1049): Сущность компонента модуля приложения
    /// 
    /// PropertyName                          Value                         CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     True                          False
    /// CanBePrimaryEntityInRelationship      True                          False
    /// CanBeRelatedEntityInRelationship      True                          False
    /// CanChangeHierarchicalRelationship     False                         False
    /// CanChangeTrackingBeEnabled            False                         False
    /// CanCreateAttributes                   False                         False
    /// CanCreateCharts                       False                         False
    /// CanCreateForms                        False                         False
    /// CanCreateViews                        False                         False
    /// CanEnableSyncToExternalSearchIndex    False                         False
    /// CanModifyAdditionalSettings           False                         True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 False
    /// CollectionSchemaName                  App Module Components
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         appmodulecomponents
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          True
    /// IsAuditEnabled                        True                          True
    /// IsAvailableOffline                    False
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False                         False
    /// IsCustomEntity                        False
    /// IsCustomizable                        False                         False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                         False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                         True
    /// IsMappable                            False                         False
    /// IsOfflineInMobileClient               False                         True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        False
    /// IsPrivate                             True
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              True                          False
    /// IsRenameable                          False                         False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False                         False
    /// IsVisibleInMobile                     False                         False
    /// IsVisibleInMobileClient               True                          False
    /// LogicalCollectionName                 appmodulecomponents
    /// LogicalName                           appmodulecomponent
    /// ObjectTypeCode                        9007
    /// OwnershipType                         None
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredAppModuleComponent
    /// SchemaName                            AppModuleComponent
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class AppModuleComponent
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "appmodulecomponent";

            public const string EntitySchemaName = "AppModuleComponent";

            public const string EntityPrimaryIdAttribute = "appmodulecomponentid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): App Module Component
                ///     (Russian - 1049): Компонент модуля приложения
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier for entity instances
                ///     (Russian - 1049): Уникальный код для экземпляров сущности
                /// 
                /// SchemaName: AppModuleComponentId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string appmodulecomponentid = "appmodulecomponentid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Application Component Unique Id
                ///     (Russian - 1049): Уникальный идентификатор компонента приложения
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the Application Component used when synchronizing customizations for the Microsoft Dynamics 365 client for Outlook
                ///     (Russian - 1049): Уникальный идентификатор компонента приложения, используемого при синхронизации настроек для клиента Microsoft Dynamics 365 для Outlook.
                /// 
                /// SchemaName: AppModuleComponentIdUnique
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string appmodulecomponentidunique = "appmodulecomponentidunique";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): AppModule
                /// 
                /// Description:
                ///     (English - United States - 1033): The App Module Id Unique
                ///     (Russian - 1049): Уникальный идентификатор модуля приложения
                /// 
                /// SchemaName: AppModuleIdUnique
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: appmodule
                /// 
                ///     Target appmodule    PrimaryIdAttribute appmoduleid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): App
                ///         (Russian - 1049): Приложение
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Apps
                ///         (Russian - 1049): Приложения
                ///         
                ///         Description:
                ///         (English - United States - 1033): To provide specific CRM UI context .For internal use only
                ///         (Russian - 1049): Для определения конкретного контекста пользовательского интерфейса CRM. Только для внутреннего использования
                ///</summary>
                public const string appmoduleidunique = "appmoduleidunique";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Object Type Code
                ///     (Russian - 1049): Код типа объекта
                /// 
                /// Description:
                ///     (English - United States - 1033): The object type code of the component.
                ///     (Russian - 1049): Код типа объекта компонента.
                /// 
                /// SchemaName: ComponentType
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet appmodulecomponent_componenttype
                /// DefaultFormValue = Null
                ///</summary>
                public const string componenttype = "componenttype";

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
                ///     (Russian - 1049): Дата создания
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the record was created.
                ///     (Russian - 1049): Дата и время создания записи.
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
                ///     (English - United States - 1033): ExchangeRate
                ///     (Russian - 1049): Курс валюты
                /// 
                /// Description:
                ///     (English - United States - 1033): Exchange rate for the currency associated with the Application Component with respect to the base currency.
                ///     (Russian - 1049): Обменный курс валюты, связанной с компонентом приложения, по отношению к базовой валюте.
                /// 
                /// SchemaName: ExchangeRate
                /// DecimalAttributeMetadata    AttributeType: Decimal    AttributeTypeName: DecimalType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0,0000000001    MaxValue = 100000000000    Precision = 10
                /// ImeMode = Disabled
                ///</summary>
                public const string exchangerate = "exchangerate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Introduced Version
                ///     (Russian - 1049): Введенная версия
                /// 
                /// Description:
                ///     (English - United States - 1033): Version in which the application component record is introduced.
                ///     (Russian - 1049): Версия, в которой была введена запись компонента приложения.
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
                ///     (English - United States - 1033): Is Metadata
                ///     (Russian - 1049): Является метаданными
                /// 
                /// Description:
                /// 
                /// SchemaName: IsMetadata
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Data
                ///     (Russian - 1049): Данные
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Metadata
                ///     (Russian - 1049): Метаданные
                /// TrueOption = 1
                ///</summary>
                public const string ismetadata = "ismetadata";

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
                ///     (Russian - 1049): Дата изменения
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
                ///     (Russian - 1049): Изменено (представитель)
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
                ///     (English - United States - 1033): Object
                ///     (Russian - 1049): Объект
                /// 
                /// Description:
                ///     (English - United States - 1033): Object Id
                ///     (Russian - 1049): Идентификатор объекта
                /// 
                /// SchemaName: ObjectId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string objectid = "objectid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created On
                ///     (Russian - 1049): Когда создано
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the record was created.
                ///     (Russian - 1049): Дата и время, когда была создана запись.
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
                ///     (English - United States - 1033): Root App Module Component
                ///     (Russian - 1049): Корневой компонент модуля приложения
                /// 
                /// Description:
                ///     (English - United States - 1033): The parent ID of the subcomponent, which will be a root
                ///     (Russian - 1049): Родительский идентификатор подкомпонента, который будет корневым
                /// 
                /// SchemaName: RootAppModuleComponentId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string rootappmodulecomponentid = "rootappmodulecomponentid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Root Component Behavior
                ///     (Russian - 1049): Поведение корневого компонента
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates the include behavior of the root component.
                ///     (Russian - 1049): Указывает на поведение включения корневого компонента.
                /// 
                /// SchemaName: RootComponentBehavior
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet appmodulecomponent_rootcomponentbehavior
                /// DefaultFormValue = -1
                ///</summary>
                public const string rootcomponentbehavior = "rootcomponentbehavior";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Time Zone Rule Version Number
                ///     (Russian - 1049): Номер версии правила часового пояса
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: TimeZoneRuleVersionNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -1    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string timezoneruleversionnumber = "timezoneruleversionnumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): UTC Conversion Time Zone Code
                ///     (Russian - 1049): Идентификатор часового пояса (преобразование в UTC)
                /// 
                /// Description:
                ///     (English - United States - 1033): Time zone code that was in use when the record was created.
                ///     (Russian - 1049): Идентификатор часового пояса, использовавшийся при создании записи.
                /// 
                /// SchemaName: UTCConversionTimeZoneCode
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -1    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string utcconversiontimezonecode = "utcconversiontimezonecode";

                ///<summary>
                /// SchemaName: VersionNumber
                /// BigIntAttributeMetadata    AttributeType: BigInt    AttributeTypeName: BigIntType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -9223372036854775808    MaxValue = 9223372036854775807
                ///</summary>
                public const string versionnumber = "versionnumber";
            }

            #endregion Attributes.

            #region OptionSets.

            public static partial class OptionSets
            {

                #region Picklist OptionSet OptionSets.
                ///<summary>
                /// Attribute: componenttype
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Object Type Code
                ///     (Russian - 1049): Код типа объекта
                /// 
                /// Description:
                ///     (English - United States - 1033): The object type code of the component.
                ///     (Russian - 1049): Код типа объекта компонента.
                /// 
                /// Local System  OptionSet appmodulecomponent_componenttype
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Component Type
                ///     (Russian - 1049): Тип компонента
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates component type
                ///     (Russian - 1049): Показывает тип компонента
                ///</summary>
                public enum componenttype
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Entities
                    ///     (Russian - 1049): Сущности
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Entities_1 = 1,

                    ///<summary>
                    /// 26
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Views
                    ///     (Russian - 1049): Представления
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Views_26 = 26,

                    ///<summary>
                    /// 29
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Business Process Flows
                    ///     (Russian - 1049): Последовательности операций бизнес-процесса
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Business_Process_Flows_29 = 29,

                    ///<summary>
                    /// 48
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Command (Ribbon) for Forms, Grids, sub grids
                    ///     (Russian - 1049): Команда (лента) для форм, сеток и вложенных сеток
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Command_Ribbon_for_Forms_Grids_sub_grids_48 = 48,

                    ///<summary>
                    /// 59
                    /// DisplayOrder: 5
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Charts
                    ///     (Russian - 1049): Диаграммы
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Charts_59 = 59,

                    ///<summary>
                    /// 60
                    /// DisplayOrder: 6
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Forms
                    ///     (Russian - 1049): Формы
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Forms_60 = 60,

                    ///<summary>
                    /// 62
                    /// DisplayOrder: 7
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Sitemap
                    ///     (Russian - 1049): Карта сайта
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Sitemap_62 = 62,
                }

                ///<summary>
                /// Attribute: rootcomponentbehavior
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Root Component Behavior
                ///     (Russian - 1049): Поведение корневого компонента
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates the include behavior of the root component.
                ///     (Russian - 1049): Указывает на поведение включения корневого компонента.
                /// 
                /// Local System  OptionSet appmodulecomponent_rootcomponentbehavior
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Include Behavior
                ///     (Russian - 1049): Поведение включения
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates the include behavior of the root component.
                ///     (Russian - 1049): Указывает на поведение включения корневого компонента.
                ///</summary>
                public enum rootcomponentbehavior
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Include Subcomponents
                    ///     (Russian - 1049): Включить подкомпоненты
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Include_Subcomponents_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Do not include subcomponents
                    ///     (Russian - 1049): Не включать подкомпоненты
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Do_not_include_subcomponents_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Include As Shell Only
                    ///     (Russian - 1049): Включить только как оболочку
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Include_As_Shell_Only_2 = 2,
                }

                #endregion Picklist OptionSets.
            }

            #endregion OptionSets.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship appmodule_appmodulecomponent
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
                /// ReferencedEntity appmodule:
                ///     DisplayName:
                ///     (English - United States - 1033): App
                ///     (Russian - 1049): Приложение
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Apps
                ///     (Russian - 1049): Приложения
                ///     
                ///     Description:
                ///     (English - United States - 1033): To provide specific CRM UI context .For internal use only
                ///     (Russian - 1049): Для определения конкретного контекста пользовательского интерфейса CRM. Только для внутреннего использования
                ///</summary>
                public static partial class appmodule_appmodulecomponent
                {
                    public const string Name = "appmodule_appmodulecomponent";

                    public const string ReferencedEntity_appmodule = "appmodule";

                    public const string ReferencedAttribute_appmoduleidunique = "appmoduleidunique";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_appmodulecomponent = "appmodulecomponent";

                    public const string ReferencingAttribute_appmoduleidunique = "appmoduleidunique";
                }

                ///<summary>
                /// N:1 - Relationship lk_appmodulecomponent_createdby
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     appmodulecomponent_createdby
                /// ReferencingEntityNavigationPropertyName    appmodulecomponent_createdby
                /// IsCustomizable                             True                            False
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
                public static partial class lk_appmodulecomponent_createdby
                {
                    public const string Name = "lk_appmodulecomponent_createdby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_appmodulecomponent = "appmodulecomponent";

                    public const string ReferencingAttribute_createdby = "createdby";
                }

                ///<summary>
                /// N:1 - Relationship lk_appmodulecomponent_createdonbehalfby
                /// 
                /// PropertyName                               Value                                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_appmodulecomponent_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    createdonbehalfby
                /// IsCustomizable                             False                                      False
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
                public static partial class lk_appmodulecomponent_createdonbehalfby
                {
                    public const string Name = "lk_appmodulecomponent_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_appmodulecomponent = "appmodulecomponent";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_appmodulecomponent_modifiedby
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     appmodulecomponent_modifiedby
                /// ReferencingEntityNavigationPropertyName    appmodulecomponent_modifiedby
                /// IsCustomizable                             True                             False
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
                public static partial class lk_appmodulecomponent_modifiedby
                {
                    public const string Name = "lk_appmodulecomponent_modifiedby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_appmodulecomponent = "appmodulecomponent";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";
                }

                ///<summary>
                /// N:1 - Relationship lk_appmodulecomponent_modifiedonbehalfby
                /// 
                /// PropertyName                               Value                                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_appmodulecomponent_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    modifiedonbehalfby
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
                public static partial class lk_appmodulecomponent_modifiedonbehalfby
                {
                    public const string Name = "lk_appmodulecomponent_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_appmodulecomponent = "appmodulecomponent";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }
            }

            #endregion Relationship ManyToOne - N:1.
        }
    }
}
