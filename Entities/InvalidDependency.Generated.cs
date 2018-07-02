
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): Invalid Dependency
    /// (Russian - 1049): Неверная зависимость
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): Invalid Dependencies
    /// (Russian - 1049): Неверные зависимости
    /// 
    /// Description:
    /// (English - United States - 1033): An invalid dependency in the CRM system.
    /// (Russian - 1049): Неверная зависимость в системе CRM.
    /// 
    /// PropertyName                          Value                  CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False                  False
    /// CanBePrimaryEntityInRelationship      False                  False
    /// CanBeRelatedEntityInRelationship      False                  False
    /// CanChangeHierarchicalRelationship     False                  False
    /// CanChangeTrackingBeEnabled            False                  False
    /// CanCreateAttributes                   False                  False
    /// CanCreateCharts                       False                  False
    /// CanCreateForms                        False                  False
    /// CanCreateViews                        False                  False
    /// CanEnableSyncToExternalSearchIndex    False                  False
    /// CanModifyAdditionalSettings           False                  True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 False
    /// CollectionSchemaName                  InvalidDependencies
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         invaliddependencies
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False                  False
    /// IsAvailableOffline                    False
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False                  False
    /// IsCustomEntity                        False
    /// IsCustomizable                        False                  False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                  False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                  False
    /// IsMappable                            False                  False
    /// IsOfflineInMobileClient               False                  True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        False
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False                  False
    /// IsRenameable                          False                  False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False                  False
    /// IsVisibleInMobile                     False                  False
    /// IsVisibleInMobileClient               False                  False
    /// LogicalCollectionName                 invaliddependencies
    /// LogicalName                           invaliddependency
    /// ObjectTypeCode                        7107
    /// OwnershipType                         None
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName
    /// SchemaName                            InvalidDependency
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class InvalidDependency
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "invaliddependency";

            public const string EntitySchemaName = "InvalidDependency";

            public const string EntityPrimaryIdAttribute = "invaliddependencyid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Existing Object Id
                ///     (Russian - 1049): Существующий идентификатор объекта
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the object that has an invalid dependency
                ///     (Russian - 1049): Уникальный идентификатор объекта с неверной зависимостью.
                /// 
                /// SchemaName: ExistingComponentId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string existingcomponentid = "existingcomponentid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Existing Object's Component Type
                ///     (Russian - 1049): Существующий тип компонента объекта
                /// 
                /// Description:
                ///     (English - United States - 1033): Component type of the object that has an invalid dependency
                ///     (Russian - 1049): Тип компонента объекта с неверной зависимостью
                /// 
                /// SchemaName: ExistingComponentType
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
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
                public const string existingcomponenttype = "existingcomponenttype";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Weight
                ///     (Russian - 1049): Вес
                /// 
                /// Description:
                ///     (English - United States - 1033): The dependency type of the invalid dependency.
                ///     (Russian - 1049): Тип неверной зависимости.
                /// 
                /// SchemaName: ExistingDependencyType
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Global System  OptionSet dependencytype
                /// DefaultFormValue = Null
                /// 
                ///         DisplayName:
                ///             (English - United States - 1033): Dependency Type
                ///             (Russian - 1049): Тип зависимости
                ///         
                ///         Description:
                ///             (English - United States - 1033): The kind of dependency.
                ///             (Russian - 1049): Вид зависимости.
                ///</summary>
                public const string existingdependencytype = "existingdependencytype";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Invalid Dependency Identifier
                ///     (Russian - 1049): Идентификатор неверной зависимости
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the invalid dependency.
                ///     (Russian - 1049): Уникальный идентификатор неверной зависимости.
                /// 
                /// SchemaName: InvalidDependencyId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string invaliddependencyid = "invaliddependencyid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is this node the required component
                ///     (Russian - 1049): Является ли узел требуемым компонентом
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the existing node is the required component in the dependency
                ///     (Russian - 1049): Указывает, является ли существующий узел требуемым компонентом в зависимости
                /// 
                /// SchemaName: IsExistingNodeRequiredComponent
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = True
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Descendent
                ///     (Russian - 1049): Потомок
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Ancestor
                ///     (Russian - 1049): Предок
                /// TrueOption = 1
                ///</summary>
                public const string isexistingnoderequiredcomponent = "isexistingnoderequiredcomponent";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Regarding
                ///     (Russian - 1049): В отношении
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the missing component.
                ///     (Russian - 1049): Уникальный идентификатор отсутствующего компонента.
                /// 
                /// SchemaName: MissingComponentId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string missingcomponentid = "missingcomponentid";

                ///<summary>
                /// SchemaName: MissingComponentInfo
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1073741823
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string missingcomponentinfo = "missingcomponentinfo";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Lookup Type
                ///     (Russian - 1049): Тип поиска
                /// 
                /// Description:
                ///     (English - United States - 1033): The lookup type of the missing component.
                ///     (Russian - 1049): Тип поиска отсутствующего компонента.
                /// 
                /// SchemaName: MissingComponentLookupType
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string missingcomponentlookuptype = "missingcomponentlookuptype";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Type Code
                ///     (Russian - 1049): Код типа
                /// 
                /// Description:
                ///     (English - United States - 1033): The object type code of the missing component.
                ///     (Russian - 1049): Код типа объекта отсутствующего компонента.
                /// 
                /// SchemaName: MissingComponentType
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
                public const string missingcomponenttype = "missingcomponenttype";
            }

            #endregion Attributes.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship userentityinstancedata_invaliddependency
                /// 
                /// PropertyName                               Value                                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_invaliddependency
                /// ReferencingEntityNavigationPropertyName    objectid_invaliddependency
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
                ///     Description:
                ///     (English - United States - 1033): Per User item instance data
                ///     (Russian - 1049): Данные экземпляра позиции "на пользователя"
                ///</summary>
                public static partial class userentityinstancedata_invaliddependency
                {
                    public const string Name = "userentityinstancedata_invaliddependency";

                    public const string ReferencedEntity_invaliddependency = "invaliddependency";

                    public const string ReferencedAttribute_invaliddependencyid = "invaliddependencyid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}