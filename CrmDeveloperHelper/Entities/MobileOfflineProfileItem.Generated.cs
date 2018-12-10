
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): Mobile Offline Profile Item
    /// (Russian - 1049): Элемент профиля Mobile Offline
    /// 
    /// DisplayCollectionName:
    /// 
    /// Description:
    /// (English - United States - 1033): Information on entity availability to mobile devices in offline mode for a mobile offline profile item.
    /// (Russian - 1049): Сведения о доступности сущности для мобильных устройств в автономном режиме для элемента профиля Mobile Offline.
    /// 
    /// PropertyName                          Value                               CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False                               False
    /// CanBePrimaryEntityInRelationship      True                                False
    /// CanBeRelatedEntityInRelationship      True                                False
    /// CanChangeHierarchicalRelationship     False                               False
    /// CanChangeTrackingBeEnabled            True                                True
    /// CanCreateAttributes                   False                               False
    /// CanCreateCharts                       False                               False
    /// CanCreateForms                        False                               False
    /// CanCreateViews                        False                               False
    /// CanEnableSyncToExternalSearchIndex    False                               False
    /// CanModifyAdditionalSettings           False                               True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 True
    /// CollectionSchemaName                  MobileOfflineProfileItems
    /// DaysSinceRecordLastModified           9999
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         mobileofflineprofileitems
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          True
    /// IsAuditEnabled                        False                               False
    /// IsAvailableOffline                    False
    /// IsBusinessProcessEnabled              True
    /// IsChildEntity                         True
    /// IsConnectionsEnabled                  False                               False
    /// IsCustomEntity                        False
    /// IsCustomizable                        False                               False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                               True
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     True
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                               True
    /// IsMappable                            True                                False
    /// IsOfflineInMobileClient               False                               True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        False
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False                               False
    /// IsRenameable                          False                               False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False                               True
    /// IsVisibleInMobile                     True                                False
    /// IsVisibleInMobileClient               True                                True
    /// LogicalCollectionName                 mobileofflineprofileitems
    /// LogicalName                           mobileofflineprofileitem
    /// ObjectTypeCode                        9867
    /// OwnershipType                         OrganizationOwned
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredMobileOfflineProfileItem
    /// SchemaName                            MobileOfflineProfileItem
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class MobileOfflineProfileItem
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "mobileofflineprofileitem";

            public const string EntitySchemaName = "MobileOfflineProfileItem";

            public const string EntityPrimaryNameAttribute = "name";

            public const string EntityPrimaryIdAttribute = "mobileofflineprofileitemid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Allow Entity to Follow Relationship
                ///     (Russian - 1049): Разрешить сущности подписываться на отношение
                /// 
                /// Description:
                ///     (English - United States - 1033): Specifies whether records of this entity can be followed.
                ///     (Russian - 1049): Указывает, возможна ли подписка на записи этой сущности.
                /// 
                /// SchemaName: CanBeFollowed
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                public const string canbefollowed = "canbefollowed";

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
                ///     (Russian - 1049): Кем создано
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows who created the record.
                ///     (Russian - 1049): Показывает, кто создал запись.
                /// 
                /// SchemaName: CreatedBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
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
                ///     (English - United States - 1033): Shows the date and time when the record was created. The date and time are displayed in the time zone selected in Microsoft Dynamics 365 options.
                ///     (Russian - 1049): Показывает дату и время, в которые была создана запись. Дата и время отображаются для часового пояса, выбранного в параметрах Microsoft Dynamics 365.
                /// 
                /// SchemaName: CreatedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
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
                ///     (English - United States - 1033): Shows who created the record on behalf of another user.
                ///     (Russian - 1049): Показывает, кто создал запись от имени другого пользователя.
                /// 
                /// SchemaName: CreatedOnBehalfBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
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
                ///     (English - United States - 1033): Internal Use Only
                ///     (Russian - 1049): Только для внутреннего использования
                /// 
                /// Description:
                /// 
                /// SchemaName: EntityObjectTypeCode
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 1000000000
                /// Format = None
                ///</summary>
                public const string entityobjecttypecode = "entityobjecttypecode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Get Related Entities
                ///     (Russian - 1049): Получить связанные сущности
                /// 
                /// Description:
                ///     (English - United States - 1033): Specify whether records related to this entity will be made available for offline access.
                ///     (Russian - 1049): Указывает, будут ли записи, связанные с этой сущностью, доступны для доступа в автономном режиме.
                /// 
                /// SchemaName: GetRelatedEntityRecords
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                public const string getrelatedentityrecords = "getrelatedentityrecords";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Introduced Version
                ///     (Russian - 1049): Версия введения
                /// 
                /// Description:
                ///     (English - United States - 1033): Version in which the Mobile offline Profile Item is introduced.
                ///     (Russian - 1049): Версия, в которой введен элемент профиля Mobile Offline.
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
                ///     (English - United States - 1033): Is Managed
                ///     (Russian - 1049): Управляется
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
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
                ///     (English - United States - 1033): Is Valid For Mobile Offline
                ///     (Russian - 1049): Является допустимым для Mobile Offline
                /// 
                /// Description:
                ///     (English - United States - 1033): Information about whether profile item is validated or not
                ///     (Russian - 1049): Информация о том, является элемент профиля проверенным или нет
                /// 
                /// SchemaName: IsValidated
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string isvalidated = "isvalidated";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Visible In Grid
                ///     (Russian - 1049): Отображается в сетке
                /// 
                /// Description:
                ///     (English - United States - 1033): Information about whether the mobile offline profile item is visible in the Profile Item subgrid.
                ///     (Russian - 1049): Сведения о том, отображается ли элемент профиля Mobile Offline во вложенной сетке элемента профиля.
                /// 
                /// SchemaName: IsVisibleInGrid
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = True
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): False
                ///     (Russian - 1049): Ложь
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): True
                ///     (Russian - 1049): Истина
                /// TrueOption = 1
                ///</summary>
                public const string isvisibleingrid = "isvisibleingrid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Mobile Offline Profile Item
                ///     (Russian - 1049): Элемент профиля Mobile Offline
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the mobile offline profile item.
                ///     (Russian - 1049): Уникальный идентификатор элемента профиля Mobile Offline.
                /// 
                /// SchemaName: MobileOfflineProfileItemId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string mobileofflineprofileitemid = "mobileofflineprofileitemid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Unique Id
                ///     (Russian - 1049): Уникальный идентификатор
                /// 
                /// Description:
                ///     (English - United States - 1033): For Internal Use Only
                ///     (Russian - 1049): Только для внутреннего использования
                /// 
                /// SchemaName: MobileOfflineProfileItemIdUnique
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string mobileofflineprofileitemidunique = "mobileofflineprofileitemidunique";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By
                ///     (Russian - 1049): Кем изменено
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows who last updated the record.
                ///     (Russian - 1049): Показывает, кто последним обновил запись.
                /// 
                /// SchemaName: ModifiedBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
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
                ///     (English - United States - 1033): Shows the date and time when the record was last updated. The date and time are displayed in the time zone selected in Microsoft Dynamics 365 options.
                ///     (Russian - 1049): Показывает дату и время последнего обновления записи. Дата и время отображаются для часового пояса, выбранного в параметрах Microsoft Dynamics 365.
                /// 
                /// SchemaName: ModifiedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
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
                ///     (English - United States - 1033): Shows who updated the record on behalf of another user.
                ///     (Russian - 1049): Показывает, кто обновил запись от имени другого пользователя.
                /// 
                /// SchemaName: ModifiedOnBehalfBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
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
                ///     (English - United States - 1033): Enter the name of the mobile offline profile item.
                ///     (Russian - 1049): Введите имя элемента профиля Mobile Offline.
                /// 
                /// SchemaName: Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 255
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string name = "name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Organization
                ///     (Russian - 1049): Организация
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the organization associated with the Mobile Offline Profile Item.
                ///     (Russian - 1049): Уникальный идентификатор организации, связанной с элементом профиля Mobile Offline.
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
                ///     (English - United States - 1033): Process
                ///     (Russian - 1049): Процесс
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the ID of the process.
                ///     (Russian - 1049): Показывает идентификатор процесса.
                /// 
                /// SchemaName: ProcessId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string processid = "processid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Profile item entity filter
                ///     (Russian - 1049): Фильтр сущностей элементов профиля
                /// 
                /// Description:
                ///     (English - United States - 1033): Profile item entity filter criteria
                ///     (Russian - 1049): Критерии фильтра сущностей элементов профиля
                /// 
                /// SchemaName: ProfileItemEntityFilter
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string profileitementityfilter = "profileitementityfilter";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): View to sync data to device
                ///     (Russian - 1049): Просмотр для синхронизации данных на устройство
                /// 
                /// Description:
                ///     (English - United States - 1033): Saved Query associated with the Mobile offline profile item rule.
                ///     (Russian - 1049): Сохраненный запрос, связанный с правилом элемента профиля Mobile Offline.
                /// 
                /// SchemaName: ProfileItemRule
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: savedquery
                /// 
                ///     Target savedquery    PrimaryIdAttribute savedqueryid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): View
                ///         (Russian - 1049): Представление
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Views
                ///         (Russian - 1049): Представления
                ///         
                ///         Description:
                ///         (English - United States - 1033): Saved query against the database.
                ///         (Russian - 1049): Сохраненный запрос к базе данных.
                ///</summary>
                public const string profileitemrule = "profileitemrule";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Published On
                ///     (Russian - 1049): Дата публикации
                /// 
                /// Description:
                ///     (English - United States - 1033): Displays the last published date time.
                ///     (Russian - 1049): Показывает дату и время последней публикации.
                /// 
                /// SchemaName: PublishedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateAndTime
                ///</summary>
                public const string publishedon = "publishedon";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Data Download Filter
                ///     (Russian - 1049): Фильтр загрузки данных
                /// 
                /// Description:
                ///     (English - United States - 1033): Specify data download filter for selected entity
                ///     (Russian - 1049): Задайте фильтр загрузки данных для выбранной сущности
                /// 
                /// SchemaName: RecordDistributionCriteria
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet mobileofflineprofileitem_recorddistributioncriteria
                /// DefaultFormValue = 0
                ///</summary>
                public const string recorddistributioncriteria = "recorddistributioncriteria";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Download my records
                ///     (Russian - 1049): Загрузить мои записи
                /// 
                /// Description:
                /// 
                /// SchemaName: RecordsOwnedByMe
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): False
                ///     (Russian - 1049): Ложь
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): True
                ///     (Russian - 1049): Истина
                /// TrueOption = 1
                ///</summary>
                public const string recordsownedbyme = "recordsownedbyme";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Download my business unit's records
                ///     (Russian - 1049): Загрузить записи моего подразделения
                /// 
                /// Description:
                /// 
                /// SchemaName: RecordsOwnedByMyBusinessUnit
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): False
                ///     (Russian - 1049): Ложь
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): True
                ///     (Russian - 1049): Истина
                /// TrueOption = 1
                ///</summary>
                public const string recordsownedbymybusinessunit = "recordsownedbymybusinessunit";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Download my team's records
                ///     (Russian - 1049): Загрузить записи моей рабочей группы
                /// 
                /// Description:
                /// 
                /// SchemaName: RecordsOwnedByMyTeam
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): False
                ///     (Russian - 1049): Ложь
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): True
                ///     (Russian - 1049): Истина
                /// TrueOption = 1
                ///</summary>
                public const string recordsownedbymyteam = "recordsownedbymyteam";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Regarding
                ///     (Russian - 1049): В отношении
                /// 
                /// Description:
                ///     (English - United States - 1033): Items contained with a particular Profile.
                ///     (Russian - 1049): Элементы, содержащиеся в конкретном профиле.
                /// 
                /// SchemaName: RegardingObjectId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: mobileofflineprofile
                /// 
                ///     Target mobileofflineprofile    PrimaryIdAttribute mobileofflineprofileid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Mobile Offline Profile
                ///         (Russian - 1049): Профиль Mobile Offline
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Mobile Offline Profiles
                ///         (Russian - 1049): Профили Mobile Offline
                ///         
                ///         Description:
                ///         (English - United States - 1033): Information to administer and manage the data available to mobile devices in offline mode.
                ///         (Russian - 1049): Сведения для администрирования данных, доступных мобильным устройствам в автономном режиме, и управления ими.
                ///</summary>
                public const string regardingobjectid = "regardingobjectid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Internal Use Only
                ///     (Russian - 1049): Только для внутреннего использования
                /// 
                /// Description:
                /// 
                /// SchemaName: RelationshipData
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string relationshipdata = "relationshipdata";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Internal Use Only
                ///     (Russian - 1049): Только для внутреннего использования
                /// 
                /// Description:
                /// 
                /// SchemaName: SelectedEntityMetadata
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string selectedentitymetadata = "selectedentitymetadata";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Entity
                ///     (Russian - 1049): Сущность
                /// 
                /// Description:
                ///     (English - United States - 1033): Mobile offline enabled entity
                ///     (Russian - 1049): Сущность с включенным режимом Mobile Offline
                /// 
                /// SchemaName: SelectedEntityTypeCode
                /// EntityNameAttributeMetadata    AttributeType: EntityName    AttributeTypeName: EntityNameType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Global System  OptionSet mobileofflineenabledentities
                /// DefaultFormValue = Null
                ///</summary>
                public const string selectedentitytypecode = "selectedentitytypecode";

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
                ///     (English - United States - 1033): Process Stage
                ///     (Russian - 1049): Стадия процесса
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the ID of the stage.
                ///     (Russian - 1049): Показывает идентификатор стадии.
                /// 
                /// SchemaName: StageId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string stageid = "stageid";

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
                ///     (English - United States - 1033): Traversed Path
                ///     (Russian - 1049): Пройденный путь
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: TraversedPath
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1250
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string traversedpath = "traversedpath";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Version Number
                ///     (Russian - 1049): Номер версии
                /// 
                /// Description:
                ///     (English - United States - 1033): Version number of the Mobile Offline Profile Item.
                ///     (Russian - 1049): Номер версии элемента профиля Mobile Offline.
                /// 
                /// SchemaName: VersionNumber
                /// BigIntAttributeMetadata    AttributeType: BigInt    AttributeTypeName: BigIntType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -9223372036854775808    MaxValue = 9223372036854775807
                ///</summary>
                public const string versionnumber = "versionnumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): View Query
                ///     (Russian - 1049): Просмотр запроса
                /// 
                /// Description:
                ///     (English - United States - 1033): Contains converted sql of the referenced view.
                ///     (Russian - 1049): Содержит преобразованный SQL-код представления, на которое указывает ссылка.
                /// 
                /// SchemaName: ViewQuery
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string viewquery = "viewquery";
            }

            #endregion Attributes.

            #region OptionSets.

            public static partial class OptionSets
            {

                #region Picklist OptionSet OptionSets.
                ///<summary>
                /// Attribute: recorddistributioncriteria
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Data Download Filter
                ///     (Russian - 1049): Фильтр загрузки данных
                /// 
                /// Description:
                ///     (English - United States - 1033): Specify data download filter for selected entity
                ///     (Russian - 1049): Задайте фильтр загрузки данных для выбранной сущности
                /// 
                /// Local System  OptionSet mobileofflineprofileitem_recorddistributioncriteria
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): The rules for record download in offline
                ///     (Russian - 1049): Правила загрузки записей в автономном режиме
                /// 
                /// Description:
                ///</summary>
                public enum recorddistributioncriteria
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Download related data only
                    ///     (Russian - 1049): Загрузить только связанные данные
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Download_related_data_only_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): All records
                    ///     (Russian - 1049): Все записи
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    All_records_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Other data filter
                    ///     (Russian - 1049): Другой фильтр данных
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Other_data_filter_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Custom data filter
                    ///     (Russian - 1049): Пользовательский фильтр данных
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Custom_data_filter_3 = 3,
                }

                #endregion Picklist OptionSets.
            }

            #endregion OptionSets.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship lk_MobileOfflineProfileItem_createdby
                /// 
                /// PropertyName                               Value                                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_MobileOfflineProfileItem_createdby
                /// ReferencingEntityNavigationPropertyName    createdby
                /// IsCustomizable                             True                                     False
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
                public static partial class lk_mobileofflineprofileitem_createdby
                {
                    public const string Name = "lk_MobileOfflineProfileItem_createdby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_mobileofflineprofileitem = "mobileofflineprofileitem";

                    public const string ReferencingAttribute_createdby = "createdby";
                }

                ///<summary>
                /// N:1 - Relationship lk_mobileofflineprofileitem_createdonbehalfby
                /// 
                /// PropertyName                               Value                                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_mobileofflineprofileitem_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    createdonbehalfby
                /// IsCustomizable                             True                                             False
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
                public static partial class lk_mobileofflineprofileitem_createdonbehalfby
                {
                    public const string Name = "lk_mobileofflineprofileitem_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_mobileofflineprofileitem = "mobileofflineprofileitem";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_mobileofflineprofileitem_modifiedby
                /// 
                /// PropertyName                               Value                                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_mobileofflineprofileitem_modifiedby
                /// ReferencingEntityNavigationPropertyName    modifiedby
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
                public static partial class lk_mobileofflineprofileitem_modifiedby
                {
                    public const string Name = "lk_mobileofflineprofileitem_modifiedby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_mobileofflineprofileitem = "mobileofflineprofileitem";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";
                }

                ///<summary>
                /// N:1 - Relationship lk_mobileofflineprofileitem_modifiedonbehalfby
                /// 
                /// PropertyName                               Value                                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_mobileofflineprofileitem_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    modifiedonbehalfby
                /// IsCustomizable                             True                                              False
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
                public static partial class lk_mobileofflineprofileitem_modifiedonbehalfby
                {
                    public const string Name = "lk_mobileofflineprofileitem_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_mobileofflineprofileitem = "mobileofflineprofileitem";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_mobileofflineprofileitem_savedquery
                /// 
                /// PropertyName                               Value                                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_mobileofflineprofileitem_savedquery
                /// ReferencingEntityNavigationPropertyName    profileitemrule
                /// IsCustomizable                             False                                     False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
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
                /// ReferencedEntity savedquery:
                ///     DisplayName:
                ///     (English - United States - 1033): View
                ///     (Russian - 1049): Представление
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Views
                ///     (Russian - 1049): Представления
                ///     
                ///     Description:
                ///     (English - United States - 1033): Saved query against the database.
                ///     (Russian - 1049): Сохраненный запрос к базе данных.
                ///</summary>
                public static partial class lk_mobileofflineprofileitem_savedquery
                {
                    public const string Name = "lk_mobileofflineprofileitem_savedquery";

                    public const string ReferencedEntity_savedquery = "savedquery";

                    public const string ReferencedAttribute_savedqueryid = "savedqueryid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_mobileofflineprofileitem = "mobileofflineprofileitem";

                    public const string ReferencingAttribute_profileitemrule = "profileitemrule";
                }

                ///<summary>
                /// N:1 - Relationship MobileOfflineProfile_MobileOfflineProfileItem
                /// 
                /// PropertyName                               Value                                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     MobileOfflineProfile_MobileOfflineProfileItem
                /// ReferencingEntityNavigationPropertyName    regardingobjectid
                /// IsCustomizable                             True                                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              ParentChild
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity mobileofflineprofile:
                ///     DisplayName:
                ///     (English - United States - 1033): Mobile Offline Profile
                ///     (Russian - 1049): Профиль Mobile Offline
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Mobile Offline Profiles
                ///     (Russian - 1049): Профили Mobile Offline
                ///     
                ///     Description:
                ///     (English - United States - 1033): Information to administer and manage the data available to mobile devices in offline mode.
                ///     (Russian - 1049): Сведения для администрирования данных, доступных мобильным устройствам в автономном режиме, и управления ими.
                /// 
                /// AttributeMaps:
                ///     SourceEntity                    TargetEntity
                ///     mobileofflineprofile      ->    mobileofflineprofileitem
                ///     
                ///     SourceAttribute                 TargetAttribute
                ///     mobileofflineprofileid    ->    regardingobjectid
                ///     name                      ->    regardingobjectidname
                ///     selectedentitymetadata    ->    selectedentitymetadata
                ///</summary>
                public static partial class mobileofflineprofile_mobileofflineprofileitem
                {
                    public const string Name = "MobileOfflineProfile_MobileOfflineProfileItem";

                    public const string ReferencedEntity_mobileofflineprofile = "mobileofflineprofile";

                    public const string ReferencedAttribute_mobileofflineprofileid = "mobileofflineprofileid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_mobileofflineprofileitem = "mobileofflineprofileitem";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";
                }

                ///<summary>
                /// N:1 - Relationship MobileOfflineProfileItem_organization
                /// 
                /// PropertyName                               Value                                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     MobileOfflineProfileItem_organization
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
                public static partial class mobileofflineprofileitem_organization
                {
                    public const string Name = "MobileOfflineProfileItem_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_mobileofflineprofileitem = "mobileofflineprofileitem";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship MobileOfflineProfileItem_MobileOfflineProfileItemAssociation
                /// 
                /// PropertyName                               Value                                                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     MobileOfflineProfileItem_MobileOfflineProfileItemAssociation
                /// ReferencingEntityNavigationPropertyName    regardingobjectid
                /// IsCustomizable                             True                                                            False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              ParentChild
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity mobileofflineprofileitemassociation:
                ///     DisplayName:
                ///     (English - United States - 1033): Mobile Offline Profile Item Association
                ///     (Russian - 1049): Связь элемента профиля Mobile Offline
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Mobile Offline Profile Item Associations
                ///     (Russian - 1049): Связи элементов профиля Mobile Offline
                ///     
                ///     Description:
                ///     (English - United States - 1033): Information on relationships to be used to follow related entity's records for mobile offline profile item.
                ///     (Russian - 1049): Сведения об отношениях, которые должны использоваться для подписки на записи связанной сущности для элемента профиля Mobile Offline.
                /// 
                /// AttributeMaps:
                ///     SourceEntity                        TargetEntity
                ///     mobileofflineprofileitem      ->    mobileofflineprofileitemassociation
                ///     
                ///     SourceAttribute                     TargetAttribute
                ///     mobileofflineprofileitemid    ->    mobileofflineprofileitemid
                ///     name                          ->    mobileofflineprofileitemidname
                ///     relationshipdata              ->    relationshipdata
                ///</summary>
                public static partial class mobileofflineprofileitem_mobileofflineprofileitemassociation
                {
                    public const string Name = "MobileOfflineProfileItem_MobileOfflineProfileItemAssociation";

                    public const string ReferencedEntity_mobileofflineprofileitem = "mobileofflineprofileitem";

                    public const string ReferencedAttribute_mobileofflineprofileitemid = "mobileofflineprofileitemid";

                    public const string ReferencingEntity_mobileofflineprofileitemassociation = "mobileofflineprofileitemassociation";

                    public const string ReferencingAttribute_mobileofflineprofileitemid = "mobileofflineprofileitemid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}
