
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    ///     (Russian - 1049): Поставщик данных виртуальной сущности
    /// 
    /// DisplayCollectionName:
    ///     (Russian - 1049): Поставщики данных виртуальных сущностей
    /// 
    /// Description:
    ///     (Russian - 1049): Разработчики могут регистрировать подключаемые модули для поставщика данных, чтобы обеспечить виртуальным сущностям в системе доступ к данным.
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
    /// CollectionSchemaName                  EntityDataProviders
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         entitydataproviders
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False
    /// IsAvailableOffline                    False
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
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False
    /// IsMappable                            False
    /// IsOfflineInMobileClient               False
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  False
    /// IsReadOnlyInMobileClient              False
    /// IsRenameable                          False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                False
    /// IsValidForQueue                       False
    /// IsVisibleInMobile                     False
    /// IsVisibleInMobileClient               False
    /// LogicalCollectionName                 entitydataproviders
    /// LogicalName                           entitydataprovider
    /// ObjectTypeCode                        78
    /// OwnershipType                         OrganizationOwned
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName
    /// SchemaName                            EntityDataProvider
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class EntityDataProvider
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "entitydataprovider";

            public const string EntitySchemaName = "EntityDataProvider";

            public const string EntityPrimaryNameAttribute = "name";

            public const string EntityPrimaryIdAttribute = "entitydataproviderid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Состояние компонента
                /// 
                /// Description:
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: ComponentState
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Global System  OptionSet componentstate
                /// DefaultFormValue = -1
                /// 
                ///         DisplayName:
                ///             (Russian - 1049): Состояние компонента
                ///         
                ///         Description:
                ///             (Russian - 1049): Состояние этого компонента.
                ///</summary>
                public const string componentstate = "componentstate";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Подключаемый модуль Create
                /// 
                /// SchemaName: CreatePlugin
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string createplugin = "createplugin";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Логическое имя сущности источника данных
                /// 
                /// Description:
                ///     (Russian - 1049): При создании поставщика данных конечный пользователь должен выбрать сущность источника данных, которая будет создана для этого поставщика.
                /// 
                /// SchemaName: DataSourceLogicalName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string datasourcelogicalname = "datasourcelogicalname";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Подключаемый модуль Delete
                /// 
                /// SchemaName: DeletePlugin
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string deleteplugin = "deleteplugin";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Описание
                /// 
                /// Description:
                ///     (Russian - 1049): Для чего используется этот поставщик данных? На какие технологии хранилища данных он ориентирован?
                /// 
                /// SchemaName: Description
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1000
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string description = "description";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Поставщик данных
                /// 
                /// Description:
                ///     (Russian - 1049): Уникальный идентификатор поставщика данных.
                /// 
                /// SchemaName: EntityDataProviderId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string entitydataproviderid = "entitydataproviderid";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Уникальный идентификатор
                /// 
                /// Description:
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: EntityDataProviderIdUnique
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string entitydataprovideridunique = "entitydataprovideridunique";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Версия добавления
                /// 
                /// Description:
                ///     (Russian - 1049): Версия, в которой была добавлена форма.
                /// 
                /// SchemaName: IntroducedVersion
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 48
                /// Format = VersionNumber    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string introducedversion = "introducedversion";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Настраиваемый
                /// 
                /// Description:
                ///     (Russian - 1049): Сведения, указывающие на возможность настройки этого компонента.
                /// 
                /// SchemaName: IsCustomizable
                /// ManagedPropertyAttributeMetadata    AttributeType: ManagedProperty    AttributeTypeName: ManagedPropertyType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string iscustomizable = "iscustomizable";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Состояние
                /// 
                /// Description:
                ///     (Russian - 1049): Указывает, является ли компонент решения частью управляемого решения.
                /// 
                /// SchemaName: IsManaged
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (Russian - 1049): Неуправляемый
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (Russian - 1049): Управляемый
                /// TrueOption = 1
                ///</summary>
                public const string ismanaged = "ismanaged";

                ///<summary>
                /// SchemaName: IsManagedName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'ismanaged'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                //public const string ismanagedname = "ismanagedname";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Имя
                /// 
                /// Description:
                ///     (Russian - 1049): Имя этого поставщика данных. Это имя отображается в раскрывающемся списке при создании новой сущности.
                /// 
                /// SchemaName: Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string name = "name";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Код организации
                /// 
                /// Description:
                ///     (Russian - 1049): Уникальный идентификатор организации.
                /// 
                /// SchemaName: OrganizationId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string organizationid = "organizationid";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Время замены записи
                /// 
                /// Description:
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: OverwriteTime
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateOnly
                ///</summary>
                public const string overwritetime = "overwritetime";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Подключаемый модуль MultipleRetrieve
                /// 
                /// SchemaName: RetrieveMultiplePlugin
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string retrievemultipleplugin = "retrievemultipleplugin";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Подключаемый модуль Retrieve
                /// 
                /// SchemaName: RetrievePlugin
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string retrieveplugin = "retrieveplugin";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Решение
                /// 
                /// Description:
                ///     (Russian - 1049): Уникальный идентификатор связанного решения.
                /// 
                /// SchemaName: SolutionId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string solutionid = "solutionid";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Решение
                /// 
                /// Description:
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: SupportingSolutionId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: False    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string supportingsolutionid = "supportingsolutionid";

                ///<summary>
                /// DisplayName:
                ///     (Russian - 1049): Подключаемый модуль Update
                /// 
                /// SchemaName: UpdatePlugin
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string updateplugin = "updateplugin";
            }

            #endregion Attributes.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship organization_entitydataprovider
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_entitydataprovider
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False
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
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity organization:
                ///     DisplayName:
                ///         (Russian - 1049): Предприятие
                ///     
                ///     DisplayCollectionName:
                ///         (Russian - 1049): Предприятия
                ///     
                ///     Description:
                ///         (Russian - 1049): Верхний уровень бизнес-иерархии Microsoft Dynamics 365. Организация может являться конкретной компанией, холдингом или корпорацией.
                ///</summary>
                public static partial class organization_entitydataprovider
                {
                    public const string Name = "organization_entitydataprovider";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_entitydataprovider = "entitydataprovider";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship entitydataprovider_datasource
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     entitydataprovider_datasource
                /// ReferencingEntityNavigationPropertyName    entitydataproviderid
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                RemoveLink
                /// CascadeConfiguration.Merge                 Cascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity entitydatasource:
                ///     DisplayName:
                ///         (Russian - 1049): Источник данных виртуальной сущности
                ///     
                ///     DisplayCollectionName:
                ///         (Russian - 1049): Источники данных виртуальных сущностей
                ///     
                ///     Description:
                ///         (Russian - 1049): Внутренняя сущность, в которой хранится информация источника данных для всех установленных поставщиков.
                ///</summary>
                public static partial class entitydataprovider_datasource
                {
                    public const string Name = "entitydataprovider_datasource";

                    public const string ReferencedEntity_entitydataprovider = "entitydataprovider";

                    public const string ReferencedAttribute_entitydataproviderid = "entitydataproviderid";

                    public const string ReferencingEntity_entitydatasource = "entitydatasource";

                    public const string ReferencingAttribute_entitydataproviderid = "entitydataproviderid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}