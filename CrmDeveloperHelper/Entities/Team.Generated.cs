
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class Team
    {
        ///<summary>
        /// DisplayName:
        ///     (English - United States - 1033): Team
        /// 
        /// DisplayCollectionName:
        ///     (English - United States - 1033): Teams
        /// 
        /// Description:
        ///     (English - United States - 1033): Collection of system users that routinely collaborate. Teams can be used to simplify record sharing and provide team members with common access to organization data when team members belong to different Business Units.
        /// 
        /// PropertyName                          Value
        /// ActivityTypeMask                      0
        /// AutoCreateAccessTeams                 False
        /// AutoRouteToOwnerQueue                 False
        /// CanBeInManyToMany                     True
        /// CanBePrimaryEntityInRelationship      True
        /// CanBeRelatedEntityInRelationship      True
        /// CanChangeHierarchicalRelationship     True
        /// CanChangeTrackingBeEnabled            True
        /// CanCreateAttributes                   True
        /// CanCreateCharts                       True
        /// CanCreateForms                        True
        /// CanCreateViews                        True
        /// CanEnableSyncToExternalSearchIndex    True
        /// CanModifyAdditionalSettings           True
        /// CanTriggerWorkflow                    True
        /// ChangeTrackingEnabled                 True
        /// CollectionSchemaName                  Teams
        /// DataProviderId                        null
        /// DataSourceId                          null
        /// EnforceStateTransitions               False
        /// EntityHelpUrlEnabled                  False
        /// EntitySetName                         teams
        /// IntroducedVersion                     5.0.0.0
        /// IsAIRUpdated                          True
        /// IsActivity                            False
        /// IsActivityParty                       False
        /// IsAvailableOffline                    True
        /// IsBPFEntity                           False
        /// IsBusinessProcessEnabled              True
        /// IsChildEntity                         False
        /// IsConnectionsEnabled                  True
        /// IsCustomEntity                        False
        /// IsCustomizable                        True
        /// IsDocumentManagementEnabled           False
        /// IsDocumentRecommendationsEnabled      False
        /// IsDuplicateDetectionEnabled           False
        /// IsEnabledForCharts                    True
        /// IsEnabledForExternalChannels          False
        /// IsEnabledForTrace                     False
        /// IsImportable                          True
        /// IsInteractionCentricEnabled           False
        /// IsIntersect                           False
        /// IsKnowledgeManagementEnabled          False
        /// IsLogicalEntity                       False
        /// IsMailMergeEnabled                    False
        /// IsMappable                            True
        /// IsOfflineInMobileClient               True
        /// IsOneNoteIntegrationEnabled           False
        /// IsOptimisticConcurrencyEnabled        True
        /// IsPrivate                             False
        /// IsQuickCreateEnabled                  False
        /// IsReadOnlyInMobileClient              True
        /// IsReadingPaneEnabled                  True
        /// IsRenameable                          False
        /// IsSLAEnabled                          False
        /// IsStateModelAware                     False
        /// IsValidForAdvancedFind                True
        /// IsValidForQueue                       False
        /// IsVisibleInMobile                     False
        /// IsVisibleInMobileClient               True
        /// LogicalCollectionName                 teams
        /// LogicalName                           team
        /// MobileOfflineFilters                  <fetch version="1.0" output-format="xml-platform" mapping="logical" distinct="false">
        /// MobileOfflineFilters                        <entity name="team">
        /// MobileOfflineFilters                          <filter type="and">
        /// MobileOfflineFilters                            <condition attribute="modifiedon" operator="on-or-after" value="1900-01-01"/>
        /// MobileOfflineFilters                          </filter>
        /// MobileOfflineFilters                        </entity>
        /// MobileOfflineFilters                      </fetch>
        /// ObjectTypeCode                        9
        /// OwnershipType                         BusinessOwned
        /// PrimaryIdAttribute                    teamid
        /// PrimaryNameAttribute                  name
        /// ReportViewName                        FilteredTeam
        /// SchemaName                            Team
        /// SyncToExternalSearchIndex             False
        /// UsesBusinessDataLabelTable            False
        ///</summary>
        public static partial class Schema
        {
            public const string EntityLogicalName = "team";

            public const string EntitySchemaName = "Team";

            public const string EntityPrimaryIdAttribute = "teamid";

            public const string EntityPrimaryNameAttribute = "name";

            public const int EntityObjectTypeCode = 9;

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Team
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier for the team.
                /// 
                /// SchemaName: TeamId
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
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   True
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    True
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  True
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                public const string teamid = "teamid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Team Name
                /// 
                /// Description:
                ///     (English - United States - 1033): Name of the team.
                /// 
                /// SchemaName: Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 160
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  True
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  True
                /// IsSearchable                   True
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 True
                ///</summary>
                public const string name = "name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Administrator
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user primary responsible for the team.
                /// 
                /// SchemaName: AdministratorId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
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
                /// IsValidForGrid                 True
                ///</summary>
                public const string administratorid = "administratorid";

                ///<summary>
                /// SchemaName: AdministratorIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'administratorid'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
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
                //public const string administratoridname = "administratoridname";

                ///<summary>
                /// SchemaName: AdministratorIdYomiName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'administratorid'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
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
                //public const string administratoridyominame = "administratoridyominame";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Business Unit
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the business unit with which the team is associated.
                /// 
                /// SchemaName: BusinessUnitId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: businessunit
                /// 
                ///     Target businessunit    PrimaryIdAttribute businessunitid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Business Unit
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Business Units
                ///         
                ///         Description:
                ///             (English - United States - 1033): Business, division, or department in the Microsoft Dynamics 365 database.
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   True
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 True
                ///</summary>
                public const string businessunitid = "businessunitid";

                ///<summary>
                /// SchemaName: BusinessUnitIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired    AttributeOf 'businessunitid'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
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
                //public const string businessunitidname = "businessunitidname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who created the team.
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
                public const string createdby = "createdby";

                ///<summary>
                /// SchemaName: CreatedByName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'createdby'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
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
                //public const string createdbyname = "createdbyname";

                ///<summary>
                /// SchemaName: CreatedByYomiName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'createdby'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
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
                //public const string createdbyyominame = "createdbyyominame";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created On
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the team was created.
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
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   True
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  True
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who created the team.
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
                /// CanModifyAdditionalSettings    False
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
                /// CanModifyAdditionalSettings    False
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
                ///     (English - United States - 1033): Description of the team.
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
                public const string description = "description";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Email
                /// 
                /// Description:
                ///     (English - United States - 1033): Email address for the team.
                /// 
                /// SchemaName: EMailAddress
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Email    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
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
                public const string emailaddress = "emailaddress";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Exchange Rate
                /// 
                /// Description:
                ///     (English - United States - 1033): Exchange rate for the currency associated with the team with respect to the base currency.
                /// 
                /// SchemaName: ExchangeRate
                /// DecimalAttributeMetadata    AttributeType: Decimal    AttributeTypeName: DecimalType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0,0000000001    MaxValue = 100000000000    Precision = 10
                /// ImeMode = Disabled
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
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
                public const string exchangerate = "exchangerate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Import Sequence Number
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the data import or data migration that created this record.
                /// 
                /// SchemaName: ImportSequenceNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
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
                public const string importsequencenumber = "importsequencenumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Default
                /// 
                /// Description:
                ///     (English - United States - 1033): Information about whether the team is a default business unit team.
                /// 
                /// SchemaName: IsDefault
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
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
                /// CanModifyAdditionalSettings    False
                /// IntroducedVersion              5.0.0.0
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
                /// IsValidForGrid                 True
                ///</summary>
                public const string isdefault = "isdefault";

                ///<summary>
                /// SchemaName: IsDefaultName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'isdefault'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
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
                //public const string isdefaultname = "isdefaultname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who last modified the team.
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
                public const string modifiedby = "modifiedby";

                ///<summary>
                /// SchemaName: ModifiedByName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'modifiedby'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
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
                //public const string modifiedbyname = "modifiedbyname";

                ///<summary>
                /// SchemaName: ModifiedByYomiName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'modifiedby'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
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
                //public const string modifiedbyyominame = "modifiedbyyominame";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified On
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the team was last modified.
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
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   True
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  True
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who last modified the team.
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
                /// CanModifyAdditionalSettings    False
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
                /// CanModifyAdditionalSettings    False
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
                ///     (English - United States - 1033): Organization 
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the organization associated with the team.
                /// 
                /// SchemaName: OrganizationId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
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
                /// CanModifyAdditionalSettings    False
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
                //public const string organizationidname = "organizationidname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Record Created On
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time that the record was migrated.
                /// 
                /// SchemaName: OverriddenCreatedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateOnly
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
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
                /// IsValidForGrid                 True
                ///</summary>
                public const string overriddencreatedon = "overriddencreatedon";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Process
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the ID of the process.
                /// 
                /// SchemaName: ProcessId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
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
                /// IsValidForGrid                 True
                ///</summary>
                public const string processid = "processid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Default Queue
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the default queue for the team.
                /// 
                /// SchemaName: QueueId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: queue
                /// 
                ///     Target queue    PrimaryIdAttribute queueid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Queue
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Queues
                ///         
                ///         Description:
                ///             (English - United States - 1033): A list of records that require action, such as accounts, activities, and cases.
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
                /// IntroducedVersion              5.0.0.0
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
                /// IsValidForGrid                 True
                ///</summary>
                public const string queueid = "queueid";

                ///<summary>
                /// SchemaName: QueueIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'queueid'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 400
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
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
                //public const string queueidname = "queueidname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Regarding Object Id
                /// 
                /// Description:
                ///     (English - United States - 1033): Choose the record that the team relates to.
                /// 
                /// SchemaName: RegardingObjectId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: knowledgearticle,opportunity
                /// 
                ///     Target knowledgearticle    PrimaryIdAttribute knowledgearticleid    PrimaryNameAttribute title
                ///         DisplayName:
                ///             (English - United States - 1033): Knowledge Article
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Knowledge Articles
                ///         
                ///         Description:
                ///             (English - United States - 1033): Organizational knowledge for internal and external use.
                /// 
                ///     Target opportunity    PrimaryIdAttribute opportunityid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Opportunity
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Opportunities
                ///         
                ///         Description:
                ///             (English - United States - 1033): Potential revenue-generating event, or sale to an account, which needs to be tracked through a sales process to completion.
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              6.0.0.0
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
                public const string regardingobjectid = "regardingobjectid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Regarding Object Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of the associated record for team - used for system managed access teams only.
                /// 
                /// SchemaName: RegardingObjectTypeCode
                /// EntityNameAttributeMetadata    AttributeType: EntityName    AttributeTypeName: EntityNameType    RequiredLevel: None    AttributeOf 'regardingobjectid'
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
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
                //public const string regardingobjecttypecode = "regardingobjecttypecode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Process Stage
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the ID of the stage.
                /// 
                /// SchemaName: StageId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
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
                /// IsValidForGrid                 True
                ///</summary>
                public const string stageid = "stageid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is System Managed
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether the team will be managed by the system.
                /// 
                /// SchemaName: SystemManaged
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
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
                /// CanModifyAdditionalSettings    False
                /// IntroducedVersion              6.0.0.0
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
                /// IsValidForGrid                 True
                ///</summary>
                public const string systemmanaged = "systemmanaged";

                ///<summary>
                /// SchemaName: SystemManagedName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'systemmanaged'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
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
                //public const string systemmanagedname = "systemmanagedname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Team Template Identifier
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the team template that is associated with the team.
                /// 
                /// SchemaName: TeamTemplateId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: teamtemplate
                /// 
                ///     Target teamtemplate    PrimaryIdAttribute teamtemplateid    PrimaryNameAttribute teamtemplatename
                ///         DisplayName:
                ///             (English - United States - 1033): Team template
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Team templates
                ///         
                ///         Description:
                ///             (English - United States - 1033): Team template for an entity enabled for automatically created access teams.
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              6.0.0.0
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
                public const string teamtemplateid = "teamtemplateid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Team Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the team type.
                /// 
                /// SchemaName: TeamType
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet team_type
                /// DefaultFormValue = 0
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
                /// IntroducedVersion              6.0.0.0
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
                /// IsValidForGrid                 True
                ///</summary>
                public const string teamtype = "teamtype";

                ///<summary>
                /// SchemaName: TeamTypeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'teamtype'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
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
                //public const string teamtypename = "teamtypename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Currency
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the currency associated with the team.
                /// 
                /// SchemaName: TransactionCurrencyId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: transactioncurrency
                /// 
                ///     Target transactioncurrency    PrimaryIdAttribute transactioncurrencyid    PrimaryNameAttribute currencyname
                ///         DisplayName:
                ///             (English - United States - 1033): Currency
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Currencies
                ///         
                ///         Description:
                ///             (English - United States - 1033): Currency in which a financial transaction is carried out.
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
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
                public const string transactioncurrencyid = "transactioncurrencyid";

                ///<summary>
                /// SchemaName: TransactionCurrencyIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'transactioncurrencyid'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
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
                //public const string transactioncurrencyidname = "transactioncurrencyidname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Traversed Path
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                /// 
                /// SchemaName: TraversedPath
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1250
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
                /// IntroducedVersion              7.0.0.0
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
                public const string traversedpath = "traversedpath";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Version number
                /// 
                /// Description:
                ///     (English - United States - 1033): Version number of the team.
                /// 
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
                /// CanModifyAdditionalSettings    False
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
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                public const string versionnumber = "versionnumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Yomi Name
                /// 
                /// Description:
                ///     (English - United States - 1033): Pronunciation of the full name of the team, written in phonetic hiragana or katakana characters.
                /// 
                /// SchemaName: YomiName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 160
                /// Format = PhoneticGuide    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
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
                public const string yominame = "yominame";
            }

            #endregion Attributes.

            #region OptionSets.

            public static partial class OptionSets
            {

                #region Picklist OptionSet OptionSets.
                ///<summary>
                /// Attribute:
                ///     teamtype
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Team Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the team type.
                /// 
                /// Local System  OptionSet team_type
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Team Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Information about team type.
                ///</summary>
                public enum teamtype
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Owner
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Owner_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Access
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Access_1 = 1,
                }

                #endregion Picklist OptionSets.
            }

            #endregion OptionSets.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship business_unit_teams
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_teams
                /// ReferencingEntityNavigationPropertyName    businessunitid
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Restrict
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity businessunit:    PrimaryIdAttribute businessunitid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Business Unit
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Business Units
                ///     
                ///     Description:
                ///         (English - United States - 1033): Business, division, or department in the Microsoft Dynamics 365 database.
                /// 
                /// AttributeMaps:
                ///     SourceEntity             TargetEntity
                ///     businessunit       ->    team
                ///     
                ///     SourceAttribute          TargetAttribute
                ///     businessunitid     ->    businessunitid
                ///     name               ->    businessunitidname
                ///</summary>
                public static partial class business_unit_teams
                {
                    public const string Name = "business_unit_teams";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_businessunitid = "businessunitid";
                }

                ///<summary>
                /// N:1 - Relationship knowledgearticle_Teams
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     knowledgearticle_Teams
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_knowledgearticle
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity knowledgearticle:    PrimaryIdAttribute knowledgearticleid    PrimaryNameAttribute title
                ///     DisplayName:
                ///         (English - United States - 1033): Knowledge Article
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Knowledge Articles
                ///     
                ///     Description:
                ///         (English - United States - 1033): Organizational knowledge for internal and external use.
                ///</summary>
                public static partial class knowledgearticle_teams
                {
                    public const string Name = "knowledgearticle_Teams";

                    public const string ReferencedEntity_knowledgearticle = "knowledgearticle";

                    public const string ReferencedAttribute_knowledgearticleid = "knowledgearticleid";

                    public const string ReferencedEntity_PrimaryNameAttribute_title = "title";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";
                }

                ///<summary>
                /// N:1 - Relationship lk_team_createdonbehalfby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_team_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    createdonbehalfby
                /// IsCustomizable                             True
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
                public static partial class lk_team_createdonbehalfby
                {
                    public const string Name = "lk_team_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_team_modifiedonbehalfby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_team_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    modifiedonbehalfby
                /// IsCustomizable                             True
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
                public static partial class lk_team_modifiedonbehalfby
                {
                    public const string Name = "lk_team_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_teambase_administratorid
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_teambase_administratorid
                /// ReferencingEntityNavigationPropertyName    administratorid
                /// IsCustomizable                             True
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
                public static partial class lk_teambase_administratorid
                {
                    public const string Name = "lk_teambase_administratorid";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_administratorid = "administratorid";
                }

                ///<summary>
                /// N:1 - Relationship lk_teambase_createdby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_teambase_createdby
                /// ReferencingEntityNavigationPropertyName    createdby
                /// IsCustomizable                             True
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
                public static partial class lk_teambase_createdby
                {
                    public const string Name = "lk_teambase_createdby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_createdby = "createdby";
                }

                ///<summary>
                /// N:1 - Relationship lk_teambase_modifiedby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_teambase_modifiedby
                /// ReferencingEntityNavigationPropertyName    modifiedby
                /// IsCustomizable                             True
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
                public static partial class lk_teambase_modifiedby
                {
                    public const string Name = "lk_teambase_modifiedby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";
                }

                ///<summary>
                /// N:1 - Relationship opportunity_Teams
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     opportunity_Teams
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_opportunity
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity opportunity:    PrimaryIdAttribute opportunityid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Opportunity
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Opportunities
                ///     
                ///     Description:
                ///         (English - United States - 1033): Potential revenue-generating event, or sale to an account, which needs to be tracked through a sales process to completion.
                ///</summary>
                public static partial class opportunity_teams
                {
                    public const string Name = "opportunity_Teams";

                    public const string ReferencedEntity_opportunity = "opportunity";

                    public const string ReferencedAttribute_opportunityid = "opportunityid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";
                }

                ///<summary>
                /// N:1 - Relationship organization_teams
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_teams
                /// ReferencingEntityNavigationPropertyName    organizationid_organization
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
                public static partial class organization_teams
                {
                    public const string Name = "organization_teams";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// N:1 - Relationship processstage_teams
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     processstage_teams
                /// ReferencingEntityNavigationPropertyName    stageid_processstage
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
                /// ReferencedEntity processstage:    PrimaryIdAttribute processstageid    PrimaryNameAttribute stagename
                ///     DisplayName:
                ///         (English - United States - 1033): Process Stage
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Process Stages
                ///     
                ///     Description:
                ///         (English - United States - 1033): Stage associated with a process.
                ///</summary>
                public static partial class processstage_teams
                {
                    public const string Name = "processstage_teams";

                    public const string ReferencedEntity_processstage = "processstage";

                    public const string ReferencedAttribute_processstageid = "processstageid";

                    public const string ReferencedEntity_PrimaryNameAttribute_stagename = "stagename";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_stageid = "stageid";
                }

                ///<summary>
                /// N:1 - Relationship queue_team
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     queue_team
                /// ReferencingEntityNavigationPropertyName    queueid
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                RemoveLink
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity queue:    PrimaryIdAttribute queueid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Queue
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Queues
                ///     
                ///     Description:
                ///         (English - United States - 1033): A list of records that require action, such as accounts, activities, and cases.
                ///</summary>
                public static partial class queue_team
                {
                    public const string Name = "queue_team";

                    public const string ReferencedEntity_queue = "queue";

                    public const string ReferencedAttribute_queueid = "queueid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_queueid = "queueid";
                }

                ///<summary>
                /// N:1 - Relationship teamtemplate_Teams
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     teamtemplate_Teams
                /// ReferencingEntityNavigationPropertyName    associatedteamtemplateid
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity teamtemplate:    PrimaryIdAttribute teamtemplateid    PrimaryNameAttribute teamtemplatename
                ///     DisplayName:
                ///         (English - United States - 1033): Team template
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Team templates
                ///     
                ///     Description:
                ///         (English - United States - 1033): Team template for an entity enabled for automatically created access teams.
                ///</summary>
                public static partial class teamtemplate_teams
                {
                    public const string Name = "teamtemplate_Teams";

                    public const string ReferencedEntity_teamtemplate = "teamtemplate";

                    public const string ReferencedAttribute_teamtemplateid = "teamtemplateid";

                    public const string ReferencedEntity_PrimaryNameAttribute_teamtemplatename = "teamtemplatename";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_teamtemplateid = "teamtemplateid";
                }

                ///<summary>
                /// N:1 - Relationship TransactionCurrency_Team
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     TransactionCurrency_Team
                /// ReferencingEntityNavigationPropertyName    transactioncurrencyid
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Restrict
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity transactioncurrency:    PrimaryIdAttribute transactioncurrencyid    PrimaryNameAttribute currencyname
                ///     DisplayName:
                ///         (English - United States - 1033): Currency
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Currencies
                ///     
                ///     Description:
                ///         (English - United States - 1033): Currency in which a financial transaction is carried out.
                ///</summary>
                public static partial class transactioncurrency_team
                {
                    public const string Name = "TransactionCurrency_Team";

                    public const string ReferencedEntity_transactioncurrency = "transactioncurrency";

                    public const string ReferencedAttribute_transactioncurrencyid = "transactioncurrencyid";

                    public const string ReferencedEntity_PrimaryNameAttribute_currencyname = "currencyname";

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_transactioncurrencyid = "transactioncurrencyid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship ImportFile_Team
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     ImportFile_Team
                /// ReferencingEntityNavigationPropertyName    recordsownerid_team
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
                /// ReferencingEntity importfile:    PrimaryIdAttribute importfileid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Import Source File
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Imports
                ///     
                ///     Description:
                ///         (English - United States - 1033): File name of file used for import.
                ///</summary>
                public static partial class importfile_team
                {
                    public const string Name = "ImportFile_Team";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_importfile = "importfile";

                    public const string ReferencingAttribute_recordsownerid = "recordsownerid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship lead_owning_team
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lead_owning_team
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity lead:    PrimaryIdAttribute leadid    PrimaryNameAttribute fullname
                ///     DisplayName:
                ///         (English - United States - 1033): Lead
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Leads
                ///     
                ///     Description:
                ///         (English - United States - 1033): Prospect or potential sales opportunity. Leads are converted into accounts, contacts, or opportunities when they are qualified. Otherwise, they are deleted or archived.
                ///</summary>
                public static partial class lead_owning_team
                {
                    public const string Name = "lead_owning_team";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_lead = "lead";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// 1:N - Relationship OwningTeam_postfollows
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     OwningTeam_postfollows
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity postfollow:    PrimaryIdAttribute postfollowid    PrimaryNameAttribute regardingobjectidname
                ///     DisplayName:
                ///         (English - United States - 1033): Follow
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Follows
                ///     
                ///     Description:
                ///         (English - United States - 1033): Represents a user following the activity feed of an object.
                ///</summary>
                public static partial class owningteam_postfollows
                {
                    public const string Name = "OwningTeam_postfollows";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_postfollow = "postfollow";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_regardingobjectidname = "regardingobjectidname";
                }

                ///<summary>
                /// 1:N - Relationship team_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_accounts
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity account:    PrimaryIdAttribute accountid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Account
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Accounts
                ///     
                ///     Description:
                ///         (English - United States - 1033): Business that represents a customer or potential customer. The company that is billed in business transactions.
                ///</summary>
                public static partial class team_accounts
                {
                    public const string Name = "team_accounts";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_actioncardusersettings
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_actioncardusersettings
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity actioncardusersettings:    PrimaryIdAttribute actioncardusersettingsid
                ///     DisplayName:
                ///         (English - United States - 1033): Action Card User Settings
                ///     
                ///     Description:
                ///         (English - United States - 1033): Stores user settings for action cards
                ///</summary>
                public static partial class team_actioncardusersettings
                {
                    public const string Name = "team_actioncardusersettings";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_actioncardusersettings = "actioncardusersettings";

                    public const string ReferencingAttribute_owningteam = "owningteam";
                }

                ///<summary>
                /// 1:N - Relationship team_activity
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_activity
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity activitypointer:    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///     DisplayName:
                ///         (English - United States - 1033): Activity
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Activities
                ///     
                ///     Description:
                ///         (English - United States - 1033): Task performed, or to be performed, by a user. An activity is any action for which an entry can be made on a calendar.
                ///</summary>
                public static partial class team_activity
                {
                    public const string Name = "team_activity";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_activitypointer = "activitypointer";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship team_annotations
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_annotations
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity annotation:    PrimaryIdAttribute annotationid    PrimaryNameAttribute subject
                ///     DisplayName:
                ///         (English - United States - 1033): Note
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Notes
                ///     
                ///     Description:
                ///         (English - United States - 1033): Note that is attached to one or more objects, including other notes.
                ///</summary>
                public static partial class team_annotations
                {
                    public const string Name = "team_annotations";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_annotation = "annotation";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship team_appointment
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_appointment
                /// ReferencingEntityNavigationPropertyName    owningteam_appointment
                /// IsCustomizable                             True
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
                /// ReferencingEntity appointment:    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///     DisplayName:
                ///         (English - United States - 1033): Appointment
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Appointments
                ///     
                ///     Description:
                ///         (English - United States - 1033): Commitment representing a time interval with start/end times and duration.
                ///</summary>
                public static partial class team_appointment
                {
                    public const string Name = "team_appointment";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_appointment = "appointment";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship team_asyncoperation
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_asyncoperation
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity asyncoperation:    PrimaryIdAttribute asyncoperationid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): System Job
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): System Jobs
                ///     
                ///     Description:
                ///         (English - United States - 1033): Process whose execution can proceed independently or in the background.
                ///</summary>
                public static partial class team_asyncoperation
                {
                    public const string Name = "team_asyncoperation";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_asyncoperation = "asyncoperation";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship Team_AsyncOperations
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Team_AsyncOperations
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_team
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
                /// ReferencingEntity asyncoperation:    PrimaryIdAttribute asyncoperationid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): System Job
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): System Jobs
                ///     
                ///     Description:
                ///         (English - United States - 1033): Process whose execution can proceed independently or in the background.
                ///</summary>
                public static partial class team_asyncoperations
                {
                    public const string Name = "Team_AsyncOperations";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_asyncoperation = "asyncoperation";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_bookableresource
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_bookableresource
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity bookableresource:    PrimaryIdAttribute bookableresourceid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Bookable Resource
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Bookable Resources
                ///     
                ///     Description:
                ///         (English - United States - 1033): Resource that has capacity which can be allocated to work.
                ///</summary>
                public static partial class team_bookableresource
                {
                    public const string Name = "team_bookableresource";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_bookableresource = "bookableresource";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_bookableresourcebooking
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_bookableresourcebooking
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity bookableresourcebooking:    PrimaryIdAttribute bookableresourcebookingid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Bookable Resource Booking
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Bookable Resource Bookings
                ///     
                ///     Description:
                ///         (English - United States - 1033): Represents the line details of a resource booking.
                ///</summary>
                public static partial class team_bookableresourcebooking
                {
                    public const string Name = "team_bookableresourcebooking";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_bookableresourcebooking = "bookableresourcebooking";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_bookableresourcebookingexchangesyncidmapping
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_bookableresourcebookingexchangesyncidmapping
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity bookableresourcebookingexchangesyncidmapping:    PrimaryIdAttribute bookableresourcebookingexchangesyncidmappingid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): BookableResourceBooking to Exchange Id Mapping
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): BookableResourceBooking to Exchange Id Mappings
                ///     
                ///     Description:
                ///         (English - United States - 1033): The mapping used to keep track of the IDs for items synced between CRM BookableResourceBooking and Exchange.
                ///</summary>
                public static partial class team_bookableresourcebookingexchangesyncidmapping
                {
                    public const string Name = "team_bookableresourcebookingexchangesyncidmapping";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_bookableresourcebookingexchangesyncidmapping = "bookableresourcebookingexchangesyncidmapping";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_bookableresourcebookingheader
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_bookableresourcebookingheader
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity bookableresourcebookingheader:    PrimaryIdAttribute bookableresourcebookingheaderid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Bookable Resource Booking Header
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Bookable Resource Booking Headers
                ///     
                ///     Description:
                ///         (English - United States - 1033): Reservation entity representing the summary of the associated resource bookings.
                ///</summary>
                public static partial class team_bookableresourcebookingheader
                {
                    public const string Name = "team_bookableresourcebookingheader";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_bookableresourcebookingheader = "bookableresourcebookingheader";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_bookableresourcecategory
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_bookableresourcecategory
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity bookableresourcecategory:    PrimaryIdAttribute bookableresourcecategoryid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Bookable Resource Category
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Bookable Resource Categories
                ///     
                ///     Description:
                ///         (English - United States - 1033): Categorize resources that have capacity into categories such as roles.
                ///</summary>
                public static partial class team_bookableresourcecategory
                {
                    public const string Name = "team_bookableresourcecategory";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_bookableresourcecategory = "bookableresourcecategory";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_bookableresourcecategoryassn
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_bookableresourcecategoryassn
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity bookableresourcecategoryassn:    PrimaryIdAttribute bookableresourcecategoryassnid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Bookable Resource Category Assn
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Bookable Resource Category Assns
                ///     
                ///     Description:
                ///         (English - United States - 1033): Association entity to model the categorization of resources.
                ///</summary>
                public static partial class team_bookableresourcecategoryassn
                {
                    public const string Name = "team_bookableresourcecategoryassn";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_bookableresourcecategoryassn = "bookableresourcecategoryassn";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_bookableresourcecharacteristic
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_bookableresourcecharacteristic
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity bookableresourcecharacteristic:    PrimaryIdAttribute bookableresourcecharacteristicid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Bookable Resource Characteristic
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Bookable Resource Characteristics
                ///     
                ///     Description:
                ///         (English - United States - 1033): Associates resources with their characteristics and specifies the proficiency level of a resource for that characteristic.
                ///</summary>
                public static partial class team_bookableresourcecharacteristic
                {
                    public const string Name = "team_bookableresourcecharacteristic";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_bookableresourcecharacteristic = "bookableresourcecharacteristic";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_bookableresourcegroup
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_bookableresourcegroup
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity bookableresourcegroup:    PrimaryIdAttribute bookableresourcegroupid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Bookable Resource Group
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Bookable Resource Groups
                ///     
                ///     Description:
                ///         (English - United States - 1033): Associates resources with resource groups that they are a member of.
                ///</summary>
                public static partial class team_bookableresourcegroup
                {
                    public const string Name = "team_bookableresourcegroup";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_bookableresourcegroup = "bookableresourcegroup";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_bookingstatus
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_bookingstatus
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity bookingstatus:    PrimaryIdAttribute bookingstatusid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Booking Status
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Booking Statuses
                ///     
                ///     Description:
                ///         (English - United States - 1033): Allows creation of multiple sub statuses mapped to a booking status option.
                ///</summary>
                public static partial class team_bookingstatus
                {
                    public const string Name = "team_bookingstatus";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_bookingstatus = "bookingstatus";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship Team_BulkDeleteFailures
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Team_BulkDeleteFailures
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_team
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
                /// ReferencingEntity bulkdeletefailure:    PrimaryIdAttribute bulkdeletefailureid
                ///     DisplayName:
                ///         (English - United States - 1033): Bulk Delete Failure
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Bulk Delete Failures
                ///     
                ///     Description:
                ///         (English - United States - 1033): Record that was not deleted during a bulk deletion job.
                ///</summary>
                public static partial class team_bulkdeletefailures
                {
                    public const string Name = "Team_BulkDeleteFailures";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_bulkdeletefailure = "bulkdeletefailure";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";
                }

                ///<summary>
                /// 1:N - Relationship team_BulkOperation
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_BulkOperation
                /// ReferencingEntityNavigationPropertyName    owningteam_bulkoperation
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
                /// ReferencingEntity bulkoperation:    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///     DisplayName:
                ///         (English - United States - 1033): Quick Campaign
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Quick Campaigns
                ///     
                ///     Description:
                ///         (English - United States - 1033): System operation used to perform lengthy and asynchronous operations on large data sets, such as distributing a campaign activity or quick campaign.
                ///</summary>
                public static partial class team_bulkoperation
                {
                    public const string Name = "team_BulkOperation";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_bulkoperation = "bulkoperation";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship team_bulkoperationlog
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_bulkoperationlog
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity bulkoperationlog:    PrimaryIdAttribute bulkoperationlogid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Bulk Operation Log
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Bulk Operation Logs
                ///     
                ///     Description:
                ///         (English - United States - 1033): Log used to track bulk operation execution, successes, and failures.
                ///</summary>
                public static partial class team_bulkoperationlog
                {
                    public const string Name = "team_bulkoperationlog";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_bulkoperationlog = "bulkoperationlog";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_campaignactivity
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_campaignactivity
                /// ReferencingEntityNavigationPropertyName    owningteam_campaignactivity
                /// IsCustomizable                             True
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
                /// ReferencingEntity campaignactivity:    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///     DisplayName:
                ///         (English - United States - 1033): Campaign Activity
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Campaign Activities
                ///     
                ///     Description:
                ///         (English - United States - 1033): Task performed, or to be performed, by a user for planning or running a campaign.
                ///</summary>
                public static partial class team_campaignactivity
                {
                    public const string Name = "team_campaignactivity";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_campaignactivity = "campaignactivity";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship team_campaignresponse
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_campaignresponse
                /// ReferencingEntityNavigationPropertyName    owningteam_campaignresponse
                /// IsCustomizable                             True
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
                /// ReferencingEntity campaignresponse:    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///     DisplayName:
                ///         (English - United States - 1033): Campaign Response
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Campaign Responses
                ///     
                ///     Description:
                ///         (English - United States - 1033): Response from an existing or a potential new customer for a campaign.
                ///</summary>
                public static partial class team_campaignresponse
                {
                    public const string Name = "team_campaignresponse";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_campaignresponse = "campaignresponse";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship team_Campaigns
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_Campaigns
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity campaign:    PrimaryIdAttribute campaignid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Campaign
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Campaigns
                ///     
                ///     Description:
                ///         (English - United States - 1033): Container for campaign activities and responses, sales literature, products, and lists to create, plan, execute, and track the results of a specific marketing campaign through its life.
                ///</summary>
                public static partial class team_campaigns
                {
                    public const string Name = "team_Campaigns";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_campaign = "campaign";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_channelaccessprofile
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_channelaccessprofile
                /// ReferencingEntityNavigationPropertyName    team_channelaccessprofile
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity channelaccessprofile:    PrimaryIdAttribute channelaccessprofileid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Channel Access Profile
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Channel Access Profiles
                ///     
                ///     Description:
                ///         (English - United States - 1033): Information about permissions needed to access Dynamics 365 through external channels.For internal use only
                ///</summary>
                public static partial class team_channelaccessprofile
                {
                    public const string Name = "team_channelaccessprofile";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_channelaccessprofile = "channelaccessprofile";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_characteristic
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_characteristic
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity characteristic:    PrimaryIdAttribute characteristicid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Characteristic
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Characteristics
                ///     
                ///     Description:
                ///         (English - United States - 1033): Skills, education and certifications of resources.
                ///</summary>
                public static partial class team_characteristic
                {
                    public const string Name = "team_characteristic";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_characteristic = "characteristic";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_connections1
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_connections1
                /// ReferencingEntityNavigationPropertyName    record1id_team
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          100
                /// 
                /// ReferencingEntity connection:    PrimaryIdAttribute connectionid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Connection
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Connections
                ///     
                ///     Description:
                ///         (English - United States - 1033): Relationship between two entities.
                ///</summary>
                public static partial class team_connections1
                {
                    public const string Name = "team_connections1";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_connection = "connection";

                    public const string ReferencingAttribute_record1id = "record1id";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_connections2
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_connections2
                /// ReferencingEntityNavigationPropertyName    record2id_team
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Order          100
                /// 
                /// ReferencingEntity connection:    PrimaryIdAttribute connectionid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Connection
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Connections
                ///     
                ///     Description:
                ///         (English - United States - 1033): Relationship between two entities.
                ///</summary>
                public static partial class team_connections2
                {
                    public const string Name = "team_connections2";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_connection = "connection";

                    public const string ReferencingAttribute_record2id = "record2id";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_contacts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_contacts
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity contact:    PrimaryIdAttribute contactid    PrimaryNameAttribute fullname
                ///     DisplayName:
                ///         (English - United States - 1033): Contact
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Contacts
                ///     
                ///     Description:
                ///         (English - United States - 1033): Person with whom a business unit has a relationship, such as customer, supplier, and colleague.
                ///</summary>
                public static partial class team_contacts
                {
                    public const string Name = "team_contacts";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_contact = "contact";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// 1:N - Relationship team_contractdetail
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_contractdetail
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity contractdetail:    PrimaryIdAttribute contractdetailid    PrimaryNameAttribute title
                ///     DisplayName:
                ///         (English - United States - 1033): Contract Line
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Contract Lines
                ///     
                ///     Description:
                ///         (English - United States - 1033): Line item in a contract that specifies the type of service a customer is entitled to.
                ///</summary>
                public static partial class team_contractdetail
                {
                    public const string Name = "team_contractdetail";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_contractdetail = "contractdetail";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship team_convertrule
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_convertrule
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity convertrule:    PrimaryIdAttribute convertruleid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Record Creation and Update Rule
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Record Creation and Update Rules
                ///     
                ///     Description:
                ///         (English - United States - 1033): Defines the settings for automatic record creation.
                ///</summary>
                public static partial class team_convertrule
                {
                    public const string Name = "team_convertrule";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_convertrule = "convertrule";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_customer_opportunity_roles
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_customer_opportunity_roles
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity customeropportunityrole:    PrimaryIdAttribute customeropportunityroleid    PrimaryNameAttribute opportunityroleidname
                ///     DisplayName:
                ///         (English - United States - 1033): Opportunity Relationship
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Opportunity Relationships
                ///     
                ///     Description:
                ///         (English - United States - 1033): Relationship between an account or contact and an opportunity.
                ///</summary>
                public static partial class team_customer_opportunity_roles
                {
                    public const string Name = "team_customer_opportunity_roles";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_customeropportunityrole = "customeropportunityrole";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_opportunityroleidname = "opportunityroleidname";
                }

                ///<summary>
                /// 1:N - Relationship team_customer_relationship
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_customer_relationship
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity customerrelationship:    PrimaryIdAttribute customerrelationshipid    PrimaryNameAttribute customerroleidname
                ///     DisplayName:
                ///         (English - United States - 1033): Customer Relationship
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Customer Relationships
                ///     
                ///     Description:
                ///         (English - United States - 1033): Relationship between a customer and a partner in which either can be an account or contact.
                ///</summary>
                public static partial class team_customer_relationship
                {
                    public const string Name = "team_customer_relationship";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_customerrelationship = "customerrelationship";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_customerroleidname = "customerroleidname";
                }

                ///<summary>
                /// 1:N - Relationship Team_DuplicateBaseRecord
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Team_DuplicateBaseRecord
                /// ReferencingEntityNavigationPropertyName    baserecordid_team
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
                /// ReferencingEntity duplicaterecord:    PrimaryIdAttribute duplicateid
                ///     DisplayName:
                ///         (English - United States - 1033): Duplicate Record
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Duplicate Records
                ///     
                ///     Description:
                ///         (English - United States - 1033): Potential duplicate record.
                ///</summary>
                public static partial class team_duplicatebaserecord
                {
                    public const string Name = "Team_DuplicateBaseRecord";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_duplicaterecord = "duplicaterecord";

                    public const string ReferencingAttribute_baserecordid = "baserecordid";
                }

                ///<summary>
                /// 1:N - Relationship Team_DuplicateMatchingRecord
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Team_DuplicateMatchingRecord
                /// ReferencingEntityNavigationPropertyName    duplicaterecordid_team
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
                /// ReferencingEntity duplicaterecord:    PrimaryIdAttribute duplicateid
                ///     DisplayName:
                ///         (English - United States - 1033): Duplicate Record
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Duplicate Records
                ///     
                ///     Description:
                ///         (English - United States - 1033): Potential duplicate record.
                ///</summary>
                public static partial class team_duplicatematchingrecord
                {
                    public const string Name = "Team_DuplicateMatchingRecord";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_duplicaterecord = "duplicaterecord";

                    public const string ReferencingAttribute_duplicaterecordid = "duplicaterecordid";
                }

                ///<summary>
                /// 1:N - Relationship team_DuplicateRules
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_DuplicateRules
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity duplicaterule:    PrimaryIdAttribute duplicateruleid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Duplicate Detection Rule
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Duplicate Detection Rules
                ///     
                ///     Description:
                ///         (English - United States - 1033): Rule used to identify potential duplicates.
                ///</summary>
                public static partial class team_duplicaterules
                {
                    public const string Name = "team_DuplicateRules";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_duplicaterule = "duplicaterule";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_DynamicPropertyInstance
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_DynamicPropertyInstance
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity dynamicpropertyinstance:    PrimaryIdAttribute dynamicpropertyinstanceid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Property Instance
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Property Instances
                ///     
                ///     Description:
                ///         (English - United States - 1033): Instance of a property with its value.
                ///</summary>
                public static partial class team_dynamicpropertyinstance
                {
                    public const string Name = "team_DynamicPropertyInstance";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_dynamicpropertyinstance = "dynamicpropertyinstance";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_email
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_email
                /// ReferencingEntityNavigationPropertyName    owningteam_email
                /// IsCustomizable                             True
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
                /// ReferencingEntity email:    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///     DisplayName:
                ///         (English - United States - 1033): Email
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Email Messages
                ///     
                ///     Description:
                ///         (English - United States - 1033): Activity that is delivered using email protocols.
                ///</summary>
                public static partial class team_email
                {
                    public const string Name = "team_email";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_email = "email";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship team_email_templates
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_email_templates
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity template:    PrimaryIdAttribute templateid    PrimaryNameAttribute title
                ///     DisplayName:
                ///         (English - United States - 1033): Email Template
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Email Templates
                ///     
                ///     Description:
                ///         (English - United States - 1033): Template for an email message that contains the standard attributes of an email message.
                ///</summary>
                public static partial class team_email_templates
                {
                    public const string Name = "team_email_templates";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_template = "template";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship team_emailserverprofile
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_emailserverprofile
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity emailserverprofile:    PrimaryIdAttribute emailserverprofileid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Email Server Profile
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Email Server Profiles
                ///     
                ///     Description:
                ///         (English - United States - 1033): Holds the Email Server Profiles of an organization
                ///</summary>
                public static partial class team_emailserverprofile
                {
                    public const string Name = "team_emailserverprofile";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_emailserverprofile = "emailserverprofile";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_entitlement
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_entitlement
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity entitlement:    PrimaryIdAttribute entitlementid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Entitlement
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Entitlements
                ///     
                ///     Description:
                ///         (English - United States - 1033): Defines the amount and type of support a customer should receive.
                ///</summary>
                public static partial class team_entitlement
                {
                    public const string Name = "team_entitlement";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_entitlement = "entitlement";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_entitlementchannel
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_entitlementchannel
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity entitlementchannel:    PrimaryIdAttribute entitlementchannelid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Entitlement Channel
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Entitlement Channels
                ///     
                ///     Description:
                ///         (English - United States - 1033): Defines the amount and type of support for a channel.
                ///</summary>
                public static partial class team_entitlementchannel
                {
                    public const string Name = "team_entitlementchannel";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_entitlementchannel = "entitlementchannel";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_exchangesyncidmapping
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_exchangesyncidmapping
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity exchangesyncidmapping:    PrimaryIdAttribute exchangesyncidmappingid
                ///     DisplayName:
                ///         (English - United States - 1033): Exchange Sync Id Mapping
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Exchange Sync Id Mappings
                ///     
                ///     Description:
                ///         (English - United States - 1033): The mapping used to keep track of the IDs for items synced between CRM and Exchange.
                ///</summary>
                public static partial class team_exchangesyncidmapping
                {
                    public const string Name = "team_exchangesyncidmapping";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_exchangesyncidmapping = "exchangesyncidmapping";

                    public const string ReferencingAttribute_owningteam = "owningteam";
                }

                ///<summary>
                /// 1:N - Relationship team_externalparty
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_externalparty
                /// ReferencingEntityNavigationPropertyName    team_externalparty_externalparty
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity externalparty:    PrimaryIdAttribute externalpartyid    PrimaryNameAttribute fullname
                ///     DisplayName:
                ///         (English - United States - 1033): External Party
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): External Parties
                ///     
                ///     Description:
                ///         (English - United States - 1033): Information about external parties that need to access Dynamics 365 from external channels.For internal use only
                ///</summary>
                public static partial class team_externalparty
                {
                    public const string Name = "team_externalparty";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_externalparty = "externalparty";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// 1:N - Relationship team_fax
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_fax
                /// ReferencingEntityNavigationPropertyName    owningteam_fax
                /// IsCustomizable                             True
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
                /// ReferencingEntity fax:    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///     DisplayName:
                ///         (English - United States - 1033): Fax
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Faxes
                ///     
                ///     Description:
                ///         (English - United States - 1033): Activity that tracks call outcome and number of pages for a fax and optionally stores an electronic copy of the document.
                ///</summary>
                public static partial class team_fax
                {
                    public const string Name = "team_fax";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_fax = "fax";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship team_goal
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_goal
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity goal:    PrimaryIdAttribute goalid    PrimaryNameAttribute title
                ///     DisplayName:
                ///         (English - United States - 1033): Goal
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Goals
                ///     
                ///     Description:
                ///         (English - United States - 1033): Target objective for a user or a team for a specified time period.
                ///</summary>
                public static partial class team_goal
                {
                    public const string Name = "team_goal";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_goal = "goal";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship team_goal_goalowner
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_goal_goalowner
                /// ReferencingEntityNavigationPropertyName    goalownerid_team
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                NoCascade
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          110
                /// 
                /// ReferencingEntity goal:    PrimaryIdAttribute goalid    PrimaryNameAttribute title
                ///     DisplayName:
                ///         (English - United States - 1033): Goal
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Goals
                ///     
                ///     Description:
                ///         (English - United States - 1033): Target objective for a user or a team for a specified time period.
                ///</summary>
                public static partial class team_goal_goalowner
                {
                    public const string Name = "team_goal_goalowner";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_goal = "goal";

                    public const string ReferencingAttribute_goalownerid = "goalownerid";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship team_goalrollupquery
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_goalrollupquery
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity goalrollupquery:    PrimaryIdAttribute goalrollupqueryid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Rollup Query
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Rollup Queries
                ///     
                ///     Description:
                ///         (English - United States - 1033): Query that is used to filter the results of the goal rollup.
                ///</summary>
                public static partial class team_goalrollupquery
                {
                    public const string Name = "team_goalrollupquery";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_goalrollupquery = "goalrollupquery";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_ImportData
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_ImportData
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity importdata:    PrimaryIdAttribute importdataid    PrimaryNameAttribute data
                ///     DisplayName:
                ///         (English - United States - 1033): Import Data
                ///     
                ///     Description:
                ///         (English - United States - 1033): Unprocessed data from imported files.
                ///</summary>
                public static partial class team_importdata
                {
                    public const string Name = "team_ImportData";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_importdata = "importdata";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_data = "data";
                }

                ///<summary>
                /// 1:N - Relationship team_ImportFiles
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_ImportFiles
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity importfile:    PrimaryIdAttribute importfileid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Import Source File
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Imports
                ///     
                ///     Description:
                ///         (English - United States - 1033): File name of file used for import.
                ///</summary>
                public static partial class team_importfiles
                {
                    public const string Name = "team_ImportFiles";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_importfile = "importfile";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_ImportLogs
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_ImportLogs
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity importlog:    PrimaryIdAttribute importlogid
                ///     DisplayName:
                ///         (English - United States - 1033): Import Log
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): ImportLogs
                ///     
                ///     Description:
                ///         (English - United States - 1033): Failure reason and other detailed information for a record that failed to import.
                ///</summary>
                public static partial class team_importlogs
                {
                    public const string Name = "team_ImportLogs";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_importlog = "importlog";

                    public const string ReferencingAttribute_owningteam = "owningteam";
                }

                ///<summary>
                /// 1:N - Relationship team_ImportMaps
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_ImportMaps
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity importmap:    PrimaryIdAttribute importmapid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Data Map
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Data Maps
                ///     
                ///     Description:
                ///         (English - United States - 1033): Data map used in import.
                ///</summary>
                public static partial class team_importmaps
                {
                    public const string Name = "team_ImportMaps";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_importmap = "importmap";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_Imports
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_Imports
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity import:    PrimaryIdAttribute importid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Data Import
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Data Imports
                ///     
                ///     Description:
                ///         (English - United States - 1033): Status and ownership information for an import job.
                ///</summary>
                public static partial class team_imports
                {
                    public const string Name = "team_Imports";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_import = "import";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_incidentresolution
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_incidentresolution
                /// ReferencingEntityNavigationPropertyName    owningteam_incidentresolution
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
                /// ReferencingEntity incidentresolution:    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///     DisplayName:
                ///         (English - United States - 1033): Case Resolution
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Case Resolutions
                ///     
                ///     Description:
                ///         (English - United States - 1033): Special type of activity that includes description of the resolution, billing status, and the duration of the case.
                ///</summary>
                public static partial class team_incidentresolution
                {
                    public const string Name = "team_incidentresolution";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_incidentresolution = "incidentresolution";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship team_incidents
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_incidents
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity incident:    PrimaryIdAttribute incidentid    PrimaryNameAttribute title
                ///     DisplayName:
                ///         (English - United States - 1033): Case
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Cases
                ///     
                ///     Description:
                ///         (English - United States - 1033): Service request case associated with a contract.
                ///</summary>
                public static partial class team_incidents
                {
                    public const string Name = "team_incidents";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_incident = "incident";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship team_interactionforemail
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_new_interactionforemail
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity interactionforemail:    PrimaryIdAttribute interactionforemailid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Interaction for Email
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Interactions for Email
                ///</summary>
                public static partial class team_interactionforemail
                {
                    public const string Name = "team_interactionforemail";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_interactionforemail = "interactionforemail";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_invoicedetail
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_invoicedetail
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity invoicedetail:    PrimaryIdAttribute invoicedetailid    PrimaryNameAttribute invoicedetailname
                ///     DisplayName:
                ///         (English - United States - 1033): Invoice Product
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Invoice Products
                ///     
                ///     Description:
                ///         (English - United States - 1033): Line item in an invoice containing detailed billing information for a product.
                ///</summary>
                public static partial class team_invoicedetail
                {
                    public const string Name = "team_invoicedetail";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_invoicedetail = "invoicedetail";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_invoicedetailname = "invoicedetailname";
                }

                ///<summary>
                /// 1:N - Relationship team_invoices
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_invoices
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity invoice:    PrimaryIdAttribute invoiceid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Invoice
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Invoices
                ///     
                ///     Description:
                ///         (English - United States - 1033): Order that has been billed.
                ///</summary>
                public static partial class team_invoices
                {
                    public const string Name = "team_invoices";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_invoice = "invoice";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_knowledgearticle
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_knowledgearticle
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                public static partial class team_knowledgearticle
                {
                    public const string Name = "team_knowledgearticle";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_knowledgearticle = "knowledgearticle";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship team_knowledgearticleincident
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_knowledgearticleincident
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity knowledgearticleincident:    PrimaryIdAttribute knowledgearticleincidentid    PrimaryNameAttribute knowledgearticleidname
                ///     DisplayName:
                ///         (English - United States - 1033): Knowledge Article Incident
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Knowledge Article Incidents
                ///     
                ///     Description:
                ///         (English - United States - 1033): Association between an knowledge article and incident.
                ///</summary>
                public static partial class team_knowledgearticleincident
                {
                    public const string Name = "team_knowledgearticleincident";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_knowledgearticleincident = "knowledgearticleincident";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_knowledgearticleidname = "knowledgearticleidname";
                }

                ///<summary>
                /// 1:N - Relationship team_letter
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_letter
                /// ReferencingEntityNavigationPropertyName    owningteam_letter
                /// IsCustomizable                             True
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
                /// ReferencingEntity letter:    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///     DisplayName:
                ///         (English - United States - 1033): Letter
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Letters
                ///     
                ///     Description:
                ///         (English - United States - 1033): Activity that tracks the delivery of a letter. The activity can contain the electronic copy of the letter.
                ///</summary>
                public static partial class team_letter
                {
                    public const string Name = "team_letter";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_letter = "letter";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship team_list
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_list
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity list:    PrimaryIdAttribute listid    PrimaryNameAttribute listname
                ///     DisplayName:
                ///         (English - United States - 1033): Marketing List
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Marketing Lists
                ///     
                ///     Description:
                ///         (English - United States - 1033): Group of existing or potential customers created for a marketing campaign or other sales purposes.
                ///</summary>
                public static partial class team_list
                {
                    public const string Name = "team_list";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_list = "list";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_listname = "listname";
                }

                ///<summary>
                /// 1:N - Relationship team_mailbox
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_mailbox
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity mailbox:    PrimaryIdAttribute mailboxid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Mailbox
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Mailboxes
                ///</summary>
                public static partial class team_mailbox
                {
                    public const string Name = "team_mailbox";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_mailbox = "mailbox";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_mailboxtrackingcategory
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_mailboxtrackingcategory
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity mailboxtrackingcategory:    PrimaryIdAttribute mailboxtrackingcategoryid
                ///     DisplayName:
                ///         (English - United States - 1033): Mailbox Tracking Category
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Mailbox Tracking Categories
                ///     
                ///     Description:
                ///         (English - United States - 1033): Stores data about what categories for a mailbox are tracked
                ///</summary>
                public static partial class team_mailboxtrackingcategory
                {
                    public const string Name = "team_mailboxtrackingcategory";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_mailboxtrackingcategory = "mailboxtrackingcategory";

                    public const string ReferencingAttribute_owningteam = "owningteam";
                }

                ///<summary>
                /// 1:N - Relationship team_mailboxtrackingfolder
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_mailboxtrackingfolder
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity mailboxtrackingfolder:    PrimaryIdAttribute mailboxtrackingfolderid
                ///     DisplayName:
                ///         (English - United States - 1033): Mailbox Auto Tracking Folder
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Mailbox Auto Tracking Folders
                ///     
                ///     Description:
                ///         (English - United States - 1033): Stores data about what folders for a mailbox are auto tracked
                ///</summary>
                public static partial class team_mailboxtrackingfolder
                {
                    public const string Name = "team_mailboxtrackingfolder";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_mailboxtrackingfolder = "mailboxtrackingfolder";

                    public const string ReferencingAttribute_owningteam = "owningteam";
                }

                ///<summary>
                /// 1:N - Relationship team_msdyn_postalbum
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_msdyn_postalbum
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity msdyn_postalbum:    PrimaryIdAttribute msdyn_postalbumid    PrimaryNameAttribute msdyn_name
                ///     DisplayName:
                ///         (English - United States - 1033): Profile Album
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Profile Albums
                ///     
                ///     Description:
                ///         (English - United States - 1033): Contains user profile images that are stored as attachments and displayed in posts.
                ///</summary>
                public static partial class team_msdyn_postalbum
                {
                    public const string Name = "team_msdyn_postalbum";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_msdyn_postalbum = "msdyn_postalbum";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_msdyn_name = "msdyn_name";
                }

                ///<summary>
                /// 1:N - Relationship team_msdyn_relationshipinsightsunifiedconfig
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_msdyn_relationshipinsightsunifiedconfig
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity msdyn_relationshipinsightsunifiedconfig:    PrimaryIdAttribute msdyn_relationshipinsightsunifiedconfigid    PrimaryNameAttribute new_name
                ///     DisplayName:
                ///         (English - United States - 1033): msdyn_relationshipinsightsunifiedconfig
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): msdyn_relationshipinsightsunifiedconfigs
                ///</summary>
                public static partial class team_msdyn_relationshipinsightsunifiedconfig
                {
                    public const string Name = "team_msdyn_relationshipinsightsunifiedconfig";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_msdyn_relationshipinsightsunifiedconfig = "msdyn_relationshipinsightsunifiedconfig";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_new_name = "new_name";
                }

                ///<summary>
                /// 1:N - Relationship team_msdyn_siconfig
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_msdyn_siconfig
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity msdyn_siconfig:    PrimaryIdAttribute msdyn_siconfigid    PrimaryNameAttribute msdyn_version
                ///     DisplayName:
                ///         (English - United States - 1033): siconfig
                ///</summary>
                public static partial class team_msdyn_siconfig
                {
                    public const string Name = "team_msdyn_siconfig";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_msdyn_siconfig = "msdyn_siconfig";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_msdyn_version = "msdyn_version";
                }

                ///<summary>
                /// 1:N - Relationship team_msdyn_wallsavedqueryusersettings
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_msdyn_wallsavedqueryusersettings
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity msdyn_wallsavedqueryusersettings:    PrimaryIdAttribute msdyn_wallsavedqueryusersettingsid    PrimaryNameAttribute msdyn_entityname
                ///     DisplayName:
                ///         (English - United States - 1033): Filter
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Filters
                ///     
                ///     Description:
                ///         (English - United States - 1033): Contains user personalization information regarding which of the administrator’s selected views to display on a user’s personal wall.
                ///</summary>
                public static partial class team_msdyn_wallsavedqueryusersettings
                {
                    public const string Name = "team_msdyn_wallsavedqueryusersettings";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_msdyn_wallsavedqueryusersettings = "msdyn_wallsavedqueryusersettings";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_msdyn_entityname = "msdyn_entityname";
                }

                ///<summary>
                /// 1:N - Relationship team_opportunities
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_opportunities
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity opportunity:    PrimaryIdAttribute opportunityid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Opportunity
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Opportunities
                ///     
                ///     Description:
                ///         (English - United States - 1033): Potential revenue-generating event, or sale to an account, which needs to be tracked through a sales process to completion.
                ///</summary>
                public static partial class team_opportunities
                {
                    public const string Name = "team_opportunities";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_opportunity = "opportunity";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_opportunityclose
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_opportunityclose
                /// ReferencingEntityNavigationPropertyName    owningteam_opportunityclose
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
                /// ReferencingEntity opportunityclose:    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///     DisplayName:
                ///         (English - United States - 1033): Opportunity Close
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Opportunity Close Activities
                ///     
                ///     Description:
                ///         (English - United States - 1033): Activity that is created automatically when an opportunity is closed, containing information such as the description of the closing and actual revenue.
                ///</summary>
                public static partial class team_opportunityclose
                {
                    public const string Name = "team_opportunityclose";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_opportunityclose = "opportunityclose";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship team_opportunityproduct
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_opportunityproduct
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity opportunityproduct:    PrimaryIdAttribute opportunityproductid    PrimaryNameAttribute opportunityproductname
                ///     DisplayName:
                ///         (English - United States - 1033): Opportunity Product
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Opportunity Products
                ///     
                ///     Description:
                ///         (English - United States - 1033): Association between an opportunity and a product.
                ///</summary>
                public static partial class team_opportunityproduct
                {
                    public const string Name = "team_opportunityproduct";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_opportunityproduct = "opportunityproduct";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_opportunityproductname = "opportunityproductname";
                }

                ///<summary>
                /// 1:N - Relationship team_orderclose
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_orderclose
                /// ReferencingEntityNavigationPropertyName    owningteam_orderclose
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
                /// ReferencingEntity orderclose:    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///     DisplayName:
                ///         (English - United States - 1033): Order Close
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Order Close Activities
                ///     
                ///     Description:
                ///         (English - United States - 1033): Activity generated automatically when an order is closed.
                ///</summary>
                public static partial class team_orderclose
                {
                    public const string Name = "team_orderclose";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_orderclose = "orderclose";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship team_orders
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_orders
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity salesorder:    PrimaryIdAttribute salesorderid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Order
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Orders
                ///     
                ///     Description:
                ///         (English - United States - 1033): Quote that has been accepted.
                ///</summary>
                public static partial class team_orders
                {
                    public const string Name = "team_orders";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_salesorder = "salesorder";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_phonecall
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_phonecall
                /// ReferencingEntityNavigationPropertyName    owningteam_phonecall
                /// IsCustomizable                             True
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
                /// ReferencingEntity phonecall:    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///     DisplayName:
                ///         (English - United States - 1033): Phone Call
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Phone Calls
                ///     
                ///     Description:
                ///         (English - United States - 1033): Activity to track a telephone call.
                ///</summary>
                public static partial class team_phonecall
                {
                    public const string Name = "team_phonecall";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_phonecall = "phonecall";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship team_PostRegardings
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_PostRegardings
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_team
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity postregarding:    PrimaryIdAttribute postregardingid
                ///     DisplayName:
                ///         (English - United States - 1033): Post Regarding
                ///     
                ///     Description:
                ///         (English - United States - 1033): Represents which object an activity feed post is regarding. For internal use only.
                ///</summary>
                public static partial class team_postregardings
                {
                    public const string Name = "team_PostRegardings";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_postregarding = "postregarding";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";
                }

                ///<summary>
                /// 1:N - Relationship team_PostRoles
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_PostRoles
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_team
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity postrole:    PrimaryIdAttribute postroleid
                ///     DisplayName:
                ///         (English - United States - 1033): Post Role
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Post Roles
                ///     
                ///     Description:
                ///         (English - United States - 1033): Represents the objects with which an activity feed post is associated. For internal use only.
                ///</summary>
                public static partial class team_postroles
                {
                    public const string Name = "team_PostRoles";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_postrole = "postrole";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";
                }

                ///<summary>
                /// 1:N - Relationship team_principalobjectattributeaccess
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_principalobjectattributeaccess
                /// ReferencingEntityNavigationPropertyName    objectid_team
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
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity principalobjectattributeaccess:    PrimaryIdAttribute principalobjectattributeaccessid
                ///     DisplayName:
                ///         (English - United States - 1033): Field Sharing
                ///     
                ///     Description:
                ///         (English - United States - 1033): Defines CRM security principals (users and teams) access rights to secured field for an entity instance.
                ///</summary>
                public static partial class team_principalobjectattributeaccess
                {
                    public const string Name = "team_principalobjectattributeaccess";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_principalobjectattributeaccess = "principalobjectattributeaccess";

                    public const string ReferencingAttribute_objectid = "objectid";
                }

                ///<summary>
                /// 1:N - Relationship team_principalobjectattributeaccess_principalid
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_principalobjectattributeaccess_principalid
                /// ReferencingEntityNavigationPropertyName    principalid_team
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
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity principalobjectattributeaccess:    PrimaryIdAttribute principalobjectattributeaccessid
                ///     DisplayName:
                ///         (English - United States - 1033): Field Sharing
                ///     
                ///     Description:
                ///         (English - United States - 1033): Defines CRM security principals (users and teams) access rights to secured field for an entity instance.
                ///</summary>
                public static partial class team_principalobjectattributeaccess_principalid
                {
                    public const string Name = "team_principalobjectattributeaccess_principalid";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_principalobjectattributeaccess = "principalobjectattributeaccess";

                    public const string ReferencingAttribute_principalid = "principalid";
                }

                ///<summary>
                /// 1:N - Relationship team_processsession
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_processsession
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity processsession:    PrimaryIdAttribute processsessionid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Process Session
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Process Sessions
                ///     
                ///     Description:
                ///         (English - United States - 1033): Information that is generated when a dialog is run. Every time that you run a dialog, a dialog session is created.
                ///</summary>
                public static partial class team_processsession
                {
                    public const string Name = "team_processsession";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_processsession = "processsession";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship Team_ProcessSessions
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Team_ProcessSessions
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_team
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          110
                /// 
                /// ReferencingEntity processsession:    PrimaryIdAttribute processsessionid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Process Session
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Process Sessions
                ///     
                ///     Description:
                ///         (English - United States - 1033): Information that is generated when a dialog is run. Every time that you run a dialog, a dialog session is created.
                ///</summary>
                public static partial class team_processsessions
                {
                    public const string Name = "Team_ProcessSessions";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_processsession = "processsession";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_profilerule
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_profilerule
                /// ReferencingEntityNavigationPropertyName    teamid
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity channelaccessprofilerule:    PrimaryIdAttribute channelaccessprofileruleid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Channel Access Profile Rule
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Channel Access Profile Rules
                ///     
                ///     Description:
                ///         (English - United States - 1033): Defines the rules for automatically associating channel access profiles to external party records.For internal use only
                ///</summary>
                public static partial class team_profilerule
                {
                    public const string Name = "team_profilerule";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_channelaccessprofilerule = "channelaccessprofilerule";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_queueitembase_workerid
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_queueitembase_workerid
                /// ReferencingEntityNavigationPropertyName    workerid_team
                /// IsCustomizable                             True
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
                /// ReferencingEntity queueitem:    PrimaryIdAttribute queueitemid    PrimaryNameAttribute title
                ///     DisplayName:
                ///         (English - United States - 1033): Queue Item
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Queue Items
                ///     
                ///     Description:
                ///         (English - United States - 1033): A specific item in a queue, such as a case record or an activity record.
                ///</summary>
                public static partial class team_queueitembase_workerid
                {
                    public const string Name = "team_queueitembase_workerid";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_queueitem = "queueitem";

                    public const string ReferencingAttribute_workerid = "workerid";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship team_quoteclose
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_quoteclose
                /// ReferencingEntityNavigationPropertyName    owningteam_quoteclose
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
                /// ReferencingEntity quoteclose:    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///     DisplayName:
                ///         (English - United States - 1033): Quote Close
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Quote Close Activities
                ///     
                ///     Description:
                ///         (English - United States - 1033): Activity generated when a quote is closed.
                ///</summary>
                public static partial class team_quoteclose
                {
                    public const string Name = "team_quoteclose";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_quoteclose = "quoteclose";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship team_quotedetail
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_quotedetail
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity quotedetail:    PrimaryIdAttribute quotedetailid    PrimaryNameAttribute quotedetailname
                ///     DisplayName:
                ///         (English - United States - 1033): Quote Product
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Quote Products
                ///     
                ///     Description:
                ///         (English - United States - 1033): Product line item in a quote. The details include such information as product ID, description, quantity, and cost.
                ///</summary>
                public static partial class team_quotedetail
                {
                    public const string Name = "team_quotedetail";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_quotedetail = "quotedetail";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_quotedetailname = "quotedetailname";
                }

                ///<summary>
                /// 1:N - Relationship team_quotes
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_quotes
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity quote:    PrimaryIdAttribute quoteid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Quote
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Quotes
                ///     
                ///     Description:
                ///         (English - United States - 1033): Formal offer for products and/or services, proposed at specific prices and related payment terms, which is sent to a prospective customer.
                ///</summary>
                public static partial class team_quotes
                {
                    public const string Name = "team_quotes";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_quote = "quote";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_ratingmodel
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_ratingmodel
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity ratingmodel:    PrimaryIdAttribute ratingmodelid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Rating Model
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Rating Models
                ///     
                ///     Description:
                ///         (English - United States - 1033): Represents a model to evaluate skills or other related entities.
                ///</summary>
                public static partial class team_ratingmodel
                {
                    public const string Name = "team_ratingmodel";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_ratingmodel = "ratingmodel";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_ratingvalue
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_ratingvalue
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity ratingvalue:    PrimaryIdAttribute ratingvalueid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Rating Value
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Rating Values
                ///     
                ///     Description:
                ///         (English - United States - 1033): A unique value associated with a rating model that allows providing a user friendly rating value.
                ///</summary>
                public static partial class team_ratingvalue
                {
                    public const string Name = "team_ratingvalue";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_ratingvalue = "ratingvalue";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_recurringappointmentmaster
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_recurringappointmentmaster
                /// ReferencingEntityNavigationPropertyName    owningteam_recurringappointmentmaster
                /// IsCustomizable                             True
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
                /// ReferencingEntity recurringappointmentmaster:    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///     DisplayName:
                ///         (English - United States - 1033): Recurring Appointment
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Recurring Appointments
                ///     
                ///     Description:
                ///         (English - United States - 1033): The Master appointment of a recurring appointment series.
                ///</summary>
                public static partial class team_recurringappointmentmaster
                {
                    public const string Name = "team_recurringappointmentmaster";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_recurringappointmentmaster = "recurringappointmentmaster";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship team_resource_groups
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_resource_groups
                /// ReferencingEntityNavigationPropertyName    resourcegroupid_team
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
                /// ReferencingEntity resourcegroup:    PrimaryIdAttribute resourcegroupid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Scheduling Group
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Scheduling Groups
                ///     
                ///     Description:
                ///         (English - United States - 1033): Resource group or team whose members can be scheduled for a service.
                ///</summary>
                public static partial class team_resource_groups
                {
                    public const string Name = "team_resource_groups";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_resourcegroup = "resourcegroup";

                    public const string ReferencingAttribute_resourcegroupid = "resourcegroupid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_resource_specs
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_resource_specs
                /// ReferencingEntityNavigationPropertyName    groupobjectid_team
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Restrict
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity resourcespec:    PrimaryIdAttribute resourcespecid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Resource Specification
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Resource Specifications
                ///     
                ///     Description:
                ///         (English - United States - 1033): Selection rule that allows the scheduling engine to select a number of resources from a pool of resources. The rules can be associated with a service.
                ///</summary>
                public static partial class team_resource_specs
                {
                    public const string Name = "team_resource_specs";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_resourcespec = "resourcespec";

                    public const string ReferencingAttribute_groupobjectid = "groupobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_routingrule
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_routingrule
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity routingrule:    PrimaryIdAttribute routingruleid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Routing Rule Set
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Routing Rule Sets
                ///     
                ///     Description:
                ///         (English - United States - 1033): Define Routing Rule to route cases to right people at the right time
                ///</summary>
                public static partial class team_routingrule
                {
                    public const string Name = "team_routingrule";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_routingrule = "routingrule";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_routingruleitem
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_routingruleitem
                /// ReferencingEntityNavigationPropertyName    assignobjectid_team
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
                /// ReferencingEntity routingruleitem:    PrimaryIdAttribute routingruleitemid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Rule Item
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Rule Items
                ///     
                ///     Description:
                ///         (English - United States - 1033): Please provide the description for entity
                ///</summary>
                public static partial class team_routingruleitem
                {
                    public const string Name = "team_routingruleitem";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_routingruleitem = "routingruleitem";

                    public const string ReferencingAttribute_assignobjectid = "assignobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_salesorderdetail
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_salesorderdetail
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity salesorderdetail:    PrimaryIdAttribute salesorderdetailid    PrimaryNameAttribute salesorderdetailname
                ///     DisplayName:
                ///         (English - United States - 1033): Order Product
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Order Products
                ///     
                ///     Description:
                ///         (English - United States - 1033): Line item in a sales order.
                ///</summary>
                public static partial class team_salesorderdetail
                {
                    public const string Name = "team_salesorderdetail";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_salesorderdetail = "salesorderdetail";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_salesorderdetailname = "salesorderdetailname";
                }

                ///<summary>
                /// 1:N - Relationship team_service_appointments
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_service_appointments
                /// ReferencingEntityNavigationPropertyName    owningteam_serviceappointment
                /// IsCustomizable                             True
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
                /// ReferencingEntity serviceappointment:    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///     DisplayName:
                ///         (English - United States - 1033): Service Activity
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Service Activities
                ///     
                ///     Description:
                ///         (English - United States - 1033): Activity offered by the organization to satisfy its customer's needs. Each service activity includes date, time, duration, and required resources.
                ///</summary>
                public static partial class team_service_appointments
                {
                    public const string Name = "team_service_appointments";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_serviceappointment = "serviceappointment";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship team_service_contracts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_service_contracts
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity contract:    PrimaryIdAttribute contractid    PrimaryNameAttribute title
                ///     DisplayName:
                ///         (English - United States - 1033): Contract
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Contracts
                ///     
                ///     Description:
                ///         (English - United States - 1033): Agreement to provide customer service during a specified amount of time or number of cases.
                ///</summary>
                public static partial class team_service_contracts
                {
                    public const string Name = "team_service_contracts";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_contract = "contract";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship team_sharepointdocumentlocation
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_sharepointdocumentlocation
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity sharepointdocumentlocation:    PrimaryIdAttribute sharepointdocumentlocationid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Document Location
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Document Locations
                ///     
                ///     Description:
                ///         (English - United States - 1033): Document libraries or folders on a SharePoint server from where documents can be managed in Microsoft Dynamics 365.
                ///</summary>
                public static partial class team_sharepointdocumentlocation
                {
                    public const string Name = "team_sharepointdocumentlocation";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_sharepointdocumentlocation = "sharepointdocumentlocation";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_sharepointsite
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_sharepointsite
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity sharepointsite:    PrimaryIdAttribute sharepointsiteid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): SharePoint Site
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): SharePoint Sites
                ///     
                ///     Description:
                ///         (English - United States - 1033): SharePoint site from where documents can be managed in Microsoft Dynamics 365.
                ///</summary>
                public static partial class team_sharepointsite
                {
                    public const string Name = "team_sharepointsite";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_sharepointsite = "sharepointsite";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_slaBase
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_slaBase
                /// ReferencingEntityNavigationPropertyName    owningteam
                /// IsCustomizable                             True
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
                /// ReferencingEntity sla:    PrimaryIdAttribute slaid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): SLA
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): SLAs
                ///     
                ///     Description:
                ///         (English - United States - 1033): Contains information about the tracked service-level KPIs for cases that belong to different customers.
                ///</summary>
                public static partial class team_slabase
                {
                    public const string Name = "team_slaBase";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_sla = "sla";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_socialactivity
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_socialactivity
                /// ReferencingEntityNavigationPropertyName    owningteam_socialactivity
                /// IsCustomizable                             True
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
                /// ReferencingEntity socialactivity:    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///     DisplayName:
                ///         (English - United States - 1033): Social Activity
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Social Activities
                ///     
                ///     Description:
                ///         (English - United States - 1033): For internal use only.
                ///</summary>
                public static partial class team_socialactivity
                {
                    public const string Name = "team_socialactivity";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_socialactivity = "socialactivity";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship team_SyncError
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_SyncError
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity syncerror:    PrimaryIdAttribute syncerrorid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Sync Error
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Sync Errors
                ///     
                ///     Description:
                ///         (English - United States - 1033): Failure reason and other detailed information for a record that failed to sync.
                ///</summary>
                public static partial class team_syncerror
                {
                    public const string Name = "team_SyncError";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_syncerror = "syncerror";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship Team_SyncErrors
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Team_SyncErrors
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_team_syncerror
                /// IsCustomizable                             True
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
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity syncerror:    PrimaryIdAttribute syncerrorid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Sync Error
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Sync Errors
                ///     
                ///     Description:
                ///         (English - United States - 1033): Failure reason and other detailed information for a record that failed to sync.
                ///</summary>
                public static partial class team_syncerrors
                {
                    public const string Name = "Team_SyncErrors";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_syncerror = "syncerror";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_task
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_task
                /// ReferencingEntityNavigationPropertyName    owningteam_task
                /// IsCustomizable                             True
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
                /// ReferencingEntity task:    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///     DisplayName:
                ///         (English - United States - 1033): Task
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Tasks
                ///     
                ///     Description:
                ///         (English - United States - 1033): Generic activity representing work needed to be done.
                ///</summary>
                public static partial class team_task
                {
                    public const string Name = "team_task";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_task = "task";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship team_userentityinstancedata
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_userentityinstancedata
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity userentityinstancedata:    PrimaryIdAttribute userentityinstancedataid
                ///     DisplayName:
                ///         (English - United States - 1033): User Entity Instance Data
                ///     
                ///     Description:
                ///         (English - United States - 1033): Per User item instance data
                ///</summary>
                public static partial class team_userentityinstancedata
                {
                    public const string Name = "team_userentityinstancedata";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_owningteam = "owningteam";
                }

                ///<summary>
                /// 1:N - Relationship team_userentityuisettings
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_userentityuisettings
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity userentityuisettings:    PrimaryIdAttribute userentityuisettingsid
                ///     DisplayName:
                ///         (English - United States - 1033): User Entity UI Settings
                ///     
                ///     Description:
                ///         (English - United States - 1033): Stores user settings for entity views.
                ///</summary>
                public static partial class team_userentityuisettings
                {
                    public const string Name = "team_userentityuisettings";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_userentityuisettings = "userentityuisettings";

                    public const string ReferencingAttribute_owningteam = "owningteam";
                }

                ///<summary>
                /// 1:N - Relationship team_userform
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_userform
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity userform:    PrimaryIdAttribute userformid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): User Dashboard
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): User Dashboards
                ///     
                ///     Description:
                ///         (English - United States - 1033): User-owned dashboards.
                ///</summary>
                public static partial class team_userform
                {
                    public const string Name = "team_userform";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_userform = "userform";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_userquery
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_userquery
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity userquery:    PrimaryIdAttribute userqueryid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Saved View
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Saved Views
                ///     
                ///     Description:
                ///         (English - United States - 1033): Saved database query that is owned by a user.
                ///</summary>
                public static partial class team_userquery
                {
                    public const string Name = "team_userquery";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_userquery = "userquery";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_userqueryvisualizations
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_userqueryvisualizations
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity userqueryvisualization:    PrimaryIdAttribute userqueryvisualizationid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): User Chart
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): User Charts
                ///     
                ///     Description:
                ///         (English - United States - 1033): Chart attached to an entity.
                ///</summary>
                public static partial class team_userqueryvisualizations
                {
                    public const string Name = "team_userqueryvisualizations";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_userqueryvisualization = "userqueryvisualization";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_workflow
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_workflow
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity workflow:    PrimaryIdAttribute workflowid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Process
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Processes
                ///     
                ///     Description:
                ///         (English - United States - 1033): Set of logical rules that define the steps necessary to automate a specific business process, task, or set of actions to be performed.
                ///</summary>
                public static partial class team_workflow
                {
                    public const string Name = "team_workflow";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_workflow = "workflow";

                    public const string ReferencingAttribute_owningteam = "owningteam";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship team_workflowlog
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     team_workflowlog
                /// ReferencingEntityNavigationPropertyName    owningteam
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
                /// ReferencingEntity workflowlog:    PrimaryIdAttribute workflowlogid
                ///     DisplayName:
                ///         (English - United States - 1033): Process Log
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Process Logs
                ///     
                ///     Description:
                ///         (English - United States - 1033): Log used to track process execution.
                ///</summary>
                public static partial class team_workflowlog
                {
                    public const string Name = "team_workflowlog";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_workflowlog = "workflowlog";

                    public const string ReferencingAttribute_owningteam = "owningteam";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_team
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_team
                /// ReferencingEntityNavigationPropertyName    objectid_team
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
                public static partial class userentityinstancedata_team
                {
                    public const string Name = "userentityinstancedata_team";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.

            #region Relationship ManyToMany - N:N.

            public static partial class ManyToMany
            {
                ///<summary>
                /// N:N - Relationship teammembership_association
                /// 
                /// PropertyName                                   Value
                /// Entity1NavigationPropertyName                  teammembership_association
                /// Entity2NavigationPropertyName                  teammembership_association
                /// IsCustomizable                                 False
                /// IsCustomRelationship                           False
                /// IsValidForAdvancedFind                         True
                /// RelationshipType                               ManyToManyRelationship
                /// SecurityTypes                                  None
                /// Entity1AssociatedMenuConfiguration.Behavior    DoNotDisplay
                /// Entity1AssociatedMenuConfiguration.Group       Details
                /// Entity1AssociatedMenuConfiguration.Order       null
                /// Entity2AssociatedMenuConfiguration.Behavior    DoNotDisplay
                /// Entity2AssociatedMenuConfiguration.Group       Details
                /// Entity2AssociatedMenuConfiguration.Order       null
                /// 
                /// Entity2LogicalName systemuser:    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///     DisplayName:
                ///         (English - United States - 1033): User
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///     
                ///     Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///</summary>
                public static partial class teammembership_association
                {
                    public const string Name = "teammembership_association";

                    public const string IntersectEntity_teammembership = "teammembership";

                    public const string Entity1_team = "team";

                    public const string Entity1Attribute_teamid = "teamid";

                    public const string Entity2_systemuser = "systemuser";

                    public const string Entity2Attribute_systemuserid = "systemuserid";

                    public const string Entity2LogicalName_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// N:N - Relationship teamprofiles_association
                /// 
                /// PropertyName                                   Value
                /// Entity1NavigationPropertyName                  teamprofiles_association
                /// Entity2NavigationPropertyName                  teamprofiles_association
                /// IsCustomizable                                 False
                /// IsCustomRelationship                           False
                /// IsValidForAdvancedFind                         True
                /// RelationshipType                               ManyToManyRelationship
                /// SecurityTypes                                  None
                /// Entity1AssociatedMenuConfiguration.Behavior    UseCollectionName
                /// Entity1AssociatedMenuConfiguration.Group       Details
                /// Entity1AssociatedMenuConfiguration.Order       null
                /// Entity2AssociatedMenuConfiguration.Behavior    UseCollectionName
                /// Entity2AssociatedMenuConfiguration.Group       Details
                /// Entity2AssociatedMenuConfiguration.Order       null
                /// 
                /// Entity2LogicalName fieldsecurityprofile:    PrimaryIdAttribute fieldsecurityprofileid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Field Security Profile
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Field Security Profiles
                ///     
                ///     Description:
                ///         (English - United States - 1033): Profile which defines access level for secured attributes
                ///</summary>
                public static partial class teamprofiles_association
                {
                    public const string Name = "teamprofiles_association";

                    public const string IntersectEntity_teamprofiles = "teamprofiles";

                    public const string Entity1_team = "team";

                    public const string Entity1Attribute_teamid = "teamid";

                    public const string Entity2_fieldsecurityprofile = "fieldsecurityprofile";

                    public const string Entity2Attribute_fieldsecurityprofileid = "fieldsecurityprofileid";

                    public const string Entity2LogicalName_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// N:N - Relationship teamroles_association
                /// 
                /// PropertyName                                   Value
                /// Entity1NavigationPropertyName                  teamroles_association
                /// Entity2NavigationPropertyName                  teamroles_association
                /// IsCustomizable                                 False
                /// IsCustomRelationship                           False
                /// IsValidForAdvancedFind                         True
                /// RelationshipType                               ManyToManyRelationship
                /// SecurityTypes                                  None
                /// Entity1AssociatedMenuConfiguration.Behavior    DoNotDisplay
                /// Entity1AssociatedMenuConfiguration.Group       Details
                /// Entity1AssociatedMenuConfiguration.Order       null
                /// Entity2AssociatedMenuConfiguration.Behavior    DoNotDisplay
                /// Entity2AssociatedMenuConfiguration.Group       Details
                /// Entity2AssociatedMenuConfiguration.Order       null
                /// 
                /// Entity2LogicalName role:    PrimaryIdAttribute roleid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Security Role
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Security Roles
                ///     
                ///     Description:
                ///         (English - United States - 1033): Grouping of security privileges. Users are assigned roles that authorize their access to the Microsoft CRM system.
                ///</summary>
                public static partial class teamroles_association
                {
                    public const string Name = "teamroles_association";

                    public const string IntersectEntity_teamroles = "teamroles";

                    public const string Entity1_team = "team";

                    public const string Entity1Attribute_teamid = "teamid";

                    public const string Entity2_role = "role";

                    public const string Entity2Attribute_roleid = "roleid";

                    public const string Entity2LogicalName_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// N:N - Relationship teamsyncattributemappingprofiles_association
                /// 
                /// PropertyName                                   Value
                /// Entity1NavigationPropertyName                  teamsyncattributemappingprofiles_association
                /// Entity2NavigationPropertyName                  teamsyncattributemappingprofiles_association
                /// IsCustomizable                                 False
                /// IsCustomRelationship                           False
                /// IsValidForAdvancedFind                         True
                /// RelationshipType                               ManyToManyRelationship
                /// SecurityTypes                                  None
                /// Entity1AssociatedMenuConfiguration.Behavior    UseCollectionName
                /// Entity1AssociatedMenuConfiguration.Group       Details
                /// Entity1AssociatedMenuConfiguration.Order       null
                /// Entity2AssociatedMenuConfiguration.Behavior    UseCollectionName
                /// Entity2AssociatedMenuConfiguration.Group       Details
                /// Entity2AssociatedMenuConfiguration.Order       null
                /// 
                /// Entity2LogicalName syncattributemappingprofile:    PrimaryIdAttribute syncattributemappingprofileid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Sync Attribute Mapping Profile
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Sync Attribute Mapping Profiles
                ///     
                ///     Description:
                ///         (English - United States - 1033): Profile which defines sync attribute mapping
                ///</summary>
                public static partial class teamsyncattributemappingprofiles_association
                {
                    public const string Name = "teamsyncattributemappingprofiles_association";

                    public const string IntersectEntity_teamsyncattributemappingprofiles = "teamsyncattributemappingprofiles";

                    public const string Entity1_team = "team";

                    public const string Entity1Attribute_teamid = "teamid";

                    public const string Entity2_syncattributemappingprofile = "syncattributemappingprofile";

                    public const string Entity2Attribute_syncattributemappingprofileid = "syncattributemappingprofileid";

                    public const string Entity2LogicalName_PrimaryNameAttribute_name = "name";
                }
            }

            #endregion Relationship ManyToMany - N:N.
        }
    }
}