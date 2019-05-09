
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class ServiceEndpoint
    {
        ///<summary>
        /// DisplayName:
        ///     (English - United States - 1033): Service Endpoint
        /// 
        /// DisplayCollectionName:
        ///     (English - United States - 1033): Service Endpoints
        /// 
        /// Description:
        ///     (English - United States - 1033): Service endpoint that can be contacted.
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
        /// CollectionSchemaName                  ServiceEndpoints
        /// DataProviderId                        null
        /// DataSourceId                          null
        /// EnforceStateTransitions               False
        /// EntityHelpUrlEnabled                  False
        /// EntitySetName                         serviceendpoints
        /// IntroducedVersion                     5.0.0.0
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
        /// IsValidForAdvancedFind                True
        /// IsValidForQueue                       False
        /// IsVisibleInMobile                     False
        /// IsVisibleInMobileClient               False
        /// LogicalCollectionName                 serviceendpoints
        /// LogicalName                           serviceendpoint
        /// ObjectTypeCode                        4618
        /// OwnershipType                         OrganizationOwned
        /// PrimaryIdAttribute                    serviceendpointid
        /// PrimaryNameAttribute                  name
        /// ReportViewName                        FilteredServiceEndpoint
        /// SchemaName                            ServiceEndpoint
        /// SyncToExternalSearchIndex             False
        /// UsesBusinessDataLabelTable            False
        ///</summary>
        public static partial class Schema
        {
            public const string EntityLogicalName = "serviceendpoint";

            public const string EntitySchemaName = "ServiceEndpoint";

            public const string EntityPrimaryIdAttribute = "serviceendpointid";

            public const string EntityPrimaryNameAttribute = "name";

            public const int EntityObjectTypeCode = 4618;

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the service endpoint.
                /// 
                /// SchemaName: ServiceEndpointId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
                /// IsCustomizable                 False
                /// IsDataSourceSecret             False
                /// IsFilterable                   True
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    True
                /// IsPrimaryName                  False
                /// IsRenameable                   False
                /// IsRequiredForForm              False
                /// IsRetrievable                  True
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                public const string serviceendpointid = "serviceendpointid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Name
                /// 
                /// Description:
                ///     (English - United States - 1033): Name of Service end point.
                /// 
                /// SchemaName: Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
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
                /// IsPrimaryName                  True
                /// IsRenameable                   False
                /// IsRequiredForForm              False
                /// IsRetrievable                  True
                /// IsSearchable                   True
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string name = "name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Specifies mode of authentication with SB
                /// 
                /// SchemaName: AuthType
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet serviceendpoint_authtype
                /// DefaultFormValue = Null
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.1.0.0
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
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string authtype = "authtype";

                ///<summary>
                /// SchemaName: AuthTypeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'authtype'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.1.0.0
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
                //public const string authtypename = "authtypename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Authentication Value
                /// 
                /// SchemaName: AuthValue
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: False    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 2048
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              9.0.0.0
                /// IsCustomizable                 False
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
                /// IsValidForGrid                 False
                ///</summary>
                public const string authvalue = "authvalue";

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
                public const string componentstate = "componentstate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Connection Mode
                /// 
                /// Description:
                ///     (English - United States - 1033): Connection mode to contact the service endpoint.
                /// 
                /// SchemaName: ConnectionMode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet serviceendpoint_connectionmode
                /// DefaultFormValue = 1
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
                /// IsRetrievable                  True
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string connectionmode = "connectionmode";

                ///<summary>
                /// SchemaName: ConnectionModeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'connectionmode'
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
                //public const string connectionmodename = "connectionmodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Contract
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of the endpoint contract.
                /// 
                /// SchemaName: Contract
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet serviceendpoint_contract
                /// DefaultFormValue = 1
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
                /// IsRetrievable                  True
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string contract = "contract";

                ///<summary>
                /// SchemaName: ContractName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'contract'
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
                //public const string contractname = "contractname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who created the service endpoint.
                /// 
                /// SchemaName: CreatedBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///             (English - United States - 1033): User
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Users
                ///         
                ///         Description:
                ///             (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
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
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string createdby = "createdby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created On
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the service endpoint was created.
                /// 
                /// SchemaName: CreatedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateAndTime
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
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string createdon = "createdon";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By (Delegate)
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the delegate user who created the service endpoint.
                /// 
                /// SchemaName: CreatedOnBehalfBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///             (English - United States - 1033): User
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Users
                ///         
                ///         Description:
                ///             (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
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
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string createdonbehalfby = "createdonbehalfby";

                ///<summary>
                /// SchemaName: CreatedOnBehalfByName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'createdonbehalfby'
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
                //public const string createdonbehalfbyname = "createdonbehalfbyname";

                ///<summary>
                /// SchemaName: CreatedOnBehalfByYomiName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'createdonbehalfby'
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
                //public const string createdonbehalfbyyominame = "createdonbehalfbyyominame";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Description
                /// 
                /// Description:
                ///     (English - United States - 1033): Description of the service endpoint.
                /// 
                /// SchemaName: Description
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 2000
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
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
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string description = "description";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Introduced Version
                /// 
                /// Description:
                ///     (English - United States - 1033): Version in which the form is introduced.
                /// 
                /// SchemaName: IntroducedVersion
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 48
                /// Format = VersionNumber    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
                /// IntroducedVersion              6.0.0.0
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
                public const string introducedversion = "introducedversion";

                ///<summary>
                /// SchemaName: IsAuthValueSet
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): No
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Yes
                /// TrueOption = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              9.0.0.0
                /// IsCustomizable                 False
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
                public const string isauthvalueset = "isauthvalueset";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Customizable
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether this component can be customized.
                /// 
                /// SchemaName: IsCustomizable
                /// ManagedPropertyAttributeMetadata    AttributeType: ManagedProperty    AttributeTypeName: ManagedPropertyType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// ManagedPropertyLogicalName = iscustomizableanddeletable    ValueAttributeTypeCode = Boolean
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
                public const string iscustomizable = "iscustomizable";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): State
                /// 
                /// Description:
                ///     (English - United States - 1033): Information that specifies whether this component is managed.
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
                /// IsValidForGrid                 True
                ///</summary>
                public const string ismanaged = "ismanaged";

                ///<summary>
                /// SchemaName: IsManagedName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'ismanaged'
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
                //public const string ismanagedname = "ismanagedname";

                ///<summary>
                /// SchemaName: IsSASKeySet
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): No
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Yes
                /// TrueOption = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.1.0.0
                /// IsCustomizable                 False
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
                public const string issaskeyset = "issaskeyset";

                ///<summary>
                /// SchemaName: IsSASTokenSet
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): No
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Yes
                /// TrueOption = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.1.0.0
                /// IsCustomizable                 False
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
                public const string issastokenset = "issastokenset";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Content type of the message
                /// 
                /// SchemaName: MessageFormat
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet serviceendpoint_messageformat
                /// DefaultFormValue = Null
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.1.0.0
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
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string messageformat = "messageformat";

                ///<summary>
                /// SchemaName: MessageFormatName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'messageformat'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.1.0.0
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
                //public const string messageformatname = "messageformatname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who last modified the service endpoint.
                /// 
                /// SchemaName: ModifiedBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///             (English - United States - 1033): User
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Users
                ///         
                ///         Description:
                ///             (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
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
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string modifiedby = "modifiedby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified On
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the service endpoint was last modified.
                /// 
                /// SchemaName: ModifiedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateAndTime
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
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string modifiedon = "modifiedon";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By (Delegate)
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the delegate user who modified the service endpoint.
                /// 
                /// SchemaName: ModifiedOnBehalfBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///             (English - United States - 1033): User
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Users
                ///         
                ///         Description:
                ///             (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
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
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string modifiedonbehalfby = "modifiedonbehalfby";

                ///<summary>
                /// SchemaName: ModifiedOnBehalfByName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'modifiedonbehalfby'
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
                //public const string modifiedonbehalfbyname = "modifiedonbehalfbyname";

                ///<summary>
                /// SchemaName: ModifiedOnBehalfByYomiName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'modifiedonbehalfby'
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
                //public const string modifiedonbehalfbyyominame = "modifiedonbehalfbyyominame";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Namespace Address
                /// 
                /// Description:
                ///     (English - United States - 1033): Full service endpoint address.
                /// 
                /// SchemaName: NamespaceAddress
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 255
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.1.0.0
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
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string namespaceaddress = "namespaceaddress";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Format of Service Bus Namespace
                /// 
                /// SchemaName: NamespaceFormat
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet serviceendpoint_namespaceformat
                /// DefaultFormValue = Null
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.1.0.0
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
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string namespaceformat = "namespaceformat";

                ///<summary>
                /// SchemaName: NamespaceFormatName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'namespaceformat'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.1.0.0
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
                //public const string namespaceformatname = "namespaceformatname";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the organization with which the service endpoint is associated.
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
                public const string organizationid = "organizationid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Record Overwrite Time
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
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
                public const string overwritetime = "overwritetime";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Path
                /// 
                /// Description:
                ///     (English - United States - 1033): Path to the service endpoint.
                /// 
                /// SchemaName: Path
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 160
                /// Format = Text    ImeMode = Active    IsLocalizable = False
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
                /// IsRetrievable                  True
                /// IsSearchable                   True
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string path = "path";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Shared Access Key
                /// 
                /// SchemaName: SASKey
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: False    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.1.0.0
                /// IsCustomizable                 False
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
                /// IsValidForGrid                 False
                ///</summary>
                public const string saskey = "saskey";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Shared Access Key Name
                /// 
                /// SchemaName: SASKeyName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.1.0.0
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
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string saskeyname = "saskeyname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Shared Access Token
                /// 
                /// SchemaName: SASToken
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: False    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.1.0.0
                /// IsCustomizable                 False
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
                /// IsValidForGrid                 False
                ///</summary>
                public const string sastoken = "sastoken";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the service endpoint.
                /// 
                /// SchemaName: ServiceEndpointIdUnique
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
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
                public const string serviceendpointidunique = "serviceendpointidunique";

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
                public const string solutionid = "solutionid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Service Namespace
                /// 
                /// Description:
                ///     (English - United States - 1033): Namespace of the App Fabric solution.
                /// 
                /// SchemaName: SolutionNamespace
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 160
                /// Format = Text    ImeMode = Active    IsLocalizable = False
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
                /// IsRetrievable                  True
                /// IsSearchable                   True
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string solutionnamespace = "solutionnamespace";

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
                /// IsValidForRead: False    IsValidForAdvancedFind: True
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
                /// IsValidForGrid                 True
                ///</summary>
                public const string supportingsolutionid = "supportingsolutionid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Url Address
                /// 
                /// Description:
                ///     (English - United States - 1033): Full service endpoint Url.
                /// 
                /// SchemaName: Url
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 2000
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              9.0.0.0
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
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string url = "url";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): User Claim
                /// 
                /// Description:
                ///     (English - United States - 1033): Additional user claim value type.
                /// 
                /// SchemaName: UserClaim
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet serviceendpoint_userclaim
                /// DefaultFormValue = 1
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
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string userclaim = "userclaim";

                ///<summary>
                /// SchemaName: UserClaimName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'userclaim'
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
                //public const string userclaimname = "userclaimname";
            }

            #endregion Attributes.

            #region OptionSets.

            public static partial class OptionSets
            {

                #region Picklist OptionSet OptionSets.
                ///<summary>
                /// Attribute:
                ///     authtype
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Specifies mode of authentication with SB
                /// 
                /// Local System  OptionSet serviceendpoint_authtype
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Authentication type for Service Bus
                ///</summary>
                public enum authtype
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): ACS
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    ACS_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): SAS Key
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    SAS_Key_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): SAS Token
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    SAS_Token_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Webhook Key
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Webhook_Key_4 = 4,

                    ///<summary>
                    /// 5
                    /// DisplayOrder: 5
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Http Header
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Http_Header_5 = 5,

                    ///<summary>
                    /// 6
                    /// DisplayOrder: 6
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Http Query String
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Http_Query_String_6 = 6,
                }

                ///<summary>
                /// Attribute:
                ///     connectionmode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Connection Mode
                /// 
                /// Description:
                ///     (English - United States - 1033): Connection mode to contact the service endpoint.
                /// 
                /// Local System  OptionSet serviceendpoint_connectionmode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Connection mode
                /// 
                /// Description:
                ///     (English - United States - 1033): Connection mode.
                ///</summary>
                public enum connectionmode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Normal
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Normal_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Federated
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Federated_2 = 2,
                }

                ///<summary>
                /// Attribute:
                ///     contract
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Contract
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of the endpoint contract.
                /// 
                /// Local System  OptionSet serviceendpoint_contract
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Contract
                /// 
                /// Description:
                ///     (English - United States - 1033): Contract type.
                ///</summary>
                public enum contract
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): OneWay
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    OneWay_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Queue
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Queue_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Rest
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Rest_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): TwoWay
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    TwoWay_4 = 4,

                    ///<summary>
                    /// 5
                    /// DisplayOrder: 5
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Topic
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Topic_5 = 5,

                    ///<summary>
                    /// 6
                    /// DisplayOrder: 6
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Queue (Persistent)
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Queue_Persistent_6 = 6,

                    ///<summary>
                    /// 7
                    /// DisplayOrder: 7
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Event Hub
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Event_Hub_7 = 7,

                    ///<summary>
                    /// 8
                    /// DisplayOrder: 8
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Webhook
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Webhook_8 = 8,
                }

                ///<summary>
                /// Attribute:
                ///     messageformat
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Content type of the message
                /// 
                /// Local System  OptionSet serviceendpoint_messageformat
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Message Format
                /// 
                /// Description:
                ///     (English - United States - 1033):  Format type.
                ///</summary>
                public enum messageformat
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Binary XML
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Binary_XML_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Json
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Json_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Text XML
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Text_XML_3 = 3,
                }

                ///<summary>
                /// Attribute:
                ///     namespaceformat
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Format of Service Bus Namespace
                /// 
                /// Local System  OptionSet serviceendpoint_namespaceformat
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Format for Service Bus Namespace
                ///</summary>
                public enum namespaceformat
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Namespace Name
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Namespace_Name_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Namespace Address
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Namespace_Address_2 = 2,
                }

                ///<summary>
                /// Attribute:
                ///     userclaim
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): User Claim
                /// 
                /// Description:
                ///     (English - United States - 1033): Additional user claim value type.
                /// 
                /// Local System  OptionSet serviceendpoint_userclaim
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): User claim
                /// 
                /// Description:
                ///     (English - United States - 1033): User claim value type.
                ///</summary>
                public enum userclaim
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): None
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    None_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): UserId
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    UserId_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): UserInfo
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    UserInfo_3 = 3,
                }

                #endregion Picklist OptionSets.
            }

            #endregion OptionSets.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship createdby_serviceendpoint
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     createdby_serviceendpoint
                /// ReferencingEntityNavigationPropertyName    createdby
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
                /// ReferencedEntity systemuser:    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///     DisplayName:
                ///         (English - United States - 1033): User
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///     
                ///     Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///</summary>
                public static partial class createdby_serviceendpoint
                {
                    public const string Name = "createdby_serviceendpoint";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_serviceendpoint = "serviceendpoint";

                    public const string ReferencingAttribute_createdby = "createdby";
                }

                ///<summary>
                /// N:1 - Relationship lk_serviceendpointbase_createdonbehalfby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_serviceendpointbase_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    createdonbehalfby
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
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
                /// ReferencedEntity systemuser:    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///     DisplayName:
                ///         (English - United States - 1033): User
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///     
                ///     Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///</summary>
                public static partial class lk_serviceendpointbase_createdonbehalfby
                {
                    public const string Name = "lk_serviceendpointbase_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_serviceendpoint = "serviceendpoint";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_serviceendpointbase_modifiedonbehalfby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_serviceendpointbase_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    modifiedonbehalfby
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
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
                /// ReferencedEntity systemuser:    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///     DisplayName:
                ///         (English - United States - 1033): User
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///     
                ///     Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///</summary>
                public static partial class lk_serviceendpointbase_modifiedonbehalfby
                {
                    public const string Name = "lk_serviceendpointbase_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_serviceendpoint = "serviceendpoint";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship modifiedby_serviceendpoint
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     modifiedby_serviceendpoint
                /// ReferencingEntityNavigationPropertyName    modifiedby
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
                /// ReferencedEntity systemuser:    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///     DisplayName:
                ///         (English - United States - 1033): User
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///     
                ///     Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///</summary>
                public static partial class modifiedby_serviceendpoint
                {
                    public const string Name = "modifiedby_serviceendpoint";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_serviceendpoint = "serviceendpoint";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";
                }

                ///<summary>
                /// N:1 - Relationship organization_serviceendpoint
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_serviceendpoint
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
                public static partial class organization_serviceendpoint
                {
                    public const string Name = "organization_serviceendpoint";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_serviceendpoint = "serviceendpoint";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship serviceendpoint_sdkmessageprocessingstep
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     serviceendpoint_sdkmessageprocessingstep
                /// ReferencingEntityNavigationPropertyName    eventhandler_serviceendpoint
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
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
                /// ReferencingEntity sdkmessageprocessingstep:    PrimaryIdAttribute sdkmessageprocessingstepid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Sdk Message Processing Step
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Sdk Message Processing Steps
                ///     
                ///     Description:
                ///         (English - United States - 1033): Stage in the execution pipeline that a plug-in is to execute.
                ///</summary>
                public static partial class serviceendpoint_sdkmessageprocessingstep
                {
                    public const string Name = "serviceendpoint_sdkmessageprocessingstep";

                    public const string ReferencedEntity_serviceendpoint = "serviceendpoint";

                    public const string ReferencedAttribute_serviceendpointid = "serviceendpointid";

                    public const string ReferencingEntity_sdkmessageprocessingstep = "sdkmessageprocessingstep";

                    public const string ReferencingAttribute_eventhandler = "eventhandler";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_serviceendpoint
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_serviceendpoint
                /// ReferencingEntityNavigationPropertyName    objectid_serviceendpoint
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
                ///     
                ///     Description:
                ///         (English - United States - 1033): Per User item instance data
                ///</summary>
                public static partial class userentityinstancedata_serviceendpoint
                {
                    public const string Name = "userentityinstancedata_serviceendpoint";

                    public const string ReferencedEntity_serviceendpoint = "serviceendpoint";

                    public const string ReferencedAttribute_serviceendpointid = "serviceendpointid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}