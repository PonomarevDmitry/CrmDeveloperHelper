
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): Organization
    /// (Russian - 1049): Предприятие
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): Organizations
    /// (Russian - 1049): Предприятия
    /// 
    /// Description:
    /// (English - United States - 1033): Top level of the Microsoft Dynamics 365 business hierarchy. The organization can be a specific business, holding company, or corporation.
    /// (Russian - 1049): Верхний уровень бизнес-иерархии Microsoft Dynamics 365. Организация может являться конкретной компанией, холдингом или корпорацией.
    /// 
    /// PropertyName                          Value                   CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False                   False
    /// CanBePrimaryEntityInRelationship      False                   False
    /// CanBeRelatedEntityInRelationship      False                   False
    /// CanChangeHierarchicalRelationship     False                   False
    /// CanChangeTrackingBeEnabled            True                    True
    /// CanCreateAttributes                   False                   False
    /// CanCreateCharts                       False                   False
    /// CanCreateForms                        False                   False
    /// CanCreateViews                        False                   False
    /// CanEnableSyncToExternalSearchIndex    False                   False
    /// CanModifyAdditionalSettings           True                    True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 True
    /// CollectionSchemaName                  Organizations
    /// DaysSinceRecordLastModified           9999
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         organizations
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False                   True
    /// IsAvailableOffline                    True
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False                   False
    /// IsCustomEntity                        False
    /// IsCustomizable                        True                    False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                   False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                   False
    /// IsMappable                            False                   False
    /// IsOfflineInMobileClient               False                   True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False                   True
    /// IsRenameable                          False                   False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False                   False
    /// IsVisibleInMobile                     False                   False
    /// IsVisibleInMobileClient               False                   True
    /// LogicalCollectionName                 organizations
    /// LogicalName                           organization
    /// ObjectTypeCode                        1019
    /// OwnershipType                         OrganizationOwned
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredOrganization
    /// SchemaName                            Organization
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class Organization
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "organization";

            public const string EntitySchemaName = "Organization";

            public const string EntityPrimaryNameAttribute = "name";

            public const string EntityPrimaryIdAttribute = "organizationid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): ACI Tenant URL.
                ///     (Russian - 1049): URL-адрес клиента ACI.
                /// 
                /// Description:
                ///     (English - United States - 1033): ACI Web Endpoint URL.
                ///     (Russian - 1049): URL-адрес веб-конечной точки ACI.
                /// 
                /// SchemaName: ACIWebEndpointUrl
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 500
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string aciwebendpointurl = "aciwebendpointurl";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Acknowledgement Template
                ///     (Russian - 1049): Шаблон подтверждения
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the template to be used for acknowledgement when a user unsubscribes.
                ///     (Russian - 1049): Уникальный идентификатор шаблона, используемого для подтверждения отмены подписки.
                /// 
                /// SchemaName: AcknowledgementTemplateId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: template
                /// 
                ///     Target template    PrimaryIdAttribute templateid    PrimaryNameAttribute title
                ///         DisplayName:
                ///         (English - United States - 1033): Email Template
                ///         (Russian - 1049): Шаблон электронной почты
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Email Templates
                ///         (Russian - 1049): Шаблоны электронной почты
                ///         
                ///         Description:
                ///         (English - United States - 1033): Template for an email message that contains the standard attributes of an email message.
                ///         (Russian - 1049): Шаблон сообщения электронной почты, содержащий стандартные атрибуты сообщения электронной почты.
                ///</summary>
                public const string acknowledgementtemplateid = "acknowledgementtemplateid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Allow Address Book Synchronization
                ///     (Russian - 1049): Разрешить синхронизацию адресной книги
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether background address book synchronization in Microsoft Office Outlook is allowed.
                ///     (Russian - 1049): Указывает, включена ли фоновая синхронизация адресной книги в Microsoft Office Outlook.
                /// 
                /// SchemaName: AllowAddressBookSyncs
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
                public const string allowaddressbooksyncs = "allowaddressbooksyncs";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Allow Automatic Response Creation
                ///     (Russian - 1049): Разрешить создание автоматического ответа
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether automatic response creation is allowed.
                ///     (Russian - 1049): Указывает, разрешено ли создание автоматического ответа.
                /// 
                /// SchemaName: AllowAutoResponseCreation
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
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
                public const string allowautoresponsecreation = "allowautoresponsecreation";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Allow Automatic Unsubscribe
                ///     (Russian - 1049): Разрешить автоматическую отмену подписки
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether automatic unsubscribe is allowed.
                ///     (Russian - 1049): Указывает, разрешена ли автоматическая отмена подписки.
                /// 
                /// SchemaName: AllowAutoUnsubscribe
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
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
                public const string allowautounsubscribe = "allowautounsubscribe";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Allow Automatic Unsubscribe Acknowledgement
                ///     (Russian - 1049): Разрешить автоматическое подтверждение отмены подписки
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether automatic unsubscribe acknowledgement email is allowed to send.
                ///     (Russian - 1049): Указывает, разрешено ли отправлять сообщения автоматического подтверждения отмены подписки.
                /// 
                /// SchemaName: AllowAutoUnsubscribeAcknowledgement
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
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
                public const string allowautounsubscribeacknowledgement = "allowautounsubscribeacknowledgement";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Allow Outlook Client Message Bar Advertisement
                ///     (Russian - 1049): Разрешить рекламу клиента Outlook в панели сообщений
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether Outlook Client message bar advertisement is allowed.
                ///     (Russian - 1049): Установите или снимите флажок, чтобы включить или отключить рекламу клиента Outlook в панели сообщений.
                /// 
                /// SchemaName: AllowClientMessageBarAd
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
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
                public const string allowclientmessagebarad = "allowclientmessagebarad";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Allow Entity Level Auditing
                ///     (Russian - 1049): Разрешить аудит на уровне сущностей
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether auditing of changes to entity is allowed when no attributes have changed.
                ///     (Russian - 1049): Указывает, разрешен ли аудит внесенных в сущность изменений, если атрибуты не изменялись.
                /// 
                /// SchemaName: AllowEntityOnlyAudit
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string allowentityonlyaudit = "allowentityonlyaudit";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Allow Marketing Email Execution
                ///     (Russian - 1049): Разрешить выполнение маркетинговой эл. почты
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether marketing emails execution is allowed.
                ///     (Russian - 1049): Указывает, разрешено ли выполнение маркетинговой электронной почты.
                /// 
                /// SchemaName: AllowMarketingEmailExecution
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
                public const string allowmarketingemailexecution = "allowmarketingemailexecution";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Allow Offline Scheduled Synchronization
                ///     (Russian - 1049): Разрешить автономную синхронизацию по расписанию
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether background offline synchronization in Microsoft Office Outlook is allowed.
                ///     (Russian - 1049): Указывает, включена ли автономная фоновая синхронизация в Microsoft Office Outlook.
                /// 
                /// SchemaName: AllowOfflineScheduledSyncs
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
                public const string allowofflinescheduledsyncs = "allowofflinescheduledsyncs";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Allow Scheduled Synchronization
                ///     (Russian - 1049): Разрешить синхронизацию по расписанию
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether scheduled synchronizations to Outlook are allowed.
                ///     (Russian - 1049): Указывает, разрешены ли операции синхронизации по расписанию с Outlook.
                /// 
                /// SchemaName: AllowOutlookScheduledSyncs
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
                public const string allowoutlookscheduledsyncs = "allowoutlookscheduledsyncs";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Allow Unresolved Address Email Send
                ///     (Russian - 1049): Разрешить отправлять эл. почту на неразрешенные адреса
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether users are allowed to send email to unresolved parties (parties must still have an email address).
                ///     (Russian - 1049): Указывает, разрешено ли пользователям отправлять электронную почту неразрешенным сторонам (требуется адрес электронной почты).
                /// 
                /// SchemaName: AllowUnresolvedPartiesOnEmailSend
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
                public const string allowunresolvedpartiesonemailsend = "allowunresolvedpartiesonemailsend";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Allow User Form Mode Preference
                ///     (Russian - 1049): Разрешить пользовательские настройки режима формы
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether individuals can select their form mode preference in their personal options.
                ///     (Russian - 1049): Указывает, могут ли пользователи выбирать режимы формы в своих персональных параметрах.
                /// 
                /// SchemaName: AllowUserFormModePreference
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string allowuserformmodepreference = "allowuserformmodepreference";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Allow the showing tablet application notification bars in a browser.
                ///     (Russian - 1049): Разрешить показ панелей уведомлений в планшетных приложениях в браузере.
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the showing tablet application notification bars in a browser is allowed.
                ///     (Russian - 1049): Указывает, разрешен ли показ панелей уведомлений в планшетных приложениях в браузере.
                /// 
                /// SchemaName: AllowUsersSeeAppdownloadMessage
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = True
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
                public const string allowusersseeappdownloadmessage = "allowusersseeappdownloadmessage";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Allow Export to Excel
                ///     (Russian - 1049): Разрешить экспорт в Excel
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether Web-based export of grids to Microsoft Office Excel is allowed.
                ///     (Russian - 1049): Указывает, разрешен ли веб-экспорт таблиц в Microsoft Office Excel.
                /// 
                /// SchemaName: AllowWebExcelExport
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string allowwebexcelexport = "allowwebexcelexport";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): AM Designator
                ///     (Russian - 1049): Обозначение AM
                /// 
                /// Description:
                ///     (English - United States - 1033): AM designator to use throughout Microsoft Dynamics CRM.
                ///     (Russian - 1049): Обозначение времени до полудня для использования в Microsoft Dynamics CRM.
                /// 
                /// SchemaName: AMDesignator
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 25
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string amdesignator = "amdesignator";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable App Designer Experience for this Organization
                ///     (Russian - 1049): Включить режим конструктора приложений для этой организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the appDesignerExperience is enabled for the organization.
                ///     (Russian - 1049): Указывает, включена ли функция appDesignerExperience для организации.
                /// 
                /// SchemaName: AppDesignerExperienceEnabled
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
                public const string appdesignerexperienceenabled = "appdesignerexperienceenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Auto Apply Default Entitlement on Case Create
                ///     (Russian - 1049): Автоматически применять объем обслуживания по умолчанию при создании обращения
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether to auto apply the default customer entitlement on case creation.
                ///     (Russian - 1049): Укажите, следует ли автоматически применять объем обслуживания клиента по умолчанию при создании обращения.
                /// 
                /// SchemaName: AutoApplyDefaultonCaseCreate
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
                public const string autoapplydefaultoncasecreate = "autoapplydefaultoncasecreate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Auto Apply Default Entitlement on Case Update
                ///     (Russian - 1049): Автоматически применять объем обслуживания по умолчанию при обновлении обращения
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether to auto apply the default customer entitlement on case update.
                ///     (Russian - 1049): Укажите, следует ли автоматически применять объем обслуживания клиента по умолчанию при обновлении обращения.
                /// 
                /// SchemaName: AutoApplyDefaultonCaseUpdate
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
                public const string autoapplydefaultoncaseupdate = "autoapplydefaultoncaseupdate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Auto-apply SLA After Manually Over-riding
                ///     (Russian - 1049): Нужно ли автоматически применять SLA после переопределения вручную
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether to Auto-apply SLA on case record update after SLA was manually applied.
                ///     (Russian - 1049): Указывает, следует ли автоматически применять SLA при обновлении записи обращения после того, как SLA было применено вручную.
                /// 
                /// SchemaName: AutoApplySLA
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
                public const string autoapplysla = "autoapplysla";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// Description:
                /// 
                /// SchemaName: AzureSchedulerJobCollectionName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string azureschedulerjobcollectionname = "azureschedulerjobcollectionname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Currency
                ///     (Russian - 1049): Валюта
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the base currency of the organization.
                ///     (Russian - 1049): Уникальный идентификатор базовой валюты организации.
                /// 
                /// SchemaName: BaseCurrencyId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
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
                public const string basecurrencyid = "basecurrencyid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Base Currency Precision
                ///     (Russian - 1049): Точность базовой валюты
                /// 
                /// Description:
                ///     (English - United States - 1033): Number of decimal places that can be used for the base currency.
                ///     (Russian - 1049): Число десятичных знаков, допускаемых в базовой валюте.
                /// 
                /// SchemaName: BaseCurrencyPrecision
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 4
                /// Format = None
                ///</summary>
                public const string basecurrencyprecision = "basecurrencyprecision";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Base Currency Symbol
                ///     (Russian - 1049): Обозначение ден. ед. базовой валюты
                /// 
                /// Description:
                ///     (English - United States - 1033): Symbol used for the base currency.
                ///     (Russian - 1049): Символ, обозначающий базовую валюту.
                /// 
                /// SchemaName: BaseCurrencySymbol
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 5
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string basecurrencysymbol = "basecurrencysymbol";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Base ISO Currency Code
                ///     (Russian - 1049): ISO-код базовой валюты
                /// 
                /// SchemaName: BaseISOCurrencyCode
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: False    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 5
                /// Format = Text    ImeMode = Disabled    IsLocalizable = False
                ///</summary>
                public const string baseisocurrencycode = "baseisocurrencycode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Bing Maps API Key
                ///     (Russian - 1049): Ключ интерфейса API карт Bing
                /// 
                /// Description:
                ///     (English - United States - 1033): Api Key to be used in requests to Bing Maps services.
                ///     (Russian - 1049): Ключ интерфейса API, используемый при запросах к службам кари Bing.
                /// 
                /// SchemaName: BingMapsApiKey
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1024
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string bingmapsapikey = "bingmapsapikey";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Block Attachments
                ///     (Russian - 1049): Блокировать вложения
                /// 
                /// Description:
                ///     (English - United States - 1033): Prevent upload or download of certain attachment types that are considered dangerous.
                ///     (Russian - 1049): Запрет передачи или загрузки определенных типов вложений, которые считаются опасными.
                /// 
                /// SchemaName: BlockedAttachments
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1073741823
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string blockedattachments = "blockedattachments";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Bulk Operation Prefix
                ///     (Russian - 1049): Префикс массовой операции
                /// 
                /// Description:
                ///     (English - United States - 1033): Prefix used for bulk operation numbering.
                ///     (Russian - 1049): Префикс, используемый при нумерации в групповой операции.
                /// 
                /// SchemaName: BulkOperationPrefix
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string bulkoperationprefix = "bulkoperationprefix";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Business Closure Calendar
                ///     (Russian - 1049): Календарь нерабочих дней
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the business closure calendar of organization.
                ///     (Russian - 1049): Уникальный идентификатор календаря нерабочих дней организации.
                /// 
                /// SchemaName: BusinessClosureCalendarId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string businessclosurecalendarid = "businessclosurecalendarid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Calendar Type
                ///     (Russian - 1049): Тип календаря
                /// 
                /// Description:
                ///     (English - United States - 1033): Calendar type for the system. Set to Gregorian US by default.
                ///     (Russian - 1049): Тип календаря системы. По умолчанию используется григорианский.
                /// 
                /// SchemaName: CalendarType
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string calendartype = "calendartype";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Campaign Prefix
                ///     (Russian - 1049): Префикс кампании
                /// 
                /// Description:
                ///     (English - United States - 1033): Prefix used for campaign numbering.
                ///     (Russian - 1049): Префикс, используемый при нумерации в кампании.
                /// 
                /// SchemaName: CampaignPrefix
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string campaignprefix = "campaignprefix";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Cascade Status Update
                ///     (Russian - 1049): Каскадное обновление состояния
                /// 
                /// Description:
                ///     (English - United States - 1033): Flag to cascade Update on incident.
                ///     (Russian - 1049): Флаг для каскадного обновления в случае инцидента.
                /// 
                /// SchemaName: CascadeStatusUpdate
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
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
                public const string cascadestatusupdate = "cascadestatusupdate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Case Prefix
                ///     (Russian - 1049): Префикс обращений
                /// 
                /// Description:
                ///     (English - United States - 1033): Prefix to use for all cases throughout Microsoft Dynamics 365.
                ///     (Russian - 1049): Префикс, используемый для всех обращений в Microsoft Dynamics 365.
                /// 
                /// SchemaName: CasePrefix
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string caseprefix = "caseprefix";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Category Prefix
                ///     (Russian - 1049): Префикс категории
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the prefix to use for all categories in Microsoft Dynamics 365.
                ///     (Russian - 1049): Ввод префикса, используемого для всех категорий в Microsoft Dynamics 365.
                /// 
                /// SchemaName: CategoryPrefix
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string categoryprefix = "categoryprefix";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Contract Prefix
                ///     (Russian - 1049): Префикс контрактов
                /// 
                /// Description:
                ///     (English - United States - 1033): Prefix to use for all contracts throughout Microsoft Dynamics 365.
                ///     (Russian - 1049): Префикс, используемый для всех контрактов в Microsoft Dynamics 365.
                /// 
                /// SchemaName: ContractPrefix
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string contractprefix = "contractprefix";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Cortana Proactive Experience Flow processes for this Organization
                ///     (Russian - 1049): Включить процессы потока упреждающих функций Кортаны для этой организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the feature CortanaProactiveExperience Flow processes should be enabled for the organization.
                ///     (Russian - 1049): Указывает, должны ли быть включены процессы потока CortanaProactiveExperience для этой организации.
                /// 
                /// SchemaName: CortanaProactiveExperienceEnabled
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
                public const string cortanaproactiveexperienceenabled = "cortanaproactiveexperienceenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By
                ///     (Russian - 1049): Создано
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who created the organization.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, создавшего организацию.
                /// 
                /// SchemaName: CreatedBy
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
                public const string createdby = "createdby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created On
                ///     (Russian - 1049): Дата создания
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the organization was created.
                ///     (Russian - 1049): Дата и время создания организации.
                /// 
                /// SchemaName: CreatedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who created the organization.
                ///     (Russian - 1049): Уникальный идентификатор пользователя-делегата, создавшего организацию.
                /// 
                /// SchemaName: CreatedOnBehalfBy
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
                public const string createdonbehalfby = "createdonbehalfby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Active Initial Product State
                ///     (Russian - 1049): Включить активное начальное состояние продукта
                /// 
                /// Description:
                ///     (English - United States - 1033): Enable Initial state of newly created products to be Active instead of Draft
                ///     (Russian - 1049): Включить для вновь созданных продуктов начальное состояние "активен", а не "черновик"
                /// 
                /// SchemaName: CreateProductsWithoutParentInActiveState
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
                public const string createproductswithoutparentinactivestate = "createproductswithoutparentinactivestate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Currency Decimal Precision
                ///     (Russian - 1049): Точность дробной части валют
                /// 
                /// Description:
                ///     (English - United States - 1033): Number of decimal places that can be used for currency.
                ///     (Russian - 1049): Число десятичных знаков для валюты.
                /// 
                /// SchemaName: CurrencyDecimalPrecision
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string currencydecimalprecision = "currencydecimalprecision";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Display Currencies Using
                ///     (Russian - 1049): Отображение валют с использованием
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether to display money fields with currency code or currency symbol.
                ///     (Russian - 1049): Указание о том, следует ли отображать в денежных полях код или обозначение денежной единицы.
                /// 
                /// SchemaName: CurrencyDisplayOption
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet organization_currencydisplayoption
                /// DefaultFormValue = 0
                ///</summary>
                public const string currencydisplayoption = "currencydisplayoption";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Currency Format Code
                ///     (Russian - 1049): Код форматирования валюты
                /// 
                /// Description:
                ///     (English - United States - 1033): Information about how currency symbols are placed throughout Microsoft Dynamics CRM.
                ///     (Russian - 1049): Информация о размещении обозначений денежных единиц в Microsoft Dynamics CRM.
                /// 
                /// SchemaName: CurrencyFormatCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet organization_currencyformatcode
                /// DefaultFormValue = 0
                ///</summary>
                public const string currencyformatcode = "currencyformatcode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Currency Symbol
                ///     (Russian - 1049): Обозначение денежной единицы
                /// 
                /// Description:
                ///     (English - United States - 1033): Symbol used for currency throughout Microsoft Dynamics 365.
                ///     (Russian - 1049): Символ, используемый для обозначения валюты в Microsoft Dynamics 365.
                /// 
                /// SchemaName: CurrencySymbol
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 13
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string currencysymbol = "currencysymbol";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Current Bulk Operation Number
                ///     (Russian - 1049): Номер текущей массовой операции
                /// 
                /// Description:
                ///     (English - United States - 1033): Current bulk operation number.
                ///     (Russian - 1049): Текущий номер групповой операции.
                /// 
                /// SchemaName: CurrentBulkOperationNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string currentbulkoperationnumber = "currentbulkoperationnumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Current Campaign Number
                ///     (Russian - 1049): Номер текущей кампании
                /// 
                /// Description:
                ///     (English - United States - 1033): Current campaign number.
                ///     (Russian - 1049): Текущий номер кампании.
                /// 
                /// SchemaName: CurrentCampaignNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string currentcampaignnumber = "currentcampaignnumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Current Case Number
                ///     (Russian - 1049): Номер текущего обращения
                /// 
                /// Description:
                ///     (English - United States - 1033): First case number to use.
                ///     (Russian - 1049): Номер обращения, используемый первым.
                /// 
                /// SchemaName: CurrentCaseNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string currentcasenumber = "currentcasenumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Current Category Number
                ///     (Russian - 1049): Номер текущей категории
                /// 
                /// Description:
                ///     (English - United States - 1033): Enter the first number to use for Categories.
                ///     (Russian - 1049): Введите первый номер, который необходимо использовать для категорий.
                /// 
                /// SchemaName: CurrentCategoryNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -1    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string currentcategorynumber = "currentcategorynumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Current Contract Number
                ///     (Russian - 1049): Номер текущего контракта
                /// 
                /// Description:
                ///     (English - United States - 1033): First contract number to use.
                ///     (Russian - 1049): Номер контракта, используемый первым.
                /// 
                /// SchemaName: CurrentContractNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string currentcontractnumber = "currentcontractnumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Current Import Sequence Number
                ///     (Russian - 1049): Порядковый номер текущего импорта
                /// 
                /// Description:
                ///     (English - United States - 1033): Import sequence to use.
                ///     (Russian - 1049): Последовательность импорта для использования.
                /// 
                /// SchemaName: CurrentImportSequenceNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string currentimportsequencenumber = "currentimportsequencenumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Current Invoice Number
                ///     (Russian - 1049): Номер текущего счета
                /// 
                /// Description:
                ///     (English - United States - 1033): First invoice number to use.
                ///     (Russian - 1049): Номер счета, используемый первым.
                /// 
                /// SchemaName: CurrentInvoiceNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string currentinvoicenumber = "currentinvoicenumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Current Knowledge Article Number
                ///     (Russian - 1049): Номер текущей статьи базы знаний
                /// 
                /// Description:
                ///     (English - United States - 1033): Enter the first number to use for knowledge articles.
                ///     (Russian - 1049): Введите первый номер, который необходимо использовать для статей базы знаний.
                /// 
                /// SchemaName: CurrentKaNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -1    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string currentkanumber = "currentkanumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Current Article Number
                ///     (Russian - 1049): Номер текущей статьи
                /// 
                /// Description:
                ///     (English - United States - 1033): First article number to use.
                ///     (Russian - 1049): Номер статьи, используемый первым.
                /// 
                /// SchemaName: CurrentKbNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string currentkbnumber = "currentkbnumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Current Order Number
                ///     (Russian - 1049): Номер текущего заказа
                /// 
                /// Description:
                ///     (English - United States - 1033): First order number to use.
                ///     (Russian - 1049): Номер заказа, используемый первым.
                /// 
                /// SchemaName: CurrentOrderNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string currentordernumber = "currentordernumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Current Parsed Table Number
                ///     (Russian - 1049): Номер текущей проанализированной таблицы
                /// 
                /// Description:
                ///     (English - United States - 1033): First parsed table number to use.
                ///     (Russian - 1049): Номер первой разобранной таблицы для использования.
                /// 
                /// SchemaName: CurrentParsedTableNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string currentparsedtablenumber = "currentparsedtablenumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Current Quote Number
                ///     (Russian - 1049): Номер текущего предложения с расценками
                /// 
                /// Description:
                ///     (English - United States - 1033): First quote number to use.
                ///     (Russian - 1049): Номер предложения с расценками, используемый первым.
                /// 
                /// SchemaName: CurrentQuoteNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string currentquotenumber = "currentquotenumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Date Format Code
                ///     (Russian - 1049): Код форматирования даты
                /// 
                /// Description:
                ///     (English - United States - 1033): Information about how the date is displayed throughout Microsoft CRM.
                ///     (Russian - 1049): Сведения о формате отображения даты в Microsoft CRM.
                /// 
                /// SchemaName: DateFormatCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet organization_dateformatcode
                /// DefaultFormValue = -1
                ///</summary>
                public const string dateformatcode = "dateformatcode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Date Format String
                ///     (Russian - 1049): Строка формата даты
                /// 
                /// Description:
                ///     (English - United States - 1033): String showing how the date is displayed throughout Microsoft CRM.
                ///     (Russian - 1049): Строка, демонстрирующая формат отображения даты в Microsoft CRM.
                /// 
                /// SchemaName: DateFormatString
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 255
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string dateformatstring = "dateformatstring";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Date Separator
                ///     (Russian - 1049): Разделитель компонентов даты
                /// 
                /// Description:
                ///     (English - United States - 1033): Character used to separate the month, the day, and the year in dates throughout Microsoft Dynamics 365.
                ///     (Russian - 1049): Символ, используемый для разделения месяца, дня и года в датах системы Microsoft Dynamics 365.
                /// 
                /// SchemaName: DateSeparator
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 5
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string dateseparator = "dateseparator";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Max value of Days since record last modified
                ///     (Russian - 1049): Максимальное количество дней с последнего изменения
                /// 
                /// Description:
                ///     (English - United States - 1033): The maximum value for the Mobile Offline setting Days since record last modified
                ///     (Russian - 1049): Максимальное значение параметра "Дни" Mobile Offline, после которого запись была изменена.
                /// 
                /// SchemaName: DaysSinceRecordLastModifiedMaxValue
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string dayssincerecordlastmodifiedmaxvalue = "dayssincerecordlastmodifiedmaxvalue";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Decimal Symbol
                ///     (Russian - 1049): Десятичный символ
                /// 
                /// Description:
                ///     (English - United States - 1033): Symbol used for decimal in Microsoft Dynamics 365.
                ///     (Russian - 1049): Знак десятичного разделителя в Microsoft Dynamics 365.
                /// 
                /// SchemaName: DecimalSymbol
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 5
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string decimalsymbol = "decimalsymbol";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Default Country Code
                ///     (Russian - 1049): Код страны по умолчанию
                /// 
                /// Description:
                ///     (English - United States - 1033): Text area to enter default country code.
                ///     (Russian - 1049): Текстовое поле для ввода кода страны по умолчанию.
                /// 
                /// SchemaName: DefaultCountryCode
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 30
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string defaultcountrycode = "defaultcountrycode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Name of the default app
                ///     (Russian - 1049): Имя приложения по умолчанию
                /// 
                /// Description:
                ///     (English - United States - 1033): Name of the default crm custom.
                ///     (Russian - 1049): Имя crm custom по умолчанию.
                /// 
                /// SchemaName: DefaultCrmCustomName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string defaultcrmcustomname = "defaultcrmcustomname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Email Server Profile
                ///     (Russian - 1049): Профиль сервера электронной почты
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the default email server profile.
                ///     (Russian - 1049): Уникальный идентификатор профиля сервера электронной почты по умолчанию.
                /// 
                /// SchemaName: DefaultEmailServerProfileId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: emailserverprofile
                /// 
                ///     Target emailserverprofile    PrimaryIdAttribute emailserverprofileid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Email Server Profile
                ///         (Russian - 1049): Профиль сервера электронной почты
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Email Server Profiles
                ///         (Russian - 1049): Профили серверов электронной почты
                ///         
                ///         Description:
                ///         (English - United States - 1033): Holds the Email Server Profiles of an organization
                ///         (Russian - 1049): Содержит профили серверов электронной почты организации
                ///</summary>
                public const string defaultemailserverprofileid = "defaultemailserverprofileid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Default Email Settings
                ///     (Russian - 1049): Параметры электронной почты по умолчанию
                /// 
                /// Description:
                ///     (English - United States - 1033): XML string containing the default email settings that are applied when a user or queue is created.
                ///     (Russian - 1049): XML-строка, содержащая настройки электронной почты по умолчанию, которые применяются при создании пользователя или очереди.
                /// 
                /// SchemaName: DefaultEmailSettings
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1073741823
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string defaultemailsettings = "defaultemailsettings";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Default Mobile Offline Profile
                ///     (Russian - 1049): Профиль Mobile Offline по умолчанию
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the default mobile offline profile.
                ///     (Russian - 1049): Уникальный идентификатор профиля Mobile Offline по умолчанию.
                /// 
                /// SchemaName: DefaultMobileOfflineProfileId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
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
                public const string defaultmobileofflineprofileid = "defaultmobileofflineprofileid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Default Recurrence End Range Type
                ///     (Russian - 1049): Тип диапазона окончания повторения по умолчанию
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of default recurrence end range date.
                ///     (Russian - 1049): Тип даты диапазона окончания повторения по умолчанию.
                /// 
                /// SchemaName: DefaultRecurrenceEndRangeType
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet organization_defaultrecurrenceendrangetype
                /// DefaultFormValue = Null
                ///</summary>
                public const string defaultrecurrenceendrangetype = "defaultrecurrenceendrangetype";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Default Theme Data
                ///     (Russian - 1049): Данные темы по умолчанию
                /// 
                /// Description:
                ///     (English - United States - 1033): Default theme data for the organization.
                ///     (Russian - 1049): Данные темы по умолчанию для организации.
                /// 
                /// SchemaName: DefaultThemeData
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string defaultthemedata = "defaultthemedata";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Delegated Admin
                ///     (Russian - 1049): Делегированный администратор
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the delegated admin user for the organization.
                ///     (Russian - 1049): Уникальный идентификатор пользователя организации, выполняющего роль делегированного администратора.
                /// 
                /// SchemaName: DelegatedAdminUserId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string delegatedadminuserid = "delegatedadminuserid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Disabled Reason
                ///     (Russian - 1049): Причина отключения
                /// 
                /// Description:
                ///     (English - United States - 1033): Reason for disabling the organization.
                ///     (Russian - 1049): Причина отключения организации.
                /// 
                /// SchemaName: DisabledReason
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 500
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string disabledreason = "disabledreason";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Social Care disabled
                ///     (Russian - 1049): Отключена ли функция Social Care.
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether Social Care is disabled.
                ///     (Russian - 1049): Указывает, отключена ли функция Social Care.
                /// 
                /// SchemaName: DisableSocialCare
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
                public const string disablesocialcare = "disablesocialcare";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Discount calculation method
                ///     (Russian - 1049): Способ расчета скидки
                /// 
                /// Description:
                ///     (English - United States - 1033): Discount calculation method for the QOOI product.
                ///     (Russian - 1049): Способ расчета скидки для продукта QOOI.
                /// 
                /// SchemaName: DiscountCalculationMethod
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet organization_discountcalculationmethod
                /// DefaultFormValue = 0
                ///</summary>
                public const string discountcalculationmethod = "discountcalculationmethod";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Display Navigation Tour
                ///     (Russian - 1049): Отображать обзор навигации
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether or not navigation tour is displayed.
                ///     (Russian - 1049): Указывает, отображается ли обзор навигации.
                /// 
                /// SchemaName: DisplayNavigationTour
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string displaynavigationtour = "displaynavigationtour";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Email Connection Channel
                ///     (Russian - 1049): Канал подключения электронной почты
                /// 
                /// Description:
                ///     (English - United States - 1033): Select if you want to use the Email Router or server-side synchronization for email processing.
                ///     (Russian - 1049): Укажите, следует ли использовать для обработки электронной почты маршрутизатор электронной почты или синхронизацию на стороне сервера.
                /// 
                /// SchemaName: EmailConnectionChannel
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet organization_emailconnectionchannel
                /// DefaultFormValue = 0
                ///</summary>
                public const string emailconnectionchannel = "emailconnectionchannel";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Use Email Correlation
                ///     (Russian - 1049): Использовать корреляцию электронной почты
                /// 
                /// Description:
                ///     (English - United States - 1033): Flag to turn email correlation on or off.
                ///     (Russian - 1049): Флаг для включения и выключения корреляции электронной почты.
                /// 
                /// SchemaName: EmailCorrelationEnabled
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string emailcorrelationenabled = "emailcorrelationenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Email Send Polling Frequency
                ///     (Russian - 1049): Периодичность опроса отправки электронной почты
                /// 
                /// Description:
                ///     (English - United States - 1033): Normal polling frequency used for sending email in Microsoft Office Outlook.
                ///     (Russian - 1049): Обычная частота опросов для отправки электронной почты в Microsoft Office Outlook.
                /// 
                /// SchemaName: EmailSendPollingPeriod
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string emailsendpollingperiod = "emailsendpollingperiod";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Integration with Bing Maps
                ///     (Russian - 1049): Включить интеграцию с картами Bing
                /// 
                /// Description:
                /// 
                /// SchemaName: EnableBingMapsIntegration
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string enablebingmapsintegration = "enablebingmapsintegration";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Learning Path Authoring
                ///     (Russian - 1049): Включить разработку схемы обучения
                /// 
                /// Description:
                ///     (English - United States - 1033): Select to enable learning path auhtoring.
                ///     (Russian - 1049): Выберите, чтобы включить разработку схемы обучения.
                /// 
                /// SchemaName: EnableLPAuthoring
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
                public const string enablelpauthoring = "enablelpauthoring";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Integration with Microsoft Flow
                ///     (Russian - 1049): Включить интеграцию с Microsoft Flow
                /// 
                /// Description:
                /// 
                /// SchemaName: EnableMicrosoftFlowIntegration
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string enablemicrosoftflowintegration = "enablemicrosoftflowintegration";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Pricing On Create
                ///     (Russian - 1049): Включить назначение цен при создании
                /// 
                /// Description:
                ///     (English - United States - 1033): Enable pricing calculations on a Create call.
                ///     (Russian - 1049): Включение расчета цен при создании звонков.
                /// 
                /// SchemaName: EnablePricingOnCreate
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string enablepricingoncreate = "enablepricingoncreate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Smart Matching
                ///     (Russian - 1049): Включить интеллектуальное сопоставление
                /// 
                /// Description:
                ///     (English - United States - 1033): Use Smart Matching.
                ///     (Russian - 1049): Использовать интеллектуальное сопоставление.
                /// 
                /// SchemaName: EnableSmartMatching
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
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
                public const string enablesmartmatching = "enablesmartmatching";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Organization setting to enforce read only plugins.
                ///     (Russian - 1049): Параметр организации, обеспечивающий принудительное использование подключаемых модулей только для чтения.
                /// 
                /// Description:
                /// 
                /// SchemaName: EnforceReadOnlyPlugins
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
                public const string enforcereadonlyplugins = "enforcereadonlyplugins";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Entity Image Id
                ///     (Russian - 1049): Код изображения сущности
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: EntityImageId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string entityimageid = "entityimageid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Days to Expire Change Tracking Deleted Records
                ///     (Russian - 1049): Дни до истечения отслеживания изменений удаленных записей
                /// 
                /// Description:
                ///     (English - United States - 1033): Maximum number of days to keep change tracking deleted records
                ///     (Russian - 1049): Максимальное число дней, в течение которого сохраняется отслеживание изменений для удаленных записей
                /// 
                /// SchemaName: ExpireChangeTrackingInDays
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 365
                /// Format = None
                ///</summary>
                public const string expirechangetrackingindays = "expirechangetrackingindays";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Days to Expire Subscriptions
                ///     (Russian - 1049): Дней до истечения срока действия подписок
                /// 
                /// Description:
                ///     (English - United States - 1033): Maximum number of days before deleting inactive subscriptions.
                ///     (Russian - 1049): Предел ожидания перед удалением неактивных подписок (в днях).
                /// 
                /// SchemaName: ExpireSubscriptionsInDays
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string expiresubscriptionsindays = "expiresubscriptionsindays";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): External Base URL
                ///     (Russian - 1049): Базовый внешний URL-адрес
                /// 
                /// Description:
                ///     (English - United States - 1033): Specify the base URL to use to look for external document suggestions.
                ///     (Russian - 1049): Определение базового URL-адреса для поиска предложений внешних документов.
                /// 
                /// SchemaName: ExternalBaseUrl
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 500
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string externalbaseurl = "externalbaseurl";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): ExternalPartyEnabled Entities correlation Keys
                ///     (Russian - 1049): Ключи корреляции ExternalPartyEnabled
                /// 
                /// Description:
                ///     (English - United States - 1033): XML string containing the ExternalPartyEnabled entities correlation keys for association of existing External Party instance entities to newly created IsExternalPartyEnabled entities.For internal use only
                ///     (Russian - 1049): Строка XML, содержащая ключи корреляции сущностей ExternalPartyEnabled для связи существующих сущностей экземпляра внешней стороны и созданных сущностей IsExternalPartyEnabled. Только для внутреннего использования.
                /// 
                /// SchemaName: ExternalPartyCorrelationKeys
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1073741823
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string externalpartycorrelationkeys = "externalpartycorrelationkeys";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): ExternalPartyEnabled Entities Settings.For internal use only
                ///     (Russian - 1049): Параметры сущностей с включенной внешней стороной. Только для внутреннего использования.
                /// 
                /// Description:
                ///     (English - United States - 1033): XML string containing the ExternalPartyEnabled entities settings.
                ///     (Russian - 1049): Строка XML, содержащая параметры сущностей ExternalPartyEnabled.
                /// 
                /// SchemaName: ExternalPartyEntitySettings
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1073741823
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string externalpartyentitysettings = "externalpartyentitysettings";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Feature Set
                ///     (Russian - 1049): Набор возможностей
                /// 
                /// Description:
                ///     (English - United States - 1033): Features to be enabled as an XML BLOB.
                ///     (Russian - 1049): Функции для включения в виде объекта BLOB XML.
                /// 
                /// SchemaName: FeatureSet
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1073741823
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string featureset = "featureset";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Fiscal Calendar Start
                ///     (Russian - 1049): Начало финансового календаря
                /// 
                /// Description:
                ///     (English - United States - 1033): Start date for the fiscal period that is to be used throughout Microsoft CRM.
                ///     (Russian - 1049): Дата начала финансового периода, используемая в Microsoft CRM.
                /// 
                /// SchemaName: FiscalCalendarStart
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateOnly
                ///</summary>
                public const string fiscalcalendarstart = "fiscalcalendarstart";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Fiscal Period Format
                ///     (Russian - 1049): Формат финансового периода
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies how the name of the fiscal period is displayed throughout Microsoft CRM.
                ///     (Russian - 1049): Информация, определяющая формат отображения названия финансового периода в Microsoft CRM.
                /// 
                /// SchemaName: FiscalPeriodFormat
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 25
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string fiscalperiodformat = "fiscalperiodformat";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Format for Fiscal Period
                ///     (Russian - 1049): Формат финансового периода
                /// 
                /// Description:
                ///     (English - United States - 1033): Format in which the fiscal period will be displayed.
                ///     (Russian - 1049): Формат, в котором будет отображаться финансовый период.
                /// 
                /// SchemaName: FiscalPeriodFormatPeriod
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet organization_fiscalperiodformat
                /// DefaultFormValue = -1
                ///</summary>
                public const string fiscalperiodformatperiod = "fiscalperiodformatperiod";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Fiscal Period Type
                ///     (Russian - 1049): Тип финансового периода
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of fiscal period used throughout Microsoft CRM.
                ///     (Russian - 1049): Тип финансового периода, используемого в Microsoft CRM.
                /// 
                /// SchemaName: FiscalPeriodType
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string fiscalperiodtype = "fiscalperiodtype";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Fiscal Settings Updated
                ///     (Russian - 1049): Финансовые параметры обновлены?
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether the fiscal settings have been updated.
                ///     (Russian - 1049): Сведения о том, были ли обновлены финансовые параметры.
                /// 
                /// SchemaName: FiscalSettingsUpdated
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
                public const string fiscalsettingsupdated = "fiscalsettingsupdated";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Fiscal Year Display
                ///     (Russian - 1049): Отображение финансового года
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether the fiscal year should be displayed based on the start date or the end date of the fiscal year.
                ///     (Russian - 1049): Сведения, указывающие, следует ли отображать финансовый год на основании начальной или конечной даты финансового года.
                /// 
                /// SchemaName: FiscalYearDisplayCode
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = Null    MaxValue = Null
                /// Format = None
                ///</summary>
                public const string fiscalyeardisplaycode = "fiscalyeardisplaycode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Fiscal Year Format
                ///     (Russian - 1049): Формат финансового года
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies how the name of the fiscal year is displayed throughout Microsoft CRM.
                ///     (Russian - 1049): Информация, определяющая формат отображения названия финансового года в Microsoft CRM.
                /// 
                /// SchemaName: FiscalYearFormat
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 25
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string fiscalyearformat = "fiscalyearformat";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Prefix for Fiscal Year
                ///     (Russian - 1049): Префикс финансового года
                /// 
                /// Description:
                ///     (English - United States - 1033): Prefix for the display of the fiscal year.
                ///     (Russian - 1049): Префикс, отображаемый для финансового года.
                /// 
                /// SchemaName: FiscalYearFormatPrefix
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet organization_fiscalyearformatprefix
                /// DefaultFormValue = Null
                ///</summary>
                public const string fiscalyearformatprefix = "fiscalyearformatprefix";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Suffix for Fiscal Year
                ///     (Russian - 1049): Суффикс финансового года
                /// 
                /// Description:
                ///     (English - United States - 1033): Suffix for the display of the fiscal year.
                ///     (Russian - 1049): Суффикс, отображаемый для финансового года.
                /// 
                /// SchemaName: FiscalYearFormatSuffix
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet organization_fiscalyearformatsuffix
                /// DefaultFormValue = Null
                ///</summary>
                public const string fiscalyearformatsuffix = "fiscalyearformatsuffix";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Fiscal Year Format Year
                ///     (Russian - 1049): Формат компонента года финансового года
                /// 
                /// Description:
                ///     (English - United States - 1033): Format for the year.
                ///     (Russian - 1049): Формат года.
                /// 
                /// SchemaName: FiscalYearFormatYear
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet organization_fiscalyearformatyear
                /// DefaultFormValue = Null
                ///</summary>
                public const string fiscalyearformatyear = "fiscalyearformatyear";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Fiscal Year Period Connector
                ///     (Russian - 1049): Связывание финансового года и периода
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies how the names of the fiscal year and the fiscal period should be connected when displayed together.
                ///     (Russian - 1049): Информация, определяющая, как следует связывать названия финансового года и финансового периода при их совместном отображении.
                /// 
                /// SchemaName: FiscalYearPeriodConnect
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 5
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string fiscalyearperiodconnect = "fiscalyearperiodconnect";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Full Name Display Order
                ///     (Russian - 1049): Порядок отображения полных имен
                /// 
                /// Description:
                ///     (English - United States - 1033): Order in which names are to be displayed throughout Microsoft CRM.
                ///     (Russian - 1049): Порядок, в котором имена отображаются в Microsoft CRM.
                /// 
                /// SchemaName: FullNameConventionCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet organization_fullnameconventioncode
                /// DefaultFormValue = 0
                ///</summary>
                public const string fullnameconventioncode = "fullnameconventioncode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Future Expansion Window
                ///     (Russian - 1049): Окно расширения в будущее
                /// 
                /// Description:
                ///     (English - United States - 1033): Specifies the maximum number of months in future for which the recurring activities can be created.
                ///     (Russian - 1049): Указывает максимальное число месяцев в будущем, для которых можно создать повторяющиеся действия.
                /// 
                /// SchemaName: FutureExpansionWindow
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 1    MaxValue = 140
                /// Format = None
                ///</summary>
                public const string futureexpansionwindow = "futureexpansionwindow";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Generate Alerts For Errors
                ///     (Russian - 1049): Формировать оповещения для ошибок
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether alerts will be generated for errors.
                ///     (Russian - 1049): Указывает, будут ли формироваться оповещения для ошибок.
                /// 
                /// SchemaName: GenerateAlertsForErrors
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string generatealertsforerrors = "generatealertsforerrors";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Generate Alerts For Information
                ///     (Russian - 1049): Формировать информационные оповещения
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether alerts will be generated for information.
                ///     (Russian - 1049): Указывает, будут ли формироваться информационные оповещения.
                /// 
                /// SchemaName: GenerateAlertsForInformation
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string generatealertsforinformation = "generatealertsforinformation";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Generate Alerts For Warnings
                ///     (Russian - 1049): Формировать оповещения с предупреждениями
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether alerts will be generated for warnings.
                ///     (Russian - 1049): Указывает, будут ли формироваться предупреждающие оповещения.
                /// 
                /// SchemaName: GenerateAlertsForWarnings
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string generatealertsforwarnings = "generatealertsforwarnings";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Get Started Pane Content Enabled
                ///     (Russian - 1049): Содержимое панели "Начало работы" включено?
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether Get Started content is enabled for this organization.
                ///     (Russian - 1049): Указывает, включено ли содержимое панели "Начало работы" для этой организации.
                /// 
                /// SchemaName: GetStartedPaneContentEnabled
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string getstartedpanecontentenabled = "getstartedpanecontentenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is AppendUrl Parameters enabled
                ///     (Russian - 1049): Включены ли параметры AppendUrl
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the append URL parameters is enabled.
                ///     (Russian - 1049): Указывает, включены ли параметры присоединения URL-адреса.
                /// 
                /// SchemaName: GlobalAppendUrlParametersEnabled
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string globalappendurlparametersenabled = "globalappendurlparametersenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Global Help URL.
                ///     (Russian - 1049): URL-адрес глобальной справки.
                /// 
                /// Description:
                ///     (English - United States - 1033): URL for the web page global help.
                ///     (Russian - 1049): URL-адрес веб-страницы глобальной справки.
                /// 
                /// SchemaName: GlobalHelpUrl
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 500
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string globalhelpurl = "globalhelpurl";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Customizable Global Help enabled
                ///     (Russian - 1049): Включена ли настраиваемая глобальная справка
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the customizable global help is enabled.
                ///     (Russian - 1049): Указывает, включена ли настраиваемая глобальная справка.
                /// 
                /// SchemaName: GlobalHelpUrlEnabled
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string globalhelpurlenabled = "globalhelpurlenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Rollup Expiration Time for Goal
                ///     (Russian - 1049): Время до окончания срока действия свертки для цели
                /// 
                /// Description:
                ///     (English - United States - 1033): Number of days after the goal's end date after which the rollup of the goal stops automatically.
                ///     (Russian - 1049): Количество дней после даты окончания цели, по истечение которых свертка цели автоматически останавливается.
                /// 
                /// SchemaName: GoalRollupExpiryTime
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 400
                /// Format = None
                ///</summary>
                public const string goalrollupexpirytime = "goalrollupexpirytime";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Automatic Rollup Frequency for Goal
                ///     (Russian - 1049): Периодичность автоматических сверток для цели
                /// 
                /// Description:
                ///     (English - United States - 1033): Number of hours between automatic rollup jobs .
                ///     (Russian - 1049): Количество часов между автоматическими заданиями свертки.
                /// 
                /// SchemaName: GoalRollupFrequency
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 1    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string goalrollupfrequency = "goalrollupfrequency";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Grant Access To Network Service
                ///     (Russian - 1049): Предоставить доступ к сетевой службе
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: GrantAccessToNetworkService
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string grantaccesstonetworkservice = "grantaccesstonetworkservice";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Hash Delta Subject Count
                ///     (Russian - 1049): Счетчик хэша отличий темы
                /// 
                /// Description:
                ///     (English - United States - 1033): Maximum difference allowed between subject keywords count of the email messaged to be correlated
                ///     (Russian - 1049): Максимальное отличие между количеством ключевых слов в теме сообщений электронной почты, которое будет коррелироваться
                /// 
                /// SchemaName: HashDeltaSubjectCount
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string hashdeltasubjectcount = "hashdeltasubjectcount";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Hash Filter Keywords
                ///     (Russian - 1049): Ключевые слова хэша
                /// 
                /// Description:
                ///     (English - United States - 1033): Filter Subject Keywords
                ///     (Russian - 1049): Фильтровать ключевые слова в теме
                /// 
                /// SchemaName: HashFilterKeywords
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1073741823
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string hashfilterkeywords = "hashfilterkeywords";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Hash Max Count
                ///     (Russian - 1049): Максимальное количество хэшей
                /// 
                /// Description:
                ///     (English - United States - 1033): Maximum number of subject keywords or recipients used for correlation
                ///     (Russian - 1049): Максимальное количество ключевых слов в теме или получателях, используемых для корреляции
                /// 
                /// SchemaName: HashMaxCount
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string hashmaxcount = "hashmaxcount";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Hash Min Address Count
                ///     (Russian - 1049): Количество мин. адресов хэша
                /// 
                /// Description:
                ///     (English - United States - 1033): Minimum number of recipients required to match for email messaged to be correlated
                ///     (Russian - 1049): Минимальное количество получателей, необходимое для сопоставления для корреляции сообщений электронной почты
                /// 
                /// SchemaName: HashMinAddressCount
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string hashminaddresscount = "hashminaddresscount";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): High contrast Theme Data
                ///     (Russian - 1049): Данные темы высокой контрастности
                /// 
                /// Description:
                ///     (English - United States - 1033): High contrast theme data for the organization.
                ///     (Russian - 1049): Данные темы высокой контрастности для организации.
                /// 
                /// SchemaName: HighContrastThemeData
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string highcontrastthemedata = "highcontrastthemedata";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Ignore Internal Email
                ///     (Russian - 1049): Игнорировать внутреннюю электронную почту
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether incoming email sent by internal Microsoft Dynamics 365 users or queues should be tracked.
                ///     (Russian - 1049): Указывает, будет ли отслеживаться входящая электронная почта, отправленная внутренними пользователями или очередями Microsoft Dynamics 365.
                /// 
                /// SchemaName: IgnoreInternalEmail
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
                public const string ignoreinternalemail = "ignoreinternalemail";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Inactivity timeout enabled
                ///     (Russian - 1049): Время ожидания активности включено
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether Inactivity timeout is enabled
                ///     (Russian - 1049): Сведения о том, включено ли время ожидания активности
                /// 
                /// SchemaName: InactivityTimeoutEnabled
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
                public const string inactivitytimeoutenabled = "inactivitytimeoutenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Inactivity timeout in minutes
                ///     (Russian - 1049): Время ожидания активности в минутах
                /// 
                /// Description:
                /// 
                /// SchemaName: InactivityTimeoutInMins
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string inactivitytimeoutinmins = "inactivitytimeoutinmins";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Inactivity timeout reminder in minutes
                ///     (Russian - 1049): Время до напоминания при ожидании активности в минутах
                /// 
                /// Description:
                /// 
                /// SchemaName: InactivityTimeoutReminderInMins
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string inactivitytimeoutreminderinmins = "inactivitytimeoutreminderinmins";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Exchange Email Retrieval Batch Size
                ///     (Russian - 1049): Размер пакета извлечения для сервера Exchange
                /// 
                /// Description:
                ///     (English - United States - 1033): Setting for the Async Service Mailbox Queue. Defines the retrieval batch size of exchange server.
                ///     (Russian - 1049): Параметр для очереди почтовых ящиков служб асинхронного обновления. Определяет размер пакета извлечения для сервера Exchange.
                /// 
                /// SchemaName: IncomingEmailExchangeEmailRetrievalBatchSize
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 1    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string incomingemailexchangeemailretrievalbatchsize = "incomingemailexchangeemailretrievalbatchsize";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Initial Version
                ///     (Russian - 1049): Первоначальная версия
                /// 
                /// Description:
                ///     (English - United States - 1033): Initial version of the organization.
                ///     (Russian - 1049): Первоначальная версия организации.
                /// 
                /// SchemaName: InitialVersion
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string initialversion = "initialversion";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Integration User
                ///     (Russian - 1049): Пользователь-интегратор
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the integration user for the organization.
                ///     (Russian - 1049): Уникальный идентификатор пользователя-интегратора организации.
                /// 
                /// SchemaName: IntegrationUserId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string integrationuserid = "integrationuserid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Invoice Prefix
                ///     (Russian - 1049): Префикс счетов
                /// 
                /// Description:
                ///     (English - United States - 1033): Prefix to use for all invoice numbers throughout Microsoft Dynamics 365.
                ///     (Russian - 1049): Префикс, используемый во всех номерах счетов в Microsoft Dynamics 365.
                /// 
                /// SchemaName: InvoicePrefix
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string invoiceprefix = "invoiceprefix";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Action Card for this Organization
                ///     (Russian - 1049): Включить карточку действия для этой организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the feature Action Card should be enabled for the organization.
                ///     (Russian - 1049): Указывает, должна ли быть включена функция карточки действия для организации.
                /// 
                /// SchemaName: IsActionCardEnabled
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
                public const string isactioncardenabled = "isactioncardenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Relationship Analytics for this Organization
                ///     (Russian - 1049): Включить аналитику отношений для этой организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the feature Relationship Analytics should be enabled for the organization.
                ///     (Russian - 1049): Указывает, должна ли быть включена функция аналитики отношений для этой организации.
                /// 
                /// SchemaName: IsActivityAnalysisEnabled
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
                public const string isactivityanalysisenabled = "isactivityanalysisenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Application Mode Enabled
                ///     (Russian - 1049): Режим приложения включен?
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether loading of Microsoft Dynamics 365 in a browser window that does not have address, tool, and menu bars is enabled.
                ///     (Russian - 1049): Указывает, будет ли система Microsoft Dynamics 365 открыта в окне браузера без адресной строки, панели инструментов и меню.
                /// 
                /// SchemaName: IsAppMode
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
                public const string isappmode = "isappmode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Attachment Sync Enabled
                ///     (Russian - 1049): Включена ли синхронизация вложений
                /// 
                /// Description:
                ///     (English - United States - 1033): Enable or disable attachments sync for outlook and exchange.
                ///     (Russian - 1049): Включение или отключение синхронизации вложений для Outlook и Exchange.
                /// 
                /// SchemaName: IsAppointmentAttachmentSyncEnabled
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
                public const string isappointmentattachmentsyncenabled = "isappointmentattachmentsyncenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Assigned Tasks Sync Enabled
                ///     (Russian - 1049): Включена ли синхронизация назначенных задач
                /// 
                /// Description:
                ///     (English - United States - 1033): Enable or disable assigned tasks sync for outlook and exchange.
                ///     (Russian - 1049): Включение или отключение синхронизации назначенных задач для Outlook и Exchange.
                /// 
                /// SchemaName: IsAssignedTasksSyncEnabled
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
                public const string isassignedtaskssyncenabled = "isassignedtaskssyncenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Auditing Enabled
                ///     (Russian - 1049): Аудит включен?
                /// 
                /// Description:
                ///     (English - United States - 1033): Enable or disable auditing of changes.
                ///     (Russian - 1049): Включение или отключение аудита изменений.
                /// 
                /// SchemaName: IsAuditEnabled
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
                public const string isauditenabled = "isauditenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Auto Capture for this Organization
                ///     (Russian - 1049): Включить автоматическую запись для этой организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the feature Auto Capture should be enabled for the organization.
                ///     (Russian - 1049): Указывает, должна ли быть включена функция автоматической записи для этой организации.
                /// 
                /// SchemaName: IsAutoDataCaptureEnabled
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
                public const string isautodatacaptureenabled = "isautodatacaptureenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Auto Save Enabled
                ///     (Russian - 1049): Автоматическое сохранение включено
                /// 
                /// Description:
                ///     (English - United States - 1033): Information on whether auto save is enabled.
                ///     (Russian - 1049): Сведения о том, включено ли автоматическое сохранение.
                /// 
                /// SchemaName: IsAutoSaveEnabled
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string isautosaveenabled = "isautosaveenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Conflict Detection for Mobile Client enabled
                ///     (Russian - 1049): Включено ли обнаружение конфликтов для мобильного клиента
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether conflict detection for mobile client is enabled.
                ///     (Russian - 1049): Сведения, которые определяют, включено ли обнаружение конфликтов для мобильного клиента.
                /// 
                /// SchemaName: IsConflictDetectionEnabledForMobileClient
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
                public const string isconflictdetectionenabledformobileclient = "isconflictdetectionenabledformobileclient";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Mailing Address Sync Enabled
                ///     (Russian - 1049): Включена ли синхронизация почтовых адресов
                /// 
                /// Description:
                ///     (English - United States - 1033): Enable or disable mailing address sync for outlook and exchange.
                ///     (Russian - 1049): Включение или отключение синхронизации почтовых адресов для Outlook и Exchange.
                /// 
                /// SchemaName: IsContactMailingAddressSyncEnabled
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string iscontactmailingaddresssyncenabled = "iscontactmailingaddresssyncenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable or disable country code selection
                ///     (Russian - 1049): Включить или отключить выбор кода страны
                /// 
                /// Description:
                ///     (English - United States - 1033): Enable or disable country code selection.
                ///     (Russian - 1049): Включить или отключить выбор кода страны.
                /// 
                /// SchemaName: IsDefaultCountryCodeCheckEnabled
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string isdefaultcountrycodecheckenabled = "isdefaultcountrycodecheckenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Delegation Access Enabled
                ///     (Russian - 1049): Включен ли доступ по делегированию
                /// 
                /// Description:
                ///     (English - United States - 1033): Enable Delegation Access content
                ///     (Russian - 1049): Включить содержимое доступа по делегированию
                /// 
                /// SchemaName: IsDelegateAccessEnabled
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
                public const string isdelegateaccessenabled = "isdelegateaccessenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Action Hub for this Organization
                ///     (Russian - 1049): Включает центр действий для этой организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the feature Action Hub should be enabled for the organization.
                ///     (Russian - 1049): Указывает, должна ли быть включена функция "Центр действий" для организации.
                /// 
                /// SchemaName: IsDelveActionHubIntegrationEnabled
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
                public const string isdelveactionhubintegrationenabled = "isdelveactionhubintegrationenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Organization Disabled
                ///     (Russian - 1049): Организация отключена?
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether the organization is disabled.
                ///     (Russian - 1049): Сведения о том, отключена ли организация.
                /// 
                /// SchemaName: IsDisabled
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
                public const string isdisabled = "isdisabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Duplicate Detection Enabled
                ///     (Russian - 1049): Поиск повторяющихся данных включен?
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether duplicate detection of records is enabled.
                ///     (Russian - 1049): Указывает, включено ли обнаружение повторяющихся записей.
                /// 
                /// SchemaName: IsDuplicateDetectionEnabled
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
                public const string isduplicatedetectionenabled = "isduplicatedetectionenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Duplicate Detection Enabled For Import
                ///     (Russian - 1049): Поиск повторяющихся данных для импорта включен?
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether duplicate detection of records during import is enabled.
                ///     (Russian - 1049): Указывает, включено ли обнаружение повторяющихся записей при импорте.
                /// 
                /// SchemaName: IsDuplicateDetectionEnabledForImport
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
                public const string isduplicatedetectionenabledforimport = "isduplicatedetectionenabledforimport";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Duplicate Detection Enabled For Offline Synchronization
                ///     (Russian - 1049): Поиск повторяющихся данных для автономной синхронизации включен?
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether duplicate detection of records during offline synchronization is enabled.
                ///     (Russian - 1049): Указывает, включено ли обнаружение повторяющихся записей при автономной синхронизации.
                /// 
                /// SchemaName: IsDuplicateDetectionEnabledForOfflineSync
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
                public const string isduplicatedetectionenabledforofflinesync = "isduplicatedetectionenabledforofflinesync";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Duplicate Detection Enabled for Online Create/Update
                ///     (Russian - 1049): Поиск повторяющихся данных для создания или обновления в оперативном режиме включен?
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether duplicate detection during online create or update is enabled.
                ///     (Russian - 1049): Указывает, включено ли обнаружение повторяющихся данных при создании или обновлении в оперативном режиме.
                /// 
                /// SchemaName: IsDuplicateDetectionEnabledForOnlineCreateUpdate
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
                public const string isduplicatedetectionenabledforonlinecreateupdate = "isduplicatedetectionenabledforonlinecreateupdate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Allow tracking recipient activity on sent emails
                ///     (Russian - 1049): Разрешить отслеживание действий получателя для отправленных сообщений электронной почты
                /// 
                /// Description:
                ///     (English - United States - 1033): Allow tracking recipient activity on sent emails.
                ///     (Russian - 1049): Разрешить отслеживание действий получателя для отправленных сообщений электронной почты.
                /// 
                /// SchemaName: IsEmailMonitoringAllowed
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string isemailmonitoringallowed = "isemailmonitoringallowed";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Email Server Profile Content Filtering Enabled
                ///     (Russian - 1049): Включена ли фильтрация содержимого профиля почтового сервера
                /// 
                /// Description:
                ///     (English - United States - 1033): Enable Email Server Profile content filtering
                ///     (Russian - 1049): Включить фильтрацию содержимого профиля почтового сервера
                /// 
                /// SchemaName: IsEmailServerProfileContentFilteringEnabled
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
                public const string isemailserverprofilecontentfilteringenabled = "isemailserverprofilecontentfilteringenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): option set values for isenabledforallroles
                ///     (Russian - 1049): значения набора параметров для isenabledforallroles
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether appmodule is enabled for all roles
                ///     (Russian - 1049): Показывает, включен ли модуль приложения для всех ролей
                /// 
                /// SchemaName: IsEnabledForAllRoles
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string isenabledforallroles = "isenabledforallroles";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable external search data syncing
                ///     (Russian - 1049): Включение синхронизации данных со внешним механизмом поиска
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether data can be synchronized with an external search index.
                ///     (Russian - 1049): Указывает, можно ли синхронизировать данные с внешним индексом поиска.
                /// 
                /// SchemaName: IsExternalSearchIndexEnabled
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
                public const string isexternalsearchindexenabled = "isexternalsearchindexenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Fiscal Period Monthly
                ///     (Russian - 1049): Финансовый период помесячный?
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the fiscal period is displayed as the month number.
                ///     (Russian - 1049): Указывает, следует ли отображать финансовый период как номер месяца.
                /// 
                /// SchemaName: IsFiscalPeriodMonthBased
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
                public const string isfiscalperiodmonthbased = "isfiscalperiodmonthbased";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Automatically create folders
                ///     (Russian - 1049): Автоматически создавать папки
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether folders should be automatically created on SharePoint.
                ///     (Russian - 1049): Выберите, должны ли папки автоматически создаваться на сайте SharePoint.
                /// 
                /// SchemaName: IsFolderAutoCreatedonSP
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
                public const string isfolderautocreatedonsp = "isfolderautocreatedonsp";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Folder Based Tracking Enabled
                ///     (Russian - 1049): Включено ли отслеживание на основе папок
                /// 
                /// Description:
                ///     (English - United States - 1033): Enable or disable folder based tracking for Server Side Sync.
                ///     (Russian - 1049): Включить или отключить отслеживание на основе папок для синхронизации на стороне сервера.
                /// 
                /// SchemaName: IsFolderBasedTrackingEnabled
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
                public const string isfolderbasedtrackingenabled = "isfolderbasedtrackingenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Full-text search for Quick Find
                ///     (Russian - 1049): Включить полнотекстовый поиск в функции быстрого поиска
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether full-text search for Quick Find entities should be enabled for the organization.
                ///     (Russian - 1049): Указывает, должен ли быть включен полнотекстовый поиск для сущностей быстрого поиска в этой организации.
                /// 
                /// SchemaName: IsFullTextSearchEnabled
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string isfulltextsearchenabled = "isfulltextsearchenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Hierarchical Security Model
                ///     (Russian - 1049): Включить модель иерархической безопасности
                /// 
                /// Description:
                /// 
                /// SchemaName: IsHierarchicalSecurityModelEnabled
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string ishierarchicalsecuritymodelenabled = "ishierarchicalsecuritymodelenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Mailbox Forced Unlocking Enabled
                ///     (Russian - 1049): Включена ли принудительная разблокировка почтового ящика
                /// 
                /// Description:
                ///     (English - United States - 1033): Enable or disable forced unlocking for Server Side Sync mailboxes.
                ///     (Russian - 1049): Включить или отключить принудительную разблокировку для почтовых ящиков с синхронизацией на стороне сервера.
                /// 
                /// SchemaName: IsMailboxForcedUnlockingEnabled
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
                public const string ismailboxforcedunlockingenabled = "ismailboxforcedunlockingenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Mailbox Keep Alive Enabled
                ///     (Russian - 1049): Включена ли проверка активности почтового ящика
                /// 
                /// Description:
                ///     (English - United States - 1033): Enable or disable mailbox keep alive for Server Side Sync.
                ///     (Russian - 1049): Включить или отключить проверку активности подключения к почтовому ящику для синхронизации на стороне сервера.
                /// 
                /// SchemaName: IsMailboxInactiveBackoffEnabled
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
                public const string ismailboxinactivebackoffenabled = "ismailboxinactivebackoffenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Mobile Client On Demand Sync enabled
                ///     (Russian - 1049): Включена ли синхронизация мобильного клиента по требованию
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether mobile client on demand sync is enabled.
                ///     (Russian - 1049): Сведения, которые определяют, включена ли синхронизация мобильного клиента по требованию.
                /// 
                /// SchemaName: IsMobileClientOnDemandSyncEnabled
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
                public const string ismobileclientondemandsyncenabled = "ismobileclientondemandsyncenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable MobileOffline for this Organization
                ///     (Russian - 1049): Включает мобильную автономную работу для этой организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the feature MobileOffline should be enabled for the organization.
                ///     (Russian - 1049): Указывает, должна ли быть включена функция автономного мобильного режима для этой организации.
                /// 
                /// SchemaName: IsMobileOfflineEnabled
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
                public const string ismobileofflineenabled = "ismobileofflineenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable OfficeGraph for this Organization
                ///     (Russian - 1049): Включает Office Graph для этой организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the feature OfficeGraph should be enabled for the organization.
                ///     (Russian - 1049): Указывает, должна ли быть включена функция Office Graph для этой организации.
                /// 
                /// SchemaName: IsOfficeGraphEnabled
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
                public const string isofficegraphenabled = "isofficegraphenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable One Drive for this Organization
                ///     (Russian - 1049): Включает OneDrive для этой организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the feature One Drive should be enabled for the organization.
                ///     (Russian - 1049): Указывает, должна ли быть включена функция OneDrive для этой организации.
                /// 
                /// SchemaName: IsOneDriveEnabled
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
                public const string isonedriveenabled = "isonedriveenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Presence Enabled
                ///     (Russian - 1049): Режим присутствия включен
                /// 
                /// Description:
                ///     (English - United States - 1033): Information on whether IM presence is enabled.
                ///     (Russian - 1049): Сведения о включении и отключении режима присутствия IM.
                /// 
                /// SchemaName: IsPresenceEnabled
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string ispresenceenabled = "ispresenceenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Preview Action Card feature for this Organization
                ///     (Russian - 1049): Включить функцию предварительного просмотра карточки действия для этой организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the Preview feature for Action Card should be enabled for the organization.
                ///     (Russian - 1049): Указывает, должна ли быть включена функция предварительного просмотра для карточки действия для организации.
                /// 
                /// SchemaName: IsPreviewEnabledForActionCard
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
                public const string ispreviewenabledforactioncard = "ispreviewenabledforactioncard";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Auto Capture for this Organization at Preview Settings
                ///     (Russian - 1049): Включить автоматический сбор данных для этой организации в параметрах предварительного просмотра
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the feature Auto Capture should be enabled for the organization at Preview Settings.
                ///     (Russian - 1049): Указывает, должна ли в параметрах предварительного просмотра быть включена функция автоматического сбора для организации.
                /// 
                /// SchemaName: IsPreviewForAutoCaptureEnabled
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
                public const string ispreviewforautocaptureenabled = "ispreviewforautocaptureenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Allows Preview For Email Monitoring
                ///     (Russian - 1049): Разрешает предварительный просмотр для мониторинга электронной почты
                /// 
                /// Description:
                ///     (English - United States - 1033): Is Preview For Email Monitoring Allowed.
                ///     (Russian - 1049): Разрешен ли предварительный просмотр для мониторинга электронной почты.
                /// 
                /// SchemaName: IsPreviewForEmailMonitoringAllowed
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string ispreviewforemailmonitoringallowed = "ispreviewforemailmonitoringallowed";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Relationship Insights for this Organization
                ///     (Russian - 1049): Включить аналитику отношений для этой организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the feature Relationship Insights should be enabled for the organization.
                ///     (Russian - 1049): Указывает, должна ли быть включена функция аналитики отношений для этой организации.
                /// 
                /// SchemaName: IsRelationshipInsightsEnabled
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
                public const string isrelationshipinsightsenabled = "isrelationshipinsightsenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Resource booking synchronization enabled
                ///     (Russian - 1049): Синхронизация резервирования ресурсов включена
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates if the synchronization of user resource booking with Exchange is enabled at organization level.
                ///     (Russian - 1049): Указывает, что синхронизация резервирования ресурсов пользователей с Exchange включена на уровне организации.
                /// 
                /// SchemaName: IsResourceBookingExchangeSyncEnabled
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string isresourcebookingexchangesyncenabled = "isresourcebookingexchangesyncenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Sales Order Integration Enabled
                ///     (Russian - 1049): Интеграция заказов на продажу включена?
                /// 
                /// Description:
                ///     (English - United States - 1033): Enable sales order processing integration.
                ///     (Russian - 1049): Включите объединение c обработкой заказов на продажу.
                /// 
                /// SchemaName: IsSOPIntegrationEnabled
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
                public const string issopintegrationenabled = "issopintegrationenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is User Access Auditing Enabled
                ///     (Russian - 1049): Аудит доступа пользователей включен?
                /// 
                /// Description:
                ///     (English - United States - 1033): Enable or disable auditing of user access.
                ///     (Russian - 1049): Включение или отключение аудита доступа пользователей.
                /// 
                /// SchemaName: IsUserAccessAuditEnabled
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
                public const string isuseraccessauditenabled = "isuseraccessauditenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): ISV Integration Mode
                ///     (Russian - 1049): Режим интеграции ISV
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether loading of Microsoft Dynamics 365 in a browser window that does not have address, tool, and menu bars is enabled.
                ///     (Russian - 1049): Указывает, будет ли система Microsoft Dynamics 365 открыта в окне браузера без адресной строки, панели инструментов и меню.
                /// 
                /// SchemaName: ISVIntegrationCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet organization_isvintegrationcode
                /// DefaultFormValue = -1
                ///</summary>
                public const string isvintegrationcode = "isvintegrationcode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Knowledge Article Prefix
                ///     (Russian - 1049): Префикс статьи базы знаний
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the prefix to use for all knowledge articles in Microsoft Dynamics 365.
                ///     (Russian - 1049): Ввод префикса, используемого для всех статей базы знаний в Microsoft Dynamics 365.
                /// 
                /// SchemaName: KaPrefix
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string kaprefix = "kaprefix";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Article Prefix
                ///     (Russian - 1049): Префикс статей
                /// 
                /// Description:
                ///     (English - United States - 1033): Prefix to use for all articles in Microsoft Dynamics 365.
                ///     (Russian - 1049): Префикс, используемый для всех статей в Microsoft Dynamics 365.
                /// 
                /// SchemaName: KbPrefix
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string kbprefix = "kbprefix";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Knowledge Management Settings
                ///     (Russian - 1049): Параметры управления знаниями
                /// 
                /// Description:
                ///     (English - United States - 1033): XML string containing the Knowledge Management settings that are applied in Knowledge Management Wizard.
                ///     (Russian - 1049): XML-строка, содержащая настройки управления знаниями по умолчанию, которые применяются в мастере управления знаниями.
                /// 
                /// SchemaName: KMSettings
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1073741823
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string kmsettings = "kmsettings";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Language
                ///     (Russian - 1049): Язык
                /// 
                /// Description:
                ///     (English - United States - 1033): Preferred language for the organization.
                ///     (Russian - 1049): Предпочитаемый язык для организации.
                /// 
                /// SchemaName: LanguageCode
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = Locale
                ///</summary>
                public const string languagecode = "languagecode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Locale
                ///     (Russian - 1049): Локаль
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the locale of the organization.
                ///     (Russian - 1049): Уникальный идентификатор локали организации.
                /// 
                /// SchemaName: LocaleId
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = Locale
                ///</summary>
                public const string localeid = "localeid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Long Date Format
                ///     (Russian - 1049): Полный формат даты
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies how the Long Date format is displayed in Microsoft Dynamics 365.
                ///     (Russian - 1049): Информация о способе отображения полного формата даты в Microsoft Dynamics 365.
                /// 
                /// SchemaName: LongDateFormatCode
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string longdateformatcode = "longdateformatcode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Lower Threshold For Mailbox Intermittent Issue
                ///     (Russian - 1049): Более низкий порог для временной проблемы с почтовым ящиком
                /// 
                /// Description:
                ///     (English - United States - 1033): Lower Threshold For Mailbox Intermittent Issue.
                ///     (Russian - 1049): Более низкий порог для временной проблемы с почтовым ящиком.
                /// 
                /// SchemaName: MailboxIntermittentIssueMinRange
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string mailboxintermittentissueminrange = "mailboxintermittentissueminrange";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Lower Threshold For Mailbox Permanent Issue.
                ///     (Russian - 1049): Более низкий порог для постоянной проблемы с почтовым ящиком.
                /// 
                /// Description:
                /// 
                /// SchemaName: MailboxPermanentIssueMinRange
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string mailboxpermanentissueminrange = "mailboxpermanentissueminrange";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Max Appointment Duration
                ///     (Russian - 1049): Максимальная продолжительность встречи
                /// 
                /// Description:
                ///     (English - United States - 1033): Maximum number of days an appointment can last.
                ///     (Russian - 1049): Наибольшее число дней, которые может длиться встреча.
                /// 
                /// SchemaName: MaxAppointmentDurationDays
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string maxappointmentdurationdays = "maxappointmentdurationdays";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Maximum number of conditions allowed for mobile offline filters
                ///     (Russian - 1049): Максимально разрешенное количество условий для фильтров Mobile Offline
                /// 
                /// Description:
                /// 
                /// SchemaName: MaxConditionsForMobileOfflineFilters
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string maxconditionsformobileofflinefilters = "maxconditionsformobileofflinefilters";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Maximum depth for hierarchy security propagation.
                ///     (Russian - 1049): Максимальная глубина распространения иерархической безопасности.
                /// 
                /// Description:
                /// 
                /// SchemaName: MaxDepthForHierarchicalSecurityModel
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string maxdepthforhierarchicalsecuritymodel = "maxdepthforhierarchicalsecuritymodel";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Max Folder Based Tracking Mappings
                ///     (Russian - 1049): Макс. сопоставлений отслеживания на основе папок
                /// 
                /// Description:
                ///     (English - United States - 1033): Maximum number of Folder Based Tracking mappings user can add
                ///     (Russian - 1049): Максимальное число сопоставлений отслеживания на основе папок, которое может добавлять пользователь
                /// 
                /// SchemaName: MaxFolderBasedTrackingMappings
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 1    MaxValue = 25
                /// Format = None
                ///</summary>
                public const string maxfolderbasedtrackingmappings = "maxfolderbasedtrackingmappings";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Maximum active business process flows per entity
                ///     (Russian - 1049): Максимальное число активных последовательностей операций бизнес-процесса на одну сущность
                /// 
                /// Description:
                ///     (English - United States - 1033): Maximum number of active business process flows allowed per entity
                ///     (Russian - 1049): Максимальное допустимое число активных последовательностей операций бизнес-процесса на одну сущность
                /// 
                /// SchemaName: MaximumActiveBusinessProcessFlowsAllowedPerEntity
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 1    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string maximumactivebusinessprocessflowsallowedperentity = "maximumactivebusinessprocessflowsallowedperentity";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Product Properties Item Limit
                ///     (Russian - 1049): Предел элементов свойств продукта
                /// 
                /// Description:
                ///     (English - United States - 1033): Restrict the maximum number of product properties for a product family/bundle
                ///     (Russian - 1049): Ограничить максимальное число свойств продукта для семейства или набора продуктов
                /// 
                /// SchemaName: MaximumDynamicPropertiesAllowed
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string maximumdynamicpropertiesallowed = "maximumdynamicpropertiesallowed";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Maximum number of active SLA allowed per entity in online
                ///     (Russian - 1049): Максимальное допустимое число активных SLA на одну сущность в режиме подключения к сети
                /// 
                /// Description:
                /// 
                /// SchemaName: MaximumEntitiesWithActiveSLA
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string maximumentitieswithactivesla = "maximumentitieswithactivesla";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Maximum number of active SLA KPI allowed per entity in online
                ///     (Russian - 1049): Максимальное допустимое число активных KPI SLA на одну сущность в режиме подключения к сети
                /// 
                /// Description:
                ///     (English - United States - 1033): Maximum number of SLA KPI per active SLA allowed for entity in online
                ///     (Russian - 1049): Максимальное допустимое число KPI SLA на активное SLA на одну сущность в режиме подключения к сети
                /// 
                /// SchemaName: MaximumSLAKPIPerEntityWithActiveSLA
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string maximumslakpiperentitywithactivesla = "maximumslakpiperentitywithactivesla";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Max Tracking Number
                ///     (Russian - 1049): Макс. знач. кода отслеживания
                /// 
                /// Description:
                ///     (English - United States - 1033): Maximum tracking number before recycling takes place.
                ///     (Russian - 1049): Максимальное значение кода отслеживания перед повторным использованием.
                /// 
                /// SchemaName: MaximumTrackingNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string maximumtrackingnumber = "maximumtrackingnumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Bundle Item Limit
                ///     (Russian - 1049): Предел по элементам набора
                /// 
                /// Description:
                ///     (English - United States - 1033): Restrict the maximum no of items in a bundle
                ///     (Russian - 1049): Ограничить максимальное число элементов в наборе
                /// 
                /// SchemaName: MaxProductsInBundle
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string maxproductsinbundle = "maxproductsinbundle";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Max Records For Excel Export
                ///     (Russian - 1049): Макс. число экспортируемых в Excel записей
                /// 
                /// Description:
                ///     (English - United States - 1033): Maximum number of records that will be exported to a static Microsoft Office Excel worksheet when exporting from the grid.
                ///     (Russian - 1049): Наибольшее число записей, которые будут экспортированы в статический лист Microsoft Office Excel при экспорте из таблицы.
                /// 
                /// SchemaName: MaxRecordsForExportToExcel
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string maxrecordsforexporttoexcel = "maxrecordsforexporttoexcel";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Max Records Filter Selection
                ///     (Russian - 1049): Макс. число выбранных для фильтрации записей
                /// 
                /// Description:
                ///     (English - United States - 1033): Maximum number of lookup and picklist records that can be selected by user for filtering.
                ///     (Russian - 1049): Максимальное число записей поиска и полей выбора, которые могут быть выбраны для фильтрации пользователем.
                /// 
                /// SchemaName: MaxRecordsForLookupFilters
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string maxrecordsforlookupfilters = "maxrecordsforlookupfilters";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Max supported IE version
                ///     (Russian - 1049): Максимальная поддерживаемая версия IE
                /// 
                /// Description:
                ///     (English - United States - 1033): The maximum version of IE to run browser emulation for in Outlook client
                ///     (Russian - 1049): Максимальная версия IE для запуска эмуляции браузера в клиенте Outlook
                /// 
                /// SchemaName: MaxSupportedInternetExplorerVersion
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string maxsupportedinternetexplorerversion = "maxsupportedinternetexplorerversion";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Max Upload File Size
                ///     (Russian - 1049): Макс. размер отправляемых файлов
                /// 
                /// Description:
                ///     (English - United States - 1033): Maximum allowed size of an attachment.
                ///     (Russian - 1049): Наибольший разрешенный размер вложения.
                /// 
                /// SchemaName: MaxUploadFileSize
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string maxuploadfilesize = "maxuploadfilesize";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Max No Of Mailboxes To Enable For Verbose Logging
                ///     (Russian - 1049): Максимальное число почтовых ящиков для подробного ведения журнала
                /// 
                /// Description:
                ///     (English - United States - 1033): Maximum number of mailboxes that can be toggled for verbose logging
                ///     (Russian - 1049): Максимальное число почтовых ящиков, которые можно включить для подробного ведения журнала
                /// 
                /// SchemaName: MaxVerboseLoggingMailbox
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string maxverboseloggingmailbox = "maxverboseloggingmailbox";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Maximum number of sync cycles for which verbose logging will be enabled by default
                ///     (Russian - 1049): Максимальное число циклов синхронизации, для которых по умолчанию включается подробное ведение журнала
                /// 
                /// Description:
                /// 
                /// SchemaName: MaxVerboseLoggingSyncCycles
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string maxverboseloggingsynccycles = "maxverboseloggingsynccycles";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): The last date/time for never expired metadata tracking deleted objects
                ///     (Russian - 1049): Последние дата/время для никогда не истекавших по сроку действия метаданных отслеживания удаленных объектов
                /// 
                /// Description:
                ///     (English - United States - 1033): What is the last date/time where there are metadata tracking deleted objects that have never been outside of the expiration period.
                ///     (Russian - 1049): Последние дата/время, для которых имеются метаданные, отслеживающие удаленные объекты, которые никогда не выходили за рамки периода истечения срока действия.
                /// 
                /// SchemaName: MetadataSyncLastTimeOfNeverExpiredDeletedObjects
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: False    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateAndTime
                ///</summary>
                public const string metadatasynclasttimeofneverexpireddeletedobjects = "metadatasynclasttimeofneverexpireddeletedobjects";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Metadata sync version
                ///     (Russian - 1049): Версия синхронизации метаданных
                /// 
                /// Description:
                ///     (English - United States - 1033): Contains the maximum version number for attributes used by metadata synchronization that have changed.
                ///     (Russian - 1049): Содержит максимальный номер версии для измененных атрибутов, используемых синхронизацией метаданных.
                /// 
                /// SchemaName: MetadataSyncTimestamp
                /// BigIntAttributeMetadata    AttributeType: BigInt    AttributeTypeName: BigIntType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: False    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -9223372036854775808    MaxValue = 9223372036854775807
                ///</summary>
                public const string metadatasynctimestamp = "metadatasynctimestamp";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Min Address Synchronization Frequency
                ///     (Russian - 1049): Мин. периодичность синхронизации адресов
                /// 
                /// Description:
                ///     (English - United States - 1033): Normal polling frequency used for address book synchronization in Microsoft Office Outlook.
                ///     (Russian - 1049): Обычная частота опросов для синхронизации адресной книги в Microsoft Office Outlook.
                /// 
                /// SchemaName: MinAddressBookSyncInterval
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string minaddressbooksyncinterval = "minaddressbooksyncinterval";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Min Offline Synchronization Frequency
                ///     (Russian - 1049): Мин. периодичность синхронизации в автономном режиме
                /// 
                /// Description:
                ///     (English - United States - 1033): Normal polling frequency used for background offline synchronization in Microsoft Office Outlook.
                ///     (Russian - 1049): Обычная частота опросов для фоновой синхронизации автономного режима в Microsoft Office Outlook.
                /// 
                /// SchemaName: MinOfflineSyncInterval
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string minofflinesyncinterval = "minofflinesyncinterval";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Min Synchronization Frequency
                ///     (Russian - 1049): Мин. периодичность синхронизации
                /// 
                /// Description:
                ///     (English - United States - 1033): Minimum allowed time between scheduled Outlook synchronizations.
                ///     (Russian - 1049): Минимальное разрешенное время между запланированными операциями синхронизации Outlook.
                /// 
                /// SchemaName: MinOutlookSyncInterval
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string minoutlooksyncinterval = "minoutlooksyncinterval";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Minimum number of user license required for mobile offline service by production/preview organization
                /// 
                /// Description:
                /// 
                /// SchemaName: MobileOfflineMinLicenseProd
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string mobileofflineminlicenseprod = "mobileofflineminlicenseprod";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Minimum number of user license required for mobile offline service by trial organization
                /// 
                /// Description:
                /// 
                /// SchemaName: MobileOfflineMinLicenseTrial
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string mobileofflineminlicensetrial = "mobileofflineminlicensetrial";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Sync interval for mobile offline.
                ///     (Russian - 1049): Интервал синхронизации для Mobile Offline.
                /// 
                /// Description:
                /// 
                /// SchemaName: MobileOfflineSyncInterval
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string mobileofflinesyncinterval = "mobileofflinesyncinterval";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By
                ///     (Russian - 1049): Изменено
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who last modified the organization.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, последним изменившего организацию.
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
                ///     (English - United States - 1033): Date and time when the organization was last modified.
                ///     (Russian - 1049): Дата и время последнего изменения организации.
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who last modified the organization.
                ///     (Russian - 1049): Уникальный идентификатор пользователя-делегата, последним изменившего организацию.
                /// 
                /// SchemaName: ModifiedOnBehalfBy
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
                public const string modifiedonbehalfby = "modifiedonbehalfby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Organization Name
                ///     (Russian - 1049): Название организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Name of the organization. The name is set when Microsoft CRM is installed and should not be changed.
                ///     (Russian - 1049): Название организации. Это название задается при установке Microsoft CRM. Изменять его нельзя.
                /// 
                /// SchemaName: Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 160
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string name = "name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Negative Currency Format
                ///     (Russian - 1049): Формат отрицательных денежных сумм
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies how negative currency numbers are displayed throughout Microsoft Dynamics 365.
                ///     (Russian - 1049): Информация о способе отображения отрицательных значений валют в системе Microsoft Dynamics 365.
                /// 
                /// SchemaName: NegativeCurrencyFormatCode
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string negativecurrencyformatcode = "negativecurrencyformatcode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Negative Format
                ///     (Russian - 1049): Форматирование отрицательных сумм
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies how negative numbers are displayed throughout Microsoft CRM.
                ///     (Russian - 1049): Сведения, определяющие формат отображения отрицательных значений в Microsoft CRM.
                /// 
                /// SchemaName: NegativeFormatCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet organization_negativeformatcode
                /// DefaultFormValue = 0
                ///</summary>
                public const string negativeformatcode = "negativeformatcode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Next Entity Type Code
                ///     (Russian - 1049): Следующий код типа сущностей
                /// 
                /// Description:
                ///     (English - United States - 1033): Next entity type code to use for custom entities.
                ///     (Russian - 1049): Следующий код типа сущностей, используемый для настраиваемых сущностей.
                /// 
                /// SchemaName: NextCustomObjectTypeCode
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: False    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 10000    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string nextcustomobjecttypecode = "nextcustomobjecttypecode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Next Tracking Number
                ///     (Russian - 1049): Следующий код отслеживания
                /// 
                /// Description:
                ///     (English - United States - 1033): Next token to be placed on the subject line of an email message.
                ///     (Russian - 1049): Следующий токен, вставляемый в строку темы сообщения электронной почты.
                /// 
                /// SchemaName: NextTrackingNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string nexttrackingnumber = "nexttrackingnumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Notify Mailbox Owner Of Email Server Level Alerts
                ///     (Russian - 1049): Уведомлять владельца почтового ящика об оповещениях на уровне сервера электронной почты
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether mailbox owners will be notified of email server profile level alerts.
                ///     (Russian - 1049): Указывает, будут ли владельцы почтовых ящиков получать уведомления об оповещениях уровня профиля сервера электронной почты.
                /// 
                /// SchemaName: NotifyMailboxOwnerOfEmailServerLevelAlerts
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string notifymailboxownerofemailserverlevelalerts = "notifymailboxownerofemailserverlevelalerts";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Number Format
                ///     (Russian - 1049): Форматирование чисел
                /// 
                /// Description:
                ///     (English - United States - 1033): Specification of how numbers are displayed throughout Microsoft CRM.
                ///     (Russian - 1049): Информация, определяющая формат отображения чисел в Microsoft CRM.
                /// 
                /// SchemaName: NumberFormat
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 2
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string numberformat = "numberformat";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Number Grouping Format
                ///     (Russian - 1049): Форматирование группирований чисел
                /// 
                /// Description:
                ///     (English - United States - 1033): Specifies how numbers are grouped in Microsoft Dynamics 365.
                ///     (Russian - 1049): Определение способа группировки чисел в Microsoft Dynamics 365.
                /// 
                /// SchemaName: NumberGroupFormat
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string numbergroupformat = "numbergroupformat";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Number Separator
                ///     (Russian - 1049): Разделитель чисел
                /// 
                /// Description:
                ///     (English - United States - 1033): Symbol used for number separation in Microsoft Dynamics 365.
                ///     (Russian - 1049): Символ разделителя групп разрядов в Microsoft Dynamics 365.
                /// 
                /// SchemaName: NumberSeparator
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 5
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string numberseparator = "numberseparator";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Office Apps Auto Deployment for this Organization
                ///     (Russian - 1049): Включить автоматическое развертывание приложений Office для этой организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the Office Apps auto deployment is enabled for the organization.
                ///     (Russian - 1049): Указывает, включено ли автоматическое развертывание приложений Office для организации.
                /// 
                /// SchemaName: OfficeAppsAutoDeploymentEnabled
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
                public const string officeappsautodeploymentenabled = "officeappsautodeploymentenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): The url to open the Delve
                ///     (Russian - 1049): URL-адрес, открывающий Delve
                /// 
                /// Description:
                ///     (English - United States - 1033): The url to open the Delve for the organization.
                ///     (Russian - 1049): URL-адрес, открывающий Delve для организации.
                /// 
                /// SchemaName: OfficeGraphDelveUrl
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1000
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string officegraphdelveurl = "officegraphdelveurl";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable OOB Price calculation
                ///     (Russian - 1049): Включить расчет цены OOB
                /// 
                /// Description:
                ///     (English - United States - 1033): Enable OOB pricing calculation logic for Opportunity, Quote, Order and Invoice entities.
                ///     (Russian - 1049): Включение логики расчета цен OOB для сущностей возможной сделки, предложения с расценками, заказа и счета.
                /// 
                /// SchemaName: OOBPriceCalculationEnabled
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string oobpricecalculationenabled = "oobpricecalculationenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Order Prefix
                ///     (Russian - 1049): Префикс заказов
                /// 
                /// Description:
                ///     (English - United States - 1033): Prefix to use for all orders throughout Microsoft Dynamics 365.
                ///     (Russian - 1049): Префикс, используемый для всех заказов в Microsoft Dynamics 365.
                /// 
                /// SchemaName: OrderPrefix
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string orderprefix = "orderprefix";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Organization
                ///     (Russian - 1049): Предприятие
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the organization.
                ///     (Russian - 1049): Уникальный идентификатор организации.
                /// 
                /// SchemaName: OrganizationId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string organizationid = "organizationid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Organization Database Organization Settings
                ///     (Russian - 1049): Параметры организации в базе данных организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Organization settings stored in Organization Database.
                ///     (Russian - 1049): Параметры организации, хранящиеся в базе данных организации.
                /// 
                /// SchemaName: OrgDbOrgSettings
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1073741823
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string orgdborgsettings = "orgdborgsettings";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable OrgInsights for this Organization
                ///     (Russian - 1049): Включить сведения об организации для этой организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether to turn on OrgInsights for the organization.
                ///     (Russian - 1049): Выберите, следует ли включать сведения об организации для организации.
                /// 
                /// SchemaName: OrgInsightsEnabled
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
                public const string orginsightsenabled = "orginsightsenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Parsed Table Column Prefix
                ///     (Russian - 1049): Префикс проанализированного столбца таблицы
                /// 
                /// Description:
                ///     (English - United States - 1033): Prefix used for parsed table columns.
                ///     (Russian - 1049): Префикс для разобранных столбцов таблиц.
                /// 
                /// SchemaName: ParsedTableColumnPrefix
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string parsedtablecolumnprefix = "parsedtablecolumnprefix";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Parsed Table Prefix
                ///     (Russian - 1049): Префикс проанализированной таблицы
                /// 
                /// Description:
                ///     (English - United States - 1033): Prefix used for parsed tables.
                ///     (Russian - 1049): Префикс для разобранных таблиц.
                /// 
                /// SchemaName: ParsedTablePrefix
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string parsedtableprefix = "parsedtableprefix";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Past Expansion Window
                ///     (Russian - 1049): Окно расширения в прошлое
                /// 
                /// Description:
                ///     (English - United States - 1033): Specifies the maximum number of months in past for which the recurring activities can be created.
                ///     (Russian - 1049): Указывает максимальное число месяцев в прошлом, для которых можно создать повторяющиеся действия.
                /// 
                /// SchemaName: PastExpansionWindow
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 1    MaxValue = 120
                /// Format = None
                ///</summary>
                public const string pastexpansionwindow = "pastexpansionwindow";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Picture
                ///     (Russian - 1049): Рисунок
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: Picture
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string picture = "picture";

                ///<summary>
                /// SchemaName: PinpointLanguageCode
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = Locale
                ///</summary>
                public const string pinpointlanguagecode = "pinpointlanguagecode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Plug-in Trace Log Setting
                ///     (Russian - 1049): Настройка журнала трассировки подключаемого модуля
                /// 
                /// Description:
                ///     (English - United States - 1033): Plug-in Trace Log Setting for the Organization.
                ///     (Russian - 1049): Настройка журнала трассировки подключаемых модулей для организации.
                /// 
                /// SchemaName: PluginTraceLogSetting
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet organization_plugintracelogsetting
                /// DefaultFormValue = 0
                ///</summary>
                public const string plugintracelogsetting = "plugintracelogsetting";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): PM Designator
                ///     (Russian - 1049): Обозначение PM
                /// 
                /// Description:
                ///     (English - United States - 1033): PM designator to use throughout Microsoft Dynamics 365.
                ///     (Russian - 1049): Обозначение времени после полудня для использования в Microsoft Dynamics 365.
                /// 
                /// SchemaName: PMDesignator
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 25
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string pmdesignator = "pmdesignator";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// Description:
                /// 
                /// SchemaName: PostMessageWhitelistDomains
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 500
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string postmessagewhitelistdomains = "postmessagewhitelistdomains";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Power BI feature for this Organization
                ///     (Russian - 1049): Включить функцию Power BI для этой организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the Power BI feature should be enabled for the organization.
                ///     (Russian - 1049): Указывает, должна ли быть включена функция Power BI для этой организации.
                /// 
                /// SchemaName: PowerBiFeatureEnabled
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Disable
                ///     (Russian - 1049): Отключить
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Enable
                ///     (Russian - 1049): Включить
                /// TrueOption = 1
                ///</summary>
                public const string powerbifeatureenabled = "powerbifeatureenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Pricing Decimal Precision
                ///     (Russian - 1049): Число десятичных знаков в ценах
                /// 
                /// Description:
                ///     (English - United States - 1033): Number of decimal places that can be used for prices.
                ///     (Russian - 1049): Число десятичных разрядов в ценах.
                /// 
                /// SchemaName: PricingDecimalPrecision
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 4
                /// Format = None
                ///</summary>
                public const string pricingdecimalprecision = "pricingdecimalprecision";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Privacy Statement URL
                ///     (Russian - 1049): URL-адрес заявления о конфиденциальности
                /// 
                /// Description:
                /// 
                /// SchemaName: PrivacyStatementUrl
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 500
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string privacystatementurl = "privacystatementurl";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Privilege User Group
                ///     (Russian - 1049): Привилегия группы пользователей
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the default privilege for users in the organization.
                ///     (Russian - 1049): Уникальный идентификатор привилегии по умолчанию для пользователей в организации.
                /// 
                /// SchemaName: PrivilegeUserGroupId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string privilegeusergroupid = "privilegeusergroupid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Privilege Reporting Group
                ///     (Russian - 1049): Привилегия группы подготовки отчетности
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: PrivReportingGroupId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string privreportinggroupid = "privreportinggroupid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Privilege Reporting Group Name
                ///     (Russian - 1049): Название привилегии группы подготовки отчетности
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: PrivReportingGroupName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string privreportinggroupname = "privreportinggroupname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Product Recommendations for this Organization
                ///     (Russian - 1049): Включить рекомендации продуктов для этой организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether to turn on product recommendations for the organization.
                ///     (Russian - 1049): Выберите, следует ли включать рекомендации продуктов для организации.
                /// 
                /// SchemaName: ProductRecommendationsEnabled
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
                public const string productrecommendationsenabled = "productrecommendationsenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Quick Find Record Limit Enabled
                ///     (Russian - 1049): Ограничение записей быстрого поиска включено
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether a quick find record limit should be enabled for this organization (allows for faster Quick Find queries but prevents overly broad searches).
                ///     (Russian - 1049): Указывает, следует ли включить ограничение записей быстрого поиска для этой организации (обеспечивает более быстрый поиск, но не допускает выполнение более общего поиска).
                /// 
                /// SchemaName: QuickFindRecordLimitEnabled
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string quickfindrecordlimitenabled = "quickfindrecordlimitenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Quote Prefix
                ///     (Russian - 1049): Префикс предложений с расценками
                /// 
                /// Description:
                ///     (English - United States - 1033): Prefix to use for all quotes throughout Microsoft Dynamics 365.
                ///     (Russian - 1049): Префикс, используемый для всех предложений с расценками в Microsoft Dynamics 365.
                /// 
                /// SchemaName: QuotePrefix
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string quoteprefix = "quoteprefix";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Recurrence Default Number of Occurrences
                ///     (Russian - 1049): Значение по умолчанию для повторов
                /// 
                /// Description:
                ///     (English - United States - 1033): Specifies the default value for number of occurrences field in the recurrence dialog.
                ///     (Russian - 1049): Указывает значение по умолчанию для поля числа повторов в диалоговом окне повторения.
                /// 
                /// SchemaName: RecurrenceDefaultNumberOfOccurrences
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 1    MaxValue = 999
                /// Format = None
                ///</summary>
                public const string recurrencedefaultnumberofoccurrences = "recurrencedefaultnumberofoccurrences";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Recurrence Expansion Job Batch Interval
                ///     (Russian - 1049): Интервал пакетного задания расширения повторения
                /// 
                /// Description:
                ///     (English - United States - 1033): Specifies the interval (in seconds) for pausing expansion job.
                ///     (Russian - 1049): Указывает интервал (в секундах) для приостановки задания расширения.
                /// 
                /// SchemaName: RecurrenceExpansionJobBatchInterval
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string recurrenceexpansionjobbatchinterval = "recurrenceexpansionjobbatchinterval";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Recurrence Expansion On Demand Job Batch Size
                ///     (Russian - 1049): Размер пакетного задания расширения повторения с запуском вручную
                /// 
                /// Description:
                ///     (English - United States - 1033): Specifies the value for number of instances created in on demand job in one shot.
                ///     (Russian - 1049): Указывает число экземпляров, создаваемых заданием с запуском вручную за один раз.
                /// 
                /// SchemaName: RecurrenceExpansionJobBatchSize
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string recurrenceexpansionjobbatchsize = "recurrenceexpansionjobbatchsize";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Recurrence Expansion Synchronization Create Maximum
                ///     (Russian - 1049): Максимальное количество создаваемых синхронизаций расширения повторения
                /// 
                /// Description:
                ///     (English - United States - 1033): Specifies the maximum number of instances to be created synchronously after creating a recurring appointment.
                ///     (Russian - 1049): Указывает максимальное число экземпляров, синхронно создаваемых после создания повторяющейся встречи.
                /// 
                /// SchemaName: RecurrenceExpansionSynchCreateMax
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 1    MaxValue = 1000
                /// Format = None
                ///</summary>
                public const string recurrenceexpansionsynchcreatemax = "recurrenceexpansionsynchcreatemax";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Reference SiteMap XML
                ///     (Russian - 1049): XML ссылочной карты сайта
                /// 
                /// Description:
                ///     (English - United States - 1033): XML string that defines the navigation structure for the application. This is the site map from the previously upgraded build and is used in a 3-way merge during upgrade.
                ///     (Russian - 1049): Строка XML, определяющая структуру переходов для приложения. Это карта сайта из ранее обновленной сборки, которая используется в трехстороннем слиянии при обновлении.
                /// 
                /// SchemaName: ReferenceSiteMapXml
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1073741823
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string referencesitemapxml = "referencesitemapxml";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Render Secure Frame For Email
                ///     (Russian - 1049): Отображать безопасные фреймы в электронной почте
                /// 
                /// Description:
                ///     (English - United States - 1033): Flag to render the body of email in the Web form in an IFRAME with the security='restricted' attribute set. This is additional security but can cause a credentials prompt.
                ///     (Russian - 1049): Флаг для отображения текста электронного сообщения в веб-форме и в рамке IFRAME со значением атрибута security='restricted'. Это обеспечивает более высокий уровень безопасности, но может потребоваться еще раз ввести учетные данные.
                /// 
                /// SchemaName: RenderSecureIFrameForEmail
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
                public const string rendersecureiframeforemail = "rendersecureiframeforemail";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Reporting Group
                ///     (Russian - 1049): Группа подготовки отчетности
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: ReportingGroupId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string reportinggroupid = "reportinggroupid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Reporting Group Name
                ///     (Russian - 1049): Название группы подготовки отчетности
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: ReportingGroupName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string reportinggroupname = "reportinggroupname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Report Script Errors
                ///     (Russian - 1049): Ошибки скрипта отчета
                /// 
                /// Description:
                ///     (English - United States - 1033): Picklist for selecting the organization preference for reporting scripting errors.
                ///     (Russian - 1049): Поле выбора, отвечающее за предпочтения организации в области сообщения об ошибках в скриптах.
                /// 
                /// SchemaName: ReportScriptErrors
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet organization_reportscripterrors
                /// DefaultFormValue = -1
                ///</summary>
                public const string reportscripterrors = "reportscripterrors";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Approval For Queue Email Required
                ///     (Russian - 1049): Утверждение сообщений в очереди требуется?
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether Send As Other User privilege is enabled.
                ///     (Russian - 1049): Указывает, включена ли привилегия отправления от имени другого пользователя.
                /// 
                /// SchemaName: RequireApprovalForQueueEmail
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
                public const string requireapprovalforqueueemail = "requireapprovalforqueueemail";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Approval For User Email Required
                ///     (Russian - 1049): Утверждение сообщений пользователя требуется?
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether Send As Other User privilege is enabled.
                ///     (Russian - 1049): Указывает, включена ли привилегия отправления от имени другого пользователя.
                /// 
                /// SchemaName: RequireApprovalForUserEmail
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
                public const string requireapprovalforuseremail = "requireapprovalforuseremail";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Restrict Status Update
                ///     (Russian - 1049): Ограничить обновление состояния
                /// 
                /// Description:
                ///     (English - United States - 1033): Flag to restrict Update on incident.
                ///     (Russian - 1049): Флаг для ограничения обновления в случае инцидента.
                /// 
                /// SchemaName: RestrictStatusUpdate
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
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
                public const string restrictstatusupdate = "restrictstatusupdate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Error status of Relationship Insights provisioning.
                ///     (Russian - 1049): Состояние ошибки подготовки компонента "Аналитический обзор отношений".
                /// 
                /// Description:
                /// 
                /// SchemaName: RiErrorStatus
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string rierrorstatus = "rierrorstatus";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Sample Data Import
                ///     (Russian - 1049): Импорт демонстрационных данных
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the sample data import job.
                ///     (Russian - 1049): Уникальный идентификатор задания импорта демонстрационных данных.
                /// 
                /// SchemaName: SampleDataImportId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string sampledataimportid = "sampledataimportid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Customization Name Prefix
                ///     (Russian - 1049): Префикс названия настройки
                /// 
                /// Description:
                ///     (English - United States - 1033): Prefix used for custom entities and attributes.
                ///     (Russian - 1049): Префикс, используемый для настраиваемых сущностей и атрибутов.
                /// 
                /// SchemaName: SchemaNamePrefix
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 8
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string schemanameprefix = "schemanameprefix";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Session timeout enabled
                ///     (Russian - 1049): Закрытие сеанса по истечении времени ожидания включено
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether session timeout is enabled
                ///     (Russian - 1049): Сведения о том, включено ли закрытие сеанса по истечении времени ожидания
                /// 
                /// SchemaName: SessionTimeoutEnabled
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
                public const string sessiontimeoutenabled = "sessiontimeoutenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Session timeout in minutes
                ///     (Russian - 1049): Время ожидания закрытия сеанса в минутах
                /// 
                /// Description:
                /// 
                /// SchemaName: SessionTimeoutInMins
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string sessiontimeoutinmins = "sessiontimeoutinmins";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Session timeout reminder in minutes
                ///     (Russian - 1049): Время до напоминания о закрытии сеанса в минутах
                /// 
                /// Description:
                /// 
                /// SchemaName: SessionTimeoutReminderInMins
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string sessiontimeoutreminderinmins = "sessiontimeoutreminderinmins";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Choose SharePoint Deployment Type
                ///     (Russian - 1049): Выберите тип развертывания SharePoint
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates which SharePoint deployment type is configured for Server to Server. (Online or On-Premises)
                ///     (Russian - 1049): Указывает, какой тип развертывания SharePoint настроен для проверки подлинности типа "сервер-сервер". (Online или локальный)
                /// 
                /// SchemaName: SharePointDeploymentType
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet organization_sharepointdeploymenttype
                /// DefaultFormValue = 0
                ///</summary>
                public const string sharepointdeploymenttype = "sharepointdeploymenttype";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Share To Previous Owner On Assign
                ///     (Russian - 1049): Cовместно использовать с предыдущим ответственным при назначении
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether to share to previous owner on assign.
                ///     (Russian - 1049): Сведения указывающие, следует ли совместно использовать с предыдущим ответственным при назначении.
                /// 
                /// SchemaName: ShareToPreviousOwnerOnAssign
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string sharetopreviousowneronassign = "sharetopreviousowneronassign";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Show KBArticle deprecation message to user
                ///     (Russian - 1049): Показывать пользователю сообщение о недопустимой статье базы знаний
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether to display a KB article deprecation notification to the user.
                ///     (Russian - 1049): Выберите, следует ли отображать пользователю уведомление о недопустимой статье базы знаний.
                /// 
                /// SchemaName: ShowKBArticleDeprecationNotification
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
                public const string showkbarticledeprecationnotification = "showkbarticledeprecationnotification";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Show Week Number
                ///     (Russian - 1049): Отображать номер недели
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether to display the week number in calendar displays throughout Microsoft CRM.
                ///     (Russian - 1049): Информация, указывающая, следует ли отображать номер недели в календарях системы Microsoft CRM.
                /// 
                /// SchemaName: ShowWeekNumber
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
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
                public const string showweeknumber = "showweeknumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): CRMForOutlookDownloadURL
                /// 
                /// Description:
                ///     (English - United States - 1033): CRM for Outlook Download URL
                ///     (Russian - 1049): Ссылка для скачивания CRM для Outlook
                /// 
                /// SchemaName: SignupOutlookDownloadFWLink
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string signupoutlookdownloadfwlink = "signupoutlookdownloadfwlink";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): SiteMap XML
                ///     (Russian - 1049): XML карты сайта
                /// 
                /// Description:
                ///     (English - United States - 1033): XML string that defines the navigation structure for the application.
                ///     (Russian - 1049): Строка XML, определяющая структуру переходов для приложения.
                /// 
                /// SchemaName: SiteMapXml
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string sitemapxml = "sitemapxml";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): SLA pause states
                ///     (Russian - 1049): Состояния приостановки SLA
                /// 
                /// Description:
                ///     (English - United States - 1033): Contains the on hold case status values.
                ///     (Russian - 1049): Содержит значения состояния обращения "приостановлено"
                /// 
                /// SchemaName: SlaPauseStates
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1073741823
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string slapausestates = "slapausestates";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Social Insights Enabled
                ///     (Russian - 1049): Программа "Социальные данные" включена
                /// 
                /// Description:
                ///     (English - United States - 1033): Flag for whether the organization is using Social Insights.
                ///     (Russian - 1049): Флаг, указывающий, использует ли организация программу "Социальные данные".
                /// 
                /// SchemaName: SocialInsightsEnabled
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string socialinsightsenabled = "socialinsightsenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Social Insights instance identifier
                ///     (Russian - 1049): Идентификатор экземпляра программы "Социальные данные".
                /// 
                /// Description:
                ///     (English - United States - 1033): Identifier for the Social Insights instance for the organization.
                ///     (Russian - 1049): Идентификатор экземпляра программы "Социальные данные" для организации.
                /// 
                /// SchemaName: SocialInsightsInstance
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 2048
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string socialinsightsinstance = "socialinsightsinstance";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Social Insights Terms of Use
                ///     (Russian - 1049): Условия использования программы "Социальные данные"
                /// 
                /// Description:
                ///     (English - United States - 1033): Flag for whether the organization has accepted the Social Insights terms of use.
                ///     (Russian - 1049): Флаг, указывающий, приняла ли организация условия использования программы "Социальные данные".
                /// 
                /// SchemaName: SocialInsightsTermsAccepted
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string socialinsightstermsaccepted = "socialinsightstermsaccepted";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Sort
                ///     (Russian - 1049): Сортировка
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: SortId
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string sortid = "sortid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): SQL Access Group
                ///     (Russian - 1049): Группа доступа к SQL
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: SqlAccessGroupId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string sqlaccessgroupid = "sqlaccessgroupid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): SQL Access Group Name
                ///     (Russian - 1049): Название группы доступа к SQL
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: SqlAccessGroupName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string sqlaccessgroupname = "sqlaccessgroupname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is SQM Enabled
                ///     (Russian - 1049): SQM-показатели включены?
                /// 
                /// Description:
                ///     (English - United States - 1033): Setting for SQM data collection, 0 no, 1 yes enabled
                ///     (Russian - 1049): Параметр для сбора данных SQM, 0 - нет, 1 - да
                /// 
                /// SchemaName: SQMEnabled
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
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
                public const string sqmenabled = "sqmenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Support User
                ///     (Russian - 1049): Пользователь поддержки
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the support user for the organization.
                ///     (Russian - 1049): Уникальный идентификатор пользователя поддержки организации.
                /// 
                /// SchemaName: SupportUserId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string supportuserid = "supportuserid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is SLA suppressed
                ///     (Russian - 1049): SLA является скрытым
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether SLA is suppressed.
                ///     (Russian - 1049): Указывает, является ли SLA скрытым.
                /// 
                /// SchemaName: SuppressSLA
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
                public const string suppresssla = "suppresssla";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): System User
                ///     (Russian - 1049): Системный пользователь
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the system user for the organization.
                ///     (Russian - 1049): Уникальный идентификатор системного пользователя организации.
                /// 
                /// SchemaName: SystemUserId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string systemuserid = "systemuserid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Auto-Tag Max Cycles
                ///     (Russian - 1049): Макс. число циклов авт. присв. тегов
                /// 
                /// Description:
                ///     (English - United States - 1033): Maximum number of aggressive polling cycles executed for email auto-tagging when a new email is received.
                ///     (Russian - 1049): Максимальное число агрессивных циклов опроса, выполняемых над новым сообщением электронной почты при его получении для автоматического присвоения тегов.
                /// 
                /// SchemaName: TagMaxAggressiveCycles
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = Null    MaxValue = Null
                /// Format = None
                ///</summary>
                public const string tagmaxaggressivecycles = "tagmaxaggressivecycles";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Auto-Tag Interval
                ///     (Russian - 1049): Периодичность авт. присв. тегов
                /// 
                /// Description:
                ///     (English - United States - 1033): Normal polling frequency used for email receive auto-tagging in outlook.
                ///     (Russian - 1049): Обычная периодичность опроса, используемая для автоматического присвоения тегов при получении сообщений в Outlook.
                /// 
                /// SchemaName: TagPollingPeriod
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string tagpollingperiod = "tagpollingperiod";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Task Flow processes for this Organization
                ///     (Russian - 1049): Включить процессы потока задач для этой организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether to turn on task flows for the organization.
                ///     (Russian - 1049): Выберите, следует ли включать потоки задач для организации.
                /// 
                /// SchemaName: TaskBasedFlowEnabled
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
                public const string taskbasedflowenabled = "taskbasedflowenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Enable Text Analytics for this Organization
                ///     (Russian - 1049): Включить текстовую аналитику для этой организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether to turn on text analytics for the organization.
                ///     (Russian - 1049): Выберите, следует ли включать текстовую аналитику для организации.
                /// 
                /// SchemaName: TextAnalyticsEnabled
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
                public const string textanalyticsenabled = "textanalyticsenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Time Format Code
                ///     (Russian - 1049): Код форматирования времени
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies how the time is displayed throughout Microsoft CRM.
                ///     (Russian - 1049): Указывает, как в Microsoft CRM отображается время.
                /// 
                /// SchemaName: TimeFormatCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet organization_timeformatcode
                /// DefaultFormValue = -1
                ///</summary>
                public const string timeformatcode = "timeformatcode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Time Format String
                ///     (Russian - 1049): Строка формата времени
                /// 
                /// Description:
                ///     (English - United States - 1033): Text for how time is displayed in Microsoft Dynamics 365.
                ///     (Russian - 1049): Текст, указывающий на способ отображения времени в Microsoft Dynamics 365.
                /// 
                /// SchemaName: TimeFormatString
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 255
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string timeformatstring = "timeformatstring";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Time Separator
                ///     (Russian - 1049): Разделитель компонентов времени
                /// 
                /// Description:
                ///     (English - United States - 1033): Text for how the time separator is displayed throughout Microsoft Dynamics 365.
                ///     (Russian - 1049): Текст, описывающий отображение разделителя времени в Microsoft Dynamics 365.
                /// 
                /// SchemaName: TimeSeparator
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 5
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string timeseparator = "timeseparator";

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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -1    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string timezoneruleversionnumber = "timezoneruleversionnumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Token Expiration Duration
                ///     (Russian - 1049): Продолжительность окончания срока действия токена
                /// 
                /// Description:
                ///     (English - United States - 1033): Duration used for token expiration.
                /// 
                /// SchemaName: TokenExpiry
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = Null    MaxValue = Null
                /// Format = None
                ///</summary>
                public const string tokenexpiry = "tokenexpiry";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Token Key
                ///     (Russian - 1049): Ключ токена
                /// 
                /// Description:
                ///     (English - United States - 1033): Token key.
                ///     (Russian - 1049): Ключ токена.
                /// 
                /// SchemaName: TokenKey
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: False    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 90
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string tokenkey = "tokenkey";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Tracking Prefix
                ///     (Russian - 1049): Префикс отслеживания
                /// 
                /// Description:
                ///     (English - United States - 1033): History list of tracking token prefixes.
                ///     (Russian - 1049): Список журнала префиксов токена отслеживания.
                /// 
                /// SchemaName: TrackingPrefix
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string trackingprefix = "trackingprefix";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Tracking Token Base
                ///     (Russian - 1049): База токена отслеживания
                /// 
                /// Description:
                ///     (English - United States - 1033): Base number used to provide separate tracking token identifiers to users belonging to different deployments.
                ///     (Russian - 1049): Базовое число, используемое для предоставления различающихся идентификаторов токенов отслеживания пользователям, входящим в разные развертывания.
                /// 
                /// SchemaName: TrackingTokenIdBase
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string trackingtokenidbase = "trackingtokenidbase";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Tracking Token Digits
                ///     (Russian - 1049): Цифр в токене отслеживания
                /// 
                /// Description:
                ///     (English - United States - 1033): Number of digits used to represent a tracking token identifier.
                ///     (Russian - 1049): Число цифр, используемых в идентификаторе токена отслеживания.
                /// 
                /// SchemaName: TrackingTokenIdDigits
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = Null    MaxValue = Null
                /// Format = None
                ///</summary>
                public const string trackingtokeniddigits = "trackingtokeniddigits";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Unique String Length
                ///     (Russian - 1049): Уникальная длина строки
                /// 
                /// Description:
                ///     (English - United States - 1033): Number of characters appended to invoice, quote, and order numbers.
                ///     (Russian - 1049): Количество знаков, добавляемых к счету, предложению с расценками и номерам заказов.
                /// 
                /// SchemaName: UniqueSpecifierLength
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 4    MaxValue = 6
                /// Format = None
                ///</summary>
                public const string uniquespecifierlength = "uniquespecifierlength";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Set To,cc,bcc fields as unresolved if multiple matches are found
                ///     (Russian - 1049): Пометить поля "Кому", "Копия", "Скрытая копия" как неопределенные при обнаружении нескольких совпадений
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether email address should be unresolved if multiple matches are found
                ///     (Russian - 1049): Указывает, следует ли помечать адрес электронной почты как неопределенный при обнаружении нескольких совпадений
                /// 
                /// SchemaName: UnresolveEmailAddressIfMultipleMatch
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
                public const string unresolveemailaddressifmultiplematch = "unresolveemailaddressifmultiplematch";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Use Inbuilt Rule For Default Pricelist Selection
                ///     (Russian - 1049): Использовать встроенное правило для выбора прайс-листа по умолчанию
                /// 
                /// Description:
                ///     (English - United States - 1033): Flag indicates whether to Use Inbuilt Rule For DefaultPricelist.
                ///     (Russian - 1049): Этот флаг указывает, будет ли использоваться встроенное правило для DefaultPricelist.
                /// 
                /// SchemaName: UseInbuiltRuleForDefaultPricelistSelection
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string useinbuiltrulefordefaultpricelistselection = "useinbuiltrulefordefaultpricelistselection";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Legacy Form Rendering
                ///     (Russian - 1049): Отображение форм предыдущих версий
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether to use legacy form rendering.
                ///     (Russian - 1049): Укажите, следует ли использовать отображение форм предыдущих версий.
                /// 
                /// SchemaName: UseLegacyRendering
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
                public const string uselegacyrendering = "uselegacyrendering";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Use position hierarchy
                ///     (Russian - 1049): Использовать иерархию положений
                /// 
                /// Description:
                /// 
                /// SchemaName: UsePositionHierarchy
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
                public const string usepositionhierarchy = "usepositionhierarchy";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): User Authentication Auditing Interval
                ///     (Russian - 1049): Интервал аудита проверки подлинности пользователей
                /// 
                /// Description:
                ///     (English - United States - 1033): The interval at which user access is checked for auditing.
                ///     (Russian - 1049): Интервал, с которым проверяется доступ пользователей в целях аудита.
                /// 
                /// SchemaName: UserAccessAuditingInterval
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string useraccessauditinginterval = "useraccessauditinginterval";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Use Read-Optimized Form
                ///     (Russian - 1049): Использовать оптимальную форму для чтения
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the read-optimized form should be enabled for this organization.
                ///     (Russian - 1049): Указывает, должна ли быть включена оптимальная форма для чтения для этой организации.
                /// 
                /// SchemaName: UseReadForm
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
                public const string usereadform = "usereadform";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): User Group
                ///     (Russian - 1049): Группа пользователей
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the default group of users in the organization.
                ///     (Russian - 1049): Уникальный идентификатор группы пользователей по умолчанию в организации.
                /// 
                /// SchemaName: UserGroupId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string usergroupid = "usergroupid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): User Skype Protocol
                ///     (Russian - 1049): Протокол Skype пользователя
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates default protocol selected for organization.
                ///     (Russian - 1049): Указывает выбранный по умолчанию для организации протокол.
                /// 
                /// SchemaName: UseSkypeProtocol
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string useskypeprotocol = "useskypeprotocol";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): UTC Conversion Time Zone Code
                ///     (Russian - 1049): Код часового пояса (преобразование в UTC)
                /// 
                /// Description:
                ///     (English - United States - 1033): Time zone code that was in use when the record was created.
                ///     (Russian - 1049): Код часового пояса, использовавшийся при создании записи.
                /// 
                /// SchemaName: UTCConversionTimeZoneCode
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -1    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string utcconversiontimezonecode = "utcconversiontimezonecode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): V3 Callout Hash
                ///     (Russian - 1049): Хэш метки V3
                /// 
                /// Description:
                ///     (English - United States - 1033): Hash of the V3 callout configuration file.
                ///     (Russian - 1049): Хэш файла конфигурации выноски V3.
                /// 
                /// SchemaName: V3CalloutConfigHash
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string v3calloutconfighash = "v3calloutconfighash";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Version Number
                ///     (Russian - 1049): Номер версии
                /// 
                /// Description:
                ///     (English - United States - 1033): Version number of the organization.
                ///     (Russian - 1049): Номер версии организации.
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
                ///     (English - United States - 1033): Web resource hash
                ///     (Russian - 1049): Хэш веб-ресурса
                /// 
                /// Description:
                ///     (English - United States - 1033): Hash value of web resources.
                ///     (Russian - 1049): Значение хэша для веб-ресурса.
                /// 
                /// SchemaName: WebResourceHash
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string webresourcehash = "webresourcehash";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Week Start Day Code
                ///     (Russian - 1049): Код первого дня недели
                /// 
                /// Description:
                ///     (English - United States - 1033): Designated first day of the week throughout Microsoft Dynamics 365.
                ///     (Russian - 1049): Первый день недели в Microsoft Dynamics 365.
                /// 
                /// SchemaName: WeekStartDayCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet organization_weekstartdaycode
                /// DefaultFormValue = -1
                ///</summary>
                public const string weekstartdaycode = "weekstartdaycode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): For Internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// Description:
                /// 
                /// SchemaName: WidgetProperties
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Disabled    IsLocalizable = False
                ///</summary>
                public const string widgetproperties = "widgetproperties";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Yammer Group Id
                ///     (Russian - 1049): Код группы Yammer
                /// 
                /// Description:
                ///     (English - United States - 1033): Denotes the Yammer group ID
                ///     (Russian - 1049): Обозначает код группы Yammer
                /// 
                /// SchemaName: YammerGroupId
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string yammergroupid = "yammergroupid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Yammer Network Permalink
                ///     (Russian - 1049): Постоянная ссылка в сети Yammer
                /// 
                /// Description:
                ///     (English - United States - 1033): Denotes the Yammer network permalink
                ///     (Russian - 1049): Обозначает постоянную ссылку в сети Yammer
                /// 
                /// SchemaName: YammerNetworkPermalink
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string yammernetworkpermalink = "yammernetworkpermalink";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Yammer OAuth Access Token Expired
                ///     (Russian - 1049): Срок действия маркера доступа Yammer OAuth истек
                /// 
                /// Description:
                ///     (English - United States - 1033): Denotes whether the OAuth access token for Yammer network has expired
                ///     (Russian - 1049): Указывает, истек ли срок действия маркера доступа OAuth для сети Yammer
                /// 
                /// SchemaName: YammerOAuthAccessTokenExpired
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string yammeroauthaccesstokenexpired = "yammeroauthaccesstokenexpired";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Internal Use Only
                ///     (Russian - 1049): Только для внутреннего использования
                /// 
                /// Description:
                /// 
                /// SchemaName: YammerPostMethod
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet organization_yammerpostmethod
                /// DefaultFormValue = 0
                ///</summary>
                public const string yammerpostmethod = "yammerpostmethod";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Year Start Week Code
                ///     (Russian - 1049): Код первого дня года
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies how the first week of the year is specified in Microsoft Dynamics 365.
                ///     (Russian - 1049): Информация о способе определения первой недели года в Microsoft Dynamics 365.
                /// 
                /// SchemaName: YearStartWeekCode
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string yearstartweekcode = "yearstartweekcode";
            }

            #endregion Attributes.

            #region OptionSets.

            public static partial class OptionSets
            {

                #region Picklist OptionSet OptionSets.
                ///<summary>
                /// Attribute: currencydisplayoption
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Display Currencies Using
                ///     (Russian - 1049): Отображение валют с использованием
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether to display money fields with currency code or currency symbol.
                ///     (Russian - 1049): Указание о том, следует ли отображать в денежных полях код или обозначение денежной единицы.
                /// 
                /// Local System  OptionSet organization_currencydisplayoption
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Display currencies using
                ///     (Russian - 1049): Отображение валют с использованием
                /// 
                /// Description:
                ///     (English - United States - 1033): Indication of whether to display money fields with currency code or currency symbol.
                ///     (Russian - 1049): Указание о том, следует ли отображать в денежных полях код или символ валюты.
                ///</summary>
                public enum currencydisplayoption
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Currency symbol
                    ///     (Russian - 1049): Обозначение денежной единицы
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Currency_symbol_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Currency code
                    ///     (Russian - 1049): Код валюты
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Currency_code_1 = 1,
                }

                ///<summary>
                /// Attribute: currencyformatcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Currency Format Code
                ///     (Russian - 1049): Код форматирования валюты
                /// 
                /// Description:
                ///     (English - United States - 1033): Information about how currency symbols are placed throughout Microsoft Dynamics CRM.
                ///     (Russian - 1049): Информация о размещении обозначений денежных единиц в Microsoft Dynamics CRM.
                /// 
                /// Local System  OptionSet organization_currencyformatcode
                /// 
                /// Description:
                ///     (English - United States - 1033): Information about how currency symbols are placed throughout Microsoft CRM.
                ///     (Russian - 1049): Сведения о том, как символы валюты размещаются в Microsoft CRM.
                ///</summary>
                public enum currencyformatcode
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): $123
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    V_123_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): 123$
                    ///     (Russian - 1049): 123 $
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    V_123_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): $ 123
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    V_123_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): 123 $
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    V_123_3 = 3,
                }

                ///<summary>
                /// Attribute: defaultrecurrenceendrangetype
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Default Recurrence End Range Type
                ///     (Russian - 1049): Тип диапазона окончания повторения по умолчанию
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of default recurrence end range date.
                ///     (Russian - 1049): Тип даты диапазона окончания повторения по умолчанию.
                /// 
                /// Local System  OptionSet organization_defaultrecurrenceendrangetype
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): DefaultRecurrenceEndRangeType
                /// 
                /// Description:
                ///     (English - United States - 1033): Specifies the default end recurrence range to be used in recurrence dialog.
                ///     (Russian - 1049): Задает диапазон окончания повтора по умолчанию для использования в диалоговом окне повторения.
                ///</summary>
                public enum defaultrecurrenceendrangetype
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): No End Date
                    ///     (Russian - 1049): Без даты окончания
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    No_End_Date_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Number of Occurrences
                    ///     (Russian - 1049): Число повторов
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Number_of_Occurrences_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): End By Date
                    ///     (Russian - 1049): Окончание по дате
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    End_By_Date_3 = 3,
                }

                ///<summary>
                /// Attribute: discountcalculationmethod
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Discount calculation method
                ///     (Russian - 1049): Способ расчета скидки
                /// 
                /// Description:
                ///     (English - United States - 1033): Discount calculation method for the QOOI product.
                ///     (Russian - 1049): Способ расчета скидки для продукта QOOI.
                /// 
                /// Local System  OptionSet organization_discountcalculationmethod
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Discount calculation Method
                ///     (Russian - 1049): Способ расчета скидки
                /// 
                /// Description:
                ///     (English - United States - 1033): Discount calculation Method for the QOOI product
                ///     (Russian - 1049): Способ расчета скидки для продукта QOOI
                ///</summary>
                public enum discountcalculationmethod
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Line item
                    ///     (Russian - 1049): Позиция строки
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Line_item_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Per unit
                    ///     (Russian - 1049): За единицу
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Per_unit_1 = 1,
                }

                ///<summary>
                /// Attribute: emailconnectionchannel
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Email Connection Channel
                ///     (Russian - 1049): Канал подключения электронной почты
                /// 
                /// Description:
                ///     (English - United States - 1033): Select if you want to use the Email Router or server-side synchronization for email processing.
                ///     (Russian - 1049): Укажите, следует ли использовать для обработки электронной почты маршрутизатор электронной почты или синхронизацию на стороне сервера.
                /// 
                /// Local System  OptionSet organization_emailconnectionchannel
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether you want to use the Email Router or server-side synchronization for email processing.
                ///     (Russian - 1049): Укажите, следует ли использовать для обработки электронной почты маршрутизатор электронной почты или синхронизацию на стороне сервера.
                ///</summary>
                public enum emailconnectionchannel
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Server-Side Synchronization
                    ///     (Russian - 1049): Синхронизация на стороне сервера
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Server_Side_Synchronization_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Microsoft Dynamics 365 Email Router
                    ///     (Russian - 1049): Маршрутизатор электронной почты Microsoft Dynamics 365
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Microsoft_Dynamics_365_Email_Router_1 = 1,
                }

                ///<summary>
                /// Attribute: fiscalperiodformatperiod
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Format for Fiscal Period
                ///     (Russian - 1049): Формат финансового периода
                /// 
                /// Description:
                ///     (English - United States - 1033): Format in which the fiscal period will be displayed.
                ///     (Russian - 1049): Формат, в котором будет отображаться финансовый период.
                /// 
                /// Local System  OptionSet organization_fiscalperiodformat
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Fiscal Period Format
                ///     (Russian - 1049): Формат финансового периода
                /// 
                /// Description:
                ///</summary>
                public enum fiscalperiodformatperiod
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Quarter {0}
                    ///     (Russian - 1049): Квартал {0}
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Quarter_0_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Q{0}
                    ///     (Russian - 1049): К{0}
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Q_0_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): P{0}
                    ///     (Russian - 1049): П{0}
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    P_0_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Month {0}
                    ///     (Russian - 1049): Месяц {0}
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Month_0_4 = 4,

                    ///<summary>
                    /// 5
                    /// DisplayOrder: 5
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): M{0}
                    ///     (Russian - 1049): М{0}
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    M_0_5 = 5,

                    ///<summary>
                    /// 6
                    /// DisplayOrder: 6
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Semester {0}
                    ///     (Russian - 1049): Семестр {0}
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Semester_0_6 = 6,

                    ///<summary>
                    /// 7
                    /// DisplayOrder: 7
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Month Name
                    ///     (Russian - 1049): Название месяца
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Month_Name_7 = 7,
                }

                ///<summary>
                /// Attribute: fiscalyearformatprefix
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Prefix for Fiscal Year
                ///     (Russian - 1049): Префикс финансового года
                /// 
                /// Description:
                ///     (English - United States - 1033): Prefix for the display of the fiscal year.
                ///     (Russian - 1049): Префикс, отображаемый для финансового года.
                /// 
                /// Local System  OptionSet organization_fiscalyearformatprefix
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Fiscal Year Format Prefix
                ///     (Russian - 1049): Префикс финансового года
                /// 
                /// Description:
                ///</summary>
                public enum fiscalyearformatprefix
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): FY
                    ///     (Russian - 1049): ФГ
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    FY_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    V_2 = 2,
                }

                ///<summary>
                /// Attribute: fiscalyearformatsuffix
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Suffix for Fiscal Year
                ///     (Russian - 1049): Суффикс финансового года
                /// 
                /// Description:
                ///     (English - United States - 1033): Suffix for the display of the fiscal year.
                ///     (Russian - 1049): Суффикс, отображаемый для финансового года.
                /// 
                /// Local System  OptionSet organization_fiscalyearformatsuffix
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Fiscal Year Format Suffix
                ///     (Russian - 1049): Суффикс финансового года
                /// 
                /// Description:
                ///</summary>
                public enum fiscalyearformatsuffix
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): FY
                    ///     (Russian - 1049): ФГ
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    FY_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033):  Fiscal Year
                    ///     (Russian - 1049):  Финансовый год
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Fiscal_Year_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    V_3 = 3,
                }

                ///<summary>
                /// Attribute: fiscalyearformatyear
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Fiscal Year Format Year
                ///     (Russian - 1049): Формат компонента года финансового года
                /// 
                /// Description:
                ///     (English - United States - 1033): Format for the year.
                ///     (Russian - 1049): Формат года.
                /// 
                /// Local System  OptionSet organization_fiscalyearformatyear
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Fiscal Year Format
                ///     (Russian - 1049): Формат финансового года
                /// 
                /// Description:
                ///</summary>
                public enum fiscalyearformatyear
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): YYYY
                    ///     (Russian - 1049): ГГГГ
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    YYYY_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): YY
                    ///     (Russian - 1049): ГГ
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    YY_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): GGYY
                    ///     (Russian - 1049): ВВГГ
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    GGYY_3 = 3,
                }

                ///<summary>
                /// Attribute: fullnameconventioncode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Full Name Display Order
                ///     (Russian - 1049): Порядок отображения полных имен
                /// 
                /// Description:
                ///     (English - United States - 1033): Order in which names are to be displayed throughout Microsoft CRM.
                ///     (Russian - 1049): Порядок, в котором имена отображаются в Microsoft CRM.
                /// 
                /// Local System  OptionSet organization_fullnameconventioncode
                /// 
                /// Description:
                ///     (English - United States - 1033): Order in which names are to be displayed throughout Microsoft CRM.
                ///     (Russian - 1049): Порядок, в котором имена отображаются в Microsoft CRM.
                ///</summary>
                public enum fullnameconventioncode
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Last Name, First Name
                    ///     (Russian - 1049): Фамилия, Имя
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Last_Name_First_Name_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): First Name
                    ///     (Russian - 1049): Имя
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    First_Name_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Last Name, First Name, Middle Initial
                    ///     (Russian - 1049): Фамилия, Имя, О.
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Last_Name_First_Name_Middle_Initial_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): First Name, Middle Initial, Last Name
                    ///     (Russian - 1049): Имя, О. Фамилия
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    First_Name_Middle_Initial_Last_Name_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 5
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Last Name, First Name, Middle Name
                    ///     (Russian - 1049): Фамилия, имя, отчество
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Last_Name_First_Name_Middle_Name_4 = 4,

                    ///<summary>
                    /// 5
                    /// DisplayOrder: 6
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): First Name, Middle Name, Last Name
                    ///     (Russian - 1049): Имя, отчество, фамилия
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    First_Name_Middle_Name_Last_Name_5 = 5,

                    ///<summary>
                    /// 6
                    /// DisplayOrder: 7
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Last Name, space, First Name
                    ///     (Russian - 1049): Фамилия, пробел, имя
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Last_Name_space_First_Name_6 = 6,

                    ///<summary>
                    /// 7
                    /// DisplayOrder: 8
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Last Name, no space, First Name
                    ///     (Russian - 1049): Фамилия, без пробела, имя
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Last_Name_no_space_First_Name_7 = 7,
                }

                ///<summary>
                /// Attribute: isvintegrationcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): ISV Integration Mode
                ///     (Russian - 1049): Режим интеграции ISV
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether loading of Microsoft Dynamics 365 in a browser window that does not have address, tool, and menu bars is enabled.
                ///     (Russian - 1049): Указывает, будет ли система Microsoft Dynamics 365 открыта в окне браузера без адресной строки, панели инструментов и меню.
                /// 
                /// Local System  OptionSet organization_isvintegrationcode
                /// 
                /// Description:
                ///     (English - United States - 1033): Flag that determines whether or not MSCRM should be loaded in an browser window that does not have address, tool and menu bars.
                ///     (Russian - 1049): Флаг, определяющий, будет ли система MSCRM открыта в окне браузера без адресной строки, панели инструментов и меню.
                ///</summary>
                public enum isvintegrationcode
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
                    ///     (English - United States - 1033): Web
                    ///     (Russian - 1049): Веб
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Web_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Outlook Workstation Client
                    ///     (Russian - 1049): Клиент Outlook для настольных ПК
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Outlook_Workstation_Client_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Web; Outlook Workstation Client
                    ///     (Russian - 1049): Интернет; клиент Outlook для настольных ПК
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Web_Outlook_Workstation_Client_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 5
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Outlook Laptop Client
                    ///     (Russian - 1049): Клиент Outlook для ноутбуков
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Outlook_Laptop_Client_4 = 4,

                    ///<summary>
                    /// 5
                    /// DisplayOrder: 6
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Web; Outlook Laptop Client
                    ///     (Russian - 1049): Интернет; клиент Outlook для ноутбуков
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Web_Outlook_Laptop_Client_5 = 5,

                    ///<summary>
                    /// 6
                    /// DisplayOrder: 7
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Outlook
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Outlook_6 = 6,

                    ///<summary>
                    /// 7
                    /// DisplayOrder: 8
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): All
                    ///     (Russian - 1049): Все
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    All_7 = 7,
                }

                ///<summary>
                /// Attribute: negativeformatcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Negative Format
                ///     (Russian - 1049): Форматирование отрицательных сумм
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies how negative numbers are displayed throughout Microsoft CRM.
                ///     (Russian - 1049): Сведения, определяющие формат отображения отрицательных значений в Microsoft CRM.
                /// 
                /// Local System  OptionSet organization_negativeformatcode
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies how negative numbers are displayed throughout Microsoft CRM.
                ///     (Russian - 1049): Сведения, определяющие формат отображения отрицательных значений в Microsoft CRM.
                ///</summary>
                public enum negativeformatcode
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Brackets
                    ///     (Russian - 1049): Квадратные скобки
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Brackets_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Dash
                    ///     (Russian - 1049): Шаблон
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Dash_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Dash plus Space
                    ///     (Russian - 1049): Дефис и пробел
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Dash_plus_Space_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Trailing Dash
                    ///     (Russian - 1049): Дефис в конце
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Trailing_Dash_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 5
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Space plus Trailing Dash
                    ///     (Russian - 1049): Пробел и дефис в конце
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Space_plus_Trailing_Dash_4 = 4,
                }

                ///<summary>
                /// Attribute: plugintracelogsetting
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Plug-in Trace Log Setting
                ///     (Russian - 1049): Настройка журнала трассировки подключаемого модуля
                /// 
                /// Description:
                ///     (English - United States - 1033): Plug-in Trace Log Setting for the Organization.
                ///     (Russian - 1049): Настройка журнала трассировки подключаемых модулей для организации.
                /// 
                /// Local System  OptionSet organization_plugintracelogsetting
                /// 
                /// Description:
                ///     (English - United States - 1033): Plug-in Trace Log Setting for the Organization.
                ///     (Russian - 1049): Настройка журнала трассировки подключаемых модулей для организации.
                ///</summary>
                public enum plugintracelogsetting
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Off
                    ///     (Russian - 1049): Выкл
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Off_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Exception
                    ///     (Russian - 1049): Исключение
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Exception_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): All
                    ///     (Russian - 1049): Все
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    All_2 = 2,
                }

                ///<summary>
                /// Attribute: reportscripterrors
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Report Script Errors
                ///     (Russian - 1049): Ошибки скрипта отчета
                /// 
                /// Description:
                ///     (English - United States - 1033): Picklist for selecting the organization preference for reporting scripting errors.
                ///     (Russian - 1049): Поле выбора, отвечающее за предпочтения организации в области сообщения об ошибках в скриптах.
                /// 
                /// Local System  OptionSet organization_reportscripterrors
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Report Script Errors
                ///     (Russian - 1049): Ошибки скрипта отчета
                /// 
                /// Description:
                ///     (English - United States - 1033): Picklist for selecting the user preference for reporting scripting errors.
                ///     (Russian - 1049): Список выбора для пользовательских параметров отчета об ошибках скриптов.
                ///</summary>
                public enum reportscripterrors
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): No preference for sending an error report to Microsoft about Microsoft Dynamics 365
                    ///     (Russian - 1049): Параметр отправки в Майкрософт отчета об ошибках, связанных с Microsoft Dynamics 365, не выбран
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    No_preference_for_sending_an_error_report_to_Microsoft_about_Microsoft_Dynamics_365_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Ask me for permission to send an error report to Microsoft
                    ///     (Russian - 1049): Запрашивать разрешение на отправку отчета об ошибках в Майкрософт
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Ask_me_for_permission_to_send_an_error_report_to_Microsoft_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Automatically send an error report to Microsoft without asking me for permission
                    ///     (Russian - 1049): Автоматически отправлять отчет об ошибках в корпорацию Майкрософт, не запрашивая разрешения
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Automatically_send_an_error_report_to_Microsoft_without_asking_me_for_permission_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Never send an error report to Microsoft about Microsoft Dynamics 365
                    ///     (Russian - 1049): Никогда не отправлять отчет об ошибках Microsoft Dynamics 365 в корпорацию Майкрософт
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Never_send_an_error_report_to_Microsoft_about_Microsoft_Dynamics_365_3 = 3,
                }

                ///<summary>
                /// Attribute: sharepointdeploymenttype
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Choose SharePoint Deployment Type
                ///     (Russian - 1049): Выберите тип развертывания SharePoint
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates which SharePoint deployment type is configured for Server to Server. (Online or On-Premises)
                ///     (Russian - 1049): Указывает, какой тип развертывания SharePoint настроен для проверки подлинности типа "сервер-сервер". (Online или локальный)
                /// 
                /// Local System  OptionSet organization_sharepointdeploymenttype
                /// 
                /// Description:
                ///     (English - United States - 1033): SharePoint Deployment Type
                ///     (Russian - 1049): Тип развертывания SharePoint
                ///</summary>
                public enum sharepointdeploymenttype
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Online
                    ///     (Russian - 1049): В сети
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Online_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): On-Premises
                    ///     (Russian - 1049): Локальное
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    On_Premises_1 = 1,
                }

                ///<summary>
                /// Attribute: yammerpostmethod
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Internal Use Only
                ///     (Russian - 1049): Только для внутреннего использования
                /// 
                /// Description:
                /// 
                /// Local System  OptionSet organization_yammerpostmethod
                /// 
                /// Description:
                ///     (English - United States - 1033): Yammer Post Method
                ///     (Russian - 1049): Метод публикации в Yammer
                ///</summary>
                public enum yammerpostmethod
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Public
                    ///     (Russian - 1049): Открытый
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Public_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Private
                    ///     (Russian - 1049): Закрытый
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Private_1 = 1,
                }

                #endregion Picklist OptionSets.
            }

            #endregion OptionSets.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship basecurrency_organization
                /// 
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     basecurrency_organization
                /// ReferencingEntityNavigationPropertyName    basecurrencyid
                /// IsCustomizable                             False                        False
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
                public static partial class basecurrency_organization
                {
                    public const string Name = "basecurrency_organization";

                    public const string ReferencedEntity_transactioncurrency = "transactioncurrency";

                    public const string ReferencedAttribute_transactioncurrencyid = "transactioncurrencyid";

                    public const string ReferencedEntity_PrimaryNameAttribute_currencyname = "currencyname";

                    public const string ReferencingEntity_organization = "organization";

                    public const string ReferencingAttribute_basecurrencyid = "basecurrencyid";
                }

                ///<summary>
                /// N:1 - Relationship calendar_organization
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     calendar_organization
                /// ReferencingEntityNavigationPropertyName    businessclosurecalendarid_calendar
                /// IsCustomizable                             False                                 False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
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
                public static partial class calendar_organization
                {
                    public const string Name = "calendar_organization";

                    public const string ReferencedEntity_calendar = "calendar";

                    public const string ReferencedAttribute_calendarid = "calendarid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_organization = "organization";

                    public const string ReferencingAttribute_businessclosurecalendarid = "businessclosurecalendarid";
                }

                ///<summary>
                /// N:1 - Relationship DefaultMobileOfflineProfile_Organization
                /// 
                /// PropertyName                               Value                                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     DefaultMobileOfflineProfile_Organization
                /// ReferencingEntityNavigationPropertyName    defaultmobileofflineprofileid
                /// IsCustomizable                             False                                       False
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
                ///</summary>
                public static partial class defaultmobileofflineprofile_organization
                {
                    public const string Name = "DefaultMobileOfflineProfile_Organization";

                    public const string ReferencedEntity_mobileofflineprofile = "mobileofflineprofile";

                    public const string ReferencedAttribute_mobileofflineprofileid = "mobileofflineprofileid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_organization = "organization";

                    public const string ReferencingAttribute_defaultmobileofflineprofileid = "defaultmobileofflineprofileid";
                }

                ///<summary>
                /// N:1 - Relationship EmailServerProfile_Organization
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     EmailServerProfile_Organization
                /// ReferencingEntityNavigationPropertyName    defaultemailserverprofileid
                /// IsCustomizable                             False                              False
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
                /// ReferencedEntity emailserverprofile:
                ///     DisplayName:
                ///     (English - United States - 1033): Email Server Profile
                ///     (Russian - 1049): Профиль сервера электронной почты
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Email Server Profiles
                ///     (Russian - 1049): Профили серверов электронной почты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Holds the Email Server Profiles of an organization
                ///     (Russian - 1049): Содержит профили серверов электронной почты организации
                ///</summary>
                public static partial class emailserverprofile_organization
                {
                    public const string Name = "EmailServerProfile_Organization";

                    public const string ReferencedEntity_emailserverprofile = "emailserverprofile";

                    public const string ReferencedAttribute_emailserverprofileid = "emailserverprofileid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_organization = "organization";

                    public const string ReferencingAttribute_defaultemailserverprofileid = "defaultemailserverprofileid";
                }

                ///<summary>
                /// N:1 - Relationship lk_organization_createdonbehalfby
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_organization_createdonbehalfby
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
                public static partial class lk_organization_createdonbehalfby
                {
                    public const string Name = "lk_organization_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_organization = "organization";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_organization_entityimage
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_organization_entityimage
                /// ReferencingEntityNavigationPropertyName    entityimageid_imagedescriptor
                /// IsCustomizable                             False                            False
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
                /// ReferencedEntity imagedescriptor:
                ///     DisplayName:
                ///     (English - United States - 1033): Image Descriptor
                ///     (Russian - 1049): Дескриптор изображения
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Image Descriptors
                ///     (Russian - 1049): Дескрипторы изображений
                ///     
                ///     Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                ///</summary>
                public static partial class lk_organization_entityimage
                {
                    public const string Name = "lk_organization_entityimage";

                    public const string ReferencedEntity_imagedescriptor = "imagedescriptor";

                    public const string ReferencedAttribute_imagedescriptorid = "imagedescriptorid";

                    public const string ReferencingEntity_organization = "organization";

                    public const string ReferencingAttribute_entityimageid = "entityimageid";
                }

                ///<summary>
                /// N:1 - Relationship lk_organization_modifiedonbehalfby
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_organization_modifiedonbehalfby
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
                public static partial class lk_organization_modifiedonbehalfby
                {
                    public const string Name = "lk_organization_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_organization = "organization";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_organizationbase_createdby
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_organizationbase_createdby
                /// ReferencingEntityNavigationPropertyName    createdby
                /// IsCustomizable                             False                            False
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
                public static partial class lk_organizationbase_createdby
                {
                    public const string Name = "lk_organizationbase_createdby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_organization = "organization";

                    public const string ReferencingAttribute_createdby = "createdby";
                }

                ///<summary>
                /// N:1 - Relationship lk_organizationbase_modifiedby
                /// 
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_organizationbase_modifiedby
                /// ReferencingEntityNavigationPropertyName    modifiedby
                /// IsCustomizable                             False                             False
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
                public static partial class lk_organizationbase_modifiedby
                {
                    public const string Name = "lk_organizationbase_modifiedby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_organization = "organization";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";
                }

                ///<summary>
                /// N:1 - Relationship Template_Organization
                /// 
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Template_Organization
                /// ReferencingEntityNavigationPropertyName    acknowledgementtemplateid
                /// IsCustomizable                             False                        False
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
                /// ReferencedEntity template:
                ///     DisplayName:
                ///     (English - United States - 1033): Email Template
                ///     (Russian - 1049): Шаблон электронной почты
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Email Templates
                ///     (Russian - 1049): Шаблоны электронной почты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Template for an email message that contains the standard attributes of an email message.
                ///     (Russian - 1049): Шаблон сообщения электронной почты, содержащий стандартные атрибуты сообщения электронной почты.
                ///</summary>
                public static partial class template_organization
                {
                    public const string Name = "Template_Organization";

                    public const string ReferencedEntity_template = "template";

                    public const string ReferencedAttribute_templateid = "templateid";

                    public const string ReferencedEntity_PrimaryNameAttribute_title = "title";

                    public const string ReferencingEntity_organization = "organization";

                    public const string ReferencingAttribute_acknowledgementtemplateid = "acknowledgementtemplateid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship channelproperty_organization
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     channelproperty_organization
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                           False
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
                /// ReferencingEntity channelproperty:
                ///     DisplayName:
                ///     (English - United States - 1033): Channel Property
                ///     (Russian - 1049): Свойство канала
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Channel Properties
                ///     (Russian - 1049): Свойства канала
                ///     
                ///     Description:
                ///     (English - United States - 1033): Instance of a channel property containing its name and corresponding data type.
                ///     (Russian - 1049): Экземпляр свойства канала, содержащий его имя и соответствующий тип данных.
                ///</summary>
                public static partial class channelproperty_organization
                {
                    public const string Name = "channelproperty_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_channelproperty = "channelproperty";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship channelpropertygroup_organization
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     channelpropertygroup_organization
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                False
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
                /// ReferencingEntity channelpropertygroup:
                ///     DisplayName:
                ///     (English - United States - 1033): Channel Property Group
                ///     (Russian - 1049): Группа свойств канала
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Channel Property Groups
                ///     (Russian - 1049): Группы свойств канала
                ///     
                ///     Description:
                ///     (English - United States - 1033): Group or collection of channel properties provided by the external channel for a Microsoft Dynamics 365 activity.
                ///     (Russian - 1049): Группа или коллекция свойств канала, предоставленная внешним каналом для действия Microsoft Dynamics 365.
                ///</summary>
                public static partial class channelpropertygroup_organization
                {
                    public const string Name = "channelpropertygroup_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_channelpropertygroup = "channelpropertygroup";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship customcontrol_organization
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     customcontrol_organization
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity customcontrol:
                ///     DisplayName:
                ///     (English - United States - 1033): Custom Control
                ///     (Russian - 1049): Пользовательский элемент управления
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Custom Controls
                ///     (Russian - 1049): Пользовательские элементы управления
                ///     
                ///     Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                ///</summary>
                public static partial class customcontrol_organization
                {
                    public const string Name = "customcontrol_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_customcontrol = "customcontrol";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship customcontroldefaultconfig_organization
                /// 
                /// PropertyName                               Value                                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     customcontroldefaultconfig_organization
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity customcontroldefaultconfig:
                ///     DisplayName:
                ///     (English - United States - 1033): Custom Control Default Config
                ///     (Russian - 1049): Конфигурация пользовательского элемента управления по умолчанию
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Custom Control Default Configs
                ///     (Russian - 1049): Конфигурации пользовательского элемента управления по умолчанию
                ///     
                ///     Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                ///</summary>
                public static partial class customcontroldefaultconfig_organization
                {
                    public const string Name = "customcontroldefaultconfig_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_customcontroldefaultconfig = "customcontroldefaultconfig";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship customcontrolresource_organization
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     customcontrolresource_organization
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity customcontrolresource:
                ///     DisplayName:
                ///     (English - United States - 1033): Custom Control Resource
                ///     (Russian - 1049): Ресурс пользовательского элемента управления
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Custom Control Resources
                ///     (Russian - 1049): Ресурсы пользовательского элемента управления
                ///     
                ///     Description:
                ///     (English - United States - 1033): Custom Control Resource Id
                ///     (Russian - 1049): Код ресурса пользовательского элемента управления
                ///</summary>
                public static partial class customcontrolresource_organization
                {
                    public const string Name = "customcontrolresource_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_customcontrolresource = "customcontrolresource";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship dynamicproperty_organization
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     dynamicproperty_organization
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                           False
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
                /// ReferencingEntity dynamicproperty:
                ///     DisplayName:
                ///     (English - United States - 1033): Property
                ///     (Russian - 1049): Свойство
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Properties
                ///     (Russian - 1049): Свойства
                ///     
                ///     Description:
                ///     (English - United States - 1033): Information about a product property.
                ///     (Russian - 1049): Сведения о свойстве продукта.
                ///</summary>
                public static partial class dynamicproperty_organization
                {
                    public const string Name = "dynamicproperty_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_dynamicproperty = "dynamicproperty";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship DynamicPropertyAssociation_organization
                /// 
                /// PropertyName                               Value                                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     DynamicPropertyAssociation_organization
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity dynamicpropertyassociation:
                ///     DisplayName:
                ///     (English - United States - 1033): Property Association
                ///     (Russian - 1049): Связь свойства
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Property Associations
                ///     (Russian - 1049): Связи свойства
                ///     
                ///     Description:
                ///     (English - United States - 1033): Association of a property definition with another entity in the system.
                ///     (Russian - 1049): Связь определения свойства с другой сущностью в системе.
                ///</summary>
                public static partial class dynamicpropertyassociation_organization
                {
                    public const string Name = "DynamicPropertyAssociation_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_dynamicpropertyassociation = "dynamicpropertyassociation";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship DynamicPropertyOptionSetItem_organization
                /// 
                /// PropertyName                               Value                                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     DynamicPropertyOptionSetItem_organization
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                        False
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
                /// ReferencingEntity dynamicpropertyoptionsetitem:
                ///     DisplayName:
                ///     (English - United States - 1033): Property Option Set Item
                ///     (Russian - 1049): Элемент набора параметров свойства
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Property Option Set Items
                ///     (Russian - 1049): Элементы набора параметров свойства
                ///     
                ///     Description:
                ///     (English - United States - 1033): Item with a name and value in a property option set type.
                ///     (Russian - 1049): Элемент с именем и значением в типе набора параметров свойства.
                ///</summary>
                public static partial class dynamicpropertyoptionsetitem_organization
                {
                    public const string Name = "DynamicPropertyOptionSetItem_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_dynamicpropertyoptionsetitem = "dynamicpropertyoptionsetitem";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_dynamicpropertyoptionname = "dynamicpropertyoptionname";
                }

                ///<summary>
                /// 1:N - Relationship entitlementchannel_organization
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     entitlementchannel_organization
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                              False
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
                /// ReferencingEntity entitlementchannel:
                ///     DisplayName:
                ///     (English - United States - 1033): Entitlement Channel
                ///     (Russian - 1049): Канал объема обслуживания
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): EntitlementChannels
                ///     
                ///     Description:
                ///     (English - United States - 1033): Defines the amount and type of support for a channel.
                ///     (Russian - 1049): Определяет объем и тип поддержки для канала.
                ///</summary>
                public static partial class entitlementchannel_organization
                {
                    public const string Name = "entitlementchannel_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_entitlementchannel = "entitlementchannel";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship entitlementtemplate_organization
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     entitlementtemplate_organization
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                public static partial class entitlementtemplate_organization
                {
                    public const string Name = "entitlementtemplate_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_entitlementtemplate = "entitlementtemplate";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship entitlementtemplatechannel_organization
                /// 
                /// PropertyName                               Value                                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     entitlementtemplatechannel_organization
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity entitlementtemplatechannel:
                ///     DisplayName:
                ///     (English - United States - 1033): Entitlement Template Channel
                ///     (Russian - 1049): Канал шаблона объема обслуживания
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                ///     
                ///     Description:
                ///     (English - United States - 1033): Contains predefined support terms for a channel to create entitlements for customers.
                ///     (Russian - 1049): Содержит предопределенные условия поддержки для канала, согласно которым будут создаваться объемы обслуживания для клиентов.
                ///</summary>
                public static partial class entitlementtemplatechannel_organization
                {
                    public const string Name = "entitlementtemplatechannel_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_entitlementtemplatechannel = "entitlementtemplatechannel";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship languagelocale_organization
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     languagelocale_organization
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity languagelocale:
                ///     DisplayName:
                ///     (English - United States - 1033): Language
                ///     (Russian - 1049): Язык
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Languages
                ///     (Russian - 1049): Языки
                ///     
                ///     Description:
                ///</summary>
                public static partial class languagelocale_organization
                {
                    public const string Name = "languagelocale_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_languagelocale = "languagelocale";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship lk_authorizationserver_organizationid
                /// 
                /// PropertyName                               Value                                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_authorizationserver_organizationid
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
                /// ReferencingEntity authorizationserver:
                ///     DisplayName:
                ///     (English - United States - 1033): Authorization Server
                ///     (Russian - 1049): Сервер авторизации
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Authorization Servers
                ///     (Russian - 1049): Серверы авторизации
                ///     
                ///     Description:
                ///     (English - United States - 1033): Authorization servers that trust this organization
                ///     (Russian - 1049): Серверы авторизации, доверяющие этой организации
                ///</summary>
                public static partial class lk_authorizationserver_organizationid
                {
                    public const string Name = "lk_authorizationserver_organizationid";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_authorizationserver = "authorizationserver";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship lk_dataperformance_organizationid
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_dataperformance_organizationid
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                False
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
                /// ReferencingEntity dataperformance:
                ///     DisplayName:
                ///     (English - United States - 1033): Data Performance Dashboard
                ///     (Russian - 1049): Панель мониторинга производительности данных
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Data Performance Collection
                ///     
                ///     Description:
                ///     (English - United States - 1033): Data Performance Dashboard.
                ///     (Russian - 1049): Панель мониторинга производительности данных.
                ///</summary>
                public static partial class lk_dataperformance_organizationid
                {
                    public const string Name = "lk_dataperformance_organizationid";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_dataperformance = "dataperformance";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship lk_documenttemplatebase_organization
                /// 
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_documenttemplatebase_organization
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                   False
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
                /// ReferencingEntity documenttemplate:
                ///     DisplayName:
                ///     (English - United States - 1033): Document Template
                ///     (Russian - 1049): Шаблон документа
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Document Templates
                ///     (Russian - 1049): Шаблоны документов
                ///     
                ///     Description:
                ///     (English - United States - 1033): Used to store Document Templates in database in binary format.
                ///     (Russian - 1049): Используется для хранения шаблонов документов в базе данных в двоичном формате.
                ///</summary>
                public static partial class lk_documenttemplatebase_organization
                {
                    public const string Name = "lk_documenttemplatebase_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_documenttemplate = "documenttemplate";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship lk_fieldsecurityprofile_organizationid
                /// 
                /// PropertyName                               Value                                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_fieldsecurityprofile_organizationid
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                     False
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
                /// ReferencingEntity fieldsecurityprofile:
                ///     DisplayName:
                ///     (English - United States - 1033): Field Security Profile
                ///     (Russian - 1049): Профиль безопасности поля
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Field Security Profiles
                ///     (Russian - 1049): Профили безопасности полей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Profile which defines access level for secured attributes
                ///     (Russian - 1049): Профиль, который определяет уровень доступа к защищенным атрибутам
                ///</summary>
                public static partial class lk_fieldsecurityprofile_organizationid
                {
                    public const string Name = "lk_fieldsecurityprofile_organizationid";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_fieldsecurityprofile = "fieldsecurityprofile";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship lk_organizationui_organizationid
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_organizationui_organizationid
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity organizationui:
                ///     DisplayName:
                ///     (English - United States - 1033): Organization UI
                ///     (Russian - 1049): ИП организации
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Organization UI Settings
                ///     (Russian - 1049): Параметры ИП организации
                ///     
                ///     Description:
                ///     (English - United States - 1033): Entity customizations including form layout and icons. Includes current and past versions.
                ///     (Russian - 1049): Настройки сущности, включая макет формы и значки. Включает текущие и предыдущие версии.
                ///</summary>
                public static partial class lk_organizationui_organizationid
                {
                    public const string Name = "lk_organizationui_organizationid";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_organizationui = "organizationui";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship lk_partnerapplication_organizationid
                /// 
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_partnerapplication_organizationid
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                   False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity partnerapplication:
                ///     DisplayName:
                ///     (English - United States - 1033): Partner Application
                ///     (Russian - 1049): Приложение-партнер
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Partner Applications
                ///     (Russian - 1049): Приложения-партнеры
                ///     
                ///     Description:
                ///     (English - United States - 1033): Partner applications registered for this organization
                ///     (Russian - 1049): Приложения-партнеры, зарегистрированные для этой организации
                ///</summary>
                public static partial class lk_partnerapplication_organizationid
                {
                    public const string Name = "lk_partnerapplication_organizationid";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_partnerapplication = "partnerapplication";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship lk_principalobjectattributeaccess_organizationid
                /// 
                /// PropertyName                               Value                                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_principalobjectattributeaccess_organizationid
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                               False
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
                /// ReferencingEntity principalobjectattributeaccess:
                ///     DisplayName:
                ///     (English - United States - 1033): Field Sharing
                ///     (Russian - 1049): Общий доступ к полям
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Defines CRM security principals (users and teams) access rights to secured field for an entity instance.
                ///     (Russian - 1049): Определяет права на доступ субъектов безопасности CRM (пользователей и рабочих группы) к защищенному полю экземпляра сущности.
                ///</summary>
                public static partial class lk_principalobjectattributeaccess_organizationid
                {
                    public const string Name = "lk_principalobjectattributeaccess_organizationid";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_principalobjectattributeaccess = "principalobjectattributeaccess";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship lk_principalsyncattributemap_organizationid
                /// 
                /// PropertyName                               Value                                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_principalsyncattributemap_organizationid
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                          False
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
                /// ReferencingEntity principalsyncattributemap:
                ///     DisplayName:
                ///     (English - United States - 1033): Principal Sync Attribute Map
                ///     (Russian - 1049): Сопоставление атрибутов синхронизации субъекта
                ///     
                ///     Description:
                ///     (English - United States - 1033): Maps security principals (users and teams) to sync attribute mappings.
                ///     (Russian - 1049): Сопоставление субъектов безопасности(пользователей и рабочих групп) с сопоставлениями атрибутов синхронизации.
                ///</summary>
                public static partial class lk_principalsyncattributemap_organizationid
                {
                    public const string Name = "lk_principalsyncattributemap_organizationid";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_principalsyncattributemap = "principalsyncattributemap";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship lk_syncattributemappingprofile_organizationid
                /// 
                /// PropertyName                               Value                                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_syncattributemappingprofile_organizationid
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity syncattributemappingprofile:
                ///     DisplayName:
                ///     (English - United States - 1033): Sync Attribute Mapping Profile
                ///     (Russian - 1049): Профиль сопоставления атрибутов синхронизации
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Sync Attribute Mapping Profiles
                ///     (Russian - 1049): Профили сопоставления атрибутов синхронизации
                ///     
                ///     Description:
                ///     (English - United States - 1033): Profile which defines sync attribute mapping
                ///     (Russian - 1049): Профиль, определяющий сопоставление атрибутов синхронизации
                ///</summary>
                public static partial class lk_syncattributemappingprofile_organizationid
                {
                    public const string Name = "lk_syncattributemappingprofile_organizationid";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_syncattributemappingprofile = "syncattributemappingprofile";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship MobileOfflineProfile_organization
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     MobileOfflineProfile_organization
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                False
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
                /// ReferencingEntity mobileofflineprofile:
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
                ///</summary>
                public static partial class mobileofflineprofile_organization
                {
                    public const string Name = "MobileOfflineProfile_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_mobileofflineprofile = "mobileofflineprofile";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship MobileOfflineProfileItem_organization
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
                /// ReferencingEntity mobileofflineprofileitem:
                ///     DisplayName:
                ///     (English - United States - 1033): Mobile Offline Profile Item
                ///     (Russian - 1049): Элемент профиля Mobile Offline
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Information on entity availability to mobile devices in offline mode for a mobile offline profile item.
                ///     (Russian - 1049): Сведения о доступности сущности для мобильных устройств в автономном режиме для элемента профиля Mobile Offline.
                ///</summary>
                public static partial class mobileofflineprofileitem_organization
                {
                    public const string Name = "MobileOfflineProfileItem_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_mobileofflineprofileitem = "mobileofflineprofileitem";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship MobileOfflineProfileItemAssociation_organization
                /// 
                /// PropertyName                               Value                                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     MobileOfflineProfileItemAssociation_organization
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                               False
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
                ///</summary>
                public static partial class mobileofflineprofileitemassociation_organization
                {
                    public const string Name = "MobileOfflineProfileItemAssociation_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_mobileofflineprofileitemassociation = "mobileofflineprofileitemassociation";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_advancedsimilarityrule
                /// 
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_advancedsimilarityrule
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity advancedsimilarityrule:
                ///     DisplayName:
                ///     (English - United States - 1033): Advanced Similarity Rule
                ///     (Russian - 1049): Расширенное правило подобия
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Advanced Similarity Rules
                ///     (Russian - 1049): Расширенные правила подобия
                ///     
                ///     Description:
                ///     (English - United States - 1033): A text match rule identifies similar records using keywords and key phrases determined with text analytics
                ///     (Russian - 1049): Правило текстового совпадения, выявляющее сходные записи на основе ключевых слов и фраз, которые определяются с помощью текстовой аналитики
                ///</summary>
                public static partial class organization_advancedsimilarityrule
                {
                    public const string Name = "organization_advancedsimilarityrule";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_advancedsimilarityrule = "advancedsimilarityrule";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_applicationfile
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_applicationfile
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                           False
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
                /// ReferencingEntity applicationfile:
                ///     DisplayName:
                ///     (English - United States - 1033): Application File
                ///     (Russian - 1049): Файл приложения
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Application Files
                ///     (Russian - 1049): Файлы приложения
                ///     
                ///     Description:
                ///     (English - United States - 1033): Files used by the application
                ///     (Russian - 1049): Файлы, используемые приложением
                ///</summary>
                public static partial class organization_applicationfile
                {
                    public const string Name = "organization_applicationfile";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_applicationfile = "applicationfile";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_appmodule
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
                /// ReferencingEntity appmodule:
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
                public static partial class organization_appmodule
                {
                    public const string Name = "organization_appmodule";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_appmodule = "appmodule";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship Organization_AsyncOperations
                /// 
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Organization_AsyncOperations
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_organization
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
                public static partial class organization_asyncoperations
                {
                    public const string Name = "Organization_AsyncOperations";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_asyncoperation = "asyncoperation";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_attributemap
                /// 
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_attributemap
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                        False
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
                /// ReferencingEntity attributemap:
                ///     DisplayName:
                ///     (English - United States - 1033): Attribute Map
                ///     (Russian - 1049): Сопоставление атрибутов
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Attribute Maps
                ///     (Russian - 1049): Сопоставления атрибутов
                ///     
                ///     Description:
                ///     (English - United States - 1033): Represents a mapping between attributes where the attribute values should be copied from a record into the form of a new related record.
                ///     (Russian - 1049): Представляет сопоставление атрибутов, в котором значения атрибутов нужно скопировать из записи в форму новой связанной записи.
                ///</summary>
                public static partial class organization_attributemap
                {
                    public const string Name = "organization_attributemap";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_attributemap = "attributemap";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_azureserviceconnection
                /// 
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_azureserviceconnection
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity azureserviceconnection:
                ///     DisplayName:
                ///     (English - United States - 1033): Azure Service Connection
                ///     (Russian - 1049): Подключение к службе Azure
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Azure Service Connections
                ///     (Russian - 1049): Подключения к службе Azure
                ///     
                ///     Description:
                ///     (English - United States - 1033): Stores connection information for an Azure service
                ///     (Russian - 1049): Хранит сведения о подключениях к службе Azure
                ///</summary>
                public static partial class organization_azureserviceconnection
                {
                    public const string Name = "organization_azureserviceconnection";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_azureserviceconnection = "azureserviceconnection";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship Organization_BulkDeleteFailures
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Organization_BulkDeleteFailures
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_organization
                /// IsCustomizable                             False                              False
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
                public static partial class organization_bulkdeletefailures
                {
                    public const string Name = "Organization_BulkDeleteFailures";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_bulkdeletefailure = "bulkdeletefailure";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";
                }

                ///<summary>
                /// 1:N - Relationship organization_business_unit_news_articles
                /// 
                /// PropertyName                               Value                                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_business_unit_news_articles
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                       False
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
                /// ReferencingEntity businessunitnewsarticle:
                ///     DisplayName:
                ///     (English - United States - 1033): Announcement
                ///     (Russian - 1049): Объявление
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Announcements
                ///     (Russian - 1049): Объявления
                ///     
                ///     Description:
                ///     (English - United States - 1033): Announcement associated with an organization.
                ///     (Russian - 1049): Объявление связанное с организацией.
                ///</summary>
                public static partial class organization_business_unit_news_articles
                {
                    public const string Name = "organization_business_unit_news_articles";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_businessunitnewsarticle = "businessunitnewsarticle";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_articletitle = "articletitle";
                }

                ///<summary>
                /// 1:N - Relationship organization_business_units
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_business_units
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity businessunit:
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
                public static partial class organization_business_units
                {
                    public const string Name = "organization_business_units";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_businessunit = "businessunit";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_calendars
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_calendars
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                     False
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
                /// ReferencingEntity calendar:
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
                public static partial class organization_calendars
                {
                    public const string Name = "organization_calendars";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_calendar = "calendar";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_cdi_domain
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_cdi_domain
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                       True
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
                /// ReferencingEntity cdi_domain:
                ///     DisplayName:
                ///     (English - United States - 1033): Domain
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Domains
                ///</summary>
                public static partial class organization_cdi_domain
                {
                    public const string Name = "organization_cdi_domain";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_cdi_domain = "cdi_domain";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship organization_cdi_filter
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_cdi_filter
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                       True
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
                /// ReferencingEntity cdi_filter:
                ///     DisplayName:
                ///     (English - United States - 1033): Filter
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Filters
                ///</summary>
                public static partial class organization_cdi_filter
                {
                    public const string Name = "organization_cdi_filter";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_cdi_filter = "cdi_filter";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship organization_cdi_formcapture
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_cdi_formcapture
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                            True
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
                /// ReferencingEntity cdi_formcapture:
                ///     DisplayName:
                ///     (English - United States - 1033): Form Capture
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Form Captures
                ///</summary>
                public static partial class organization_cdi_formcapture
                {
                    public const string Name = "organization_cdi_formcapture";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_cdi_formcapture = "cdi_formcapture";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship organization_cdi_formcapturefield
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_cdi_formcapturefield
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                                 True
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
                /// ReferencingEntity cdi_formcapturefield:
                ///     DisplayName:
                ///     (English - United States - 1033): Form Capture Field
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Form Capture Fields
                ///</summary>
                public static partial class organization_cdi_formcapturefield
                {
                    public const string Name = "organization_cdi_formcapturefield";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_cdi_formcapturefield = "cdi_formcapturefield";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_label = "cdi_label";
                }

                ///<summary>
                /// 1:N - Relationship organization_cdi_formfield
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_cdi_formfield
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                          True
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
                /// ReferencingEntity cdi_formfield:
                ///     DisplayName:
                ///     (English - United States - 1033): Form Field
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Form Fields
                ///</summary>
                public static partial class organization_cdi_formfield
                {
                    public const string Name = "organization_cdi_formfield";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_cdi_formfield = "cdi_formfield";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship organization_cdi_optionmapping
                /// 
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_cdi_optionmapping
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                              True
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
                /// ReferencingEntity cdi_optionmapping:
                ///     DisplayName:
                ///     (Russian - 1049): Option Mapping
                ///     
                ///     DisplayCollectionName:
                ///</summary>
                public static partial class organization_cdi_optionmapping
                {
                    public const string Name = "organization_cdi_optionmapping";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_cdi_optionmapping = "cdi_optionmapping";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_option = "cdi_option";
                }

                ///<summary>
                /// 1:N - Relationship organization_cdi_profile
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_cdi_profile
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                        True
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
                /// ReferencingEntity cdi_profile:
                ///     DisplayName:
                ///     (English - United States - 1033): Profile
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Profiles
                ///</summary>
                public static partial class organization_cdi_profile
                {
                    public const string Name = "organization_cdi_profile";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_cdi_profile = "cdi_profile";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship organization_cdi_setting
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_cdi_setting
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                        True
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
                /// ReferencingEntity cdi_setting:
                ///     DisplayName:
                ///     (English - United States - 1033): Setting
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Settings
                ///</summary>
                public static partial class organization_cdi_setting
                {
                    public const string Name = "organization_cdi_setting";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_cdi_setting = "cdi_setting";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_key = "cdi_key";
                }

                ///<summary>
                /// 1:N - Relationship organization_competitors
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_competitors
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
                /// ReferencingEntity competitor:
                ///     DisplayName:
                ///     (English - United States - 1033): Competitor
                ///     (Russian - 1049): Конкурент
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Competitors
                ///     (Russian - 1049): Конкуренты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Business competing for the sale represented by a lead or opportunity.
                ///     (Russian - 1049): Компания, которая борется за продажу, представленную интересом или возможной сделкой.
                ///</summary>
                public static partial class organization_competitors
                {
                    public const string Name = "organization_competitors";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_competitor = "competitor";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_complexcontrols
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_complexcontrols
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                           False
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
                /// ReferencingEntity complexcontrol:
                ///     DisplayName:
                ///     (English - United States - 1033): Process Configuration
                ///     (Russian - 1049): Конфигурация процесса
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Process Configurations
                ///     (Russian - 1049): Конфигурации процесса
                ///     
                ///     Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                ///</summary>
                public static partial class organization_complexcontrols
                {
                    public const string Name = "organization_complexcontrols";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_complexcontrol = "complexcontrol";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_connection_roles
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_connection_roles
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                            False
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
                /// ReferencingEntity connectionrole:
                ///     DisplayName:
                ///     (English - United States - 1033): Connection Role
                ///     (Russian - 1049): Роль подключения
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Connection Roles
                ///     (Russian - 1049): Роли подключения
                ///     
                ///     Description:
                ///     (English - United States - 1033): Role describing a relationship between a two records.
                ///     (Russian - 1049): Роль, описывающая отношение между двумя записями.
                ///</summary>
                public static partial class organization_connection_roles
                {
                    public const string Name = "organization_connection_roles";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_connectionrole = "connectionrole";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_constraint_based_groups
                /// 
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_constraint_based_groups
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                   False
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
                /// ReferencingEntity constraintbasedgroup:
                ///     DisplayName:
                ///     (English - United States - 1033): Resource Group
                ///     (Russian - 1049): Группа ресурсов
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Resource Groups
                ///     (Russian - 1049): Группы ресурсов
                ///     
                ///     Description:
                ///     (English - United States - 1033): Group or collection of people, equipment, and/or facilities that can be scheduled.
                ///     (Russian - 1049): Группа людей, оборудования и (или) помещений, которые могут быть запланированы.
                ///</summary>
                public static partial class organization_constraint_based_groups
                {
                    public const string Name = "organization_constraint_based_groups";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_constraintbasedgroup = "constraintbasedgroup";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_contract_templates
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_contract_templates
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                              False
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
                /// ReferencingEntity contracttemplate:
                ///     DisplayName:
                ///     (English - United States - 1033): Contract Template
                ///     (Russian - 1049): Шаблон контракта
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Contract Templates
                ///     (Russian - 1049): Шаблоны контрактов
                ///     
                ///     Description:
                ///     (English - United States - 1033): Template for a contract containing the standard attributes of a contract.
                ///     (Russian - 1049): Шаблон контракта, содержащий стандартные атрибуты контракта.
                ///</summary>
                public static partial class organization_contract_templates
                {
                    public const string Name = "organization_contract_templates";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_contracttemplate = "contracttemplate";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_custom_displaystrings
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_custom_displaystrings
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity displaystring:
                ///     DisplayName:
                ///     (English - United States - 1033): Display String
                ///     (Russian - 1049): Отображаемая строка
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Display Strings
                ///     (Russian - 1049): Отображаемые строки
                ///     
                ///     Description:
                ///     (English - United States - 1033): Customized messages for an entity that has been renamed.
                ///     (Russian - 1049): Изменяемые сообщения для сущности, которая была переименована.
                ///</summary>
                public static partial class organization_custom_displaystrings
                {
                    public const string Name = "organization_custom_displaystrings";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_displaystring = "displaystring";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_delveactionhub
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_delveactionhub
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                           False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity delveactionhub:
                ///     DisplayName:
                ///     (English - United States - 1033): DelveActionHub
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Smart Actions
                ///     (Russian - 1049): Смарт-действия
                ///     
                ///     Description:
                ///     (English - United States - 1033): Delve Action Hubs Description
                ///     (Russian - 1049): Описание центров действий Delve
                ///</summary>
                public static partial class organization_delveactionhub
                {
                    public const string Name = "organization_delveactionhub";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_delveactionhub = "delveactionhub";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship organization_discount_types
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_discount_types
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity discounttype:
                ///     DisplayName:
                ///     (English - United States - 1033): Discount List
                ///     (Russian - 1049): Список скидок
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Discount Lists
                ///     (Russian - 1049): Списки скидок
                ///     
                ///     Description:
                ///     (English - United States - 1033): Type of discount specified as either a percentage or an amount.
                ///     (Russian - 1049): Тип скидки, указанной в виде процента или суммы.
                ///</summary>
                public static partial class organization_discount_types
                {
                    public const string Name = "organization_discount_types";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_discounttype = "discounttype";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_emailserverprofile
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_emailserverprofile
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                              False
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
                /// ReferencingEntity emailserverprofile:
                ///     DisplayName:
                ///     (English - United States - 1033): Email Server Profile
                ///     (Russian - 1049): Профиль сервера электронной почты
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Email Server Profiles
                ///     (Russian - 1049): Профили серверов электронной почты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Holds the Email Server Profiles of an organization
                ///     (Russian - 1049): Содержит профили серверов электронной почты организации
                ///</summary>
                public static partial class organization_emailserverprofile
                {
                    public const string Name = "organization_emailserverprofile";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_emailserverprofile = "emailserverprofile";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_entitymap
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_entitymap
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                     False
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
                /// ReferencingEntity entitymap:
                ///     DisplayName:
                ///     (English - United States - 1033): Entity Map
                ///     (Russian - 1049): Сопоставление сущностей
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Entity Maps
                ///     (Russian - 1049): Сопоставления сущностей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Represents a mapping between two related entities so that data from one record can be copied into the form of a new related record.
                ///     (Russian - 1049): Представляет сопоставление между двумя связанными сущностями таким образом, что данные из одной записи могут быть скопированы в форму новой связанной записи.
                ///</summary>
                public static partial class organization_entitymap
                {
                    public const string Name = "organization_entitymap";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_entitymap = "entitymap";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_equipment
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_equipment
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                     False
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
                /// ReferencingEntity equipment:
                ///     DisplayName:
                ///     (English - United States - 1033): Facility/Equipment
                ///     (Russian - 1049): Помещения / оборудование
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Facilities/Equipment
                ///     
                ///     Description:
                ///     (English - United States - 1033): Resource that can be scheduled.
                ///     (Russian - 1049): Ресурс, который может быть запланирован.
                ///</summary>
                public static partial class organization_equipment
                {
                    public const string Name = "organization_equipment";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_equipment = "equipment";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_expiredprocess
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_expiredprocess
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                           False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity expiredprocess:
                ///     DisplayName:
                ///     (English - United States - 1033): Expired Process
                ///     (Russian - 1049): Процесс с истекшим сроком действия
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Expired Process Business Process Flow
                ///     (Russian - 1049): Процесс с истекшим сроком действия — последовательность операций бизнес-процесса
                ///</summary>
                public static partial class organization_expiredprocess
                {
                    public const string Name = "organization_expiredprocess";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_expiredprocess = "expiredprocess";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_hierarchyrules
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_hierarchyrules
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity hierarchyrule:
                ///     DisplayName:
                ///     (English - United States - 1033): Hierarchy Rule
                ///     (Russian - 1049): Правило иерархии
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Hierarchy Rules
                ///     (Russian - 1049): Правила иерархии
                ///     
                ///     Description:
                ///     (English - United States - 1033): Organization-owned entity customizations including mapping Quick view form with Relationship Id
                ///     (Russian - 1049): Настройки сущности, принадлежащие организации, включая сопоставление экспресс-формы с идентификатором отношения.
                ///</summary>
                public static partial class organization_hierarchyrules
                {
                    public const string Name = "organization_hierarchyrules";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_hierarchyrule = "hierarchyrule";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_importjob
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_importjob
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                     False
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
                /// ReferencingEntity importjob:
                ///     DisplayName:
                ///     (English - United States - 1033): Import Job
                ///     (Russian - 1049): Задание импорта
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Import Jobs
                ///     (Russian - 1049): Задания импорта
                ///     
                ///     Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                ///</summary>
                public static partial class organization_importjob
                {
                    public const string Name = "organization_importjob";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_importjob = "importjob";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_indexed_documents
                /// 
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_indexed_documents
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                             False
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
                /// ReferencingEntity documentindex:
                ///     DisplayName:
                ///     (English - United States - 1033): Indexed Article
                ///     (Russian - 1049): Индексированная статья
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Indexed Articles
                ///     (Russian - 1049): Индексированные статьи
                ///     
                ///     Description:
                ///     (English - United States - 1033): Article indexed for search purposes.
                ///     (Russian - 1049): Статья, индексированная в целях ускорения поиска.
                ///</summary>
                public static partial class organization_indexed_documents
                {
                    public const string Name = "organization_indexed_documents";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_documentindex = "documentindex";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship organization_integration_statuses
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_integration_statuses
                /// ReferencingEntityNavigationPropertyName    organizationid_organization
                /// IsCustomizable                             False                                False
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
                /// ReferencingEntity integrationstatus:
                ///     DisplayName:
                ///     (English - United States - 1033): Integration Status
                ///     (Russian - 1049): Состояние объединения
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Integration Statuses
                ///     (Russian - 1049): Состояния объединения
                ///     
                ///     Description:
                ///     (English - United States - 1033): Contains integration status information.
                ///     (Russian - 1049): Содержит сведения о состоянии объединения.
                ///</summary>
                public static partial class organization_integration_statuses
                {
                    public const string Name = "organization_integration_statuses";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_integrationstatus = "integrationstatus";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_isvconfigs
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_isvconfigs
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
                /// ReferencingEntity isvconfig:
                ///     DisplayName:
                ///     (English - United States - 1033): ISV Config
                ///     (Russian - 1049): Конфигурация ISV
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): An XML document used to configure client extension controls.
                ///     (Russian - 1049): XML-документ, используемый для настройки элементов управления расширением клиента.
                ///</summary>
                public static partial class organization_isvconfigs
                {
                    public const string Name = "organization_isvconfigs";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_isvconfig = "isvconfig";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_kb_article_templates
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_kb_article_templates
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                False
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
                /// ReferencingEntity kbarticletemplate:
                ///     DisplayName:
                ///     (English - United States - 1033): Article Template
                ///     (Russian - 1049): Шаблон статьи
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Article Templates
                ///     (Russian - 1049): Шаблоны статей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Template for a knowledge base article that contains the standard attributes of an article.
                ///     (Russian - 1049): Шаблон статьи базы знаний, содержащий стандартные атрибуты статьи.
                ///</summary>
                public static partial class organization_kb_article_templates
                {
                    public const string Name = "organization_kb_article_templates";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_kbarticletemplate = "kbarticletemplate";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship organization_kb_articles
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_kb_articles
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
                /// ReferencingEntity kbarticle:
                ///     DisplayName:
                ///     (English - United States - 1033): Article
                ///     (Russian - 1049): Статья
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Articles
                ///     (Russian - 1049): Статьи
                ///     
                ///     Description:
                ///     (English - United States - 1033): Structured content that is part of the knowledge base.
                ///     (Russian - 1049): Структурированный контент, являющийся частью базы знаний.
                ///</summary>
                public static partial class organization_kb_articles
                {
                    public const string Name = "organization_kb_articles";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_kbarticle = "kbarticle";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship organization_KnowledgeBaseRecord
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_KnowledgeBaseRecord
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                                False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity knowledgebaserecord:
                ///     DisplayName:
                ///     (English - United States - 1033): Knowledge Base Record
                ///     (Russian - 1049): Запись базы знаний
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Knowledge Base Records
                ///     (Russian - 1049): Записи базы знаний
                ///     
                ///     Description:
                ///     (English - United States - 1033): Metadata of knowledge base (KB) articles associated with Microsoft Dynamics 365 entities.
                ///     (Russian - 1049): Метаданные статей базы знаний, связанных с сущностями Microsoft Dynamics 365.
                ///</summary>
                public static partial class organization_knowledgebaserecord
                {
                    public const string Name = "organization_KnowledgeBaseRecord";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_knowledgebaserecord = "knowledgebaserecord";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship organization_knowledgesearchmodel
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_knowledgesearchmodel
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                False
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
                /// ReferencingEntity knowledgesearchmodel:
                ///     DisplayName:
                ///     (English - United States - 1033): Knowledge Search Model
                ///     (Russian - 1049): Модель поиска в базе знаний
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Configuration for automatic suggestion of knowledge articles using text analytics and search
                ///     (Russian - 1049): Конфигурация для автоматических рекомендаций статей базы знаний с помощью текстовой аналитики и поиска
                ///</summary>
                public static partial class organization_knowledgesearchmodel
                {
                    public const string Name = "organization_knowledgesearchmodel";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_knowledgesearchmodel = "knowledgesearchmodel";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_leadtoopportunitysalesprocess
                /// 
                /// PropertyName                               Value                                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_leadtoopportunitysalesprocess
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                                          False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity leadtoopportunitysalesprocess:
                ///     DisplayName:
                ///     (English - United States - 1033): Lead To Opportunity Sales Process
                ///     (Russian - 1049): Преобразование интереса в возможную сделку
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Lead To Opportunity Sales Process Business Process Flow
                ///     (Russian - 1049): Преобразование интереса в возможную сделку — последовательность операций бизнес-процесса
                ///</summary>
                public static partial class organization_leadtoopportunitysalesprocess
                {
                    public const string Name = "organization_leadtoopportunitysalesprocess";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_leadtoopportunitysalesprocess = "leadtoopportunitysalesprocess";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_licenses
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_licenses
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
                /// ReferencingEntity license:
                ///     DisplayName:
                ///     (English - United States - 1033): License
                ///     (Russian - 1049): Лицензия
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Licenses
                ///     (Russian - 1049): Лицензии
                ///     
                ///     Description:
                ///     (English - United States - 1033): Stores information about a Microsoft CRM license.
                ///     (Russian - 1049): Хранит в себе информацию о лицензии Microsoft CRM.
                ///</summary>
                public static partial class organization_licenses
                {
                    public const string Name = "organization_licenses";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_license = "license";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_mailbox
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_mailbox
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
                /// ReferencingEntity mailbox:
                ///     DisplayName:
                ///     (English - United States - 1033): Mailbox
                ///     (Russian - 1049): Почтовый ящик
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Mailboxes
                ///     (Russian - 1049): Почтовые ящики
                ///</summary>
                public static partial class organization_mailbox
                {
                    public const string Name = "organization_mailbox";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_mailbox = "mailbox";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_mailboxstatistics
                /// 
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_mailboxstatistics
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                             False
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
                /// ReferencingEntity mailboxstatistics:
                ///     DisplayName:
                ///     (English - United States - 1033): Mailbox Statistics
                ///     (Russian - 1049): Статистика почтовых ящиков
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Stores data regarding Mailbox processing cycles
                ///     (Russian - 1049): Хранит данные о циклах обработки почтовых ящиков
                ///</summary>
                public static partial class organization_mailboxstatistics
                {
                    public const string Name = "organization_mailboxstatistics";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_mailboxstatistics = "mailboxstatistics";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship Organization_MailboxTrackingFolder
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Organization_MailboxTrackingFolder
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity mailboxtrackingfolder:
                ///     DisplayName:
                ///     (English - United States - 1033): Mailbox Auto Tracking Folder
                ///     (Russian - 1049): Папка автоматического отслеживания почтового ящика
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Mailbox Auto Tracking Folders
                ///     (Russian - 1049): Папки автоматического отслеживания почтового ящика
                ///     
                ///     Description:
                ///     (English - United States - 1033): Stores data about what folders for a mailbox are auto tracked
                ///     (Russian - 1049): Хранит данные о том, какие папки для почтового ящика отслеживаются автоматически
                ///</summary>
                public static partial class organization_mailboxtrackingfolder
                {
                    public const string Name = "Organization_MailboxTrackingFolder";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_mailboxtrackingfolder = "mailboxtrackingfolder";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_metric
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_metric
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity metric:
                ///     DisplayName:
                ///     (English - United States - 1033): Goal Metric
                ///     (Russian - 1049): Показатель цели
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Goal Metrics
                ///     (Russian - 1049): Показатели цели
                ///     
                ///     Description:
                ///     (English - United States - 1033): Type of measurement for a goal, such as money amount or count.
                ///     (Russian - 1049): Тип измерения цели, например, сумма денег или число.
                ///</summary>
                public static partial class organization_metric
                {
                    public const string Name = "organization_metric";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_metric = "metric";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_msdyn_postconfig
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_msdyn_postconfig
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                             True
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
                /// ReferencingEntity msdyn_postconfig:
                ///     DisplayName:
                ///     (English - United States - 1033): Post Configuration
                ///     (Russian - 1049): Конфигурация записи
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Post Configurations
                ///     (Russian - 1049): Конфигурации записей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Enable or disable entities for Activity Feeds and Yammer collaboration.
                ///     (Russian - 1049): Включить или отключить сущности для совместной работы Лент новостей и Yammer.
                ///</summary>
                public static partial class organization_msdyn_postconfig
                {
                    public const string Name = "organization_msdyn_postconfig";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_msdyn_postconfig = "msdyn_postconfig";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_msdyn_entitydisplayname = "msdyn_entitydisplayname";
                }

                ///<summary>
                /// 1:N - Relationship organization_msdyn_postruleconfig
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_msdyn_postruleconfig
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                                 True
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
                /// ReferencingEntity msdyn_postruleconfig:
                ///     DisplayName:
                ///     (English - United States - 1033): Post Rule Configuration
                ///     (Russian - 1049): Конфигурация правила записей
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Post Rule Configurations
                ///     (Russian - 1049): Конфигурации правил записей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Enable or disable system post rules for an entity for Activity Feeds and Yammer.
                ///     (Russian - 1049): Включить или отключить системные правила публикации для объекта в лентах активности и в Yammer.
                ///</summary>
                public static partial class organization_msdyn_postruleconfig
                {
                    public const string Name = "organization_msdyn_postruleconfig";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_msdyn_postruleconfig = "msdyn_postruleconfig";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_msdyn_name = "msdyn_name";
                }

                ///<summary>
                /// 1:N - Relationship organization_msdyn_wallsavedquery
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_msdyn_wallsavedquery
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                                 True
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
                /// ReferencingEntity msdyn_wallsavedquery:
                ///     DisplayName:
                ///     (English - United States - 1033): Wall View
                ///     (Russian - 1049): Представление стены
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Wall Views
                ///     (Russian - 1049): Представления стены
                ///     
                ///     Description:
                ///     (English - United States - 1033): Contains information regarding which views are available for users to display on their personal walls. Only an administrator can specify the views that users can choose from to display on their personal walls.
                ///     (Russian - 1049): Содержит сведения о том, какие представления доступны пользователям для отображения на личной стене. Только администратор может указать представления, которые пользователи могут выбрать для отображения на личной стене.
                ///</summary>
                public static partial class organization_msdyn_wallsavedquery
                {
                    public const string Name = "organization_msdyn_wallsavedquery";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_msdyn_wallsavedquery = "msdyn_wallsavedquery";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_msdyn_entityname = "msdyn_entityname";
                }

                ///<summary>
                /// 1:N - Relationship organization_newprocess
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_newprocess
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                       False
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
                /// ReferencingEntity newprocess:
                ///     DisplayName:
                ///     (English - United States - 1033): New Process
                ///     (Russian - 1049): Новый процесс
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): New Process Business Process Flow
                ///     (Russian - 1049): Новый процесс — последовательность операций бизнес-процесса
                ///</summary>
                public static partial class organization_newprocess
                {
                    public const string Name = "organization_newprocess";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_newprocess = "newprocess";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_officegraphdocument
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_officegraphdocument
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                                False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity officegraphdocument:
                ///     DisplayName:
                ///     (English - United States - 1033): Office Graph Document
                ///     (Russian - 1049): Документ Office Graph
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Office Graph Documents
                ///     (Russian - 1049): Документы Office Graph
                ///     
                ///     Description:
                ///     (English - United States - 1033): Office Graph Documents Description
                ///     (Russian - 1049): Описание документов Office Graph
                ///</summary>
                public static partial class organization_officegraphdocument
                {
                    public const string Name = "organization_officegraphdocument";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_officegraphdocument = "officegraphdocument";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship organization_opportunitysalesprocess
                /// 
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_opportunitysalesprocess
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                                    False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity opportunitysalesprocess:
                ///     DisplayName:
                ///     (English - United States - 1033): Opportunity Sales Process
                ///     (Russian - 1049): Преобразование возможной сделки в продажу
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Opportunity Sales Process Business Process Flow
                ///     (Russian - 1049): Преобразование возможной сделки в продажу — последовательность операций бизнес-процесса
                ///</summary>
                public static partial class organization_opportunitysalesprocess
                {
                    public const string Name = "organization_opportunitysalesprocess";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_opportunitysalesprocess = "opportunitysalesprocess";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_orginsightsmetric
                /// 
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_orginsightsmetric
                /// ReferencingEntityNavigationPropertyName    organization_orginsightsmetric
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
                /// ReferencingEntity orginsightsmetric:
                ///     DisplayName:
                ///     (English - United States - 1033): Organization Insights Metric
                ///     (Russian - 1049): Показатель сведений об организации
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Stores data regarding organization insights metric
                ///     (Russian - 1049): Сохраняет данные, связанные с показателем сведений об организации
                ///</summary>
                public static partial class organization_orginsightsmetric
                {
                    public const string Name = "organization_orginsightsmetric";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_orginsightsmetric = "orginsightsmetric";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_phonetocaseprocess
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_phonetocaseprocess
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                               False
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
                /// ReferencingEntity phonetocaseprocess:
                ///     DisplayName:
                ///     (English - United States - 1033): Phone To Case Process
                ///     (Russian - 1049): Преобразование звонка в обращение
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Phone To Case Process Business Process Flow
                ///     (Russian - 1049): Преобразование звонка в обращение — последовательность операций бизнес-процесса
                ///</summary>
                public static partial class organization_phonetocaseprocess
                {
                    public const string Name = "organization_phonetocaseprocess";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_phonetocaseprocess = "phonetocaseprocess";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_pluginassembly
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_pluginassembly
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity pluginassembly:
                ///     DisplayName:
                ///     (English - United States - 1033): Plug-in Assembly
                ///     (Russian - 1049): Сборка подключаемого модуля
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Plug-in Assemblies
                ///     (Russian - 1049): Сборки подключаемых модулей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Assembly that contains one or more plug-in types.
                ///     (Russian - 1049): Сборка, содержащая один или несколько типов подключаемых модулей.
                ///</summary>
                public static partial class organization_pluginassembly
                {
                    public const string Name = "organization_pluginassembly";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_pluginassembly = "pluginassembly";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_plugintype
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_plugintype
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity plugintype:
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
                public static partial class organization_plugintype
                {
                    public const string Name = "organization_plugintype";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_plugintype = "plugintype";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_plugintypestatistic
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_plugintypestatistic
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity plugintypestatistic:
                ///     DisplayName:
                ///     (English - United States - 1033): Plug-in Type Statistic
                ///     (Russian - 1049): Статистика по типам подключаемых модулей
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Plug-in Type Statistics
                ///     (Russian - 1049): Статистики по типам подключаемых модулей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Plug-in type statistic.
                ///     (Russian - 1049): Статистика по типам подключаемых модулей.
                ///</summary>
                public static partial class organization_plugintypestatistic
                {
                    public const string Name = "organization_plugintypestatistic";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_plugintypestatistic = "plugintypestatistic";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_position
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_position
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity position:
                ///     DisplayName:
                ///     (English - United States - 1033): Position
                ///     (Russian - 1049): Положение
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Positions
                ///     (Russian - 1049): Положения
                ///     
                ///     Description:
                ///     (English - United States - 1033): Position of a user in the hierarchy
                ///     (Russian - 1049): Положение пользователя в иерархии
                ///</summary>
                public static partial class organization_position
                {
                    public const string Name = "organization_position";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_position = "position";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_post
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_post
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
                /// ReferencingEntity post:
                ///     DisplayName:
                ///     (English - United States - 1033): Post
                ///     (Russian - 1049): Запись
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Posts
                ///     (Russian - 1049): Записи
                ///     
                ///     Description:
                ///     (English - United States - 1033): An activity feed post.
                ///     (Russian - 1049): Запись в ленте новостей.
                ///</summary>
                public static partial class organization_post
                {
                    public const string Name = "organization_post";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_post = "post";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_text = "text";
                }

                ///<summary>
                /// 1:N - Relationship organization_PostComment
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_PostComment
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
                /// ReferencingEntity postcomment:
                ///     DisplayName:
                ///     (English - United States - 1033): Comment
                ///     (Russian - 1049): Комментарий
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Comments
                ///     (Russian - 1049): Комментарии
                ///     
                ///     Description:
                ///     (English - United States - 1033): A comment on an activity feed post.
                ///     (Russian - 1049): Комментарий к записи в ленте новостей.
                ///</summary>
                public static partial class organization_postcomment
                {
                    public const string Name = "organization_PostComment";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_postcomment = "postcomment";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_text = "text";
                }

                ///<summary>
                /// 1:N - Relationship organization_postlike
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_postlike
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
                /// ReferencingEntity postlike:
                ///     DisplayName:
                ///     (English - United States - 1033): Like
                ///     (Russian - 1049): Нравится
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Likes
                ///     
                ///     Description:
                ///     (English - United States - 1033): A like on an activity feed post.
                ///     (Russian - 1049): Выражение симпатии к записи в ленте новостей.
                ///</summary>
                public static partial class organization_postlike
                {
                    public const string Name = "organization_postlike";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_postlike = "postlike";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_postrole
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_postrole
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
                /// ReferencingEntity postrole:
                ///     DisplayName:
                ///     (English - United States - 1033): Post Role
                ///     (Russian - 1049): Роль записи
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Post Roles
                ///     (Russian - 1049): Роли записей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Represents the objects with which an activity feed post is associated. For internal use only.
                ///     (Russian - 1049): Представляет объекты, с которыми связана запись ленты новостей. Только для внутреннего использования.
                ///</summary>
                public static partial class organization_postrole
                {
                    public const string Name = "organization_postrole";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_postrole = "postrole";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_price_levels
                /// 
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_price_levels
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                        False
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
                /// ReferencingEntity pricelevel:
                ///     DisplayName:
                ///     (English - United States - 1033): Price List
                ///     (Russian - 1049): Прайс-лист
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Price Lists
                ///     (Russian - 1049): Прайс-листы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Entity that defines pricing levels.
                ///     (Russian - 1049): Сущность, определяющая уровни ценообразования.
                ///</summary>
                public static partial class organization_price_levels
                {
                    public const string Name = "organization_price_levels";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_pricelevel = "pricelevel";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_ProductAssociation
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_ProductAssociation
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity productassociation:
                ///     DisplayName:
                ///     (English - United States - 1033): Product Association
                ///     (Russian - 1049): Соответствие продукта
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Product Associations
                ///     (Russian - 1049): Соответствие продуктов
                ///     
                ///     Description:
                ///     (English - United States - 1033): Instance of a product added to a bundle or kit.
                ///     (Russian - 1049): Экземпляр продукта, добавленного в набор или комплект.
                ///</summary>
                public static partial class organization_productassociation
                {
                    public const string Name = "organization_ProductAssociation";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_productassociation = "productassociation";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_productidname = "productidname";
                }

                ///<summary>
                /// 1:N - Relationship organization_products
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_products
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
                /// ReferencingEntity product:
                ///     DisplayName:
                ///     (English - United States - 1033): Product
                ///     (Russian - 1049): Продукт
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Products
                ///     (Russian - 1049): Продукты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Information about products and their pricing information.
                ///     (Russian - 1049): Информация о продуктах и ценообразовании, применяющемся к ним.
                ///</summary>
                public static partial class organization_products
                {
                    public const string Name = "organization_products";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_product = "product";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_ProductSubstitute
                /// 
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_ProductSubstitute
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                             False
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
                /// ReferencingEntity productsubstitute:
                ///     DisplayName:
                ///     (English - United States - 1033): Product Relationship
                ///     (Russian - 1049): Отношение продукта
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Product Relationships
                ///     (Russian - 1049): Отношения продуктов
                ///     
                ///     Description:
                ///     (English - United States - 1033): Information about the selling relationship between two products, including the relationship type, such as up-sell, cross-sell, substitute, or accessory.
                ///     (Russian - 1049): Сведения об отношении между двумя продуктами с точки зрения продаж, включая тип отношения – "дополнительные продажи", "перекрестные продажи", "заменитель" или "дополнительный продукт".
                ///</summary>
                public static partial class organization_productsubstitute
                {
                    public const string Name = "organization_ProductSubstitute";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_productsubstitute = "productsubstitute";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_publisher
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_publisher
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                     False
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
                /// ReferencingEntity publisher:
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
                public static partial class organization_publisher
                {
                    public const string Name = "organization_publisher";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_publisher = "publisher";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_friendlyname = "friendlyname";
                }

                ///<summary>
                /// 1:N - Relationship organization_queueitems
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_queueitems
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity queueitem:
                ///     DisplayName:
                ///     (English - United States - 1033): Queue Item
                ///     (Russian - 1049): Элемент очереди
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Queue Items
                ///     (Russian - 1049): Элементы очереди
                ///     
                ///     Description:
                ///     (English - United States - 1033): A specific item in a queue, such as a case record or an activity record.
                ///     (Russian - 1049): Конкретный элемент в очереди (например, запись обращения или действия).
                ///</summary>
                public static partial class organization_queueitems
                {
                    public const string Name = "organization_queueitems";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_queueitem = "queueitem";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship organization_queues
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_queues
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
                /// ReferencingEntity queue:
                ///     DisplayName:
                ///     (English - United States - 1033): Queue
                ///     (Russian - 1049): Очередь
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Queues
                ///     (Russian - 1049): Очереди
                ///     
                ///     Description:
                ///     (English - United States - 1033): A list of records that require action, such as accounts, activities, and cases.
                ///     (Russian - 1049): Список записей, требующих действий от пользователя, например, организаций, действий и обращений.
                ///</summary>
                public static partial class organization_queues
                {
                    public const string Name = "organization_queues";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_queue = "queue";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_recommendationmodel
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_recommendationmodel
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity recommendationmodel:
                ///     DisplayName:
                ///     (English - United States - 1033): Product Recommendation Model
                ///     (Russian - 1049): Модель рекомендаций по продукту
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Product Recommendation Models
                ///     (Russian - 1049): Модели рекомендаций по продукту
                ///     
                ///     Description:
                ///     (English - United States - 1033): The product recommendation model built using the Azure Recommendations service to provide cross-sell recommendations.
                ///     (Russian - 1049): Модель рекомендаций по продукту, собранная с использованием службы рекомендаций Azure для предоставления рекомендаций для перекрестных продаж.
                ///</summary>
                public static partial class organization_recommendationmodel
                {
                    public const string Name = "organization_recommendationmodel";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_recommendationmodel = "recommendationmodel";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_recommendationmodelmapping
                /// 
                /// PropertyName                               Value                                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_recommendationmodelmapping
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity recommendationmodelmapping:
                ///     DisplayName:
                ///     (English - United States - 1033): Model Entity Mapping
                ///     (Russian - 1049): Сопоставление модели с сущностью
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Model Entity Mappings
                ///     (Russian - 1049): Сопоставления моделей с сущностями
                ///     
                ///     Description:
                ///     (English - United States - 1033): Entity mapping for the product recommendation model.
                ///     (Russian - 1049): Сопоставление с сущностью для модели рекомендаций по продукту.
                ///</summary>
                public static partial class organization_recommendationmodelmapping
                {
                    public const string Name = "organization_recommendationmodelmapping";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_recommendationmodelmapping = "recommendationmodelmapping";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_entitydisplayname = "entitydisplayname";
                }

                ///<summary>
                /// 1:N - Relationship organization_recommendationmodelversion
                /// 
                /// PropertyName                               Value                                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_recommendationmodelversion
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                                       False
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
                /// ReferencingEntity recommendationmodelversion:
                ///     DisplayName:
                ///     (English - United States - 1033): Recommendation Model Version
                ///     (Russian - 1049): Версия модели рекомендаций
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Recommendation Model Versions
                ///     (Russian - 1049): Версии модели рекомендаций
                ///     
                ///     Description:
                ///     (English - United States - 1033): The product recommendation model version that's built using the Azure recommendation service.
                ///     (Russian - 1049): Версия модели рекомендаций по продукту, собранная с использованием службы рекомендаций Azure.
                ///</summary>
                public static partial class organization_recommendationmodelversion
                {
                    public const string Name = "organization_recommendationmodelversion";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_recommendationmodelversion = "recommendationmodelversion";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_recommendationmodelversionhistory
                /// 
                /// PropertyName                               Value                                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_recommendationmodelversionhistory
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                                              False
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
                /// ReferencingEntity recommendationmodelversionhistory:
                ///     DisplayName:
                ///     (English - United States - 1033): Recommendation Model Version Execution History
                ///     (Russian - 1049): Журнал выполнения версии модели рекомендаций
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Recommendation Model Version Execution History Records
                ///     (Russian - 1049): Записи журнала выполнения версии модели рекомендаций
                ///</summary>
                public static partial class organization_recommendationmodelversionhistory
                {
                    public const string Name = "organization_recommendationmodelversionhistory";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_recommendationmodelversionhistory = "recommendationmodelversionhistory";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_recommendeddocument
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_recommendeddocument
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                                False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity recommendeddocument:
                ///     DisplayName:
                ///     (English - United States - 1033): Document Suggestions
                ///     (Russian - 1049): Предложения документов
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///</summary>
                public static partial class organization_recommendeddocument
                {
                    public const string Name = "organization_recommendeddocument";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_recommendeddocument = "recommendeddocument";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship organization_relationship_roles
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_relationship_roles
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                              False
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
                /// ReferencingEntity relationshiprole:
                ///     DisplayName:
                ///     (English - United States - 1033): Relationship Role
                ///     (Russian - 1049): Роль отношений
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Relationship Roles
                ///     (Russian - 1049): Роли в отношениях
                ///     
                ///     Description:
                ///     (English - United States - 1033): Relationship between an account or contact and an opportunity.
                ///     (Russian - 1049): Отношение между организацией или контактом и возможной сделкой.
                ///</summary>
                public static partial class organization_relationship_roles
                {
                    public const string Name = "organization_relationship_roles";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_relationshiprole = "relationshiprole";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_resource_groups
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_resource_groups
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                           False
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
                /// ReferencingEntity resourcegroup:
                ///     DisplayName:
                ///     (English - United States - 1033): Scheduling Group
                ///     (Russian - 1049): Группа планирования
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Scheduling Groups
                ///     (Russian - 1049): Группы планирования
                ///     
                ///     Description:
                ///     (English - United States - 1033): Resource group or team whose members can be scheduled for a service.
                ///     (Russian - 1049): Группа ресурсов или рабочая группа, участники которой могут быть запланированы для сервиса.
                ///</summary>
                public static partial class organization_resource_groups
                {
                    public const string Name = "organization_resource_groups";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_resourcegroup = "resourcegroup";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_resource_specs
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_resource_specs
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity resourcespec:
                ///     DisplayName:
                ///     (English - United States - 1033): Resource Specification
                ///     (Russian - 1049): Спецификация ресурсов
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Resource Specifications
                ///     (Russian - 1049): Спецификации ресурсов
                ///     
                ///     Description:
                ///     (English - United States - 1033): Selection rule that allows the scheduling engine to select a number of resources from a pool of resources. The rules can be associated with a service.
                ///     (Russian - 1049): Правило выбора, позволяющее ядру планирования выбирать определенное количество ресурсов из пула ресурсов. Правила могут быть связаны с сервисом.
                ///</summary>
                public static partial class organization_resource_specs
                {
                    public const string Name = "organization_resource_specs";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_resourcespec = "resourcespec";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_resources
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_resources
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                     False
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
                /// ReferencingEntity resource:
                ///     DisplayName:
                ///     (English - United States - 1033): Resource
                ///     (Russian - 1049): Ресурс
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Resources
                ///     (Russian - 1049): Ресурсы
                ///     
                ///     Description:
                ///     (English - United States - 1033): User or facility/equipment that can be scheduled for a service.
                ///     (Russian - 1049): Пользователь или оборудование, которые могут быть запланированы для сервиса.
                ///</summary>
                public static partial class organization_resources
                {
                    public const string Name = "organization_resources";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_resource = "resource";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_ribbon_command
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_ribbon_command
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity ribboncommand:
                ///     DisplayName:
                ///     (English - United States - 1033): Ribbon Command
                ///     (Russian - 1049): Команда ленты
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Ribbon Commands
                ///     (Russian - 1049): Команды ленты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Ribbon Commands - the command definition, rules, etc.
                ///     (Russian - 1049): Команды ленты - определение команды, правила и т.п.
                ///</summary>
                public static partial class organization_ribbon_command
                {
                    public const string Name = "organization_ribbon_command";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_ribboncommand = "ribboncommand";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_ribbon_context_group
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_ribbon_context_group
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                False
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
                /// ReferencingEntity ribboncontextgroup:
                ///     DisplayName:
                ///     (English - United States - 1033): Ribbon Context Group
                ///     (Russian - 1049): Контекстная группа ленты
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Ribbon Context Groups
                ///     (Russian - 1049): Контекстные группы ленты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Groupings of contextual tabs.
                ///     (Russian - 1049): Группы контекстных вкладок.
                ///</summary>
                public static partial class organization_ribbon_context_group
                {
                    public const string Name = "organization_ribbon_context_group";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_ribboncontextgroup = "ribboncontextgroup";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_ribbon_customization
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_ribbon_customization
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                False
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
                /// ReferencingEntity ribboncustomization:
                ///     DisplayName:
                ///     (English - United States - 1033): Application Ribbons
                ///     (Russian - 1049): Ленты приложения
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Ribbon customizations for the application ribbon and entity ribbon templates.
                ///     (Russian - 1049): Настройки ленты для ленты приложения и шаблоны ленты сущности.
                ///</summary>
                public static partial class organization_ribbon_customization
                {
                    public const string Name = "organization_ribbon_customization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_ribboncustomization = "ribboncustomization";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_ribbon_diff
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_ribbon_diff
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
                /// ReferencingEntity ribbondiff:
                ///     DisplayName:
                ///     (English - United States - 1033): Ribbon Difference
                ///     (Russian - 1049): Различие ленты
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Ribbon Differences
                ///     (Russian - 1049): Различия ленты
                ///     
                ///     Description:
                ///     (English - United States - 1033): All layout customizations to be applied to the ribbons, which contain only the differences from the base ribbon.
                ///     (Russian - 1049): Все настройки макета, подлежащие применению к лентам, называются различиями. К базовой ленте применяются только изменения (различия).
                ///</summary>
                public static partial class organization_ribbon_diff
                {
                    public const string Name = "organization_ribbon_diff";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_ribbondiff = "ribbondiff";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_ribbon_rule
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_ribbon_rule
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
                /// ReferencingEntity ribbonrule:
                ///     DisplayName:
                ///     (English - United States - 1033): Ribbon Rule
                ///     (Russian - 1049): Правило ленты
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Ribbon Rules
                ///     (Russian - 1049): Правила ленты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Ribbon rule definitions, used to enable and disable, show and hide ribbon elements.
                ///     (Russian - 1049): Определение правил ленты, которые используются для включения и отключения, а также скрытия элементов ленты.
                ///</summary>
                public static partial class organization_ribbon_rule
                {
                    public const string Name = "organization_ribbon_rule";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_ribbonrule = "ribbonrule";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_ribbon_tab_to_command_map
                /// 
                /// PropertyName                               Value                                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_ribbon_tab_to_command_map
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                     False
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
                /// ReferencingEntity ribbontabtocommandmap:
                ///     DisplayName:
                ///     (English - United States - 1033): Ribbon Tab To Command Mapping
                ///     (Russian - 1049): Сопоставление вкладки ленты с командой
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Ribbon Tab To Command Map
                ///     
                ///     Description:
                ///     (English - United States - 1033): A mapping between Tab Ids, and the Commands within those tabs.
                ///     (Russian - 1049): Сопоставление между идентификаторами вкладок и командами на этих вкладках.
                ///</summary>
                public static partial class organization_ribbon_tab_to_command_map
                {
                    public const string Name = "organization_ribbon_tab_to_command_map";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_ribbontabtocommandmap = "ribbontabtocommandmap";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_roles
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
                /// ReferencingEntity role:
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
                public static partial class organization_roles
                {
                    public const string Name = "organization_roles";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_role = "role";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_routingruleitems
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_routingruleitems
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                            False
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
                /// ReferencingEntity routingruleitem:
                ///     DisplayName:
                ///     (English - United States - 1033): Rule Item
                ///     (Russian - 1049): Элемент правила
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Rule Items
                ///     (Russian - 1049): Элементы правила
                ///     
                ///     Description:
                ///     (English - United States - 1033): Please provide the description for entity
                ///     (Russian - 1049): Введите описание сущности
                ///</summary>
                public static partial class organization_routingruleitems
                {
                    public const string Name = "organization_routingruleitems";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_routingruleitem = "routingruleitem";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_RoutingRules
                /// 
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_RoutingRules
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                        False
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
                /// ReferencingEntity routingrule:
                ///     DisplayName:
                ///     (English - United States - 1033): Routing Rule Set
                ///     (Russian - 1049): Набор правил маршрутизации
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Routing Rule Sets
                ///     (Russian - 1049): Наборы правил маршрутизации
                ///     
                ///     Description:
                ///     (English - United States - 1033): Define Routing Rule to route cases to right people at the right time
                ///     (Russian - 1049): Определите правило маршрутизации для своевременного направления обращений нужным лицам
                ///</summary>
                public static partial class organization_routingrules
                {
                    public const string Name = "organization_RoutingRules";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_routingrule = "routingrule";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_sales_literature
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_sales_literature
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                            False
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
                /// ReferencingEntity salesliterature:
                ///     DisplayName:
                ///     (English - United States - 1033): Sales Literature
                ///     (Russian - 1049): Литература
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Storage of sales literature, which may contain one or more documents.
                ///     (Russian - 1049): Хранилище литературы, которое может содержать один или несколько документов.
                ///</summary>
                public static partial class organization_sales_literature
                {
                    public const string Name = "organization_sales_literature";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_salesliterature = "salesliterature";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_saved_queries
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_saved_queries
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity savedquery:
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
                public static partial class organization_saved_queries
                {
                    public const string Name = "organization_saved_queries";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_savedquery = "savedquery";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_saved_query_visualizations
                /// 
                /// PropertyName                               Value                                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_saved_query_visualizations
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity savedqueryvisualization:
                ///     DisplayName:
                ///     (English - United States - 1033): System Chart
                ///     (Russian - 1049): Системная диаграмма
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): System Charts
                ///     (Russian - 1049): Системные диаграммы
                ///     
                ///     Description:
                ///     (English - United States - 1033): System chart attached to an entity.
                ///     (Russian - 1049): Системная диаграмма, присоединенная к сущности.
                ///</summary>
                public static partial class organization_saved_query_visualizations
                {
                    public const string Name = "organization_saved_query_visualizations";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_savedqueryvisualization = "savedqueryvisualization";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_savedorginsightsconfiguration
                /// 
                /// PropertyName                               Value                                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_savedorginsightsconfiguration
                /// ReferencingEntityNavigationPropertyName    organization_savedorginsightsconfiguration
                /// IsCustomizable                             False                                         False
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
                /// ReferencingEntity savedorginsightsconfiguration:
                ///     DisplayName:
                ///     (English - United States - 1033): Saved Organization Insights Configuration
                ///     (Russian - 1049): Сохраненная конфигурация сведений об организации
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Saved Organization Insights Configurations
                ///     
                ///     Description:
                ///     (English - United States - 1033): Saved configuration for the organization insights
                ///</summary>
                public static partial class organization_savedorginsightsconfiguration
                {
                    public const string Name = "organization_savedorginsightsconfiguration";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_savedorginsightsconfiguration = "savedorginsightsconfiguration";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_sdkmessage
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_sdkmessage
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity sdkmessage:
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
                public static partial class organization_sdkmessage
                {
                    public const string Name = "organization_sdkmessage";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_sdkmessage = "sdkmessage";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_sdkmessagefilter
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_sdkmessagefilter
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                            False
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
                /// ReferencingEntity sdkmessagefilter:
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
                public static partial class organization_sdkmessagefilter
                {
                    public const string Name = "organization_sdkmessagefilter";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_sdkmessagefilter = "sdkmessagefilter";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_sdkmessagepair
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_sdkmessagepair
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity sdkmessagepair:
                ///     DisplayName:
                ///     (English - United States - 1033): Sdk Message Pair
                ///     (Russian - 1049): Пара сообщений SDK
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Sdk Message Pairs
                ///     (Russian - 1049): Пары сообщений SDK
                ///     
                ///     Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                ///</summary>
                public static partial class organization_sdkmessagepair
                {
                    public const string Name = "organization_sdkmessagepair";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_sdkmessagepair = "sdkmessagepair";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_sdkmessageprocessingstep
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
                /// ReferencingEntity sdkmessageprocessingstep:
                ///     DisplayName:
                ///     (English - United States - 1033): Sdk Message Processing Step
                ///     (Russian - 1049): Шаг обработки сообщения SDK
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Sdk Message Processing Steps
                ///     (Russian - 1049): Шаги обработки сообщения SDK
                ///     
                ///     Description:
                ///     (English - United States - 1033): Stage in the execution pipeline that a plug-in is to execute.
                ///     (Russian - 1049): Стадия конвейерной обработки, на которой выполняется запуск подключаемого модуля.
                ///</summary>
                public static partial class organization_sdkmessageprocessingstep
                {
                    public const string Name = "organization_sdkmessageprocessingstep";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_sdkmessageprocessingstep = "sdkmessageprocessingstep";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_sdkmessageprocessingstepimage
                /// 
                /// PropertyName                               Value                                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_sdkmessageprocessingstepimage
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                         False
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
                public static partial class organization_sdkmessageprocessingstepimage
                {
                    public const string Name = "organization_sdkmessageprocessingstepimage";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_sdkmessageprocessingstepimage = "sdkmessageprocessingstepimage";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_sdkmessageprocessingstepsecureconfig
                /// 
                /// PropertyName                               Value                                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_sdkmessageprocessingstepsecureconfig
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                                False
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
                /// ReferencingEntity sdkmessageprocessingstepsecureconfig:
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
                public static partial class organization_sdkmessageprocessingstepsecureconfig
                {
                    public const string Name = "organization_sdkmessageprocessingstepsecureconfig";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_sdkmessageprocessingstepsecureconfig = "sdkmessageprocessingstepsecureconfig";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_sdkmessagerequest
                /// 
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_sdkmessagerequest
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                             False
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
                /// ReferencingEntity sdkmessagerequest:
                ///     DisplayName:
                ///     (English - United States - 1033): Sdk Message Request
                ///     (Russian - 1049): Запрос сообщения SDK
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Sdk Message Requests
                ///     (Russian - 1049): Запросы сообщений SDK
                ///     
                ///     Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                ///</summary>
                public static partial class organization_sdkmessagerequest
                {
                    public const string Name = "organization_sdkmessagerequest";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_sdkmessagerequest = "sdkmessagerequest";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_sdkmessagerequestfield
                /// 
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_sdkmessagerequestfield
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity sdkmessagerequestfield:
                ///     DisplayName:
                ///     (English - United States - 1033): Sdk Message Request Field
                ///     (Russian - 1049): Поле запроса сообщения SDK
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Sdk Message Request Fields
                ///     (Russian - 1049): Поля запроса сообщения SDK
                ///     
                ///     Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                ///</summary>
                public static partial class organization_sdkmessagerequestfield
                {
                    public const string Name = "organization_sdkmessagerequestfield";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_sdkmessagerequestfield = "sdkmessagerequestfield";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_sdkmessageresponse
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_sdkmessageresponse
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                              False
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
                /// ReferencingEntity sdkmessageresponse:
                ///     DisplayName:
                ///     (English - United States - 1033): Sdk Message Response
                ///     (Russian - 1049): Ответ на сообщение SDK
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Sdk MessageResponses
                ///     (Russian - 1049): Ответы на сообщение SDK
                ///     
                ///     Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                ///</summary>
                public static partial class organization_sdkmessageresponse
                {
                    public const string Name = "organization_sdkmessageresponse";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_sdkmessageresponse = "sdkmessageresponse";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_sdkmessageresponsefield
                /// 
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_sdkmessageresponsefield
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                   False
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
                /// ReferencingEntity sdkmessageresponsefield:
                ///     DisplayName:
                ///     (English - United States - 1033): Sdk Message Response Field
                ///     (Russian - 1049): Поле ответа на сообщение SDK
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Sdk MessageResponse Fields
                ///     (Russian - 1049): Поля ответа на сообщение SDK
                ///     
                ///     Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                ///</summary>
                public static partial class organization_sdkmessageresponsefield
                {
                    public const string Name = "organization_sdkmessageresponsefield";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_sdkmessageresponsefield = "sdkmessageresponsefield";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_serviceendpoint
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_serviceendpoint
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                           False
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
                /// ReferencingEntity serviceendpoint:
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
                public static partial class organization_serviceendpoint
                {
                    public const string Name = "organization_serviceendpoint";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_serviceendpoint = "serviceendpoint";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_services
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_services
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
                /// ReferencingEntity service:
                ///     DisplayName:
                ///     (English - United States - 1033): Service
                ///     (Russian - 1049): Сервис
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Services
                ///     (Russian - 1049): Сервисы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity that represents work done to satisfy a customer's need.
                ///     (Russian - 1049): Действие, представляющее работы, выполненные с целью удовлетворения потребностей клиента.
                ///</summary>
                public static partial class organization_services
                {
                    public const string Name = "organization_services";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_service = "service";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_sharepointdata
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_sharepointdata
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity sharepointdata:
                ///     DisplayName:
                ///     (English - United States - 1033): SharePoint Data
                ///     (Russian - 1049): Данные SharePoint
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): SharePoint's Data Corresponding to a user , Record , Location and Page
                ///     (Russian - 1049): Данные SharePoint, соответствующие пользователю, записи, расположению и странице
                ///</summary>
                public static partial class organization_sharepointdata
                {
                    public const string Name = "organization_sharepointdata";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_sharepointdata = "sharepointdata";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_sharepointdocument
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_sharepointdocument
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                              False
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
                /// ReferencingEntity sharepointdocument:
                ///     DisplayName:
                ///     (English - United States - 1033): Sharepoint Document
                ///     (Russian - 1049): Документ SharePoint
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Documents
                ///     (Russian - 1049): Документы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Document libraries or folders on a SharePoint server from where documents can be managed in Microsoft Dynamics 365.
                ///     (Russian - 1049): Библиотеки документов или папки на сервере SharePoint, документами из которых можно управлять с помощью Microsoft Dynamics 365.
                ///</summary>
                public static partial class organization_sharepointdocument
                {
                    public const string Name = "organization_sharepointdocument";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_sharepointdocument = "sharepointdocument";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// 1:N - Relationship organization_similarityrule
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_similarityrule
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                           False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity similarityrule:
                ///     DisplayName:
                ///     (English - United States - 1033): Similarity Rule
                ///     (Russian - 1049): Правило подобия
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Similarity Rules
                ///     (Russian - 1049): Правила подобия
                ///</summary>
                public static partial class organization_similarityrule
                {
                    public const string Name = "organization_similarityrule";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_similarityrule = "similarityrule";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_sitemap
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_sitemap
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
                /// ReferencingEntity sitemap:
                ///     DisplayName:
                ///     (English - United States - 1033): Site Map
                ///     (Russian - 1049): Карта сайта
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Site Maps
                ///     (Russian - 1049): Карты сайтов
                ///     
                ///     Description:
                ///     (English - United States - 1033): XML data used to control the application navigation pane.
                ///     (Russian - 1049): Данные XML, используемые для управления областью навигации приложения.
                ///</summary>
                public static partial class organization_sitemap
                {
                    public const string Name = "organization_sitemap";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_sitemap = "sitemap";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_sites
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_sites
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
                /// ReferencingEntity site:
                ///     DisplayName:
                ///     (English - United States - 1033): Site
                ///     (Russian - 1049): Место
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Sites
                ///     (Russian - 1049): Места
                ///     
                ///     Description:
                ///     (English - United States - 1033): Location or branch office where an organization does business. An organization can have multiple sites.
                ///     (Russian - 1049): Расположение или филиал, в котором организация осуществляет коммерческую деятельность. Организация может иметь несколько участков.
                ///</summary>
                public static partial class organization_sites
                {
                    public const string Name = "organization_sites";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_site = "site";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_socialinsightsconfiguration
                /// 
                /// PropertyName                               Value                                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_socialinsightsconfiguration
                /// ReferencingEntityNavigationPropertyName    regardingobjectid
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
                /// ReferencingEntity socialinsightsconfiguration:
                ///     DisplayName:
                ///     (English - United States - 1033): SocialInsightsConfiguration
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): SocialInsightsConfigurations
                ///     
                ///     Description:
                ///     (English - United States - 1033): Configuration for the social insights.
                ///     (Russian - 1049): Настройка программы "Социальные данные".
                ///</summary>
                public static partial class organization_socialinsightsconfiguration
                {
                    public const string Name = "organization_socialinsightsconfiguration";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_socialinsightsconfiguration = "socialinsightsconfiguration";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_solution
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
                /// ReferencingEntity solution:
                ///     DisplayName:
                ///     (English - United States - 1033): Solution
                ///     (Russian - 1049): Решение
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Solutions
                ///     (Russian - 1049): Решения
                ///     
                ///     Description:
                ///     (English - United States - 1033): A solution which contains CRM customizations.
                ///     (Russian - 1049): Решение, содержащее настройки CRM.
                ///</summary>
                public static partial class organization_solution
                {
                    public const string Name = "organization_solution";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_solution = "solution";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_friendlyname = "friendlyname";
                }

                ///<summary>
                /// 1:N - Relationship organization_status_maps
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_status_maps
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
                /// ReferencingEntity statusmap:
                ///     DisplayName:
                ///     (English - United States - 1033): Status Map
                ///     (Russian - 1049): Сопоставление состояния
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Status Maps
                ///     (Russian - 1049): Сопоставления состояний
                ///     
                ///     Description:
                ///     (English - United States - 1033): Mapping between statuses.
                ///     (Russian - 1049): Сопоставление состояний.
                ///</summary>
                public static partial class organization_status_maps
                {
                    public const string Name = "organization_status_maps";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_statusmap = "statusmap";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_string_maps
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_string_maps
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
                /// ReferencingEntity stringmap:
                ///     DisplayName:
                ///     (English - United States - 1033): String Map
                ///     (Russian - 1049): Сопоставление строки
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): String Maps
                ///     (Russian - 1049): Сопоставления строк
                ///     
                ///     Description:
                ///     (English - United States - 1033): Mapping between strings.
                ///     (Russian - 1049): Сопоставление строк.
                ///</summary>
                public static partial class organization_string_maps
                {
                    public const string Name = "organization_string_maps";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_stringmap = "stringmap";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_subjects
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_subjects
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
                /// ReferencingEntity subject:
                ///     DisplayName:
                ///     (English - United States - 1033): Subject
                ///     (Russian - 1049): Тема
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Subjects
                ///     (Russian - 1049): Темы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Information regarding subjects available in the system.
                ///     (Russian - 1049): Сведения об объектах, доступных в системе.
                ///</summary>
                public static partial class organization_subjects
                {
                    public const string Name = "organization_subjects";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_subject = "subject";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship Organization_SyncErrors
                /// 
                /// PropertyName                               Value                                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Organization_SyncErrors
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_organization_syncerror
                /// IsCustomizable                             True                                        False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                NoCascade
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
                public static partial class organization_syncerrors
                {
                    public const string Name = "Organization_SyncErrors";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_syncerror = "syncerror";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_system_users
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_system_users
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
                /// ReferencingEntity systemuser:
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
                public static partial class organization_system_users
                {
                    public const string Name = "organization_system_users";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_systemuser = "systemuser";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// 1:N - Relationship organization_systemapplicationmetadata
                /// 
                /// PropertyName                               Value                                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_systemapplicationmetadata
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                     False
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
                /// ReferencingEntity systemapplicationmetadata:
                ///     DisplayName:
                ///     (English - United States - 1033): System Application Metadata
                ///     (Russian - 1049): Метаданные системного приложения
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): System Application Metadata Collection
                ///     (Russian - 1049): Сбор метаданных системного приложения
                ///     
                ///     Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                ///</summary>
                public static partial class organization_systemapplicationmetadata
                {
                    public const string Name = "organization_systemapplicationmetadata";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_systemapplicationmetadata = "systemapplicationmetadata";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_systemforms
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_systemforms
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
                /// ReferencingEntity systemform:
                ///     DisplayName:
                ///     (English - United States - 1033): System Form
                ///     (Russian - 1049): Системная форма
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): System Forms
                ///     (Russian - 1049): Системные формы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Organization-owned entity customizations including form layout and dashboards.
                ///     (Russian - 1049): Настройки сущности, принадлежащие организации, включающая макет формы и панели мониторинга.
                ///</summary>
                public static partial class organization_systemforms
                {
                    public const string Name = "organization_systemforms";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_systemform = "systemform";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_teams
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_teams
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
                /// ReferencingEntity team:
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
                public static partial class organization_teams
                {
                    public const string Name = "organization_teams";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_territories
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_territories
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
                /// ReferencingEntity territory:
                ///     DisplayName:
                ///     (English - United States - 1033): Territory
                ///     (Russian - 1049): Территория
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Territories
                ///     (Russian - 1049): Территории
                ///     
                ///     Description:
                ///     (English - United States - 1033): Territory represents sales regions.
                ///     (Russian - 1049): Территория представляет регионы продаж.
                ///</summary>
                public static partial class organization_territories
                {
                    public const string Name = "organization_territories";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_territory = "territory";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_textanalyticsentitymapping
                /// 
                /// PropertyName                               Value                                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_textanalyticsentitymapping
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity textanalyticsentitymapping:
                ///     DisplayName:
                ///     (English - United States - 1033): Text Analytics Entity Mapping
                ///     (Russian - 1049): Сопоставления сущности текстовой аналитики
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Text Analytics Entity Mappings
                ///     (Russian - 1049): Сопоставления сущностей текстовой аналитики
                ///</summary>
                public static partial class organization_textanalyticsentitymapping
                {
                    public const string Name = "organization_textanalyticsentitymapping";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_textanalyticsentitymapping = "textanalyticsentitymapping";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_theme
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_theme
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity theme:
                ///     DisplayName:
                ///     (English - United States - 1033): Theme
                ///     (Russian - 1049): Тема
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Themes
                ///     (Russian - 1049): Темы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Information that's used to set custom visual theme options for client applications.
                ///     (Russian - 1049): Сведения, используемые для задания параметров пользовательской визуальной темы для клиентских приложений.
                ///</summary>
                public static partial class organization_theme
                {
                    public const string Name = "organization_theme";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_theme = "theme";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_topicmodel
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_topicmodel
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity topicmodel:
                ///     DisplayName:
                ///     (English - United States - 1033): Topic Model
                ///     (Russian - 1049): Модель темы
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): The model for automatic identification of topics using text analytics.
                ///     (Russian - 1049): Модель для автоматической идентификации тем с помощью текстовой аналитики.
                ///</summary>
                public static partial class organization_topicmodel
                {
                    public const string Name = "organization_topicmodel";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_topicmodel = "topicmodel";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_topicmodelconfiguration
                /// 
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_topicmodelconfiguration
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                                   False
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
                /// ReferencingEntity topicmodelconfiguration:
                ///     DisplayName:
                ///     (English - United States - 1033): Topic Model Configuration
                ///     (Russian - 1049): Конфигурация модели темы
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Topic Model Configurations
                ///     (Russian - 1049): Конфигурации моделей темы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Configuration settings for identification of topics using text analytics.
                ///     (Russian - 1049): Параметры конфигурации для идентификации тем с помощью текстовой аналитики.
                ///</summary>
                public static partial class organization_topicmodelconfiguration
                {
                    public const string Name = "organization_topicmodelconfiguration";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_topicmodelconfiguration = "topicmodelconfiguration";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_topicmodelexecutionhistory
                /// 
                /// PropertyName                               Value                                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_topicmodelexecutionhistory
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity topicmodelexecutionhistory:
                ///     DisplayName:
                ///     (English - United States - 1033): Topic Model Execution History
                ///     (Russian - 1049): Журнал выполнения модели темы
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Topic Model Execution Histories
                ///     (Russian - 1049): Журналы выполнения моделей тем
                ///     
                ///     Description:
                ///     (English - United States - 1033): Entity for Topic Model Execution History
                ///     (Russian - 1049): Сущность для журнала выполнения модели темы
                ///</summary>
                public static partial class organization_topicmodelexecutionhistory
                {
                    public const string Name = "organization_topicmodelexecutionhistory";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_topicmodelexecutionhistory = "topicmodelexecutionhistory";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_traceassociation
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_traceassociation
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                            False
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
                /// ReferencingEntity traceassociation:
                ///     DisplayName:
                ///     (English - United States - 1033): Trace Association
                ///     (Russian - 1049): Ассоциация трассировки
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Trace Associations
                ///     (Russian - 1049): Ассоциации трассировки
                ///     
                ///     Description:
                ///     (English - United States - 1033): Represents the objects with which a trace record is associated. For internal use only.
                ///     (Russian - 1049): Представляет объекты, с которыми связана запись трассировки. Только для внутреннего использования.
                ///</summary>
                public static partial class organization_traceassociation
                {
                    public const string Name = "organization_traceassociation";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_traceassociation = "traceassociation";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_tracelog
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_tracelog
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
                /// ReferencingEntity tracelog:
                ///     DisplayName:
                ///     (English - United States - 1033): Trace
                ///     (Russian - 1049): Трассировка
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Traces
                ///     (Russian - 1049): Трассировки
                ///     
                ///     Description:
                ///     (English - United States - 1033): A trace log.
                ///     (Russian - 1049): Журнал трассировки.
                ///</summary>
                public static partial class organization_tracelog
                {
                    public const string Name = "organization_tracelog";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_tracelog = "tracelog";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_text = "text";
                }

                ///<summary>
                /// 1:N - Relationship organization_transactioncurrencies
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_transactioncurrencies
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity transactioncurrency:
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
                public static partial class organization_transactioncurrencies
                {
                    public const string Name = "organization_transactioncurrencies";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_transactioncurrency = "transactioncurrency";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_currencyname = "currencyname";
                }

                ///<summary>
                /// 1:N - Relationship organization_translationprocess
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_translationprocess
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                               False
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
                /// ReferencingEntity translationprocess:
                ///     DisplayName:
                ///     (English - United States - 1033): Translation Process
                ///     (Russian - 1049): Процесс перевода
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Translation Process Business Process Flow
                ///     (Russian - 1049): Процесс перевода — последовательность операций бизнес-процесса
                ///</summary>
                public static partial class organization_translationprocess
                {
                    public const string Name = "organization_translationprocess";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_translationprocess = "translationprocess";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_uof_schedules
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_uof_schedules
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity uomschedule:
                ///     DisplayName:
                ///     (English - United States - 1033): Unit Group
                ///     (Russian - 1049): Группа единиц измерения
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Unit Groups
                ///     (Russian - 1049): Группы единиц измерения
                ///     
                ///     Description:
                ///     (English - United States - 1033): Grouping of units.
                ///     (Russian - 1049): Группа единиц измерения.
                ///</summary>
                public static partial class organization_uof_schedules
                {
                    public const string Name = "organization_uof_schedules";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_uomschedule = "uomschedule";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_UserMapping
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_UserMapping
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                        False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity usermapping:
                ///     DisplayName:
                ///     (English - United States - 1033): User Mapping
                ///     (Russian - 1049): Сопоставление пользователей
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): User Mappings
                ///     (Russian - 1049): Сопоставления пользователей
                ///     
                ///     Description:
                ///</summary>
                public static partial class organization_usermapping
                {
                    public const string Name = "organization_UserMapping";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_usermapping = "usermapping";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// 1:N - Relationship organization_vw_brand
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_vw_brand
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                     True
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
                /// ReferencingEntity vw_brand:
                ///     DisplayName:
                ///     (English - United States - 1033): Brand
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Brands
                ///</summary>
                public static partial class organization_vw_brand
                {
                    public const string Name = "organization_vw_brand";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_vw_brand = "vw_brand";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_vw_name = "vw_name";
                }

                ///<summary>
                /// 1:N - Relationship organization_vw_caseprocess
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_vw_caseprocess
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                           True
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
                /// ReferencingEntity vw_caseprocess:
                ///     DisplayName:
                ///     (English - United States - 1033): VW Case Process
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Base entity for process VW Case Process
                ///</summary>
                public static partial class organization_vw_caseprocess
                {
                    public const string Name = "organization_vw_caseprocess";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_vw_caseprocess = "vw_caseprocess";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_bpf_name = "bpf_name";
                }

                ///<summary>
                /// 1:N - Relationship organization_vw_city
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_vw_city
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                     True
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
                /// ReferencingEntity vw_city:
                ///     DisplayName:
                ///     (English - United States - 1033): City
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Cities
                ///</summary>
                public static partial class organization_vw_city
                {
                    public const string Name = "organization_vw_city";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_vw_city = "vw_city";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_vw_name = "vw_name";
                }

                ///<summary>
                /// 1:N - Relationship organization_vw_dealer
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_vw_dealer
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                      True
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
                /// ReferencingEntity vw_dealer:
                ///     DisplayName:
                ///     (English - United States - 1033): Dealer
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Dealers
                ///</summary>
                public static partial class organization_vw_dealer
                {
                    public const string Name = "organization_vw_dealer";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_vw_dealer = "vw_dealer";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_vw_name = "vw_name";
                }

                ///<summary>
                /// 1:N - Relationship organization_vw_model
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_vw_model
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                     True
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
                /// ReferencingEntity vw_model:
                ///     DisplayName:
                ///     (English - United States - 1033): Model
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Models
                ///</summary>
                public static partial class organization_vw_model
                {
                    public const string Name = "organization_vw_model";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_vw_model = "vw_model";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_vw_name = "vw_name";
                }

                ///<summary>
                /// 1:N - Relationship organization_vw_vwleadprocess
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_vw_vwleadprocess
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             True                             True
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
                /// ReferencingEntity vw_vwleadprocess:
                ///     DisplayName:
                ///     (English - United States - 1033): VW Lead Process
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Base entity for process VW Lead Process
                ///</summary>
                public static partial class organization_vw_vwleadprocess
                {
                    public const string Name = "organization_vw_vwleadprocess";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_vw_vwleadprocess = "vw_vwleadprocess";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_bpf_name = "bpf_name";
                }

                ///<summary>
                /// 1:N - Relationship organization_webwizard
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_webwizard
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                     False
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
                /// ReferencingEntity webwizard:
                ///     DisplayName:
                ///     (English - United States - 1033): Web Wizard
                ///     (Russian - 1049): Веб-мастер
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Web Wizards
                ///     (Russian - 1049): Веб-мастера
                ///     
                ///     Description:
                ///     (English - United States - 1033): Definition for a Web-based wizard.
                ///     (Russian - 1049): Определение мастера веб-страниц.
                ///</summary>
                public static partial class organization_webwizard
                {
                    public const string Name = "organization_webwizard";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_webwizard = "webwizard";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship organization_wizardaccessprivilege
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_wizardaccessprivilege
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity wizardaccessprivilege:
                ///     DisplayName:
                ///     (English - United States - 1033): Web Wizard Access Privilege
                ///     (Russian - 1049): Право доступа в мастере веб-страниц
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Web Wizard Access Privileges
                ///     (Russian - 1049): Права доступа в мастере веб-страниц
                ///     
                ///     Description:
                ///     (English - United States - 1033): Privilege needed to access a Web-based wizard.
                ///     (Russian - 1049): Права, необходимые для доступа к мастеру веб-страниц.
                ///</summary>
                public static partial class organization_wizardaccessprivilege
                {
                    public const string Name = "organization_wizardaccessprivilege";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_wizardaccessprivilege = "wizardaccessprivilege";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_entityname = "entityname";
                }

                ///<summary>
                /// 1:N - Relationship organization_wizardpage
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_wizardpage
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity wizardpage:
                ///     DisplayName:
                ///     (English - United States - 1033): Wizard Page
                ///     (Russian - 1049): Страница мастера
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Wizard Pages
                ///     (Russian - 1049): Страницы мастера
                ///     
                ///     Description:
                ///     (English - United States - 1033): Page in a Web-based wizard.
                ///     (Russian - 1049): Страница мастера веб-страниц.
                ///</summary>
                public static partial class organization_wizardpage
                {
                    public const string Name = "organization_wizardpage";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_wizardpage = "wizardpage";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_pagesequencenumber = "pagesequencenumber";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_organization
                /// 
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_organization
                /// ReferencingEntityNavigationPropertyName    objectid_organization
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
                public static partial class userentityinstancedata_organization
                {
                    public const string Name = "userentityinstancedata_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }

                ///<summary>
                /// 1:N - Relationship webresource_organization
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
                /// ReferencingEntity webresource:
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
                public static partial class webresource_organization
                {
                    public const string Name = "webresource_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_webresource = "webresource";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}
