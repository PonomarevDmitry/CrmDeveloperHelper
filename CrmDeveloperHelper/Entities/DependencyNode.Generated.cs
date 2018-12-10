
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): Dependency Node
    /// (Russian - 1049): Узел зависимости
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): Dependency Nodes
    /// (Russian - 1049): Узлы зависимости
    /// 
    /// Description:
    /// (English - United States - 1033): The representation of a component dependency node in CRM.
    /// (Russian - 1049): Представление узла зависимости компонентов в CRM.
    /// 
    /// PropertyName                          Value              CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False              False
    /// CanBePrimaryEntityInRelationship      False              False
    /// CanBeRelatedEntityInRelationship      False              False
    /// CanChangeHierarchicalRelationship     False              False
    /// CanChangeTrackingBeEnabled            False              False
    /// CanCreateAttributes                   False              False
    /// CanCreateCharts                       False              False
    /// CanCreateForms                        False              False
    /// CanCreateViews                        False              False
    /// CanEnableSyncToExternalSearchIndex    False              False
    /// CanModifyAdditionalSettings           False              True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 False
    /// CollectionSchemaName                  DependencyNodes
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         dependencynodes
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False              False
    /// IsAvailableOffline                    False
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False              False
    /// IsCustomEntity                        False
    /// IsCustomizable                        False              False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False              False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False              False
    /// IsMappable                            False              False
    /// IsOfflineInMobileClient               False              True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        False
    /// IsPrivate                             True
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False              False
    /// IsRenameable                          False              False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False              False
    /// IsVisibleInMobile                     False              False
    /// IsVisibleInMobileClient               False              False
    /// LogicalCollectionName                 dependencynodes
    /// LogicalName                           dependencynode
    /// ObjectTypeCode                        7106
    /// OwnershipType                         None
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName
    /// SchemaName                            DependencyNode
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class DependencyNode
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "dependencynode";

            public const string EntitySchemaName = "DependencyNode";

            public const string EntityPrimaryIdAttribute = "dependencynodeid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Base Solution
                ///     (Russian - 1049): Базовое решение
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who created the solution
                ///     (Russian - 1049): Уникальный идентификатор пользователя, создавшего решение.
                /// 
                /// SchemaName: BaseSolutionId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string basesolutionid = "basesolutionid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Type Code
                ///     (Russian - 1049): Код типа
                /// 
                /// Description:
                ///     (English - United States - 1033): The type code of the component.
                ///     (Russian - 1049): Код типа компонента.
                /// 
                /// SchemaName: ComponentType
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Global System  OptionSet componenttype
                /// DefaultFormValue = Null
                /// 
                ///         DisplayName:
                ///             (English - United States - 1033): Component Type
                ///             (Russian - 1049): Тип компонента
                ///         
                ///         Description:
                ///             (English - United States - 1033): All of the possible component types for solutions.
                ///             (Russian - 1049): Все возможные типы компонентов для решений.
                ///</summary>
                public const string componenttype = "componenttype";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Dependency Node Identifier
                ///     (Russian - 1049): Идентификатор узла зависимости
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the dependency node.
                ///     (Russian - 1049): Уникальный идентификатор узла зависимости.
                /// 
                /// SchemaName: DependencyNodeId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string dependencynodeid = "dependencynodeid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Introduced Version
                ///     (Russian - 1049): Версия введения
                /// 
                /// Description:
                ///     (English - United States - 1033): Introduced version for the component
                ///     (Russian - 1049): Введенная версия компонента
                /// 
                /// SchemaName: IntroducedVersion
                /// DoubleAttributeMetadata    AttributeType: Double    AttributeTypeName: DoubleType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = 0    MaxValue = 100    Precision = 5
                /// ImeMode = Disabled
                ///</summary>
                public const string introducedversion = "introducedversion";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Shared Component
                ///     (Russian - 1049): Общий компонент
                /// 
                /// Description:
                ///     (English - United States - 1033): Whether this component is shared by two solutions with the same publisher.
                ///     (Russian - 1049): Указывает, является ли этот компонент общим для двух решений, имеющих одного издателя.
                /// 
                /// SchemaName: IsSharedComponent
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
                public const string issharedcomponent = "issharedcomponent";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Regarding
                ///     (Russian - 1049): В отношении
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the object with which the node is associated.
                ///     (Russian - 1049): Уникальный идентификатор объекта, с которым связан узел.
                /// 
                /// SchemaName: ObjectId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string objectid = "objectid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Parent Entity
                ///     (Russian - 1049): Родительская сущность
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the parent entity.
                ///     (Russian - 1049): Уникальный идентификатор родительской сущности.
                /// 
                /// SchemaName: ParentId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string parentid = "parentid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Top Solution
                ///     (Russian - 1049): Лучшее решение
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the top solution.
                ///     (Russian - 1049): Уникальный идентификатор лучшего решения.
                /// 
                /// SchemaName: TopSolutionId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string topsolutionid = "topsolutionid";
            }

            #endregion Attributes.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship solution_base_dependencynode
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     solution_base_dependencynode
                /// ReferencingEntityNavigationPropertyName    basesolutionid
                /// IsCustomizable                             False                           False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
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
                public static partial class solution_base_dependencynode
                {
                    public const string Name = "solution_base_dependencynode";

                    public const string ReferencedEntity_solution = "solution";

                    public const string ReferencedAttribute_solutionid = "solutionid";

                    public const string ReferencedEntity_PrimaryNameAttribute_friendlyname = "friendlyname";

                    public const string ReferencingEntity_dependencynode = "dependencynode";

                    public const string ReferencingAttribute_basesolutionid = "basesolutionid";
                }

                ///<summary>
                /// N:1 - Relationship solution_top_dependencynode
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     solution_top_dependencynode
                /// ReferencingEntityNavigationPropertyName    topsolutionid
                /// IsCustomizable                             False                          False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                RemoveLink
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
                public static partial class solution_top_dependencynode
                {
                    public const string Name = "solution_top_dependencynode";

                    public const string ReferencedEntity_solution = "solution";

                    public const string ReferencedAttribute_solutionid = "solutionid";

                    public const string ReferencedEntity_PrimaryNameAttribute_friendlyname = "friendlyname";

                    public const string ReferencingEntity_dependencynode = "dependencynode";

                    public const string ReferencingAttribute_topsolutionid = "topsolutionid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship dependencynode_ancestor_dependency
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     dependencynode_ancestor_dependency
                /// ReferencingEntityNavigationPropertyName    requiredcomponentnodeid
                /// IsCustomizable                             False                                 False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
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
                /// ReferencingEntity dependency:
                ///     DisplayName:
                ///     (English - United States - 1033): Dependency
                ///     (Russian - 1049): Зависимость
                ///     
                ///     Description:
                ///     (English - United States - 1033): A component dependency in CRM.
                ///     (Russian - 1049): Зависимость компонента в CRM.
                ///</summary>
                public static partial class dependencynode_ancestor_dependency
                {
                    public const string Name = "dependencynode_ancestor_dependency";

                    public const string ReferencedEntity_dependencynode = "dependencynode";

                    public const string ReferencedAttribute_dependencynodeid = "dependencynodeid";

                    public const string ReferencingEntity_dependency = "dependency";

                    public const string ReferencingAttribute_requiredcomponentnodeid = "requiredcomponentnodeid";
                }

                ///<summary>
                /// 1:N - Relationship dependencynode_descendent_dependency
                /// 
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     dependencynode_descendent_dependency
                /// ReferencingEntityNavigationPropertyName    dependentcomponentnodeid
                /// IsCustomizable                             False                                   False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
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
                /// ReferencingEntity dependency:
                ///     DisplayName:
                ///     (English - United States - 1033): Dependency
                ///     (Russian - 1049): Зависимость
                ///     
                ///     Description:
                ///     (English - United States - 1033): A component dependency in CRM.
                ///     (Russian - 1049): Зависимость компонента в CRM.
                ///</summary>
                public static partial class dependencynode_descendent_dependency
                {
                    public const string Name = "dependencynode_descendent_dependency";

                    public const string ReferencedEntity_dependencynode = "dependencynode";

                    public const string ReferencedAttribute_dependencynodeid = "dependencynodeid";

                    public const string ReferencingEntity_dependency = "dependency";

                    public const string ReferencingAttribute_dependentcomponentnodeid = "dependentcomponentnodeid";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_dependencynode
                /// 
                /// PropertyName                               Value                                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_dependencynode
                /// ReferencingEntityNavigationPropertyName    objectid_dependencynode
                /// IsCustomizable                             False                                    False
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
                ///     Description:
                ///     (English - United States - 1033): Per User item instance data
                ///     (Russian - 1049): Данные экземпляра позиции "на пользователя"
                ///</summary>
                public static partial class userentityinstancedata_dependencynode
                {
                    public const string Name = "userentityinstancedata_dependencynode";

                    public const string ReferencedEntity_dependencynode = "dependencynode";

                    public const string ReferencedAttribute_dependencynodeid = "dependencynodeid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}