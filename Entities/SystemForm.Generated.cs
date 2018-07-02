
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): System Form
    /// (Russian - 1049): Системная форма
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): System Forms
    /// (Russian - 1049): Системные формы
    /// 
    /// Description:
    /// (English - United States - 1033): Organization-owned entity customizations including form layout and dashboards.
    /// (Russian - 1049): Настройки сущности, принадлежащие организации, включающая макет формы и панели мониторинга.
    /// 
    /// PropertyName                          Value                 CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False                 False
    /// CanBePrimaryEntityInRelationship      False                 False
    /// CanBeRelatedEntityInRelationship      False                 False
    /// CanChangeHierarchicalRelationship     False                 False
    /// CanChangeTrackingBeEnabled            True                  True
    /// CanCreateAttributes                   False                 False
    /// CanCreateCharts                       False                 False
    /// CanCreateForms                        False                 False
    /// CanCreateViews                        False                 False
    /// CanEnableSyncToExternalSearchIndex    False                 False
    /// CanModifyAdditionalSettings           False                 True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 True
    /// CollectionSchemaName                  SystemForms
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         systemforms
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False                 False
    /// IsAvailableOffline                    True
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False                 False
    /// IsCustomEntity                        False
    /// IsCustomizable                        False                 False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                 False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                 False
    /// IsMappable                            False                 False
    /// IsOfflineInMobileClient               False                 True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False                 False
    /// IsRenameable                          False                 False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False                 False
    /// IsVisibleInMobile                     False                 False
    /// IsVisibleInMobileClient               False                 False
    /// LogicalCollectionName                 systemforms
    /// LogicalName                           systemform
    /// ObjectTypeCode                        1030
    /// OwnershipType                         OrganizationOwned
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredSystemForm
    /// SchemaName                            SystemForm
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class SystemForm
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "systemform";

            public const string EntitySchemaName = "SystemForm";

            public const string EntityPrimaryNameAttribute = "name";

            public const string EntityPrimaryIdAttribute = "formid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Parent Form
                ///     (Russian - 1049): Родительская форма
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the parent form.
                ///     (Russian - 1049): Уникальный идентификатор родительской формы.
                /// 
                /// SchemaName: AncestorFormId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemform
                /// 
                ///     Target systemform    PrimaryIdAttribute formid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): System Form
                ///         (Russian - 1049): Системная форма
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): System Forms
                ///         (Russian - 1049): Системные формы
                ///         
                ///         Description:
                ///         (English - United States - 1033): Organization-owned entity customizations including form layout and dashboards.
                ///         (Russian - 1049): Настройки сущности, принадлежащие организации, включающая макет формы и панели мониторинга.
                ///</summary>
                public const string ancestorformid = "ancestorformid";

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
                ///</summary>
                public const string componentstate = "componentstate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Description
                ///     (Russian - 1049): Описание
                /// 
                /// Description:
                ///     (English - United States - 1033): Description of the form or dashboard.
                ///     (Russian - 1049): Описание формы или панели мониторинга.
                /// 
                /// SchemaName: Description
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 2000
                /// Format = Text    ImeMode = Auto    IsLocalizable = True
                ///</summary>
                public const string description = "description";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Form State
                ///     (Russian - 1049): Состояние формы
                /// 
                /// Description:
                ///     (English - United States - 1033): Specifies the state of the form.
                ///     (Russian - 1049): Указывает состояние формы.
                /// 
                /// SchemaName: FormActivationState
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet systemform_formactivationstate
                /// DefaultFormValue = 1
                ///</summary>
                public const string formactivationstate = "formactivationstate";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the record type form.
                ///     (Russian - 1049): Уникальный идентификатор формы типа записи.
                /// 
                /// SchemaName: FormId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string formid = "formid";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the form used when synchronizing customizations for the Microsoft Dynamics 365 client for Outlook.
                ///     (Russian - 1049): Уникальный идентификатор формы, используемой при синхронизации настроек для клиента Microsoft Dynamics 365 для Outlook.
                /// 
                /// SchemaName: FormIdUnique
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string formidunique = "formidunique";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): AIR Refreshed
                ///     (Russian - 1049): AIR обновлен
                /// 
                /// Description:
                ///     (English - United States - 1033): Specifies whether this form is in the updated UI layout in Microsoft Dynamics CRM 2015 or Microsoft Dynamics CRM Online 2015 Update.
                ///     (Russian - 1049): Указывает, входит ли эта форма в обновленный макет пользовательского интерфейса в Microsoft Dynamics CRM 2015 или в обновлении Microsoft Dynamics CRM Online 2015.
                /// 
                /// SchemaName: FormPresentation
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet systemform_formpresentation
                /// DefaultFormValue = 0
                ///</summary>
                public const string formpresentation = "formpresentation";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): XML representation of the form layout.
                ///     (Russian - 1049): XML-представление макета формы.
                /// 
                /// SchemaName: FormXml
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string formxml = "formxml";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): formXml diff as in a managed solution. for internal use only
                ///     (Russian - 1049): Различие Xml формы как в управляемом решении. Только для внутреннего использования
                /// 
                /// SchemaName: FormXmlManaged
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: False    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string formxmlmanaged = "formxmlmanaged";

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
                ///     (English - United States - 1033): Refreshed
                ///     (Russian - 1049): Обновлен
                /// 
                /// Description:
                ///     (English - United States - 1033): Specifies whether this form is merged with the updated UI layout in Microsoft Dynamics CRM 2015 or Microsoft Dynamics CRM Online 2015 Update.
                ///     (Russian - 1049): Указывает, объединена ли эта форма с обновленным макетом пользовательского интерфейса в Microsoft Dynamics CRM 2015, либо в обновлении Microsoft Dynamics CRM Online 2015.
                /// 
                /// SchemaName: IsAIRMerged
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
                public const string isairmerged = "isairmerged";

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
                ///     (English - United States - 1033): Default Form
                ///     (Russian - 1049): Форма по умолчанию
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether the form or the dashboard is the system default.
                ///     (Russian - 1049): Сведения о том, является ли форма или панель мониторинга системной по умолчанию.
                /// 
                /// SchemaName: IsDefault
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
                public const string isdefault = "isdefault";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Desktop Enabled
                ///     (Russian - 1049): Включен ли настольный вариант
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether the dashboard is enabled for desktop.
                ///     (Russian - 1049): Сведения, которые определяют, включена ли панель мониторинга для настольного компьютера.
                /// 
                /// SchemaName: IsDesktopEnabled
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
                public const string isdesktopenabled = "isdesktopenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): State
                ///     (Russian - 1049): Область
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
                ///     (English - United States - 1033): Is Tablet Enabled
                ///     (Russian - 1049): Включено для планшетного компьютера
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether the dashboard is enabled for tablet.
                ///     (Russian - 1049): Сведения, которые определяют, включена ли панель мониторинга для планшетного компьютера.
                /// 
                /// SchemaName: IsTabletEnabled
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
                public const string istabletenabled = "istabletenabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Name
                ///     (Russian - 1049): Имя
                /// 
                /// Description:
                ///     (English - United States - 1033): Name of the form.
                ///     (Russian - 1049): Название формы.
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
                /// Description:
                ///     (English - United States - 1033): Code that represents the record type.
                ///     (Russian - 1049): Код, обозначающий тип записей.
                /// 
                /// SchemaName: ObjectTypeCode
                /// EntityNameAttributeMetadata    AttributeType: EntityName    AttributeTypeName: EntityNameType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Local System  OptionSet systemform_objecttypecode
                /// DefaultFormValue = -1
                ///</summary>
                public const string objecttypecode = "objecttypecode";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the organization.
                ///     (Russian - 1049): Уникальный идентификатор организации.
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
                ///     (English - United States - 1033): Form Type
                ///     (Russian - 1049): Тип формы
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of the form, for example, Dashboard or Preview.
                ///     (Russian - 1049): Тип формы, например "Панель мониторинга" или "Просмотр".
                /// 
                /// SchemaName: Type
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet systemform_type
                /// DefaultFormValue = -1
                ///</summary>
                public const string type = "type";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Unique Name
                ///     (Russian - 1049): Уникальное имя
                /// 
                /// Description:
                /// 
                /// SchemaName: UniqueName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string uniquename = "uniquename";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: Version
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string version = "version";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Represents a version of customizations to be synchronized with the Microsoft Dynamics 365 client for Outlook.
                ///     (Russian - 1049): Указывает версию из настроек для синхронизации с клиентом Microsoft Dynamics 365 для Outlook.
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

            #region OptionSets.

            public static partial class OptionSets
            {

                #region Picklist OptionSet OptionSets.
                ///<summary>
                /// Attribute: formactivationstate
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Form State
                ///     (Russian - 1049): Состояние формы
                /// 
                /// Description:
                ///     (English - United States - 1033): Specifies the state of the form.
                ///     (Russian - 1049): Указывает состояние формы.
                /// 
                /// Local System  OptionSet systemform_formactivationstate
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Form State
                ///     (Russian - 1049): Состояние формы
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates the form state that is Active\Inactive.
                ///     (Russian - 1049): Указывает состояние формы: активная/неактивная.
                ///</summary>
                public enum formactivationstate
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Inactive
                    ///     (Russian - 1049): Неактивный
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Inactive_0 = 0,

                    ///<summary>
                    /// 1
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
                /// Attribute: formpresentation
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): AIR Refreshed
                ///     (Russian - 1049): AIR обновлен
                /// 
                /// Description:
                ///     (English - United States - 1033): Specifies whether this form is in the updated UI layout in Microsoft Dynamics CRM 2015 or Microsoft Dynamics CRM Online 2015 Update.
                ///     (Russian - 1049): Указывает, входит ли эта форма в обновленный макет пользовательского интерфейса в Microsoft Dynamics CRM 2015 или в обновлении Microsoft Dynamics CRM Online 2015.
                /// 
                /// Local System  OptionSet systemform_formpresentation
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Refreshed Layout
                ///     (Russian - 1049): Макет обновлен
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates the form layout is refreshed.
                ///     (Russian - 1049): Указывает, обновлен ли макет формы.
                ///</summary>
                public enum formpresentation
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): ClassicForm
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    ClassicForm_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): AirForm
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    AirForm_1 = 1,
                }

                ///<summary>
                /// Attribute: type
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Form Type
                ///     (Russian - 1049): Тип формы
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of the form, for example, Dashboard or Preview.
                ///     (Russian - 1049): Тип формы, например "Панель мониторинга" или "Просмотр".
                /// 
                /// Local System  OptionSet systemform_type
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Type
                ///     (Russian - 1049): Тип
                /// 
                /// Description:
                ///     (English - United States - 1033): Identifies the form type.
                ///     (Russian - 1049): Определяет тип формы.
                ///</summary>
                public enum type
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Dashboard
                    ///     (Russian - 1049): Панель мониторинга
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Dashboard_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): AppointmentBook
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    AppointmentBook_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Main
                    ///     (Russian - 1049): Основная
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Main_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): MiniCampaignBO
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    MiniCampaignBO_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 5
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Preview
                    ///     (Russian - 1049): Предварительный просмотр
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Preview_4 = 4,

                    ///<summary>
                    /// 5
                    /// DisplayOrder: 6
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Mobile - Express
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Mobile_Express_5 = 5,

                    ///<summary>
                    /// 6
                    /// DisplayOrder: 7
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Quick View Form
                    ///     (Russian - 1049): Экспресс-форма
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Quick_View_Form_6 = 6,

                    ///<summary>
                    /// 7
                    /// DisplayOrder: 8
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Quick Create
                    ///     (Russian - 1049): Быстрое создание
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Quick_Create_7 = 7,

                    ///<summary>
                    /// 8
                    /// DisplayOrder: 9
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Dialog
                    ///     (Russian - 1049): Диалоговое окно
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Dialog_8 = 8,

                    ///<summary>
                    /// 9
                    /// DisplayOrder: 10
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Task Flow Form
                    ///     (Russian - 1049): Форма, основанная на задаче
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Task_Flow_Form_9 = 9,

                    ///<summary>
                    /// 10
                    /// DisplayOrder: 11
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): InteractionCentricDashboard
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    InteractionCentricDashboard_10 = 10,

                    ///<summary>
                    /// 11
                    /// DisplayOrder: 12
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Card
                    ///     (Russian - 1049): Карточка
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Card_11 = 11,

                    ///<summary>
                    /// 12
                    /// DisplayOrder: 13
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Main - Interactive experience
                    ///     (Russian - 1049): Основная — интерактивное взаимодействие
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Main_Interactive_experience_12 = 12,

                    ///<summary>
                    /// 100
                    /// DisplayOrder: 14
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Other
                    ///     (Russian - 1049): Прочее
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Other_100 = 100,

                    ///<summary>
                    /// 101
                    /// DisplayOrder: 15
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): MainBackup
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    MainBackup_101 = 101,

                    ///<summary>
                    /// 102
                    /// DisplayOrder: 16
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): AppointmentBookBackup
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    AppointmentBookBackup_102 = 102,

                    ///<summary>
                    /// 103
                    /// DisplayOrder: 17
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Power BI Dashboard
                    ///     (Russian - 1049): Панель мониторинга Power BI
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Power_BI_Dashboard_103 = 103,
                }

                #endregion Picklist OptionSets.
            }

            #endregion OptionSets.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship form_ancestor_form
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     form_ancestor_form
                /// ReferencingEntityNavigationPropertyName    ancestorformid
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
                ///</summary>
                public static partial class form_ancestor_form
                {
                    public const string Name = "form_ancestor_form";

                    public const string ReferencedEntity_systemform = "systemform";

                    public const string ReferencedAttribute_formid = "formid";

                    public const string ReferencingEntity_systemform = "systemform";

                    public const string ReferencingAttribute_ancestorformid = "ancestorformid";
                }

                ///<summary>
                /// N:1 - Relationship organization_systemforms
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
                public static partial class organization_systemforms
                {
                    public const string Name = "organization_systemforms";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_systemform = "systemform";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship form_ancestor_form
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     form_ancestor_form
                /// ReferencingEntityNavigationPropertyName    ancestorformid
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
                ///</summary>
                public static partial class form_ancestor_form
                {
                    public const string Name = "form_ancestor_form";

                    public const string ReferencedEntity_systemform = "systemform";

                    public const string ReferencedAttribute_formid = "formid";

                    public const string ReferencingEntity_systemform = "systemform";

                    public const string ReferencingAttribute_ancestorformid = "ancestorformid";
                }

                ///<summary>
                /// 1:N - Relationship processtrigger_systemform
                /// 
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     processtrigger_systemform
                /// ReferencingEntityNavigationPropertyName    formid
                /// IsCustomizable                             False                        False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
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
                /// ReferencingEntity processtrigger:
                ///     DisplayName:
                ///     (English - United States - 1033): Process Trigger
                ///     (Russian - 1049): Триггер процесса
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Process Triggers
                ///     (Russian - 1049): Триггеры процесса
                ///     
                ///     Description:
                ///     (English - United States - 1033): Trigger that invoke a rule.
                ///     (Russian - 1049): Триггер, вызывающий срабатывание правила.
                ///</summary>
                public static partial class processtrigger_systemform
                {
                    public const string Name = "processtrigger_systemform";

                    public const string ReferencedEntity_systemform = "systemform";

                    public const string ReferencedAttribute_formid = "formid";

                    public const string ReferencingEntity_processtrigger = "processtrigger";

                    public const string ReferencingAttribute_formid = "formid";
                }

                ///<summary>
                /// 1:N - Relationship socialinsightsconfiguration_systemform
                /// 
                /// PropertyName                               Value                                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     socialinsightsconfiguration_systemform
                /// ReferencingEntityNavigationPropertyName    formid_systemform
                /// IsCustomizable                             False                                     False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
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
                public static partial class socialinsightsconfiguration_systemform
                {
                    public const string Name = "socialinsightsconfiguration_systemform";

                    public const string ReferencedEntity_systemform = "systemform";

                    public const string ReferencedAttribute_formid = "formid";

                    public const string ReferencingEntity_socialinsightsconfiguration = "socialinsightsconfiguration";

                    public const string ReferencingAttribute_formid = "formid";
                }

                ///<summary>
                /// 1:N - Relationship SystemForm_AsyncOperations
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     SystemForm_AsyncOperations
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_systemform
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
                public static partial class systemform_asyncoperations
                {
                    public const string Name = "SystemForm_AsyncOperations";

                    public const string ReferencedEntity_systemform = "systemform";

                    public const string ReferencedAttribute_formid = "formid";

                    public const string ReferencingEntity_asyncoperation = "asyncoperation";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship SystemForm_BulkDeleteFailures
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     SystemForm_BulkDeleteFailures
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_systemform
                /// IsCustomizable                             False                            False
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
                public static partial class systemform_bulkdeletefailures
                {
                    public const string Name = "SystemForm_BulkDeleteFailures";

                    public const string ReferencedEntity_systemform = "systemform";

                    public const string ReferencedAttribute_formid = "formid";

                    public const string ReferencingEntity_bulkdeletefailure = "bulkdeletefailure";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}
