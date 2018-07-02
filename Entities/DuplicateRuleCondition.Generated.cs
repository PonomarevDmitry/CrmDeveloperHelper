
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): Duplicate Rule Condition
    /// (Russian - 1049): Условие правила обнаружения повторяющихся данных
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): Duplicate Rule Conditions
    /// (Russian - 1049): Условия правила обнаружения повторов
    /// 
    /// Description:
    /// (English - United States - 1033): Condition of a duplicate detection rule.
    /// (Russian - 1049): Условие правила обнаружения повторных записей.
    /// 
    /// PropertyName                          Value                      CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False                      False
    /// CanBePrimaryEntityInRelationship      False                      False
    /// CanBeRelatedEntityInRelationship      False                      False
    /// CanChangeHierarchicalRelationship     False                      False
    /// CanChangeTrackingBeEnabled            True                       True
    /// CanCreateAttributes                   False                      False
    /// CanCreateCharts                       False                      False
    /// CanCreateForms                        False                      False
    /// CanCreateViews                        False                      False
    /// CanEnableSyncToExternalSearchIndex    False                      False
    /// CanModifyAdditionalSettings           True                       True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 False
    /// CollectionSchemaName                  DuplicateRuleConditions
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         duplicateruleconditions
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False                      True
    /// IsAvailableOffline                    False
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         True
    /// IsConnectionsEnabled                  False                      False
    /// IsCustomEntity                        False
    /// IsCustomizable                        True                       False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                      False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                      False
    /// IsMappable                            False                      False
    /// IsOfflineInMobileClient               False                      True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        False
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False                      False
    /// IsRenameable                          False                      False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False                      False
    /// IsVisibleInMobile                     False                      False
    /// IsVisibleInMobileClient               False                      False
    /// LogicalCollectionName                 duplicateruleconditions
    /// LogicalName                           duplicaterulecondition
    /// ObjectTypeCode                        4416
    /// OwnershipType                         None
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName
    /// SchemaName                            DuplicateRuleCondition
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class DuplicateRuleCondition
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "duplicaterulecondition";

            public const string EntitySchemaName = "DuplicateRuleCondition";

            public const string EntityPrimaryIdAttribute = "duplicateruleconditionid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Base Field
                ///     (Russian - 1049): Базовое поле
                /// 
                /// Description:
                ///     (English - United States - 1033): Field that is being compared.
                ///     (Russian - 1049): Сравниваемое поле.
                /// 
                /// SchemaName: BaseAttributeName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string baseattributename = "baseattributename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By
                ///     (Russian - 1049): Создано
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who created the condition.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, создавшего условие.
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
                ///     (English - United States - 1033): Date and time when the condition was created.
                ///     (Russian - 1049): Дата и время создания условия.
                /// 
                /// SchemaName: CreatedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: SystemRequired
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who created the duplicate rule condition.
                ///     (Russian - 1049): Уникальный идентификатор делегированного пользователя, создавшего условие правила поиска повторяющихся данных.
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
                ///     (English - United States - 1033): Duplicate Rule Condition
                ///     (Russian - 1049): Условие правила обнаружения повторяющихся данных
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the condition.
                ///     (Russian - 1049): Уникальный идентификатор условия.
                /// 
                /// SchemaName: DuplicateRuleConditionId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string duplicateruleconditionid = "duplicateruleconditionid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Ignore Blank Values
                ///     (Russian - 1049): Пропуск пустых значений
                /// 
                /// Description:
                ///     (English - United States - 1033): Determines whether to consider blank values as non-duplicate values
                ///     (Russian - 1049): Определяет, рассматривать ли пустые значения как не являющиеся дубликатами
                /// 
                /// SchemaName: IgnoreBlankValues
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
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
                public const string ignoreblankvalues = "ignoreblankvalues";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Matching Field
                ///     (Russian - 1049): Совпадающее поле
                /// 
                /// Description:
                ///     (English - United States - 1033): Field that is being compared with the base field.
                ///     (Russian - 1049): Поле, сравниваемое с базовым полем.
                /// 
                /// SchemaName: MatchingAttributeName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string matchingattributename = "matchingattributename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By
                ///     (Russian - 1049): Изменено
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who last modified the condition.
                ///     (Russian - 1049): Уникальный идентификатор последнего пользователя, изменившего условие.
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
                ///     (English - United States - 1033): Date and time when the condition was last modified.
                ///     (Russian - 1049): Дата и время последнего изменения условия.
                /// 
                /// SchemaName: ModifiedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: SystemRequired
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who last modified the duplicate rule condition.
                ///     (Russian - 1049): Уникальный идентификатор последнего делегированного пользователя, изменившего условие правила поиска повторяющихся данных.
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
                ///     (English - United States - 1033): Operator Code
                ///     (Russian - 1049): Код оператора
                /// 
                /// Description:
                ///     (English - United States - 1033): Operator for this rule condition.
                ///     (Russian - 1049): Оператор для этого условия правила.
                /// 
                /// SchemaName: OperatorCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet duplicaterulecondition_operatorcode
                /// DefaultFormValue = -1
                ///</summary>
                public const string operatorcode = "operatorcode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Operator Parameter
                ///     (Russian - 1049): Параметр оператора
                /// 
                /// Description:
                ///     (English - United States - 1033): Parameter value of N if the operator is Same First Characters or Same Last Characters.
                ///     (Russian - 1049): Значение параметра N при использовании оператора одинаковых первых или последних символов.
                /// 
                /// SchemaName: OperatorParam
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 1    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string operatorparam = "operatorparam";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Owner
                ///     (Russian - 1049): Ответственный
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user or team who owns the duplicate rule condition.
                ///     (Russian - 1049): Уникальный идентификатор пользователя или рабочей группы, ответственных за условие правила поиска повторяющихся данных.
                /// 
                /// SchemaName: OwnerId
                /// LookupAttributeMetadata    AttributeType: Owner    AttributeTypeName: OwnerType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser,team
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
                public const string ownerid = "ownerid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Owning Business Unit
                ///     (Russian - 1049): Ответственное подразделение
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the business unit that owns the condition.
                ///     (Russian - 1049): Уникальный идентификатор подразделения, ответственного за условие.
                /// 
                /// SchemaName: OwningBusinessUnit
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string owningbusinessunit = "owningbusinessunit";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Owning User
                ///     (Russian - 1049): Ответственный пользователь
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who owns the condition.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, ответственного за условие.
                /// 
                /// SchemaName: OwningUser
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string owninguser = "owninguser";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Regarding
                ///     (Russian - 1049): В отношении
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the object with which the condition is associated.
                ///     (Russian - 1049): Уникальный идентификатор объекта, с которым связано условие.
                /// 
                /// SchemaName: RegardingObjectId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: duplicaterule
                /// 
                ///     Target duplicaterule    PrimaryIdAttribute duplicateruleid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Duplicate Detection Rule
                ///         (Russian - 1049): Правило обнаружения повторяющихся записей
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Duplicate Detection Rules
                ///         (Russian - 1049): Правила обнаружения повторяющихся записей
                ///         
                ///         Description:
                ///         (English - United States - 1033): Rule used to identify potential duplicates.
                ///         (Russian - 1049): Правило, используемое для определения возможных повторов.
                ///</summary>
                public const string regardingobjectid = "regardingobjectid";
            }

            #endregion Attributes.

            #region OptionSets.

            public static partial class OptionSets
            {

                #region Picklist OptionSet OptionSets.
                ///<summary>
                /// Attribute: operatorcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Operator Code
                ///     (Russian - 1049): Код оператора
                /// 
                /// Description:
                ///     (English - United States - 1033): Operator for this rule condition.
                ///     (Russian - 1049): Оператор для этого условия правила.
                /// 
                /// Local System  OptionSet duplicaterulecondition_operatorcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Operator Code
                ///     (Russian - 1049): Код оператора
                /// 
                /// Description:
                ///     (English - United States - 1033): Operator for this rule condition.
                ///     (Russian - 1049): Оператор для этого условия правила.
                ///</summary>
                public enum operatorcode
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Exact Match
                    ///     (Russian - 1049): Точное совпадение
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Exact_Match_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Same First Characters
                    ///     (Russian - 1049): Одинаковые первые знаки
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Same_First_Characters_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Same Last Characters
                    ///     (Russian - 1049): Одинаковые последние знаки
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Same_Last_Characters_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Same Date
                    ///     (Russian - 1049): Одинаковая дата
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Same_Date_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 5
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Same Date and Time
                    ///     (Russian - 1049): Одинаковая дата и время
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Same_Date_and_Time_4 = 4,

                    ///<summary>
                    /// 5
                    /// DisplayOrder: 6
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Exact Match (Pick List Label)
                    ///     (Russian - 1049): Точное соответствие (подпись в списке выбора)
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Exact_Match_Pick_List_Label_5 = 5,

                    ///<summary>
                    /// 6
                    /// DisplayOrder: 7
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Exact Match (Pick List Value)
                    ///     (Russian - 1049): Точное соответствие (значение в списке выбора)
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Exact_Match_Pick_List_Value_6 = 6,
                }

                #endregion Picklist OptionSets.
            }

            #endregion OptionSets.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship DuplicateRule_DuplicateRuleConditions
                /// 
                /// PropertyName                               Value                                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     DuplicateRule_DuplicateRuleConditions
                /// ReferencingEntityNavigationPropertyName    regardingobjectid
                /// IsCustomizable                             False                                    False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
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
                /// ReferencedEntity duplicaterule:
                ///     DisplayName:
                ///     (English - United States - 1033): Duplicate Detection Rule
                ///     (Russian - 1049): Правило обнаружения повторяющихся записей
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Duplicate Detection Rules
                ///     (Russian - 1049): Правила обнаружения повторяющихся записей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Rule used to identify potential duplicates.
                ///     (Russian - 1049): Правило, используемое для определения возможных повторов.
                ///</summary>
                public static partial class duplicaterule_duplicateruleconditions
                {
                    public const string Name = "DuplicateRule_DuplicateRuleConditions";

                    public const string ReferencedEntity_duplicaterule = "duplicaterule";

                    public const string ReferencedAttribute_duplicateruleid = "duplicateruleid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_duplicaterulecondition = "duplicaterulecondition";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";
                }

                ///<summary>
                /// N:1 - Relationship lk_duplicaterulecondition_createdonbehalfby
                /// 
                /// PropertyName                               Value                                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_duplicaterulecondition_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    createdonbehalfby
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
                public static partial class lk_duplicaterulecondition_createdonbehalfby
                {
                    public const string Name = "lk_duplicaterulecondition_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_duplicaterulecondition = "duplicaterulecondition";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_duplicaterulecondition_modifiedonbehalfby
                /// 
                /// PropertyName                               Value                                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_duplicaterulecondition_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    modifiedonbehalfby
                /// IsCustomizable                             False                                           False
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
                public static partial class lk_duplicaterulecondition_modifiedonbehalfby
                {
                    public const string Name = "lk_duplicaterulecondition_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_duplicaterulecondition = "duplicaterulecondition";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_duplicateruleconditionbase_createdby
                /// 
                /// PropertyName                               Value                                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_duplicateruleconditionbase_createdby
                /// ReferencingEntityNavigationPropertyName    createdby
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
                public static partial class lk_duplicateruleconditionbase_createdby
                {
                    public const string Name = "lk_duplicateruleconditionbase_createdby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_duplicaterulecondition = "duplicaterulecondition";

                    public const string ReferencingAttribute_createdby = "createdby";
                }

                ///<summary>
                /// N:1 - Relationship lk_duplicateruleconditionbase_modifiedby
                /// 
                /// PropertyName                               Value                                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_duplicateruleconditionbase_modifiedby
                /// ReferencingEntityNavigationPropertyName    modifiedby
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
                public static partial class lk_duplicateruleconditionbase_modifiedby
                {
                    public const string Name = "lk_duplicateruleconditionbase_modifiedby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_duplicaterulecondition = "duplicaterulecondition";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship DuplicateRuleCondition_SyncErrors
                /// 
                /// PropertyName                               Value                                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     DuplicateRuleCondition_SyncErrors
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_duplicaterulecondition_syncerror
                /// IsCustomizable                             True                                                  False
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
                public static partial class duplicaterulecondition_syncerrors
                {
                    public const string Name = "DuplicateRuleCondition_SyncErrors";

                    public const string ReferencedEntity_duplicaterulecondition = "duplicaterulecondition";

                    public const string ReferencedAttribute_duplicateruleconditionid = "duplicateruleconditionid";

                    public const string ReferencingEntity_syncerror = "syncerror";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_duplicaterulecondition
                /// 
                /// PropertyName                               Value                                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_duplicaterulecondition
                /// ReferencingEntityNavigationPropertyName    objectid_duplicaterulecondition
                /// IsCustomizable                             False                                            False
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
                public static partial class userentityinstancedata_duplicaterulecondition
                {
                    public const string Name = "userentityinstancedata_duplicaterulecondition";

                    public const string ReferencedEntity_duplicaterulecondition = "duplicaterulecondition";

                    public const string ReferencedAttribute_duplicateruleconditionid = "duplicateruleconditionid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}
