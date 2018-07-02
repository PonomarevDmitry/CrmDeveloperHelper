
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - 1033): Team
    /// (Russian - 1049): Рабочая группа
    /// 
    /// DisplayCollectionName:
    /// (English - 1033): Teams
    /// (Russian - 1049): Рабочие группы
    /// 
    /// Description:
    /// (English - 1033): Collection of system users that routinely collaborate. Teams can be used to simplify record sharing and provide team members with common access to organization data when team members belong to different Business Units.
    /// (Russian - 1049): Набор системных пользователей, которые обычно участвуют в совместной работе. Рабочие группы можно использовать для упрощения предоставления участникам группы общего доступа к записям и данным организации, если участники принадлежат к разным подразделениям.
    /// 
    /// PropertyName                          Value            CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     True             False
    /// CanBePrimaryEntityInRelationship      True             False
    /// CanBeRelatedEntityInRelationship      True             False
    /// CanChangeHierarchicalRelationship     True             True
    /// CanChangeTrackingBeEnabled            True             True
    /// CanCreateAttributes                   True             False
    /// CanCreateCharts                       True             False
    /// CanCreateForms                        True             False
    /// CanCreateViews                        True             False
    /// CanEnableSyncToExternalSearchIndex    True             True
    /// CanModifyAdditionalSettings           True             True
    /// CanTriggerWorkflow                    True
    /// ChangeTrackingEnabled                 True
    /// CollectionSchemaName                  Teams
    /// DaysSinceRecordLastModified           9999
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         teams
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          True
    /// IsAuditEnabled                        False            True
    /// IsAvailableOffline                    True
    /// IsBusinessProcessEnabled              True
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  True             True
    /// IsCustomEntity                        False
    /// IsCustomizable                        True             False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False            True
    /// IsEnabledForCharts                    True
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          True
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False            True
    /// IsManaged                             True
    /// IsMappable                            True             False
    /// IsOfflineInMobileClient               True             True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              True             False
    /// IsRenameable                          False            False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                True
    /// IsValidForQueue                       False            True
    /// IsVisibleInMobile                     False            False
    /// IsVisibleInMobileClient               True             False
    /// LogicalCollectionName                 teams
    /// LogicalName                           team
    /// ObjectTypeCode                        9
    /// OwnershipType                         BusinessOwned
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredTeam
    /// SchemaName                            Team
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class Team
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "team";

            public const string EntitySchemaName = "Team";

            public const string EntityPrimaryNameAttribute = "name";

            public const string EntityPrimaryIdAttribute = "teamid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Administrator
                ///     (Russian - 1049): Администратор
                /// 
                /// Description:
                ///     (English - 1033): Unique identifier of the user primary responsible for the team.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, несущего основную ответственность за рабочую группу.
                /// 
                /// SchemaName: AdministratorId
                /// LookupAttributeMetadata   AttributeType: Lookup   AttributeTypeName: LookupType   RequiredLevel: SystemRequired   IsManaged True
                /// IsValidForCreate: True   IsValidForRead: True   IsValidForUpdate: True   IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: Simple
                /// Targets: systemuser
                ///</summary>
                public const string administratorid = "administratorid";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Business Unit
                ///     (Russian - 1049): Подразделение
                /// 
                /// Description:
                ///     (English - 1033): Unique identifier of the business unit with which the team is associated.
                ///     (Russian - 1049): Уникальный идентификатор подразделения, с которым связана рабочая группа.
                /// 
                /// SchemaName: BusinessUnitId
                /// LookupAttributeMetadata   AttributeType: Lookup   AttributeTypeName: LookupType   RequiredLevel: SystemRequired   IsManaged True
                /// IsValidForCreate: True   IsValidForRead: True   IsValidForUpdate: True   IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: Simple
                /// Targets: businessunit
                ///</summary>
                public const string businessunitid = "businessunitid";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Created By
                ///     (Russian - 1049): Создано
                /// 
                /// Description:
                ///     (English - 1033): Unique identifier of the user who created the team.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, создавшего рабочую группу.
                /// 
                /// SchemaName: CreatedBy
                /// LookupAttributeMetadata   AttributeType: Lookup   AttributeTypeName: LookupType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: False   IsValidForRead: True   IsValidForUpdate: False   IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: Simple
                /// Targets: systemuser
                ///</summary>
                public const string createdby = "createdby";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Created On
                ///     (Russian - 1049): Дата создания
                /// 
                /// Description:
                ///     (English - 1033): Date and time when the team was created.
                ///     (Russian - 1049): Дата и время создания рабочей группы.
                /// 
                /// SchemaName: CreatedOn
                /// DateTimeAttributeMetadata   AttributeType: DateTime   AttributeTypeName: DateTimeType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: False   IsValidForRead: True   IsValidForUpdate: False   IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: 0
                /// DateTimeBehavior = UserLocal   CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive   Format = DateAndTime
                ///</summary>
                public const string createdon = "createdon";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Created By (Delegate)
                ///     (Russian - 1049): Кем создано (делегат)
                /// 
                /// Description:
                ///     (English - 1033): Unique identifier of the delegate user who created the team.
                ///     (Russian - 1049): Уникальный идентификатор делегата, создавшего рабочую группу.
                /// 
                /// SchemaName: CreatedOnBehalfBy
                /// LookupAttributeMetadata   AttributeType: Lookup   AttributeTypeName: LookupType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: False   IsValidForRead: True   IsValidForUpdate: False   IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: Simple
                /// Targets: systemuser
                ///</summary>
                public const string createdonbehalfby = "createdonbehalfby";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Description
                ///     (Russian - 1049): Описание
                /// 
                /// Description:
                ///     (English - 1033): Description of the team.
                ///     (Russian - 1049): Описание рабочей группы.
                /// 
                /// SchemaName: Description
                /// MemoAttributeMetadata   AttributeType: Memo   AttributeTypeName: MemoType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: True   IsValidForRead: True   IsValidForUpdate: True   IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: Simple
                /// MaxLength = 2000
                /// Format = TextArea   ImeMode = Auto   IsLocalizable = False
                ///</summary>
                public const string description = "description";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Email
                ///     (Russian - 1049): Электронная почта
                /// 
                /// Description:
                ///     (English - 1033): Email address for the team.
                ///     (Russian - 1049): Адрес электронной почты группы.
                /// 
                /// SchemaName: EMailAddress
                /// StringAttributeMetadata   AttributeType: String   AttributeTypeName: StringType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: True   IsValidForRead: True   IsValidForUpdate: True   IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: 0
                /// MaxLength = 100
                /// Format = Email   ImeMode = Inactive   IsLocalizable = False
                ///</summary>
                public const string emailaddress = "emailaddress";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Exchange Rate
                ///     (Russian - 1049): Валютный курс
                /// 
                /// Description:
                ///     (English - 1033): Exchange rate for the currency associated with the team with respect to the base currency.
                ///     (Russian - 1049): Курс обмена валюты, связанной с рабочей группой, по отношению к базовой валюте.
                /// 
                /// SchemaName: ExchangeRate
                /// DecimalAttributeMetadata   AttributeType: Decimal   AttributeTypeName: DecimalType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: False   IsValidForRead: True   IsValidForUpdate: False   IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: 0
                /// MinValue = 0,0000000001   MaxValue = 100000000000   Precision = 10
                /// ImeMode = Disabled
                ///</summary>
                public const string exchangerate = "exchangerate";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Import Sequence Number
                ///     (Russian - 1049): Порядковый номер импорта
                /// 
                /// Description:
                ///     (English - 1033): Unique identifier of the data import or data migration that created this record.
                ///     (Russian - 1049): Уникальный идентификатор импорта или переноса данных, создавшего эту запись.
                /// 
                /// SchemaName: ImportSequenceNumber
                /// IntegerAttributeMetadata   AttributeType: Integer   AttributeTypeName: IntegerType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: True   IsValidForRead: True   IsValidForUpdate: False   IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: 0
                /// MinValue = -2147483648   MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string importsequencenumber = "importsequencenumber";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Is Default
                ///     (Russian - 1049): По умолчанию
                /// 
                /// Description:
                ///     (English - 1033): Information about whether the team is a default business unit team.
                ///     (Russian - 1049): Сведения о том, является ли рабочая группа группой подразделения по умолчанию.
                /// 
                /// SchemaName: IsDefault
                /// BooleanAttributeMetadata   AttributeType: Boolean   AttributeTypeName: BooleanType   RequiredLevel: SystemRequired   IsManaged True
                /// IsValidForCreate: False   IsValidForRead: True   IsValidForUpdate: False   IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///    (English - 1033): No
                ///    (Russian - 1049): Нет
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///    (English - 1033): Yes
                ///    (Russian - 1049): Да
                /// TrueOption = 1
                ///</summary>
                public const string isdefault = "isdefault";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Modified By
                ///     (Russian - 1049): Изменено
                /// 
                /// Description:
                ///     (English - 1033): Unique identifier of the user who last modified the team.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, внесшего последнее изменение в рабочую группу.
                /// 
                /// SchemaName: ModifiedBy
                /// LookupAttributeMetadata   AttributeType: Lookup   AttributeTypeName: LookupType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: False   IsValidForRead: True   IsValidForUpdate: False   IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: Simple
                /// Targets: systemuser
                ///</summary>
                public const string modifiedby = "modifiedby";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Modified On
                ///     (Russian - 1049): Дата изменения
                /// 
                /// Description:
                ///     (English - 1033): Date and time when the team was last modified.
                ///     (Russian - 1049): Дата и время последнего изменения рабочей группы.
                /// 
                /// SchemaName: ModifiedOn
                /// DateTimeAttributeMetadata   AttributeType: DateTime   AttributeTypeName: DateTimeType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: False   IsValidForRead: True   IsValidForUpdate: False   IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: 0
                /// DateTimeBehavior = UserLocal   CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive   Format = DateAndTime
                ///</summary>
                public const string modifiedon = "modifiedon";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Modified By (Delegate)
                ///     (Russian - 1049): Кем изменено (делегат)
                /// 
                /// Description:
                ///     (English - 1033): Unique identifier of the delegate user who last modified the team.
                ///     (Russian - 1049): Уникальный идентификатор делегата, внесшего последнее изменение в рабочую группу.
                /// 
                /// SchemaName: ModifiedOnBehalfBy
                /// LookupAttributeMetadata   AttributeType: Lookup   AttributeTypeName: LookupType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: False   IsValidForRead: True   IsValidForUpdate: False   IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: Simple
                /// Targets: systemuser
                ///</summary>
                public const string modifiedonbehalfby = "modifiedonbehalfby";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Team Name
                ///     (Russian - 1049): Название группы
                /// 
                /// Description:
                ///     (English - 1033): Name of the team.
                ///     (Russian - 1049): Название рабочей группы.
                /// 
                /// SchemaName: Name
                /// StringAttributeMetadata   AttributeType: String   AttributeTypeName: StringType   RequiredLevel: SystemRequired   IsManaged True
                /// IsValidForCreate: True   IsValidForRead: True   IsValidForUpdate: True   IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: 0
                /// MaxLength = 160
                /// Format = Text   ImeMode = Auto   IsLocalizable = False
                ///</summary>
                public const string name = "name";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Organization 
                ///     (Russian - 1049): Предприятие 
                /// 
                /// Description:
                ///     (English - 1033): Unique identifier of the organization associated with the team.
                ///     (Russian - 1049): Уникальный идентификатор организации, связанной с рабочей группой.
                /// 
                /// SchemaName: OrganizationId
                /// AttributeMetadata   AttributeType: Uniqueidentifier   AttributeTypeName: UniqueidentifierType   RequiredLevel: SystemRequired   IsManaged True
                /// IsValidForCreate: False   IsValidForRead: True   IsValidForUpdate: False   IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: Simple
                ///</summary>
                public const string organizationid = "organizationid";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Record Created On
                ///     (Russian - 1049): Дата создания записи
                /// 
                /// Description:
                ///     (English - 1033): Date and time that the record was migrated.
                ///     (Russian - 1049): Дата и время переноса записи.
                /// 
                /// SchemaName: OverriddenCreatedOn
                /// DateTimeAttributeMetadata   AttributeType: DateTime   AttributeTypeName: DateTimeType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: True   IsValidForRead: True   IsValidForUpdate: False   IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: 0
                /// DateTimeBehavior = UserLocal   CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive   Format = DateOnly
                ///</summary>
                public const string overriddencreatedon = "overriddencreatedon";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Process
                ///     (Russian - 1049): Процесс
                /// 
                /// Description:
                ///     (English - 1033): Shows the ID of the process.
                ///     (Russian - 1049): Показывает идентификатор процесса.
                /// 
                /// SchemaName: ProcessId
                /// AttributeMetadata   AttributeType: Uniqueidentifier   AttributeTypeName: UniqueidentifierType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: True   IsValidForRead: True   IsValidForUpdate: True   IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: Simple
                ///</summary>
                public const string processid = "processid";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Default Queue
                ///     (Russian - 1049): Очередь по умолчанию
                /// 
                /// Description:
                ///     (English - 1033): Unique identifier of the default queue for the team.
                ///     (Russian - 1049): Уникальный идентификатор очереди по умолчанию для рабочей группы.
                /// 
                /// SchemaName: QueueId
                /// LookupAttributeMetadata   AttributeType: Lookup   AttributeTypeName: LookupType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: True   IsValidForRead: True   IsValidForUpdate: True   IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: Simple
                /// Targets: queue
                ///</summary>
                public const string queueid = "queueid";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Regarding Object Id
                ///     (Russian - 1049): Идентификатор связанного объекта
                /// 
                /// Description:
                ///     (English - 1033): Choose the record that the team relates to.
                ///     (Russian - 1049): Выберите запись, к которой относится рабочая группа.
                /// 
                /// SchemaName: RegardingObjectId
                /// LookupAttributeMetadata   AttributeType: Lookup   AttributeTypeName: LookupType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: True   IsValidForRead: True   IsValidForUpdate: False   IsValidForAdvancedFind: False    CanBeChanged = True
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: Simple
                /// Targets: knowledgearticle,list,opportunity
                ///</summary>
                public const string regardingobjectid = "regardingobjectid";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Process Stage
                ///     (Russian - 1049): Стадия процесса
                /// 
                /// Description:
                ///     (English - 1033): Shows the ID of the stage.
                ///     (Russian - 1049): Показывает идентификатор стадии.
                /// 
                /// SchemaName: StageId
                /// AttributeMetadata   AttributeType: Uniqueidentifier   AttributeTypeName: UniqueidentifierType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: True   IsValidForRead: True   IsValidForUpdate: True   IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: Simple
                ///</summary>
                public const string stageid = "stageid";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Is System Managed
                ///     (Russian - 1049): Управляется системой
                /// 
                /// Description:
                ///     (English - 1033): Select whether the team will be managed by the system.
                ///     (Russian - 1049): Укажите, управляется ли рабочая группа системой.
                /// 
                /// SchemaName: SystemManaged
                /// BooleanAttributeMetadata   AttributeType: Boolean   AttributeTypeName: BooleanType   RequiredLevel: SystemRequired   IsManaged True
                /// IsValidForCreate: False   IsValidForRead: True   IsValidForUpdate: False   IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///    (English - 1033): No
                ///    (Russian - 1049): Нет
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///    (English - 1033): Yes
                ///    (Russian - 1049): Да
                /// TrueOption = 1
                ///</summary>
                public const string systemmanaged = "systemmanaged";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Team
                ///     (Russian - 1049): Рабочая группа
                /// 
                /// Description:
                ///     (English - 1033): Unique identifier for the team.
                ///     (Russian - 1049): Уникальный идентификатор рабочей группы.
                /// 
                /// SchemaName: TeamId
                /// AttributeMetadata   AttributeType: Uniqueidentifier   AttributeTypeName: UniqueidentifierType   RequiredLevel: SystemRequired   IsManaged True
                /// IsValidForCreate: True   IsValidForRead: True   IsValidForUpdate: False   IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: Simple
                ///</summary>
                public const string teamid = "teamid";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Team Template Identifier
                ///     (Russian - 1049): Идентификатор шаблона рабочей группы
                /// 
                /// Description:
                ///     (English - 1033): Shows the team template that is associated with the team.
                ///     (Russian - 1049): Показывает шаблон группы, связанный с данной рабочей группой.
                /// 
                /// SchemaName: TeamTemplateId
                /// LookupAttributeMetadata   AttributeType: Lookup   AttributeTypeName: LookupType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: True   IsValidForRead: True   IsValidForUpdate: False   IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: Simple
                /// Targets: teamtemplate
                ///</summary>
                public const string teamtemplateid = "teamtemplateid";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Team Type
                ///     (Russian - 1049): Тип рабочей группы
                /// 
                /// Description:
                ///     (English - 1033): Select the team type.
                ///     (Russian - 1049): Выберите тип рабочей группы.
                /// 
                /// SchemaName: TeamType
                /// PicklistAttributeMetadata   AttributeType: Picklist   AttributeTypeName: PicklistType   RequiredLevel: SystemRequired   IsManaged True
                /// IsValidForCreate: True   IsValidForRead: True   IsValidForUpdate: False   IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: 0
                /// OptionSet Name: team_type   IsGlobal: False   IsCustomOptionSet: False   IsManaged: True
                /// DefaultFormValue = 0
                ///</summary>
                public const string teamtype = "teamtype";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Currency
                ///     (Russian - 1049): Валюта
                /// 
                /// Description:
                ///     (English - 1033): Unique identifier of the currency associated with the team.
                ///     (Russian - 1049): Уникальный идентификатор валюты, связанной с рабочей группой.
                /// 
                /// SchemaName: TransactionCurrencyId
                /// LookupAttributeMetadata   AttributeType: Lookup   AttributeTypeName: LookupType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: True   IsValidForRead: True   IsValidForUpdate: True   IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: Simple
                /// Targets: transactioncurrency
                ///</summary>
                public const string transactioncurrencyid = "transactioncurrencyid";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Traversed Path
                ///     (Russian - 1049): Пройденный путь
                /// 
                /// Description:
                ///     (English - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: TraversedPath
                /// StringAttributeMetadata   AttributeType: String   AttributeTypeName: StringType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: True   IsValidForRead: True   IsValidForUpdate: True   IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: 0
                /// MaxLength = 1250
                /// Format = Text   ImeMode = Auto   IsLocalizable = False
                ///</summary>
                public const string traversedpath = "traversedpath";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Version number
                ///     (Russian - 1049): Номер версии
                /// 
                /// Description:
                ///     (English - 1033): Version number of the team.
                ///     (Russian - 1049): Номер версии рабочей группы.
                /// 
                /// SchemaName: VersionNumber
                /// BigIntAttributeMetadata   AttributeType: BigInt   AttributeTypeName: BigIntType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: False   IsValidForRead: True   IsValidForUpdate: False   IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: Simple
                /// MinValue = -9223372036854775808   MaxValue = 9223372036854775807
                ///</summary>
                public const string versionnumber = "versionnumber";

                ///<summary>
                /// DisplayName:
                ///     (English - 1033): Yomi Name
                ///     (Russian - 1049): Имя Yomi
                /// 
                /// Description:
                ///     (English - 1033): Pronunciation of the full name of the team, written in phonetic hiragana or katakana characters.
                ///     (Russian - 1049): Фонетическая транскрипция имени рабочей группы, написанная символами хираганы или катаканы.
                /// 
                /// SchemaName: YomiName
                /// StringAttributeMetadata   AttributeType: String   AttributeTypeName: StringType   RequiredLevel: None   IsManaged True
                /// IsValidForCreate: True   IsValidForRead: True   IsValidForUpdate: True   IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False   IsSecured: False   IsCustomAttribute: False   SourceType: 0
                /// MaxLength = 160
                /// Format = PhoneticGuide   ImeMode = Active   IsLocalizable = False
                ///</summary>
                public const string yominame = "yominame";
            }

            #endregion Attributes.

            #region OptionSets.

            public static partial class OptionSets
            {

                #region Picklist OptionSet OptionSets.
                ///<summary>
                /// Attribute: teamtype
                /// 
                /// DisplayName:
                ///     (English - 1033): Team Type
                ///     (Russian - 1049): Тип рабочей группы
                /// 
                /// Description:
                ///     (English - 1033): Select the team type.
                ///     (Russian - 1049): Выберите тип рабочей группы.
                /// 
                /// OptionSet Name: team_type      IsGlobal: False      IsCustomOptionSet: False    IsManaged: True
                /// 
                /// DisplayName:
                ///     (English - 1033): Team Type
                ///     (Russian - 1049): Тип рабочей группы
                /// 
                /// Description:
                ///     (English - 1033): Information about team type.
                ///     (Russian - 1049): Сведения о типе рабочей группы.
                ///</summary>
                public enum teamtype
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// IsManaged: True
                    /// 
                    /// DisplayName:
                    ///     (English - 1033): Owner
                    ///     (Russian - 1049): Ответственный
                    ///</summary>
                    Owner_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// IsManaged: True
                    /// 
                    /// DisplayName:
                    ///     (English - 1033): Access
                    ///     (Russian - 1049): Доступ
                    ///</summary>
                    Access_1 = 1,
                }

                #endregion Picklist OptionSets.
            }

            #endregion OptionSets.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship business_unit_teams
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_teams
                /// ReferencingEntityNavigationPropertyName    businessunitid
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class business_unit_teams
                {
                    public const string Name = "business_unit_teams";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_businessunitid = "businessunitid";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";
                }

                ///<summary>
                /// N:1 - Relationship knowledgearticle_Teams
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     knowledgearticle_Teams
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_knowledgearticle
                /// IsCustomizable                             True                                  False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                ///</summary>
                public static partial class knowledgearticle_teams
                {
                    public const string Name = "knowledgearticle_Teams";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencedEntity_knowledgearticle = "knowledgearticle";

                    public const string ReferencedAttribute_knowledgearticleid = "knowledgearticleid";
                }

                ///<summary>
                /// N:1 - Relationship list_Teams
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     list_Teams
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_list
                /// IsCustomizable                             True                      True
                /// IsCustomRelationship                       False
                /// IsManaged                                  False
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
                ///</summary>
                public static partial class list_teams
                {
                    public const string Name = "list_Teams";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencedEntity_list = "list";

                    public const string ReferencedAttribute_listid = "listid";
                }

                ///<summary>
                /// N:1 - Relationship lk_team_createdonbehalfby
                /// 
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_team_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    createdonbehalfby
                /// IsCustomizable                             True                         False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class lk_team_createdonbehalfby
                {
                    public const string Name = "lk_team_createdonbehalfby";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";
                }

                ///<summary>
                /// N:1 - Relationship lk_team_modifiedonbehalfby
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_team_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    modifiedonbehalfby
                /// IsCustomizable                             True                          False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class lk_team_modifiedonbehalfby
                {
                    public const string Name = "lk_team_modifiedonbehalfby";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";
                }

                ///<summary>
                /// N:1 - Relationship lk_teambase_administratorid
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_teambase_administratorid
                /// ReferencingEntityNavigationPropertyName    administratorid
                /// IsCustomizable                             True                           False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class lk_teambase_administratorid
                {
                    public const string Name = "lk_teambase_administratorid";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_administratorid = "administratorid";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";
                }

                ///<summary>
                /// N:1 - Relationship lk_teambase_createdby
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_teambase_createdby
                /// ReferencingEntityNavigationPropertyName    createdby
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class lk_teambase_createdby
                {
                    public const string Name = "lk_teambase_createdby";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_createdby = "createdby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";
                }

                ///<summary>
                /// N:1 - Relationship lk_teambase_modifiedby
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_teambase_modifiedby
                /// ReferencingEntityNavigationPropertyName    modifiedby
                /// IsCustomizable                             True                      False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class lk_teambase_modifiedby
                {
                    public const string Name = "lk_teambase_modifiedby";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";
                }

                ///<summary>
                /// N:1 - Relationship opportunity_Teams
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     opportunity_Teams
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_opportunity
                /// IsCustomizable                             True                             False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                ///</summary>
                public static partial class opportunity_teams
                {
                    public const string Name = "opportunity_Teams";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencedEntity_opportunity = "opportunity";

                    public const string ReferencedAttribute_opportunityid = "opportunityid";
                }

                ///<summary>
                /// N:1 - Relationship organization_teams
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_teams
                /// ReferencingEntityNavigationPropertyName    organizationid_organization
                /// IsCustomizable                             False                          False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class organization_teams
                {
                    public const string Name = "organization_teams";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_organizationid = "organizationid";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// N:1 - Relationship processstage_teams
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     processstage_teams
                /// ReferencingEntityNavigationPropertyName    stageid_processstage
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class processstage_teams
                {
                    public const string Name = "processstage_teams";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_stageid = "stageid";

                    public const string ReferencedEntity_processstage = "processstage";

                    public const string ReferencedAttribute_processstageid = "processstageid";
                }

                ///<summary>
                /// N:1 - Relationship queue_team
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     queue_team
                /// ReferencingEntityNavigationPropertyName    queueid
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class queue_team
                {
                    public const string Name = "queue_team";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_queueid = "queueid";

                    public const string ReferencedEntity_queue = "queue";

                    public const string ReferencedAttribute_queueid = "queueid";
                }

                ///<summary>
                /// N:1 - Relationship teamtemplate_Teams
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     teamtemplate_Teams
                /// ReferencingEntityNavigationPropertyName    associatedteamtemplateid
                /// IsCustomizable                             False                       False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                ///</summary>
                public static partial class teamtemplate_teams
                {
                    public const string Name = "teamtemplate_Teams";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_teamtemplateid = "teamtemplateid";

                    public const string ReferencedEntity_teamtemplate = "teamtemplate";

                    public const string ReferencedAttribute_teamtemplateid = "teamtemplateid";
                }

                ///<summary>
                /// N:1 - Relationship TransactionCurrency_Team
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     TransactionCurrency_Team
                /// ReferencingEntityNavigationPropertyName    transactioncurrencyid
                /// IsCustomizable                             False                       False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class transactioncurrency_team
                {
                    public const string Name = "TransactionCurrency_Team";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_transactioncurrencyid = "transactioncurrencyid";

                    public const string ReferencedEntity_transactioncurrency = "transactioncurrency";

                    public const string ReferencedAttribute_transactioncurrencyid = "transactioncurrencyid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {

                ///<summary>
                /// 1:N - Relationship ImportFile_Team
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     ImportFile_Team
                /// ReferencingEntityNavigationPropertyName    recordsownerid_team
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class importfile_team
                {
                    public const string Name = "ImportFile_Team";

                    public const string ReferencingEntity_importfile = "importfile";

                    public const string ReferencingAttribute_recordsownerid = "recordsownerid";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship lead_owning_team
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lead_owning_team
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class lead_owning_team
                {
                    public const string Name = "lead_owning_team";

                    public const string ReferencingEntity_lead = "lead";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship OwningTeam_postfollows
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     OwningTeam_postfollows
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class owningteam_postfollows
                {
                    public const string Name = "OwningTeam_postfollows";

                    public const string ReferencingEntity_postfollow = "postfollow";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_accounts
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_accounts
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_accounts
                {
                    public const string Name = "team_accounts";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_actioncardusersettings
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_actioncardusersettings
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                          False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_actioncardusersettings
                {
                    public const string Name = "team_actioncardusersettings";

                    public const string ReferencingEntity_actioncardusersettings = "actioncardusersettings";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_activity
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_activity
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_activity
                {
                    public const string Name = "team_activity";

                    public const string ReferencingEntity_activitypointer = "activitypointer";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_annotations
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_annotations
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_annotations
                {
                    public const string Name = "team_annotations";

                    public const string ReferencingEntity_annotation = "annotation";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_appointment
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_appointment
                /// ReferencingEntityNavigationPropertyName    owningteam_appointment
                /// IsCustomizable                             True                      False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_appointment
                {
                    public const string Name = "team_appointment";

                    public const string ReferencingEntity_appointment = "appointment";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_asyncoperation
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_asyncoperation
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_asyncoperation
                {
                    public const string Name = "team_asyncoperation";

                    public const string ReferencingEntity_asyncoperation = "asyncoperation";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship Team_AsyncOperations
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Team_AsyncOperations
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_team
                /// IsCustomizable                             False                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_asyncoperations
                {
                    public const string Name = "Team_AsyncOperations";

                    public const string ReferencingEntity_asyncoperation = "asyncoperation";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_bookableresource
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_bookableresource
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_bookableresource
                {
                    public const string Name = "team_bookableresource";

                    public const string ReferencingEntity_bookableresource = "bookableresource";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_bookableresourcebooking
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_bookableresourcebooking
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                            False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_bookableresourcebooking
                {
                    public const string Name = "team_bookableresourcebooking";

                    public const string ReferencingEntity_bookableresourcebooking = "bookableresourcebooking";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_bookableresourcebookingexchangesyncidmapping
                /// 
                /// PropertyName                               Value                                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_bookableresourcebookingexchangesyncidmapping
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                                                True
                /// IsCustomRelationship                       False
                /// IsManaged                                  False
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
                ///</summary>
                public static partial class team_bookableresourcebookingexchangesyncidmapping
                {
                    public const string Name = "team_bookableresourcebookingexchangesyncidmapping";

                    public const string ReferencingEntity_bookableresourcebookingexchangesyncidmapping = "bookableresourcebookingexchangesyncidmapping";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_bookableresourcebookingheader
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_bookableresourcebookingheader
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                                  False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_bookableresourcebookingheader
                {
                    public const string Name = "team_bookableresourcebookingheader";

                    public const string ReferencingEntity_bookableresourcebookingheader = "bookableresourcebookingheader";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_bookableresourcecategory
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_bookableresourcecategory
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                             False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_bookableresourcecategory
                {
                    public const string Name = "team_bookableresourcecategory";

                    public const string ReferencingEntity_bookableresourcecategory = "bookableresourcecategory";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_bookableresourcecategoryassn
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_bookableresourcecategoryassn
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                                 False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_bookableresourcecategoryassn
                {
                    public const string Name = "team_bookableresourcecategoryassn";

                    public const string ReferencingEntity_bookableresourcecategoryassn = "bookableresourcecategoryassn";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_bookableresourcecharacteristic
                /// 
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_bookableresourcecharacteristic
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                                   False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_bookableresourcecharacteristic
                {
                    public const string Name = "team_bookableresourcecharacteristic";

                    public const string ReferencingEntity_bookableresourcecharacteristic = "bookableresourcecharacteristic";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_bookableresourcegroup
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_bookableresourcegroup
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                          False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_bookableresourcegroup
                {
                    public const string Name = "team_bookableresourcegroup";

                    public const string ReferencingEntity_bookableresourcegroup = "bookableresourcegroup";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_bookingstatus
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_bookingstatus
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_bookingstatus
                {
                    public const string Name = "team_bookingstatus";

                    public const string ReferencingEntity_bookingstatus = "bookingstatus";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship Team_BulkDeleteFailures
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Team_BulkDeleteFailures
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_team
                /// IsCustomizable                             False                      False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_bulkdeletefailures
                {
                    public const string Name = "Team_BulkDeleteFailures";

                    public const string ReferencingEntity_bulkdeletefailure = "bulkdeletefailure";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_BulkOperation
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_BulkOperation
                /// ReferencingEntityNavigationPropertyName    owningteam_bulkoperation
                /// IsCustomizable                             False                       False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_bulkoperation
                {
                    public const string Name = "team_BulkOperation";

                    public const string ReferencingEntity_bulkoperation = "bulkoperation";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_campaignactivity
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_campaignactivity
                /// ReferencingEntityNavigationPropertyName    owningteam_campaignactivity
                /// IsCustomizable                             True                           False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_campaignactivity
                {
                    public const string Name = "team_campaignactivity";

                    public const string ReferencingEntity_campaignactivity = "campaignactivity";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_campaignresponse
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_campaignresponse
                /// ReferencingEntityNavigationPropertyName    owningteam_campaignresponse
                /// IsCustomizable                             True                           False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_campaignresponse
                {
                    public const string Name = "team_campaignresponse";

                    public const string ReferencingEntity_campaignresponse = "campaignresponse";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_Campaigns
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_Campaigns
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_campaigns
                {
                    public const string Name = "team_Campaigns";

                    public const string ReferencingEntity_campaign = "campaign";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_channelaccessprofile
                /// 
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_channelaccessprofile
                /// ReferencingEntityNavigationPropertyName    team_channelaccessprofile
                /// IsCustomizable                             True                         False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_channelaccessprofile
                {
                    public const string Name = "team_channelaccessprofile";

                    public const string ReferencingEntity_channelaccessprofile = "channelaccessprofile";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_characteristic
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_characteristic
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_characteristic
                {
                    public const string Name = "team_characteristic";

                    public const string ReferencingEntity_characteristic = "characteristic";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_connections1
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_connections1
                /// ReferencingEntityNavigationPropertyName    record1id_team
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_connections1
                {
                    public const string Name = "team_connections1";

                    public const string ReferencingEntity_connection = "connection";

                    public const string ReferencingAttribute_record1id = "record1id";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_connections2
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_connections2
                /// ReferencingEntityNavigationPropertyName    record2id_team
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_connections2
                {
                    public const string Name = "team_connections2";

                    public const string ReferencingEntity_connection = "connection";

                    public const string ReferencingAttribute_record2id = "record2id";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_contacts
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_contacts
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_contacts
                {
                    public const string Name = "team_contacts";

                    public const string ReferencingEntity_contact = "contact";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_convertrule
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_convertrule
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_convertrule
                {
                    public const string Name = "team_convertrule";

                    public const string ReferencingEntity_convertrule = "convertrule";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_customer_opportunity_roles
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_customer_opportunity_roles
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                              False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_customer_opportunity_roles
                {
                    public const string Name = "team_customer_opportunity_roles";

                    public const string ReferencingEntity_customeropportunityrole = "customeropportunityrole";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_customer_relationship
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_customer_relationship
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                         False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_customer_relationship
                {
                    public const string Name = "team_customer_relationship";

                    public const string ReferencingEntity_customerrelationship = "customerrelationship";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship Team_DuplicateBaseRecord
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Team_DuplicateBaseRecord
                /// ReferencingEntityNavigationPropertyName    baserecordid_team
                /// IsCustomizable                             False                       False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_duplicatebaserecord
                {
                    public const string Name = "Team_DuplicateBaseRecord";

                    public const string ReferencingEntity_duplicaterecord = "duplicaterecord";

                    public const string ReferencingAttribute_baserecordid = "baserecordid";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship Team_DuplicateMatchingRecord
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Team_DuplicateMatchingRecord
                /// ReferencingEntityNavigationPropertyName    duplicaterecordid_team
                /// IsCustomizable                             False                           False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_duplicatematchingrecord
                {
                    public const string Name = "Team_DuplicateMatchingRecord";

                    public const string ReferencingEntity_duplicaterecord = "duplicaterecord";

                    public const string ReferencingAttribute_duplicaterecordid = "duplicaterecordid";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_DuplicateRules
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_DuplicateRules
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_duplicaterules
                {
                    public const string Name = "team_DuplicateRules";

                    public const string ReferencingEntity_duplicaterule = "duplicaterule";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_DynamicPropertyInstance
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_DynamicPropertyInstance
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                            False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_dynamicpropertyinstance
                {
                    public const string Name = "team_DynamicPropertyInstance";

                    public const string ReferencingEntity_dynamicpropertyinstance = "dynamicpropertyinstance";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_email
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_email
                /// ReferencingEntityNavigationPropertyName    owningteam_email
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_email
                {
                    public const string Name = "team_email";

                    public const string ReferencingEntity_email = "email";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_email_templates
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_email_templates
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_email_templates
                {
                    public const string Name = "team_email_templates";

                    public const string ReferencingEntity_template = "template";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_emailserverprofile
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_emailserverprofile
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                       False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_emailserverprofile
                {
                    public const string Name = "team_emailserverprofile";

                    public const string ReferencingEntity_emailserverprofile = "emailserverprofile";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_entitlement
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_entitlement
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_entitlement
                {
                    public const string Name = "team_entitlement";

                    public const string ReferencingEntity_entitlement = "entitlement";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_exchangesyncidmapping
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_exchangesyncidmapping
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                         False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_exchangesyncidmapping
                {
                    public const string Name = "team_exchangesyncidmapping";

                    public const string ReferencingEntity_exchangesyncidmapping = "exchangesyncidmapping";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_externalparty
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_externalparty
                /// ReferencingEntityNavigationPropertyName    team_externalparty_externalparty
                /// IsCustomizable                             True                                False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_externalparty
                {
                    public const string Name = "team_externalparty";

                    public const string ReferencingEntity_externalparty = "externalparty";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_fax
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_fax
                /// ReferencingEntityNavigationPropertyName    owningteam_fax
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_fax
                {
                    public const string Name = "team_fax";

                    public const string ReferencingEntity_fax = "fax";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_goal
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_goal
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_goal
                {
                    public const string Name = "team_goal";

                    public const string ReferencingEntity_goal = "goal";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_goal_goalowner
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_goal_goalowner
                /// ReferencingEntityNavigationPropertyName    goalownerid_team
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_goal_goalowner
                {
                    public const string Name = "team_goal_goalowner";

                    public const string ReferencingEntity_goal = "goal";

                    public const string ReferencingAttribute_goalownerid = "goalownerid";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_goalrollupquery
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_goalrollupquery
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_goalrollupquery
                {
                    public const string Name = "team_goalrollupquery";

                    public const string ReferencingEntity_goalrollupquery = "goalrollupquery";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_ImportData
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_ImportData
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_importdata
                {
                    public const string Name = "team_ImportData";

                    public const string ReferencingEntity_importdata = "importdata";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_ImportFiles
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_ImportFiles
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_importfiles
                {
                    public const string Name = "team_ImportFiles";

                    public const string ReferencingEntity_importfile = "importfile";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_ImportLogs
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_ImportLogs
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_importlogs
                {
                    public const string Name = "team_ImportLogs";

                    public const string ReferencingEntity_importlog = "importlog";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_ImportMaps
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_ImportMaps
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_importmaps
                {
                    public const string Name = "team_ImportMaps";

                    public const string ReferencingEntity_importmap = "importmap";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_Imports
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_Imports
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_imports
                {
                    public const string Name = "team_Imports";

                    public const string ReferencingEntity_import = "import";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_incidentresolution
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_incidentresolution
                /// ReferencingEntityNavigationPropertyName    owningteam_incidentresolution
                /// IsCustomizable                             False                            False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_incidentresolution
                {
                    public const string Name = "team_incidentresolution";

                    public const string ReferencingEntity_incidentresolution = "incidentresolution";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_incidents
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_incidents
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_incidents
                {
                    public const string Name = "team_incidents";

                    public const string ReferencingEntity_incident = "incident";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_interactionforemail
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_new_interactionforemail
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                            False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_interactionforemail
                {
                    public const string Name = "team_interactionforemail";

                    public const string ReferencingEntity_interactionforemail = "interactionforemail";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_invoices
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_invoices
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_invoices
                {
                    public const string Name = "team_invoices";

                    public const string ReferencingEntity_invoice = "invoice";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_knowledgearticle
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_knowledgearticle
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_knowledgearticle
                {
                    public const string Name = "team_knowledgearticle";

                    public const string ReferencingEntity_knowledgearticle = "knowledgearticle";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_letter
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_letter
                /// ReferencingEntityNavigationPropertyName    owningteam_letter
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_letter
                {
                    public const string Name = "team_letter";

                    public const string ReferencingEntity_letter = "letter";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_list
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_list
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_list
                {
                    public const string Name = "team_list";

                    public const string ReferencingEntity_list = "list";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_mailbox
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_mailbox
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_mailbox
                {
                    public const string Name = "team_mailbox";

                    public const string ReferencingEntity_mailbox = "mailbox";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_mailboxtrackingfolder
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_mailboxtrackingfolder
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                          False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_mailboxtrackingfolder
                {
                    public const string Name = "team_mailboxtrackingfolder";

                    public const string ReferencingEntity_mailboxtrackingfolder = "mailboxtrackingfolder";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_msdyn_postalbum
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_msdyn_postalbum
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     True
                /// IsCustomRelationship                       False
                /// IsManaged                                  False
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
                public static partial class team_msdyn_postalbum
                {
                    public const string Name = "team_msdyn_postalbum";

                    public const string ReferencingEntity_msdyn_postalbum = "msdyn_postalbum";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_msdyn_wallsavedqueryusersettings
                /// 
                /// PropertyName                               Value                                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_msdyn_wallsavedqueryusersettings
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                                     True
                /// IsCustomRelationship                       False
                /// IsManaged                                  False
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
                public static partial class team_msdyn_wallsavedqueryusersettings
                {
                    public const string Name = "team_msdyn_wallsavedqueryusersettings";

                    public const string ReferencingEntity_msdyn_wallsavedqueryusersettings = "msdyn_wallsavedqueryusersettings";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_opportunities
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_opportunities
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_opportunities
                {
                    public const string Name = "team_opportunities";

                    public const string ReferencingEntity_opportunity = "opportunity";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_opportunityclose
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_opportunityclose
                /// ReferencingEntityNavigationPropertyName    owningteam_opportunityclose
                /// IsCustomizable                             False                          False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_opportunityclose
                {
                    public const string Name = "team_opportunityclose";

                    public const string ReferencingEntity_opportunityclose = "opportunityclose";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_orderclose
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_orderclose
                /// ReferencingEntityNavigationPropertyName    owningteam_orderclose
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_orderclose
                {
                    public const string Name = "team_orderclose";

                    public const string ReferencingEntity_orderclose = "orderclose";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_orders
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_orders
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_orders
                {
                    public const string Name = "team_orders";

                    public const string ReferencingEntity_salesorder = "salesorder";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_phonecall
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_phonecall
                /// ReferencingEntityNavigationPropertyName    owningteam_phonecall
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_phonecall
                {
                    public const string Name = "team_phonecall";

                    public const string ReferencingEntity_phonecall = "phonecall";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_PostRegardings
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_PostRegardings
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_team
                /// IsCustomizable                             False                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_postregardings
                {
                    public const string Name = "team_PostRegardings";

                    public const string ReferencingEntity_postregarding = "postregarding";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_PostRoles
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_PostRoles
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_team
                /// IsCustomizable                             False                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_postroles
                {
                    public const string Name = "team_PostRoles";

                    public const string ReferencingEntity_postrole = "postrole";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_principalobjectattributeaccess
                /// 
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_principalobjectattributeaccess
                /// ReferencingEntityNavigationPropertyName    objectid_team
                /// IsCustomizable                             False                                  False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_principalobjectattributeaccess
                {
                    public const string Name = "team_principalobjectattributeaccess";

                    public const string ReferencingEntity_principalobjectattributeaccess = "principalobjectattributeaccess";

                    public const string ReferencingAttribute_objectid = "objectid";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_principalobjectattributeaccess_principalid
                /// 
                /// PropertyName                               Value                                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_principalobjectattributeaccess_principalid
                /// ReferencingEntityNavigationPropertyName    principalid_team
                /// IsCustomizable                             False                                              False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_principalobjectattributeaccess_principalid
                {
                    public const string Name = "team_principalobjectattributeaccess_principalid";

                    public const string ReferencingEntity_principalobjectattributeaccess = "principalobjectattributeaccess";

                    public const string ReferencingAttribute_principalid = "principalid";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_processsession
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_processsession
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_processsession
                {
                    public const string Name = "team_processsession";

                    public const string ReferencingEntity_processsession = "processsession";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship Team_ProcessSessions
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Team_ProcessSessions
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_team
                /// IsCustomizable                             False                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_processsessions
                {
                    public const string Name = "Team_ProcessSessions";

                    public const string ReferencingEntity_processsession = "processsession";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_profilerule
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_profilerule
                /// ReferencingEntityNavigationPropertyName    teamid
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_profilerule
                {
                    public const string Name = "team_profilerule";

                    public const string ReferencingEntity_channelaccessprofilerule = "channelaccessprofilerule";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_queueitembase_workerid
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_queueitembase_workerid
                /// ReferencingEntityNavigationPropertyName    workerid_team
                /// IsCustomizable                             True                           False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_queueitembase_workerid
                {
                    public const string Name = "team_queueitembase_workerid";

                    public const string ReferencingEntity_queueitem = "queueitem";

                    public const string ReferencingAttribute_workerid = "workerid";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_quoteclose
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_quoteclose
                /// ReferencingEntityNavigationPropertyName    owningteam_quoteclose
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_quoteclose
                {
                    public const string Name = "team_quoteclose";

                    public const string ReferencingEntity_quoteclose = "quoteclose";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_quotes
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_quotes
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_quotes
                {
                    public const string Name = "team_quotes";

                    public const string ReferencingEntity_quote = "quote";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_ratingmodel
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_ratingmodel
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_ratingmodel
                {
                    public const string Name = "team_ratingmodel";

                    public const string ReferencingEntity_ratingmodel = "ratingmodel";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_ratingvalue
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_ratingvalue
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_ratingvalue
                {
                    public const string Name = "team_ratingvalue";

                    public const string ReferencingEntity_ratingvalue = "ratingvalue";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_recurringappointmentmaster
                /// 
                /// PropertyName                               Value                                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_recurringappointmentmaster
                /// ReferencingEntityNavigationPropertyName    owningteam_recurringappointmentmaster
                /// IsCustomizable                             True                                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_recurringappointmentmaster
                {
                    public const string Name = "team_recurringappointmentmaster";

                    public const string ReferencingEntity_recurringappointmentmaster = "recurringappointmentmaster";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_resource_groups
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_resource_groups
                /// ReferencingEntityNavigationPropertyName    resourcegroupid_team
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_resource_groups
                {
                    public const string Name = "team_resource_groups";

                    public const string ReferencingEntity_resourcegroup = "resourcegroup";

                    public const string ReferencingAttribute_resourcegroupid = "resourcegroupid";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_resource_specs
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_resource_specs
                /// ReferencingEntityNavigationPropertyName    groupobjectid_team
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_resource_specs
                {
                    public const string Name = "team_resource_specs";

                    public const string ReferencingEntity_resourcespec = "resourcespec";

                    public const string ReferencingAttribute_groupobjectid = "groupobjectid";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_routingrule
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_routingrule
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_routingrule
                {
                    public const string Name = "team_routingrule";

                    public const string ReferencingEntity_routingrule = "routingrule";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_routingruleitem
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_routingruleitem
                /// ReferencingEntityNavigationPropertyName    assignobjectid_team
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_routingruleitem
                {
                    public const string Name = "team_routingruleitem";

                    public const string ReferencingEntity_routingruleitem = "routingruleitem";

                    public const string ReferencingAttribute_assignobjectid = "assignobjectid";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_service_appointments
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_service_appointments
                /// ReferencingEntityNavigationPropertyName    owningteam_serviceappointment
                /// IsCustomizable                             True                             False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_service_appointments
                {
                    public const string Name = "team_service_appointments";

                    public const string ReferencingEntity_serviceappointment = "serviceappointment";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_service_contracts
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_service_contracts
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                      False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_service_contracts
                {
                    public const string Name = "team_service_contracts";

                    public const string ReferencingEntity_contract = "contract";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_sharepointdocumentlocation
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_sharepointdocumentlocation
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                               False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_sharepointdocumentlocation
                {
                    public const string Name = "team_sharepointdocumentlocation";

                    public const string ReferencingEntity_sharepointdocumentlocation = "sharepointdocumentlocation";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_sharepointsite
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_sharepointsite
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_sharepointsite
                {
                    public const string Name = "team_sharepointsite";

                    public const string ReferencingEntity_sharepointsite = "sharepointsite";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_slaBase
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_slaBase
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_slabase
                {
                    public const string Name = "team_slaBase";

                    public const string ReferencingEntity_sla = "sla";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_socialactivity
                /// 
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_socialactivity
                /// ReferencingEntityNavigationPropertyName    owningteam_socialactivity
                /// IsCustomizable                             True                         False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_socialactivity
                {
                    public const string Name = "team_socialactivity";

                    public const string ReferencingEntity_socialactivity = "socialactivity";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_SyncError
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_SyncError
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_syncerror
                {
                    public const string Name = "team_SyncError";

                    public const string ReferencingEntity_syncerror = "syncerror";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship Team_SyncErrors
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Team_SyncErrors
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_team_syncerror
                /// IsCustomizable                             True                                False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class team_syncerrors
                {
                    public const string Name = "Team_SyncErrors";

                    public const string ReferencingEntity_syncerror = "syncerror";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_task
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_task
                /// ReferencingEntityNavigationPropertyName    owningteam_task
                /// IsCustomizable                             True                     False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_task
                {
                    public const string Name = "team_task";

                    public const string ReferencingEntity_task = "task";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_userentityinstancedata
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_userentityinstancedata
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                          False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_userentityinstancedata
                {
                    public const string Name = "team_userentityinstancedata";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_userentityuisettings
                /// 
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_userentityuisettings
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                        False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_userentityuisettings
                {
                    public const string Name = "team_userentityuisettings";

                    public const string ReferencingEntity_userentityuisettings = "userentityuisettings";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_userform
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_userform
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_userform
                {
                    public const string Name = "team_userform";

                    public const string ReferencingEntity_userform = "userform";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_userquery
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_userquery
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_userquery
                {
                    public const string Name = "team_userquery";

                    public const string ReferencingEntity_userquery = "userquery";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_userqueryvisualizations
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_userqueryvisualizations
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                           False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_userqueryvisualizations
                {
                    public const string Name = "team_userqueryvisualizations";

                    public const string ReferencingEntity_userqueryvisualization = "userqueryvisualization";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_workflow
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_workflow
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_workflow
                {
                    public const string Name = "team_workflow";

                    public const string ReferencingEntity_workflow = "workflow";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship team_workflowlog
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_workflowlog
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                public static partial class team_workflowlog
                {
                    public const string Name = "team_workflowlog";

                    public const string ReferencingEntity_workflowlog = "workflowlog";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_team
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_team
                /// ReferencingEntityNavigationPropertyName    objectid_team
                /// IsCustomizable                             False                          False
                /// IsCustomRelationship                       False
                /// IsManaged                                  True
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
                ///</summary>
                public static partial class userentityinstancedata_team
                {
                    public const string Name = "userentityinstancedata_team";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";
                }
            }

            #endregion Relationship OneToMany - 1:N.

            #region Relationship ManyToMany - N:N.

            public static partial class ManyToMany
            {
                ///<summary>
                /// N:N - Relationship teammembership_association
                /// 
                /// PropertyName                                   Value                         CanBeChanged
                /// Entity1NavigationPropertyName                  teammembership_association
                /// Entity2NavigationPropertyName                  teammembership_association
                /// IsCustomizable                                 False                         False
                /// IsCustomRelationship                           False
                /// IsManaged                                      True
                /// IsValidForAdvancedFind                         True
                /// RelationshipType                               ManyToManyRelationship
                /// SecurityTypes                                  None
                /// Entity1AssociatedMenuConfiguration.Behavior    DoNotDisplay
                /// Entity1AssociatedMenuConfiguration.Group       Details
                /// Entity1AssociatedMenuConfiguration.Order       null
                /// Entity2AssociatedMenuConfiguration.Behavior    DoNotDisplay
                /// Entity2AssociatedMenuConfiguration.Group       Details
                /// Entity2AssociatedMenuConfiguration.Order       null
                ///</summary>
                public static partial class teammembership_association
                {
                    public const string Name = "teammembership_association";

                    public const string IntersectEntity_teammembership = "teammembership";

                    public const string Entity1_team = "team";

                    public const string Entity1Attribute_teamid = "teamid";

                    public const string Entity2_systemuser = "systemuser";

                    public const string Entity2Attribute_systemuserid = "systemuserid";
                }

                ///<summary>
                /// N:N - Relationship teamprofiles_association
                /// 
                /// PropertyName                                   Value                       CanBeChanged
                /// Entity1NavigationPropertyName                  teamprofiles_association
                /// Entity2NavigationPropertyName                  teamprofiles_association
                /// IsCustomizable                                 False                       False
                /// IsCustomRelationship                           False
                /// IsManaged                                      True
                /// IsValidForAdvancedFind                         True
                /// RelationshipType                               ManyToManyRelationship
                /// SecurityTypes                                  None
                /// Entity1AssociatedMenuConfiguration.Behavior    UseCollectionName
                /// Entity1AssociatedMenuConfiguration.Group       Details
                /// Entity1AssociatedMenuConfiguration.Order       null
                /// Entity2AssociatedMenuConfiguration.Behavior    UseCollectionName
                /// Entity2AssociatedMenuConfiguration.Group       Details
                /// Entity2AssociatedMenuConfiguration.Order       null
                ///</summary>
                public static partial class teamprofiles_association
                {
                    public const string Name = "teamprofiles_association";

                    public const string IntersectEntity_teamprofiles = "teamprofiles";

                    public const string Entity1_team = "team";

                    public const string Entity1Attribute_teamid = "teamid";

                    public const string Entity2_fieldsecurityprofile = "fieldsecurityprofile";

                    public const string Entity2Attribute_fieldsecurityprofileid = "fieldsecurityprofileid";
                }

                ///<summary>
                /// N:N - Relationship teamroles_association
                /// 
                /// PropertyName                                   Value                     CanBeChanged
                /// Entity1NavigationPropertyName                  teamroles_association
                /// Entity2NavigationPropertyName                  teamroles_association
                /// IsCustomizable                                 False                     False
                /// IsCustomRelationship                           False
                /// IsManaged                                      True
                /// IsValidForAdvancedFind                         True
                /// RelationshipType                               ManyToManyRelationship
                /// SecurityTypes                                  None
                /// Entity1AssociatedMenuConfiguration.Behavior    DoNotDisplay
                /// Entity1AssociatedMenuConfiguration.Group       Details
                /// Entity1AssociatedMenuConfiguration.Order       null
                /// Entity2AssociatedMenuConfiguration.Behavior    DoNotDisplay
                /// Entity2AssociatedMenuConfiguration.Group       Details
                /// Entity2AssociatedMenuConfiguration.Order       null
                ///</summary>
                public static partial class teamroles_association
                {
                    public const string Name = "teamroles_association";

                    public const string IntersectEntity_teamroles = "teamroles";

                    public const string Entity1_team = "team";

                    public const string Entity1Attribute_teamid = "teamid";

                    public const string Entity2_role = "role";

                    public const string Entity2Attribute_roleid = "roleid";
                }

                ///<summary>
                /// N:N - Relationship teamsyncattributemappingprofiles_association
                /// 
                /// PropertyName                                   Value                                           CanBeChanged
                /// Entity1NavigationPropertyName                  teamsyncattributemappingprofiles_association
                /// Entity2NavigationPropertyName                  teamsyncattributemappingprofiles_association
                /// IsCustomizable                                 False                                           False
                /// IsCustomRelationship                           False
                /// IsManaged                                      True
                /// IsValidForAdvancedFind                         True
                /// RelationshipType                               ManyToManyRelationship
                /// SecurityTypes                                  None
                /// Entity1AssociatedMenuConfiguration.Behavior    UseCollectionName
                /// Entity1AssociatedMenuConfiguration.Group       Details
                /// Entity1AssociatedMenuConfiguration.Order       null
                /// Entity2AssociatedMenuConfiguration.Behavior    UseCollectionName
                /// Entity2AssociatedMenuConfiguration.Group       Details
                /// Entity2AssociatedMenuConfiguration.Order       null
                ///</summary>
                public static partial class teamsyncattributemappingprofiles_association
                {
                    public const string Name = "teamsyncattributemappingprofiles_association";

                    public const string IntersectEntity_teamsyncattributemappingprofiles = "teamsyncattributemappingprofiles";

                    public const string Entity1_team = "team";

                    public const string Entity1Attribute_teamid = "teamid";

                    public const string Entity2_syncattributemappingprofile = "syncattributemappingprofile";

                    public const string Entity2Attribute_syncattributemappingprofileid = "syncattributemappingprofileid";
                }
            }

            #endregion Relationship ManyToMany - N:N.
        }
    }
}
