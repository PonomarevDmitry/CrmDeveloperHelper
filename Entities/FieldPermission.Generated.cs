
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): Field Permission
    /// (Russian - 1049): Разрешение поля
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): Field Permissions
    /// (Russian - 1049): Поле разрешений
    /// 
    /// Description:
    /// (English - United States - 1033): Group of privileges used to categorize users to provide appropriate access to secured columns.
    /// (Russian - 1049): Группа привилегий, используемых для распределения пользователей по категориям с целью обеспечения соответствующего доступа к защищенным столбцам.
    /// 
    /// PropertyName                          Value               CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False               False
    /// CanBePrimaryEntityInRelationship      False               False
    /// CanBeRelatedEntityInRelationship      False               False
    /// CanChangeHierarchicalRelationship     False               False
    /// CanChangeTrackingBeEnabled            True                True
    /// CanCreateAttributes                   False               False
    /// CanCreateCharts                       False               False
    /// CanCreateForms                        False               False
    /// CanCreateViews                        False               False
    /// CanEnableSyncToExternalSearchIndex    False               False
    /// CanModifyAdditionalSettings           True                True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 True
    /// CollectionSchemaName                  FieldPermissions
    /// DaysSinceRecordLastModified           9999
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         fieldpermissions
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False               True
    /// IsAvailableOffline                    False
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         True
    /// IsConnectionsEnabled                  False               False
    /// IsCustomEntity                        False
    /// IsCustomizable                        True                False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False               False
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          False
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False               False
    /// IsMappable                            False               False
    /// IsOfflineInMobileClient               False               True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False               False
    /// IsRenameable                          False               False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False               False
    /// IsVisibleInMobile                     False               False
    /// IsVisibleInMobileClient               False               False
    /// LogicalCollectionName                 fieldpermissions
    /// LogicalName                           fieldpermission
    /// ObjectTypeCode                        1201
    /// OwnershipType                         None
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName
    /// SchemaName                            FieldPermission
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class FieldPermission
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "fieldpermission";

            public const string EntitySchemaName = "FieldPermission";

            public const string EntityPrimaryIdAttribute = "fieldpermissionid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Name of the attribute for which this privilege is defined
                ///     (Russian - 1049): Имя атрибута, для которого определена эта привилегия
                /// 
                /// Description:
                ///     (English - United States - 1033): Attribute Name.
                ///     (Russian - 1049): Имя атрибута.
                /// 
                /// SchemaName: AttributeLogicalName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string attributelogicalname = "attributelogicalname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Can create the attribute
                ///     (Russian - 1049): Может создать атрибут
                /// 
                /// Description:
                ///     (English - United States - 1033): Can this Profile create the attribute
                ///     (Russian - 1049): Этот профиль может создать атрибут
                /// 
                /// SchemaName: CanCreate
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Global System  OptionSet field_security_permission_type
                /// DefaultFormValue = 0
                ///</summary>
                public const string cancreate = "cancreate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Can Read the attribute
                ///     (Russian - 1049): Может читать атрибут
                /// 
                /// Description:
                ///     (English - United States - 1033): Can this Profile read the attribute
                ///     (Russian - 1049): Этот профиль может читать атрибут
                /// 
                /// SchemaName: CanRead
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Global System  OptionSet field_security_permission_type
                /// DefaultFormValue = 0
                ///</summary>
                public const string canread = "canread";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Can Update the attribute
                ///     (Russian - 1049): Может обновлять атрибут
                /// 
                /// Description:
                ///     (English - United States - 1033): Can this Profile update the attribute
                ///     (Russian - 1049): Этот профиль может обновлять атрибут
                /// 
                /// SchemaName: CanUpdate
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Global System  OptionSet field_security_permission_type
                /// DefaultFormValue = 0
                ///</summary>
                public const string canupdate = "canupdate";

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
                ///</summary>
                public const string componentstate = "componentstate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Name of the Entity for which this privilege is defined
                ///     (Russian - 1049): Название сущности, для которой определена эта привилегия
                /// 
                /// Description:
                ///     (English - United States - 1033): Entity name.
                ///     (Russian - 1049): Имя сущности.
                /// 
                /// SchemaName: EntityName
                /// EntityNameAttributeMetadata    AttributeType: EntityName    AttributeTypeName: EntityNameType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string entityname = "entityname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Field Permission
                ///     (Russian - 1049): Разрешение поля
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the Field Permission.
                ///     (Russian - 1049): Уникальный идентификатор поля разрешения.
                /// 
                /// SchemaName: FieldPermissionId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string fieldpermissionid = "fieldpermissionid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Field Permission
                ///     (Russian - 1049): Разрешение поля
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: FieldPermissionIdUnique
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string fieldpermissionidunique = "fieldpermissionidunique";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Profile
                ///     (Russian - 1049): Профиль
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of profile to which this privilege belongs.
                ///     (Russian - 1049): Уникальный идентификатор профиля, к которому относится эта привилегия.
                /// 
                /// SchemaName: FieldSecurityProfileId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: fieldsecurityprofile
                /// 
                ///     Target fieldsecurityprofile    PrimaryIdAttribute fieldsecurityprofileid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Field Security Profile
                ///         (Russian - 1049): Профиль безопасности поля
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Field Security Profiles
                ///         (Russian - 1049): Профили безопасности полей
                ///         
                ///         Description:
                ///         (English - United States - 1033): Profile which defines access level for secured attributes
                ///         (Russian - 1049): Профиль, который определяет уровень доступа к защищенным атрибутам
                ///</summary>
                public const string fieldsecurityprofileid = "fieldsecurityprofileid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Managed
                ///     (Russian - 1049): Управляемый
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the solution component is part of a managed solution.
                ///     (Russian - 1049): Указывает, является ли компонент решения частью управляемого решения.
                /// 
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
                ///     (English - United States - 1033): Organization Id
                ///     (Russian - 1049): Идентификатор организации
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier for the organization
                ///     (Russian - 1049): Уникальный идентификатор организации
                /// 
                /// SchemaName: OrganizationId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string organizationid = "organizationid";

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
                ///     (Russian - 1049): Уникальный идентификатор связанного решения.
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

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship lk_fieldpermission_fieldsecurityprofileid
                /// 
                /// PropertyName                               Value                                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_fieldpermission_fieldsecurityprofileid
                /// ReferencingEntityNavigationPropertyName    fieldsecurityprofileid
                /// IsCustomizable                             False                                        False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
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
                /// ReferencedEntity fieldsecurityprofile:
                ///     DisplayName:
                ///     (English - United States - 1033): Field Security Profile
                ///     (Russian - 1049): Профиль безопасности поля
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Field Security Profiles
                ///     (Russian - 1049): Профили безопасности полей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Profile which defines access level for secured attributes
                ///     (Russian - 1049): Профиль, который определяет уровень доступа к защищенным атрибутам
                ///</summary>
                public static partial class lk_fieldpermission_fieldsecurityprofileid
                {
                    public const string Name = "lk_fieldpermission_fieldsecurityprofileid";

                    public const string ReferencedEntity_fieldsecurityprofile = "fieldsecurityprofile";

                    public const string ReferencedAttribute_fieldsecurityprofileid = "fieldsecurityprofileid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_fieldpermission = "fieldpermission";

                    public const string ReferencingAttribute_fieldsecurityprofileid = "fieldsecurityprofileid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship FieldPermission_SyncErrors
                /// 
                /// PropertyName                               Value                                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     FieldPermission_SyncErrors
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_fieldpermission_syncerror
                /// IsCustomizable                             True                                           False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 Cascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity syncerror:
                ///     DisplayName:
                ///     (English - United States - 1033): Sync Error
                ///     (Russian - 1049): Ошибка синхронизации
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Sync Errors
                ///     (Russian - 1049): Ошибки синхронизации
                ///     
                ///     Description:
                ///     (English - United States - 1033): Failure reason and other detailed information for a record that failed to sync.
                ///     (Russian - 1049): Причина сбоя и другие подробные сведения о записи, при синхронизации которой произошла ошибка.
                ///</summary>
                public static partial class fieldpermission_syncerrors
                {
                    public const string Name = "FieldPermission_SyncErrors";

                    public const string ReferencedEntity_fieldpermission = "fieldpermission";

                    public const string ReferencedAttribute_fieldpermissionid = "fieldpermissionid";

                    public const string ReferencingEntity_syncerror = "syncerror";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_fieldpermission
                /// 
                /// PropertyName                               Value                                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_fieldpermission
                /// ReferencingEntityNavigationPropertyName    objectid_fieldpermission
                /// IsCustomizable                             False                                     False
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
                public static partial class userentityinstancedata_fieldpermission
                {
                    public const string Name = "userentityinstancedata_fieldpermission";

                    public const string ReferencedEntity_fieldpermission = "fieldpermission";

                    public const string ReferencedAttribute_fieldpermissionid = "fieldpermissionid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}
