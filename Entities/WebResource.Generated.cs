
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Web Resource
    ///     (Russian - 1049): Веб-ресурс
    /// 
    /// DisplayCollectionName:
    ///     (English - United States - 1033): Web Resources
    ///     (Russian - 1049): Веб-ресурсы
    /// 
    /// Description:
    ///     (English - United States - 1033): Data equivalent to files used in Web development. Web resources provide client-side components that are used to provide custom user interface elements.
    ///     (Russian - 1049): Данные, эквивалентные файлам, используемым в разработке веб-контента. Веб-ресурсы обеспечивают компоненты на стороне клиента, которые используются для создания настраиваемых элементов интерфейса пользователя.
    /// 
    /// PropertyName                          Value                  CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False                  False
    /// CanBePrimaryEntityInRelationship      False                  False
    /// CanBeRelatedEntityInRelationship      False                  False
    /// CanChangeHierarchicalRelationship     False                  False
    /// CanChangeTrackingBeEnabled            True                   True
    /// CanCreateAttributes                   False                  False
    /// CanCreateCharts                       False                  False
    /// CanCreateForms                        False                  False
    /// CanCreateViews                        False                  False
    /// CanEnableSyncToExternalSearchIndex    False                  False
    /// CanModifyAdditionalSettings           False                  True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 False
    /// CollectionSchemaName                  WebResources
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         webresourceset
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False                  False
    /// IsAvailableOffline                    True
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False                  False
    /// IsCustomEntity                        False
    /// IsCustomizable                        False                  False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                  False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                  False
    /// IsMappable                            False                  False
    /// IsOfflineInMobileClient               False                  True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              True                   False
    /// IsRenameable                          False                  False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False                  False
    /// IsVisibleInMobile                     False                  False
    /// IsVisibleInMobileClient               True                   False
    /// LogicalCollectionName                 webresources
    /// LogicalName                           webresource
    /// ObjectTypeCode                        9333
    /// OwnershipType                         OrganizationOwned
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredWebResource
    /// SchemaName                            WebResource
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class WebResource
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "webresource";

            public const string EntitySchemaName = "WebResource";

            public const string EntityPrimaryNameAttribute = "name";

            public const string EntityPrimaryIdAttribute = "webresourceid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Can Be Deleted
                ///     (Russian - 1049): Можно удалить
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether this component can be deleted.
                ///     (Russian - 1049): Сведения, указывающие на возможность удаления этого компонента.
                /// 
                /// SchemaName: CanBeDeleted
                /// ManagedPropertyAttributeMetadata    AttributeType: ManagedProperty    AttributeTypeName: ManagedPropertyType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string canbedeleted = "canbedeleted";

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
                /// 
                ///         DisplayName:
                ///             (English - United States - 1033): Component State
                ///             (Russian - 1049): Состояние компонента
                ///         
                ///         Description:
                ///             (English - United States - 1033): The state of this component.
                ///             (Russian - 1049): Состояние этого компонента.
                ///</summary>
                public const string componentstate = "componentstate";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Bytes of the web resource, in Base64 format.
                ///     (Russian - 1049): Размер веб-ресурса в байтах в формате Base64.
                /// 
                /// SchemaName: Content
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string content = "content";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By
                ///     (Russian - 1049): Создано
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who created the web resource.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, создавшего веб-ресурс.
                /// 
                /// SchemaName: CreatedBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///             (English - United States - 1033): User
                ///             (Russian - 1049): Пользователь
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Users
                ///             (Russian - 1049): Пользователи
                ///         
                ///         Description:
                ///             (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///             (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public const string createdby = "createdby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created On
                ///     (Russian - 1049): Дата создания
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the web resource was created.
                ///     (Russian - 1049): Дата и время создания веб-ресурса.
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who created the web resource.
                ///     (Russian - 1049): Уникальный идентификатор делегата, создавшего веб-ресурс.
                /// 
                /// SchemaName: CreatedOnBehalfBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///             (English - United States - 1033): User
                ///             (Russian - 1049): Пользователь
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Users
                ///             (Russian - 1049): Пользователи
                ///         
                ///         Description:
                ///             (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///             (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public const string createdonbehalfby = "createdonbehalfby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): DependencyXML
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: DependencyXml
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 5000
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string dependencyxml = "dependencyxml";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Description
                ///     (Russian - 1049): Описание
                /// 
                /// Description:
                ///     (English - United States - 1033): Description of the web resource.
                ///     (Russian - 1049): Описание веб-ресурса.
                /// 
                /// SchemaName: Description
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
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
                ///     (English - United States - 1033): Display name of the web resource.
                ///     (Russian - 1049): Отображаемое имя веб-ресурса.
                /// 
                /// SchemaName: DisplayName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Text    ImeMode = Auto    IsLocalizable = True
                ///</summary>
                public const string displayname = "displayname";

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
                ///     (English - United States - 1033): Is Available For Mobile Offline
                ///     (Russian - 1049): Доступен для Mobile Offline
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether this web resource is available for mobile client in offline mode.
                ///     (Russian - 1049): Сведения, которые определяют, доступен ли этот веб-ресурс для мобильного клиента в автономном режиме.
                /// 
                /// SchemaName: IsAvailableForMobileOffline
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
                public const string isavailableformobileoffline = "isavailableformobileoffline";

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
                ///     (English - United States - 1033): Is Enabled For Mobile Client
                ///     (Russian - 1049): Включен для мобильного клиента
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether this web resource is enabled for mobile client.
                ///     (Russian - 1049): Сведения, которые определяют, включен ли этот веб-ресурс для мобильного клиента.
                /// 
                /// SchemaName: IsEnabledForMobileClient
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
                public const string isenabledformobileclient = "isenabledformobileclient";

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
                ///     (English - United States - 1033): Language
                ///     (Russian - 1049): Язык
                /// 
                /// Description:
                ///     (English - United States - 1033): Language of the web resource.
                ///     (Russian - 1049): Язык веб-ресурса.
                /// 
                /// SchemaName: LanguageCode
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = Language
                ///</summary>
                public const string languagecode = "languagecode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By
                ///     (Russian - 1049): Изменено
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who last modified the web resource.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, внесшего последнее изменение в веб-ресурс.
                /// 
                /// SchemaName: ModifiedBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///             (English - United States - 1033): User
                ///             (Russian - 1049): Пользователь
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Users
                ///             (Russian - 1049): Пользователи
                ///         
                ///         Description:
                ///             (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///             (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public const string modifiedby = "modifiedby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified On
                ///     (Russian - 1049): Дата изменения
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the web resource was last modified.
                ///     (Russian - 1049): Дата и время внесения последнего изменения в веб-ресурс.
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
                ///     (English - United States - 1033): Created By (Delegate)
                ///     (Russian - 1049): Кем создано (делегат)
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the delegate user who modified the web resource.
                ///     (Russian - 1049): Уникальный идентификатор делегата, внесшего последнее изменение в веб-ресурс.
                /// 
                /// SchemaName: ModifiedOnBehalfBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///             (English - United States - 1033): User
                ///             (Russian - 1049): Пользователь
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Users
                ///             (Russian - 1049): Пользователи
                ///         
                ///         Description:
                ///             (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///             (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public const string modifiedonbehalfby = "modifiedonbehalfby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Name
                ///     (Russian - 1049): Имя
                /// 
                /// Description:
                ///     (English - United States - 1033): Name of the web resource.
                ///     (Russian - 1049): Название веб-ресурса.
                /// 
                /// SchemaName: Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string name = "name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Organization
                ///     (Russian - 1049): Предприятие
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the organization associated with the web resource.
                ///     (Russian - 1049): Уникальный идентификатор организации, связанной с веб-ресурсом.
                /// 
                /// SchemaName: OrganizationId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: organization
                /// 
                ///     Target organization    PrimaryIdAttribute organizationid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Organization
                ///             (Russian - 1049): Предприятие
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Organizations
                ///             (Russian - 1049): Предприятия
                ///         
                ///         Description:
                ///             (English - United States - 1033): Top level of the Microsoft Dynamics 365 business hierarchy. The organization can be a specific business, holding company, or corporation.
                ///             (Russian - 1049): Верхний уровень бизнес-иерархии Microsoft Dynamics 365. Организация может являться конкретной компанией, холдингом или корпорацией.
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
                ///     (English - United States - 1033): Silverlight Version
                ///     (Russian - 1049): Версия Silverlight
                /// 
                /// Description:
                ///     (English - United States - 1033): Silverlight runtime version number required by a silverlight web resource.
                ///     (Russian - 1049): Номер версии среды выполнения Silverlight, которая требуется веб-ресурсу Silverlight.
                /// 
                /// SchemaName: SilverlightVersion
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Auto    IsLocalizable = True
                ///</summary>
                public const string silverlightversion = "silverlightversion";

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
                /// SchemaName: VersionNumber
                /// BigIntAttributeMetadata    AttributeType: BigInt    AttributeTypeName: BigIntType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -9223372036854775808    MaxValue = 9223372036854775807
                ///</summary>
                public const string versionnumber = "versionnumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Web Resource Identifier
                ///     (Russian - 1049): Идентификатор веб-ресурса
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the web resource.
                ///     (Russian - 1049): Уникальный идентификатор веб-ресурса.
                /// 
                /// SchemaName: WebResourceId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string webresourceid = "webresourceid";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: WebResourceIdUnique
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string webresourceidunique = "webresourceidunique";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Type
                ///     (Russian - 1049): Тип
                /// 
                /// Description:
                ///     (English - United States - 1033): Drop-down list for selecting the type of the web resource.
                ///     (Russian - 1049): Раскрывающийся список для выбора типа веб-ресурса.
                /// 
                /// SchemaName: WebResourceType
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet webresource_webresourcetype
                /// DefaultFormValue = -1
                ///</summary>
                public const string webresourcetype = "webresourcetype";
            }

            #endregion Attributes.

            #region OptionSets.

            public static partial class OptionSets
            {

                #region Picklist OptionSet OptionSets.
                ///<summary>
                /// Attribute: webresourcetype
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Type
                ///     (Russian - 1049): Тип
                /// 
                /// Description:
                ///     (English - United States - 1033): Drop-down list for selecting the type of the web resource.
                ///     (Russian - 1049): Раскрывающийся список для выбора типа веб-ресурса.
                /// 
                /// Local System  OptionSet webresource_webresourcetype
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Type
                ///     (Russian - 1049): Тип
                /// 
                /// Description:
                ///     (English - United States - 1033): Drop-down list for selecting the type of the web resource.
                ///     (Russian - 1049): Раскрывающийся список для выбора типа веб-ресурса.
                ///</summary>
                public enum webresourcetype
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Webpage (HTML)
                    ///     (Russian - 1049): Веб-страница (HTML)
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Webpage_HTML_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Style Sheet (CSS)
                    ///     (Russian - 1049): Таблица стилей (CSS)
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Style_Sheet_CSS_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Script (JScript)
                    ///     (Russian - 1049): Скрипт (JScript)
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Script_JScript_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Data (XML)
                    ///     (Russian - 1049): Данные (XML)
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Data_XML_4 = 4,

                    ///<summary>
                    /// 5
                    /// DisplayOrder: 5
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): PNG format
                    ///     (Russian - 1049): Формат PNG
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    PNG_format_5 = 5,

                    ///<summary>
                    /// 6
                    /// DisplayOrder: 6
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): JPG format
                    ///     (Russian - 1049): Формат JPG
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    JPG_format_6 = 6,

                    ///<summary>
                    /// 7
                    /// DisplayOrder: 7
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): GIF format
                    ///     (Russian - 1049): Формат GIF
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    GIF_format_7 = 7,

                    ///<summary>
                    /// 8
                    /// DisplayOrder: 8
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Silverlight (XAP)
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Silverlight_XAP_8 = 8,

                    ///<summary>
                    /// 9
                    /// DisplayOrder: 9
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Style Sheet (XSL)
                    ///     (Russian - 1049): Таблица стилей (XSL)
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Style_Sheet_XSL_9 = 9,

                    ///<summary>
                    /// 10
                    /// DisplayOrder: 10
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): ICO format
                    ///     (Russian - 1049): Формат ICO
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    ICO_format_10 = 10,

                    ///<summary>
                    /// 11
                    /// DisplayOrder: 11
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): SVG format
                    ///     (Russian - 1049): Формат SVG
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    SVG_format_11 = 11,
                }

                #endregion Picklist OptionSets.
            }

            #endregion OptionSets.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship lk_webresourcebase_createdonbehalfby
                /// 
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_webresourcebase_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    createdonbehalfby
                /// IsCustomizable                             False                                   False
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
                ///         (English - United States - 1033): User
                ///         (Russian - 1049): Пользователь
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///         (Russian - 1049): Пользователи
                ///     
                ///     Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///         (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public static partial class lk_webresourcebase_createdonbehalfby
                {
                    public const string Name = "lk_webresourcebase_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_webresource = "webresource";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_webresourcebase_modifiedonbehalfby
                /// 
                /// PropertyName                               Value                                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_webresourcebase_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    modifiedonbehalfby
                /// IsCustomizable                             False                                    False
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
                ///         (English - United States - 1033): User
                ///         (Russian - 1049): Пользователь
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///         (Russian - 1049): Пользователи
                ///     
                ///     Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///         (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public static partial class lk_webresourcebase_modifiedonbehalfby
                {
                    public const string Name = "lk_webresourcebase_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_webresource = "webresource";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship webresource_createdby
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     webresource_createdby
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
                ///         (English - United States - 1033): User
                ///         (Russian - 1049): Пользователь
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///         (Russian - 1049): Пользователи
                ///     
                ///     Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///         (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public static partial class webresource_createdby
                {
                    public const string Name = "webresource_createdby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_webresource = "webresource";

                    public const string ReferencingAttribute_createdby = "createdby";
                }

                ///<summary>
                /// N:1 - Relationship webresource_modifiedby
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     webresource_modifiedby
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
                ///         (English - United States - 1033): User
                ///         (Russian - 1049): Пользователь
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///         (Russian - 1049): Пользователи
                ///     
                ///     Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///         (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public static partial class webresource_modifiedby
                {
                    public const string Name = "webresource_modifiedby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_webresource = "webresource";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";
                }

                ///<summary>
                /// N:1 - Relationship webresource_organization
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     webresource_organization
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                       False
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
                ///         (English - United States - 1033): Organization
                ///         (Russian - 1049): Предприятие
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Organizations
                ///         (Russian - 1049): Предприятия
                ///     
                ///     Description:
                ///         (English - United States - 1033): Top level of the Microsoft Dynamics 365 business hierarchy. The organization can be a specific business, holding company, or corporation.
                ///         (Russian - 1049): Верхний уровень бизнес-иерархии Microsoft Dynamics 365. Организация может являться конкретной компанией, холдингом или корпорацией.
                ///</summary>
                public static partial class webresource_organization
                {
                    public const string Name = "webresource_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_webresource = "webresource";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship lk_theme_logoid
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_theme_logoid
                /// ReferencingEntityNavigationPropertyName    logoimage
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
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
                /// ReferencingEntity theme:
                ///     DisplayName:
                ///         (English - United States - 1033): Theme
                ///         (Russian - 1049): Тема
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Themes
                ///         (Russian - 1049): Темы
                ///     
                ///     Description:
                ///         (English - United States - 1033): Information that's used to set custom visual theme options for client applications.
                ///         (Russian - 1049): Сведения, используемые для задания параметров пользовательской визуальной темы для клиентских приложений.
                ///</summary>
                public static partial class lk_theme_logoid
                {
                    public const string Name = "lk_theme_logoid";

                    public const string ReferencedEntity_webresource = "webresource";

                    public const string ReferencedAttribute_webresourceid = "webresourceid";

                    public const string ReferencingEntity_theme = "theme";

                    public const string ReferencingAttribute_logoid = "logoid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship solution_configuration_webresource
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
                /// ReferencingEntity solution:
                ///     DisplayName:
                ///         (English - United States - 1033): Solution
                ///         (Russian - 1049): Решение
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Solutions
                ///         (Russian - 1049): Решения
                ///     
                ///     Description:
                ///         (English - United States - 1033): A solution which contains CRM customizations.
                ///         (Russian - 1049): Решение, содержащее настройки CRM.
                ///</summary>
                public static partial class solution_configuration_webresource
                {
                    public const string Name = "solution_configuration_webresource";

                    public const string ReferencedEntity_webresource = "webresource";

                    public const string ReferencedAttribute_webresourceid = "webresourceid";

                    public const string ReferencingEntity_solution = "solution";

                    public const string ReferencingAttribute_configurationpageid = "configurationpageid";

                    public const string ReferencingEntity_PrimaryNameAttribute_friendlyname = "friendlyname";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_webresource
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_webresource
                /// ReferencingEntityNavigationPropertyName    objectid_webresource
                /// IsCustomizable                             False                                 False
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
                ///         (English - United States - 1033): User Entity Instance Data
                ///         (Russian - 1049): Данные экземпляра сущности пользователя
                ///     
                ///     Description:
                ///         (English - United States - 1033): Per User item instance data
                ///         (Russian - 1049): Данные экземпляра позиции "на пользователя"
                ///</summary>
                public static partial class userentityinstancedata_webresource
                {
                    public const string Name = "userentityinstancedata_webresource";

                    public const string ReferencedEntity_webresource = "webresource";

                    public const string ReferencedAttribute_webresourceid = "webresourceid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }

                ///<summary>
                /// 1:N - Relationship webresource_savedqueryvisualizations
                /// 
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     webresource_savedqueryvisualizations
                /// ReferencingEntityNavigationPropertyName    webresourceid
                /// IsCustomizable                             False                                   False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
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
                /// ReferencingEntity savedqueryvisualization:
                ///     DisplayName:
                ///         (English - United States - 1033): System Chart
                ///         (Russian - 1049): Системная диаграмма
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): System Charts
                ///         (Russian - 1049): Системные диаграммы
                ///     
                ///     Description:
                ///         (English - United States - 1033): System chart attached to an entity.
                ///         (Russian - 1049): Системная диаграмма, присоединенная к сущности.
                ///</summary>
                public static partial class webresource_savedqueryvisualizations
                {
                    public const string Name = "webresource_savedqueryvisualizations";

                    public const string ReferencedEntity_webresource = "webresource";

                    public const string ReferencedAttribute_webresourceid = "webresourceid";

                    public const string ReferencingEntity_savedqueryvisualization = "savedqueryvisualization";

                    public const string ReferencingAttribute_webresourceid = "webresourceid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship webresource_userqueryvisualizations
                /// 
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     webresource_userqueryvisualizations
                /// ReferencingEntityNavigationPropertyName    webresourceid
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
                /// ReferencingEntity userqueryvisualization:
                ///     DisplayName:
                ///         (English - United States - 1033): User Chart
                ///         (Russian - 1049): Диаграмма пользователя
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): User Charts
                ///         (Russian - 1049): Диаграммы пользователя
                ///     
                ///     Description:
                ///         (English - United States - 1033): Chart attached to an entity.
                ///         (Russian - 1049): Диаграмма, присоединенная к сущности.
                ///</summary>
                public static partial class webresource_userqueryvisualizations
                {
                    public const string Name = "webresource_userqueryvisualizations";

                    public const string ReferencedEntity_webresource = "webresource";

                    public const string ReferencedAttribute_webresourceid = "webresourceid";

                    public const string ReferencingEntity_userqueryvisualization = "userqueryvisualization";

                    public const string ReferencingAttribute_webresourceid = "webresourceid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}