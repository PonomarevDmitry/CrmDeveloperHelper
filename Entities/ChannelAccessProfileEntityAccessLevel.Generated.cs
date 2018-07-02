
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// Description:
    /// (English - United States - 1033): Group of privileges used to categorize users to provide appropriate access to entities.
    /// (Russian - 1049): Группа привилегий, которая используется для разделения пользователей на категории для предоставления соответствующего доступа к сущностям.
    /// 
    /// PropertyName                          Value                                     CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False                                     False
    /// CanBePrimaryEntityInRelationship      False                                     False
    /// CanBeRelatedEntityInRelationship      False                                     False
    /// CanChangeHierarchicalRelationship     False                                     False
    /// CanChangeTrackingBeEnabled            False                                     False
    /// CanCreateAttributes                   False                                     False
    /// CanCreateCharts                       False                                     False
    /// CanCreateForms                        False                                     False
    /// CanCreateViews                        False                                     False
    /// CanEnableSyncToExternalSearchIndex    False                                     False
    /// CanModifyAdditionalSettings           True                                      True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 False
    /// CollectionSchemaName
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         channelaccessprofileentityaccesslevels
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False                                     True
    /// IsAvailableOffline                    False
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False                                     False
    /// IsCustomEntity                        False
    /// IsCustomizable                        False                                     False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                                     False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           True
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                                     False
    /// IsMappable                            True                                      False
    /// IsOfflineInMobileClient               False                                     True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False                                     False
    /// IsRenameable                          False                                     False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False                                     False
    /// IsVisibleInMobile                     False                                     False
    /// IsVisibleInMobileClient               False                                     False
    /// LogicalCollectionName
    /// LogicalName                           channelaccessprofileentityaccesslevel
    /// ObjectTypeCode                        9404
    /// OwnershipType                         None
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName
    /// SchemaName                            ChannelAccessProfileEntityAccessLevel
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class ChannelAccessProfileEntityAccessLevel
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "channelaccessprofileentityaccesslevel";

            public const string EntitySchemaName = "ChannelAccessProfileEntityAccessLevel";

            public const string EntityPrimaryIdAttribute = "channelaccessprofileentityaccesslevelid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the entity access level associated with the channel access profile.
                ///     (Russian - 1049): Уникальный код уровня доступа сущности, с которым связан профиль доступа к каналам.
                /// 
                /// SchemaName: ChannelAccessProfileEntityAccessLevelId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string channelaccessprofileentityaccesslevelid = "channelaccessprofileentityaccesslevelid";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: ChannelAccessProfileEntityAccessLevelIdUnique
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string channelaccessprofileentityaccesslevelidunique = "channelaccessprofileentityaccesslevelidunique";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the channel access profile.
                ///     (Russian - 1049): Уникальный код профиля доступа к каналам.
                /// 
                /// SchemaName: ChannelAccessProfileId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string channelaccessprofileid = "channelaccessprofileid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Component State
                ///     (Russian - 1049): Состояние компонента
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: ComponentState
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Global System  OptionSet componentstate
                /// DefaultFormValue = -1
                /// 
                ///         DisplayName:
                ///             (English - United States - 1033): Component State
                ///             (Russian - 1049): Состояние компонента
                ///         
                ///         Description:
                ///             (English - United States - 1033): The state of this component.
                ///             (Russian - 1049): Состояние этого компонента.
                ///</summary>
                public const string componentstate = "componentstate";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): System-generated attribute that stores the privileges associated with the role.
                ///     (Russian - 1049): Атрибут, создаваемый системой, который хранит привилегии, связанные с ролью.
                /// 
                /// SchemaName: EntityAccessLevelDepthMask
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string entityaccessleveldepthmask = "entityaccessleveldepthmask";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the entity access level associated with the channel access profile
                ///     (Russian - 1049): Уникальный код уровня доступа сущности, с которым связан профиль доступа к каналам
                /// 
                /// SchemaName: EntityAccessLevelId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string entityaccesslevelid = "entityaccesslevelid";

                ///<summary>
                /// SchemaName: IsManaged
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Unmanaged
                ///     (Russian - 1049): Неуправляемый
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Managed
                ///     (Russian - 1049): Управляемый
                /// TrueOption = 1
                ///</summary>
                public const string ismanaged = "ismanaged";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Record Overwrite Time
                ///     (Russian - 1049): Время замены записи
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: OverwriteTime
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateOnly
                ///</summary>
                public const string overwritetime = "overwritetime";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Solution
                ///     (Russian - 1049): Решение
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the associated solution.
                ///     (Russian - 1049): Уникальный код связанного решения.
                /// 
                /// SchemaName: SolutionId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string solutionid = "solutionid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Solution
                ///     (Russian - 1049): Решение
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: SupportingSolutionId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: False    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string supportingsolutionid = "supportingsolutionid";

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

            #region Relationship ManyToMany - N:N.

            public static partial class ManyToMany
            {
                ///<summary>
                /// N:N - Relationship ChannelAccessProfile_Privilege
                /// 
                /// PropertyName                                   Value                             CanBeChanged
                /// Entity1NavigationPropertyName                  ChannelAccessProfile_Privilege
                /// Entity2NavigationPropertyName                  ChannelAccessProfile_Privilege
                /// IsCustomizable                                 False                             False
                /// IsCustomRelationship                           False
                /// IsValidForAdvancedFind                         False
                /// RelationshipType                               ManyToManyRelationship
                /// SecurityTypes                                  None
                /// Entity1AssociatedMenuConfiguration.Behavior    DoNotDisplay
                /// Entity1AssociatedMenuConfiguration.Group       Details
                /// Entity1AssociatedMenuConfiguration.Order       null
                /// Entity2AssociatedMenuConfiguration.Behavior    DoNotDisplay
                /// Entity2AssociatedMenuConfiguration.Group       Details
                /// Entity2AssociatedMenuConfiguration.Order       null
                /// 
                /// Entity1LogicalName privilege:
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
                public static partial class channelaccessprofile_privilege
                {
                    public const string Name = "ChannelAccessProfile_Privilege";

                    public const string IntersectEntity_channelaccessprofileentityaccesslevel = "channelaccessprofileentityaccesslevel";

                    public const string Entity1_privilege = "privilege";

                    public const string Entity1Attribute_entityaccesslevelid = "entityaccesslevelid";

                    public const string Entity1LogicalName_PrimaryNameAttribute_name = "name";

                    public const string Entity2_channelaccessprofile = "channelaccessprofile";

                    public const string Entity2Attribute_channelaccessprofileid = "channelaccessprofileid";

                    public const string Entity2LogicalNamename = "name";
                }
            }

            #endregion Relationship ManyToMany - N:N.
        }
    }
}