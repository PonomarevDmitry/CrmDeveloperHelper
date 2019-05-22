
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class ConnectionRoleObjectTypeCode
    {
        ///<summary>
        /// DisplayName:
        ///     (English - United States - 1033): Connection Role Object Type Code
        ///     (Russian - 1049): Код типа объекта роли подключения
        /// 
        /// DisplayCollectionName:
        ///     (English - United States - 1033): Connection Role Object Type Codes
        ///     (Russian - 1049): Коды типа объекта роли подключения
        /// 
        /// Description:
        ///     (English - United States - 1033): Specifies the entity type that can play specific role in a connection.
        ///     (Russian - 1049): Определение типа сущности, которая может играть определенную роль в подключении.
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
        /// CollectionSchemaName                  ConnectionRoleObjectTypeCodes
        /// DataProviderId                        null
        /// DataSourceId                          null
        /// EnforceStateTransitions               False
        /// EntityHelpUrlEnabled                  False
        /// EntitySetName                         connectionroleobjecttypecodes
        /// IntroducedVersion                     5.0.0.0
        /// IsAIRUpdated                          False
        /// IsActivity                            False
        /// IsActivityParty                       False
        /// IsAvailableOffline                    True
        /// IsBPFEntity                           False
        /// IsBusinessProcessEnabled              False
        /// IsChildEntity                         True
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
        /// IsOptimisticConcurrencyEnabled        True
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
        /// LogicalCollectionName                 connectionroleobjecttypecodes
        /// LogicalName                           connectionroleobjecttypecode
        /// ObjectTypeCode                        3233
        /// OwnershipType                         None
        /// PrimaryIdAttribute                    connectionroleobjecttypecodeid
        /// SchemaName                            ConnectionRoleObjectTypeCode
        /// SyncToExternalSearchIndex             False
        /// UsesBusinessDataLabelTable            False
        ///</summary>
        public static partial class Schema
        {
            public const string EntityLogicalName = "connectionroleobjecttypecode";

            public const string EntitySchemaName = "ConnectionRoleObjectTypeCode";

            public const string EntityPrimaryIdAttribute = "connectionroleobjecttypecodeid";

            public const int EntityObjectTypeCode = 3233;

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the connection role object type association.
                ///     (Russian - 1049): Уникальный идентификатор связи типа объекта роли подключения.
                /// 
                /// SchemaName: ConnectionRoleObjectTypeCodeId
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
                [System.ComponentModel.DescriptionAttribute("Unique identifier of the connection role object type association.")]
                public const string connectionroleobjecttypecodeid = "connectionroleobjecttypecodeid";

                ///<summary>
                /// SchemaName: AssociatedObjectTypeCode
                /// EntityNameAttributeMetadata    AttributeType: EntityName    AttributeTypeName: EntityNameType    RequiredLevel: SystemRequired
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
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: connectionrole
                /// 
                ///     Target connectionrole    PrimaryIdAttribute connectionroleid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Connection Role
                ///             (Russian - 1049): Роль подключения
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Connection Roles
                ///             (Russian - 1049): Роли подключения
                ///         
                ///         Description:
                ///             (English - United States - 1033): Role describing a relationship between a two records.
                ///             (Russian - 1049): Роль, описывающая отношение между двумя записями.
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
                [System.ComponentModel.DescriptionAttribute("Connection Role")]
                public const string connectionroleid = "connectionroleid";

                ///<summary>
                /// SchemaName: ConnectionRoleIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'connectionroleid'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
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
                //public const string connectionroleidname = "connectionroleidname";

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
                [System.ComponentModel.DescriptionAttribute("Organization ")]
                public const string organizationid = "organizationid";

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

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship connection_role_connection_role_object_type_codes
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     connection_role_connection_role_object_type_codes
                /// ReferencingEntityNavigationPropertyName    connectionroleid
                /// IsCustomizable                             False
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
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity connectionrole:    PrimaryIdAttribute connectionroleid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Connection Role
                ///         (Russian - 1049): Роль подключения
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Connection Roles
                ///         (Russian - 1049): Роли подключения
                ///     
                ///     Description:
                ///         (English - United States - 1033): Role describing a relationship between a two records.
                ///         (Russian - 1049): Роль, описывающая отношение между двумя записями.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship connection_role_connection_role_object_type_codes")]
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
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_connectionroleobjecttypecode
                /// ReferencingEntityNavigationPropertyName    objectid_connectionroleobjecttypecode
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship userentityinstancedata_connectionroleobjecttypecode")]
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