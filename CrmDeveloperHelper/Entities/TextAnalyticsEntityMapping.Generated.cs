
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class TextAnalyticsEntityMapping
    {
        ///<summary>
        /// DisplayName:
        ///     (English - United States - 1033): Text Analytics Entity Mapping
        /// 
        /// DisplayCollectionName:
        ///     (English - United States - 1033): Text Analytics Entity Mappings
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
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier for entity instances
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
                public const string textanalyticsentitymappingid = "textanalyticsentitymappingid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Advanced Similarity RuleId
                /// 
                /// Description:
                ///     (English - United States - 1033): Advanced Similarity RuleId associated with entity mapping.
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
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Cases
                ///         
                ///         Description:
                ///             (English - United States - 1033): Service request case associated with a contract.
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
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
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
                ///         
                ///         Description:
                ///             (English - United States - 1033): The state of this component.
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
                public const string componentstate = "componentstate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Entity
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
                public const string entity = "entity";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Entity Name
                /// 
                /// Description:
                ///     (English - United States - 1033): Entity Display Name
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
                public const string entitydisplayname = "entitydisplayname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Entity
                /// 
                /// Description:
                ///     (English - United States - 1033): Select Entity
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
                public const string field = "field";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Field Name
                /// 
                /// Description:
                ///     (English - United States - 1033): Field Display Name
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
                public const string fielddisplayname = "fielddisplayname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Field
                /// 
                /// Description:
                ///     (English - United States - 1033): Select Field
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
                /// 
                /// Description:
                ///     (English - United States - 1033): Is Manageed
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
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Managed
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
                public const string ismanaged = "ismanaged";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Criteria
                /// 
                /// Description:
                ///     (English - United States - 1033): Specify if the mapping is for text match or exact match
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
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Text Match
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
                /// 
                /// Description:
                ///     (English - United States - 1033): Knowledge Search Model associated with entity mapping.
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
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Cases
                ///         
                ///         Description:
                ///             (English - United States - 1033): Service request case associated with a contract.
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
                /// 
                /// Description:
                ///     (English - United States - 1033): Model Type.
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
                public const string modeltype = "modeltype";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Organization
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the organization associated with the Text Analytics Entity Mapping.
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
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Organizations
                ///         
                ///         Description:
                ///             (English - United States - 1033): Top level of the Microsoft Dynamics 365 business hierarchy. The organization can be a specific business, holding company, or corporation.
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
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the record was created.
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
                public const string overwritetime = "overwritetime";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Relationship Name
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
                public const string relationshipname = "relationshipname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Similarity Rule Id
                /// 
                /// Description:
                ///     (English - United States - 1033): Similarity Rule associated with entity mapping.
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
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Similarity Rules
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
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the associated solution.
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
                public const string solutionid = "solutionid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Solution
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
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
                public const string supportingsolutionid = "supportingsolutionid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Text Analytics Entity Mapping Unique Id
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
                public const string textanalyticsentitymappingidunique = "textanalyticsentitymappingidunique";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Topic Model Configuration Id
                /// 
                /// Description:
                ///     (English - United States - 1033): Topic Model Configuration associated with entity mapping.
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
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Topic Model Configurations
                ///         
                ///         Description:
                ///             (English - United States - 1033): Configuration settings for identification of topics using text analytics.
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
                /// 
                /// Description:
                ///     (English - United States - 1033): Select Field
                /// 
                /// Local System  OptionSet textanalyticsentitymapping_fields
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Field
                /// 
                /// Description:
                ///     (English - United States - 1033): Field Description
                ///</summary>
                public enum fieldpicklist
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): No
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    No_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Yes
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Yes_2 = 2,
                }

                ///<summary>
                /// Attribute:
                ///     componentstate
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Component State
                /// 
                /// Description:
                ///     (English - United States - 1033): The state of this component.
                /// 
                /// Global System  OptionSet componentstate
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Component State
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///</summary>
                public enum componentstate
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Published
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Published_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Unpublished
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Unpublished_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Deleted
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Deleted_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Deleted Unpublished
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Deleted_Unpublished_3 = 3,
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
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Advanced Similarity Rules
                ///     
                ///     Description:
                ///         (English - United States - 1033): A text match rule identifies similar records using keywords and key phrases determined with text analytics
                /// 
                /// AttributeMaps:
                ///     SourceEntity                      TargetEntity
                ///     advancedsimilarityrule      ->    textanalyticsentitymapping
                ///     
                ///     SourceAttribute                   TargetAttribute
                ///     advancedsimilarityruleid    ->    advancedsimilarityruleid
                ///     name                        ->    advancedsimilarityruleidname
                ///</summary>
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
                ///     
                ///     Description:
                ///         (English - United States - 1033): Configuration for automatic suggestion of knowledge articles using text analytics and search
                /// 
                /// AttributeMaps:
                ///     SourceEntity                    TargetEntity
                ///     knowledgesearchmodel      ->    textanalyticsentitymapping
                ///     
                ///     SourceAttribute                 TargetAttribute
                ///     knowledgesearchmodelid    ->    knowledgesearchmodelid
                ///     name                      ->    knowledgesearchmodelidname
                ///</summary>
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
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Organizations
                ///     
                ///     Description:
                ///         (English - United States - 1033): Top level of the Microsoft Dynamics 365 business hierarchy. The organization can be a specific business, holding company, or corporation.
                ///</summary>
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
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Similarity Rules
                ///</summary>
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
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Topic Model Configurations
                ///     
                ///     Description:
                ///         (English - United States - 1033): Configuration settings for identification of topics using text analytics.
                /// 
                /// AttributeMaps:
                ///     SourceEntity                       TargetEntity
                ///     topicmodelconfiguration      ->    textanalyticsentitymapping
                ///     
                ///     SourceAttribute                    TargetAttribute
                ///     name                         ->    topicmodelconfigurationidname
                ///     topicmodelconfigurationid    ->    topicmodelconfigurationid
                ///</summary>
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