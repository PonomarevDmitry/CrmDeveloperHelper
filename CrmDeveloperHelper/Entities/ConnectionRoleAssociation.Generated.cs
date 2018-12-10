
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// PropertyName                          Value                                CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False                                False
    /// CanBePrimaryEntityInRelationship      False                                False
    /// CanBeRelatedEntityInRelationship      False                                False
    /// CanChangeHierarchicalRelationship     False                                False
    /// CanChangeTrackingBeEnabled            False                                False
    /// CanCreateAttributes                   False                                False
    /// CanCreateCharts                       False                                False
    /// CanCreateForms                        False                                False
    /// CanCreateViews                        False                                False
    /// CanEnableSyncToExternalSearchIndex    False                                False
    /// CanModifyAdditionalSettings           False                                True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 False
    /// CollectionSchemaName
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         connectionroleassociations
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False                                False
    /// IsAvailableOffline                    True
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False                                False
    /// IsCustomEntity                        False
    /// IsCustomizable                        False                                False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                                False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           True
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                                False
    /// IsMappable                            False                                False
    /// IsOfflineInMobileClient               False                                True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False                                False
    /// IsRenameable                          False                                False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False                                False
    /// IsVisibleInMobile                     False                                False
    /// IsVisibleInMobileClient               False                                False
    /// LogicalCollectionName
    /// LogicalName                           connectionroleassociation
    /// ObjectTypeCode                        3232
    /// OwnershipType                         None
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredConnectionRoleAssociation
    /// SchemaName                            ConnectionRoleAssociation
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class ConnectionRoleAssociation
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "connectionroleassociation";

            public const string EntitySchemaName = "ConnectionRoleAssociation";

            public const string EntityPrimaryIdAttribute = "connectionroleassociationid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// SchemaName: AssociatedConnectionRoleId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string associatedconnectionroleid = "associatedconnectionroleid";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the connection role association.
                ///     (Russian - 1049): Уникальный идентификатор связи роли подключения.
                /// 
                /// SchemaName: ConnectionRoleAssociationId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string connectionroleassociationid = "connectionroleassociationid";

                ///<summary>
                /// SchemaName: ConnectionRoleId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string connectionroleid = "connectionroleid";

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

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship userentityinstancedata_connectionroleassociation
                /// 
                /// PropertyName                               Value                                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_connectionroleassociation
                /// ReferencingEntityNavigationPropertyName    objectid_connectionroleassociation
                /// IsCustomizable                             False                                               False
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
                public static partial class userentityinstancedata_connectionroleassociation
                {
                    public const string Name = "userentityinstancedata_connectionroleassociation";

                    public const string ReferencedEntity_connectionroleassociation = "connectionroleassociation";

                    public const string ReferencedAttribute_connectionroleassociationid = "connectionroleassociationid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.

            #region Relationship ManyToMany - N:N.

            public static partial class ManyToMany
            {
                ///<summary>
                /// N:N - Relationship connectionroleassociation_association
                /// 
                /// PropertyName                                   Value                                    CanBeChanged
                /// Entity1NavigationPropertyName                  connectionroleassociation_association
                /// Entity2NavigationPropertyName                  connectionroleassociation_association
                /// IsCustomizable                                 False                                    False
                /// IsCustomRelationship                           False
                /// IsValidForAdvancedFind                         True
                /// RelationshipType                               ManyToManyRelationship
                /// SecurityTypes                                  None
                /// Entity1AssociatedMenuConfiguration.Behavior    DoNotDisplay
                /// Entity1AssociatedMenuConfiguration.Group       Details
                /// Entity1AssociatedMenuConfiguration.Order       null
                /// Entity2AssociatedMenuConfiguration.Behavior    DoNotDisplay
                /// Entity2AssociatedMenuConfiguration.Group       Details
                /// Entity2AssociatedMenuConfiguration.Order       null
                /// 
                /// Entity1LogicalName connectionrole:
                ///     DisplayName:
                ///     (English - United States - 1033): Connection Role
                ///     (Russian - 1049): Роль подключения
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Connection Roles
                ///     (Russian - 1049): Роли подключения
                ///     
                ///     Description:
                ///     (English - United States - 1033): Role describing a relationship between a two records.
                ///     (Russian - 1049): Роль, описывающая отношение между двумя записями.
                ///</summary>
                public static partial class connectionroleassociation_association
                {
                    public const string Name = "connectionroleassociation_association";

                    public const string IntersectEntity_connectionroleassociation = "connectionroleassociation";

                    public const string Entity1_connectionrole = "connectionrole";

                    public const string Entity1Attribute_connectionroleid = "connectionroleid";

                    public const string Entity1LogicalName_PrimaryNameAttribute_name = "name";

                    public const string Entity2_connectionrole = "connectionrole";

                    public const string Entity2Attribute_associatedconnectionroleid = "associatedconnectionroleid";

                    public const string Entity2LogicalNamename = "name";
                }
            }

            #endregion Relationship ManyToMany - N:N.
        }
    }
}
