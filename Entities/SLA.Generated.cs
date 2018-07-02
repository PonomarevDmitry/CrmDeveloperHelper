
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): SLA
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): SLAs
    /// 
    /// Description:
    /// (English - United States - 1033): Contains information about the tracked service-level KPIs for cases that belong to different customers.
    /// (Russian - 1049): Содержит информацию об отслеживаемых ключевых индикаторах уровня обслуживания (KPI) для обращений, принадлежащих разным клиентам.
    /// 
    /// PropertyName                          Value        CanBeChanged
    /// ActivityTypeMask                      1
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False        False
    /// CanBePrimaryEntityInRelationship      False        False
    /// CanBeRelatedEntityInRelationship      False        False
    /// CanChangeHierarchicalRelationship     False        False
    /// CanChangeTrackingBeEnabled            True         True
    /// CanCreateAttributes                   False        False
    /// CanCreateCharts                       False        False
    /// CanCreateForms                        False        False
    /// CanCreateViews                        True         False
    /// CanEnableSyncToExternalSearchIndex    False        False
    /// CanModifyAdditionalSettings           False        True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 False
    /// CollectionSchemaName                  SLAs
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         slas
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          True
    /// IsAuditEnabled                        False        True
    /// IsAvailableOffline                    False
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False        False
    /// IsCustomEntity                        False
    /// IsCustomizable                        True         False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False        False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False        False
    /// IsMappable                            True         False
    /// IsOfflineInMobileClient               False        True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        False
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  False
    /// IsReadOnlyInMobileClient              False        True
    /// IsRenameable                          False        False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                True
    /// IsValidForQueue                       False        False
    /// IsVisibleInMobile                     False        False
    /// IsVisibleInMobileClient               False        False
    /// LogicalCollectionName                 slas
    /// LogicalName                           sla
    /// ObjectTypeCode                        9750
    /// OwnershipType                         UserOwned
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName
    /// SchemaName                            SLA
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class SLA
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "sla";

            public const string EntitySchemaName = "SLA";

            public const string EntityPrimaryNameAttribute = "name";

            public const string EntityPrimaryIdAttribute = "slaid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Allow Pause and Resume
                ///     (Russian - 1049): Разрешать приостановку и возобновление
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether this SLA will allow pausing and resuming during the time calculation.
                ///     (Russian - 1049): Укажите, будут ли для этого SLA при расчете времени разрешены приостановка и возобновление.
                /// 
                /// SchemaName: AllowPauseResume
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Do Not Allow
                ///     (Russian - 1049): Запретить
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Allow
                ///     (Russian - 1049): Разрешить
                /// TrueOption = 1
                ///</summary>
                public const string allowpauseresume = "allowpauseresume";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Applicable From
                ///     (Russian - 1049): Применимо из
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the field that specifies the date and time from which the SLA items will be calculated for the case record. For example, if you select the Case Created On field, SLA calculation will begin from the time the case is created. 
                ///     (Russian - 1049): Укажите поле, задающее дату и время, от которых будут рассчитываться элементы SLA для записи обращения. Например, если выбрано поле "Обращение, созданное от", расчет SLA начнется с момента создания обращения. 
                /// 
                /// SchemaName: ApplicableFrom
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string applicablefrom = "applicablefrom";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Applicable From
                ///     (Russian - 1049): Применимо из
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the field that specifies the date and time from which the SLA items will be calculated. For example, if you select the Case Created On field, SLA calculation will begin from the time the case is created.
                ///     (Russian - 1049): Укажите поле, задающее дату и время, от которых будут рассчитываться элементы SLA. Например, если выбрано поле "Обращение, созданное от", расчет SLA начнется с момента создания обращения.
                /// 
                /// SchemaName: ApplicableFromPickList
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet slabase_applicablefrom
                /// DefaultFormValue = -1
                ///</summary>
                public const string applicablefrompicklist = "applicablefrompicklist";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Business Hours
                ///     (Russian - 1049): Рабочие часы
                /// 
                /// Description:
                ///     (English - United States - 1033): Choose the business hours for calculating SLA item timelines.
                ///     (Russian - 1049): Выберите рабочие часы для расчета конкретных дат элемента SLA.
                /// 
                /// SchemaName: BusinessHoursId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: calendar
                /// 
                ///     Target calendar    PrimaryIdAttribute calendarid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Calendar
                ///         (Russian - 1049): Календарь
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Calendars
                ///         (Russian - 1049): Календари
                ///         
                ///         Description:
                ///         (English - United States - 1033): Calendar used by the scheduling system to define when an appointment or activity is to occur.
                ///         (Russian - 1049): Календарь, используемый системой планирования для определения времени проведения встречи или выполнения действия.
                ///</summary>
                public const string businesshoursid = "businesshoursid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): ChangedAttributeList
                /// 
                /// Description:
                ///     (English - United States - 1033): Type additional information to describe the SLA
                ///     (Russian - 1049): Введите дополнительные сведения, описывающие SLA
                /// 
                /// SchemaName: ChangedAttributeList
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1000
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string changedattributelist = "changedattributelist";

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
                ///     (English - United States - 1033): Description
                ///     (Russian - 1049): Описание
                /// 
                /// Description:
                ///     (English - United States - 1033): Type additional information to describe the SLA
                ///     (Russian - 1049): Введите дополнительные сведения, описывающие SLA
                /// 
                /// SchemaName: Description
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string description = "description";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Exchange Rate
                ///     (Russian - 1049): Валютный курс
                /// 
                /// Description:
                ///     (English - United States - 1033): Exchange rate between the currency associated with the SLA record and the base currency.
                ///     (Russian - 1049): Обменный курс валюты, связанной с записью SLA, по отношению к базовой валюте.
                /// 
                /// SchemaName: ExchangeRate
                /// DecimalAttributeMetadata    AttributeType: Decimal    AttributeTypeName: DecimalType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0,0000000001    MaxValue = 100000000000    Precision = 10
                /// ImeMode = Disabled
                ///</summary>
                public const string exchangerate = "exchangerate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Default
                ///     (Russian - 1049): По умолчанию
                /// 
                /// Description:
                ///     (English - United States - 1033): Tells whether this SLA is the default one.
                ///     (Russian - 1049): Сообщает, используется ли данное SLA по умолчанию.
                /// 
                /// SchemaName: IsDefault
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = True
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
                ///     (English - United States - 1033): Is Managed
                ///     (Russian - 1049): Управляемый
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
                ///     (English - United States - 1033): Modified By
                ///     (Russian - 1049): Изменено
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows who last updated the record.
                ///     (Russian - 1049): Показывает, кто последний обновил запись.
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
                ///     (English - United States - 1033): Shows who created the record on behalf of another user.
                ///     (Russian - 1049): Показывает, кто создал запись от имени другого пользователя.
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
                ///     (Russian - 1049): Имя
                /// 
                /// Description:
                ///     (English - United States - 1033): Type a descriptive name of the service level agreement (SLA).
                ///     (Russian - 1049): Введите описательное имя соглашения об уровнях обслуживания (SLA).
                /// 
                /// SchemaName: Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string name = "name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Object Type Code
                ///     (Russian - 1049): Код типа объекта
                /// 
                /// Description:
                ///     (English - United States - 1033): Choose the entity type that the SLA is defined.
                ///     (Russian - 1049): Выберите тип сущности, которой определено SLA.
                /// 
                /// SchemaName: ObjectTypeCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet sla_objecttypecode
                /// DefaultFormValue = -1
                ///</summary>
                public const string objecttypecode = "objecttypecode";

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
                ///     (English - United States - 1033): Owner
                ///     (Russian - 1049): Ответственный
                /// 
                /// Description:
                ///     (English - United States - 1033): Enter the user or team who owns the SLA. This field is updated every time the item is assigned to a different user.
                ///     (Russian - 1049): Введите пользователя или рабочую группу, которым принадлежит SLA. Это поле обновляется при каждом назначении элемента другому пользователю.
                /// 
                /// SchemaName: OwnerId
                /// LookupAttributeMetadata    AttributeType: Owner    AttributeTypeName: OwnerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                public const string ownerid = "ownerid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Owning Business Unit
                ///     (Russian - 1049): Ответственное подразделение
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier for the business unit that owns the record
                ///     (Russian - 1049): Уникальный идентификатор подразделения, владеющего этой записью
                /// 
                /// SchemaName: OwningBusinessUnit
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = True
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
                public const string owningbusinessunit = "owningbusinessunit";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Owning Team
                ///     (Russian - 1049): Ответственная рабочая группа
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier for the team that owns the record.
                ///     (Russian - 1049): Уникальный идентификатор группы, которой принадлежит запись.
                /// 
                /// SchemaName: OwningTeam
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: team
                /// 
                ///     Target team    PrimaryIdAttribute teamid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Team
                ///         (Russian - 1049): Рабочая группа
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Teams
                ///         (Russian - 1049): Рабочие группы
                ///         
                ///         Description:
                ///         (English - United States - 1033): Collection of system users that routinely collaborate. Teams can be used to simplify record sharing and provide team members with common access to organization data when team members belong to different Business Units.
                ///         (Russian - 1049): Набор системных пользователей, которые обычно участвуют в совместной работе. Рабочие группы можно использовать для упрощения предоставления участникам группы общего доступа к записям и данным организации, если участники принадлежат к разным подразделениям.
                ///</summary>
                public const string owningteam = "owningteam";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Owning User
                ///     (Russian - 1049): Ответственный пользователь
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier for the user that owns the record.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, которому принадлежит эта запись.
                /// 
                /// SchemaName: OwningUser
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
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
                public const string owninguser = "owninguser";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Primary Entity
                ///     (Russian - 1049): Основная сущность
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the primary entity that the SLA has been created for.
                ///     (Russian - 1049): Показывает основную сущность, для которой было создано SLA.
                /// 
                /// SchemaName: PrimaryEntityOTC
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string primaryentityotc = "primaryentityotc";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): SLA
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the SLA.
                ///     (Russian - 1049): Уникальный идентификатор SLA.
                /// 
                /// SchemaName: SLAId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string slaid = "slaid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Unique Id
                ///     (Russian - 1049): Уникальный идентификатор
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: SLAIdUnique
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string slaidunique = "slaidunique";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): SLA Type
                ///     (Russian - 1049): Тип SLA
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the type of service level agreement (SLA).
                ///     (Russian - 1049): Выберите тип соглашения об уровне обслуживания (SLA).
                /// 
                /// SchemaName: SLAType
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet sla_slatype
                /// DefaultFormValue = 0
                ///</summary>
                public const string slatype = "slatype";

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
                ///     (English - United States - 1033): Status
                ///     (Russian - 1049): Состояние
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows whether the Service Level Agreement (SLA) is active or inactive.
                ///     (Russian - 1049): Указывает, активно или неактивно SLA.
                /// 
                /// SchemaName: StateCode
                /// StateAttributeMetadata    AttributeType: State    AttributeTypeName: StateType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                ///     (English - United States - 1033): Select the status of the service level agreement (SLA).
                ///     (Russian - 1049): Укажите состояние соглашения об уровне обслуживания (SLA).
                /// 
                /// SchemaName: StatusCode
                /// StatusAttributeMetadata    AttributeType: Status    AttributeTypeName: StatusType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// DefaultFormValue = 1
                ///</summary>
                public const string statuscode = "statuscode";

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
                ///     (English - United States - 1033): Currency
                ///     (Russian - 1049): Валюта
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the currency associated with the SLA record.
                ///     (Russian - 1049): Уникальный идентификатор валюты, связанной с записью SLA.
                /// 
                /// SchemaName: TransactionCurrencyId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: transactioncurrency
                /// 
                ///     Target transactioncurrency    PrimaryIdAttribute transactioncurrencyid    PrimaryNameAttribute currencyname
                ///         DisplayName:
                ///         (English - United States - 1033): Currency
                ///         (Russian - 1049): Валюта
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Currencies
                ///         (Russian - 1049): Валюты
                ///         
                ///         Description:
                ///         (English - United States - 1033): Currency in which a financial transaction is carried out.
                ///         (Russian - 1049): Валюта, в которой выполняется финансовая операция.
                ///</summary>
                public const string transactioncurrencyid = "transactioncurrencyid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Version Number
                ///     (Russian - 1049): Номер версии
                /// 
                /// Description:
                ///     (English - United States - 1033): Version number of the SLA.
                ///     (Russian - 1049): Номер версии SLA.
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
                ///     (English - United States - 1033): Workflow ID
                ///     (Russian - 1049): Идентификатор бизнес-процесса
                /// 
                /// Description:
                ///     (English - United States - 1033): Workflow associated with the SLA.
                ///     (Russian - 1049): Бизнес-процесс, связанный с SLA.
                /// 
                /// SchemaName: WorkflowId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: workflow
                /// 
                ///     Target workflow    PrimaryIdAttribute workflowid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Process
                ///         (Russian - 1049): Процесс
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Processes
                ///         (Russian - 1049): Процессы
                ///         
                ///         Description:
                ///         (English - United States - 1033): Set of logical rules that define the steps necessary to automate a specific business process, task, or set of actions to be performed.
                ///         (Russian - 1049): Задайте логические правила, определяющие необходимые действия для автоматизации конкретных бизнес-процессов, заданий или наборов действий.
                ///</summary>
                public const string workflowid = "workflowid";
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
                ///     (English - United States - 1033): Shows whether the Service Level Agreement (SLA) is active or inactive.
                ///     (Russian - 1049): Указывает, активно или неактивно SLA.
                ///</summary>
                public enum statecode
                {
                    ///<summary>
                    /// Default statuscode: Draft_1, 1
                    /// InvariantName: Draft
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Draft
                    ///     (Russian - 1049): Черновик
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Draft_0 = 0,

                    ///<summary>
                    /// Default statuscode: Active_2, 2
                    /// InvariantName: Active
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Active
                    ///     (Russian - 1049): Активный
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Active_1 = 1,
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
                ///     (English - United States - 1033): Shows whether the Service Level Agreement (SLA) is active or inactive.
                ///     (Russian - 1049): Указывает, активно или неактивно SLA.
                ///</summary>
                public enum statuscode
                {
                    ///<summary>
                    /// Linked Statecode: Draft_0, 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Draft
                    ///     (Russian - 1049): Черновик
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Draft_0_Draft_1 = 1,

                    ///<summary>
                    /// Linked Statecode: Active_1, 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Active
                    ///     (Russian - 1049): Активный
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Active_1_Active_2 = 2,
                }

                #endregion State and Status OptionSets.

                #region Picklist OptionSet OptionSets.

                ///<summary>
                /// Attribute: applicablefrompicklist
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Applicable From
                ///     (Russian - 1049): Применимо из
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the field that specifies the date and time from which the SLA items will be calculated. For example, if you select the Case Created On field, SLA calculation will begin from the time the case is created.
                ///     (Russian - 1049): Укажите поле, задающее дату и время, от которых будут рассчитываться элементы SLA. Например, если выбрано поле "Обращение, созданное от", расчет SLA начнется с момента создания обращения.
                /// 
                /// Local System  OptionSet slabase_applicablefrom
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Date Attributes of case
                ///     (Russian - 1049): Атрибуты даты обращения
                /// 
                /// Description:
                ///     (English - United States - 1033): Date Attributes of Case
                ///</summary>
                public enum applicablefrompicklist
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): No
                    ///     (Russian - 1049): Нет
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    No_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Yes
                    ///     (Russian - 1049): Да
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Yes_2 = 2,
                }

                ///<summary>
                /// Attribute: objecttypecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Object Type Code
                ///     (Russian - 1049): Код типа объекта
                /// 
                /// Description:
                ///     (English - United States - 1033): Choose the entity type that the SLA is defined.
                ///     (Russian - 1049): Выберите тип сущности, которой определено SLA.
                /// 
                /// Local System  OptionSet sla_objecttypecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Object Type Code
                ///     (Russian - 1049): Код типа объекта
                /// 
                /// Description:
                ///     (English - United States - 1033): The entity the SLA is applicable from.
                ///     (Russian - 1049): Сущность, из которой применимо SLA.
                ///</summary>
                //public enum objecttypecode

                ///<summary>
                /// Attribute: slatype
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): SLA Type
                ///     (Russian - 1049): Тип SLA
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the type of service level agreement (SLA).
                ///     (Russian - 1049): Выберите тип соглашения об уровне обслуживания (SLA).
                /// 
                /// Local System  OptionSet sla_slatype
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): SLA Type
                ///     (Russian - 1049): Тип SLA
                /// 
                /// Description:
                ///     (English - United States - 1033): Tells whether whether SLA KPI Instances will be used for saving SLA status and failure time.
                ///</summary>
                public enum slatype
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Standard
                    ///     (Russian - 1049): Стандартный
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Standard_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Enhanced
                    ///     (Russian - 1049): Улучшенный
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Enhanced_1 = 1,
                }

                #endregion Picklist OptionSets.
            }

            #endregion OptionSets.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship business_unit_slabase
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_slabase
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                     False
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
                ///</summary>
                public static partial class business_unit_slabase
                {
                    public const string Name = "business_unit_slabase";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_sla = "sla";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";
                }

                ///<summary>
                /// N:1 - Relationship lk_slabase_createdby
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_slabase_createdby
                /// ReferencingEntityNavigationPropertyName    createdby
                /// IsCustomizable                             True                     False
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
                public static partial class lk_slabase_createdby
                {
                    public const string Name = "lk_slabase_createdby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_sla = "sla";

                    public const string ReferencingAttribute_createdby = "createdby";
                }

                ///<summary>
                /// N:1 - Relationship lk_slabase_createdonbehalfby
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_slabase_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    createdonbehalfby
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
                public static partial class lk_slabase_createdonbehalfby
                {
                    public const string Name = "lk_slabase_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_sla = "sla";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_slabase_modifiedby
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_slabase_modifiedby
                /// ReferencingEntityNavigationPropertyName    modifiedby
                /// IsCustomizable                             True                     False
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
                public static partial class lk_slabase_modifiedby
                {
                    public const string Name = "lk_slabase_modifiedby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_sla = "sla";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";
                }

                ///<summary>
                /// N:1 - Relationship lk_slabase_modifiedonbehalfby
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_slabase_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    modifiedonbehalfby
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
                public static partial class lk_slabase_modifiedonbehalfby
                {
                    public const string Name = "lk_slabase_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_sla = "sla";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship owner_slas
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     owner_slas
                /// ReferencingEntityNavigationPropertyName    ownerid
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
                /// ReferencedEntity owner:
                ///     DisplayName:
                ///     (English - United States - 1033): Owner
                ///     (Russian - 1049): Ответственный
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Owners
                ///     (Russian - 1049): Ответственные
                ///     
                ///     Description:
                ///     (English - United States - 1033): Group of undeleted system users and undeleted teams. Owners can be used to control access to specific objects.
                ///     (Russian - 1049): Группа для восстановленных системных пользователей и рабочих групп. Для контроля доступа к конкретным объектам можно использовать ответственных.
                ///</summary>
                public static partial class owner_slas
                {
                    public const string Name = "owner_slas";

                    public const string ReferencedEntity_owner = "owner";

                    public const string ReferencedAttribute_ownerid = "ownerid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_sla = "sla";

                    public const string ReferencingAttribute_ownerid = "ownerid";
                }

                ///<summary>
                /// N:1 - Relationship slabase_businesshoursid
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     slabase_businesshoursid
                /// ReferencingEntityNavigationPropertyName    businesshoursid
                /// IsCustomizable                             False                      False
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
                /// ReferencedEntity calendar:
                ///     DisplayName:
                ///     (English - United States - 1033): Calendar
                ///     (Russian - 1049): Календарь
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Calendars
                ///     (Russian - 1049): Календари
                ///     
                ///     Description:
                ///     (English - United States - 1033): Calendar used by the scheduling system to define when an appointment or activity is to occur.
                ///     (Russian - 1049): Календарь, используемый системой планирования для определения времени проведения встречи или выполнения действия.
                ///</summary>
                public static partial class slabase_businesshoursid
                {
                    public const string Name = "slabase_businesshoursid";

                    public const string ReferencedEntity_calendar = "calendar";

                    public const string ReferencedAttribute_calendarid = "calendarid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_sla = "sla";

                    public const string ReferencingAttribute_businesshoursid = "businesshoursid";
                }

                ///<summary>
                /// N:1 - Relationship slabase_workflowid
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     slabase_workflowid
                /// ReferencingEntityNavigationPropertyName    workflowid
                /// IsCustomizable                             True                     False
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
                /// ReferencedEntity workflow:
                ///     DisplayName:
                ///     (English - United States - 1033): Process
                ///     (Russian - 1049): Процесс
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Processes
                ///     (Russian - 1049): Процессы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Set of logical rules that define the steps necessary to automate a specific business process, task, or set of actions to be performed.
                ///     (Russian - 1049): Задайте логические правила, определяющие необходимые действия для автоматизации конкретных бизнес-процессов, заданий или наборов действий.
                ///</summary>
                public static partial class slabase_workflowid
                {
                    public const string Name = "slabase_workflowid";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_sla = "sla";

                    public const string ReferencingAttribute_workflowid = "workflowid";
                }

                ///<summary>
                /// N:1 - Relationship team_slaBase
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_slaBase
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
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
                /// ReferencedEntity team:
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
                public static partial class team_slabase
                {
                    public const string Name = "team_slaBase";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_sla = "sla";

                    public const string ReferencingAttribute_owningteam = "owningteam";
                }

                ///<summary>
                /// N:1 - Relationship TransactionCurrency_SLA
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     TransactionCurrency_SLA
                /// ReferencingEntityNavigationPropertyName    transactioncurrencyid
                /// IsCustomizable                             False                      False
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
                /// ReferencedEntity transactioncurrency:
                ///     DisplayName:
                ///     (English - United States - 1033): Currency
                ///     (Russian - 1049): Валюта
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Currencies
                ///     (Russian - 1049): Валюты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Currency in which a financial transaction is carried out.
                ///     (Russian - 1049): Валюта, в которой выполняется финансовая операция.
                ///</summary>
                public static partial class transactioncurrency_sla
                {
                    public const string Name = "TransactionCurrency_SLA";

                    public const string ReferencedEntity_transactioncurrency = "transactioncurrency";

                    public const string ReferencedAttribute_transactioncurrencyid = "transactioncurrencyid";

                    public const string ReferencedEntity_PrimaryNameAttribute_currencyname = "currencyname";

                    public const string ReferencingEntity_sla = "sla";

                    public const string ReferencingAttribute_transactioncurrencyid = "transactioncurrencyid";
                }

                ///<summary>
                /// N:1 - Relationship user_slabase
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     user_slabase
                /// ReferencingEntityNavigationPropertyName    owninguser
                /// IsCustomizable                             True                     False
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
                public static partial class user_slabase
                {
                    public const string Name = "user_slabase";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_sla = "sla";

                    public const string ReferencingAttribute_owninguser = "owninguser";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship cdi_txtmessage_sla_slaid
                /// 
                /// PropertyName                               Value                                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     cdi_txtmessage_sla_slaid
                /// ReferencingEntityNavigationPropertyName    sla_activitypointer_sla_cdi_txtmessage
                /// IsCustomizable                             True                                      True
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
                /// ReferencingEntity cdi_txtmessage:
                ///     DisplayName:
                ///     (English - United States - 1033): Text Message
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Text Messages
                ///</summary>
                public static partial class cdi_txtmessage_sla_slaid
                {
                    public const string Name = "cdi_txtmessage_sla_slaid";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_cdi_txtmessage = "cdi_txtmessage";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship cdi_txtmessage_sla_slainvokedid
                /// 
                /// PropertyName                               Value                                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     cdi_txtmessage_sla_slainvokedid
                /// ReferencingEntityNavigationPropertyName    slainvokedid_activitypointer_sla_cdi_txtmessage
                /// IsCustomizable                             True                                               True
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
                /// ReferencingEntity cdi_txtmessage:
                ///     DisplayName:
                ///     (English - United States - 1033): Text Message
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Text Messages
                ///</summary>
                public static partial class cdi_txtmessage_sla_slainvokedid
                {
                    public const string Name = "cdi_txtmessage_sla_slainvokedid";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_cdi_txtmessage = "cdi_txtmessage";

                    public const string ReferencingAttribute_slainvokedid = "slainvokedid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship manualsla_account
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     manualsla_account
                /// ReferencingEntityNavigationPropertyName    sla_account_sla
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity account:
                ///     DisplayName:
                ///     (English - United States - 1033): Account
                ///     (Russian - 1049): Организация
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Accounts
                ///     (Russian - 1049): Организации
                ///     
                ///     Description:
                ///     (English - United States - 1033): Business that represents a customer or potential customer. The company that is billed in business transactions.
                ///     (Russian - 1049): Компания, представляющая существующего или потенциального клиента. Компания, которой выставляется счет в деловых транзакциях.
                ///</summary>
                public static partial class manualsla_account
                {
                    public const string Name = "manualsla_account";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship manualsla_activitypointer
                /// 
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     manualsla_activitypointer
                /// ReferencingEntityNavigationPropertyName    sla_activitypointer_sla
                /// IsCustomizable                             True                         False
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
                /// ReferencingEntity activitypointer:
                ///     DisplayName:
                ///     (English - United States - 1033): Activity
                ///     (Russian - 1049): Действие
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Activities
                ///     (Russian - 1049): Действия
                ///     
                ///     Description:
                ///     (English - United States - 1033): Task performed, or to be performed, by a user. An activity is any action for which an entry can be made on a calendar.
                ///     (Russian - 1049): Выполненная пользователем задача или задача, которую пользователь должен выполнить. "Действие" — это любое действие, для которого в календаре можно создать запись.
                ///</summary>
                public static partial class manualsla_activitypointer
                {
                    public const string Name = "manualsla_activitypointer";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_activitypointer = "activitypointer";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship manualsla_appointment
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     manualsla_appointment
                /// ReferencingEntityNavigationPropertyName    sla_appointment_sla
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity appointment:
                ///     DisplayName:
                ///     (English - United States - 1033): Appointment
                ///     (Russian - 1049): Встреча
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Appointments
                ///     (Russian - 1049): Встречи
                ///     
                ///     Description:
                ///     (English - United States - 1033): Commitment representing a time interval with start/end times and duration.
                ///     (Russian - 1049): Обязательство, представляющее временной интервал с временем начала и окончания, а также длительностью.
                ///</summary>
                public static partial class manualsla_appointment
                {
                    public const string Name = "manualsla_appointment";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_appointment = "appointment";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship manualsla_cases
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     manualsla_cases
                /// ReferencingEntityNavigationPropertyName    slaid_sla
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity incident:
                ///     DisplayName:
                ///     (English - United States - 1033): Case
                ///     (Russian - 1049): Обращение
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Cases
                ///     (Russian - 1049): Обращения
                ///     
                ///     Description:
                ///     (English - United States - 1033): Service request case associated with a contract.
                ///     (Russian - 1049): Обращение по запросу на обслуживание, связанному с контрактом.
                ///</summary>
                public static partial class manualsla_cases
                {
                    public const string Name = "manualsla_cases";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_incident = "incident";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship manualsla_contact
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     manualsla_contact
                /// ReferencingEntityNavigationPropertyName    sla_contact_sla
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity contact:
                ///     DisplayName:
                ///     (English - United States - 1033): Person
                ///     (Russian - 1049): Контакт
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Persons
                ///     (Russian - 1049): Контакты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Person with whom a business unit has a relationship, such as customer, supplier, and colleague.
                ///     (Russian - 1049): Лицо, с которым подразделение состоит в отношениях (например, клиент, поставщик, коллега).
                ///</summary>
                public static partial class manualsla_contact
                {
                    public const string Name = "manualsla_contact";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_contact = "contact";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// 1:N - Relationship manualsla_email
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     manualsla_email
                /// ReferencingEntityNavigationPropertyName    sla_email_sla
                /// IsCustomizable                             True                     False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          10000
                /// 
                /// ReferencingEntity email:
                ///     DisplayName:
                ///     (English - United States - 1033): Email
                ///     (Russian - 1049): Электронная почта
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Email Messages
                ///     (Russian - 1049): Сообщения электронной почты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity that is delivered using email protocols.
                ///     (Russian - 1049): Действие, передаваемое с помощью протоколов электронной почты.
                ///</summary>
                public static partial class manualsla_email
                {
                    public const string Name = "manualsla_email";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_email = "email";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship manualsla_fax
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     manualsla_fax
                /// ReferencingEntityNavigationPropertyName    sla_fax_sla
                /// IsCustomizable                             True                     False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          10001
                /// 
                /// ReferencingEntity fax:
                ///     DisplayName:
                ///     (English - United States - 1033): Fax
                ///     (Russian - 1049): Факс
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Faxes
                ///     (Russian - 1049): Факсы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity that tracks call outcome and number of pages for a fax and optionally stores an electronic copy of the document.
                ///     (Russian - 1049): Действие, отслеживающее результаты звонков и число страниц в факсе и дополнительно хранящее электронную копию документа.
                ///</summary>
                public static partial class manualsla_fax
                {
                    public const string Name = "manualsla_fax";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_fax = "fax";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship manualsla_invoice
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     manualsla_invoice
                /// ReferencingEntityNavigationPropertyName    sla_invoice_sla
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity invoice:
                ///     DisplayName:
                ///     (English - United States - 1033): Invoice
                ///     (Russian - 1049): Счет
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Invoices
                ///     (Russian - 1049): Счета
                ///     
                ///     Description:
                ///     (English - United States - 1033): Order that has been billed.
                ///     (Russian - 1049): Заказ, по которому был выставлен счет.
                ///</summary>
                public static partial class manualsla_invoice
                {
                    public const string Name = "manualsla_invoice";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_invoice = "invoice";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship manualsla_lead
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     manualsla_lead
                /// ReferencingEntityNavigationPropertyName    sla_lead_sla
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity lead:
                ///     DisplayName:
                ///     (English - United States - 1033): Lead
                ///     (Russian - 1049): Интерес
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Leads
                ///     (Russian - 1049): Интересы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Prospect or potential sales opportunity. Leads are converted into accounts, contacts, or opportunities when they are qualified. Otherwise, they are deleted or archived.
                ///     (Russian - 1049): Перспективный клиент или потенциальная сделка. Интересы преобразуются в организации, контакты или возможные сделки после их оценки. В противном случае они удаляются или архивируются.
                ///</summary>
                public static partial class manualsla_lead
                {
                    public const string Name = "manualsla_lead";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_lead = "lead";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// 1:N - Relationship manualsla_letter
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     manualsla_letter
                /// ReferencingEntityNavigationPropertyName    sla_letter_sla
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity letter:
                ///     DisplayName:
                ///     (English - United States - 1033): Letter
                ///     (Russian - 1049): Письмо
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Letters
                ///     (Russian - 1049): Письма
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity that tracks the delivery of a letter. The activity can contain the electronic copy of the letter.
                ///     (Russian - 1049): Действие, отслеживающее доставку письма. Действие может содержать электронную копию письма.
                ///</summary>
                public static partial class manualsla_letter
                {
                    public const string Name = "manualsla_letter";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_letter = "letter";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship manualsla_opportunity
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     manualsla_opportunity
                /// ReferencingEntityNavigationPropertyName    sla_opportunity_sla
                /// IsCustomizable                             True                     False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          10000
                /// 
                /// ReferencingEntity opportunity:
                ///     DisplayName:
                ///     (English - United States - 1033): Opportunity
                ///     (Russian - 1049): Возможная сделка
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Opportunities
                ///     (Russian - 1049): Возможные сделки
                ///     
                ///     Description:
                ///     (English - United States - 1033): Potential revenue-generating event, or sale to an account, which needs to be tracked through a sales process to completion.
                ///     (Russian - 1049): Событие, потенциально создающее прибыль, или продажа организации, которая должна отслеживаться в процессе продажи до завершения.
                ///</summary>
                public static partial class manualsla_opportunity
                {
                    public const string Name = "manualsla_opportunity";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_opportunity = "opportunity";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship manualsla_phonecall
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     manualsla_phonecall
                /// ReferencingEntityNavigationPropertyName    sla_phonecall_sla
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity phonecall:
                ///     DisplayName:
                ///     (English - United States - 1033): Phone Call
                ///     (Russian - 1049): Звонок
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Phone Calls
                ///     (Russian - 1049): Звонки
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity to track a telephone call.
                ///     (Russian - 1049): Действие для отслеживания телефонного звонка.
                ///</summary>
                public static partial class manualsla_phonecall
                {
                    public const string Name = "manualsla_phonecall";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_phonecall = "phonecall";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship manualsla_quote
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     manualsla_quote
                /// ReferencingEntityNavigationPropertyName    sla_quote_sla
                /// IsCustomizable                             True                     False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          10000
                /// 
                /// ReferencingEntity quote:
                ///     DisplayName:
                ///     (English - United States - 1033): Quote
                ///     (Russian - 1049): Предложение с расценками
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Quotes
                ///     (Russian - 1049): Предложения с расценками
                ///     
                ///     Description:
                ///     (English - United States - 1033): Formal offer for products and/or services, proposed at specific prices and related payment terms, which is sent to a prospective customer.
                ///     (Russian - 1049): Формальное предложение продуктов и (или) услуг с конкретными ценами и условиями оплаты, отправляемое потенциальным клиентам.
                ///</summary>
                public static partial class manualsla_quote
                {
                    public const string Name = "manualsla_quote";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_quote = "quote";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship manualsla_salesorder
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     manualsla_salesorder
                /// ReferencingEntityNavigationPropertyName    sla_salesorder_sla
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity salesorder:
                ///     DisplayName:
                ///     (English - United States - 1033): Order
                ///     (Russian - 1049): Заказ
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Orders
                ///     (Russian - 1049): Заказы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Quote that has been accepted.
                ///     (Russian - 1049): Предложение с расценками принято.
                ///</summary>
                public static partial class manualsla_salesorder
                {
                    public const string Name = "manualsla_salesorder";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_salesorder = "salesorder";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship manualsla_serviceappointment
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     manualsla_serviceappointment
                /// ReferencingEntityNavigationPropertyName    SLAId
                /// IsCustomizable                             True                            False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          10000
                /// 
                /// ReferencingEntity serviceappointment:
                ///     DisplayName:
                ///     (English - United States - 1033): Service Activity
                ///     (Russian - 1049): Действие сервиса
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Service Activities
                ///     (Russian - 1049): Действия сервиса
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity offered by the organization to satisfy its customer's needs. Each service activity includes date, time, duration, and required resources.
                ///     (Russian - 1049): Действие, предлагаемое организацией с целью удовлетворить потребности клиента. Каждое действие сервиса включает дату, время, продолжительность и необходимые ресурсы.
                ///</summary>
                public static partial class manualsla_serviceappointment
                {
                    public const string Name = "manualsla_serviceappointment";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_serviceappointment = "serviceappointment";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship manualsla_socialactivity
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     manualsla_socialactivity
                /// ReferencingEntityNavigationPropertyName    sla_socialactivity_sla
                /// IsCustomizable                             True                        False
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
                /// ReferencingEntity socialactivity:
                ///     DisplayName:
                ///     (English - United States - 1033): Social Activity
                ///     (Russian - 1049): Действие социальной сети
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Social Activities
                ///     (Russian - 1049): Действия социальной сети
                ///     
                ///     Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                ///</summary>
                public static partial class manualsla_socialactivity
                {
                    public const string Name = "manualsla_socialactivity";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_socialactivity = "socialactivity";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship manualsla_task
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     manualsla_task
                /// ReferencingEntityNavigationPropertyName    sla_task_sla
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity task:
                ///     DisplayName:
                ///     (English - United States - 1033): Task
                ///     (Russian - 1049): Задача
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Tasks
                ///     (Russian - 1049): Задачи
                ///     
                ///     Description:
                ///     (English - United States - 1033): Generic activity representing work needed to be done.
                ///     (Russian - 1049): Универсальное действие, представляющее работу, которую необходимо выполнить.
                ///</summary>
                public static partial class manualsla_task
                {
                    public const string Name = "manualsla_task";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_task = "task";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship sla_account
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_account
                /// ReferencingEntityNavigationPropertyName    slainvokedid_account_sla
                /// IsCustomizable                             True                        False
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
                /// ReferencingEntity account:
                ///     DisplayName:
                ///     (English - United States - 1033): Account
                ///     (Russian - 1049): Организация
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Accounts
                ///     (Russian - 1049): Организации
                ///     
                ///     Description:
                ///     (English - United States - 1033): Business that represents a customer or potential customer. The company that is billed in business transactions.
                ///     (Russian - 1049): Компания, представляющая существующего или потенциального клиента. Компания, которой выставляется счет в деловых транзакциях.
                ///</summary>
                public static partial class sla_account
                {
                    public const string Name = "sla_account";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_slainvokedid = "slainvokedid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship sla_activitypointer
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_activitypointer
                /// ReferencingEntityNavigationPropertyName    slainvokedid_activitypointer_sla
                /// IsCustomizable                             True                                False
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
                /// ReferencingEntity activitypointer:
                ///     DisplayName:
                ///     (English - United States - 1033): Activity
                ///     (Russian - 1049): Действие
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Activities
                ///     (Russian - 1049): Действия
                ///     
                ///     Description:
                ///     (English - United States - 1033): Task performed, or to be performed, by a user. An activity is any action for which an entry can be made on a calendar.
                ///     (Russian - 1049): Выполненная пользователем задача или задача, которую пользователь должен выполнить. "Действие" — это любое действие, для которого в календаре можно создать запись.
                ///</summary>
                public static partial class sla_activitypointer
                {
                    public const string Name = "sla_activitypointer";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_activitypointer = "activitypointer";

                    public const string ReferencingAttribute_slainvokedid = "slainvokedid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship sla_Annotation
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_Annotation
                /// ReferencingEntityNavigationPropertyName    objectid_sla
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity annotation:
                ///     DisplayName:
                ///     (English - United States - 1033): Note
                ///     (Russian - 1049): Примечание
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Notes
                ///     (Russian - 1049): Примечания
                ///     
                ///     Description:
                ///     (English - United States - 1033): Note that is attached to one or more objects, including other notes.
                ///     (Russian - 1049): Примечание, которое прикреплено к одному или нескольким объектам, в том числе другим примечаниям.
                ///</summary>
                public static partial class sla_annotation
                {
                    public const string Name = "sla_Annotation";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_annotation = "annotation";

                    public const string ReferencingAttribute_objectid = "objectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship sla_appointment
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_appointment
                /// ReferencingEntityNavigationPropertyName    slainvokedid_appointment_sla
                /// IsCustomizable                             True                            False
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
                /// ReferencingEntity appointment:
                ///     DisplayName:
                ///     (English - United States - 1033): Appointment
                ///     (Russian - 1049): Встреча
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Appointments
                ///     (Russian - 1049): Встречи
                ///     
                ///     Description:
                ///     (English - United States - 1033): Commitment representing a time interval with start/end times and duration.
                ///     (Russian - 1049): Обязательство, представляющее временной интервал с временем начала и окончания, а также длительностью.
                ///</summary>
                public static partial class sla_appointment
                {
                    public const string Name = "sla_appointment";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_appointment = "appointment";

                    public const string ReferencingAttribute_slainvokedid = "slainvokedid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship sla_cases
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_cases
                /// ReferencingEntityNavigationPropertyName    slainvokedid_sla
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity incident:
                ///     DisplayName:
                ///     (English - United States - 1033): Case
                ///     (Russian - 1049): Обращение
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Cases
                ///     (Russian - 1049): Обращения
                ///     
                ///     Description:
                ///     (English - United States - 1033): Service request case associated with a contract.
                ///     (Russian - 1049): Обращение по запросу на обслуживание, связанному с контрактом.
                ///</summary>
                public static partial class sla_cases
                {
                    public const string Name = "sla_cases";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_incident = "incident";

                    public const string ReferencingAttribute_slainvokedid = "slainvokedid";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship sla_contact
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_contact
                /// ReferencingEntityNavigationPropertyName    slainvokedid_contact_sla
                /// IsCustomizable                             True                        False
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
                /// ReferencingEntity contact:
                ///     DisplayName:
                ///     (English - United States - 1033): Person
                ///     (Russian - 1049): Контакт
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Persons
                ///     (Russian - 1049): Контакты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Person with whom a business unit has a relationship, such as customer, supplier, and colleague.
                ///     (Russian - 1049): Лицо, с которым подразделение состоит в отношениях (например, клиент, поставщик, коллега).
                ///</summary>
                public static partial class sla_contact
                {
                    public const string Name = "sla_contact";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_contact = "contact";

                    public const string ReferencingAttribute_slainvokedid = "slainvokedid";

                    public const string ReferencingEntity_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// 1:N - Relationship sla_email
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_email
                /// ReferencingEntityNavigationPropertyName    slainvokedid_email_sla
                /// IsCustomizable                             True                      False
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
                /// ReferencingEntity email:
                ///     DisplayName:
                ///     (English - United States - 1033): Email
                ///     (Russian - 1049): Электронная почта
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Email Messages
                ///     (Russian - 1049): Сообщения электронной почты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity that is delivered using email protocols.
                ///     (Russian - 1049): Действие, передаваемое с помощью протоколов электронной почты.
                ///</summary>
                public static partial class sla_email
                {
                    public const string Name = "sla_email";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_email = "email";

                    public const string ReferencingAttribute_slainvokedid = "slainvokedid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship sla_entitlement
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_entitlement
                /// ReferencingEntityNavigationPropertyName    slaid
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity entitlement:
                ///     DisplayName:
                ///     (English - United States - 1033): Entitlement
                ///     (Russian - 1049): Объем обслуживания
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Entitlements
                ///     (Russian - 1049): Объемы обслуживания
                ///     
                ///     Description:
                ///     (English - United States - 1033): Defines the amount and type of support a customer should receive.
                ///     (Russian - 1049): Определяет объем и тип поддержки, которую должен получать клиент.
                ///</summary>
                public static partial class sla_entitlement
                {
                    public const string Name = "sla_entitlement";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_entitlement = "entitlement";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship sla_entitlementtemplate
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_entitlementtemplate
                /// ReferencingEntityNavigationPropertyName    slaid
                /// IsCustomizable                             True                       False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          10000
                /// 
                /// ReferencingEntity entitlementtemplate:
                ///     DisplayName:
                ///     (English - United States - 1033): Entitlement Template
                ///     (Russian - 1049): Шаблон объема обслуживания
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Entitlement Templates
                ///     (Russian - 1049): Шаблоны объемов обслуживания
                ///     
                ///     Description:
                ///     (English - United States - 1033): Contains predefined customer support terms that can be used to created entitlements for customers.
                ///     (Russian - 1049): Содержит предопределенные условия поддержки клиентов, которые могут использоваться для создания объемов обслуживания для клиентов.
                ///</summary>
                public static partial class sla_entitlementtemplate
                {
                    public const string Name = "sla_entitlementtemplate";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_entitlementtemplate = "entitlementtemplate";

                    public const string ReferencingAttribute_slaid = "slaid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship sla_fax
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_fax
                /// ReferencingEntityNavigationPropertyName    slainvokedid_fax_sla
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity fax:
                ///     DisplayName:
                ///     (English - United States - 1033): Fax
                ///     (Russian - 1049): Факс
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Faxes
                ///     (Russian - 1049): Факсы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity that tracks call outcome and number of pages for a fax and optionally stores an electronic copy of the document.
                ///     (Russian - 1049): Действие, отслеживающее результаты звонков и число страниц в факсе и дополнительно хранящее электронную копию документа.
                ///</summary>
                public static partial class sla_fax
                {
                    public const string Name = "sla_fax";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_fax = "fax";

                    public const string ReferencingAttribute_slainvokedid = "slainvokedid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship sla_invoice
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_invoice
                /// ReferencingEntityNavigationPropertyName    slainvokedid_invoice_sla
                /// IsCustomizable                             True                        False
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
                /// ReferencingEntity invoice:
                ///     DisplayName:
                ///     (English - United States - 1033): Invoice
                ///     (Russian - 1049): Счет
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Invoices
                ///     (Russian - 1049): Счета
                ///     
                ///     Description:
                ///     (English - United States - 1033): Order that has been billed.
                ///     (Russian - 1049): Заказ, по которому был выставлен счет.
                ///</summary>
                public static partial class sla_invoice
                {
                    public const string Name = "sla_invoice";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_invoice = "invoice";

                    public const string ReferencingAttribute_slainvokedid = "slainvokedid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship sla_lead
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_lead
                /// ReferencingEntityNavigationPropertyName    slainvokedid_lead_sla
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity lead:
                ///     DisplayName:
                ///     (English - United States - 1033): Lead
                ///     (Russian - 1049): Интерес
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Leads
                ///     (Russian - 1049): Интересы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Prospect or potential sales opportunity. Leads are converted into accounts, contacts, or opportunities when they are qualified. Otherwise, they are deleted or archived.
                ///     (Russian - 1049): Перспективный клиент или потенциальная сделка. Интересы преобразуются в организации, контакты или возможные сделки после их оценки. В противном случае они удаляются или архивируются.
                ///</summary>
                public static partial class sla_lead
                {
                    public const string Name = "sla_lead";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_lead = "lead";

                    public const string ReferencingAttribute_slainvokedid = "slainvokedid";

                    public const string ReferencingEntity_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// 1:N - Relationship sla_letter
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_letter
                /// ReferencingEntityNavigationPropertyName    slainvokedid_letter_sla
                /// IsCustomizable                             True                       False
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
                /// ReferencingEntity letter:
                ///     DisplayName:
                ///     (English - United States - 1033): Letter
                ///     (Russian - 1049): Письмо
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Letters
                ///     (Russian - 1049): Письма
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity that tracks the delivery of a letter. The activity can contain the electronic copy of the letter.
                ///     (Russian - 1049): Действие, отслеживающее доставку письма. Действие может содержать электронную копию письма.
                ///</summary>
                public static partial class sla_letter
                {
                    public const string Name = "sla_letter";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_letter = "letter";

                    public const string ReferencingAttribute_slainvokedid = "slainvokedid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship sla_opportunity
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_opportunity
                /// ReferencingEntityNavigationPropertyName    slainvokedid_opportunity_sla
                /// IsCustomizable                             True                            False
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
                /// ReferencingEntity opportunity:
                ///     DisplayName:
                ///     (English - United States - 1033): Opportunity
                ///     (Russian - 1049): Возможная сделка
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Opportunities
                ///     (Russian - 1049): Возможные сделки
                ///     
                ///     Description:
                ///     (English - United States - 1033): Potential revenue-generating event, or sale to an account, which needs to be tracked through a sales process to completion.
                ///     (Russian - 1049): Событие, потенциально создающее прибыль, или продажа организации, которая должна отслеживаться в процессе продажи до завершения.
                ///</summary>
                public static partial class sla_opportunity
                {
                    public const string Name = "sla_opportunity";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_opportunity = "opportunity";

                    public const string ReferencingAttribute_slainvokedid = "slainvokedid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship sla_phonecall
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_phonecall
                /// ReferencingEntityNavigationPropertyName    slainvokedid_phonecall_sla
                /// IsCustomizable                             True                          False
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
                /// ReferencingEntity phonecall:
                ///     DisplayName:
                ///     (English - United States - 1033): Phone Call
                ///     (Russian - 1049): Звонок
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Phone Calls
                ///     (Russian - 1049): Звонки
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity to track a telephone call.
                ///     (Russian - 1049): Действие для отслеживания телефонного звонка.
                ///</summary>
                public static partial class sla_phonecall
                {
                    public const string Name = "sla_phonecall";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_phonecall = "phonecall";

                    public const string ReferencingAttribute_slainvokedid = "slainvokedid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship sla_quote
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_quote
                /// ReferencingEntityNavigationPropertyName    slainvokedid_quote_sla
                /// IsCustomizable                             True                      False
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
                /// ReferencingEntity quote:
                ///     DisplayName:
                ///     (English - United States - 1033): Quote
                ///     (Russian - 1049): Предложение с расценками
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Quotes
                ///     (Russian - 1049): Предложения с расценками
                ///     
                ///     Description:
                ///     (English - United States - 1033): Formal offer for products and/or services, proposed at specific prices and related payment terms, which is sent to a prospective customer.
                ///     (Russian - 1049): Формальное предложение продуктов и (или) услуг с конкретными ценами и условиями оплаты, отправляемое потенциальным клиентам.
                ///</summary>
                public static partial class sla_quote
                {
                    public const string Name = "sla_quote";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_quote = "quote";

                    public const string ReferencingAttribute_slainvokedid = "slainvokedid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship sla_salesorder
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_salesorder
                /// ReferencingEntityNavigationPropertyName    slainvokedid_salesorder_sla
                /// IsCustomizable                             True                           False
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
                /// ReferencingEntity salesorder:
                ///     DisplayName:
                ///     (English - United States - 1033): Order
                ///     (Russian - 1049): Заказ
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Orders
                ///     (Russian - 1049): Заказы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Quote that has been accepted.
                ///     (Russian - 1049): Предложение с расценками принято.
                ///</summary>
                public static partial class sla_salesorder
                {
                    public const string Name = "sla_salesorder";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_salesorder = "salesorder";

                    public const string ReferencingAttribute_slainvokedid = "slainvokedid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship sla_serviceappointment
                /// 
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_serviceappointment
                /// ReferencingEntityNavigationPropertyName    slainvokedid_serviceappointment_sla
                /// IsCustomizable                             True                                   False
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
                /// ReferencingEntity serviceappointment:
                ///     DisplayName:
                ///     (English - United States - 1033): Service Activity
                ///     (Russian - 1049): Действие сервиса
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Service Activities
                ///     (Russian - 1049): Действия сервиса
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity offered by the organization to satisfy its customer's needs. Each service activity includes date, time, duration, and required resources.
                ///     (Russian - 1049): Действие, предлагаемое организацией с целью удовлетворить потребности клиента. Каждое действие сервиса включает дату, время, продолжительность и необходимые ресурсы.
                ///</summary>
                public static partial class sla_serviceappointment
                {
                    public const string Name = "sla_serviceappointment";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_serviceappointment = "serviceappointment";

                    public const string ReferencingAttribute_slainvokedid = "slainvokedid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship sla_slaitem_slaId
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_slaitem_slaId
                /// ReferencingEntityNavigationPropertyName    slaid
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              ParentChild
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
                /// ReferencingEntity slaitem:
                ///     DisplayName:
                ///     (English - United States - 1033): SLA Item
                ///     (Russian - 1049): Элемент SLA
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): SLA Items
                ///     (Russian - 1049): Элементы SLA
                ///     
                ///     Description:
                ///     (English - United States - 1033): Contains information about a tracked support KPI for a specific customer.
                ///     (Russian - 1049): Содержит информацию об отслеживаемом индикаторе KPI для определенного клиента.
                /// 
                /// AttributeMaps:
                ///     SourceEntity             TargetEntity
                ///     sla                ->    slaitem
                ///     
                ///     SourceAttribute          TargetAttribute
                ///     name               ->    slaidname
                ///     slaid              ->    slaid
                ///</summary>
                public static partial class sla_slaitem_slaid
                {
                    public const string Name = "sla_slaitem_slaId";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_slaitem = "slaitem";

                    public const string ReferencingAttribute_slaid = "slaid";
                }

                ///<summary>
                /// 1:N - Relationship sla_socialactivity
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_socialactivity
                /// ReferencingEntityNavigationPropertyName    slainvokedid_socialactivity_sla
                /// IsCustomizable                             True                               False
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
                /// ReferencingEntity socialactivity:
                ///     DisplayName:
                ///     (English - United States - 1033): Social Activity
                ///     (Russian - 1049): Действие социальной сети
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Social Activities
                ///     (Russian - 1049): Действия социальной сети
                ///     
                ///     Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                ///</summary>
                public static partial class sla_socialactivity
                {
                    public const string Name = "sla_socialactivity";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_socialactivity = "socialactivity";

                    public const string ReferencingAttribute_slainvokedid = "slainvokedid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship SLA_SyncErrors
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     SLA_SyncErrors
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_sla_syncerror
                /// IsCustomizable                             True                               False
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
                public static partial class sla_syncerrors
                {
                    public const string Name = "SLA_SyncErrors";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_syncerror = "syncerror";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship sla_task
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_task
                /// ReferencingEntityNavigationPropertyName    slainvokedid_task_sla
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity task:
                ///     DisplayName:
                ///     (English - United States - 1033): Task
                ///     (Russian - 1049): Задача
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Tasks
                ///     (Russian - 1049): Задачи
                ///     
                ///     Description:
                ///     (English - United States - 1033): Generic activity representing work needed to be done.
                ///     (Russian - 1049): Универсальное действие, представляющее работу, которую необходимо выполнить.
                ///</summary>
                public static partial class sla_task
                {
                    public const string Name = "sla_task";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_task = "task";

                    public const string ReferencingAttribute_slainvokedid = "slainvokedid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship slabase_AsyncOperations
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     slabase_AsyncOperations
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_sla
                /// IsCustomizable                             False                      False
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
                public static partial class slabase_asyncoperations
                {
                    public const string Name = "slabase_AsyncOperations";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_asyncoperation = "asyncoperation";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship slabase_BulkDeleteFailures
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     slabase_BulkDeleteFailures
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_sla
                /// IsCustomizable                             False                         False
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
                public static partial class slabase_bulkdeletefailures
                {
                    public const string Name = "slabase_BulkDeleteFailures";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_bulkdeletefailure = "bulkdeletefailure";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";
                }

                ///<summary>
                /// 1:N - Relationship slabase_ProcessSessions
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     slabase_ProcessSessions
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_sla
                /// IsCustomizable                             False                      False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          110
                /// 
                /// ReferencingEntity processsession:
                ///     DisplayName:
                ///     (English - United States - 1033): Process Session
                ///     (Russian - 1049): Сеанс процесса
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Process Sessions
                ///     (Russian - 1049): Сеансы процесса
                ///     
                ///     Description:
                ///     (English - United States - 1033): Information that is generated when a dialog is run. Every time that you run a dialog, a dialog session is created.
                ///     (Russian - 1049): Информация, созданная после запуска диалогового окна. При каждом запуске диалогового окна создается сеанс диалогового окна.
                ///</summary>
                public static partial class slabase_processsessions
                {
                    public const string Name = "slabase_ProcessSessions";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_processsession = "processsession";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship slabase_userentityinstancedatas
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     slabase_userentityinstancedatas
                /// ReferencingEntityNavigationPropertyName    objectid_sla
                /// IsCustomizable                             False                              False
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
                public static partial class slabase_userentityinstancedatas
                {
                    public const string Name = "slabase_userentityinstancedatas";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}
