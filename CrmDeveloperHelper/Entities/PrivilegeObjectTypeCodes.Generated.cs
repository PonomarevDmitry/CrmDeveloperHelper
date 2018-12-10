
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): Privilege Object Type Code
    /// (Russian - 1049): Код типа объекта права
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): Privilege Object Type Codes
    /// (Russian - 1049): Коды типов объектов прав
    /// 
    /// Description:
    /// (English - United States - 1033): For internal use only.
    /// (Russian - 1049): Только для внутреннего использования.
    /// 
    /// PropertyName                          Value                          CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False                          False
    /// CanBePrimaryEntityInRelationship      False                          False
    /// CanBeRelatedEntityInRelationship      False                          False
    /// CanChangeHierarchicalRelationship     False                          False
    /// CanChangeTrackingBeEnabled            True                           True
    /// CanCreateAttributes                   False                          False
    /// CanCreateCharts                       False                          False
    /// CanCreateForms                        False                          False
    /// CanCreateViews                        False                          False
    /// CanEnableSyncToExternalSearchIndex    False                          False
    /// CanModifyAdditionalSettings           False                          True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 True
    /// CollectionSchemaName                  PrivilegeObjectTypeCodeses
    /// DaysSinceRecordLastModified           9999
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         privilegeobjecttypecodesset
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False                          False
    /// IsAvailableOffline                    True
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False                          False
    /// IsCustomEntity                        False
    /// IsCustomizable                        False                          False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                          False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                          False
    /// IsMappable                            False                          False
    /// IsOfflineInMobileClient               False                          True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             True
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False                          False
    /// IsRenameable                          False                          False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False                          False
    /// IsVisibleInMobile                     False                          False
    /// IsVisibleInMobileClient               False                          False
    /// LogicalCollectionName                 privilegeobjecttypecodeses
    /// LogicalName                           privilegeobjecttypecodes
    /// ObjectTypeCode                        31
    /// OwnershipType                         None
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName
    /// SchemaName                            PrivilegeObjectTypeCodes
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class PrivilegeObjectTypeCodes
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "privilegeobjecttypecodes";

            public const string EntitySchemaName = "PrivilegeObjectTypeCodes";

            public const string EntityPrimaryIdAttribute = "privilegeobjecttypecodeid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: ObjectTypeCode
                /// EntityNameAttributeMetadata    AttributeType: EntityName    AttributeTypeName: EntityNameType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string objecttypecode = "objecttypecode";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: PrivilegeId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: privilege
                /// 
                ///     Target privilege    PrimaryIdAttribute privilegeid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Privilege
                ///         (Russian - 1049): Привилегия
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Privileges
                ///         (Russian - 1049): Права
                ///         
                ///         Description:
                ///         (English - United States - 1033): Permission to perform an action in Microsoft CRM. The platform checks for the privilege and rejects the attempt if the user does not hold the privilege.
                ///         (Russian - 1049): Разрешение на выполнение действия в Microsoft CRM. Платформа проверяет наличие привилегии и запрещает попытку, если у пользователя нет требуемой привилегии.
                ///</summary>
                public const string privilegeid = "privilegeid";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: PrivilegeObjectTypeCodeId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string privilegeobjecttypecodeid = "privilegeobjecttypecodeid";

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
                /// N:1 - Relationship FK_PrivilegeObjectTypeCodes
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     FK_PrivilegeObjectTypeCodes
                /// ReferencingEntityNavigationPropertyName    privilegeid
                /// IsCustomizable                             False                          False
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
                /// ReferencedEntity privilege:
                ///     DisplayName:
                ///     (English - United States - 1033): Privilege
                ///     (Russian - 1049): Привилегия
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Privileges
                ///     (Russian - 1049): Права
                ///     
                ///     Description:
                ///     (English - United States - 1033): Permission to perform an action in Microsoft CRM. The platform checks for the privilege and rejects the attempt if the user does not hold the privilege.
                ///     (Russian - 1049): Разрешение на выполнение действия в Microsoft CRM. Платформа проверяет наличие привилегии и запрещает попытку, если у пользователя нет требуемой привилегии.
                ///</summary>
                public static partial class fk_privilegeobjecttypecodes
                {
                    public const string Name = "FK_PrivilegeObjectTypeCodes";

                    public const string ReferencedEntity_privilege = "privilege";

                    public const string ReferencedAttribute_privilegeid = "privilegeid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_privilegeobjecttypecodes = "privilegeobjecttypecodes";

                    public const string ReferencingAttribute_privilegeid = "privilegeid";
                }
            }

            #endregion Relationship ManyToOne - N:1.
        }
    }
}
