
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): Process
    /// (Russian - 1049): Процесс
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): Processes
    /// (Russian - 1049): Процессы
    /// 
    /// Description:
    /// (English - United States - 1033): Set of logical rules that define the steps necessary to automate a specific business process, task, or set of actions to be performed.
    /// (Russian - 1049): Задайте логические правила, определяющие необходимые действия для автоматизации конкретных бизнес-процессов, заданий или наборов действий.
    /// 
    /// PropertyName                          Value               CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False               False
    /// CanBePrimaryEntityInRelationship      True                False
    /// CanBeRelatedEntityInRelationship      False               False
    /// CanChangeHierarchicalRelationship     False               False
    /// CanChangeTrackingBeEnabled            True                True
    /// CanCreateAttributes                   False               False
    /// CanCreateCharts                       False               False
    /// CanCreateForms                        False               False
    /// CanCreateViews                        True                False
    /// CanEnableSyncToExternalSearchIndex    False               False
    /// CanModifyAdditionalSettings           True                True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 True
    /// CollectionSchemaName                  Workflows
    /// DaysSinceRecordLastModified           9999
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         workflows
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False               True
    /// IsAvailableOffline                    True
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False               False
    /// IsCustomEntity                        False
    /// IsCustomizable                        True                False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False               False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False               False
    /// IsMappable                            False               False
    /// IsOfflineInMobileClient               False               True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False               False
    /// IsRenameable                          False               False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                True
    /// IsValidForQueue                       False               False
    /// IsVisibleInMobile                     False               False
    /// IsVisibleInMobileClient               False               False
    /// LogicalCollectionName                 workflows
    /// LogicalName                           workflow
    /// ObjectTypeCode                        4703
    /// OwnershipType                         UserOwned
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredWorkflow
    /// SchemaName                            Workflow
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class Workflow
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "workflow";

            public const string EntitySchemaName = "Workflow";

            public const string EntityPrimaryNameAttribute = "name";

            public const string EntityPrimaryIdAttribute = "workflowid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Active Process ID
                ///     (Russian - 1049): Идентификатор активного процесса
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the latest activation record for the process.
                ///     (Russian - 1049): Уникальный идентификатор последней записи активации для процесса.
                /// 
                /// SchemaName: ActiveWorkflowId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
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
                public const string activeworkflowid = "activeworkflowid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Delete Job On Completion
                ///     (Russian - 1049): Удалить задание по выполнении
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the asynchronous system job is automatically deleted on completion.
                ///     (Russian - 1049): Указывает, будет ли асинхронное системное задание автоматически удаляться по выполнении.
                /// 
                /// SchemaName: AsyncAutoDelete
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
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
                public const string asyncautodelete = "asyncautodelete";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Business Process Type
                ///     (Russian - 1049): Тип бизнес-процесса
                /// 
                /// Description:
                ///     (English - United States - 1033): Business Process Type.
                ///     (Russian - 1049): Тип бизнес-процесса.
                /// 
                /// SchemaName: BusinessProcessType
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet workflow_businessprocesstype
                /// DefaultFormValue = 0
                ///</summary>
                public const string businessprocesstype = "businessprocesstype";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Category
                ///     (Russian - 1049): Категория
                /// 
                /// Description:
                ///     (English - United States - 1033): Category of the process.
                ///     (Russian - 1049): Категория процесса.
                /// 
                /// SchemaName: Category
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet workflow_category
                /// DefaultFormValue = 0
                ///</summary>
                public const string category = "category";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Client Data
                ///     (Russian - 1049): Данные клиента
                /// 
                /// Description:
                ///     (English - United States - 1033): Business logic converted into client data
                ///     (Russian - 1049): Бизнес-логика, преобразованная в данные клиента
                /// 
                /// SchemaName: ClientData
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string clientdata = "clientdata";

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
                ///     (English - United States - 1033): Unique identifier of the user who created the process.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, создавшего процесс.
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
                ///     (English - United States - 1033): Date and time when the process was created.
                ///     (Russian - 1049): Дата и время создания процесса.
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who created the process.
                ///     (Russian - 1049): Уникальный идентификатор делегата, создавшего бизнес-процесс.
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
                ///     (English - United States - 1033): Create Stage
                ///     (Russian - 1049): Создать стадию
                /// 
                /// Description:
                ///     (English - United States - 1033): Stage of the process when triggered on Create.
                ///     (Russian - 1049): Стадия процесса во время запуска при создании.
                /// 
                /// SchemaName: CreateStage
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Global System  OptionSet workflow_stage
                /// DefaultFormValue = -1
                ///</summary>
                public const string createstage = "createstage";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Delete stage
                ///     (Russian - 1049): Удалить стадию
                /// 
                /// Description:
                ///     (English - United States - 1033): Stage of the process when triggered on Delete.
                ///     (Russian - 1049): Стадия процесса во время запуска при удалении.
                /// 
                /// SchemaName: DeleteStage
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Global System  OptionSet workflow_stage
                /// DefaultFormValue = -1
                ///</summary>
                public const string deletestage = "deletestage";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Description
                ///     (Russian - 1049): Описание
                /// 
                /// Description:
                ///     (English - United States - 1033): Description of the process.
                ///     (Russian - 1049): Описание процесса.
                /// 
                /// SchemaName: Description
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 100000
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = True
                ///</summary>
                public const string description = "description";

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
                ///     (English - United States - 1033): Form ID
                ///     (Russian - 1049): Идентификатор формы
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the associated form.
                ///     (Russian - 1049): Уникальный идентификатор связанной формы.
                /// 
                /// SchemaName: FormId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string formid = "formid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Input Parameters
                ///     (Russian - 1049): Входные параметры
                /// 
                /// Description:
                ///     (English - United States - 1033): Input parameters to the process.
                ///     (Russian - 1049): Входные параметры для бизнес-процесса.
                /// 
                /// SchemaName: InputParameters
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string inputparameters = "inputparameters";

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
                ///     (English - United States - 1033): Is CRM Process
                ///     (Russian - 1049): Является бизнес-процессом CRM
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the process was created using the Microsoft Dynamics 365 Web application.
                ///     (Russian - 1049): Указывает, был ли создан процесс в веб-приложении Microsoft Dynamics 365.
                /// 
                /// SchemaName: IsCrmUIWorkflow
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
                public const string iscrmuiworkflow = "iscrmuiworkflow";

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
                ///     (English - United States - 1033): Is Managed
                ///     (Russian - 1049): Управляемый
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the solution component is part of a managed solution.
                ///     (Russian - 1049): Указывает, является ли компонент решения частью управляемого решения.
                /// 
                /// SchemaName: IsManaged
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
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
                ///     (English - United States - 1033): Is Transacted
                ///     (Russian - 1049): Производится транзакция
                /// 
                /// Description:
                ///     (English - United States - 1033): Whether or not the steps in the process are executed in a single transaction.
                ///     (Russian - 1049): Выполняются ли шаги процесса в одной транзакции.
                /// 
                /// SchemaName: IsTransacted
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
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
                public const string istransacted = "istransacted";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Language
                ///     (Russian - 1049): Язык
                /// 
                /// Description:
                ///     (English - United States - 1033): Language of the process.
                ///     (Russian - 1049): Язык бизнес-процесса.
                /// 
                /// SchemaName: LanguageCode
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = Language
                ///</summary>
                public const string languagecode = "languagecode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Mode
                ///     (Russian - 1049): Режим
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the mode of the process.
                ///     (Russian - 1049): Показывает режим процесса.
                /// 
                /// SchemaName: Mode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet workflow_mode
                /// DefaultFormValue = 0
                ///</summary>
                public const string mode = "mode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By
                ///     (Russian - 1049): Изменено
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who last modified the process.
                ///     (Russian - 1049): Уникальный идентификатор последнего пользователя, изменившего процесс.
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
                ///     (English - United States - 1033): Date and time when the process was last modified.
                ///     (Russian - 1049): Дата и время последнего изменения процесса.
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who last modified the process.
                ///     (Russian - 1049): Уникальный идентификатор делегата, внесшего последнее изменение в бизнес-процесс.
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
                ///     (English - United States - 1033): Process Name
                ///     (Russian - 1049): Название процесса
                /// 
                /// Description:
                ///     (English - United States - 1033): Name of the process.
                ///     (Russian - 1049): Имя процесса.
                /// 
                /// SchemaName: Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = True
                ///</summary>
                public const string name = "name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Run as On Demand
                ///     (Russian - 1049): Выполнять по запросу
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the process is able to run as an on-demand process.
                ///     (Russian - 1049): Указывает, может ли бизнес-процесс выполняться по запросу.
                /// 
                /// SchemaName: OnDemand
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
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
                public const string ondemand = "ondemand";

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
                ///     (English - United States - 1033): Unique identifier of the user or team who owns the process.
                ///     (Russian - 1049): Уникальный идентификатор пользователя или рабочей группы, ответственных за бизнес-процесс.
                /// 
                /// SchemaName: OwnerId
                /// LookupAttributeMetadata    AttributeType: Owner    AttributeTypeName: OwnerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
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
                ///     (English - United States - 1033): Unique identifier of the business unit that owns the process.
                ///     (Russian - 1049): Уникальный идентификатор подразделения, которому принадлежит процесс.
                /// 
                /// SchemaName: OwningBusinessUnit
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
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
                ///     (English - United States - 1033): Unique identifier of the team who owns the process.
                ///     (Russian - 1049): Уникальный идентификатор рабочей группы, которой принадлежит бизнес-процесс.
                /// 
                /// SchemaName: OwningTeam
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
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
                ///     (English - United States - 1033): Unique identifier of the user who owns the process.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, владеющего процессом.
                /// 
                /// SchemaName: OwningUser
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
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
                ///     (English - United States - 1033): Parent Process ID
                ///     (Russian - 1049): Идентификатор родительского процесса
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the definition for process activation.
                ///     (Russian - 1049): Уникальный идентификатор определения активации процесса.
                /// 
                /// SchemaName: ParentWorkflowId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
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
                public const string parentworkflowid = "parentworkflowid";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the plug-in type.
                ///     (Russian - 1049): Уникальный идентификатор подключаемого модуля.
                /// 
                /// SchemaName: PluginTypeId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: sdkmessagefilter
                /// 
                ///     Target sdkmessagefilter    PrimaryIdAttribute sdkmessagefilterid
                ///         DisplayName:
                ///         (English - United States - 1033): Sdk Message Filter
                ///         (Russian - 1049): Фильтр сообщения SDK
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Sdk Message Filters
                ///         (Russian - 1049): Фильтры сообщений SDK
                ///         
                ///         Description:
                ///         (English - United States - 1033): Filter that defines which SDK messages are valid for each type of entity.
                ///         (Russian - 1049): Фильтр, определяющий, какие сообщения SDK подходят для каждого типа сущности.
                ///</summary>
                public const string plugintypeid = "plugintypeid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Primary Entity
                ///     (Russian - 1049): Основная сущность
                /// 
                /// Description:
                ///     (English - United States - 1033): Primary entity for the process. The process can be associated for one or more SDK operations defined on the primary entity.
                ///     (Russian - 1049): Основная сущность для процесса. Процесс может быть связан с одной или несколькими операциями SDK, определенными в основной сущности.
                /// 
                /// SchemaName: PrimaryEntity
                /// EntityNameAttributeMetadata    AttributeType: EntityName    AttributeTypeName: EntityNameType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Local System  OptionSet workflow_primaryentity
                /// DefaultFormValue = -1
                ///</summary>
                public const string primaryentity = "primaryentity";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Process Order
                ///     (Russian - 1049): Порядок обработки
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the business process flow order.
                ///     (Russian - 1049): Укажите порядок последовательности операций бизнес-процесса.
                /// 
                /// SchemaName: ProcessOrder
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string processorder = "processorder";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Role assignment for Process
                ///     (Russian - 1049): Назначение роли для процесса
                /// 
                /// Description:
                ///     (English - United States - 1033): Contains the role assignment for the process.
                ///     (Russian - 1049): Содержит назначение ролей для процесса.
                /// 
                /// SchemaName: ProcessRoleAssignment
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1048576
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string processroleassignment = "processroleassignment";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Rank
                ///     (Russian - 1049): Ранг
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates the rank for order of execution for the synchronous workflow.
                ///     (Russian - 1049): Указывает ранг порядка выполнения для синхронного бизнес-процесса.
                /// 
                /// SchemaName: Rank
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string rank = "rank";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Renderer Type
                ///     (Russian - 1049): Тип отображения
                /// 
                /// Description:
                ///     (English - United States - 1033): The renderer type of Workflow
                ///     (Russian - 1049): Тип отображения бизнес-процесса
                /// 
                /// SchemaName: RendererObjectTypeCode
                /// EntityNameAttributeMetadata    AttributeType: EntityName    AttributeTypeName: EntityNameType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string rendererobjecttypecode = "rendererobjecttypecode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Run As User
                ///     (Russian - 1049): Выполнять от имени пользователя
                /// 
                /// Description:
                ///     (English - United States - 1033): Specifies the system user account under which a workflow executes.
                ///     (Russian - 1049): Определяет системную учетную запись, от имени которой выполняется бизнес-процесс.
                /// 
                /// SchemaName: RunAs
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Global System  OptionSet workflow_runas
                /// DefaultFormValue = 1
                ///</summary>
                public const string runas = "runas";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Scope
                ///     (Russian - 1049): Область
                /// 
                /// Description:
                ///     (English - United States - 1033): Scope of the process.
                ///     (Russian - 1049): Область процесса.
                /// 
                /// SchemaName: Scope
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet workflow_scope
                /// DefaultFormValue = -1
                ///</summary>
                public const string scope = "scope";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): SDK Message
                ///     (Russian - 1049): Сообщение SDK
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the SDK Message associated with this workflow.
                ///     (Russian - 1049): Уникальный идентификатор сообщения SDK, связанного с этим бизнес-процессом.
                /// 
                /// SchemaName: SdkMessageId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: sdkmessage
                /// 
                ///     Target sdkmessage    PrimaryIdAttribute sdkmessageid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Sdk Message
                ///         (Russian - 1049): Сообщение SDK
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Sdk Messages
                ///         (Russian - 1049): Сообщения SDK
                ///         
                ///         Description:
                ///         (English - United States - 1033): Message that is supported by the SDK.
                ///         (Russian - 1049): Сообщение, поддерживаемое SDK.
                ///</summary>
                public const string sdkmessageid = "sdkmessageid";

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
                ///     (English - United States - 1033): Status of the process.
                ///     (Russian - 1049): Состояние процесса.
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
                ///     (English - United States - 1033): Additional information about status of the process.
                ///     (Russian - 1049): Дополнительные сведения о состоянии процесса.
                /// 
                /// SchemaName: StatusCode
                /// StatusAttributeMetadata    AttributeType: Status    AttributeTypeName: StatusType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// DefaultFormValue = -1
                ///</summary>
                public const string statuscode = "statuscode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Child Process
                ///     (Russian - 1049): Дочерний процесс?
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the process can be included in other processes as a child process.
                ///     (Russian - 1049): Указывает, можно ли включить процесс в другие процессы в качестве дочернего.
                /// 
                /// SchemaName: Subprocess
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
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
                public const string subprocess = "subprocess";

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
                ///     (English - United States - 1033): Log upon Failure
                ///     (Russian - 1049): Записывать в журнал при сбое
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether synchronous workflow failures will be saved to log files.
                ///     (Russian - 1049): Укажите, будут ли записываться сбои синхронных бизнес-процессов в файлы журналов.
                /// 
                /// SchemaName: SyncWorkflowLogOnFailure
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
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
                public const string syncworkflowlogonfailure = "syncworkflowlogonfailure";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Trigger On Create
                ///     (Russian - 1049): Запуск при создании
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the process will be triggered when the primary entity is created.
                ///     (Russian - 1049): Указывает, будет ли бизнес-процесс запущен при создании основной сущности.
                /// 
                /// SchemaName: TriggerOnCreate
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
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
                public const string triggeroncreate = "triggeroncreate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Trigger On Delete
                ///     (Russian - 1049): Запуск при удалении
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the process will be triggered on deletion of the primary entity.
                ///     (Russian - 1049): Указывает, будет ли бизнес-процесс запущен при удалении основной сущности.
                /// 
                /// SchemaName: TriggerOnDelete
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
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
                public const string triggerondelete = "triggerondelete";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Trigger On Update Attribute List
                ///     (Russian - 1049): Список атрибутов запуска при обновлении
                /// 
                /// Description:
                ///     (English - United States - 1033): Attributes that trigger the process when updated.
                ///     (Russian - 1049): Атрибуты, запускающие процесс при обновлении.
                /// 
                /// SchemaName: TriggerOnUpdateAttributeList
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string triggeronupdateattributelist = "triggeronupdateattributelist";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Type
                ///     (Russian - 1049): Тип
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of the process.
                ///     (Russian - 1049): Тип процесса.
                /// 
                /// SchemaName: Type
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet workflow_type
                /// DefaultFormValue = -1
                ///</summary>
                public const string type = "type";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Unique Name
                ///     (Russian - 1049): Уникальное имя
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique name of the process
                ///     (Russian - 1049): Уникальное имя процесса
                /// 
                /// SchemaName: UniqueName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string uniquename = "uniquename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Update Stage
                ///     (Russian - 1049): Обновить стадию
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the stage a process will be triggered on update.
                ///     (Russian - 1049): Выберите стадию, на которой будет запущен процесс при обновлении.
                /// 
                /// SchemaName: UpdateStage
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Global System  OptionSet workflow_stage
                /// DefaultFormValue = -1
                ///</summary>
                public const string updatestage = "updatestage";

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
                ///     (English - United States - 1033): Process
                ///     (Russian - 1049): Процесс
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the process.
                ///     (Russian - 1049): Уникальный идентификатор процесса.
                /// 
                /// SchemaName: WorkflowId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string workflowid = "workflowid";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: WorkflowIdUnique
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string workflowidunique = "workflowidunique";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): XAML that defines the process.
                ///     (Russian - 1049): XAML, определяющий процесс.
                /// 
                /// SchemaName: Xaml
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string xaml = "xaml";
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
                ///     (English - United States - 1033): Status of the process.
                ///     (Russian - 1049): Состояние процесса.
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
                    /// Default statuscode: Activated_2, 2
                    /// InvariantName: Activated
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Activated
                    ///     (Russian - 1049): Активирован
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Activated_1 = 1,
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
                ///     (English - United States - 1033): Status of the process.
                ///     (Russian - 1049): Состояние процесса.
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
                    /// Linked Statecode: Activated_1, 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Activated
                    ///     (Russian - 1049): Активирован
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Activated_1_Activated_2 = 2,
                }

                #endregion State and Status OptionSets.

                #region Picklist OptionSet OptionSets.

                ///<summary>
                /// Attribute: businessprocesstype
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Business Process Type
                ///     (Russian - 1049): Тип бизнес-процесса
                /// 
                /// Description:
                ///     (English - United States - 1033): Business Process Type.
                ///     (Russian - 1049): Тип бизнес-процесса.
                /// 
                /// Local System  OptionSet workflow_businessprocesstype
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Business Process Type
                ///     (Russian - 1049): Тип бизнес-процесса
                /// 
                /// Description:
                ///     (English - United States - 1033): Business Process Type.
                ///     (Russian - 1049): Тип бизнес-процесса.
                ///</summary>
                public enum businessprocesstype
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Business Flow
                    ///     (Russian - 1049): Бизнес-процесс
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Business_Flow_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Task Flow
                    ///     (Russian - 1049): Поток задач
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Task_Flow_1 = 1,
                }

                ///<summary>
                /// Attribute: category
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Category
                ///     (Russian - 1049): Категория
                /// 
                /// Description:
                ///     (English - United States - 1033): Category of the process.
                ///     (Russian - 1049): Категория процесса.
                /// 
                /// Local System  OptionSet workflow_category
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Category
                ///     (Russian - 1049): Категория
                /// 
                /// Description:
                ///     (English - United States - 1033): Category of the process.
                ///     (Russian - 1049): Категория процесса.
                ///</summary>
                public enum category
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Workflow
                    ///     (Russian - 1049): Бизнес-процесс
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Workflow_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Dialog
                    ///     (Russian - 1049): Диалоговое окно
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Dialog_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Business Rule
                    ///     (Russian - 1049): Бизнес-правило
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Business_Rule_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Action
                    ///     (Russian - 1049): Действие
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Action_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 5
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Business Process Flow
                    ///     (Russian - 1049): Последовательность операций бизнес-процесса
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Business_Process_Flow_4 = 4,
                }

                ///<summary>
                /// Attribute: mode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Mode
                ///     (Russian - 1049): Режим
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the mode of the process.
                ///     (Russian - 1049): Показывает режим процесса.
                /// 
                /// Local System  OptionSet workflow_mode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Mode
                ///     (Russian - 1049): Режим
                /// 
                /// Description:
                ///     (English - United States - 1033): Mode of the process.
                ///     (Russian - 1049): Режим процесса.
                ///</summary>
                public enum mode
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Background
                    ///     (Russian - 1049): Фон
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Background_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Real-time
                    ///     (Russian - 1049): В реальном времени
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Real_time_1 = 1,
                }

                ///<summary>
                /// Attribute: scope
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Scope
                ///     (Russian - 1049): Область
                /// 
                /// Description:
                ///     (English - United States - 1033): Scope of the process.
                ///     (Russian - 1049): Область процесса.
                /// 
                /// Local System  OptionSet workflow_scope
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Scope
                ///     (Russian - 1049): Область
                /// 
                /// Description:
                ///     (English - United States - 1033): Workflow scope.
                ///     (Russian - 1049): Область бизнес-процесса.
                ///</summary>
                public enum scope
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): User
                    ///     (Russian - 1049): Пользователь
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    User_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Business Unit
                    ///     (Russian - 1049): Подразделение
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Business_Unit_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Parent: Child Business Units
                    ///     (Russian - 1049): Родительский элемент: дочерние подразделения
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Parent_Child_Business_Units_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Organization
                    ///     (Russian - 1049): Предприятие
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Organization_4 = 4,
                }

                ///<summary>
                /// Attribute: type
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Type
                ///     (Russian - 1049): Тип
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of the process.
                ///     (Russian - 1049): Тип процесса.
                /// 
                /// Local System  OptionSet workflow_type
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Type
                ///     (Russian - 1049): Тип
                /// 
                /// Description:
                ///     (English - United States - 1033): Workflow type.
                ///     (Russian - 1049): Тип бизнес-процесса.
                ///</summary>
                public enum type
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Definition
                    ///     (Russian - 1049): Определение
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Definition_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Activation
                    ///     (Russian - 1049): Активация
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Activation_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Template
                    ///     (Russian - 1049): Шаблон
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Template_3 = 3,
                }

                #endregion Picklist OptionSets.
            }

            #endregion OptionSets.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship business_unit_workflow
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_workflow
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_workflow
                {
                    public const string Name = "business_unit_workflow";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_workflow = "workflow";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";
                }

                ///<summary>
                /// N:1 - Relationship owner_workflows
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     owner_workflows
                /// ReferencingEntityNavigationPropertyName    ownerid
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
                public static partial class owner_workflows
                {
                    public const string Name = "owner_workflows";

                    public const string ReferencedEntity_owner = "owner";

                    public const string ReferencedAttribute_ownerid = "ownerid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_workflow = "workflow";

                    public const string ReferencingAttribute_ownerid = "ownerid";
                }

                ///<summary>
                /// N:1 - Relationship system_user_workflow
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     system_user_workflow
                /// ReferencingEntityNavigationPropertyName    owninguser
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
                public static partial class system_user_workflow
                {
                    public const string Name = "system_user_workflow";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_workflow = "workflow";

                    public const string ReferencingAttribute_owninguser = "owninguser";
                }

                ///<summary>
                /// N:1 - Relationship team_workflow
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_workflow
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                public static partial class team_workflow
                {
                    public const string Name = "team_workflow";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_workflow = "workflow";

                    public const string ReferencingAttribute_owningteam = "owningteam";
                }

                ///<summary>
                /// N:1 - Relationship workflow_active_workflow
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     workflow_active_workflow
                /// ReferencingEntityNavigationPropertyName    activeworkflowid
                /// IsCustomizable                             False                       False
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
                ///</summary>
                public static partial class workflow_active_workflow
                {
                    public const string Name = "workflow_active_workflow";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_workflow = "workflow";

                    public const string ReferencingAttribute_activeworkflowid = "activeworkflowid";
                }

                ///<summary>
                /// N:1 - Relationship workflow_createdby
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     workflow_createdby
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
                public static partial class workflow_createdby
                {
                    public const string Name = "workflow_createdby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_workflow = "workflow";

                    public const string ReferencingAttribute_createdby = "createdby";
                }

                ///<summary>
                /// N:1 - Relationship workflow_createdonbehalfby
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     workflow_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    createdonbehalfby
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
                public static partial class workflow_createdonbehalfby
                {
                    public const string Name = "workflow_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_workflow = "workflow";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship workflow_entityimage
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     workflow_entityimage
                /// ReferencingEntityNavigationPropertyName    entityimageinstance_workflow
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
                public static partial class workflow_entityimage
                {
                    public const string Name = "workflow_entityimage";

                    public const string ReferencedEntity_imagedescriptor = "imagedescriptor";

                    public const string ReferencedAttribute_imagedescriptorid = "imagedescriptorid";

                    public const string ReferencingEntity_workflow = "workflow";

                    public const string ReferencingAttribute_entityimageid = "entityimageid";
                }

                ///<summary>
                /// N:1 - Relationship workflow_modifiedby
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     workflow_modifiedby
                /// ReferencingEntityNavigationPropertyName    modifiedby
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
                public static partial class workflow_modifiedby
                {
                    public const string Name = "workflow_modifiedby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_workflow = "workflow";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";
                }

                ///<summary>
                /// N:1 - Relationship workflow_modifiedonbehalfby
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     workflow_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    modifiedonbehalfby
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
                public static partial class workflow_modifiedonbehalfby
                {
                    public const string Name = "workflow_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_workflow = "workflow";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship workflow_parent_workflow
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     workflow_parent_workflow
                /// ReferencingEntityNavigationPropertyName    parentworkflowid
                /// IsCustomizable                             False                       False
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
                public static partial class workflow_parent_workflow
                {
                    public const string Name = "workflow_parent_workflow";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_workflow = "workflow";

                    public const string ReferencingAttribute_parentworkflowid = "parentworkflowid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship convertruleitembase_workflowid
                /// 
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     convertruleitembase_workflowid
                /// ReferencingEntityNavigationPropertyName    workflowid
                /// IsCustomizable                             True                              False
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
                /// ReferencingEntity convertruleitem:
                ///     DisplayName:
                ///     (English - United States - 1033): Record Creation and Update Rule Item
                ///     (Russian - 1049): Элемент правила создания и обновления записей
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Record Creation and Update Rule Items
                ///     (Russian - 1049): Элементы правила создания и обновления записей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Defines the individual conditions required for creating records automatically.
                ///     (Russian - 1049): Определяет отдельные условия, необходимые для автоматического создания записей.
                ///</summary>
                public static partial class convertruleitembase_workflowid
                {
                    public const string Name = "convertruleitembase_workflowid";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_convertruleitem = "convertruleitem";

                    public const string ReferencingAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship lk_asyncoperation_workflowactivationid
                /// 
                /// PropertyName                               Value                                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_asyncoperation_workflowactivationid
                /// ReferencingEntityNavigationPropertyName    workflowactivationid
                /// IsCustomizable                             False                                     False
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
                public static partial class lk_asyncoperation_workflowactivationid
                {
                    public const string Name = "lk_asyncoperation_workflowactivationid";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_asyncoperation = "asyncoperation";

                    public const string ReferencingAttribute_workflowactivationid = "workflowactivationid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship lk_expiredprocess_processid
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     workflow_expiredprocess
                /// ReferencingEntityNavigationPropertyName    processid
                /// IsCustomizable                             True                       False
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
                public static partial class lk_expiredprocess_processid
                {
                    public const string Name = "lk_expiredprocess_processid";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_expiredprocess = "expiredprocess";

                    public const string ReferencingAttribute_processid = "processid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship lk_leadtoopportunitysalesprocess_processid
                /// 
                /// PropertyName                               Value                                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     workflow_leadtoopportunitysalesprocess
                /// ReferencingEntityNavigationPropertyName    processid
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
                public static partial class lk_leadtoopportunitysalesprocess_processid
                {
                    public const string Name = "lk_leadtoopportunitysalesprocess_processid";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_leadtoopportunitysalesprocess = "leadtoopportunitysalesprocess";

                    public const string ReferencingAttribute_processid = "processid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship lk_newprocess_processid
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     workflow_newprocess
                /// ReferencingEntityNavigationPropertyName    processid
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
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
                public static partial class lk_newprocess_processid
                {
                    public const string Name = "lk_newprocess_processid";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_newprocess = "newprocess";

                    public const string ReferencingAttribute_processid = "processid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship lk_opportunitysalesprocess_processid
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     workflow_opportunitysalesprocess
                /// ReferencingEntityNavigationPropertyName    processid
                /// IsCustomizable                             True                                False
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
                public static partial class lk_opportunitysalesprocess_processid
                {
                    public const string Name = "lk_opportunitysalesprocess_processid";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_opportunitysalesprocess = "opportunitysalesprocess";

                    public const string ReferencingAttribute_processid = "processid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship lk_phonetocaseprocess_processid
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     workflow_phonetocaseprocess
                /// ReferencingEntityNavigationPropertyName    processid
                /// IsCustomizable                             True                           False
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
                public static partial class lk_phonetocaseprocess_processid
                {
                    public const string Name = "lk_phonetocaseprocess_processid";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_phonetocaseprocess = "phonetocaseprocess";

                    public const string ReferencingAttribute_processid = "processid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship lk_processsession_processid
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_processsession_processid
                /// ReferencingEntityNavigationPropertyName    processid
                /// IsCustomizable                             False                          False
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
                public static partial class lk_processsession_processid
                {
                    public const string Name = "lk_processsession_processid";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_processsession = "processsession";

                    public const string ReferencingAttribute_processid = "processid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship lk_translationprocess_processid
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     workflow_translationprocess
                /// ReferencingEntityNavigationPropertyName    processid
                /// IsCustomizable                             True                           False
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
                public static partial class lk_translationprocess_processid
                {
                    public const string Name = "lk_translationprocess_processid";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_translationprocess = "translationprocess";

                    public const string ReferencingAttribute_processid = "processid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship lk_vw_caseprocess_processid
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_vw_caseprocess_processid
                /// ReferencingEntityNavigationPropertyName    processidname
                /// IsCustomizable                             True                           True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
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
                /// ReferencingEntity vw_caseprocess:
                ///     DisplayName:
                ///     (English - United States - 1033): VW Case Process
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Base entity for process VW Case Process
                ///</summary>
                public static partial class lk_vw_caseprocess_processid
                {
                    public const string Name = "lk_vw_caseprocess_processid";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_vw_caseprocess = "vw_caseprocess";

                    public const string ReferencingAttribute_processid = "processid";

                    public const string ReferencingEntity_PrimaryNameAttribute_bpf_name = "bpf_name";
                }

                ///<summary>
                /// 1:N - Relationship lk_vw_vwleadprocess_processid
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_vw_vwleadprocess_processid
                /// ReferencingEntityNavigationPropertyName    processidname
                /// IsCustomizable                             True                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
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
                /// ReferencingEntity vw_vwleadprocess:
                ///     DisplayName:
                ///     (English - United States - 1033): VW Lead Process
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Base entity for process VW Lead Process
                ///</summary>
                public static partial class lk_vw_vwleadprocess_processid
                {
                    public const string Name = "lk_vw_vwleadprocess_processid";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_vw_vwleadprocess = "vw_vwleadprocess";

                    public const string ReferencingAttribute_processid = "processid";

                    public const string ReferencingEntity_PrimaryNameAttribute_bpf_name = "bpf_name";
                }

                ///<summary>
                /// 1:N - Relationship process_processstage
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     process_processstage
                /// ReferencingEntityNavigationPropertyName    processid
                /// IsCustomizable                             False                    False
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
                /// ReferencingEntity processstage:
                ///     DisplayName:
                ///     (English - United States - 1033): Process Stage
                ///     (Russian - 1049): Стадия процесса
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Process Stages
                ///     (Russian - 1049): Стадии процесса
                ///     
                ///     Description:
                ///     (English - United States - 1033): Stage associated with a process.
                ///     (Russian - 1049): Стадия, связанная с процессом.
                ///</summary>
                public static partial class process_processstage
                {
                    public const string Name = "process_processstage";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_processstage = "processstage";

                    public const string ReferencingAttribute_processid = "processid";

                    public const string ReferencingEntity_PrimaryNameAttribute_stagename = "stagename";
                }

                ///<summary>
                /// 1:N - Relationship process_processtrigger
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     process_processtrigger
                /// ReferencingEntityNavigationPropertyName    processid
                /// IsCustomizable                             False                     False
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
                public static partial class process_processtrigger
                {
                    public const string Name = "process_processtrigger";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_processtrigger = "processtrigger";

                    public const string ReferencingAttribute_processid = "processid";
                }

                ///<summary>
                /// 1:N - Relationship slabase_workflowid
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
                /// ReferencingEntity sla:
                ///     DisplayName:
                ///     (English - United States - 1033): SLA
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): SLAs
                ///     
                ///     Description:
                ///     (English - United States - 1033): Contains information about the tracked service-level KPIs for cases that belong to different customers.
                ///     (Russian - 1049): Содержит информацию об отслеживаемых ключевых индикаторах уровня обслуживания (KPI) для обращений, принадлежащих разным клиентам.
                ///</summary>
                public static partial class slabase_workflowid
                {
                    public const string Name = "slabase_workflowid";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_sla = "sla";

                    public const string ReferencingAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship slaitembase_workflowid
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     slaitembase_workflowid
                /// ReferencingEntityNavigationPropertyName    workflowid
                /// IsCustomizable                             True                      False
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
                ///</summary>
                public static partial class slaitembase_workflowid
                {
                    public const string Name = "slaitembase_workflowid";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_slaitem = "slaitem";

                    public const string ReferencingAttribute_workflowid = "workflowid";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_workflow
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_workflow
                /// ReferencingEntityNavigationPropertyName    objectid_workflow
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
                public static partial class userentityinstancedata_workflow
                {
                    public const string Name = "userentityinstancedata_workflow";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }

                ///<summary>
                /// 1:N - Relationship workflow_active_workflow
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     workflow_active_workflow
                /// ReferencingEntityNavigationPropertyName    activeworkflowid
                /// IsCustomizable                             False                       False
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
                ///</summary>
                public static partial class workflow_active_workflow
                {
                    public const string Name = "workflow_active_workflow";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_workflow = "workflow";

                    public const string ReferencingAttribute_activeworkflowid = "activeworkflowid";
                }

                ///<summary>
                /// 1:N - Relationship Workflow_Annotation
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Workflow_Annotation
                /// ReferencingEntityNavigationPropertyName    objectid_workflow
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
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
                public static partial class workflow_annotation
                {
                    public const string Name = "Workflow_Annotation";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_annotation = "annotation";

                    public const string ReferencingAttribute_objectid = "objectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship workflow_dependencies
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     workflow_dependencies
                /// ReferencingEntityNavigationPropertyName    workflowid
                /// IsCustomizable                             False                    False
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
                /// ReferencingEntity workflowdependency:
                ///     DisplayName:
                ///     (English - United States - 1033): Process Dependency
                ///     (Russian - 1049): Зависимость процесса
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Process Dependencies
                ///     (Russian - 1049): Зависимости процесса
                ///     
                ///     Description:
                ///     (English - United States - 1033): Dependencies for a process.
                ///     (Russian - 1049): Зависимости процесса.
                ///</summary>
                public static partial class workflow_dependencies
                {
                    public const string Name = "workflow_dependencies";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_workflowdependency = "workflowdependency";

                    public const string ReferencingAttribute_workflowid = "workflowid";
                }

                ///<summary>
                /// 1:N - Relationship workflow_parent_workflow
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     workflow_parent_workflow
                /// ReferencingEntityNavigationPropertyName    parentworkflowid
                /// IsCustomizable                             False                       False
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
                public static partial class workflow_parent_workflow
                {
                    public const string Name = "workflow_parent_workflow";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_workflow = "workflow";

                    public const string ReferencingAttribute_parentworkflowid = "parentworkflowid";
                }

                ///<summary>
                /// 1:N - Relationship Workflow_routingrule
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Workflow_routingrule
                /// ReferencingEntityNavigationPropertyName    workflowid
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
                public static partial class workflow_routingrule
                {
                    public const string Name = "Workflow_routingrule";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_routingrule = "routingrule";

                    public const string ReferencingAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship Workflow_SyncErrors
                /// 
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Workflow_SyncErrors
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_workflow_syncerror
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
                public static partial class workflow_syncerrors
                {
                    public const string Name = "Workflow_SyncErrors";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_syncerror = "syncerror";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship workflowid_convertrule
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     workflowid_convertrule
                /// ReferencingEntityNavigationPropertyName    workflowid
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
                /// ReferencingEntity convertrule:
                ///     DisplayName:
                ///     (English - United States - 1033): Record Creation and Update Rule
                ///     (Russian - 1049): Правило создания и обновления записей
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Record Creation and Update Rules
                ///     (Russian - 1049): Правила создания и обновления записей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Defines the settings for automatic record creation.
                ///     (Russian - 1049): Определяет параметры автоматического создания записей.
                ///</summary>
                public static partial class workflowid_convertrule
                {
                    public const string Name = "workflowid_convertrule";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_convertrule = "convertrule";

                    public const string ReferencingAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship workflowid_profilerule
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     workflowid_profilerule
                /// ReferencingEntityNavigationPropertyName    workflowid
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
                /// ReferencingEntity channelaccessprofilerule:
                ///     DisplayName:
                ///     (English - United States - 1033): Channel Access Profile Rule
                ///     (Russian - 1049): Правило профиля доступа к каналам
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Channel Access Profile Rules
                ///     (Russian - 1049): Правила профиля доступа к каналам
                ///     
                ///     Description:
                ///     (English - United States - 1033): Defines the rules for automatically associating channel access profiles to external party records.For internal use only
                ///     (Russian - 1049): Определяет правила для автоматической связи профилей доступа к каналам с записями внешней стороны. Только для внутреннего использования.
                ///</summary>
                public static partial class workflowid_profilerule
                {
                    public const string Name = "workflowid_profilerule";

                    public const string ReferencedEntity_workflow = "workflow";

                    public const string ReferencedAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_channelaccessprofilerule = "channelaccessprofilerule";

                    public const string ReferencingAttribute_workflowid = "workflowid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}
