
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): Solution
    /// (Russian - 1049): Решение
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): Solutions
    /// (Russian - 1049): Решения
    /// 
    /// Description:
    /// (English - United States - 1033): A solution which contains CRM customizations.
    /// (Russian - 1049): Решение, содержащее настройки CRM.
    /// 
    /// PropertyName                          Value                CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False                False
    /// CanBePrimaryEntityInRelationship      False                False
    /// CanBeRelatedEntityInRelationship      False                False
    /// CanChangeHierarchicalRelationship     False                False
    /// CanChangeTrackingBeEnabled            True                 True
    /// CanCreateAttributes                   False                False
    /// CanCreateCharts                       False                False
    /// CanCreateForms                        False                False
    /// CanCreateViews                        False                False
    /// CanEnableSyncToExternalSearchIndex    False                False
    /// CanModifyAdditionalSettings           True                 True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 True
    /// CollectionSchemaName                  Solutions
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         solutions
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False                True
    /// IsAvailableOffline                    True
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False                False
    /// IsCustomEntity                        False
    /// IsCustomizable                        True                 False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                False
    /// IsMappable                            False                False
    /// IsOfflineInMobileClient               False                True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False                False
    /// IsRenameable                          False                False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False                False
    /// IsVisibleInMobile                     False                False
    /// IsVisibleInMobileClient               False                False
    /// LogicalCollectionName                 solutions
    /// LogicalName                           solution
    /// ObjectTypeCode                        7100
    /// OwnershipType                         OrganizationOwned
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredSolution
    /// SchemaName                            Solution
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class Solution
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "solution";

            public const string EntitySchemaName = "Solution";

            public const string EntityPrimaryNameAttribute = "friendlyname";

            public const string EntityPrimaryIdAttribute = "solutionid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Configuration Page
                ///     (Russian - 1049): Страница настройки
                /// 
                /// Description:
                ///     (English - United States - 1033): A link to an optional configuration page for this solution.
                ///     (Russian - 1049): Ссылка на дополнительную страницу настройки для данного решения.
                /// 
                /// SchemaName: ConfigurationPageId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: webresource
                /// 
                ///     Target webresource    PrimaryIdAttribute webresourceid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Web Resource
                ///         (Russian - 1049): Веб-ресурс
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Web Resources
                ///         (Russian - 1049): Веб-ресурсы
                ///         
                ///         Description:
                ///         (English - United States - 1033): Data equivalent to files used in Web development. Web resources provide client-side components that are used to provide custom user interface elements.
                ///         (Russian - 1049): Данные, эквивалентные файлам, используемым в разработке веб-контента. Веб-ресурсы обеспечивают компоненты на стороне клиента, которые используются для создания настраиваемых элементов интерфейса пользователя.
                ///</summary>
                public const string configurationpageid = "configurationpageid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By
                ///     (Russian - 1049): Создано
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who created the solution.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, создавшего решение.
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
                ///     (English - United States - 1033): Date and time when the solution was created.
                ///     (Russian - 1049): Дата и время создания решения.
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who created the solution.
                ///     (Russian - 1049): Уникальный идентификатор пользователя-делегата, создавшего решение.
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
                ///     (English - United States - 1033): Description
                ///     (Russian - 1049): Описание
                /// 
                /// Description:
                ///     (English - United States - 1033): Description of the solution.
                ///     (Russian - 1049): Описание решения.
                /// 
                /// SchemaName: Description
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 2000
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = True
                ///</summary>
                public const string description = "description";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Display Name
                ///     (Russian - 1049): Отображаемое имя
                /// 
                /// Description:
                ///     (English - United States - 1033): User display name for the solution.
                ///     (Russian - 1049): Понятное имя решения.
                /// 
                /// SchemaName: FriendlyName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = True
                ///</summary>
                public const string friendlyname = "friendlyname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Installed On
                ///     (Russian - 1049): Дата установки
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the solution was installed/upgraded.
                ///     (Russian - 1049): Дата и время установки или обновления решения.
                /// 
                /// SchemaName: InstalledOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Auto    Format = DateOnly
                ///</summary>
                public const string installedon = "installedon";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is internal solution
                ///     (Russian - 1049): Является внутренним решением
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the solution is internal or not.
                ///     (Russian - 1049): Указывает, является ли решение внутренним.
                /// 
                /// SchemaName: IsInternal
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: False    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string isinternal = "isinternal";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Package Type
                ///     (Russian - 1049): Тип пакета
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the solution is managed or unmanaged.
                ///     (Russian - 1049): Указывает, является ли решение управляемым или неуправляемым.
                /// 
                /// SchemaName: IsManaged
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Unmanaged
                ///     (Russian - 1049): Неуправляемое
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Managed
                ///     (Russian - 1049): Управляемое
                /// TrueOption = 1
                ///</summary>
                public const string ismanaged = "ismanaged";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Visible Outside Platform
                ///     (Russian - 1049): Видимо за пределами платформы
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the solution is visible outside of the platform.
                ///     (Russian - 1049): Указывает, видимо ли решение за пределами платформы.
                /// 
                /// SchemaName: IsVisible
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = True
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
                public const string isvisible = "isvisible";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By
                ///     (Russian - 1049): Изменено
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who last modified the solution.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, внесшего последнее изменение в решение.
                /// 
                /// SchemaName: ModifiedBy
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
                public const string modifiedby = "modifiedby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified On
                ///     (Russian - 1049): Дата изменения
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the solution was last modified.
                ///     (Russian - 1049): Дата и время последнего изменения решения.
                /// 
                /// SchemaName: ModifiedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who modified the solution.
                ///     (Russian - 1049): Уникальный идентификатор делегата, изменившего решение.
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
                ///     (English - United States - 1033): Organization
                ///     (Russian - 1049): Предприятие
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the organization associated with the solution.
                ///     (Russian - 1049): Уникальный идентификатор организации, связанной с решением.
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
                ///     (English - United States - 1033): Parent Solution
                ///     (Russian - 1049): Родительское решение
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the parent solution. Should only be non-null if this solution is a patch.
                ///     (Russian - 1049): Уникальный идентификатор родительского решения. Должен быть непустым только в случае, если это решение представляет собой исправление.
                /// 
                /// SchemaName: ParentSolutionId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: solution
                /// 
                ///     Target solution    PrimaryIdAttribute solutionid    PrimaryNameAttribute friendlyname
                ///         DisplayName:
                ///         (English - United States - 1033): Solution
                ///         (Russian - 1049): Решение
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Solutions
                ///         (Russian - 1049): Решения
                ///         
                ///         Description:
                ///         (English - United States - 1033): A solution which contains CRM customizations.
                ///         (Russian - 1049): Решение, содержащее настройки CRM.
                ///</summary>
                public const string parentsolutionid = "parentsolutionid";

                ///<summary>
                /// SchemaName: PinpointAssetId
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 255
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string pinpointassetid = "pinpointassetid";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Identifier of the publisher of this solution in Microsoft Pinpoint.
                ///     (Russian - 1049): Идентификатор издателя данного решения в Microsoft Pinpoint.
                /// 
                /// SchemaName: PinpointPublisherId
                /// BigIntAttributeMetadata    AttributeType: BigInt    AttributeTypeName: BigIntType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -9223372036854775808    MaxValue = 9223372036854775807
                ///</summary>
                public const string pinpointpublisherid = "pinpointpublisherid";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Default locale of the solution in Microsoft Pinpoint.
                ///     (Russian - 1049): Локаль по умолчанию решения Microsoft Pinpoint.
                /// 
                /// SchemaName: PinpointSolutionDefaultLocale
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 16
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string pinpointsolutiondefaultlocale = "pinpointsolutiondefaultlocale";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Identifier of the solution in Microsoft Pinpoint.
                ///     (Russian - 1049): Идентификатор решения в Microsoft Pinpoint.
                /// 
                /// SchemaName: PinpointSolutionId
                /// BigIntAttributeMetadata    AttributeType: BigInt    AttributeTypeName: BigIntType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -9223372036854775808    MaxValue = 9223372036854775807
                ///</summary>
                public const string pinpointsolutionid = "pinpointsolutionid";

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
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
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
                ///     (English - United States - 1033): Solution Identifier
                ///     (Russian - 1049): Идентификатор решения
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the solution.
                ///     (Russian - 1049): Уникальный идентификатор решения.
                /// 
                /// SchemaName: SolutionId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string solutionid = "solutionid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Solution Package Version
                ///     (Russian - 1049): Версия пакета решения
                /// 
                /// Description:
                ///     (English - United States - 1033): Solution package source organization version
                ///     (Russian - 1049): Версия организации источника пакета решения
                /// 
                /// SchemaName: SolutionPackageVersion
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = VersionNumber    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string solutionpackageversion = "solutionpackageversion";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Solution Type
                ///     (Russian - 1049): Тип решения
                /// 
                /// Description:
                /// 
                /// SchemaName: SolutionType
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet solution_solutiontype
                /// DefaultFormValue = 0
                ///</summary>
                public const string solutiontype = "solutiontype";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Name
                ///     (Russian - 1049): Имя
                /// 
                /// Description:
                ///     (English - United States - 1033): The unique name of this solution
                ///     (Russian - 1049): Уникальное имя этого решения
                /// 
                /// SchemaName: UniqueName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 65
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string uniquename = "uniquename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Version
                ///     (Russian - 1049): Версия
                /// 
                /// Description:
                ///     (English - United States - 1033): Solution version, used to identify a solution for upgrades and hotfixes.
                ///     (Russian - 1049): Версия решения, которая используется для идентификации решения для обновлений и исправлений.
                /// 
                /// SchemaName: Version
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
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
            }

            #endregion Attributes.

            #region OptionSets.

            public static partial class OptionSets
            {

                #region Picklist OptionSet OptionSets.
                ///<summary>
                /// Attribute: solutiontype
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Solution Type
                ///     (Russian - 1049): Тип решения
                /// 
                /// Description:
                /// 
                /// Local System  OptionSet solution_solutiontype
                /// 
                /// Description:
                ///     (English - United States - 1033): All possible types of solution
                ///     (Russian - 1049): Все возможные типы решений
                ///</summary>
                public enum solutiontype
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): None
                    ///     (Russian - 1049): Нет
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    None_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Snapshot
                    ///     (Russian - 1049): Снимок
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Snapshot_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Internal
                    ///     (Russian - 1049): Внутр.
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Internal_2 = 2,
                }

                #endregion Picklist OptionSets.
            }

            #endregion OptionSets.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship lk_solution_createdby
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_solution_createdby
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
                public static partial class lk_solution_createdby
                {
                    public const string Name = "lk_solution_createdby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_solution = "solution";

                    public const string ReferencingAttribute_createdby = "createdby";
                }

                ///<summary>
                /// N:1 - Relationship lk_solution_modifiedby
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_solution_modifiedby
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
                public static partial class lk_solution_modifiedby
                {
                    public const string Name = "lk_solution_modifiedby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_solution = "solution";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";
                }

                ///<summary>
                /// N:1 - Relationship lk_solutionbase_createdonbehalfby
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_solutionbase_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    createdonbehalfby
                /// IsCustomizable                             False                                False
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
                public static partial class lk_solutionbase_createdonbehalfby
                {
                    public const string Name = "lk_solutionbase_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_solution = "solution";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_solutionbase_modifiedonbehalfby
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_solutionbase_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    modifiedonbehalfby
                /// IsCustomizable                             False                                 False
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
                public static partial class lk_solutionbase_modifiedonbehalfby
                {
                    public const string Name = "lk_solutionbase_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_solution = "solution";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship organization_solution
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_solution
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                public static partial class organization_solution
                {
                    public const string Name = "organization_solution";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_solution = "solution";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// N:1 - Relationship publisher_solution
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     publisher_solution
                /// ReferencingEntityNavigationPropertyName    publisherid
                /// IsCustomizable                             False                    False
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
                public static partial class publisher_solution
                {
                    public const string Name = "publisher_solution";

                    public const string ReferencedEntity_publisher = "publisher";

                    public const string ReferencedAttribute_publisherid = "publisherid";

                    public const string ReferencedEntity_PrimaryNameAttribute_friendlyname = "friendlyname";

                    public const string ReferencingEntity_solution = "solution";

                    public const string ReferencingAttribute_publisherid = "publisherid";
                }

                ///<summary>
                /// N:1 - Relationship solution_configuration_webresource
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     solution_configuration_webresource
                /// ReferencingEntityNavigationPropertyName    configurationpageid
                /// IsCustomizable                             False                                 False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                RemoveLink
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity webresource:
                ///     DisplayName:
                ///     (English - United States - 1033): Web Resource
                ///     (Russian - 1049): Веб-ресурс
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Web Resources
                ///     (Russian - 1049): Веб-ресурсы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Data equivalent to files used in Web development. Web resources provide client-side components that are used to provide custom user interface elements.
                ///     (Russian - 1049): Данные, эквивалентные файлам, используемым в разработке веб-контента. Веб-ресурсы обеспечивают компоненты на стороне клиента, которые используются для создания настраиваемых элементов интерфейса пользователя.
                ///</summary>
                public static partial class solution_configuration_webresource
                {
                    public const string Name = "solution_configuration_webresource";

                    public const string ReferencedEntity_webresource = "webresource";

                    public const string ReferencedAttribute_webresourceid = "webresourceid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_solution = "solution";

                    public const string ReferencingAttribute_configurationpageid = "configurationpageid";
                }

                ///<summary>
                /// N:1 - Relationship solution_parent_solution
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     solution_parent_solution
                /// ReferencingEntityNavigationPropertyName    parentsolutionid
                /// IsCustomizable                             False                       False
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
                ///</summary>
                public static partial class solution_parent_solution
                {
                    public const string Name = "solution_parent_solution";

                    public const string ReferencedEntity_solution = "solution";

                    public const string ReferencedAttribute_solutionid = "solutionid";

                    public const string ReferencingEntity_solution = "solution";

                    public const string ReferencingAttribute_parentsolutionid = "parentsolutionid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship solution_base_dependencynode
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     solution_base_dependencynode
                /// ReferencingEntityNavigationPropertyName    basesolutionid
                /// IsCustomizable                             False                           False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
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
                /// ReferencingEntity dependencynode:
                ///     DisplayName:
                ///     (English - United States - 1033): Dependency Node
                ///     (Russian - 1049): Узел зависимости
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Dependency Nodes
                ///     (Russian - 1049): Узлы зависимости
                ///     
                ///     Description:
                ///     (English - United States - 1033): The representation of a component dependency node in CRM.
                ///     (Russian - 1049): Представление узла зависимости компонентов в CRM.
                ///</summary>
                public static partial class solution_base_dependencynode
                {
                    public const string Name = "solution_base_dependencynode";

                    public const string ReferencedEntity_solution = "solution";

                    public const string ReferencedAttribute_solutionid = "solutionid";

                    public const string ReferencingEntity_dependencynode = "dependencynode";

                    public const string ReferencingAttribute_basesolutionid = "basesolutionid";
                }

                ///<summary>
                /// 1:N - Relationship solution_parent_solution
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     solution_parent_solution
                /// ReferencingEntityNavigationPropertyName    parentsolutionid
                /// IsCustomizable                             False                       False
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
                ///</summary>
                public static partial class solution_parent_solution
                {
                    public const string Name = "solution_parent_solution";

                    public const string ReferencedEntity_solution = "solution";

                    public const string ReferencedAttribute_solutionid = "solutionid";

                    public const string ReferencingEntity_solution = "solution";

                    public const string ReferencingAttribute_parentsolutionid = "parentsolutionid";
                }

                ///<summary>
                /// 1:N - Relationship solution_solutioncomponent
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     solution_solutioncomponent
                /// ReferencingEntityNavigationPropertyName    solutionid
                /// IsCustomizable                             False                         False
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
                /// ReferencingEntity solutioncomponent:
                ///     DisplayName:
                ///     (English - United States - 1033): Solution Component
                ///     (Russian - 1049): Компонент решения
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Solution Components
                ///     (Russian - 1049): Компоненты решения
                ///     
                ///     Description:
                ///     (English - United States - 1033): A component of a CRM solution.
                ///     (Russian - 1049): Компонент решения CRM.
                ///</summary>
                public static partial class solution_solutioncomponent
                {
                    public const string Name = "solution_solutioncomponent";

                    public const string ReferencedEntity_solution = "solution";

                    public const string ReferencedAttribute_solutionid = "solutionid";

                    public const string ReferencingEntity_solutioncomponent = "solutioncomponent";

                    public const string ReferencingAttribute_solutionid = "solutionid";
                }

                ///<summary>
                /// 1:N - Relationship Solution_SyncErrors
                /// 
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Solution_SyncErrors
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_solution_syncerror
                /// IsCustomizable                             True                                    False
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
                public static partial class solution_syncerrors
                {
                    public const string Name = "Solution_SyncErrors";

                    public const string ReferencedEntity_solution = "solution";

                    public const string ReferencedAttribute_solutionid = "solutionid";

                    public const string ReferencingEntity_syncerror = "syncerror";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship solution_top_dependencynode
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     solution_top_dependencynode
                /// ReferencingEntityNavigationPropertyName    topsolutionid
                /// IsCustomizable                             False                          False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                RemoveLink
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity dependencynode:
                ///     DisplayName:
                ///     (English - United States - 1033): Dependency Node
                ///     (Russian - 1049): Узел зависимости
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Dependency Nodes
                ///     (Russian - 1049): Узлы зависимости
                ///     
                ///     Description:
                ///     (English - United States - 1033): The representation of a component dependency node in CRM.
                ///     (Russian - 1049): Представление узла зависимости компонентов в CRM.
                ///</summary>
                public static partial class solution_top_dependencynode
                {
                    public const string Name = "solution_top_dependencynode";

                    public const string ReferencedEntity_solution = "solution";

                    public const string ReferencedAttribute_solutionid = "solutionid";

                    public const string ReferencingEntity_dependencynode = "dependencynode";

                    public const string ReferencingAttribute_topsolutionid = "topsolutionid";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_solution
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_solution
                /// ReferencingEntityNavigationPropertyName    objectid_solution
                /// IsCustomizable                             False                              False
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
                public static partial class userentityinstancedata_solution
                {
                    public const string Name = "userentityinstancedata_solution";

                    public const string ReferencedEntity_solution = "solution";

                    public const string ReferencedAttribute_solutionid = "solutionid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}
