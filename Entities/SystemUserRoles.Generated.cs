
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
    /// EntitySetName                         systemuserrolescollection
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
    /// IsOfflineInMobileClient               False
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
    /// IsVisibleInMobileClient               False
    /// LogicalCollectionName
    /// LogicalName                           systemuserroles
    /// ObjectTypeCode                        15
    /// OwnershipType                         None
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredSystemUserRoles
    /// SchemaName                            SystemUserRoles
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class SystemUserRoles
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "systemuserroles";

            public const string EntitySchemaName = "SystemUserRoles";

            public const string EntityPrimaryIdAttribute = "systemuserroleid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// SchemaName: RoleId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string roleid = "roleid";

                ///<summary>
                /// SchemaName: SystemUserId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string systemuserid = "systemuserid";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: SystemUserRoleId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string systemuserroleid = "systemuserroleid";

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

            #region Relationship ManyToMany - N:N.

            public static partial class ManyToMany
            {
                ///<summary>
                /// N:N - Relationship systemuserroles_association
                /// 
                /// PropertyName                                   Value
                /// Entity1NavigationPropertyName                  systemuserroles_association
                /// Entity2NavigationPropertyName                  systemuserroles_association
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
                /// Entity1LogicalName systemuser:
                ///     DisplayName:
                ///         (English - United States - 1033): User
                ///         (Russian - 1049): Пользователь
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///         (Russian - 1049): Пользователи
                ///     
                ///     Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///         (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public static partial class systemuserroles_association
                {
                    public const string Name = "systemuserroles_association";

                    public const string IntersectEntity_systemuserroles = "systemuserroles";

                    public const string Entity1_systemuser = "systemuser";

                    public const string Entity1Attribute_systemuserid = "systemuserid";

                    public const string Entity1LogicalName_PrimaryNameAttribute_fullname = "fullname";

                    public const string Entity2_role = "role";

                    public const string Entity2Attribute_roleid = "roleid";

                    public const string Entity2LogicalNamename = "name";
                }
            }

            #endregion Relationship ManyToMany - N:N.
        }
    }
}