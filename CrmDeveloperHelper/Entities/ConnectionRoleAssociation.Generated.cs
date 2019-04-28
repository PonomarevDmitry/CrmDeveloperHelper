
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class ConnectionRoleAssociation
    {
        ///<summary>
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
        /// DataProviderId                        null
        /// DataSourceId                          null
        /// EnforceStateTransitions               False
        /// EntityHelpUrlEnabled                  False
        /// EntitySetName                         connectionroleassociations
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
        /// IsIntersect                           True
        /// IsKnowledgeManagementEnabled          False
        /// IsLogicalEntity                       False
        /// IsMailMergeEnabled                    False
        /// IsMappable                            False
        /// IsOfflineInMobileClient               False
        /// IsOneNoteIntegrationEnabled           False
        /// IsOptimisticConcurrencyEnabled        True
        /// IsPrivate                             False
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
        /// LogicalName                           connectionroleassociation
        /// ObjectTypeCode                        3232
        /// OwnershipType                         None
        /// PrimaryIdAttribute                    connectionroleassociationid
        /// ReportViewName                        FilteredConnectionRoleAssociation
        /// SchemaName                            ConnectionRoleAssociation
        /// SyncToExternalSearchIndex             False
        /// UsesBusinessDataLabelTable            False
        ///</summary>
        public static partial class Schema
        {
            public const string EntityLogicalName = "connectionroleassociation";

            public const string EntitySchemaName = "ConnectionRoleAssociation";

            public const string EntityPrimaryIdAttribute = "connectionroleassociationid";

            public const int EntityObjectTypeCode = 3232;

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the connection role association.
                /// 
                /// SchemaName: ConnectionRoleAssociationId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
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
                public const string connectionroleassociationid = "connectionroleassociationid";

                ///<summary>
                /// SchemaName: AssociatedConnectionRoleId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
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
                public const string associatedconnectionroleid = "associatedconnectionroleid";

                ///<summary>
                /// SchemaName: ConnectionRoleId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
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
                public const string connectionroleid = "connectionroleid";

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
                public const string versionnumber = "versionnumber";
            }

            #endregion Attributes.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship userentityinstancedata_connectionroleassociation
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_connectionroleassociation
                /// ReferencingEntityNavigationPropertyName    objectid_connectionroleassociation
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
                ///     
                ///     Description:
                ///         (English - United States - 1033): Per User item instance data
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
                /// PropertyName                                   Value
                /// Entity1NavigationPropertyName                  connectionroleassociation_association
                /// Entity2NavigationPropertyName                  connectionroleassociation_association
                /// IsCustomizable                                 False
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
                /// Entity1LogicalName connectionrole:    PrimaryIdAttribute connectionroleid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Connection Role
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Connection Roles
                ///     
                ///     Description:
                ///         (English - United States - 1033): Role describing a relationship between a two records.
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

                    public const string Entity2LogicalName_PrimaryNameAttribute_name = "name";
                }
            }

            #endregion Relationship ManyToMany - N:N.
        }
    }
}