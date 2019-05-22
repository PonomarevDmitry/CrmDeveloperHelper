
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class DependencyNode
    {
        ///<summary>
        /// DisplayName:
        ///     (English - United States - 1033): Dependency Node
        ///     (Russian - 1049): Узел зависимости
        /// 
        /// DisplayCollectionName:
        ///     (English - United States - 1033): Dependency Nodes
        ///     (Russian - 1049): Узлы зависимости
        /// 
        /// Description:
        ///     (English - United States - 1033): The representation of a component dependency node in CRM.
        ///     (Russian - 1049): Представление узла зависимости компонентов в CRM.
        /// 
        /// PropertyName                          Value
        /// ActivityTypeMask                      0
        /// AutoCreateAccessTeams                 False
        /// AutoRouteToOwnerQueue                 False
        /// CanBeInManyToMany                     False
        /// CanBePrimaryEntityInRelationship      False
        /// CanBeRelatedEntityInRelationship      False
        /// CanChangeHierarchicalRelationship     False
        /// CanChangeTrackingBeEnabled            False
        /// CanCreateAttributes                   False
        /// CanCreateCharts                       False
        /// CanCreateForms                        False
        /// CanCreateViews                        False
        /// CanEnableSyncToExternalSearchIndex    False
        /// CanModifyAdditionalSettings           False
        /// CanTriggerWorkflow                    False
        /// ChangeTrackingEnabled                 False
        /// CollectionSchemaName                  DependencyNodes
        /// DataProviderId                        null
        /// DataSourceId                          null
        /// EnforceStateTransitions               False
        /// EntityHelpUrlEnabled                  False
        /// EntitySetName                         dependencynodes
        /// IntroducedVersion                     5.0.0.0
        /// IsAIRUpdated                          False
        /// IsActivity                            False
        /// IsActivityParty                       False
        /// IsAvailableOffline                    True
        /// IsBPFEntity                           False
        /// IsBusinessProcessEnabled              False
        /// IsChildEntity                         False
        /// IsConnectionsEnabled                  False
        /// IsCustomEntity                        False
        /// IsCustomizable                        False
        /// IsDocumentManagementEnabled           False
        /// IsDocumentRecommendationsEnabled      False
        /// IsDuplicateDetectionEnabled           False
        /// IsEnabledForCharts                    False
        /// IsEnabledForExternalChannels          False
        /// IsEnabledForTrace                     False
        /// IsImportable                          False
        /// IsInteractionCentricEnabled           False
        /// IsIntersect                           False
        /// IsKnowledgeManagementEnabled          False
        /// IsLogicalEntity                       False
        /// IsMailMergeEnabled                    False
        /// IsMappable                            False
        /// IsOfflineInMobileClient               False
        /// IsOneNoteIntegrationEnabled           False
        /// IsOptimisticConcurrencyEnabled        False
        /// IsPrivate                             True
        /// IsQuickCreateEnabled                  False
        /// IsReadOnlyInMobileClient              False
        /// IsReadingPaneEnabled                  True
        /// IsRenameable                          False
        /// IsSLAEnabled                          False
        /// IsStateModelAware                     False
        /// IsValidForAdvancedFind                False
        /// IsValidForQueue                       False
        /// IsVisibleInMobile                     False
        /// IsVisibleInMobileClient               False
        /// LogicalCollectionName                 dependencynodes
        /// LogicalName                           dependencynode
        /// ObjectTypeCode                        7106
        /// OwnershipType                         None
        /// PrimaryIdAttribute                    dependencynodeid
        /// SchemaName                            DependencyNode
        /// SyncToExternalSearchIndex             False
        /// UsesBusinessDataLabelTable            False
        ///</summary>
        public static partial class Schema
        {
            public const string EntityLogicalName = "dependencynode";

            public const string EntitySchemaName = "DependencyNode";

            public const string EntityPrimaryIdAttribute = "dependencynodeid";

            public const int EntityObjectTypeCode = 7106;

            #region Attributes.

            public static partial class Attributes
            {
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
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
                /// IsCustomizable                 False
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    True
                /// IsPrimaryName                  False
                /// IsRenameable                   False
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Dependency Node Identifier")]
                public const string dependencynodeid = "dependencynodeid";

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
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: solution
                /// 
                ///     Target solution    PrimaryIdAttribute solutionid    PrimaryNameAttribute friendlyname
                ///         DisplayName:
                ///             (English - United States - 1033): Solution
                ///             (Russian - 1049): Решение
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Solutions
                ///             (Russian - 1049): Решения
                ///         
                ///         Description:
                ///             (English - United States - 1033): A solution which contains CRM customizations.
                ///             (Russian - 1049): Решение, содержащее настройки CRM.
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
                /// IsCustomizable                 False
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   False
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Base Solution")]
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
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
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
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
                /// IsCustomizable                 False
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   False
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Type Code")]
                public const string componenttype = "componenttype";

                ///<summary>
                /// SchemaName: ComponentTypeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'componenttype'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
                /// IsCustomizable                 False
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   False
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                //public const string componenttypename = "componenttypename";

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
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = 0    MaxValue = 100    Precision = 5
                /// ImeMode = Disabled
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              6.1.0.0
                /// IsCustomizable                 False
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   False
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Introduced Version")]
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
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
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
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
                /// IsCustomizable                 False
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   False
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Shared Component")]
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
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
                /// IsCustomizable                 False
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   False
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Regarding")]
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
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
                /// IsCustomizable                 False
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   False
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Parent Entity")]
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
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: solution
                /// 
                ///     Target solution    PrimaryIdAttribute solutionid    PrimaryNameAttribute friendlyname
                ///         DisplayName:
                ///             (English - United States - 1033): Solution
                ///             (Russian - 1049): Решение
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Solutions
                ///             (Russian - 1049): Решения
                ///         
                ///         Description:
                ///             (English - United States - 1033): A solution which contains CRM customizations.
                ///             (Russian - 1049): Решение, содержащее настройки CRM.
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
                /// IsCustomizable                 False
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   False
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Top Solution")]
                public const string topsolutionid = "topsolutionid";

                ///<summary>
                /// SchemaName: VersionNumber
                /// BigIntAttributeMetadata    AttributeType: BigInt    AttributeTypeName: BigIntType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -9223372036854775808    MaxValue = 9223372036854775807
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              9.0.0.0
                /// IsCustomizable                 False
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   False
                /// IsRequiredForForm              False
                /// IsRetrievable                  True
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                public const string versionnumber = "versionnumber";
            }

            #endregion Attributes.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship solution_base_dependencynode
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     solution_base_dependencynode
                /// ReferencingEntityNavigationPropertyName    basesolutionid
                /// IsCustomizable                             False
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
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity solution:    PrimaryIdAttribute solutionid    PrimaryNameAttribute friendlyname
                ///     DisplayName:
                ///         (English - United States - 1033): Solution
                ///         (Russian - 1049): Решение
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Solutions
                ///         (Russian - 1049): Решения
                ///     
                ///     Description:
                ///         (English - United States - 1033): A solution which contains CRM customizations.
                ///         (Russian - 1049): Решение, содержащее настройки CRM.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship solution_base_dependencynode")]
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
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     solution_top_dependencynode
                /// ReferencingEntityNavigationPropertyName    topsolutionid
                /// IsCustomizable                             False
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
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity solution:    PrimaryIdAttribute solutionid    PrimaryNameAttribute friendlyname
                ///     DisplayName:
                ///         (English - United States - 1033): Solution
                ///         (Russian - 1049): Решение
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Solutions
                ///         (Russian - 1049): Решения
                ///     
                ///     Description:
                ///         (English - United States - 1033): A solution which contains CRM customizations.
                ///         (Russian - 1049): Решение, содержащее настройки CRM.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship solution_top_dependencynode")]
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
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     dependencynode_ancestor_dependency
                /// ReferencingEntityNavigationPropertyName    requiredcomponentnodeid
                /// IsCustomizable                             False
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
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity dependency:    PrimaryIdAttribute dependencyid
                ///     DisplayName:
                ///         (English - United States - 1033): Dependency
                ///         (Russian - 1049): Зависимость
                ///     
                ///     Description:
                ///         (English - United States - 1033): A component dependency in CRM.
                ///         (Russian - 1049): Зависимость компонента в CRM.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship dependencynode_ancestor_dependency")]
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
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     dependencynode_descendent_dependency
                /// ReferencingEntityNavigationPropertyName    dependentcomponentnodeid
                /// IsCustomizable                             False
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
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity dependency:    PrimaryIdAttribute dependencyid
                ///     DisplayName:
                ///         (English - United States - 1033): Dependency
                ///         (Russian - 1049): Зависимость
                ///     
                ///     Description:
                ///         (English - United States - 1033): A component dependency in CRM.
                ///         (Russian - 1049): Зависимость компонента в CRM.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship dependencynode_descendent_dependency")]
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
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_dependencynode
                /// ReferencingEntityNavigationPropertyName    objectid_dependencynode
                /// IsCustomizable                             False
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
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity userentityinstancedata:    PrimaryIdAttribute userentityinstancedataid
                ///     DisplayName:
                ///         (English - United States - 1033): User Entity Instance Data
                ///         (Russian - 1049): Данные экземпляра сущности пользователя
                ///     
                ///     Description:
                ///         (English - United States - 1033): Per User item instance data
                ///         (Russian - 1049): Данные экземпляра позиции "на пользователя"
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship userentityinstancedata_dependencynode")]
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