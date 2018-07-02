
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): Security Role
    /// (Russian - 1049): Роль безопасности
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): Security Roles
    /// (Russian - 1049): Роли безопасности
    /// 
    /// Description:
    /// (English - United States - 1033): Grouping of security privileges. Users are assigned roles that authorize their access to the Microsoft CRM system.
    /// (Russian - 1049): Группа привилегий безопасности. Пользователям назначаются роли, которые контролируют их доступ к CRM-системе Microsoft.
    /// 
    /// PropertyName                          Value            CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False            False
    /// CanBePrimaryEntityInRelationship      True             False
    /// CanBeRelatedEntityInRelationship      False            False
    /// CanChangeHierarchicalRelationship     False            False
    /// CanChangeTrackingBeEnabled            True             True
    /// CanCreateAttributes                   False            False
    /// CanCreateCharts                       False            False
    /// CanCreateForms                        False            False
    /// CanCreateViews                        False            False
    /// CanEnableSyncToExternalSearchIndex    True             True
    /// CanModifyAdditionalSettings           True             True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 True
    /// CollectionSchemaName                  Roles
    /// DaysSinceRecordLastModified           9999
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         roles
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False            True
    /// IsAvailableOffline                    True
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False            False
    /// IsCustomEntity                        False
    /// IsCustomizable                        True             False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False            False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          True
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False            False
    /// IsMappable                            True             False
    /// IsOfflineInMobileClient               False            True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False            False
    /// IsRenameable                          False            False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                True
    /// IsValidForQueue                       False            False
    /// IsVisibleInMobile                     False            False
    /// IsVisibleInMobileClient               False            False
    /// LogicalCollectionName                 roles
    /// LogicalName                           role
    /// ObjectTypeCode                        1036
    /// OwnershipType                         BusinessOwned
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredRole
    /// SchemaName                            Role
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class Role
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "role";

            public const string EntitySchemaName = "Role";

            public const string EntityPrimaryNameAttribute = "name";

            public const string EntityPrimaryIdAttribute = "roleid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Business Unit
                ///     (Russian - 1049): Подразделение
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the business unit with which the role is associated.
                ///     (Russian - 1049): Уникальный идентификатор подразделения, с которым связана эта роль.
                /// 
                /// SchemaName: BusinessUnitId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: businessunit
                /// 
                ///     Target businessunit    PrimaryIdAttribute businessunitid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Business Unit
                ///         (Russian - 1049): Подразделение
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Business Units
                ///         (Russian - 1049): Подразделения
                ///         
                ///         Description:
                ///         (English - United States - 1033): Business, division, or department in the Microsoft Dynamics 365 database.
                ///         (Russian - 1049): Компания, подразделение или отдел в базе данных Microsoft Dynamics 365.
                ///</summary>
                public const string businessunitid = "businessunitid";

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
                ///     (English - United States - 1033): Unique identifier of the user who created the role.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, создавшего роль.
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
                ///     (English - United States - 1033): Date and time when the role was created.
                ///     (Russian - 1049): Дата и время создания роли.
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
                ///     (English - United States - 1033): Created By Impersonator
                ///     (Russian - 1049): Создано персонатором
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the delegate user who created the role.
                ///     (Russian - 1049): Уникальный идентификатор пользователя-делегата, создавшего роль.
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
                ///     (English - United States - 1033): Import Sequence Number
                ///     (Russian - 1049): Порядковый номер импорта
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the data import or data migration that created this record.
                ///     (Russian - 1049): Уникальный идентификатор импорта или переноса данных, создавшего эту запись.
                /// 
                /// SchemaName: ImportSequenceNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string importsequencenumber = "importsequencenumber";

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
                ///     (English - United States - 1033): State
                ///     (Russian - 1049): Область
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the solution component is part of a managed solution.
                ///     (Russian - 1049): Указывает, является ли компонент решения частью управляемого решения.
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
                ///     (English - United States - 1033): Modified By
                ///     (Russian - 1049): Изменено
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who last modified the role.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, внесшего последнее изменение в роль.
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
                ///     (English - United States - 1033): Date and time when the role was last modified.
                ///     (Russian - 1049): Дата и время последнего изменения роли.
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who last modified the role.
                ///     (Russian - 1049): Уникальный идентификатор делегата, внесшего последнее изменение в роль.
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
                ///     (Russian - 1049): Имя
                /// 
                /// Description:
                ///     (English - United States - 1033): Name of the role.
                ///     (Russian - 1049): Имя роли.
                /// 
                /// SchemaName: Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string name = "name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Organization
                ///     (Russian - 1049): Предприятие
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the organization associated with the role.
                ///     (Russian - 1049): Уникальный идентификатор организации, связанной с ролью.
                /// 
                /// SchemaName: OrganizationId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string organizationid = "organizationid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Record Created On
                ///     (Russian - 1049): Дата создания записи
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time that the record was migrated.
                ///     (Russian - 1049): Дата и время переноса записи.
                /// 
                /// SchemaName: OverriddenCreatedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateOnly
                ///</summary>
                public const string overriddencreatedon = "overriddencreatedon";

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
                ///     (English - United States - 1033): Parent Role
                ///     (Russian - 1049): Родительская роль
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the parent role.
                ///     (Russian - 1049): Уникальный идентификатор родительской роли.
                /// 
                /// SchemaName: ParentRoleId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: role
                /// 
                ///     Target role    PrimaryIdAttribute roleid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Security Role
                ///         (Russian - 1049): Роль безопасности
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Security Roles
                ///         (Russian - 1049): Роли безопасности
                ///         
                ///         Description:
                ///         (English - United States - 1033): Grouping of security privileges. Users are assigned roles that authorize their access to the Microsoft CRM system.
                ///         (Russian - 1049): Группа привилегий безопасности. Пользователям назначаются роли, которые контролируют их доступ к CRM-системе Microsoft.
                ///</summary>
                public const string parentroleid = "parentroleid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Parent Root Role
                ///     (Russian - 1049): Корневая родительская роль
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the parent root role.
                ///     (Russian - 1049): Уникальный идентификатор корневой родительской роли.
                /// 
                /// SchemaName: ParentRootRoleId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: role
                /// 
                ///     Target role    PrimaryIdAttribute roleid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Security Role
                ///         (Russian - 1049): Роль безопасности
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Security Roles
                ///         (Russian - 1049): Роли безопасности
                ///         
                ///         Description:
                ///         (English - United States - 1033): Grouping of security privileges. Users are assigned roles that authorize their access to the Microsoft CRM system.
                ///         (Russian - 1049): Группа привилегий безопасности. Пользователям назначаются роли, которые контролируют их доступ к CRM-системе Microsoft.
                ///</summary>
                public const string parentrootroleid = "parentrootroleid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Role
                ///     (Russian - 1049): Роль
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the role.
                ///     (Russian - 1049): Уникальный идентификатор роли.
                /// 
                /// SchemaName: RoleId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string roleid = "roleid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Unique Id
                ///     (Russian - 1049): Уникальный идентификатор
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: RoleIdUnique
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string roleidunique = "roleidunique";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Role Template
                ///     (Russian - 1049): Шаблон роли
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the role template that is associated with the role.
                ///     (Russian - 1049): Уникальный идентификатор шаблона роли, связанного с ролью.
                /// 
                /// SchemaName: RoleTemplateId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: roletemplate
                /// 
                ///     Target roletemplate    PrimaryIdAttribute roletemplateid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Role Template
                ///         (Russian - 1049): Шаблон роли
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Role Templates
                ///         (Russian - 1049): Шаблоны ролей
                ///         
                ///         Description:
                ///         (English - United States - 1033): Template for a role. Defines initial attributes that will be used when creating a new role.
                ///         (Russian - 1049): Шаблон роли. Определяет исходные атрибуты, которые будут использоваться при создании новой роли.
                ///</summary>
                public const string roletemplateid = "roletemplateid";

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
                ///     (English - United States - 1033): Version number
                ///     (Russian - 1049): Номер версии
                /// 
                /// Description:
                ///     (English - United States - 1033): Version number of the role.
                ///     (Russian - 1049): Номер версии роли.
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

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship business_unit_roles
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_roles
                /// ReferencingEntityNavigationPropertyName    businessunitid
                /// IsCustomizable                             False                    False
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
                /// ReferencedEntity businessunit:
                ///     DisplayName:
                ///     (English - United States - 1033): Business Unit
                ///     (Russian - 1049): Подразделение
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Business Units
                ///     (Russian - 1049): Подразделения
                ///     
                ///     Description:
                ///     (English - United States - 1033): Business, division, or department in the Microsoft Dynamics 365 database.
                ///     (Russian - 1049): Компания, подразделение или отдел в базе данных Microsoft Dynamics 365.
                /// 
                /// AttributeMaps:
                ///     SourceEntity             TargetEntity
                ///     businessunit       ->    role
                ///     
                ///     SourceAttribute          TargetAttribute
                ///     businessunitid     ->    businessunitid
                ///     name               ->    businessunitidname
                ///</summary>
                public static partial class business_unit_roles
                {
                    public const string Name = "business_unit_roles";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_role = "role";

                    public const string ReferencingAttribute_businessunitid = "businessunitid";
                }

                ///<summary>
                /// N:1 - Relationship lk_role_createdonbehalfby
                /// 
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_role_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    createdonbehalfby
                /// IsCustomizable                             False                        False
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
                public static partial class lk_role_createdonbehalfby
                {
                    public const string Name = "lk_role_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_role = "role";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_role_modifiedonbehalfby
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_role_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    modifiedonbehalfby
                /// IsCustomizable                             False                         False
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
                public static partial class lk_role_modifiedonbehalfby
                {
                    public const string Name = "lk_role_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_role = "role";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_rolebase_createdby
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_rolebase_createdby
                /// ReferencingEntityNavigationPropertyName    createdby
                /// IsCustomizable                             False                    False
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
                public static partial class lk_rolebase_createdby
                {
                    public const string Name = "lk_rolebase_createdby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_role = "role";

                    public const string ReferencingAttribute_createdby = "createdby";
                }

                ///<summary>
                /// N:1 - Relationship lk_rolebase_modifiedby
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_rolebase_modifiedby
                /// ReferencingEntityNavigationPropertyName    modifiedby
                /// IsCustomizable                             False                     False
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
                public static partial class lk_rolebase_modifiedby
                {
                    public const string Name = "lk_rolebase_modifiedby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_role = "role";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";
                }

                ///<summary>
                /// N:1 - Relationship organization_roles
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_roles
                /// ReferencingEntityNavigationPropertyName    organizationid_organization
                /// IsCustomizable                             False                          False
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
                public static partial class organization_roles
                {
                    public const string Name = "organization_roles";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_role = "role";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// N:1 - Relationship role_parent_role
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     role_parent_role
                /// ReferencingEntityNavigationPropertyName    parentroleid
                /// IsCustomizable                             False                    False
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
                ///</summary>
                public static partial class role_parent_role
                {
                    public const string Name = "role_parent_role";

                    public const string ReferencedEntity_role = "role";

                    public const string ReferencedAttribute_roleid = "roleid";

                    public const string ReferencingEntity_role = "role";

                    public const string ReferencingAttribute_parentroleid = "parentroleid";
                }

                ///<summary>
                /// N:1 - Relationship role_parent_root_role
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     role_parent_root_role
                /// ReferencingEntityNavigationPropertyName    parentrootroleid
                /// IsCustomizable                             False                    False
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
                ///</summary>
                public static partial class role_parent_root_role
                {
                    public const string Name = "role_parent_root_role";

                    public const string ReferencedEntity_role = "role";

                    public const string ReferencedAttribute_roleid = "roleid";

                    public const string ReferencingEntity_role = "role";

                    public const string ReferencingAttribute_parentrootroleid = "parentrootroleid";
                }

                ///<summary>
                /// N:1 - Relationship role_template_roles
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     role_template_roles
                /// ReferencingEntityNavigationPropertyName    roletemplateid
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
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
                /// ReferencedEntity roletemplate:
                ///     DisplayName:
                ///     (English - United States - 1033): Role Template
                ///     (Russian - 1049): Шаблон роли
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Role Templates
                ///     (Russian - 1049): Шаблоны ролей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Template for a role. Defines initial attributes that will be used when creating a new role.
                ///     (Russian - 1049): Шаблон роли. Определяет исходные атрибуты, которые будут использоваться при создании новой роли.
                ///</summary>
                public static partial class role_template_roles
                {
                    public const string Name = "role_template_roles";

                    public const string ReferencedEntity_roletemplate = "roletemplate";

                    public const string ReferencedAttribute_roletemplateid = "roletemplateid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_role = "role";

                    public const string ReferencingAttribute_roletemplateid = "roletemplateid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship Role_AsyncOperations
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Role_AsyncOperations
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_role
                /// IsCustomizable                             False                     False
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
                public static partial class role_asyncoperations
                {
                    public const string Name = "Role_AsyncOperations";

                    public const string ReferencedEntity_role = "role";

                    public const string ReferencedAttribute_roleid = "roleid";

                    public const string ReferencingEntity_asyncoperation = "asyncoperation";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship Role_BulkDeleteFailures
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Role_BulkDeleteFailures
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_role
                /// IsCustomizable                             False                      False
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
                /// ReferencingEntity bulkdeletefailure:
                ///     DisplayName:
                ///     (English - United States - 1033): Bulk Delete Failure
                ///     (Russian - 1049): Ошибка группового удаления
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Bulk Delete Failures
                ///     (Russian - 1049): Ошибки группового удаления
                ///     
                ///     Description:
                ///     (English - United States - 1033): Record that was not deleted during a bulk deletion job.
                ///     (Russian - 1049): Запись не была удалена во время задания группового удаления.
                ///</summary>
                public static partial class role_bulkdeletefailures
                {
                    public const string Name = "Role_BulkDeleteFailures";

                    public const string ReferencedEntity_role = "role";

                    public const string ReferencedAttribute_roleid = "roleid";

                    public const string ReferencingEntity_bulkdeletefailure = "bulkdeletefailure";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";
                }

                ///<summary>
                /// 1:N - Relationship role_parent_role
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     role_parent_role
                /// ReferencingEntityNavigationPropertyName    parentroleid
                /// IsCustomizable                             False                    False
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
                ///</summary>
                public static partial class role_parent_role
                {
                    public const string Name = "role_parent_role";

                    public const string ReferencedEntity_role = "role";

                    public const string ReferencedAttribute_roleid = "roleid";

                    public const string ReferencingEntity_role = "role";

                    public const string ReferencingAttribute_parentroleid = "parentroleid";
                }

                ///<summary>
                /// 1:N - Relationship role_parent_root_role
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     role_parent_root_role
                /// ReferencingEntityNavigationPropertyName    parentrootroleid
                /// IsCustomizable                             False                    False
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
                ///</summary>
                public static partial class role_parent_root_role
                {
                    public const string Name = "role_parent_root_role";

                    public const string ReferencedEntity_role = "role";

                    public const string ReferencedAttribute_roleid = "roleid";

                    public const string ReferencingEntity_role = "role";

                    public const string ReferencingAttribute_parentrootroleid = "parentrootroleid";
                }

                ///<summary>
                /// 1:N - Relationship Role_SyncErrors
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Role_SyncErrors
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_role_syncerror
                /// IsCustomizable                             True                                False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 Cascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity syncerror:
                ///     DisplayName:
                ///     (English - United States - 1033): Sync Error
                ///     (Russian - 1049): Ошибка синхронизации
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Sync Errors
                ///     (Russian - 1049): Ошибки синхронизации
                ///     
                ///     Description:
                ///     (English - United States - 1033): Failure reason and other detailed information for a record that failed to sync.
                ///     (Russian - 1049): Причина сбоя и другие подробные сведения о записи, при синхронизации которой произошла ошибка.
                ///</summary>
                public static partial class role_syncerrors
                {
                    public const string Name = "Role_SyncErrors";

                    public const string ReferencedEntity_role = "role";

                    public const string ReferencedAttribute_roleid = "roleid";

                    public const string ReferencingEntity_syncerror = "syncerror";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_role
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_role
                /// ReferencingEntityNavigationPropertyName    objectid_role
                /// IsCustomizable                             False                          False
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
                public static partial class userentityinstancedata_role
                {
                    public const string Name = "userentityinstancedata_role";

                    public const string ReferencedEntity_role = "role";

                    public const string ReferencedAttribute_roleid = "roleid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
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
                /// Entity1LogicalName appmodule:
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
                public static partial class appmoduleroles_association
                {
                    public const string Name = "appmoduleroles_association";

                    public const string IntersectEntity_appmoduleroles = "appmoduleroles";

                    public const string Entity1_appmodule = "appmodule";

                    public const string Entity1Attribute_appmoduleid = "appmoduleid";

                    public const string Entity1LogicalName_PrimaryNameAttribute_name = "name";

                    public const string Entity2_role = "role";

                    public const string Entity2Attribute_roleid = "roleid";
                }

                ///<summary>
                /// N:N - Relationship roleprivileges_association
                /// 
                /// PropertyName                                   Value                         CanBeChanged
                /// Entity1NavigationPropertyName                  roleprivileges_association
                /// Entity2NavigationPropertyName                  roleprivileges_association
                /// IsCustomizable                                 False                         False
                /// IsCustomRelationship                           False
                /// IsValidForAdvancedFind                         False
                /// RelationshipType                               ManyToManyRelationship
                /// SecurityTypes                                  None
                /// Entity1AssociatedMenuConfiguration.Behavior    DoNotDisplay
                /// Entity1AssociatedMenuConfiguration.Group       Details
                /// Entity1AssociatedMenuConfiguration.Order       null
                /// Entity2AssociatedMenuConfiguration.Behavior    DoNotDisplay
                /// Entity2AssociatedMenuConfiguration.Group       Details
                /// Entity2AssociatedMenuConfiguration.Order       null
                /// 
                /// Entity1LogicalName privilege:
                ///     DisplayName:
                ///     (English - United States - 1033): Privilege
                ///     (Russian - 1049): Привилегия
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Privileges
                ///     (Russian - 1049): Права
                ///     
                ///     Description:
                ///     (English - United States - 1033): Permission to perform an action in Microsoft CRM. The platform checks for the privilege and rejects the attempt if the user does not hold the privilege.
                ///     (Russian - 1049): Разрешение на выполнение действия в Microsoft CRM. Платформа проверяет наличие привилегии и запрещает попытку, если у пользователя нет требуемой привилегии.
                ///</summary>
                public static partial class roleprivileges_association
                {
                    public const string Name = "roleprivileges_association";

                    public const string IntersectEntity_roleprivileges = "roleprivileges";

                    public const string Entity1_privilege = "privilege";

                    public const string Entity1Attribute_privilegeid = "privilegeid";

                    public const string Entity1LogicalName_PrimaryNameAttribute_name = "name";

                    public const string Entity2_role = "role";

                    public const string Entity2Attribute_roleid = "roleid";
                }

                ///<summary>
                /// N:N - Relationship systemuserroles_association
                /// 
                /// PropertyName                                   Value                          CanBeChanged
                /// Entity1NavigationPropertyName                  systemuserroles_association
                /// Entity2NavigationPropertyName                  systemuserroles_association
                /// IsCustomizable                                 False                          False
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
                /// Entity1LogicalName systemuser:
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
                public static partial class systemuserroles_association
                {
                    public const string Name = "systemuserroles_association";

                    public const string IntersectEntity_systemuserroles = "systemuserroles";

                    public const string Entity1_systemuser = "systemuser";

                    public const string Entity1Attribute_systemuserid = "systemuserid";

                    public const string Entity1LogicalName_PrimaryNameAttribute_fullname = "fullname";

                    public const string Entity2_role = "role";

                    public const string Entity2Attribute_roleid = "roleid";
                }

                ///<summary>
                /// N:N - Relationship teamroles_association
                /// 
                /// PropertyName                                   Value                     CanBeChanged
                /// Entity1NavigationPropertyName                  teamroles_association
                /// Entity2NavigationPropertyName                  teamroles_association
                /// IsCustomizable                                 False                     False
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
                /// Entity1LogicalName team:
                ///     DisplayName:
                ///     (English - United States - 1033): Team
                ///     (Russian - 1049): Рабочая группа
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Teams
                ///     (Russian - 1049): Рабочие группы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Collection of system users that routinely collaborate. Teams can be used to simplify record sharing and provide team members with common access to organization data when team members belong to different Business Units.
                ///     (Russian - 1049): Набор системных пользователей, которые обычно участвуют в совместной работе. Рабочие группы можно использовать для упрощения предоставления участникам группы общего доступа к записям и данным организации, если участники принадлежат к разным подразделениям.
                ///</summary>
                public static partial class teamroles_association
                {
                    public const string Name = "teamroles_association";

                    public const string IntersectEntity_teamroles = "teamroles";

                    public const string Entity1_team = "team";

                    public const string Entity1Attribute_teamid = "teamid";

                    public const string Entity1LogicalName_PrimaryNameAttribute_name = "name";

                    public const string Entity2_role = "role";

                    public const string Entity2Attribute_roleid = "roleid";
                }
            }

            #endregion Relationship ManyToMany - N:N.
        }
    }
}
