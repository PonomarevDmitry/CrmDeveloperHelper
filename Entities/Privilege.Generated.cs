
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): Privilege
    /// (Russian - 1049): Привилегия
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): Privileges
    /// (Russian - 1049): Права
    /// 
    /// Description:
    /// (English - United States - 1033): Permission to perform an action in Microsoft CRM. The platform checks for the privilege and rejects the attempt if the user does not hold the privilege.
    /// (Russian - 1049): Разрешение на выполнение действия в Microsoft CRM. Платформа проверяет наличие привилегии и запрещает попытку, если у пользователя нет требуемой привилегии.
    /// 
    /// PropertyName                          Value                CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     False                False
    /// CanBePrimaryEntityInRelationship      False                False
    /// CanBeRelatedEntityInRelationship      False                False
    /// CanChangeHierarchicalRelationship     False                False
    /// CanChangeTrackingBeEnabled            True                 True
    /// CanCreateAttributes                   False                False
    /// CanCreateCharts                       False                False
    /// CanCreateForms                        False                False
    /// CanCreateViews                        False                False
    /// CanEnableSyncToExternalSearchIndex    False                False
    /// CanModifyAdditionalSettings           False                True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 True
    /// CollectionSchemaName                  Privileges
    /// DaysSinceRecordLastModified           9999
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         privileges
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
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False                False
    /// IsRenameable                          False                False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False                False
    /// IsVisibleInMobile                     False                False
    /// IsVisibleInMobileClient               False                False
    /// LogicalCollectionName                 privileges
    /// LogicalName                           privilege
    /// ObjectTypeCode                        1023
    /// OwnershipType                         None
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredPrivilege
    /// SchemaName                            Privilege
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class Privilege
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "privilege";

            public const string EntitySchemaName = "Privilege";

            public const string EntityPrimaryNameAttribute = "name";

            public const string EntityPrimaryIdAttribute = "privilegeid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Rights a user has to an instance of an entity.
                ///     (Russian - 1049): Имеющиеся у пользователя права на экземпляр сущности.
                /// 
                /// SchemaName: AccessRight
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string accessright = "accessright";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether the privilege applies to the user, the user's team, or objects shared by the user.
                ///     (Russian - 1049): Указывает, применяется ли привилегия к пользователю, рабочей группе пользователя или к объектам, общий доступ к которым инициирован пользователем.
                /// 
                /// SchemaName: CanBeBasic
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): No
                ///     (Russian - 1049): Нет
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Yes
                ///     (Russian - 1049): Да
                /// TrueOption = 1
                ///</summary>
                public const string canbebasic = "canbebasic";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether the privilege applies to child business units of the business unit associated with the user.
                ///     (Russian - 1049): Указывает, применяется ли привилегия к дочерним подразделениям подразделения в которое входит пользователь.
                /// 
                /// SchemaName: CanBeDeep
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): No
                ///     (Russian - 1049): Нет
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Yes
                ///     (Russian - 1049): Да
                /// TrueOption = 1
                ///</summary>
                public const string canbedeep = "canbedeep";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether the privilege applies to the local reference of an external party.
                ///     (Russian - 1049): Указывает, применяется ли привилегия к локальной ссылке внешней стороны.
                /// 
                /// SchemaName: CanBeEntityReference
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): No
                ///     (Russian - 1049): Нет
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Yes
                ///     (Russian - 1049): Да
                /// TrueOption = 1
                ///</summary>
                public const string canbeentityreference = "canbeentityreference";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether the privilege applies to the entire organization.
                ///     (Russian - 1049): Указывает, применяется ли привилегия ко всей организации.
                /// 
                /// SchemaName: CanBeGlobal
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): No
                ///     (Russian - 1049): Нет
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Yes
                ///     (Russian - 1049): Да
                /// TrueOption = 1
                ///</summary>
                public const string canbeglobal = "canbeglobal";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether the privilege applies to the user's business unit.
                ///     (Russian - 1049): Указывает, применяется ли привилегия к подразделению пользователя.
                /// 
                /// SchemaName: CanBeLocal
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): No
                ///     (Russian - 1049): Нет
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Yes
                ///     (Russian - 1049): Да
                /// TrueOption = 1
                ///</summary>
                public const string canbelocal = "canbelocal";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether the privilege applies to parent reference of the external party.
                ///     (Russian - 1049): Указывает, применяется ли привилегия к родительской ссылке внешней стороны.
                /// 
                /// SchemaName: CanBeParentEntityReference
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): No
                ///     (Russian - 1049): Нет
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Yes
                ///     (Russian - 1049): Да
                /// TrueOption = 1
                ///</summary>
                public const string canbeparententityreference = "canbeparententityreference";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Specifies whether the privilege is disabled.
                ///     (Russian - 1049): Определяет, отключена ли привилегия.
                /// 
                /// SchemaName: IsDisabledWhenIntegrated
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: False    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): No
                ///     (Russian - 1049): Нет
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Yes
                ///     (Russian - 1049): Да
                /// TrueOption = 1
                ///</summary>
                public const string isdisabledwhenintegrated = "isdisabledwhenintegrated";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Name of the privilege.
                ///     (Russian - 1049): Название привилегии.
                /// 
                /// SchemaName: Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string name = "name";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the privilege.
                ///     (Russian - 1049): Уникальный идентификатор привилегии.
                /// 
                /// SchemaName: PrivilegeId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string privilegeid = "privilegeid";

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
                /// 1:N - Relationship FK_PrivilegeObjectTypeCodes
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
                /// ReferencingEntity privilegeobjecttypecodes:
                ///     DisplayName:
                ///     (English - United States - 1033): Privilege Object Type Code
                ///     (Russian - 1049): Код типа объекта права
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Privilege Object Type Codes
                ///     (Russian - 1049): Коды типов объектов прав
                ///     
                ///     Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                ///</summary>
                public static partial class fk_privilegeobjecttypecodes
                {
                    public const string Name = "FK_PrivilegeObjectTypeCodes";

                    public const string ReferencedEntity_privilege = "privilege";

                    public const string ReferencedAttribute_privilegeid = "privilegeid";

                    public const string ReferencingEntity_privilegeobjecttypecodes = "privilegeobjecttypecodes";

                    public const string ReferencingAttribute_privilegeid = "privilegeid";
                }

                ///<summary>
                /// 1:N - Relationship Privilege_AsyncOperations
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Privilege_AsyncOperations
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_privilege
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
                /// ReferencingEntity asyncoperation:
                ///     DisplayName:
                ///     (English - United States - 1033): System Job
                ///     (Russian - 1049): Системное задание
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): System Jobs
                ///     (Russian - 1049): Системные задания
                ///     
                ///     Description:
                ///     (English - United States - 1033): Process whose execution can proceed independently or in the background.
                ///     (Russian - 1049): Процесс, который может выполняться независимо или в фоновом режиме.
                ///</summary>
                public static partial class privilege_asyncoperations
                {
                    public const string Name = "Privilege_AsyncOperations";

                    public const string ReferencedEntity_privilege = "privilege";

                    public const string ReferencedAttribute_privilegeid = "privilegeid";

                    public const string ReferencingEntity_asyncoperation = "asyncoperation";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship Privilege_BulkDeleteFailures
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Privilege_BulkDeleteFailures
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_privilege
                /// IsCustomizable                             False                           False
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
                /// ReferencingEntity bulkdeletefailure:
                ///     DisplayName:
                ///     (English - United States - 1033): Bulk Delete Failure
                ///     (Russian - 1049): Ошибка группового удаления
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Bulk Delete Failures
                ///     (Russian - 1049): Ошибки группового удаления
                ///     
                ///     Description:
                ///     (English - United States - 1033): Record that was not deleted during a bulk deletion job.
                ///     (Russian - 1049): Запись не была удалена во время задания группового удаления.
                ///</summary>
                public static partial class privilege_bulkdeletefailures
                {
                    public const string Name = "Privilege_BulkDeleteFailures";

                    public const string ReferencedEntity_privilege = "privilege";

                    public const string ReferencedAttribute_privilegeid = "privilegeid";

                    public const string ReferencingEntity_bulkdeletefailure = "bulkdeletefailure";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_privilege
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_privilege
                /// ReferencingEntityNavigationPropertyName    objectid_privilege
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
                public static partial class userentityinstancedata_privilege
                {
                    public const string Name = "userentityinstancedata_privilege";

                    public const string ReferencedEntity_privilege = "privilege";

                    public const string ReferencedAttribute_privilegeid = "privilegeid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.

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
                /// Entity2LogicalName channelaccessprofile:
                ///     DisplayName:
                ///     (English - United States - 1033): Channel Access Profile
                ///     (Russian - 1049): Профиль доступа к каналам
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Channel Access Profiles
                ///     (Russian - 1049): Профили доступа к каналам
                ///     
                ///     Description:
                ///     (English - United States - 1033): Information about permissions needed to access Dynamics 365 through external channels.For internal use only
                ///     (Russian - 1049): Информация о разрешениях, необходимых для доступа к Dynamics 365 через внешние каналы. Только для внутреннего использования.
                ///</summary>
                public static partial class channelaccessprofile_privilege
                {
                    public const string Name = "ChannelAccessProfile_Privilege";

                    public const string IntersectEntity_channelaccessprofileentityaccesslevel = "channelaccessprofileentityaccesslevel";

                    public const string Entity1_privilege = "privilege";

                    public const string Entity1Attribute_entityaccesslevelid = "entityaccesslevelid";

                    public const string Entity2_channelaccessprofile = "channelaccessprofile";

                    public const string Entity2Attribute_channelaccessprofileid = "channelaccessprofileid";

                    public const string Entity2LogicalNamename = "name";
                }

                ///<summary>
                /// N:N - Relationship roleprivileges_association
                /// 
                /// PropertyName                                   Value                         CanBeChanged
                /// Entity1NavigationPropertyName                  roleprivileges_association
                /// Entity2NavigationPropertyName                  roleprivileges_association
                /// IsCustomizable                                 False                         False
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
                /// Entity2LogicalName role:
                ///     DisplayName:
                ///     (English - United States - 1033): Security Role
                ///     (Russian - 1049): Роль безопасности
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Security Roles
                ///     (Russian - 1049): Роли безопасности
                ///     
                ///     Description:
                ///     (English - United States - 1033): Grouping of security privileges. Users are assigned roles that authorize their access to the Microsoft CRM system.
                ///     (Russian - 1049): Группа привилегий безопасности. Пользователям назначаются роли, которые контролируют их доступ к CRM-системе Microsoft.
                ///</summary>
                public static partial class roleprivileges_association
                {
                    public const string Name = "roleprivileges_association";

                    public const string IntersectEntity_roleprivileges = "roleprivileges";

                    public const string Entity1_privilege = "privilege";

                    public const string Entity1Attribute_privilegeid = "privilegeid";

                    public const string Entity2_role = "role";

                    public const string Entity2Attribute_roleid = "roleid";

                    public const string Entity2LogicalNamename = "name";
                }

                ///<summary>
                /// N:N - Relationship roletemplateprivileges_association
                /// 
                /// PropertyName                                   Value                                 CanBeChanged
                /// Entity1NavigationPropertyName                  roletemplateprivileges_association
                /// Entity2NavigationPropertyName                  roletemplateprivileges_association
                /// IsCustomizable                                 False                                 False
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
                /// Entity1LogicalName roletemplate:
                ///     DisplayName:
                ///     (English - United States - 1033): Role Template
                ///     (Russian - 1049): Шаблон роли
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Role Templates
                ///     (Russian - 1049): Шаблоны ролей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Template for a role. Defines initial attributes that will be used when creating a new role.
                ///     (Russian - 1049): Шаблон роли. Определяет исходные атрибуты, которые будут использоваться при создании новой роли.
                ///</summary>
                public static partial class roletemplateprivileges_association
                {
                    public const string Name = "roletemplateprivileges_association";

                    public const string IntersectEntity_roletemplateprivileges = "roletemplateprivileges";

                    public const string Entity1_roletemplate = "roletemplate";

                    public const string Entity1Attribute_roletemplateid = "roletemplateid";

                    public const string Entity1LogicalName_PrimaryNameAttribute_name = "name";

                    public const string Entity2_privilege = "privilege";

                    public const string Entity2Attribute_privilegeid = "privilegeid";
                }
            }

            #endregion Relationship ManyToMany - N:N.
        }
    }
}
