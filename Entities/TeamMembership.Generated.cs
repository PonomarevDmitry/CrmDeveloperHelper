
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
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
    /// CanChangeTrackingBeEnabled            True
    /// CanCreateAttributes                   False
    /// CanCreateCharts                       False
    /// CanCreateForms                        False
    /// CanCreateViews                        False
    /// CanEnableSyncToExternalSearchIndex    False
    /// CanModifyAdditionalSettings           False
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 True
    /// CollectionSchemaName
    /// DaysSinceRecordLastModified           9999
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         teammemberships
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False
    /// IsAvailableOffline                    True
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False
    /// IsCustomEntity                        False
    /// IsCustomizable                        False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           True
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False
    /// IsMappable                            False
    /// IsOfflineInMobileClient               True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False
    /// IsRenameable                          False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False
    /// IsVisibleInMobile                     False
    /// IsVisibleInMobileClient               True
    /// LogicalCollectionName
    /// LogicalName                           teammembership
    /// ObjectTypeCode                        23
    /// OwnershipType                         None
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredTeamMembership
    /// SchemaName                            TeamMembership
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class TeamMembership
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "teammembership";

            public const string EntitySchemaName = "TeamMembership";

            public const string EntityPrimaryIdAttribute = "teammembershipid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// SchemaName: SystemUserId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string systemuserid = "systemuserid";

                ///<summary>
                /// SchemaName: TeamId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string teamid = "teamid";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the team membership.
                ///     (Russian - 1049): Уникальный идентификатор участника рабочей группы.
                /// 
                /// SchemaName: TeamMembershipId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string teammembershipid = "teammembershipid";

                ///<summary>
                /// SchemaName: VersionNumber
                /// BigIntAttributeMetadata    AttributeType: BigInt    AttributeTypeName: BigIntType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False
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
                /// 1:N - Relationship userentityinstancedata_teammembership
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_teammembership
                /// ReferencingEntityNavigationPropertyName    objectid_teammembership
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
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity userentityinstancedata:
                ///     DisplayName:
                ///         (English - United States - 1033): User Entity Instance Data
                ///         (Russian - 1049): Данные экземпляра сущности пользователя
                ///     
                ///     Description:
                ///         (English - United States - 1033): Per User item instance data
                ///         (Russian - 1049): Данные экземпляра позиции "на пользователя"
                ///</summary>
                public static partial class userentityinstancedata_teammembership
                {
                    public const string Name = "userentityinstancedata_teammembership";

                    public const string ReferencedEntity_teammembership = "teammembership";

                    public const string ReferencedAttribute_teammembershipid = "teammembershipid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.

            #region Relationship ManyToMany - N:N.

            public static partial class ManyToMany
            {
                ///<summary>
                /// N:N - Relationship teammembership_association
                /// 
                /// PropertyName                                   Value
                /// Entity1NavigationPropertyName                  teammembership_association
                /// Entity2NavigationPropertyName                  teammembership_association
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
                /// Entity1LogicalName team:
                ///     DisplayName:
                ///         (English - United States - 1033): Team
                ///         (Russian - 1049): Рабочая группа
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Teams
                ///         (Russian - 1049): Рабочие группы
                ///     
                ///     Description:
                ///         (English - United States - 1033): Collection of system users that routinely collaborate. Teams can be used to simplify record sharing and provide team members with common access to organization data when team members belong to different Business Units.
                ///         (Russian - 1049): Набор системных пользователей, которые обычно участвуют в совместной работе. Рабочие группы можно использовать для упрощения предоставления участникам группы общего доступа к записям и данным организации, если участники принадлежат к разным подразделениям.
                ///</summary>
                public static partial class teammembership_association
                {
                    public const string Name = "teammembership_association";

                    public const string IntersectEntity_teammembership = "teammembership";

                    public const string Entity1_team = "team";

                    public const string Entity1Attribute_teamid = "teamid";

                    public const string Entity1LogicalName_PrimaryNameAttribute_name = "name";

                    public const string Entity2_systemuser = "systemuser";

                    public const string Entity2Attribute_systemuserid = "systemuserid";

                    public const string Entity2LogicalNamefullname = "fullname";
                }
            }

            #endregion Relationship ManyToMany - N:N.
        }
    }
}