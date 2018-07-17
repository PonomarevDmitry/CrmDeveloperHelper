
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    ///     (English - United States - 1033): Data Map
    ///     (Russian - 1049): Сопоставление данных
    /// 
    /// DisplayCollectionName:
    ///     (English - United States - 1033): Data Maps
    ///     (Russian - 1049): Сопоставления данных
    /// 
    /// Description:
    ///     (English - United States - 1033): Data map used in import.
    ///     (Russian - 1049): Карта данных, использованная в импорте.
    /// 
    /// PropertyName                          Value                CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False                False
    /// CanBePrimaryEntityInRelationship      True                 False
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
    /// ChangeTrackingEnabled                 False
    /// CollectionSchemaName                  ImportMaps
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         importmaps
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False                True
    /// IsAvailableOffline                    False
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
    /// IsOptimisticConcurrencyEnabled        False
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
    /// LogicalCollectionName                 importmaps
    /// LogicalName                           importmap
    /// ObjectTypeCode                        4411
    /// OwnershipType                         UserOwned
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredImportMap
    /// SchemaName                            ImportMap
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class ImportMap
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "importmap";

            public const string EntitySchemaName = "ImportMap";

            public const string EntityPrimaryNameAttribute = "name";

            public const string EntityPrimaryIdAttribute = "importmapid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By
                ///     (Russian - 1049): Создано
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows who created the record.
                ///     (Russian - 1049): Показывает, кто создал запись.
                /// 
                /// SchemaName: CreatedBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
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
                ///     (English - United States - 1033): Shows the date and time when the record was created. The date and time are displayed in the time zone selected in Microsoft Dynamics 365 options.
                ///     (Russian - 1049): Показывает дату и время, в которые была создана запись. Дата и время отображаются для часового пояса, выбранного в параметрах Microsoft Dynamics 365.
                /// 
                /// SchemaName: CreatedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: SystemRequired
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
                ///     (English - United States - 1033): Shows who created the record on behalf of another user.
                ///     (Russian - 1049): Показывает, кто создал запись от имени другого пользователя.
                /// 
                /// SchemaName: CreatedOnBehalfBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
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
                ///     (English - United States - 1033): Description
                ///     (Russian - 1049): Описание
                /// 
                /// Description:
                ///     (English - United States - 1033): Type additional information to describe the data map, such as the intended use or data source.
                ///     (Russian - 1049): Введите дополнительные сведения, описывающие сопоставление данных, например его предназначение или источник данных.
                /// 
                /// SchemaName: Description
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 2000
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string description = "description";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Entities Per File
                ///     (Russian - 1049): Сущностей на файл
                /// 
                /// Description:
                ///     (English - United States - 1033): Choose whether a data file can contain data for one or more record types.
                ///     (Russian - 1049): Укажите, может ли файл данных содержать данные для одного или для многих типов записей.
                /// 
                /// SchemaName: EntitiesPerFile
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet importmap_entitiesperfile
                /// DefaultFormValue = -1
                ///</summary>
                public const string entitiesperfile = "entitiesperfile";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Data Map
                ///     (Russian - 1049): Сопоставление данных
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the data map.
                ///     (Russian - 1049): Уникальный идентификатор сопоставления данных.
                /// 
                /// SchemaName: ImportMapId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string importmapid = "importmapid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Map Type
                ///     (Russian - 1049): Тип сопоставления
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the type of data map to distinguish out-of-the-box data maps from custom maps.
                ///     (Russian - 1049): Выберите тип сопоставления данных, чтобы отличать готовые сопоставления данных от пользовательских.
                /// 
                /// SchemaName: ImportMapType
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet importmap_importmaptype
                /// DefaultFormValue = -1
                ///</summary>
                public const string importmaptype = "importmaptype";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Valid For Import
                ///     (Russian - 1049): Является допустимой для импорта
                /// 
                /// Description:
                ///     (English - United States - 1033): Information about whether the data map is valid for use with data import.
                ///     (Russian - 1049): Информация о том, может ли эта карта данных использоваться для импорта данных.
                /// 
                /// SchemaName: IsValidForImport
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
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
                public const string isvalidforimport = "isvalidforimport";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Wizard-Created
                ///     (Russian - 1049): Создается мастером
                /// 
                /// Description:
                ///     (English - United States - 1033): Information about whether this data map was created by the Data Migration Manager.
                ///     (Russian - 1049): Информация о том, была ли эта карта данных создана мастером переноса данных.
                /// 
                /// SchemaName: IsWizardCreated
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string iswizardcreated = "iswizardcreated";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Map Customizations
                ///     (Russian - 1049): Сопоставление настроек
                /// 
                /// Description:
                ///     (English - United States - 1033): Customizations XML
                ///     (Russian - 1049): Настройки XML
                /// 
                /// SchemaName: MapCustomizations
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1073741823
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string mapcustomizations = "mapcustomizations";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By
                ///     (Russian - 1049): Изменено
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows who last updated the record.
                ///     (Russian - 1049): Показывает, кто последний обновил запись.
                /// 
                /// SchemaName: ModifiedBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
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
                ///     (English - United States - 1033): Shows the date and time when the record was last updated. The date and time are displayed in the time zone selected in Microsoft Dynamics 365 options.
                ///     (Russian - 1049): Показывает дату и время последнего обновления записи. Дата и время отображаются для часового пояса, выбранного в параметрах Microsoft Dynamics 365.
                /// 
                /// SchemaName: ModifiedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: SystemRequired
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
                ///     (English - United States - 1033): Shows who last updated the record on behalf of another user.
                ///     (Russian - 1049): Показывает, кто последним обновил запись от имени другого пользователя.
                /// 
                /// SchemaName: ModifiedOnBehalfBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
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
                ///     (English - United States - 1033): Map Name
                ///     (Russian - 1049): Имя сопоставления
                /// 
                /// Description:
                ///     (English - United States - 1033): Type a descriptive name for the data map.
                ///     (Russian - 1049): Введите информативное название сопоставления данных.
                /// 
                /// SchemaName: Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 320
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string name = "name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Owner
                ///     (Russian - 1049): Ответственный
                /// 
                /// Description:
                ///     (English - United States - 1033): Enter the user or team who is assigned to manage the record. This field is updated every time the record is assigned to a different user.
                ///     (Russian - 1049): Введите пользователя или рабочую группу, которым назначено управление записью. Это поле обновляется при каждом назначении записи другому пользователю.
                /// 
                /// SchemaName: OwnerId
                /// LookupAttributeMetadata    AttributeType: Owner    AttributeTypeName: OwnerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser,team
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
                /// 
                ///     Target team    PrimaryIdAttribute teamid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Team
                ///             (Russian - 1049): Рабочая группа
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Teams
                ///             (Russian - 1049): Рабочие группы
                ///         
                ///         Description:
                ///             (English - United States - 1033): Collection of system users that routinely collaborate. Teams can be used to simplify record sharing and provide team members with common access to organization data when team members belong to different Business Units.
                ///             (Russian - 1049): Набор системных пользователей, которые обычно участвуют в совместной работе. Рабочие группы можно использовать для упрощения предоставления участникам группы общего доступа к записям и данным организации, если участники принадлежат к разным подразделениям.
                ///</summary>
                public const string ownerid = "ownerid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Owning Business Unit
                ///     (Russian - 1049): Ответственное подразделение
                /// 
                /// Description:
                ///     (English - United States - 1033): Business unit that owns the data map.
                ///     (Russian - 1049): Подразделение, ответственное за сопоставление данных.
                /// 
                /// SchemaName: OwningBusinessUnit
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: businessunit
                /// 
                ///     Target businessunit    PrimaryIdAttribute businessunitid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Business Unit
                ///             (Russian - 1049): Подразделение
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Business Units
                ///             (Russian - 1049): Подразделения
                ///         
                ///         Description:
                ///             (English - United States - 1033): Business, division, or department in the Microsoft Dynamics 365 database.
                ///             (Russian - 1049): Компания, подразделение или отдел в базе данных Microsoft Dynamics 365.
                ///</summary>
                public const string owningbusinessunit = "owningbusinessunit";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Owning Team
                ///     (Russian - 1049): Ответственная рабочая группа
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the team who owns the data map.
                ///     (Russian - 1049): Уникальный идентификатор рабочей группы, ответственной за сопоставление данных.
                /// 
                /// SchemaName: OwningTeam
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: team
                /// 
                ///     Target team    PrimaryIdAttribute teamid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Team
                ///             (Russian - 1049): Рабочая группа
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Teams
                ///             (Russian - 1049): Рабочие группы
                ///         
                ///         Description:
                ///             (English - United States - 1033): Collection of system users that routinely collaborate. Teams can be used to simplify record sharing and provide team members with common access to organization data when team members belong to different Business Units.
                ///             (Russian - 1049): Набор системных пользователей, которые обычно участвуют в совместной работе. Рабочие группы можно использовать для упрощения предоставления участникам группы общего доступа к записям и данным организации, если участники принадлежат к разным подразделениям.
                ///</summary>
                public const string owningteam = "owningteam";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Owning User
                ///     (Russian - 1049): Ответственный пользователь
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who owns the data map.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, ответственного за сопоставление данных.
                /// 
                /// SchemaName: OwningUser
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
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
                public const string owninguser = "owninguser";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Source
                ///     (Russian - 1049): Источник
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the name of the migration source that this data map is used for.
                ///     (Russian - 1049): Введите имя источника переноса, для которого используется это сопоставление данных.
                /// 
                /// SchemaName: Source
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 160
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string source = "source";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Source System Type
                ///     (Russian - 1049): Тип исходной системы
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the migration source type that this data map is used for.
                ///     (Russian - 1049): Выберите тип источника переноса, для которого используется это сопоставление данных.
                /// 
                /// SchemaName: SourceType
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet importmap_sourcetype
                /// DefaultFormValue = -1
                ///</summary>
                public const string sourcetype = "sourcetype";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Source User Value
                ///     (Russian - 1049): Значение исходного пользователя
                /// 
                /// Description:
                ///     (English - United States - 1033): Source user value for source Microsoft Dynamics 365 user link.
                ///     (Russian - 1049): Значение исходного пользователя для ссылки на исходного пользователя Microsoft Dynamics 365.
                /// 
                /// SchemaName: SourceUserIdentifierForSourceCRMUserLink
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 160
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string sourceuseridentifierforsourcecrmuserlink = "sourceuseridentifierforsourcecrmuserlink";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Source User Identifier
                ///     (Russian - 1049): Идентификатор исходного пользователя
                /// 
                /// Description:
                ///     (English - United States - 1033): Column in the source file that uniquely identifies a user.
                ///     (Russian - 1049): Столбец в исходном файле, уникальным образом определяющий пользователя.
                /// 
                /// SchemaName: SourceUserIdentifierForSourceDataSourceUserLink
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 160
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string sourceuseridentifierforsourcedatasourceuserlink = "sourceuseridentifierforsourcedatasourceuserlink";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Status
                ///     (Russian - 1049): Состояние
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows whether the data map is active or inactive. Inactive data maps are read-only and can't be edited.
                ///     (Russian - 1049): Указывает, активно ли сопоставление данных. Неактивные сопоставления данных доступны только для чтения, их невозможно изменять.
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
                ///     (English - United States - 1033): Select the data map's status.
                ///     (Russian - 1049): Выберите состояние сопоставления данных.
                /// 
                /// SchemaName: StatusCode
                /// StatusAttributeMetadata    AttributeType: Status    AttributeTypeName: StatusType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// DefaultFormValue = 1
                ///</summary>
                public const string statuscode = "statuscode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Record Type
                ///     (Russian - 1049): Тип записи
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the name of the Microsoft Dynamics 365 record type that this data map is defined for.
                ///     (Russian - 1049): Выбор имени типа записей Microsoft Dynamics 365, для которого определено это сопоставление данных.
                /// 
                /// SchemaName: TargetEntity
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet importmap_targetentity
                /// DefaultFormValue = 0
                ///</summary>
                public const string targetentity = "targetentity";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Target User Value
                ///     (Russian - 1049): Значение конечного пользователя
                /// 
                /// Description:
                ///     (English - United States - 1033): Microsoft Dynamics 365 user.
                ///     (Russian - 1049): Пользователь Microsoft Dynamics 365.
                /// 
                /// SchemaName: TargetUserIdentifierForSourceCRMUserLink
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 160
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string targetuseridentifierforsourcecrmuserlink = "targetuseridentifierforsourcecrmuserlink";
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
                ///     (English - United States - 1033): Shows whether the data map is active or inactive. Inactive data maps are read-only and can't be edited.
                ///     (Russian - 1049): Указывает, активно ли сопоставление данных. Неактивные сопоставления данных доступны только для чтения, их невозможно изменять.
                ///</summary>
                public enum statecode
                {
                    ///<summary>
                    /// Default statuscode: Active_1, 1
                    /// InvariantName: Active
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Active
                    ///     (Russian - 1049): Активный
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Active_0 = 0,

                    ///<summary>
                    /// Default statuscode: Inactive_2, 2
                    /// InvariantName: Inactive
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Inactive
                    ///     (Russian - 1049): Неактивный
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Inactive_1 = 1,
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
                ///     (English - United States - 1033): Shows whether the data map is active or inactive. Inactive data maps are read-only and can't be edited.
                ///     (Russian - 1049): Указывает, активно ли сопоставление данных. Неактивные сопоставления данных доступны только для чтения, их невозможно изменять.
                ///</summary>
                public enum statuscode
                {
                    ///<summary>
                    /// Linked Statecode: Active_0, 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Active
                    ///     (Russian - 1049): Активный
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Active_0_Active_1 = 1,

                    ///<summary>
                    /// Linked Statecode: Inactive_1, 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Inactive
                    ///     (Russian - 1049): Неактивный
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Inactive_1_Inactive_2 = 2,
                }

                #endregion State and Status OptionSets.

                #region Picklist OptionSet OptionSets.

                ///<summary>
                /// Attribute: entitiesperfile
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Entities Per File
                ///     (Russian - 1049): Сущностей на файл
                /// 
                /// Description:
                ///     (English - United States - 1033): Choose whether a data file can contain data for one or more record types.
                ///     (Russian - 1049): Укажите, может ли файл данных содержать данные для одного или для многих типов записей.
                /// 
                /// Local System  OptionSet importmap_entitiesperfile
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Entities Per File
                ///     (Russian - 1049): Сущностей на файл
                /// 
                /// Description:
                ///     (English - United States - 1033): Denotes if a data file can have data for one or more entity
                ///     (Russian - 1049): Определяет, содержатся ли в файле данные для одной или нескольких сущностей
                ///</summary>
                public enum entitiesperfile
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Single Entity Per File
                    ///     (Russian - 1049): Одна сущность в файле
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Single_Entity_Per_File_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Multiple Entities Per File
                    ///     (Russian - 1049): Несколько сущностей в файле
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Multiple_Entities_Per_File_2 = 2,
                }

                ///<summary>
                /// Attribute: importmaptype
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Map Type
                ///     (Russian - 1049): Тип сопоставления
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the type of data map to distinguish out-of-the-box data maps from custom maps.
                ///     (Russian - 1049): Выберите тип сопоставления данных, чтобы отличать готовые сопоставления данных от пользовательских.
                /// 
                /// Local System  OptionSet importmap_importmaptype
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Map Type
                ///     (Russian - 1049): Тип сопоставления
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of data map.
                ///     (Russian - 1049): Тип сопоставления данных.
                ///</summary>
                public enum importmaptype
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Standard
                    ///     (Russian - 1049): Стандартный
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Standard_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Out of Box
                    ///     (Russian - 1049): OutOfBox
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Out_of_Box_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): In Process
                    ///     (Russian - 1049): InProcess
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    In_Process_3 = 3,
                }

                ///<summary>
                /// Attribute: sourcetype
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Source System Type
                ///     (Russian - 1049): Тип исходной системы
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the migration source type that this data map is used for.
                ///     (Russian - 1049): Выберите тип источника переноса, для которого используется это сопоставление данных.
                /// 
                /// Local System  OptionSet importmap_sourcetype
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Source System Type
                ///     (Russian - 1049): Тип исходной системы
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of the migration source for which this data map is used.
                ///     (Russian - 1049): Тип источника переноса, для которого используется это сопоставление данных.
                ///</summary>
                public enum sourcetype
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Map For SalesForce.com Full Data Export
                    ///     (Russian - 1049): Сопоставление для экспорта всех данных SalesForce.com
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Map_For_SalesForce_com_Full_Data_Export_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Map For SalesForce.com Report Export
                    ///     (Russian - 1049): Сопоставление для экспорта отчетов SalesForce.com
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Map_For_SalesForce_com_Report_Export_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Map For SalesForce.com Contact and Account Report Export
                    ///     (Russian - 1049): Сопоставление для экспорта отчетов по контактам и организациям SalesForce.com
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Map_For_SalesForce_com_Contact_and_Account_Report_Export_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Microsoft Office Outlook 2010 with Business Contact Manager
                    ///     (Russian - 1049): Microsoft Office Outlook 2010 с диспетчером контактов
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Microsoft_Office_Outlook_2010_with_Business_Contact_Manager_4 = 4,

                    ///<summary>
                    /// 5
                    /// DisplayOrder: 5
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Generic Map for Contact and Account
                    ///     (Russian - 1049): Универсальное сопоставление для контактов и организаций
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Generic_Map_for_Contact_and_Account_5 = 5,
                }

                ///<summary>
                /// Attribute: targetentity
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Record Type
                ///     (Russian - 1049): Тип записи
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the name of the Microsoft Dynamics 365 record type that this data map is defined for.
                ///     (Russian - 1049): Выбор имени типа записей Microsoft Dynamics 365, для которого определено это сопоставление данных.
                /// 
                /// Local System  OptionSet importmap_targetentity
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Record Type
                ///     (Russian - 1049): Тип записи
                /// 
                /// Description:
                ///     (English - United States - 1033): Name of the Microsoft Dynamics 365 record type for which this data map is defined.
                ///     (Russian - 1049): Имя типа записей Microsoft Dynamics 365, для которого определено это сопоставление данных.
                ///</summary>
                //public enum targetentity

                #endregion Picklist OptionSets.
            }

            #endregion OptionSets.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship BusinessUnit_ImportMaps
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_ImportMaps
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// ReferencedEntity businessunit:
                ///     DisplayName:
                ///         (English - United States - 1033): Business Unit
                ///         (Russian - 1049): Подразделение
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Business Units
                ///         (Russian - 1049): Подразделения
                ///     
                ///     Description:
                ///         (English - United States - 1033): Business, division, or department in the Microsoft Dynamics 365 database.
                ///         (Russian - 1049): Компания, подразделение или отдел в базе данных Microsoft Dynamics 365.
                ///</summary>
                public static partial class businessunit_importmaps
                {
                    public const string Name = "BusinessUnit_ImportMaps";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_importmap = "importmap";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";
                }

                ///<summary>
                /// N:1 - Relationship lk_importmap_createdonbehalfby
                /// 
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_importmap_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    createdonbehalfby
                /// IsCustomizable                             False                             False
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
                public static partial class lk_importmap_createdonbehalfby
                {
                    public const string Name = "lk_importmap_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_importmap = "importmap";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_importmap_modifiedonbehalfby
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_importmap_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    modifiedonbehalfby
                /// IsCustomizable                             False                              False
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
                public static partial class lk_importmap_modifiedonbehalfby
                {
                    public const string Name = "lk_importmap_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_importmap = "importmap";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_importmapbase_createdby
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_importmapbase_createdby
                /// ReferencingEntityNavigationPropertyName    createdby
                /// IsCustomizable                             False                         False
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
                public static partial class lk_importmapbase_createdby
                {
                    public const string Name = "lk_importmapbase_createdby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_importmap = "importmap";

                    public const string ReferencingAttribute_createdby = "createdby";
                }

                ///<summary>
                /// N:1 - Relationship lk_importmapbase_modifiedby
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_importmapbase_modifiedby
                /// ReferencingEntityNavigationPropertyName    modifiedby
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
                public static partial class lk_importmapbase_modifiedby
                {
                    public const string Name = "lk_importmapbase_modifiedby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_importmap = "importmap";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";
                }

                ///<summary>
                /// N:1 - Relationship owner_importmaps
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     owner_importmaps
                /// ReferencingEntityNavigationPropertyName    ownerid
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
                /// ReferencedEntity owner:
                ///     DisplayName:
                ///         (English - United States - 1033): Owner
                ///         (Russian - 1049): Ответственный
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Owners
                ///         (Russian - 1049): Ответственные
                ///     
                ///     Description:
                ///         (English - United States - 1033): Group of undeleted system users and undeleted teams. Owners can be used to control access to specific objects.
                ///         (Russian - 1049): Группа для восстановленных системных пользователей и рабочих групп. Для контроля доступа к конкретным объектам можно использовать ответственных.
                ///</summary>
                public static partial class owner_importmaps
                {
                    public const string Name = "owner_importmaps";

                    public const string ReferencedEntity_owner = "owner";

                    public const string ReferencedAttribute_ownerid = "ownerid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_importmap = "importmap";

                    public const string ReferencingAttribute_ownerid = "ownerid";
                }

                ///<summary>
                /// N:1 - Relationship SystemUser_ImportMaps
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     SystemUser_ImportMaps
                /// ReferencingEntityNavigationPropertyName    owninguser
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
                public static partial class systemuser_importmaps
                {
                    public const string Name = "SystemUser_ImportMaps";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_importmap = "importmap";

                    public const string ReferencingAttribute_owninguser = "owninguser";
                }

                ///<summary>
                /// N:1 - Relationship team_ImportMaps
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_ImportMaps
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencedEntity team:
                ///     DisplayName:
                ///         (English - United States - 1033): Team
                ///         (Russian - 1049): Рабочая группа
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Teams
                ///         (Russian - 1049): Рабочие группы
                ///     
                ///     Description:
                ///         (English - United States - 1033): Collection of system users that routinely collaborate. Teams can be used to simplify record sharing and provide team members with common access to organization data when team members belong to different Business Units.
                ///         (Russian - 1049): Набор системных пользователей, которые обычно участвуют в совместной работе. Рабочие группы можно использовать для упрощения предоставления участникам группы общего доступа к записям и данным организации, если участники принадлежат к разным подразделениям.
                ///</summary>
                public static partial class team_importmaps
                {
                    public const string Name = "team_ImportMaps";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_importmap = "importmap";

                    public const string ReferencingAttribute_owningteam = "owningteam";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship ColumnMapping_ImportMap
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     ColumnMapping_ImportMap
                /// ReferencingEntityNavigationPropertyName    importmapid
                /// IsCustomizable                             False                      False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
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
                /// ReferencingEntity columnmapping:
                ///     DisplayName:
                ///         (English - United States - 1033): Column Mapping
                ///         (Russian - 1049): Сопоставление столбцов
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Column Mappings
                ///         (Russian - 1049): Сопоставления столбцов
                ///     
                ///     Description:
                ///         (English - United States - 1033): Mapping for columns in a data map.
                ///         (Russian - 1049): Сопоставление столбцов в сопоставлении данных.
                ///</summary>
                public static partial class columnmapping_importmap
                {
                    public const string Name = "ColumnMapping_ImportMap";

                    public const string ReferencedEntity_importmap = "importmap";

                    public const string ReferencedAttribute_importmapid = "importmapid";

                    public const string ReferencingEntity_columnmapping = "columnmapping";

                    public const string ReferencingAttribute_importmapid = "importmapid";

                    public const string ReferencingEntity_PrimaryNameAttribute_sourceattributename = "sourceattributename";
                }

                ///<summary>
                /// 1:N - Relationship ImportEntityMapping_ImportMap
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     ImportEntityMapping_ImportMap
                /// ReferencingEntityNavigationPropertyName    importmapid
                /// IsCustomizable                             False                            False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
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
                /// ReferencingEntity importentitymapping:
                ///     DisplayName:
                ///         (English - United States - 1033): Import Entity Mapping
                ///         (Russian - 1049): Сопоставление сущностей для импорта
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Import Entity Mappings
                ///     
                ///     Description:
                ///         (English - United States - 1033): Mapping for entities in a data map.
                ///         (Russian - 1049): Сопоставление сущностей в сопоставлении данных.
                ///</summary>
                public static partial class importentitymapping_importmap
                {
                    public const string Name = "ImportEntityMapping_ImportMap";

                    public const string ReferencedEntity_importmap = "importmap";

                    public const string ReferencedAttribute_importmapid = "importmapid";

                    public const string ReferencingEntity_importentitymapping = "importentitymapping";

                    public const string ReferencingAttribute_importmapid = "importmapid";
                }

                ///<summary>
                /// 1:N - Relationship ImportMap_AsyncOperations
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     ImportMap_AsyncOperations
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_importmap
                /// IsCustomizable                             False                          False
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
                ///         (English - United States - 1033): System Job
                ///         (Russian - 1049): Системное задание
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): System Jobs
                ///         (Russian - 1049): Системные задания
                ///     
                ///     Description:
                ///         (English - United States - 1033): Process whose execution can proceed independently or in the background.
                ///         (Russian - 1049): Процесс, который может выполняться независимо или в фоновом режиме.
                ///</summary>
                public static partial class importmap_asyncoperations
                {
                    public const string Name = "ImportMap_AsyncOperations";

                    public const string ReferencedEntity_importmap = "importmap";

                    public const string ReferencedAttribute_importmapid = "importmapid";

                    public const string ReferencingEntity_asyncoperation = "asyncoperation";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship ImportMap_BulkDeleteFailures
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     ImportMap_BulkDeleteFailures
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_importmap
                /// IsCustomizable                             False                           False
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
                ///         (English - United States - 1033): Bulk Delete Failure
                ///         (Russian - 1049): Ошибка группового удаления
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Bulk Delete Failures
                ///         (Russian - 1049): Ошибки группового удаления
                ///     
                ///     Description:
                ///         (English - United States - 1033): Record that was not deleted during a bulk deletion job.
                ///         (Russian - 1049): Запись не была удалена во время задания группового удаления.
                ///</summary>
                public static partial class importmap_bulkdeletefailures
                {
                    public const string Name = "ImportMap_BulkDeleteFailures";

                    public const string ReferencedEntity_importmap = "importmap";

                    public const string ReferencedAttribute_importmapid = "importmapid";

                    public const string ReferencingEntity_bulkdeletefailure = "bulkdeletefailure";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";
                }

                ///<summary>
                /// 1:N - Relationship ImportMap_ImportFile
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     ImportMap_ImportFile
                /// ReferencingEntityNavigationPropertyName    importmapid
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
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
                /// ReferencingEntity importfile:
                ///     DisplayName:
                ///         (English - United States - 1033): Import Source File
                ///         (Russian - 1049): Файл источника импорта
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Imports
                ///         (Russian - 1049): Импорт
                ///     
                ///     Description:
                ///         (English - United States - 1033): File name of file used for import.
                ///         (Russian - 1049): Имя файла, используемого для импорта.
                ///</summary>
                public static partial class importmap_importfile
                {
                    public const string Name = "ImportMap_ImportFile";

                    public const string ReferencedEntity_importmap = "importmap";

                    public const string ReferencedAttribute_importmapid = "importmapid";

                    public const string ReferencingEntity_importfile = "importfile";

                    public const string ReferencingAttribute_importmapid = "importmapid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship ImportMap_SyncErrors
                /// 
                /// PropertyName                               Value                                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     ImportMap_SyncErrors
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_importmap_syncerror
                /// IsCustomizable                             True                                     False
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
                ///         (English - United States - 1033): Sync Error
                ///         (Russian - 1049): Ошибка синхронизации
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Sync Errors
                ///         (Russian - 1049): Ошибки синхронизации
                ///     
                ///     Description:
                ///         (English - United States - 1033): Failure reason and other detailed information for a record that failed to sync.
                ///         (Russian - 1049): Причина сбоя и другие подробные сведения о записи, при синхронизации которой произошла ошибка.
                ///</summary>
                public static partial class importmap_syncerrors
                {
                    public const string Name = "ImportMap_SyncErrors";

                    public const string ReferencedEntity_importmap = "importmap";

                    public const string ReferencedAttribute_importmapid = "importmapid";

                    public const string ReferencingEntity_syncerror = "syncerror";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship OwnerMapping_ImportMap
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     OwnerMapping_ImportMap
                /// ReferencingEntityNavigationPropertyName    importmapid
                /// IsCustomizable                             False                     False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
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
                /// ReferencingEntity ownermapping:
                ///     DisplayName:
                ///         (English - United States - 1033): Owner Mapping
                ///         (Russian - 1049): Сопоставление ответственного
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Owner Mappings
                ///         (Russian - 1049): Сопоставления ответственного
                ///     
                ///     Description:
                ///         (English - United States - 1033): In a data map, maps ownership data from the source file to Microsoft Dynamics 365.
                ///         (Russian - 1049): Сопоставление данных о типе собственности в исходном файле сопоставления данных с Microsoft Dynamics 365.
                ///</summary>
                public static partial class ownermapping_importmap
                {
                    public const string Name = "OwnerMapping_ImportMap";

                    public const string ReferencedEntity_importmap = "importmap";

                    public const string ReferencedAttribute_importmapid = "importmapid";

                    public const string ReferencingEntity_ownermapping = "ownermapping";

                    public const string ReferencingAttribute_importmapid = "importmapid";
                }

                ///<summary>
                /// 1:N - Relationship TransformationMapping_ImportMap
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     TransformationMapping_ImportMap
                /// ReferencingEntityNavigationPropertyName    importmapid
                /// IsCustomizable                             False                              False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
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
                /// ReferencingEntity transformationmapping:
                ///     DisplayName:
                ///         (English - United States - 1033): Transformation Mapping
                ///         (Russian - 1049): Сопоставление преобразования
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Transformation Mappings
                ///         (Russian - 1049): Сопоставления преобразований
                ///     
                ///     Description:
                ///         (English - United States - 1033): In a data map, maps the transformation of source attributes to Microsoft Dynamics 365 attributes.
                ///         (Russian - 1049): Сопоставление преобразования исходных атрибутов сопоставления данных с атрибутами Microsoft Dynamics 365.
                ///</summary>
                public static partial class transformationmapping_importmap
                {
                    public const string Name = "TransformationMapping_ImportMap";

                    public const string ReferencedEntity_importmap = "importmap";

                    public const string ReferencedAttribute_importmapid = "importmapid";

                    public const string ReferencingEntity_transformationmapping = "transformationmapping";

                    public const string ReferencingAttribute_importmapid = "importmapid";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_importmap
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_importmap
                /// ReferencingEntityNavigationPropertyName    objectid_importmap
                /// IsCustomizable                             False                               False
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
                public static partial class userentityinstancedata_importmap
                {
                    public const string Name = "userentityinstancedata_importmap";

                    public const string ReferencedEntity_importmap = "importmap";

                    public const string ReferencedAttribute_importmapid = "importmapid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}