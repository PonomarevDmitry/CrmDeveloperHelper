
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): String Map
    /// (Russian - 1049): Сопоставление строки
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): String Maps
    /// (Russian - 1049): Сопоставления строк
    /// 
    /// Description:
    /// (English - United States - 1033): Mapping between strings.
    /// (Russian - 1049): Сопоставление строк.
    /// 
    /// PropertyName                          Value                CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False                False
    /// CanBePrimaryEntityInRelationship      False                False
    /// CanBeRelatedEntityInRelationship      False                False
    /// CanChangeHierarchicalRelationship     False                False
    /// CanChangeTrackingBeEnabled            False                False
    /// CanCreateAttributes                   False                False
    /// CanCreateCharts                       False                False
    /// CanCreateForms                        False                False
    /// CanCreateViews                        False                False
    /// CanEnableSyncToExternalSearchIndex    False                False
    /// CanModifyAdditionalSettings           False                True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 False
    /// CollectionSchemaName                  StringMaps
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         stringmaps
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False                False
    /// IsAvailableOffline                    True
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False                False
    /// IsCustomEntity                        False
    /// IsCustomizable                        False                False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                False
    /// IsMappable                            False                False
    /// IsOfflineInMobileClient               False                True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             True
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False                False
    /// IsRenameable                          False                False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False                False
    /// IsVisibleInMobile                     False                False
    /// IsVisibleInMobileClient               False                False
    /// LogicalCollectionName                 stringmaps
    /// LogicalName                           stringmap
    /// ObjectTypeCode                        1043
    /// OwnershipType                         OrganizationOwned
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredStringMap
    /// SchemaName                            StringMap
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class StringMap
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "stringmap";

            public const string EntitySchemaName = "StringMap";

            public const string EntityPrimaryIdAttribute = "stringmapid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// SchemaName: AttributeName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string attributename = "attributename";

                ///<summary>
                /// SchemaName: AttributeValue
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string attributevalue = "attributevalue";

                ///<summary>
                /// SchemaName: DisplayOrder
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string displayorder = "displayorder";

                ///<summary>
                /// SchemaName: LangId
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string langid = "langid";

                ///<summary>
                /// SchemaName: ObjectTypeCode
                /// EntityNameAttributeMetadata    AttributeType: EntityName    AttributeTypeName: EntityNameType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Local System  OptionSet stringmap_objecttypecode
                /// DefaultFormValue = -1
                ///</summary>
                public const string objecttypecode = "objecttypecode";

                ///<summary>
                /// SchemaName: OrganizationId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string organizationid = "organizationid";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the string map.
                ///     (Russian - 1049): Уникальный идентификатор сопоставления строки.
                /// 
                /// SchemaName: StringMapId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string stringmapid = "stringmapid";

                ///<summary>
                /// SchemaName: Value
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 4000
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string value = "value";

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

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship organization_string_maps
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
                public static partial class organization_string_maps
                {
                    public const string Name = "organization_string_maps";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_stringmap = "stringmap";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship userentityinstancedata_stringmap
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_stringmap
                /// ReferencingEntityNavigationPropertyName    objectid_stringmap
                /// IsCustomizable                             False                               False
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
                public static partial class userentityinstancedata_stringmap
                {
                    public const string Name = "userentityinstancedata_stringmap";

                    public const string ReferencedEntity_stringmap = "stringmap";

                    public const string ReferencedAttribute_stringmapid = "stringmapid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}
