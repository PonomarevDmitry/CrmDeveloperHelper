
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class TextAnalyticsEntityMapping
    {
        ///<summary>
        /// DisplayName:
        ///     (English - United States - 1033): Text Analytics Entity Mapping
        ///     (Russian - 1049): Сопоставления сущности текстовой аналитики
        /// 
        /// DisplayCollectionName:
        ///     (English - United States - 1033): Text Analytics Entity Mappings
        ///     (Russian - 1049): Сопоставления сущностей текстовой аналитики
        /// 
        /// PropertyName                          Value
        /// ActivityTypeMask                      0
        /// AutoCreateAccessTeams                 False
        /// AutoRouteToOwnerQueue                 False
        /// CanBeInManyToMany                     True
        /// CanBePrimaryEntityInRelationship      True
        /// CanBeRelatedEntityInRelationship      True
        /// CanChangeHierarchicalRelationship     False
        /// CanChangeTrackingBeEnabled            False
        /// CanCreateAttributes                   False
        /// CanCreateCharts                       True
        /// CanCreateForms                        True
        /// CanCreateViews                        True
        /// CanEnableSyncToExternalSearchIndex    False
        /// CanModifyAdditionalSettings           True
        /// CanTriggerWorkflow                    False
        /// ChangeTrackingEnabled                 False
        /// CollectionSchemaName                  TextAnalyticsEntityMappings
        /// DataProviderId                        null
        /// DataSourceId                          null
        /// EnforceStateTransitions               False
        /// EntityHelpUrlEnabled                  False
        /// EntitySetName                         textanalyticsentitymappings
        /// IntroducedVersion                     8.0.0.0
        /// IsAIRUpdated                          False
        /// IsActivity                            False
        /// IsActivityParty                       False
        /// IsAvailableOffline                    False
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
        /// IsIntersect                           False
        /// IsKnowledgeManagementEnabled          False
        /// IsLogicalEntity                       False
        /// IsMailMergeEnabled                    False
        /// IsMappable                            True
        /// IsOfflineInMobileClient               False
        /// IsOneNoteIntegrationEnabled           False
        /// IsOptimisticConcurrencyEnabled        False
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
        /// LogicalCollectionName                 textanalyticsentitymapping
        /// LogicalName                           textanalyticsentitymapping
        /// ObjectTypeCode                        9945
        /// OwnershipType                         OrganizationOwned
        /// PrimaryIdAttribute                    textanalyticsentitymappingid
        /// SchemaName                            TextAnalyticsEntityMapping
        /// SyncToExternalSearchIndex             False
        /// UsesBusinessDataLabelTable            False
        ///</summary>
        public static partial class Schema
        {
            public const string EntityLogicalName = "textanalyticsentitymapping";

            public const string EntitySchemaName = "TextAnalyticsEntityMapping";

            public const string EntityPrimaryIdAttribute = "textanalyticsentitymappingid";

            public const int EntityObjectTypeCode = 9945;

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Text Analytics Entity Mapping
                ///     (Russian - 1049): Сопоставления сущности текстовой аналитики
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier for entity instances
                ///     (Russian - 1049): Уникальный идентификатор экземпляров сущности
                /// 
                /// SchemaName: TextAnalyticsEntityMappingId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    True
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Text Analytics Entity Mapping")]
                public const string textanalyticsentitymappingid = "textanalyticsentitymappingid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Advanced Similarity RuleId
                ///     (Russian - 1049): Идентификатор расширенного правила подобия
                /// 
                /// Description:
                ///     (English - United States - 1033): Advanced Similarity RuleId associated with entity mapping.
                ///     (Russian - 1049): Идентификатор расширенного правила подобия, связанного с сопоставлением сущностей.
                /// 
                /// SchemaName: AdvancedSimilarityRuleId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: incident
                /// 
                ///     Target incident    PrimaryIdAttribute incidentid    PrimaryNameAttribute title
                ///         DisplayName:
                ///             (English - United States - 1033): Case
                ///             (Russian - 1049): Обращение
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Cases
                ///             (Russian - 1049): Обращения
                ///         
                ///         Description:
                ///             (English - United States - 1033): Service request case associated with a contract.
                ///             (Russian - 1049): Обращение по запросу на обслуживание, связанному с контрактом.
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.1.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Advanced Similarity RuleId")]
                public const string advancedsimilarityruleid = "advancedsimilarityruleid";

                ///<summary>
                /// SchemaName: AdvancedSimilarityRuleIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'advancedsimilarityruleid'
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
                /// IntroducedVersion              8.1.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                //public const string advancedsimilarityruleidname = "advancedsimilarityruleidname";

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
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
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
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.0.0.0
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
                [System.ComponentModel.DescriptionAttribute("Component State")]
                public const string componentstate = "componentstate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Entity
                ///     (Russian - 1049): Сущность
                /// 
                /// SchemaName: Entity
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Entity")]
                public const string entity = "entity";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Entity Name
                ///     (Russian - 1049): Имя сущности
                /// 
                /// Description:
                ///     (English - United States - 1033): Entity Display Name
                ///     (Russian - 1049): Отображаемое имя сущности
                /// 
                /// SchemaName: EntityDisplayName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Entity Name")]
                public const string entitydisplayname = "entitydisplayname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Entity
                ///     (Russian - 1049): Сущность
                /// 
                /// Description:
                ///     (English - United States - 1033): Select Entity
                ///     (Russian - 1049): Выбрать сущность
                /// 
                /// SchemaName: EntityPickList
                /// EntityNameAttributeMetadata    AttributeType: EntityName    AttributeTypeName: EntityNameType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Local System  OptionSet textanalyticsentitymapping_entity
                /// DefaultFormValue = -1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
                /// IntroducedVersion              8.0.0.0
                /// IsCustomizable                 True
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
                /// IsValidForForm                 True
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Entity")]
                public const string entitypicklist = "entitypicklist";

                ///<summary>
                /// SchemaName: EntityPickListName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'entitypicklist'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
                /// IntroducedVersion              8.0.0.0
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
                //public const string entitypicklistname = "entitypicklistname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Field
                ///     (Russian - 1049): Поле
                /// 
                /// SchemaName: Field
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Field")]
                public const string field = "field";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Field Name
                ///     (Russian - 1049): Имя поля
                /// 
                /// Description:
                ///     (English - United States - 1033): Field Display Name
                ///     (Russian - 1049): Отображаемое имя поля
                /// 
                /// SchemaName: FieldDisplayName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Field Name")]
                public const string fielddisplayname = "fielddisplayname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Field
                ///     (Russian - 1049): Поле
                /// 
                /// Description:
                ///     (English - United States - 1033): Select Field
                ///     (Russian - 1049): Выбрать поле
                /// 
                /// SchemaName: FieldPickList
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet textanalyticsentitymapping_fields
                /// DefaultFormValue = -1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
                /// IntroducedVersion              8.0.0.0
                /// IsCustomizable                 True
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
                [System.ComponentModel.DescriptionAttribute("Field")]
                public const string fieldpicklist = "fieldpicklist";

                ///<summary>
                /// SchemaName: FieldPickListName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'fieldpicklist'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
                /// IntroducedVersion              8.0.0.0
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
                //public const string fieldpicklistname = "fieldpicklistname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): State
                ///     (Russian - 1049): Состояние
                /// 
                /// Description:
                ///     (English - United States - 1033): Is Manageed
                ///     (Russian - 1049): Управляется
                /// 
                /// SchemaName: IsManaged
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
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
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.0.0.0
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
                [System.ComponentModel.DescriptionAttribute("State")]
                public const string ismanaged = "ismanaged";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Criteria
                ///     (Russian - 1049): Условия
                /// 
                /// Description:
                ///     (English - United States - 1033): Specify if the mapping is for text match or exact match
                ///     (Russian - 1049): Укажите, режим сопоставления — текстовое совпадение или точное совпадение
                /// 
                /// SchemaName: IsTextMatchMapping
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = True
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Exact Match
                ///     (Russian - 1049): Точное совпадение
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Text Match
                ///     (Russian - 1049): Текстовое совпадение
                /// TrueOption = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.1.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Criteria")]
                public const string istextmatchmapping = "istextmatchmapping";

                ///<summary>
                /// SchemaName: IsTextMatchMappingName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'istextmatchmapping'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.1.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                //public const string istextmatchmappingname = "istextmatchmappingname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Knowledge Search Model Id
                ///     (Russian - 1049): Идентификатор модели поиска в базе знаний
                /// 
                /// Description:
                ///     (English - United States - 1033): Knowledge Search Model associated with entity mapping.
                ///     (Russian - 1049): Модель поиска в базе знаний, связанная с сопоставлением сущности.
                /// 
                /// SchemaName: KnowledgeSearchModelId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: incident
                /// 
                ///     Target incident    PrimaryIdAttribute incidentid    PrimaryNameAttribute title
                ///         DisplayName:
                ///             (English - United States - 1033): Case
                ///             (Russian - 1049): Обращение
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Cases
                ///             (Russian - 1049): Обращения
                ///         
                ///         Description:
                ///             (English - United States - 1033): Service request case associated with a contract.
                ///             (Russian - 1049): Обращение по запросу на обслуживание, связанному с контрактом.
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Knowledge Search Model Id")]
                public const string knowledgesearchmodelid = "knowledgesearchmodelid";

                ///<summary>
                /// SchemaName: KnowledgeSearchModelIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'knowledgesearchmodelid'
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
                /// IntroducedVersion              8.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                //public const string knowledgesearchmodelidname = "knowledgesearchmodelidname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Model Type
                ///     (Russian - 1049): Тип модели
                /// 
                /// Description:
                ///     (English - United States - 1033): Model Type.
                ///     (Russian - 1049): Тип модели.
                /// 
                /// SchemaName: ModelType
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 2147483647
                /// Format = None
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Model Type")]
                public const string modeltype = "modeltype";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Organization
                ///     (Russian - 1049): Организация
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the organization associated with the Text Analytics Entity Mapping.
                ///     (Russian - 1049): Уникальный идентификатор организации, связанной с сопоставлением сущности текстовой аналитики.
                /// 
                /// SchemaName: OrganizationId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: organization
                /// 
                ///     Target organization    PrimaryIdAttribute organizationid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Organization
                ///             (Russian - 1049): Предприятие
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Organizations
                ///             (Russian - 1049): Предприятия
                ///         
                ///         Description:
                ///             (English - United States - 1033): Top level of the Microsoft Dynamics 365 business hierarchy. The organization can be a specific business, holding company, or corporation.
                ///             (Russian - 1049): Верхний уровень бизнес-иерархии Microsoft Dynamics 365. Организация может являться конкретной компанией, холдингом или корпорацией.
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Organization")]
                public const string organizationid = "organizationid";

                ///<summary>
                /// SchemaName: OrganizationIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired    AttributeOf 'organizationid'
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
                /// IntroducedVersion              8.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                //public const string organizationidname = "organizationidname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created On
                ///     (Russian - 1049): Дата создания
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the record was created.
                ///     (Russian - 1049): Дата и время создания записи.
                /// 
                /// SchemaName: OverwriteTime
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateOnly
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.0.0.0
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
                [System.ComponentModel.DescriptionAttribute("Created On")]
                public const string overwritetime = "overwritetime";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Relationship Name
                ///     (Russian - 1049): Имя отношения
                /// 
                /// SchemaName: RelationshipName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Relationship Name")]
                public const string relationshipname = "relationshipname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Similarity Rule Id
                ///     (Russian - 1049): Код правила подобия
                /// 
                /// Description:
                ///     (English - United States - 1033): Similarity Rule associated with entity mapping.
                ///     (Russian - 1049): Правило подобия, связанное с сопоставлением сущностей.
                /// 
                /// SchemaName: SimilarityRuleId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: similarityrule
                /// 
                ///     Target similarityrule    PrimaryIdAttribute similarityruleid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Similarity Rule
                ///             (Russian - 1049): Правило подобия
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Similarity Rules
                ///             (Russian - 1049): Правила подобия
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Similarity Rule Id")]
                public const string similarityruleid = "similarityruleid";

                ///<summary>
                /// SchemaName: SimilarityRuleIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'similarityruleid'
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
                /// IntroducedVersion              8.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                //public const string similarityruleidname = "similarityruleidname";

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
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.0.0.0
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
                [System.ComponentModel.DescriptionAttribute("Solution")]
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
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: False    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.0.0.0
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
                [System.ComponentModel.DescriptionAttribute("Solution")]
                public const string supportingsolutionid = "supportingsolutionid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Text Analytics Entity Mapping Unique Id
                ///     (Russian - 1049): Уникальный идентификатор сопоставления сущности текстовой аналитики
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the Text Analytics Entity Mapping
                /// 
                /// SchemaName: TextAnalyticsEntityMappingIdUnique
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.0.0.0
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
                [System.ComponentModel.DescriptionAttribute("Text Analytics Entity Mapping Unique Id")]
                public const string textanalyticsentitymappingidunique = "textanalyticsentitymappingidunique";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Topic Model Configuration Id
                ///     (Russian - 1049): Код конфигурации модели темы
                /// 
                /// Description:
                ///     (English - United States - 1033): Topic Model Configuration associated with entity mapping.
                ///     (Russian - 1049): Конфигурация модели темы, связанная с сопоставлением сущностей.
                /// 
                /// SchemaName: TopicModelConfigurationId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: topicmodelconfiguration
                /// 
                ///     Target topicmodelconfiguration    PrimaryIdAttribute topicmodelconfigurationid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Topic Model Configuration
                ///             (Russian - 1049): Конфигурация тематической модели
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Topic Model Configurations
                ///             (Russian - 1049): Конфигурации тематических моделей
                ///         
                ///         Description:
                ///             (English - United States - 1033): Configuration settings for identification of topics using text analytics.
                ///             (Russian - 1049): Параметры конфигурации для идентификации тем с помощью текстовой аналитики.
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Topic Model Configuration Id")]
                public const string topicmodelconfigurationid = "topicmodelconfigurationid";

                ///<summary>
                /// SchemaName: TopicModelConfigurationIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'topicmodelconfigurationid'
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
                /// IntroducedVersion              8.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                //public const string topicmodelconfigurationidname = "topicmodelconfigurationidname";
            }

            #endregion Attributes.

            #region OptionSets.

            public static partial class OptionSets
            {

                #region Picklist OptionSet OptionSets.
                ///<summary>
                /// Attribute:
                ///     fieldpicklist
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Field
                ///     (Russian - 1049): Поле
                /// 
                /// Description:
                ///     (English - United States - 1033): Select Field
                ///     (Russian - 1049): Выбрать поле
                /// 
                /// Local System  OptionSet textanalyticsentitymapping_fields
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Field
                ///     (Russian - 1049): Поле
                /// 
                /// Description:
                ///     (English - United States - 1033): Field Description
                ///     (Russian - 1049): Описание поля
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Field")]
                public enum fieldpicklist
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): No
                    ///     (Russian - 1049): Нет
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("No")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    No_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Yes
                    ///     (Russian - 1049): Да
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Yes")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Yes_2 = 2,
                }

                #endregion Picklist OptionSets.
            }

            #endregion OptionSets.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship advancedsimilarityrule_textanalyticsentitymapping
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     advancedsimilarityrule_textanalyticsentitymapping
                /// ReferencingEntityNavigationPropertyName    advancedsimilarityruleid
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          1000
                /// 
                /// ReferencedEntity advancedsimilarityrule:    PrimaryIdAttribute advancedsimilarityruleid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Advanced Similarity Rule
                ///         (Russian - 1049): Расширенное правило подобия
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Advanced Similarity Rules
                ///         (Russian - 1049): Расширенные правила подобия
                ///     
                ///     Description:
                ///         (English - United States - 1033): A text match rule identifies similar records using keywords and key phrases determined with text analytics
                ///         (Russian - 1049): Правило текстового совпадения, выявляющее сходные записи на основе ключевых слов и фраз, которые определяются с помощью текстовой аналитики
                /// 
                /// AttributeMaps:
                ///     SourceEntity                      TargetEntity
                ///     advancedsimilarityrule      ->    textanalyticsentitymapping
                ///     
                ///     SourceAttribute                   TargetAttribute
                ///     advancedsimilarityruleid    ->    advancedsimilarityruleid
                ///     name                        ->    advancedsimilarityruleidname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship advancedsimilarityrule_textanalyticsentitymapping")]
                public static partial class advancedsimilarityrule_textanalyticsentitymapping
                {
                    public const string Name = "advancedsimilarityrule_textanalyticsentitymapping";

                    public const string ReferencedEntity_advancedsimilarityrule = "advancedsimilarityrule";

                    public const string ReferencedAttribute_advancedsimilarityruleid = "advancedsimilarityruleid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_textanalyticsentitymapping = "textanalyticsentitymapping";

                    public const string ReferencingAttribute_advancedsimilarityruleid = "advancedsimilarityruleid";
                }

                ///<summary>
                /// N:1 - Relationship knowledgesearchmodel_textanalyticsentitymapping
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     knowledgesearchmodel_textanalyticsentitymapping
                /// ReferencingEntityNavigationPropertyName    knowledgesearchmodelid
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          1000
                /// 
                /// ReferencedEntity knowledgesearchmodel:    PrimaryIdAttribute knowledgesearchmodelid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Knowledge Search Model
                ///         (Russian - 1049): Модель поиска в базе знаний
                ///     
                ///     Description:
                ///         (English - United States - 1033): Configuration for automatic suggestion of knowledge articles using text analytics and search
                ///         (Russian - 1049): Конфигурация для автоматических рекомендаций статей базы знаний с помощью текстовой аналитики и поиска
                /// 
                /// AttributeMaps:
                ///     SourceEntity                    TargetEntity
                ///     knowledgesearchmodel      ->    textanalyticsentitymapping
                ///     
                ///     SourceAttribute                 TargetAttribute
                ///     knowledgesearchmodelid    ->    knowledgesearchmodelid
                ///     name                      ->    knowledgesearchmodelidname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship knowledgesearchmodel_textanalyticsentitymapping")]
                public static partial class knowledgesearchmodel_textanalyticsentitymapping
                {
                    public const string Name = "knowledgesearchmodel_textanalyticsentitymapping";

                    public const string ReferencedEntity_knowledgesearchmodel = "knowledgesearchmodel";

                    public const string ReferencedAttribute_knowledgesearchmodelid = "knowledgesearchmodelid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_textanalyticsentitymapping = "textanalyticsentitymapping";

                    public const string ReferencingAttribute_knowledgesearchmodelid = "knowledgesearchmodelid";
                }

                ///<summary>
                /// N:1 - Relationship organization_textanalyticsentitymapping
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_textanalyticsentitymapping
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
                /// ReferencedEntity organization:    PrimaryIdAttribute organizationid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Organization
                ///         (Russian - 1049): Предприятие
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Organizations
                ///         (Russian - 1049): Предприятия
                ///     
                ///     Description:
                ///         (English - United States - 1033): Top level of the Microsoft Dynamics 365 business hierarchy. The organization can be a specific business, holding company, or corporation.
                ///         (Russian - 1049): Верхний уровень бизнес-иерархии Microsoft Dynamics 365. Организация может являться конкретной компанией, холдингом или корпорацией.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship organization_textanalyticsentitymapping")]
                public static partial class organization_textanalyticsentitymapping
                {
                    public const string Name = "organization_textanalyticsentitymapping";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_textanalyticsentitymapping = "textanalyticsentitymapping";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// N:1 - Relationship similarityrule_textanalyticsentitymapping
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     similarityrule_textanalyticsentitymapping
                /// ReferencingEntityNavigationPropertyName    similarityruleid
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          1000
                /// 
                /// ReferencedEntity similarityrule:    PrimaryIdAttribute similarityruleid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Similarity Rule
                ///         (Russian - 1049): Правило подобия
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Similarity Rules
                ///         (Russian - 1049): Правила подобия
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship similarityrule_textanalyticsentitymapping")]
                public static partial class similarityrule_textanalyticsentitymapping
                {
                    public const string Name = "similarityrule_textanalyticsentitymapping";

                    public const string ReferencedEntity_similarityrule = "similarityrule";

                    public const string ReferencedAttribute_similarityruleid = "similarityruleid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_textanalyticsentitymapping = "textanalyticsentitymapping";

                    public const string ReferencingAttribute_similarityruleid = "similarityruleid";
                }

                ///<summary>
                /// N:1 - Relationship topicmodelconfiguration_textanalyticsentitymapping
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     topicmodelconfiguration_textanalyticsentitymapping
                /// ReferencingEntityNavigationPropertyName    topicmodelconfigurationid
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          1000
                /// 
                /// ReferencedEntity topicmodelconfiguration:    PrimaryIdAttribute topicmodelconfigurationid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Topic Model Configuration
                ///         (Russian - 1049): Конфигурация тематической модели
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Topic Model Configurations
                ///         (Russian - 1049): Конфигурации тематических моделей
                ///     
                ///     Description:
                ///         (English - United States - 1033): Configuration settings for identification of topics using text analytics.
                ///         (Russian - 1049): Параметры конфигурации для идентификации тем с помощью текстовой аналитики.
                /// 
                /// AttributeMaps:
                ///     SourceEntity                       TargetEntity
                ///     topicmodelconfiguration      ->    textanalyticsentitymapping
                ///     
                ///     SourceAttribute                    TargetAttribute
                ///     name                         ->    topicmodelconfigurationidname
                ///     topicmodelconfigurationid    ->    topicmodelconfigurationid
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship topicmodelconfiguration_textanalyticsentitymapping")]
                public static partial class topicmodelconfiguration_textanalyticsentitymapping
                {
                    public const string Name = "topicmodelconfiguration_textanalyticsentitymapping";

                    public const string ReferencedEntity_topicmodelconfiguration = "topicmodelconfiguration";

                    public const string ReferencedAttribute_topicmodelconfigurationid = "topicmodelconfigurationid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_textanalyticsentitymapping = "textanalyticsentitymapping";

                    public const string ReferencingAttribute_topicmodelconfigurationid = "topicmodelconfigurationid";
                }
            }

            #endregion Relationship ManyToOne - N:1.
        }
    }
}