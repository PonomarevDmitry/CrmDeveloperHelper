
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): Connection Role Object Type Code
    /// (Russian - 1049): Код типа объекта роли подключения
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): Connection Role Object Type Codes
    /// (Russian - 1049): Коды типа объекта роли подключения
    /// 
    /// Description:
    /// (English - United States - 1033): Specifies the entity type that can play specific role in a connection.
    /// (Russian - 1049): Определение типа сущности, которая может играть определенную роль в подключении.
    /// 
    /// PropertyName                          Value                            CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False                            False
    /// CanBePrimaryEntityInRelationship      False                            False
    /// CanBeRelatedEntityInRelationship      False                            False
    /// CanChangeHierarchicalRelationship     False                            False
    /// CanChangeTrackingBeEnabled            False                            False
    /// CanCreateAttributes                   False                            False
    /// CanCreateCharts                       False                            False
    /// CanCreateForms                        False                            False
    /// CanCreateViews                        False                            False
    /// CanEnableSyncToExternalSearchIndex    False                            False
    /// CanModifyAdditionalSettings           False                            True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 False
    /// CollectionSchemaName                  ConnectionRoleObjectTypeCodes
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         connectionroleobjecttypecodes
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False                            False
    /// IsAvailableOffline                    True
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         True
    /// IsConnectionsEnabled                  False                            False
    /// IsCustomEntity                        False
    /// IsCustomizable                        False                            False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                            False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                            False
    /// IsMappable                            False                            False
    /// IsOfflineInMobileClient               False                            True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             True
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False                            False
    /// IsRenameable                          False                            False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False                            False
    /// IsVisibleInMobile                     False                            False
    /// IsVisibleInMobileClient               False                            False
    /// LogicalCollectionName                 connectionroleobjecttypecodes
    /// LogicalName                           connectionroleobjecttypecode
    /// ObjectTypeCode                        3233
    /// OwnershipType                         None
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName
    /// SchemaName                            ConnectionRoleObjectTypeCode
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class ConnectionRoleObjectTypeCode
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "connectionroleobjecttypecode";

            public const string EntitySchemaName = "ConnectionRoleObjectTypeCode";

            public const string EntityPrimaryIdAttribute = "connectionroleobjecttypecodeid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// SchemaName: AssociatedObjectTypeCode
                /// EntityNameAttributeMetadata    AttributeType: EntityName    AttributeTypeName: EntityNameType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string associatedobjecttypecode = "associatedobjecttypecode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Connection Role
                ///     (Russian - 1049): Роль подключения
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the connection role associated with the Connection Role Object Type Code.
                ///     (Russian - 1049): Уникальный идентификатор роли подключения, связанной с кодом типа объекта роли подключения.
                /// 
                /// SchemaName: ConnectionRoleId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: connectionrole
                /// 
                ///     Target connectionrole    PrimaryIdAttribute connectionroleid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Connection Role
                ///         (Russian - 1049): Роль подключения
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Connection Roles
                ///         (Russian - 1049): Роли подключения
                ///         
                ///         Description:
                ///         (English - United States - 1033): Role describing a relationship between a two records.
                ///         (Russian - 1049): Роль, описывающая отношение между двумя записями.
                ///</summary>
                public const string connectionroleid = "connectionroleid";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the connection role object type association.
                ///     (Russian - 1049): Уникальный идентификатор связи типа объекта роли подключения.
                /// 
                /// SchemaName: ConnectionRoleObjectTypeCodeId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string connectionroleobjecttypecodeid = "connectionroleobjecttypecodeid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Organization 
                ///     (Russian - 1049): Предприятие 
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the organization associated with the connectionroleobjecttypecode.
                ///     (Russian - 1049): Уникальный идентификатор организации, связанной с connectionroleobjecttypecode.
                /// 
                /// SchemaName: OrganizationId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string organizationid = "organizationid";

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
                /// N:1 - Relationship connection_role_connection_role_object_type_codes
                /// 
                /// PropertyName                               Value                                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     connection_role_connection_role_object_type_codes
                /// ReferencingEntityNavigationPropertyName    connectionroleid
                /// IsCustomizable                             False                                                False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              ParentChild
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
                /// ReferencedEntity connectionrole:
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
                public static partial class connection_role_connection_role_object_type_codes
                {
                    public const string Name = "connection_role_connection_role_object_type_codes";

                    public const string ReferencedEntity_connectionrole = "connectionrole";

                    public const string ReferencedAttribute_connectionroleid = "connectionroleid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_connectionroleobjecttypecode = "connectionroleobjecttypecode";

                    public const string ReferencingAttribute_connectionroleid = "connectionroleid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship userentityinstancedata_connectionroleobjecttypecode
                /// 
                /// PropertyName                               Value                                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_connectionroleobjecttypecode
                /// ReferencingEntityNavigationPropertyName    objectid_connectionroleobjecttypecode
                /// IsCustomizable                             False                                                  False
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
                public static partial class userentityinstancedata_connectionroleobjecttypecode
                {
                    public const string Name = "userentityinstancedata_connectionroleobjecttypecode";

                    public const string ReferencedEntity_connectionroleobjecttypecode = "connectionroleobjecttypecode";

                    public const string ReferencedAttribute_connectionroleobjecttypecodeid = "connectionroleobjecttypecodeid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}
