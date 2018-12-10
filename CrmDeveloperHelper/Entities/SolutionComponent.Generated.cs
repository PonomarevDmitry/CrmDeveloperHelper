
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): Solution Component
    /// (Russian - 1049): Компонент решения
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): Solution Components
    /// (Russian - 1049): Компоненты решения
    /// 
    /// Description:
    /// (English - United States - 1033): A component of a CRM solution.
    /// (Russian - 1049): Компонент решения CRM.
    /// 
    /// PropertyName                          Value                        CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False                        False
    /// CanBePrimaryEntityInRelationship      False                        False
    /// CanBeRelatedEntityInRelationship      False                        False
    /// CanChangeHierarchicalRelationship     False                        False
    /// CanChangeTrackingBeEnabled            False                        False
    /// CanCreateAttributes                   False                        False
    /// CanCreateCharts                       False                        False
    /// CanCreateForms                        False                        False
    /// CanCreateViews                        False                        False
    /// CanEnableSyncToExternalSearchIndex    False                        False
    /// CanModifyAdditionalSettings           False                        True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 False
    /// CollectionSchemaName                  SolutionComponents
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         solutioncomponents
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False                        False
    /// IsAvailableOffline                    True
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False                        False
    /// IsCustomEntity                        False
    /// IsCustomizable                        False                        False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                        False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                        False
    /// IsMappable                            False                        False
    /// IsOfflineInMobileClient               False                        True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False                        False
    /// IsRenameable                          False                        False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False                        False
    /// IsVisibleInMobile                     False                        False
    /// IsVisibleInMobileClient               False                        False
    /// LogicalCollectionName                 solutioncomponentss
    /// LogicalName                           solutioncomponent
    /// ObjectTypeCode                        7103
    /// OwnershipType                         None
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredSolutionComponent
    /// SchemaName                            SolutionComponent
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class SolutionComponent
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "solutioncomponent";

            public const string EntitySchemaName = "SolutionComponent";

            public const string EntityPrimaryIdAttribute = "solutioncomponentid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Object Type Code
                ///     (Russian - 1049): Код типа объекта
                /// 
                /// Description:
                ///     (English - United States - 1033): The object type code of the component.
                ///     (Russian - 1049): Код типа объекта компонента.
                /// 
                /// SchemaName: ComponentType
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Global System  OptionSet componenttype
                /// DefaultFormValue = Null
                ///</summary>
                public const string componenttype = "componenttype";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By
                ///     (Russian - 1049): Создано
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who created the solution
                ///     (Russian - 1049): Уникальный идентификатор пользователя, создавшего решение.
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
                ///     (English - United States - 1033): Date and time when the solution was created.
                ///     (Russian - 1049): Дата и время создания решения.
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who created the solution.
                ///     (Russian - 1049): Уникальный идентификатор пользователя-делегата, создавшего решение.
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
                ///     (English - United States - 1033): Is this component metadata
                ///     (Russian - 1049): Является ли компонент метаданными
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether this component is metadata or data.
                ///     (Russian - 1049): Указывает, является ли этот компонент метаданными или данными.
                /// 
                /// SchemaName: IsMetadata
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = True
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Data
                ///     (Russian - 1049): Данные
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Metadata
                ///     (Russian - 1049): Метаданные
                /// TrueOption = 1
                ///</summary>
                public const string ismetadata = "ismetadata";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By
                ///     (Russian - 1049): Изменено
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who last modified the solution.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, внесшего последнее изменение в решение.
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
                ///     (English - United States - 1033): Date and time when the solution was last modified.
                ///     (Russian - 1049): Дата и время последнего изменения решения.
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who modified the solution.
                ///     (Russian - 1049): Уникальный идентификатор делегата, изменившего решение.
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
                ///     (English - United States - 1033): Regarding
                ///     (Russian - 1049): В отношении
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the object with which the component is associated.
                ///     (Russian - 1049): Уникальный идентификатор объекта, с которым связан компонент.
                /// 
                /// SchemaName: ObjectId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string objectid = "objectid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Root Component Behavior
                ///     (Russian - 1049): Поведение корневого компонента
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates the include behavior of the root component.
                ///     (Russian - 1049): Указывает на поведение включения корневого компонента.
                /// 
                /// SchemaName: RootComponentBehavior
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet solutioncomponent_rootcomponentbehavior
                /// DefaultFormValue = -1
                ///</summary>
                public const string rootcomponentbehavior = "rootcomponentbehavior";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Root Solution Component ID
                ///     (Russian - 1049): Код корневого компонента решения
                /// 
                /// Description:
                ///     (English - United States - 1033): The parent ID of the subcomponent, which will be a root
                ///     (Russian - 1049): Родительский идентификатор подкомпонента, который будет корневым
                /// 
                /// SchemaName: RootSolutionComponentId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string rootsolutioncomponentid = "rootsolutioncomponentid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Solution Component Identifier
                ///     (Russian - 1049): Идентификатор компонента решения
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the solution component.
                ///     (Russian - 1049): Уникальный идентификатор компонента решения.
                /// 
                /// SchemaName: SolutionComponentId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string solutioncomponentid = "solutioncomponentid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Solution
                ///     (Russian - 1049): Решение
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the solution.
                ///     (Russian - 1049): Уникальный идентификатор решения.
                /// 
                /// SchemaName: SolutionId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: solution
                /// 
                ///     Target solution    PrimaryIdAttribute solutionid    PrimaryNameAttribute friendlyname
                ///         DisplayName:
                ///         (English - United States - 1033): Solution
                ///         (Russian - 1049): Решение
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Solutions
                ///         (Russian - 1049): Решения
                ///         
                ///         Description:
                ///         (English - United States - 1033): A solution which contains CRM customizations.
                ///         (Russian - 1049): Решение, содержащее настройки CRM.
                ///</summary>
                public const string solutionid = "solutionid";

                ///<summary>
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
                /// Attribute: rootcomponentbehavior
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Root Component Behavior
                ///     (Russian - 1049): Поведение корневого компонента
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates the include behavior of the root component.
                ///     (Russian - 1049): Указывает на поведение включения корневого компонента.
                /// 
                /// Local System  OptionSet solutioncomponent_rootcomponentbehavior
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Include Behavior
                ///     (Russian - 1049): Поведение включения
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates the include behavior of the root component.
                ///     (Russian - 1049): Указывает на поведение включения корневого компонента.
                ///</summary>
                public enum rootcomponentbehavior
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Include Subcomponents
                    ///     (Russian - 1049): Включить подкомпоненты
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Include_Subcomponents_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Do not include subcomponents
                    ///     (Russian - 1049): Не включать подкомпоненты
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Do_not_include_subcomponents_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Include As Shell Only
                    ///     (Russian - 1049): Включить только как оболочку
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Include_As_Shell_Only_2 = 2,
                }

                #endregion Picklist OptionSets.
            }

            #endregion OptionSets.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship lk_solutioncomponentbase_createdonbehalfby
                /// 
                /// PropertyName                               Value                                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_solutioncomponentbase_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    createdonbehalfby
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
                public static partial class lk_solutioncomponentbase_createdonbehalfby
                {
                    public const string Name = "lk_solutioncomponentbase_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_solutioncomponent = "solutioncomponent";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_solutioncomponentbase_modifiedonbehalfby
                /// 
                /// PropertyName                               Value                                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_solutioncomponentbase_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    modifiedonbehalfby
                /// IsCustomizable                             False                                          False
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
                public static partial class lk_solutioncomponentbase_modifiedonbehalfby
                {
                    public const string Name = "lk_solutioncomponentbase_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_solutioncomponent = "solutioncomponent";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship solution_solutioncomponent
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     solution_solutioncomponent
                /// ReferencingEntityNavigationPropertyName    solutionid
                /// IsCustomizable                             False                         False
                /// IsCustomRelationship                       False
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
                /// 
                /// ReferencedEntity solution:
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
                public static partial class solution_solutioncomponent
                {
                    public const string Name = "solution_solutioncomponent";

                    public const string ReferencedEntity_solution = "solution";

                    public const string ReferencedAttribute_solutionid = "solutionid";

                    public const string ReferencedEntity_PrimaryNameAttribute_friendlyname = "friendlyname";

                    public const string ReferencingEntity_solutioncomponent = "solutioncomponent";

                    public const string ReferencingAttribute_solutionid = "solutionid";
                }

                ///<summary>
                /// N:1 - Relationship solutioncomponent_parent_solutioncomponent
                /// 
                /// PropertyName                               Value                                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     solutioncomponent_parent_solutioncomponent
                /// ReferencingEntityNavigationPropertyName    rootsolutioncomponentid_solutioncomponent
                /// IsCustomizable                             False                                         False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                Restrict
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                ///</summary>
                public static partial class solutioncomponent_parent_solutioncomponent
                {
                    public const string Name = "solutioncomponent_parent_solutioncomponent";

                    public const string ReferencedEntity_solutioncomponent = "solutioncomponent";

                    public const string ReferencedAttribute_solutioncomponentid = "solutioncomponentid";

                    public const string ReferencingEntity_solutioncomponent = "solutioncomponent";

                    public const string ReferencingAttribute_rootsolutioncomponentid = "rootsolutioncomponentid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship solutioncomponent_parent_solutioncomponent
                /// 
                /// PropertyName                               Value                                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     solutioncomponent_parent_solutioncomponent
                /// ReferencingEntityNavigationPropertyName    rootsolutioncomponentid_solutioncomponent
                /// IsCustomizable                             False                                         False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                Restrict
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                ///</summary>
                public static partial class solutioncomponent_parent_solutioncomponent
                {
                    public const string Name = "solutioncomponent_parent_solutioncomponent";

                    public const string ReferencedEntity_solutioncomponent = "solutioncomponent";

                    public const string ReferencedAttribute_solutioncomponentid = "solutioncomponentid";

                    public const string ReferencingEntity_solutioncomponent = "solutioncomponent";

                    public const string ReferencingAttribute_rootsolutioncomponentid = "rootsolutioncomponentid";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_solutioncomponent
                /// 
                /// PropertyName                               Value                                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_solutioncomponent
                /// ReferencingEntityNavigationPropertyName    objectid_solutioncomponent
                /// IsCustomizable                             False                                       False
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
                public static partial class userentityinstancedata_solutioncomponent
                {
                    public const string Name = "userentityinstancedata_solutioncomponent";

                    public const string ReferencedEntity_solutioncomponent = "solutioncomponent";

                    public const string ReferencedAttribute_solutioncomponentid = "solutioncomponentid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}
