
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class LanguageLocale
    {
        ///<summary>
        /// DisplayName:
        ///     (English - United States - 1033): Language
        /// 
        /// DisplayCollectionName:
        ///     (English - United States - 1033): Languages
        /// 
        /// PropertyName                          Value
        /// ActivityTypeMask                      1
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
        /// CollectionSchemaName                  LanguageLocales
        /// DataProviderId                        null
        /// DataSourceId                          null
        /// EnforceStateTransitions               False
        /// EntityHelpUrlEnabled                  False
        /// EntitySetName                         languagelocale
        /// IntroducedVersion                     8.0.0.0
        /// IsAIRUpdated                          True
        /// IsActivity                            False
        /// IsActivityParty                       False
        /// IsAvailableOffline                    True
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
        /// IsInteractionCentricEnabled           True
        /// IsIntersect                           False
        /// IsKnowledgeManagementEnabled          False
        /// IsLogicalEntity                       False
        /// IsMailMergeEnabled                    False
        /// IsMappable                            False
        /// IsOfflineInMobileClient               False
        /// IsOneNoteIntegrationEnabled           False
        /// IsOptimisticConcurrencyEnabled        False
        /// IsPrivate                             False
        /// IsQuickCreateEnabled                  False
        /// IsReadOnlyInMobileClient              False
        /// IsReadingPaneEnabled                  True
        /// IsRenameable                          False
        /// IsSLAEnabled                          False
        /// IsStateModelAware                     False
        /// IsValidForAdvancedFind                False
        /// IsValidForQueue                       False
        /// IsVisibleInMobile                     False
        /// IsVisibleInMobileClient               True
        /// LogicalCollectionName                 languagelocales
        /// LogicalName                           languagelocale
        /// ObjectTypeCode                        9957
        /// OwnershipType                         OrganizationOwned
        /// PrimaryIdAttribute                    languagelocaleid
        /// PrimaryNameAttribute                  name
        /// SchemaName                            LanguageLocale
        /// SyncToExternalSearchIndex             False
        /// UsesBusinessDataLabelTable            False
        ///</summary>
        public static partial class Schema
        {
            public const string EntityLogicalName = "languagelocale";

            public const string EntitySchemaName = "LanguageLocale";

            public const string EntityPrimaryIdAttribute = "languagelocaleid";

            public const string EntityPrimaryNameAttribute = "name";

            public const int EntityObjectTypeCode = 9957;

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// Description:
                ///     (English - United States - 1033): LanguageLocaleId
                /// 
                /// SchemaName: LanguageLocaleId
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
                public const string languagelocaleid = "languagelocaleid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Name
                /// 
                /// SchemaName: Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
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
                /// IsCustomizable                 False
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  True
                /// IsRenameable                   False
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                public const string name = "name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Code
                /// 
                /// SchemaName: Code
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
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
                public const string code = "code";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Language
                /// 
                /// SchemaName: Language
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
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
                public const string language = "language";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Locale ID
                /// 
                /// SchemaName: LocaleId
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 1    MaxValue = 2147483647
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
                /// IsGlobalFilterEnabled          True
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              True
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                public const string localeid = "localeid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Organization
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the organization associated with the language locale.
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
                public const string organizationid = "organizationid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Region
                /// 
                /// SchemaName: Region
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
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
                public const string region = "region";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): State Code
                /// 
                /// SchemaName: statecode
                /// StateAttributeMetadata    AttributeType: State    AttributeTypeName: StateType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// DefaultFormValue = Null
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
                /// IsSortableEnabled              True
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string statecode = "statecode";

                ///<summary>
                /// SchemaName: statecodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'statecode'
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
                //public const string statecodename = "statecodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Language Status Code
                /// 
                /// SchemaName: statuscode
                /// StatusAttributeMetadata    AttributeType: Status    AttributeTypeName: StatusType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// DefaultFormValue = Null
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
                public const string statuscode = "statuscode";

                ///<summary>
                /// SchemaName: statuscodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'statuscode'
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
                //public const string statuscodename = "statuscodename";

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
                public const string versionnumber = "versionnumber";
            }

            #endregion Attributes.

            #region OptionSets.

            public static partial class OptionSets
            {
                #region State and Status OptionSets.

                ///<summary>
                /// Attribute: statecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): State Code
                ///</summary>
                public enum statecode
                {
                    ///<summary>
                    /// Default statuscode: Active_1, 1
                    /// InvariantName: Active
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Active
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Active_0 = 0,

                    ///<summary>
                    /// Default statuscode: Inactive_2, 2
                    /// InvariantName: Inactive
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Inactive
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Inactive_1 = 1,
                }

                ///<summary>
                /// Attribute: statuscode
                /// Value Format: Statecode_Statuscode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): State Code
                ///</summary>
                public enum statuscode
                {
                    ///<summary>
                    /// Linked Statecode: Active_0, 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Active
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Active_0_Active_1 = 1,

                    ///<summary>
                    /// Linked Statecode: Inactive_1, 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Inactive
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Inactive_1_Inactive_2 = 2,
                }

                #endregion State and Status OptionSets.
            }

            #endregion OptionSets.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship languagelocale_organization
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     languagelocale_organization
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
                public static partial class languagelocale_organization
                {
                    public const string Name = "languagelocale_organization";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_languagelocale = "languagelocale";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship knowledgearticle_languagelocaleid
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     knowledgearticle_languagelocaleid
                /// ReferencingEntityNavigationPropertyName    languagelocaleid
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Restrict
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity knowledgearticle:    PrimaryIdAttribute knowledgearticleid    PrimaryNameAttribute title
                ///     DisplayName:
                ///         (English - United States - 1033): Knowledge Article
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Knowledge Articles
                ///     
                ///     Description:
                ///         (English - United States - 1033): Organizational knowledge for internal and external use.
                ///</summary>
                public static partial class knowledgearticle_languagelocaleid
                {
                    public const string Name = "knowledgearticle_languagelocaleid";

                    public const string ReferencedEntity_languagelocale = "languagelocale";

                    public const string ReferencedAttribute_languagelocaleid = "languagelocaleid";

                    public const string ReferencingEntity_knowledgearticle = "knowledgearticle";

                    public const string ReferencingAttribute_languagelocaleid = "languagelocaleid";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}