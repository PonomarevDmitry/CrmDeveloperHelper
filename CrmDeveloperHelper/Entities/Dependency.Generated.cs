
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): Dependency
    /// (Russian - 1049): Зависимость
    /// 
    /// DisplayCollectionName:
    /// 
    /// Description:
    /// (English - United States - 1033): A component dependency in CRM.
    /// (Russian - 1049): Зависимость компонента в CRM.
    /// 
    /// PropertyName                          Value           CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False           False
    /// CanBePrimaryEntityInRelationship      False           False
    /// CanBeRelatedEntityInRelationship      False           False
    /// CanChangeHierarchicalRelationship     False           False
    /// CanChangeTrackingBeEnabled            False           False
    /// CanCreateAttributes                   False           False
    /// CanCreateCharts                       False           False
    /// CanCreateForms                        False           False
    /// CanCreateViews                        False           False
    /// CanEnableSyncToExternalSearchIndex    False           False
    /// CanModifyAdditionalSettings           False           True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 False
    /// CollectionSchemaName                  Dependency
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         dependencies
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False           False
    /// IsAvailableOffline                    False
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False           False
    /// IsCustomEntity                        False
    /// IsCustomizable                        False           False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False           False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False           False
    /// IsMappable                            False           False
    /// IsOfflineInMobileClient               False           True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        False
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False           False
    /// IsRenameable                          False           False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False           False
    /// IsVisibleInMobile                     False           False
    /// IsVisibleInMobileClient               False           False
    /// LogicalCollectionName                 dependencies
    /// LogicalName                           dependency
    /// ObjectTypeCode                        7105
    /// OwnershipType                         None
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName
    /// SchemaName                            Dependency
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class Dependency
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "dependency";

            public const string EntitySchemaName = "Dependency";

            public const string EntityPrimaryIdAttribute = "dependencyid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Dependency Identifier
                ///     (Russian - 1049): Идентификатор зависимости
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of a dependency.
                ///     (Russian - 1049): Уникальный идентификатор зависимости.
                /// 
                /// SchemaName: DependencyId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string dependencyid = "dependencyid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Dependency Type
                ///     (Russian - 1049): Тип зависимости
                /// 
                /// Description:
                ///     (English - United States - 1033): The dependency type of the dependency.
                ///     (Russian - 1049): Тип зависимости.
                /// 
                /// SchemaName: DependencyType
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Global System  OptionSet dependencytype
                /// DefaultFormValue = Null
                ///</summary>
                public const string dependencytype = "dependencytype";

                ///<summary>
                /// SchemaName: DependentComponentBaseSolutionId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string dependentcomponentbasesolutionid = "dependentcomponentbasesolutionid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Dependent Component
                ///     (Russian - 1049): Зависимый компонент
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the dependent component's node.
                ///     (Russian - 1049): Уникальный идентификатор узла зависимого компонента.
                /// 
                /// SchemaName: DependentComponentNodeId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: dependencynode
                /// 
                ///     Target dependencynode    PrimaryIdAttribute dependencynodeid
                ///         DisplayName:
                ///         (English - United States - 1033): Dependency Node
                ///         (Russian - 1049): Узел зависимости
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Dependency Nodes
                ///         (Russian - 1049): Узлы зависимости
                ///         
                ///         Description:
                ///         (English - United States - 1033): The representation of a component dependency node in CRM.
                ///         (Russian - 1049): Представление узла зависимости компонентов в CRM.
                ///</summary>
                public const string dependentcomponentnodeid = "dependentcomponentnodeid";

                ///<summary>
                /// SchemaName: DependentComponentObjectId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string dependentcomponentobjectid = "dependentcomponentobjectid";

                ///<summary>
                /// SchemaName: DependentComponentParentId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string dependentcomponentparentid = "dependentcomponentparentid";

                ///<summary>
                /// SchemaName: DependentComponentType
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Global System  OptionSet componenttype
                /// DefaultFormValue = Null
                ///</summary>
                public const string dependentcomponenttype = "dependentcomponenttype";

                ///<summary>
                /// SchemaName: RequiredComponentBaseSolutionId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string requiredcomponentbasesolutionid = "requiredcomponentbasesolutionid";

                ///<summary>
                /// SchemaName: RequiredComponentIntroducedVersion
                /// DoubleAttributeMetadata    AttributeType: Double    AttributeTypeName: DoubleType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = 0    MaxValue = 1000000000    Precision = 2
                /// ImeMode = Disabled
                ///</summary>
                public const string requiredcomponentintroducedversion = "requiredcomponentintroducedversion";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Required Component
                ///     (Russian - 1049): Требуемый компонент
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the required component's node
                ///     (Russian - 1049): Уникальный идентификатор узла требуемого компонента
                /// 
                /// SchemaName: RequiredComponentNodeId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: dependencynode
                /// 
                ///     Target dependencynode    PrimaryIdAttribute dependencynodeid
                ///         DisplayName:
                ///         (English - United States - 1033): Dependency Node
                ///         (Russian - 1049): Узел зависимости
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Dependency Nodes
                ///         (Russian - 1049): Узлы зависимости
                ///         
                ///         Description:
                ///         (English - United States - 1033): The representation of a component dependency node in CRM.
                ///         (Russian - 1049): Представление узла зависимости компонентов в CRM.
                ///</summary>
                public const string requiredcomponentnodeid = "requiredcomponentnodeid";

                ///<summary>
                /// SchemaName: RequiredComponentObjectId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string requiredcomponentobjectid = "requiredcomponentobjectid";

                ///<summary>
                /// SchemaName: RequiredComponentParentId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string requiredcomponentparentid = "requiredcomponentparentid";

                ///<summary>
                /// SchemaName: RequiredComponentType
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Global System  OptionSet componenttype
                /// DefaultFormValue = Null
                ///</summary>
                public const string requiredcomponenttype = "requiredcomponenttype";
            }

            #endregion Attributes.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship dependencynode_ancestor_dependency
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
                /// ReferencedEntity dependencynode:
                ///     DisplayName:
                ///     (English - United States - 1033): Dependency Node
                ///     (Russian - 1049): Узел зависимости
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Dependency Nodes
                ///     (Russian - 1049): Узлы зависимости
                ///     
                ///     Description:
                ///     (English - United States - 1033): The representation of a component dependency node in CRM.
                ///     (Russian - 1049): Представление узла зависимости компонентов в CRM.
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
                /// N:1 - Relationship dependencynode_descendent_dependency
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
                /// ReferencedEntity dependencynode:
                ///     DisplayName:
                ///     (English - United States - 1033): Dependency Node
                ///     (Russian - 1049): Узел зависимости
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Dependency Nodes
                ///     (Russian - 1049): Узлы зависимости
                ///     
                ///     Description:
                ///     (English - United States - 1033): The representation of a component dependency node in CRM.
                ///     (Russian - 1049): Представление узла зависимости компонентов в CRM.
                ///</summary>
                public static partial class dependencynode_descendent_dependency
                {
                    public const string Name = "dependencynode_descendent_dependency";

                    public const string ReferencedEntity_dependencynode = "dependencynode";

                    public const string ReferencedAttribute_dependencynodeid = "dependencynodeid";

                    public const string ReferencingEntity_dependency = "dependency";

                    public const string ReferencingAttribute_dependentcomponentnodeid = "dependentcomponentnodeid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship userentityinstancedata_dependency
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_dependency
                /// ReferencingEntityNavigationPropertyName    objectid_dependency
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
                public static partial class userentityinstancedata_dependency
                {
                    public const string Name = "userentityinstancedata_dependency";

                    public const string ReferencedEntity_dependency = "dependency";

                    public const string ReferencedAttribute_dependencyid = "dependencyid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}
