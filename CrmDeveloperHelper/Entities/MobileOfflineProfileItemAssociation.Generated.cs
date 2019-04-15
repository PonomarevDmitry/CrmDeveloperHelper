
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    ///     (Russian - 1049): Связь элемента профиля Mobile Offline
    /// 
    /// DisplayCollectionName:
    ///     (Russian - 1049): Связи элементов профиля Mobile Offline
    /// 
    /// Description:
    ///     (Russian - 1049): Сведения об отношениях, которые должны использоваться для подписки на записи связанной сущности для элемента профиля Mobile Offline.
    /// 
    /// PropertyName                          Value
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False
    /// CanBePrimaryEntityInRelationship      True
    /// CanBeRelatedEntityInRelationship      True
    /// CanChangeHierarchicalRelationship     False
    /// CanChangeTrackingBeEnabled            True
    /// CanCreateAttributes                   False
    /// CanCreateCharts                       False
    /// CanCreateForms                        False
    /// CanCreateViews                        False
    /// CanEnableSyncToExternalSearchIndex    False
    /// CanModifyAdditionalSettings           False
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 True
    /// CollectionSchemaName                  MobileOfflineProfileItemAssociations
    /// DaysSinceRecordLastModified           9999
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         mobileofflineprofileitemassociations
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          True
    /// IsAuditEnabled                        False
    /// IsAvailableOffline                    False
    /// IsBusinessProcessEnabled              True
    /// IsChildEntity                         True
    /// IsConnectionsEnabled                  False
    /// IsCustomEntity                        False
    /// IsCustomizable                        False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     True
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False
    /// IsMappable                            True
    /// IsOfflineInMobileClient               False
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        False
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False
    /// IsRenameable                          False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False
    /// IsVisibleInMobile                     True
    /// IsVisibleInMobileClient               True
    /// LogicalCollectionName                 mobileofflineprofileitemassociations
    /// LogicalName                           mobileofflineprofileitemassociation
    /// ObjectTypeCode                        9868
    /// OwnershipType                         OrganizationOwned
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredMobileOfflineProfileItemAssociation
    /// SchemaName                            MobileOfflineProfileItemAssociation
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class MobileOfflineProfileItemAssociation
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "mobileofflineprofileitemassociation";

            public const string EntitySchemaName = "MobileOfflineProfileItemAssociation";

            public const string EntityPrimaryNameAttribute = "name";

            public const string EntityPrimaryIdAttribute = "mobileofflineprofileitemassociationid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Состояние компонента
                /// 
                /// Description:
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: ComponentState
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Global System  OptionSet componentstate
                /// DefaultFormValue = -1
                /// 
                ///         DisplayName:
                ///             (Russian - 1049): Состояние компонента
                ///         
                ///         Description:
                ///             (Russian - 1049): Состояние этого компонента.
                ///</summary>
                public const string componentstate = "componentstate";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Создано
                /// 
                /// Description:
                ///     (Russian - 1049): Показывает, кто создал запись.
                /// 
                /// SchemaName: CreatedBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///             (Russian - 1049): Пользователь
                ///         
                ///         DisplayCollectionName:
                ///             (Russian - 1049): Пользователи
                ///         
                ///         Description:
                ///             (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public const string createdby = "createdby";

                ///<summary>
                /// SchemaName: CreatedByName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'createdby'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                //public const string createdbyname = "createdbyname";

                ///<summary>
                /// SchemaName: CreatedByYomiName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired    AttributeOf 'createdby'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                //public const string createdbyyominame = "createdbyyominame";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Дата создания
                /// 
                /// Description:
                ///     (Russian - 1049): Показывает дату и время, в которые была создана запись. Дата и время отображаются для часового пояса, выбранного в параметрах Microsoft Dynamics 365.
                /// 
                /// SchemaName: CreatedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateAndTime
                ///</summary>
                public const string createdon = "createdon";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Кем создано (делегат)
                /// 
                /// Description:
                ///     (Russian - 1049): Показывает, кто создал запись от имени другого пользователя.
                /// 
                /// SchemaName: CreatedOnBehalfBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///             (Russian - 1049): Пользователь
                ///         
                ///         DisplayCollectionName:
                ///             (Russian - 1049): Пользователи
                ///         
                ///         Description:
                ///             (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public const string createdonbehalfby = "createdonbehalfby";

                ///<summary>
                /// SchemaName: CreatedOnBehalfByName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'createdonbehalfby'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                //public const string createdonbehalfbyname = "createdonbehalfbyname";

                ///<summary>
                /// SchemaName: CreatedOnBehalfByYomiName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired    AttributeOf 'createdonbehalfby'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                //public const string createdonbehalfbyyominame = "createdonbehalfbyyominame";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Версия введения
                /// 
                /// Description:
                ///     (Russian - 1049): Версия, в которой введена связь элемента профиля Mobile Offline.
                /// 
                /// SchemaName: IntroducedVersion
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 48
                /// Format = VersionNumber    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string introducedversion = "introducedversion";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Управляемый
                /// 
                /// Description:
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: IsManaged
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (Russian - 1049): Неуправляемый
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (Russian - 1049): Управляемый
                /// TrueOption = 1
                ///</summary>
                public const string ismanaged = "ismanaged";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Является допустимым для Mobile Offline
                /// 
                /// Description:
                ///     (Russian - 1049): Информация о том, является связь элемента профиля проверенной или нет
                /// 
                /// SchemaName: IsValidated
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (Russian - 1049): Нет
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (Russian - 1049): Да
                /// TrueOption = 1
                ///</summary>
                public const string isvalidated = "isvalidated";

                ///<summary>
                /// Description:
                ///     (Russian - 1049): Является проверенным именем.
                /// 
                /// SchemaName: IsValidatedName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'isvalidated'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                //public const string isvalidatedname = "isvalidatedname";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Mobile Offline profileitemassociation
                /// 
                /// Description:
                ///     (Russian - 1049): Уникальный идентификатор связи элемента профиля Mobile Offline.
                /// 
                /// SchemaName: MobileOfflineProfileItemAssociationId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string mobileofflineprofileitemassociationid = "mobileofflineprofileitemassociationid";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Уникальный идентификатор
                /// 
                /// Description:
                ///     (Russian - 1049): Только для внутреннего использования
                /// 
                /// SchemaName: MobileOfflineProfileItemAssociationIdUnique
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string mobileofflineprofileitemassociationidunique = "mobileofflineprofileitemassociationidunique";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): В отношении
                /// 
                /// Description:
                ///     (Russian - 1049): Код элемента родительского профиля.
                /// 
                /// SchemaName: MobileOfflineProfileItemId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: mobileofflineprofileitem
                /// 
                ///     Target mobileofflineprofileitem    PrimaryIdAttribute mobileofflineprofileitemid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (Russian - 1049): Элемент профиля Mobile Offline
                ///         
                ///         Description:
                ///             (Russian - 1049): Сведения о доступности сущности для мобильных устройств в автономном режиме для элемента профиля Mobile Offline.
                ///</summary>
                public const string mobileofflineprofileitemid = "mobileofflineprofileitemid";

                ///<summary>
                /// SchemaName: MobileOfflineProfileItemIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired    AttributeOf 'mobileofflineprofileitemid'
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                //public const string mobileofflineprofileitemidname = "mobileofflineprofileitemidname";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Изменено
                /// 
                /// Description:
                ///     (Russian - 1049): Показывает, кто последний обновил запись.
                /// 
                /// SchemaName: ModifiedBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///             (Russian - 1049): Пользователь
                ///         
                ///         DisplayCollectionName:
                ///             (Russian - 1049): Пользователи
                ///         
                ///         Description:
                ///             (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public const string modifiedby = "modifiedby";

                ///<summary>
                /// SchemaName: ModifiedByName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'modifiedby'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                //public const string modifiedbyname = "modifiedbyname";

                ///<summary>
                /// SchemaName: ModifiedByYomiName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired    AttributeOf 'modifiedby'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                //public const string modifiedbyyominame = "modifiedbyyominame";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Дата изменения
                /// 
                /// Description:
                ///     (Russian - 1049): Показывает дату и время последнего обновления записи. Дата и время отображаются для часового пояса, выбранного в параметрах Microsoft Dynamics 365.
                /// 
                /// SchemaName: ModifiedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateAndTime
                ///</summary>
                public const string modifiedon = "modifiedon";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Кем изменено (делегат)
                /// 
                /// Description:
                ///     (Russian - 1049): Показывает, кто обновил запись от имени другого пользователя.
                /// 
                /// SchemaName: ModifiedOnBehalfBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///             (Russian - 1049): Пользователь
                ///         
                ///         DisplayCollectionName:
                ///             (Russian - 1049): Пользователи
                ///         
                ///         Description:
                ///             (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public const string modifiedonbehalfby = "modifiedonbehalfby";

                ///<summary>
                /// SchemaName: ModifiedOnBehalfByName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'modifiedonbehalfby'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                //public const string modifiedonbehalfbyname = "modifiedonbehalfbyname";

                ///<summary>
                /// SchemaName: ModifiedOnBehalfByYomiName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired    AttributeOf 'modifiedonbehalfby'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                //public const string modifiedonbehalfbyyominame = "modifiedonbehalfbyyominame";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Имя
                /// 
                /// Description:
                ///     (Russian - 1049): Введите имя связи элемента профиля Mobile Offline.
                /// 
                /// SchemaName: Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string name = "name";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Предприятие
                /// 
                /// Description:
                ///     (Russian - 1049): Уникальный идентификатор организации, относящейся к связи элемента профиля Mobile Offline.
                /// 
                /// SchemaName: OrganizationId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: organization
                /// 
                ///     Target organization    PrimaryIdAttribute organizationid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (Russian - 1049): Предприятие
                ///         
                ///         DisplayCollectionName:
                ///             (Russian - 1049): Предприятия
                ///         
                ///         Description:
                ///             (Russian - 1049): Верхний уровень бизнес-иерархии Microsoft Dynamics 365. Организация может являться конкретной компанией, холдингом или корпорацией.
                ///</summary>
                public const string organizationid = "organizationid";

                ///<summary>
                /// SchemaName: OrganizationIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired    AttributeOf 'organizationid'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                //public const string organizationidname = "organizationidname";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Время замены записи
                /// 
                /// Description:
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: OverwriteTime
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateOnly
                ///</summary>
                public const string overwritetime = "overwritetime";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Процесс
                /// 
                /// Description:
                ///     (Russian - 1049): Показывает идентификатор процесса.
                /// 
                /// SchemaName: ProcessId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string processid = "processid";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Фильтр сущностей связей элементов профиля
                /// 
                /// Description:
                ///     (Russian - 1049): Критерии фильтра сущностей связей элементов профиля.
                /// 
                /// SchemaName: ProfileItemAssociationEntityFilter
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string profileitemassociationentityfilter = "profileitemassociationentityfilter";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Дата публикации
                /// 
                /// Description:
                ///     (Russian - 1049): Показывает дату и время последней публикации.
                /// 
                /// SchemaName: PublishedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateAndTime
                ///</summary>
                public const string publishedon = "publishedon";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Только для внутреннего использования
                /// 
                /// SchemaName: RelationshipData
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string relationshipdata = "relationshipdata";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Отношение сущности
                /// 
                /// Description:
                ///     (Russian - 1049): Имя схемы отношений сущности
                /// 
                /// SchemaName: RelationshipDisplayName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string relationshipdisplayname = "relationshipdisplayname";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Показывает отношение
                /// 
                /// SchemaName: RelationshipId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string relationshipid = "relationshipid";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Отношение сущности
                /// 
                /// Description:
                ///     (Russian - 1049): Отображаемое имя отношения сущности
                /// 
                /// SchemaName: RelationshipName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string relationshipname = "relationshipname";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Отношение сущности
                /// 
                /// Description:
                ///     (Russian - 1049): Список отношений выбранной сущности в элементе родительского профиля
                /// 
                /// SchemaName: SelectedRelationShipsSchema
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Global System  OptionSet selectedmobileofflineenabledentityrelationships
                /// DefaultFormValue = Null
                /// 
                ///         DisplayName:
                ///             (Russian - 1049): Отношения выбранных сущностей с включенным режимом Mobile Offline
                ///         
                ///         Description:
                ///             (Russian - 1049): это набор параметров отношений выбранной сущности в элементе родительского профиля Mobile Offline.
                ///</summary>
                public const string selectedrelationshipsschema = "selectedrelationshipsschema";

                ///<summary>
                /// SchemaName: SelectedRelationShipsSchemaName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'selectedrelationshipsschema'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                //public const string selectedrelationshipsschemaname = "selectedrelationshipsschemaname";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Решение
                /// 
                /// Description:
                ///     (Russian - 1049): Уникальный идентификатор связанного решения.
                /// 
                /// SchemaName: SolutionId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string solutionid = "solutionid";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Стадия процесса
                /// 
                /// Description:
                ///     (Russian - 1049): Показывает идентификатор стадии.
                /// 
                /// SchemaName: StageId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string stageid = "stageid";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Решение
                /// 
                /// Description:
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: SupportingSolutionId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: False    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string supportingsolutionid = "supportingsolutionid";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Пройденный путь
                /// 
                /// Description:
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: TraversedPath
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1250
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string traversedpath = "traversedpath";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Номер версии
                /// 
                /// Description:
                ///     (Russian - 1049): Номер версии Mobile Offline profileitemassociation.
                /// 
                /// SchemaName: VersionNumber
                /// BigIntAttributeMetadata    AttributeType: BigInt    AttributeTypeName: BigIntType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
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
                /// N:1 - Relationship lk_MobileOfflineProfileItemAssociation_createdby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_MobileOfflineProfileItemAssociation_createdby
                /// ReferencingEntityNavigationPropertyName    createdby
                /// IsCustomizable                             True
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
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity systemuser:
                ///     DisplayName:
                ///         (Russian - 1049): Пользователь
                ///     
                ///     DisplayCollectionName:
                ///         (Russian - 1049): Пользователи
                ///     
                ///     Description:
                ///         (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public static partial class lk_mobileofflineprofileitemassociation_createdby
                {
                    public const string Name = "lk_MobileOfflineProfileItemAssociation_createdby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_mobileofflineprofileitemassociation = "mobileofflineprofileitemassociation";

                    public const string ReferencingAttribute_createdby = "createdby";
                }

                ///<summary>
                /// N:1 - Relationship lk_mobileofflineprofileitemassociation_createdonbehalfby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_mobileofflineprofileitemassociation_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    createdonbehalfby
                /// IsCustomizable                             True
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
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity systemuser:
                ///     DisplayName:
                ///         (Russian - 1049): Пользователь
                ///     
                ///     DisplayCollectionName:
                ///         (Russian - 1049): Пользователи
                ///     
                ///     Description:
                ///         (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public static partial class lk_mobileofflineprofileitemassociation_createdonbehalfby
                {
                    public const string Name = "lk_mobileofflineprofileitemassociation_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_mobileofflineprofileitemassociation = "mobileofflineprofileitemassociation";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_mobileofflineprofileitemassociation_modifiedby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_mobileofflineprofileitemassocaition_modifiedby
                /// ReferencingEntityNavigationPropertyName    modifiedby
                /// IsCustomizable                             True
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
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity systemuser:
                ///     DisplayName:
                ///         (Russian - 1049): Пользователь
                ///     
                ///     DisplayCollectionName:
                ///         (Russian - 1049): Пользователи
                ///     
                ///     Description:
                ///         (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public static partial class lk_mobileofflineprofileitemassociation_modifiedby
                {
                    public const string Name = "lk_mobileofflineprofileitemassociation_modifiedby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_mobileofflineprofileitemassociation = "mobileofflineprofileitemassociation";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";
                }

                ///<summary>
                /// N:1 - Relationship lk_mobileofflineprofileitemassociation_modifiedonbehalfby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_mobileofflineprofileitemassocaition_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    modifiedonbehalfby
                /// IsCustomizable                             True
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
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity systemuser:
                ///     DisplayName:
                ///         (Russian - 1049): Пользователь
                ///     
                ///     DisplayCollectionName:
                ///         (Russian - 1049): Пользователи
                ///     
                ///     Description:
                ///         (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public static partial class lk_mobileofflineprofileitemassociation_modifiedonbehalfby
                {
                    public const string Name = "lk_mobileofflineprofileitemassociation_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_mobileofflineprofileitemassociation = "mobileofflineprofileitemassociation";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship MobileOfflineProfileItem_MobileOfflineProfileItemAssociation
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     MobileOfflineProfileItem_MobileOfflineProfileItemAssociation
                /// ReferencingEntityNavigationPropertyName    regardingobjectid
                /// IsCustomizable                             True
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
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity mobileofflineprofileitem:
                ///     DisplayName:
                ///         (Russian - 1049): Элемент профиля Mobile Offline
                ///     
                ///     Description:
                ///         (Russian - 1049): Сведения о доступности сущности для мобильных устройств в автономном режиме для элемента профиля Mobile Offline.
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

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_mobileofflineprofileitemassociation = "mobileofflineprofileitemassociation";

                    public const string ReferencingAttribute_mobileofflineprofileitemid = "mobileofflineprofileitemid";
                }

                ///<summary>
                /// N:1 - Relationship MobileOfflineProfileItemAssociation_organization
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     MobileOfflineProfileItemAssociation_organization
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False
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
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity organization:
                ///     DisplayName:
                ///         (Russian - 1049): Предприятие
                ///     
                ///     DisplayCollectionName:
                ///         (Russian - 1049): Предприятия
                ///     
                ///     Description:
                ///         (Russian - 1049): Верхний уровень бизнес-иерархии Microsoft Dynamics 365. Организация может являться конкретной компанией, холдингом или корпорацией.
                ///</summary>
                public static partial class mobileofflineprofileitemassociation_organization
                {
                    public const string Name = "MobileOfflineProfileItemAssociation_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_mobileofflineprofileitemassociation = "mobileofflineprofileitemassociation";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }
            }

            #endregion Relationship ManyToOne - N:1.
        }
    }
}