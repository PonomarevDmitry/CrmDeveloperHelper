
namespace Entities
{
    public partial class Account
    {
        ///<summary>
        /// DisplayName:
        ///     (English - United States - 1033): Account
        /// 
        /// DisplayCollectionName:
        ///     (English - United States - 1033): Accounts
        /// 
        /// Description:
        ///     (English - United States - 1033): Business that represents a customer or potential customer. The company that is billed in business transactions.
        /// 
        /// PropertyName                          Value
        /// ActivityTypeMask                      0
        /// AutoCreateAccessTeams                 False
        /// AutoRouteToOwnerQueue                 False
        /// CanBeInManyToMany                     True
        /// CanBePrimaryEntityInRelationship      True
        /// CanBeRelatedEntityInRelationship      True
        /// CanChangeHierarchicalRelationship     False
        /// CanChangeTrackingBeEnabled            True
        /// CanCreateAttributes                   True
        /// CanCreateCharts                       True
        /// CanCreateForms                        True
        /// CanCreateViews                        True
        /// CanEnableSyncToExternalSearchIndex    True
        /// CanModifyAdditionalSettings           True
        /// CanTriggerWorkflow                    True
        /// ChangeTrackingEnabled                 True
        /// CollectionSchemaName                  Accounts
        /// DataProviderId                        null
        /// DataSourceId                          null
        /// EnforceStateTransitions               False
        /// EntityColor                           #794300
        /// EntityHelpUrlEnabled                  False
        /// EntitySetName                         accounts
        /// IntroducedVersion                     5.0.0.0
        /// IsAIRUpdated                          True
        /// IsActivity                            False
        /// IsActivityParty                       True
        /// IsAvailableOffline                    True
        /// IsBPFEntity                           False
        /// IsBusinessProcessEnabled              True
        /// IsChildEntity                         False
        /// IsConnectionsEnabled                  True
        /// IsCustomEntity                        False
        /// IsCustomizable                        True
        /// IsDocumentManagementEnabled           True
        /// IsDocumentRecommendationsEnabled      False
        /// IsDuplicateDetectionEnabled           True
        /// IsEnabledForCharts                    True
        /// IsEnabledForExternalChannels          True
        /// IsEnabledForTrace                     False
        /// IsImportable                          True
        /// IsInteractionCentricEnabled           True
        /// IsIntersect                           False
        /// IsKnowledgeManagementEnabled          False
        /// IsLogicalEntity                       False
        /// IsMailMergeEnabled                    True
        /// IsMappable                            True
        /// IsOfflineInMobileClient               True
        /// IsOneNoteIntegrationEnabled           True
        /// IsOptimisticConcurrencyEnabled        True
        /// IsPrivate                             False
        /// IsQuickCreateEnabled                  True
        /// IsReadOnlyInMobileClient              False
        /// IsReadingPaneEnabled                  True
        /// IsRenameable                          True
        /// IsSLAEnabled                          False
        /// IsStateModelAware                     False
        /// IsValidForAdvancedFind                True
        /// IsValidForQueue                       False
        /// IsVisibleInMobile                     True
        /// IsVisibleInMobileClient               True
        /// LogicalCollectionName                 accounts
        /// LogicalName                           account
        /// MobileOfflineFilters                  		<fetch version="1.0" output-format="xml-platform" mapping="logical" distinct="false">
        /// MobileOfflineFilters                  			<entity name="account">
        /// MobileOfflineFilters                  				<filter type="and">
        /// MobileOfflineFilters                  					<condition attribute="modifiedon" operator="last-x-days" value="10"/>
        /// MobileOfflineFilters                  				</filter>
        /// MobileOfflineFilters                  			</entity>
        /// MobileOfflineFilters                  		</fetch>
        /// MobileOfflineFilters
        /// ObjectTypeCode                        1
        /// OwnershipType                         UserOwned
        /// PrimaryIdAttribute                    accountid
        /// PrimaryImageAttribute                 entityimage
        /// PrimaryNameAttribute                  name
        /// ReportViewName                        FilteredAccount
        /// SchemaName                            Account
        /// SyncToExternalSearchIndex             True
        /// UsesBusinessDataLabelTable            False
        ///</summary>
        public static partial class Schema
        {
            public const string EntityLogicalName = "account";

            public const string EntitySchemaName = "Account";

            public const string EntityPrimaryIdAttribute = "accountid";

            public const string EntityPrimaryNameAttribute = "name";

            public const string EntityPrimaryImageAttribute = "entityimage";

            public const int EntityObjectTypeCode = 1;

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Account
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the account.
                /// 
                /// SchemaName: AccountId
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
                [System.ComponentModel.DescriptionAttribute("Account")]
                public const string accountid = "accountid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Account Name
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the company or business name.
                /// 
                /// SchemaName: Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: ApplicationRequired
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
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          False
                /// IsPrimaryId                    False
                /// IsPrimaryName                  True
                /// IsRenameable                   True
                /// IsRequiredForForm              True
                /// IsRetrievable                  True
                /// IsSearchable                   True
                /// IsSortableEnabled              True
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Account Name")]
                public const string name = "name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Category
                /// 
                /// Description:
                ///     (English - United States - 1033): Select a category to indicate whether the customer account is standard or preferred.
                /// 
                /// SchemaName: AccountCategoryCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_accountcategorycode
                /// DefaultFormValue = -1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Category")]
                public const string accountcategorycode = "accountcategorycode";

                ///<summary>
                /// SchemaName: AccountCategoryCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'accountcategorycode'
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
                //public const string accountcategorycodename = "accountcategorycodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Classification
                /// 
                /// Description:
                ///     (English - United States - 1033): Select a classification code to indicate the potential value of the customer account based on the projected return on investment, cooperation level, sales cycle length or other criteria.
                /// 
                /// SchemaName: AccountClassificationCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_accountclassificationcode
                /// DefaultFormValue = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Classification")]
                public const string accountclassificationcode = "accountclassificationcode";

                ///<summary>
                /// SchemaName: AccountClassificationCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'accountclassificationcode'
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
                //public const string accountclassificationcodename = "accountclassificationcodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Account Number
                /// 
                /// Description:
                ///     (English - United States - 1033): Type an ID number or code for the account to quickly search and identify the account in system views.
                /// 
                /// SchemaName: AccountNumber
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                /// IsRetrievable                  True
                /// IsSearchable                   True
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Account Number")]
                public const string accountnumber = "accountnumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Account Rating
                /// 
                /// Description:
                ///     (English - United States - 1033): Select a rating to indicate the value of the customer account.
                /// 
                /// SchemaName: AccountRatingCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_accountratingcode
                /// DefaultFormValue = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Account Rating")]
                public const string accountratingcode = "accountratingcode";

                ///<summary>
                /// SchemaName: AccountRatingCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'accountratingcode'
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
                //public const string accountratingcodename = "accountratingcodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: ID
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier for address 1.
                /// 
                /// SchemaName: Address1_AddressId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: ID")]
                public const string address1_addressid = "address1_addressid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Address Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the primary address type.
                /// 
                /// SchemaName: Address1_AddressTypeCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_address1_addresstypecode
                /// DefaultFormValue = -1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: Address Type")]
                public const string address1_addresstypecode = "address1_addresstypecode";

                ///<summary>
                /// SchemaName: Address1_AddressTypeCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'address1_addresstypecode'
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
                //public const string address1_addresstypecodename = "address1_addresstypecodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: City
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the city for the primary address.
                /// 
                /// SchemaName: Address1_City
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 80
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          True
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  True
                /// IsSearchable                   False
                /// IsSortableEnabled              True
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Address 1: City")]
                public const string address1_city = "address1_city";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the complete primary address.
                /// 
                /// SchemaName: Address1_Composite
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1000
                /// Format = TextArea    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            True
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
                [System.ComponentModel.DescriptionAttribute("Address 1")]
                public const string address1_composite = "address1_composite";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Country/Region
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the country or region for the primary address.
                /// 
                /// SchemaName: Address1_Country
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 80
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: Country/Region")]
                public const string address1_country = "address1_country";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: County
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the county for the primary address.
                /// 
                /// SchemaName: Address1_County
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: County")]
                public const string address1_county = "address1_county";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Fax
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the fax number associated with the primary address.
                /// 
                /// SchemaName: Address1_Fax
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: Fax")]
                public const string address1_fax = "address1_fax";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Freight Terms
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the freight terms for the primary address to make sure shipping orders are processed correctly.
                /// 
                /// SchemaName: Address1_FreightTermsCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_address1_freighttermscode
                /// DefaultFormValue = -1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: Freight Terms")]
                public const string address1_freighttermscode = "address1_freighttermscode";

                ///<summary>
                /// SchemaName: Address1_FreightTermsCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'address1_freighttermscode'
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
                //public const string address1_freighttermscodename = "address1_freighttermscodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Latitude
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the latitude value for the primary address for use in mapping and other applications.
                /// 
                /// SchemaName: Address1_Latitude
                /// DoubleAttributeMetadata    AttributeType: Double    AttributeTypeName: DoubleType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -90    MaxValue = 90    Precision = 5
                /// ImeMode = Disabled
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: Latitude")]
                public const string address1_latitude = "address1_latitude";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Street 1
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the first line of the primary address.
                /// 
                /// SchemaName: Address1_Line1
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 250
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: Street 1")]
                public const string address1_line1 = "address1_line1";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Street 2
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the second line of the primary address.
                /// 
                /// SchemaName: Address1_Line2
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 250
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: Street 2")]
                public const string address1_line2 = "address1_line2";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Street 3
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the third line of the primary address.
                /// 
                /// SchemaName: Address1_Line3
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 250
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: Street 3")]
                public const string address1_line3 = "address1_line3";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Longitude
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the longitude value for the primary address for use in mapping and other applications.
                /// 
                /// SchemaName: Address1_Longitude
                /// DoubleAttributeMetadata    AttributeType: Double    AttributeTypeName: DoubleType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -180    MaxValue = 180    Precision = 5
                /// ImeMode = Disabled
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: Longitude")]
                public const string address1_longitude = "address1_longitude";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Name
                /// 
                /// Description:
                ///     (English - United States - 1033): Type a descriptive name for the primary address, such as Corporate Headquarters.
                /// 
                /// SchemaName: Address1_Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: Name")]
                public const string address1_name = "address1_name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: ZIP/Postal Code
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the ZIP Code or postal code for the primary address.
                /// 
                /// SchemaName: Address1_PostalCode
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
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
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Address 1: ZIP/Postal Code")]
                public const string address1_postalcode = "address1_postalcode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Post Office Box
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the post office box number of the primary address.
                /// 
                /// SchemaName: Address1_PostOfficeBox
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: Post Office Box")]
                public const string address1_postofficebox = "address1_postofficebox";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Primary Contact Name
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the name of the main contact at the account's primary address.
                /// 
                /// SchemaName: Address1_PrimaryContactName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: Primary Contact Name")]
                public const string address1_primarycontactname = "address1_primarycontactname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Shipping Method
                /// 
                /// Description:
                ///     (English - United States - 1033): Select a shipping method for deliveries sent to this address.
                /// 
                /// SchemaName: Address1_ShippingMethodCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_address1_shippingmethodcode
                /// DefaultFormValue = -1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: Shipping Method")]
                public const string address1_shippingmethodcode = "address1_shippingmethodcode";

                ///<summary>
                /// SchemaName: Address1_ShippingMethodCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'address1_shippingmethodcode'
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
                //public const string address1_shippingmethodcodename = "address1_shippingmethodcodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: State/Province
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the state or province of the primary address.
                /// 
                /// SchemaName: Address1_StateOrProvince
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: State/Province")]
                public const string address1_stateorprovince = "address1_stateorprovince";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address Phone
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the main phone number associated with the primary address.
                /// 
                /// SchemaName: Address1_Telephone1
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address Phone")]
                public const string address1_telephone1 = "address1_telephone1";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Telephone 2
                /// 
                /// Description:
                ///     (English - United States - 1033): Type a second phone number associated with the primary address.
                /// 
                /// SchemaName: Address1_Telephone2
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: Telephone 2")]
                public const string address1_telephone2 = "address1_telephone2";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Telephone 3
                /// 
                /// Description:
                ///     (English - United States - 1033): Type a third phone number associated with the primary address.
                /// 
                /// SchemaName: Address1_Telephone3
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: Telephone 3")]
                public const string address1_telephone3 = "address1_telephone3";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: UPS Zone
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the UPS zone of the primary address to make sure shipping charges are calculated correctly and deliveries are made promptly, if shipped by UPS.
                /// 
                /// SchemaName: Address1_UPSZone
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 4
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: UPS Zone")]
                public const string address1_upszone = "address1_upszone";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: UTC Offset
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the time zone, or UTC offset, for this address so that other people can reference it when they contact someone at this address.
                /// 
                /// SchemaName: Address1_UTCOffset
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -1500    MaxValue = 1500
                /// Format = TimeZone
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 1: UTC Offset")]
                public const string address1_utcoffset = "address1_utcoffset";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: ID
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier for address 2.
                /// 
                /// SchemaName: Address2_AddressId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: ID")]
                public const string address2_addressid = "address2_addressid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Address Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the secondary address type.
                /// 
                /// SchemaName: Address2_AddressTypeCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_address2_addresstypecode
                /// DefaultFormValue = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: Address Type")]
                public const string address2_addresstypecode = "address2_addresstypecode";

                ///<summary>
                /// SchemaName: Address2_AddressTypeCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'address2_addresstypecode'
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
                //public const string address2_addresstypecodename = "address2_addresstypecodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: City
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the city for the secondary address.
                /// 
                /// SchemaName: Address2_City
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 80
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: City")]
                public const string address2_city = "address2_city";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the complete secondary address.
                /// 
                /// SchemaName: Address2_Composite
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1000
                /// Format = TextArea    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            True
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
                [System.ComponentModel.DescriptionAttribute("Address 2")]
                public const string address2_composite = "address2_composite";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Country/Region
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the country or region for the secondary address.
                /// 
                /// SchemaName: Address2_Country
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 80
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: Country/Region")]
                public const string address2_country = "address2_country";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: County
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the county for the secondary address.
                /// 
                /// SchemaName: Address2_County
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: County")]
                public const string address2_county = "address2_county";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Fax
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the fax number associated with the secondary address.
                /// 
                /// SchemaName: Address2_Fax
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: Fax")]
                public const string address2_fax = "address2_fax";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Freight Terms
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the freight terms for the secondary address to make sure shipping orders are processed correctly.
                /// 
                /// SchemaName: Address2_FreightTermsCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_address2_freighttermscode
                /// DefaultFormValue = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: Freight Terms")]
                public const string address2_freighttermscode = "address2_freighttermscode";

                ///<summary>
                /// SchemaName: Address2_FreightTermsCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'address2_freighttermscode'
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
                //public const string address2_freighttermscodename = "address2_freighttermscodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Latitude
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the latitude value for the secondary address for use in mapping and other applications.
                /// 
                /// SchemaName: Address2_Latitude
                /// DoubleAttributeMetadata    AttributeType: Double    AttributeTypeName: DoubleType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -90    MaxValue = 90    Precision = 5
                /// ImeMode = Disabled
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: Latitude")]
                public const string address2_latitude = "address2_latitude";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Street 1
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the first line of the secondary address.
                /// 
                /// SchemaName: Address2_Line1
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 250
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: Street 1")]
                public const string address2_line1 = "address2_line1";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Street 2
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the second line of the secondary address.
                /// 
                /// SchemaName: Address2_Line2
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 250
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: Street 2")]
                public const string address2_line2 = "address2_line2";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Street 3
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the third line of the secondary address.
                /// 
                /// SchemaName: Address2_Line3
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 250
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: Street 3")]
                public const string address2_line3 = "address2_line3";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Longitude
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the longitude value for the secondary address for use in mapping and other applications.
                /// 
                /// SchemaName: Address2_Longitude
                /// DoubleAttributeMetadata    AttributeType: Double    AttributeTypeName: DoubleType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -180    MaxValue = 180    Precision = 5
                /// ImeMode = Disabled
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: Longitude")]
                public const string address2_longitude = "address2_longitude";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Name
                /// 
                /// Description:
                ///     (English - United States - 1033): Type a descriptive name for the secondary address, such as Corporate Headquarters.
                /// 
                /// SchemaName: Address2_Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: Name")]
                public const string address2_name = "address2_name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: ZIP/Postal Code
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the ZIP Code or postal code for the secondary address.
                /// 
                /// SchemaName: Address2_PostalCode
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: ZIP/Postal Code")]
                public const string address2_postalcode = "address2_postalcode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Post Office Box
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the post office box number of the secondary address.
                /// 
                /// SchemaName: Address2_PostOfficeBox
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: Post Office Box")]
                public const string address2_postofficebox = "address2_postofficebox";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Primary Contact Name
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the name of the main contact at the account's secondary address.
                /// 
                /// SchemaName: Address2_PrimaryContactName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: Primary Contact Name")]
                public const string address2_primarycontactname = "address2_primarycontactname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Shipping Method
                /// 
                /// Description:
                ///     (English - United States - 1033): Select a shipping method for deliveries sent to this address.
                /// 
                /// SchemaName: Address2_ShippingMethodCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_address2_shippingmethodcode
                /// DefaultFormValue = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: Shipping Method")]
                public const string address2_shippingmethodcode = "address2_shippingmethodcode";

                ///<summary>
                /// SchemaName: Address2_ShippingMethodCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'address2_shippingmethodcode'
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
                //public const string address2_shippingmethodcodename = "address2_shippingmethodcodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: State/Province
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the state or province of the secondary address.
                /// 
                /// SchemaName: Address2_StateOrProvince
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: State/Province")]
                public const string address2_stateorprovince = "address2_stateorprovince";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Telephone 1
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the main phone number associated with the secondary address.
                /// 
                /// SchemaName: Address2_Telephone1
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: Telephone 1")]
                public const string address2_telephone1 = "address2_telephone1";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Telephone 2
                /// 
                /// Description:
                ///     (English - United States - 1033): Type a second phone number associated with the secondary address.
                /// 
                /// SchemaName: Address2_Telephone2
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: Telephone 2")]
                public const string address2_telephone2 = "address2_telephone2";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Telephone 3
                /// 
                /// Description:
                ///     (English - United States - 1033): Type a third phone number associated with the secondary address.
                /// 
                /// SchemaName: Address2_Telephone3
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: Telephone 3")]
                public const string address2_telephone3 = "address2_telephone3";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: UPS Zone
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the UPS zone of the secondary address to make sure shipping charges are calculated correctly and deliveries are made promptly, if shipped by UPS.
                /// 
                /// SchemaName: Address2_UPSZone
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 4
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: UPS Zone")]
                public const string address2_upszone = "address2_upszone";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: UTC Offset
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the time zone, or UTC offset, for this address so that other people can reference it when they contact someone at this address.
                /// 
                /// SchemaName: Address2_UTCOffset
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -1500    MaxValue = 1500
                /// Format = TimeZone
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Address 2: UTC Offset")]
                public const string address2_utcoffset = "address2_utcoffset";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Aging 30
                /// 
                /// Description:
                ///     (English - United States - 1033): For system use only.
                /// 
                /// SchemaName: Aging30
                /// MoneyAttributeMetadata    AttributeType: Money    AttributeTypeName: MoneyType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 100000000000000    Precision = 2    PrecisionSource = 2
                /// IsBaseCurrency = False
                /// ImeMode = Disabled
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            True
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
                [System.ComponentModel.DescriptionAttribute("Aging 30")]
                public const string aging30 = "aging30";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Aging 30 (Base)
                /// 
                /// Description:
                ///     (English - United States - 1033): The base currency equivalent of the aging 30 field.
                /// 
                /// SchemaName: Aging30_Base
                /// MoneyAttributeMetadata    AttributeType: Money    AttributeTypeName: MoneyType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -922337203685477    MaxValue = 922337203685477    Precision = 4    PrecisionSource = 2
                /// IsBaseCurrency = True
                /// ImeMode = Disabled    CalculationOf = aging30
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
                [System.ComponentModel.DescriptionAttribute("Aging 30 (Base)")]
                public const string aging30_base = "aging30_base";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Aging 60
                /// 
                /// Description:
                ///     (English - United States - 1033): For system use only.
                /// 
                /// SchemaName: Aging60
                /// MoneyAttributeMetadata    AttributeType: Money    AttributeTypeName: MoneyType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 100000000000000    Precision = 2    PrecisionSource = 2
                /// IsBaseCurrency = False
                /// ImeMode = Disabled
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            True
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
                [System.ComponentModel.DescriptionAttribute("Aging 60")]
                public const string aging60 = "aging60";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Aging 60 (Base)
                /// 
                /// Description:
                ///     (English - United States - 1033): The base currency equivalent of the aging 60 field.
                /// 
                /// SchemaName: Aging60_Base
                /// MoneyAttributeMetadata    AttributeType: Money    AttributeTypeName: MoneyType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -922337203685477    MaxValue = 922337203685477    Precision = 4    PrecisionSource = 2
                /// IsBaseCurrency = True
                /// ImeMode = Disabled    CalculationOf = aging60
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
                [System.ComponentModel.DescriptionAttribute("Aging 60 (Base)")]
                public const string aging60_base = "aging60_base";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Aging 90
                /// 
                /// Description:
                ///     (English - United States - 1033): For system use only.
                /// 
                /// SchemaName: Aging90
                /// MoneyAttributeMetadata    AttributeType: Money    AttributeTypeName: MoneyType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 100000000000000    Precision = 2    PrecisionSource = 2
                /// IsBaseCurrency = False
                /// ImeMode = Disabled
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            True
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
                [System.ComponentModel.DescriptionAttribute("Aging 90")]
                public const string aging90 = "aging90";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Aging 90 (Base)
                /// 
                /// Description:
                ///     (English - United States - 1033): The base currency equivalent of the aging 90 field.
                /// 
                /// SchemaName: Aging90_Base
                /// MoneyAttributeMetadata    AttributeType: Money    AttributeTypeName: MoneyType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -922337203685477    MaxValue = 922337203685477    Precision = 4    PrecisionSource = 2
                /// IsBaseCurrency = True
                /// ImeMode = Disabled    CalculationOf = aging90
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
                [System.ComponentModel.DescriptionAttribute("Aging 90 (Base)")]
                public const string aging90_base = "aging90_base";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Business Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the legal designation or other business type of the account for contracts or reporting purposes.
                /// 
                /// SchemaName: BusinessTypeCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_businesstypecode
                /// DefaultFormValue = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Business Type")]
                public const string businesstypecode = "businesstypecode";

                ///<summary>
                /// SchemaName: BusinessTypeCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'businesstypecode'
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
                //public const string businesstypecodename = "businesstypecodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows who created the record.
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
                [System.ComponentModel.DescriptionAttribute("Created By")]
                public const string createdby = "createdby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By (External Party)
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the external party who created the record.
                /// 
                /// SchemaName: CreatedByExternalParty
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: externalparty
                /// 
                ///     Target externalparty    PrimaryIdAttribute externalpartyid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///             (English - United States - 1033): External Party
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): External Parties
                ///         
                ///         Description:
                ///             (English - United States - 1033): Information about external parties that need to access Dynamics 365 from external channels.For internal use only
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
                [System.ComponentModel.DescriptionAttribute("Created By (External Party)")]
                public const string createdbyexternalparty = "createdbyexternalparty";

                ///<summary>
                /// SchemaName: CreatedByExternalPartyName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'createdbyexternalparty'
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
                //public const string createdbyexternalpartyname = "createdbyexternalpartyname";

                ///<summary>
                /// SchemaName: CreatedByExternalPartyYomiName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'createdbyexternalparty'
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
                //public const string createdbyexternalpartyyominame = "createdbyexternalpartyyominame";

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
                ///     (English - United States - 1033): Shows the date and time when the record was created. The date and time are displayed in the time zone selected in Microsoft Dynamics 365 options.
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
                /// IsGlobalFilterEnabled          True
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
                [System.ComponentModel.DescriptionAttribute("Created On")]
                public const string createdon = "createdon";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By (Delegate)
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows who created the record on behalf of another user.
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
                [System.ComponentModel.DescriptionAttribute("Created By (Delegate)")]
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
                ///     (English - United States - 1033): Credit Limit
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the credit limit of the account. This is a useful reference when you address invoice and accounting issues with the customer.
                /// 
                /// SchemaName: CreditLimit
                /// MoneyAttributeMetadata    AttributeType: Money    AttributeTypeName: MoneyType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 100000000000000    Precision = 2    PrecisionSource = 2
                /// IsBaseCurrency = False
                /// ImeMode = Disabled
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Credit Limit")]
                public const string creditlimit = "creditlimit";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Credit Limit (Base)
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the credit limit converted to the system's default base currency for reporting purposes.
                /// 
                /// SchemaName: CreditLimit_Base
                /// MoneyAttributeMetadata    AttributeType: Money    AttributeTypeName: MoneyType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -922337203685477    MaxValue = 922337203685477    Precision = 4    PrecisionSource = 2
                /// IsBaseCurrency = True
                /// ImeMode = Disabled    CalculationOf = creditlimit
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
                [System.ComponentModel.DescriptionAttribute("Credit Limit (Base)")]
                public const string creditlimit_base = "creditlimit_base";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Credit Hold
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether the credit for the account is on hold. This is a useful reference while addressing the invoice and accounting issues with the customer.
                /// 
                /// SchemaName: CreditOnHold
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
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
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Credit Hold")]
                public const string creditonhold = "creditonhold";

                ///<summary>
                /// SchemaName: CreditOnHoldName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'creditonhold'
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
                //public const string creditonholdname = "creditonholdname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Customer Size
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the size category or range of the account for segmentation and reporting purposes.
                /// 
                /// SchemaName: CustomerSizeCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_customersizecode
                /// DefaultFormValue = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Customer Size")]
                public const string customersizecode = "customersizecode";

                ///<summary>
                /// SchemaName: CustomerSizeCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'customersizecode'
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
                //public const string customersizecodename = "customersizecodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Relationship Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the category that best describes the relationship between the account and your organization.
                /// 
                /// SchemaName: CustomerTypeCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_customertypecode
                /// DefaultFormValue = -1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Relationship Type")]
                public const string customertypecode = "customertypecode";

                ///<summary>
                /// SchemaName: CustomerTypeCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'customertypecode'
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
                //public const string customertypecodename = "customertypecodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Price List
                /// 
                /// Description:
                ///     (English - United States - 1033): Choose the default price list associated with the account to make sure the correct product prices for this customer are applied in sales opportunities, quotes, and orders.
                /// 
                /// SchemaName: DefaultPriceLevelId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: pricelevel
                /// 
                ///     Target pricelevel    PrimaryIdAttribute pricelevelid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Price List
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Price Lists
                ///         
                ///         Description:
                ///             (English - United States - 1033): Entity that defines pricing levels.
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Price List")]
                public const string defaultpricelevelid = "defaultpricelevelid";

                ///<summary>
                /// SchemaName: DefaultPriceLevelIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired    AttributeOf 'defaultpricelevelid'
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
                //public const string defaultpricelevelidname = "defaultpricelevelidname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Description
                /// 
                /// Description:
                ///     (English - United States - 1033): Type additional information to describe the account, such as an excerpt from the company's website.
                /// 
                /// SchemaName: Description
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 2000
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Description")]
                public const string description = "description";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Do not allow Bulk Emails
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether the account allows bulk email sent through campaigns. If Do Not Allow is selected, the account can be added to marketing lists, but is excluded from email.
                /// 
                /// SchemaName: DoNotBulkEMail
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Allow
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Do Not Allow
                /// TrueOption = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Do not allow Bulk Emails")]
                public const string donotbulkemail = "donotbulkemail";

                ///<summary>
                /// SchemaName: DoNotBulkEMailName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'donotbulkemail'
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
                //public const string donotbulkemailname = "donotbulkemailname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Do not allow Bulk Mails
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether the account allows bulk postal mail sent through marketing campaigns or quick campaigns. If Do Not Allow is selected, the account can be added to marketing lists, but will be excluded from the postal mail.
                /// 
                /// SchemaName: DoNotBulkPostalMail
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
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
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Do not allow Bulk Mails")]
                public const string donotbulkpostalmail = "donotbulkpostalmail";

                ///<summary>
                /// SchemaName: DoNotBulkPostalMailName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'donotbulkpostalmail'
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
                //public const string donotbulkpostalmailname = "donotbulkpostalmailname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Do not allow Emails
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether the account allows direct email sent from Microsoft Dynamics 365.
                /// 
                /// SchemaName: DoNotEMail
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Allow
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Do Not Allow
                /// TrueOption = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Do not allow Emails")]
                public const string donotemail = "donotemail";

                ///<summary>
                /// SchemaName: DoNotEMailName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'donotemail'
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
                //public const string donotemailname = "donotemailname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Do not allow Faxes
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether the account allows faxes. If Do Not Allow is selected, the account will be excluded from fax activities distributed in marketing campaigns.
                /// 
                /// SchemaName: DoNotFax
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Allow
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Do Not Allow
                /// TrueOption = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Do not allow Faxes")]
                public const string donotfax = "donotfax";

                ///<summary>
                /// SchemaName: DoNotFaxName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'donotfax'
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
                //public const string donotfaxname = "donotfaxname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Do not allow Phone Calls
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether the account allows phone calls. If Do Not Allow is selected, the account will be excluded from phone call activities distributed in marketing campaigns.
                /// 
                /// SchemaName: DoNotPhone
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Allow
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Do Not Allow
                /// TrueOption = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Do not allow Phone Calls")]
                public const string donotphone = "donotphone";

                ///<summary>
                /// SchemaName: DoNotPhoneName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'donotphone'
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
                //public const string donotphonename = "donotphonename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Do not allow Mails
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether the account allows direct mail. If Do Not Allow is selected, the account will be excluded from letter activities distributed in marketing campaigns.
                /// 
                /// SchemaName: DoNotPostalMail
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Allow
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Do Not Allow
                /// TrueOption = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Do not allow Mails")]
                public const string donotpostalmail = "donotpostalmail";

                ///<summary>
                /// SchemaName: DoNotPostalMailName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'donotpostalmail'
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
                //public const string donotpostalmailname = "donotpostalmailname";

                ///<summary>
                /// SchemaName: DoNotSendMarketingMaterialName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'donotsendmm'
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
                //public const string donotsendmarketingmaterialname = "donotsendmarketingmaterialname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Send Marketing Materials
                /// 
                /// Description:
                ///     (English - United States - 1033): Select whether the account accepts marketing materials, such as brochures or catalogs.
                /// 
                /// SchemaName: DoNotSendMM
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = False
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Send
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Do Not Send
                /// TrueOption = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Send Marketing Materials")]
                public const string donotsendmm = "donotsendmm";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Email
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the primary email address for the account.
                /// 
                /// SchemaName: EMailAddress1
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Email    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                /// IsRetrievable                  True
                /// IsSearchable                   True
                /// IsSortableEnabled              True
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Email")]
                public const string emailaddress1 = "emailaddress1";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Email Address 2
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the secondary email address for the account.
                /// 
                /// SchemaName: EMailAddress2
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Email    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Email Address 2")]
                public const string emailaddress2 = "emailaddress2";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Email Address 3
                /// 
                /// Description:
                ///     (English - United States - 1033): Type an alternate email address for the account.
                /// 
                /// SchemaName: EMailAddress3
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Email    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Email Address 3")]
                public const string emailaddress3 = "emailaddress3";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Default Image
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the default image for the record.
                /// 
                /// SchemaName: EntityImage
                /// ImageAttributeMetadata    AttributeType: Virtual    AttributeTypeName: ImageType    RequiredLevel: None    AttributeOf 'entityimageid'
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// IsPrimaryImage = True    MaxHeight = 144    MaxWidth = 144
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
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Default Image")]
                public const string entityimage = "entityimage";

                ///<summary>
                /// SchemaName: EntityImage_Timestamp
                /// BigIntAttributeMetadata    AttributeType: BigInt    AttributeTypeName: BigIntType    RequiredLevel: None    AttributeOf 'entityimageid'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -9223372036854775808    MaxValue = 9223372036854775807
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
                public const string entityimage_timestamp = "entityimage_timestamp";

                ///<summary>
                /// SchemaName: EntityImage_URL
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'entityimageid'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Url    ImeMode = Disabled    IsLocalizable = False
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
                /// IsRetrievable                  True
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                public const string entityimage_url = "entityimage_url";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Entity Image Id
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                /// 
                /// SchemaName: EntityImageId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
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
                [System.ComponentModel.DescriptionAttribute("Entity Image Id")]
                public const string entityimageid = "entityimageid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Exchange Rate
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the conversion rate of the record's currency. The exchange rate is used to convert all money fields in the record from the local currency to the system's default currency.
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
                [System.ComponentModel.DescriptionAttribute("Exchange Rate")]
                public const string exchangerate = "exchangerate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Fax
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the fax number for the account.
                /// 
                /// SchemaName: Fax
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Fax")]
                public const string fax = "fax";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Follow Email Activity
                /// 
                /// Description:
                ///     (English - United States - 1033): Information about whether to allow following email activity like opens, attachment views and link clicks for emails sent to the account.
                /// 
                /// SchemaName: FollowEmail
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DefaultValue = True
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Do Not Allow
                /// FalseOption = 0
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Allow
                /// TrueOption = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.2.0.0
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
                [System.ComponentModel.DescriptionAttribute("Follow Email Activity")]
                public const string followemail = "followemail";

                ///<summary>
                /// SchemaName: FollowEmailName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'followemail'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
                /// IntroducedVersion              8.2.0.0
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
                //public const string followemailname = "followemailname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): FTP Site
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the URL for the account's FTP site to enable users to access data and share documents.
                /// 
                /// SchemaName: FtpSiteURL
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Url    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("FTP Site")]
                public const string ftpsiteurl = "ftpsiteurl";

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
                [System.ComponentModel.DescriptionAttribute("Import Sequence Number")]
                public const string importsequencenumber = "importsequencenumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Industry
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the account's primary industry for use in marketing segmentation and demographic analysis.
                /// 
                /// SchemaName: IndustryCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_industrycode
                /// DefaultFormValue = -1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
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
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Industry")]
                public const string industrycode = "industrycode";

                ///<summary>
                /// SchemaName: IndustryCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'industrycode'
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
                //public const string industrycodename = "industrycodename";

                ///<summary>
                /// SchemaName: IsPrivate
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: False    IsValidForAdvancedFind: False
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
                //public const string isprivate = "isprivate";

                ///<summary>
                /// SchemaName: IsPrivateName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'isprivate'
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
                //public const string isprivatename = "isprivatename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Last On Hold Time
                /// 
                /// Description:
                ///     (English - United States - 1033): Contains the date and time stamp of the last on hold time.
                /// 
                /// SchemaName: LastOnHoldTime
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateAndTime
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Last On Hold Time")]
                public const string lastonholdtime = "lastonholdtime";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Last Date Included in Campaign
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the date when the account was last included in a marketing campaign or quick campaign.
                /// 
                /// SchemaName: LastUsedInCampaign
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateOnly
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Last Date Included in Campaign")]
                public const string lastusedincampaign = "lastusedincampaign";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Market Capitalization
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the market capitalization of the account to identify the company's equity, used as an indicator in financial performance analysis.
                /// 
                /// SchemaName: MarketCap
                /// MoneyAttributeMetadata    AttributeType: Money    AttributeTypeName: MoneyType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 100000000000000    Precision = 2    PrecisionSource = 2
                /// IsBaseCurrency = False
                /// ImeMode = Disabled
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Market Capitalization")]
                public const string marketcap = "marketcap";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Market Capitalization (Base)
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the market capitalization converted to the system's default base currency.
                /// 
                /// SchemaName: MarketCap_Base
                /// MoneyAttributeMetadata    AttributeType: Money    AttributeTypeName: MoneyType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -922337203685477    MaxValue = 922337203685477    Precision = 4    PrecisionSource = 2
                /// IsBaseCurrency = True
                /// ImeMode = Disabled    CalculationOf = marketcap
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
                [System.ComponentModel.DescriptionAttribute("Market Capitalization (Base)")]
                public const string marketcap_base = "marketcap_base";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Marketing Only
                /// 
                /// Description:
                ///     (English - United States - 1033): Whether is only for marketing
                /// 
                /// SchemaName: MarketingOnly
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
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
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          True
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              8.2.0.0
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
                [System.ComponentModel.DescriptionAttribute("Marketing Only")]
                public const string marketingonly = "marketingonly";

                ///<summary>
                /// SchemaName: MarketingOnlyName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'marketingonly'
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
                /// IntroducedVersion              8.2.0.0
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
                //public const string marketingonlyname = "marketingonlyname";

                ///<summary>
                /// SchemaName: MasterAccountIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'masterid'
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
                //public const string masteraccountidname = "masteraccountidname";

                ///<summary>
                /// SchemaName: MasterAccountIdYomiName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'masterid'
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
                //public const string masteraccountidyominame = "masteraccountidyominame";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Master ID
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the master account that the account was merged with.
                /// 
                /// SchemaName: MasterId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: account
                /// 
                ///     Target account    PrimaryIdAttribute accountid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Account
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Accounts
                ///         
                ///         Description:
                ///             (English - United States - 1033): Business that represents a customer or potential customer. The company that is billed in business transactions.
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
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Master ID")]
                public const string masterid = "masterid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Merged
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows whether the account has been merged with another account.
                /// 
                /// SchemaName: Merged
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
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
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Merged")]
                public const string merged = "merged";

                ///<summary>
                /// SchemaName: MergedName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'merged'
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
                //public const string mergedname = "mergedname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows who last updated the record.
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
                [System.ComponentModel.DescriptionAttribute("Modified By")]
                public const string modifiedby = "modifiedby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By (External Party)
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the external party who modified the record.
                /// 
                /// SchemaName: ModifiedByExternalParty
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: externalparty
                /// 
                ///     Target externalparty    PrimaryIdAttribute externalpartyid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///             (English - United States - 1033): External Party
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): External Parties
                ///         
                ///         Description:
                ///             (English - United States - 1033): Information about external parties that need to access Dynamics 365 from external channels.For internal use only
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
                [System.ComponentModel.DescriptionAttribute("Modified By (External Party)")]
                public const string modifiedbyexternalparty = "modifiedbyexternalparty";

                ///<summary>
                /// SchemaName: ModifiedByExternalPartyName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'modifiedbyexternalparty'
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
                //public const string modifiedbyexternalpartyname = "modifiedbyexternalpartyname";

                ///<summary>
                /// SchemaName: ModifiedByExternalPartyYomiName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'modifiedbyexternalparty'
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
                //public const string modifiedbyexternalpartyyominame = "modifiedbyexternalpartyyominame";

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
                ///     (English - United States - 1033): Shows the date and time when the record was last updated. The date and time are displayed in the time zone selected in Microsoft Dynamics 365 options.
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
                [System.ComponentModel.DescriptionAttribute("Modified On")]
                public const string modifiedon = "modifiedon";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By (Delegate)
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows who created the record on behalf of another user.
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
                [System.ComponentModel.DescriptionAttribute("Modified By (Delegate)")]
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
                ///     (English - United States - 1033): Number of Employees
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the number of employees that work at the account for use in marketing segmentation and demographic analysis.
                /// 
                /// SchemaName: NumberOfEmployees
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 1000000000
                /// Format = None
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Number of Employees")]
                public const string numberofemployees = "numberofemployees";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): On Hold Time (Minutes)
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows how long, in minutes, that the record was on hold.
                /// 
                /// SchemaName: OnHoldTime
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
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
                [System.ComponentModel.DescriptionAttribute("On Hold Time (Minutes)")]
                public const string onholdtime = "onholdtime";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Open Deals
                /// 
                /// Description:
                ///     (English - United States - 1033): Number of open opportunities against an account and its child accounts.
                /// 
                /// SchemaName: OpenDeals
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: True    IsCustomAttribute: False    SourceType: Rollup
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None    FormulaDefinition is not null
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              7.0.0.0
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
                [System.ComponentModel.DescriptionAttribute("Open Deals")]
                public const string opendeals = "opendeals";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Open Deals (Last Updated On)
                /// 
                /// Description:
                ///     (English - United States - 1033): Last Updated time of rollup field Open Deals.
                /// 
                /// SchemaName: OpenDeals_Date
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateAndTime
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              7.0.0.0
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
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Open Deals (Last Updated On)")]
                public const string opendeals_date = "opendeals_date";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Open Deals (State)
                /// 
                /// Description:
                ///     (English - United States - 1033): State of rollup field Open Deals.
                /// 
                /// SchemaName: OpenDeals_State
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              7.0.0.0
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
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Open Deals (State)")]
                public const string opendeals_state = "opendeals_state";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Open Revenue
                /// 
                /// Description:
                ///     (English - United States - 1033): Sum of open revenue against an account and its child accounts.
                /// 
                /// SchemaName: OpenRevenue
                /// MoneyAttributeMetadata    AttributeType: Money    AttributeTypeName: MoneyType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: True    IsCustomAttribute: False    SourceType: Rollup
                /// MinValue = -922337203685477    MaxValue = 922337203685477    Precision = 2    PrecisionSource = 2
                /// IsBaseCurrency = False
                /// ImeMode = Auto    FormulaDefinition is not null
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              7.0.0.0
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
                [System.ComponentModel.DescriptionAttribute("Open Revenue")]
                public const string openrevenue = "openrevenue";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Open Revenue (Base)
                /// 
                /// Description:
                ///     (English - United States - 1033): Value of the Open Revenue in base currency.
                /// 
                /// SchemaName: OpenRevenue_Base
                /// MoneyAttributeMetadata    AttributeType: Money    AttributeTypeName: MoneyType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Rollup
                /// MinValue = -922337203685477    MaxValue = 922337203685477    Precision = 2    PrecisionSource = 2
                /// IsBaseCurrency = True
                /// ImeMode = Auto    CalculationOf = openrevenue
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              7.0.0.0
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
                [System.ComponentModel.DescriptionAttribute("Open Revenue (Base)")]
                public const string openrevenue_base = "openrevenue_base";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Open Revenue (Last Updated On)
                /// 
                /// Description:
                ///     (English - United States - 1033): Last Updated time of rollup field Open Revenue.
                /// 
                /// SchemaName: OpenRevenue_Date
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateAndTime
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              7.0.0.0
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
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Open Revenue (Last Updated On)")]
                public const string openrevenue_date = "openrevenue_date";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Open Revenue (State)
                /// 
                /// Description:
                ///     (English - United States - 1033): State of rollup field Open Revenue.
                /// 
                /// SchemaName: OpenRevenue_State
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              7.0.0.0
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
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Open Revenue (State)")]
                public const string openrevenue_state = "openrevenue_state";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Originating Lead
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the lead that the account was created from if the account was created by converting a lead in Microsoft Dynamics 365. This is used to relate the account to data on the originating lead for use in reporting and analytics.
                /// 
                /// SchemaName: OriginatingLeadId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: lead
                /// 
                ///     Target lead    PrimaryIdAttribute leadid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///             (English - United States - 1033): Lead
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Leads
                ///         
                ///         Description:
                ///             (English - United States - 1033): Prospect or potential sales opportunity. Leads are converted into accounts, contacts, or opportunities when they are qualified. Otherwise, they are deleted or archived.
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
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
                [System.ComponentModel.DescriptionAttribute("Originating Lead")]
                public const string originatingleadid = "originatingleadid";

                ///<summary>
                /// SchemaName: OriginatingLeadIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'originatingleadid'
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
                //public const string originatingleadidname = "originatingleadidname";

                ///<summary>
                /// SchemaName: OriginatingLeadIdYomiName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'originatingleadid'
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
                //public const string originatingleadidyominame = "originatingleadidyominame";

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
                [System.ComponentModel.DescriptionAttribute("Record Created On")]
                public const string overriddencreatedon = "overriddencreatedon";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Owner
                /// 
                /// Description:
                ///     (English - United States - 1033): Enter the user or team who is assigned to manage the record. This field is updated every time the record is assigned to a different user.
                /// 
                /// SchemaName: OwnerId
                /// LookupAttributeMetadata    AttributeType: Owner    AttributeTypeName: OwnerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser,team
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
                /// 
                ///     Target team    PrimaryIdAttribute teamid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Team
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Teams
                ///         
                ///         Description:
                ///             (English - United States - 1033): Collection of system users that routinely collaborate. Teams can be used to simplify record sharing and provide team members with common access to organization data when team members belong to different Business Units.
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   True
                /// IsGlobalFilterEnabled          True
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              True
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Owner")]
                public const string ownerid = "ownerid";

                ///<summary>
                /// SchemaName: OwnerIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired    AttributeOf 'ownerid'
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
                //public const string owneridname = "owneridname";

                ///<summary>
                /// SchemaName: OwnerIdType
                /// EntityNameAttributeMetadata    AttributeType: EntityName    AttributeTypeName: EntityNameType    RequiredLevel: SystemRequired    AttributeOf 'ownerid'
                /// IsValidForCreate: True    IsValidForUpdate: True
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
                //public const string owneridtype = "owneridtype";

                ///<summary>
                /// SchemaName: OwnerIdYomiName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired    AttributeOf 'ownerid'
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
                //public const string owneridyominame = "owneridyominame";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Ownership
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the account's ownership structure, such as public or private.
                /// 
                /// SchemaName: OwnershipCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_ownershipcode
                /// DefaultFormValue = -1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Ownership")]
                public const string ownershipcode = "ownershipcode";

                ///<summary>
                /// SchemaName: OwnershipCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'ownershipcode'
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
                //public const string ownershipcodename = "ownershipcodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Owning Business Unit
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the business unit that the record owner belongs to.
                /// 
                /// SchemaName: OwningBusinessUnit
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
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
                /// CanModifyAdditionalSettings    False
                /// IntroducedVersion              5.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   True
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
                [System.ComponentModel.DescriptionAttribute("Owning Business Unit")]
                public const string owningbusinessunit = "owningbusinessunit";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Owning Team
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the team who owns the account.
                /// 
                /// SchemaName: OwningTeam
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: team
                /// 
                ///     Target team    PrimaryIdAttribute teamid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Team
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Teams
                ///         
                ///         Description:
                ///             (English - United States - 1033): Collection of system users that routinely collaborate. Teams can be used to simplify record sharing and provide team members with common access to organization data when team members belong to different Business Units.
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
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Owning Team")]
                public const string owningteam = "owningteam";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Owning User
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who owns the account.
                /// 
                /// SchemaName: OwningUser
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
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
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Owning User")]
                public const string owninguser = "owninguser";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Parent Account
                /// 
                /// Description:
                ///     (English - United States - 1033): Choose the parent account associated with this account to show parent and child businesses in reporting and analytics.
                /// 
                /// SchemaName: ParentAccountId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: account
                /// 
                ///     Target account    PrimaryIdAttribute accountid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Account
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Accounts
                ///         
                ///         Description:
                ///             (English - United States - 1033): Business that represents a customer or potential customer. The company that is billed in business transactions.
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
                [System.ComponentModel.DescriptionAttribute("Parent Account")]
                public const string parentaccountid = "parentaccountid";

                ///<summary>
                /// SchemaName: ParentAccountIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'parentaccountid'
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
                //public const string parentaccountidname = "parentaccountidname";

                ///<summary>
                /// SchemaName: ParentAccountIdYomiName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'parentaccountid'
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
                //public const string parentaccountidyominame = "parentaccountidyominame";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Participates in Workflow
                /// 
                /// Description:
                ///     (English - United States - 1033): For system use only. Legacy Microsoft Dynamics CRM 3.0 workflow data.
                /// 
                /// SchemaName: ParticipatesInWorkflow
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
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
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Participates in Workflow")]
                public const string participatesinworkflow = "participatesinworkflow";

                ///<summary>
                /// SchemaName: ParticipatesInWorkflowName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'participatesinworkflow'
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
                //public const string participatesinworkflowname = "participatesinworkflowname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Payment Terms
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the payment terms to indicate when the customer needs to pay the total amount.
                /// 
                /// SchemaName: PaymentTermsCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_paymenttermscode
                /// DefaultFormValue = -1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Payment Terms")]
                public const string paymenttermscode = "paymenttermscode";

                ///<summary>
                /// SchemaName: PaymentTermsCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'paymenttermscode'
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
                //public const string paymenttermscodename = "paymenttermscodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Preferred Day
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the preferred day of the week for service appointments.
                /// 
                /// SchemaName: PreferredAppointmentDayCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_preferredappointmentdaycode
                /// DefaultFormValue = -1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Preferred Day")]
                public const string preferredappointmentdaycode = "preferredappointmentdaycode";

                ///<summary>
                /// SchemaName: PreferredAppointmentDayCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'preferredappointmentdaycode'
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
                //public const string preferredappointmentdaycodename = "preferredappointmentdaycodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Preferred Time
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the preferred time of day for service appointments.
                /// 
                /// SchemaName: PreferredAppointmentTimeCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_preferredappointmenttimecode
                /// DefaultFormValue = -1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Preferred Time")]
                public const string preferredappointmenttimecode = "preferredappointmenttimecode";

                ///<summary>
                /// SchemaName: PreferredAppointmentTimeCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'preferredappointmenttimecode'
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
                //public const string preferredappointmenttimecodename = "preferredappointmenttimecodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Preferred Method of Contact
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the preferred method of contact.
                /// 
                /// SchemaName: PreferredContactMethodCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_preferredcontactmethodcode
                /// DefaultFormValue = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Preferred Method of Contact")]
                public const string preferredcontactmethodcode = "preferredcontactmethodcode";

                ///<summary>
                /// SchemaName: PreferredContactMethodCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'preferredcontactmethodcode'
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
                //public const string preferredcontactmethodcodename = "preferredcontactmethodcodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Preferred Facility/Equipment
                /// 
                /// Description:
                ///     (English - United States - 1033): Choose the account's preferred service facility or equipment to make sure services are scheduled correctly for the customer.
                /// 
                /// SchemaName: PreferredEquipmentId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: equipment
                /// 
                ///     Target equipment    PrimaryIdAttribute equipmentid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Facility/Equipment
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Facilities/Equipment
                ///         
                ///         Description:
                ///             (English - United States - 1033): Resource that can be scheduled.
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Preferred Facility/Equipment")]
                public const string preferredequipmentid = "preferredequipmentid";

                ///<summary>
                /// SchemaName: PreferredEquipmentIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired    AttributeOf 'preferredequipmentid'
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
                //public const string preferredequipmentidname = "preferredequipmentidname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Preferred Service
                /// 
                /// Description:
                ///     (English - United States - 1033): Choose the account's preferred service for reference when you schedule service activities.
                /// 
                /// SchemaName: PreferredServiceId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: service
                /// 
                ///     Target service    PrimaryIdAttribute serviceid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Service
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Services
                ///         
                ///         Description:
                ///             (English - United States - 1033): Activity that represents work done to satisfy a customer's need.
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Preferred Service")]
                public const string preferredserviceid = "preferredserviceid";

                ///<summary>
                /// SchemaName: PreferredServiceIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired    AttributeOf 'preferredserviceid'
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
                //public const string preferredserviceidname = "preferredserviceidname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Preferred User
                /// 
                /// Description:
                ///     (English - United States - 1033): Choose the preferred service representative for reference when you schedule service activities for the account.
                /// 
                /// SchemaName: PreferredSystemUserId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
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
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Preferred User")]
                public const string preferredsystemuserid = "preferredsystemuserid";

                ///<summary>
                /// SchemaName: PreferredSystemUserIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'preferredsystemuserid'
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
                //public const string preferredsystemuseridname = "preferredsystemuseridname";

                ///<summary>
                /// SchemaName: PreferredSystemUserIdYomiName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'preferredsystemuserid'
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
                //public const string preferredsystemuseridyominame = "preferredsystemuseridyominame";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Primary Contact
                /// 
                /// Description:
                ///     (English - United States - 1033): Choose the primary contact for the account to provide quick access to contact details.
                /// 
                /// SchemaName: PrimaryContactId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: contact
                /// 
                ///     Target contact    PrimaryIdAttribute contactid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///             (English - United States - 1033): Contact
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Contacts
                ///         
                ///         Description:
                ///             (English - United States - 1033): Person with whom a business unit has a relationship, such as customer, supplier, and colleague.
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
                /// IsCustomizable                 True
                /// IsDataSourceSecret             False
                /// IsFilterable                   False
                /// IsGlobalFilterEnabled          True
                /// IsPrimaryId                    False
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  True
                /// IsSearchable                   False
                /// IsSortableEnabled              True
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Primary Contact")]
                public const string primarycontactid = "primarycontactid";

                ///<summary>
                /// SchemaName: PrimaryContactIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'primarycontactid'
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
                //public const string primarycontactidname = "primarycontactidname";

                ///<summary>
                /// SchemaName: PrimaryContactIdYomiName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'primarycontactid'
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
                //public const string primarycontactidyominame = "primarycontactidyominame";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Primary Satori ID
                /// 
                /// Description:
                ///     (English - United States - 1033): Primary Satori ID for Account
                /// 
                /// SchemaName: PrimarySatoriId
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
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
                [System.ComponentModel.DescriptionAttribute("Primary Satori ID")]
                public const string primarysatoriid = "primarysatoriid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Primary Twitter ID
                /// 
                /// Description:
                ///     (English - United States - 1033): Primary Twitter ID for Account
                /// 
                /// SchemaName: PrimaryTwitterId
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 128
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
                [System.ComponentModel.DescriptionAttribute("Primary Twitter ID")]
                public const string primarytwitterid = "primarytwitterid";

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
                [System.ComponentModel.DescriptionAttribute("Process")]
                public const string processid = "processid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Annual Revenue
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the annual revenue for the account, used as an indicator in financial performance analysis.
                /// 
                /// SchemaName: Revenue
                /// MoneyAttributeMetadata    AttributeType: Money    AttributeTypeName: MoneyType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 100000000000000    Precision = 2    PrecisionSource = 2
                /// IsBaseCurrency = False
                /// ImeMode = Disabled
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Annual Revenue")]
                public const string revenue = "revenue";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Annual Revenue (Base)
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows the annual revenue converted to the system's default base currency. The calculations use the exchange rate specified in the Currencies area.
                /// 
                /// SchemaName: Revenue_Base
                /// MoneyAttributeMetadata    AttributeType: Money    AttributeTypeName: MoneyType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -922337203685477    MaxValue = 922337203685477    Precision = 4    PrecisionSource = 2
                /// IsBaseCurrency = True
                /// ImeMode = Disabled    CalculationOf = revenue
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
                [System.ComponentModel.DescriptionAttribute("Annual Revenue (Base)")]
                public const string revenue_base = "revenue_base";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Shares Outstanding
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the number of shares available to the public for the account. This number is used as an indicator in financial performance analysis.
                /// 
                /// SchemaName: SharesOutstanding
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 1000000000
                /// Format = None
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Shares Outstanding")]
                public const string sharesoutstanding = "sharesoutstanding";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Shipping Method
                /// 
                /// Description:
                ///     (English - United States - 1033): Select a shipping method for deliveries sent to the account's address to designate the preferred carrier or other delivery option.
                /// 
                /// SchemaName: ShippingMethodCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_shippingmethodcode
                /// DefaultFormValue = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Shipping Method")]
                public const string shippingmethodcode = "shippingmethodcode";

                ///<summary>
                /// SchemaName: ShippingMethodCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'shippingmethodcode'
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
                //public const string shippingmethodcodename = "shippingmethodcodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): SIC Code
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the Standard Industrial Classification (SIC) code that indicates the account's primary industry of business, for use in marketing segmentation and demographic analysis.
                /// 
                /// SchemaName: SIC
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("SIC Code")]
                public const string sic = "sic";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): SLA
                /// 
                /// Description:
                ///     (English - United States - 1033): Choose the service level agreement (SLA) that you want to apply to the Account record.
                /// 
                /// SchemaName: SLAId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: sla
                /// 
                ///     Target sla    PrimaryIdAttribute slaid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): SLA
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): SLAs
                ///         
                ///         Description:
                ///             (English - United States - 1033): Contains information about the tracked service-level KPIs for cases that belong to different customers.
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("SLA")]
                public const string slaid = "slaid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Last SLA applied
                /// 
                /// Description:
                ///     (English - United States - 1033): Last SLA that was applied to this case. This field is for internal use only.
                /// 
                /// SchemaName: SLAInvokedId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: sla
                /// 
                ///     Target sla    PrimaryIdAttribute slaid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): SLA
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): SLAs
                ///         
                ///         Description:
                ///             (English - United States - 1033): Contains information about the tracked service-level KPIs for cases that belong to different customers.
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
                [System.ComponentModel.DescriptionAttribute("Last SLA applied")]
                public const string slainvokedid = "slainvokedid";

                ///<summary>
                /// SchemaName: SLAInvokedIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'slainvokedid'
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
                //public const string slainvokedidname = "slainvokedidname";

                ///<summary>
                /// SchemaName: SLAName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None    AttributeOf 'slaid'
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
                //public const string slaname = "slaname";

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
                [System.ComponentModel.DescriptionAttribute("Process Stage")]
                public const string stageid = "stageid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Status
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows whether the account is active or inactive. Inactive accounts are read-only and can't be edited unless they are reactivated.
                /// 
                /// SchemaName: StateCode
                /// StateAttributeMetadata    AttributeType: State    AttributeTypeName: StateType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// DefaultFormValue = 0
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
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Status")]
                public const string statecode = "statecode";

                ///<summary>
                /// SchemaName: StateCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'statecode'
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
                //public const string statecodename = "statecodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Status Reason
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the account's status.
                /// 
                /// SchemaName: StatusCode
                /// StatusAttributeMetadata    AttributeType: Status    AttributeTypeName: StatusType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// DefaultFormValue = -1
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
                [System.ComponentModel.DescriptionAttribute("Status Reason")]
                public const string statuscode = "statuscode";

                ///<summary>
                /// SchemaName: StatusCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'statuscode'
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
                //public const string statuscodename = "statuscodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Stock Exchange
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the stock exchange at which the account is listed to track their stock and financial performance of the company.
                /// 
                /// SchemaName: StockExchange
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Stock Exchange")]
                public const string stockexchange = "stockexchange";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Main Phone
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the main phone number for this account.
                /// 
                /// SchemaName: Telephone1
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                /// IsRetrievable                  True
                /// IsSearchable                   True
                /// IsSortableEnabled              True
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Main Phone")]
                public const string telephone1 = "telephone1";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Other Phone
                /// 
                /// Description:
                ///     (English - United States - 1033): Type a second phone number for this account.
                /// 
                /// SchemaName: Telephone2
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                /// IsSearchable                   True
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Other Phone")]
                public const string telephone2 = "telephone2";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Telephone 3
                /// 
                /// Description:
                ///     (English - United States - 1033): Type a third phone number for this account.
                /// 
                /// SchemaName: Telephone3
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Telephone 3")]
                public const string telephone3 = "telephone3";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Territory Code
                /// 
                /// Description:
                ///     (English - United States - 1033): Select a region or territory for the account for use in segmentation and analysis.
                /// 
                /// SchemaName: TerritoryCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet account_territorycode
                /// DefaultFormValue = 1
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Territory Code")]
                public const string territorycode = "territorycode";

                ///<summary>
                /// SchemaName: TerritoryCodeName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'territorycode'
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
                //public const string territorycodename = "territorycodename";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Territory
                /// 
                /// Description:
                ///     (English - United States - 1033): Choose the sales region or territory for the account to make sure the account is assigned to the correct representative and for use in segmentation and analysis.
                /// 
                /// SchemaName: TerritoryId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: territory
                /// 
                ///     Target territory    PrimaryIdAttribute territoryid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Territory
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Territories
                ///         
                ///         Description:
                ///             (English - United States - 1033): Territory represents sales regions.
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
                /// CanModifyAdditionalSettings    True
                /// IntroducedVersion              5.0.0.0
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
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Territory")]
                public const string territoryid = "territoryid";

                ///<summary>
                /// SchemaName: TerritoryIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired    AttributeOf 'territoryid'
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
                //public const string territoryidname = "territoryidname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Ticker Symbol
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the stock exchange symbol for the account to track financial performance of the company. You can click the code entered in this field to access the latest trading information from MSN Money.
                /// 
                /// SchemaName: TickerSymbol
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 10
                /// Format = TickerSymbol    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Ticker Symbol")]
                public const string tickersymbol = "tickersymbol";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Time Spent by me
                /// 
                /// Description:
                ///     (English - United States - 1033): Total time spent for emails (read and write) and meetings by me in relation to account record.
                /// 
                /// SchemaName: TimeSpentByMeOnEmailAndMeetings
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 1250
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          False
                /// CanBeSecuredForRead            False
                /// CanBeSecuredForUpdate          False
                /// CanModifyAdditionalSettings    False
                /// IntroducedVersion              8.2.0.0
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
                /// IsValidForGrid                 True
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Time Spent by me")]
                public const string timespentbymeonemailandmeetings = "timespentbymeonemailandmeetings";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Time Zone Rule Version Number
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                /// 
                /// SchemaName: TimeZoneRuleVersionNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -1    MaxValue = 2147483647
                /// Format = None
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
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Time Zone Rule Version Number")]
                public const string timezoneruleversionnumber = "timezoneruleversionnumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Currency
                /// 
                /// Description:
                ///     (English - United States - 1033): Choose the local currency for the record to make sure budgets are reported in the correct currency.
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
                [System.ComponentModel.DescriptionAttribute("Currency")]
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
                [System.ComponentModel.DescriptionAttribute("Traversed Path")]
                public const string traversedpath = "traversedpath";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): UTC Conversion Time Zone Code
                /// 
                /// Description:
                ///     (English - United States - 1033): Time zone code that was in use when the record was created.
                /// 
                /// SchemaName: UTCConversionTimeZoneCode
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -1    MaxValue = 2147483647
                /// Format = None
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
                /// IsValidForForm                 False
                /// IsValidForGrid                 False
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("UTC Conversion Time Zone Code")]
                public const string utcconversiontimezonecode = "utcconversiontimezonecode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Version Number
                /// 
                /// Description:
                ///     (English - United States - 1033): Version number of the account.
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
                [System.ComponentModel.DescriptionAttribute("Version Number")]
                public const string versionnumber = "versionnumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Website
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the account's website URL to get quick details about the company profile.
                /// 
                /// SchemaName: WebSiteURL
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Url    ImeMode = Inactive    IsLocalizable = False
                /// PropertyName                   Value
                /// CanBeSecuredForCreate          True
                /// CanBeSecuredForRead            True
                /// CanBeSecuredForUpdate          True
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
                [System.ComponentModel.DescriptionAttribute("Website")]
                public const string websiteurl = "websiteurl";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Yomi Account Name
                /// 
                /// Description:
                ///     (English - United States - 1033): Type the phonetic spelling of the company name, if specified in Japanese, to make sure the name is pronounced correctly in phone calls and other communications.
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
                [System.ComponentModel.DescriptionAttribute("Yomi Account Name")]
                public const string yominame = "yominame";
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
                ///     (English - United States - 1033): Status
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows whether the account is active or inactive. Inactive accounts are read-only and can't be edited unless they are reactivated.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Status")]
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
                    [System.ComponentModel.DescriptionAttribute("Active")]
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
                    [System.ComponentModel.DescriptionAttribute("Inactive")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Inactive_1 = 1,
                }

                ///<summary>
                /// Attribute: statuscode
                /// Value Format: Statecode_Statuscode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Status
                /// 
                /// Description:
                ///     (English - United States - 1033): Shows whether the account is active or inactive. Inactive accounts are read-only and can't be edited unless they are reactivated.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Status Reason")]
                public enum statuscode
                {
                    ///<summary>
                    /// Linked Statecode: Active_0, 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Active
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Active")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Active_0_Active_1 = 1,

                    ///<summary>
                    /// Linked Statecode: Inactive_1, 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Inactive
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Inactive")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Inactive_1_Inactive_2 = 2,
                }

                #endregion State and Status OptionSets.

                #region Picklist OptionSet OptionSets.

                ///<summary>
                /// Attribute:
                ///     accountcategorycode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Category
                /// 
                /// Description:
                ///     (English - United States - 1033): Select a category to indicate whether the customer account is standard or preferred.
                /// 
                /// Local System  OptionSet account_accountcategorycode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Category
                /// 
                /// Description:
                ///     (English - United States - 1033): Drop-down list for selecting the category of the account.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Category")]
                public enum accountcategorycode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Preferred Customer
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Preferred Customer")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Preferred_Customer_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Standard
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Standard")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Standard_2 = 2,
                }

                ///<summary>
                /// Attribute:
                ///     accountclassificationcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Classification
                /// 
                /// Description:
                ///     (English - United States - 1033): Select a classification code to indicate the potential value of the customer account based on the projected return on investment, cooperation level, sales cycle length or other criteria.
                /// 
                /// Local System  OptionSet account_accountclassificationcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Classification
                /// 
                /// Description:
                ///     (English - United States - 1033): Drop-down list for classifying an account.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Classification")]
                public enum accountclassificationcode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Default Value
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Default Value")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Default_Value_1 = 1,
                }

                ///<summary>
                /// Attribute:
                ///     accountratingcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Account Rating
                /// 
                /// Description:
                ///     (English - United States - 1033): Select a rating to indicate the value of the customer account.
                /// 
                /// Local System  OptionSet account_accountratingcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Account Rating
                /// 
                /// Description:
                ///     (English - United States - 1033): Drop-down list for selecting account ratings.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Account Rating")]
                public enum accountratingcode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Default Value
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Default Value")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Default_Value_1 = 1,
                }

                ///<summary>
                /// Attribute:
                ///     address1_addresstypecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Address Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the primary address type.
                /// 
                /// Local System  OptionSet account_address1_addresstypecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Address Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of address for address 1, such as billing, shipping, or primary address.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Address 1: Address Type")]
                public enum address1_addresstypecode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Bill To
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Bill To")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Bill_To_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Ship To
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Ship To")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Ship_To_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Primary
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Primary")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Primary_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Other
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Other")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Other_4 = 4,
                }

                ///<summary>
                /// Attribute:
                ///     address1_freighttermscode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Freight Terms
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the freight terms for the primary address to make sure shipping orders are processed correctly.
                /// 
                /// Local System  OptionSet account_address1_freighttermscode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Freight Terms
                /// 
                /// Description:
                ///     (English - United States - 1033): Freight terms for address 1.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Address 1: Freight Terms")]
                public enum address1_freighttermscode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): FOB
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("FOB")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    FOB_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): No Charge
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("No Charge")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    No_Charge_2 = 2,
                }

                ///<summary>
                /// Attribute:
                ///     address1_shippingmethodcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Shipping Method
                /// 
                /// Description:
                ///     (English - United States - 1033): Select a shipping method for deliveries sent to this address.
                /// 
                /// Local System  OptionSet account_address1_shippingmethodcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Shipping Method
                /// 
                /// Description:
                ///     (English - United States - 1033): Method of shipment for address 1.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Address 1: Shipping Method")]
                public enum address1_shippingmethodcode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Airborne
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Airborne")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Airborne_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): DHL
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("DHL")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    DHL_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): FedEx
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("FedEx")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    FedEx_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): UPS
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("UPS")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    UPS_4 = 4,

                    ///<summary>
                    /// 5
                    /// DisplayOrder: 5
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Postal Mail
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Postal Mail")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Postal_Mail_5 = 5,

                    ///<summary>
                    /// 6
                    /// DisplayOrder: 6
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Full Load
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Full Load")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Full_Load_6 = 6,

                    ///<summary>
                    /// 7
                    /// DisplayOrder: 7
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Will Call
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Will Call")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Will_Call_7 = 7,
                }

                ///<summary>
                /// Attribute:
                ///     address2_addresstypecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Address Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the secondary address type.
                /// 
                /// Local System  OptionSet account_address2_addresstypecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Address Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of address for address 2, such as billing, shipping, or primary address.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Address 2: Address Type")]
                public enum address2_addresstypecode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Default Value
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Default Value")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Default_Value_1 = 1,
                }

                ///<summary>
                /// Attribute:
                ///     address2_freighttermscode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Freight Terms
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the freight terms for the secondary address to make sure shipping orders are processed correctly.
                /// 
                /// Local System  OptionSet account_address2_freighttermscode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Freight Terms
                /// 
                /// Description:
                ///     (English - United States - 1033): Freight terms for address 2.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Address 2: Freight Terms")]
                public enum address2_freighttermscode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Default Value
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Default Value")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Default_Value_1 = 1,
                }

                ///<summary>
                /// Attribute:
                ///     address2_shippingmethodcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Shipping Method
                /// 
                /// Description:
                ///     (English - United States - 1033): Select a shipping method for deliveries sent to this address.
                /// 
                /// Local System  OptionSet account_address2_shippingmethodcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Shipping Method 
                /// 
                /// Description:
                ///     (English - United States - 1033): Method of shipment for address 2.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Address 2: Shipping Method ")]
                public enum address2_shippingmethodcode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Default Value
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Default Value")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Default_Value_1 = 1,
                }

                ///<summary>
                /// Attribute:
                ///     businesstypecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Business Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the legal designation or other business type of the account for contracts or reporting purposes.
                /// 
                /// Local System  OptionSet account_businesstypecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Business Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of business associated with the account.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Business Type")]
                public enum businesstypecode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Default Value
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Default Value")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Default_Value_1 = 1,
                }

                ///<summary>
                /// Attribute:
                ///     customersizecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Customer Size
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the size category or range of the account for segmentation and reporting purposes.
                /// 
                /// Local System  OptionSet account_customersizecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Customer Size
                /// 
                /// Description:
                ///     (English - United States - 1033): Size of the account.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Customer Size")]
                public enum customersizecode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Default Value
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Default Value")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Default_Value_1 = 1,
                }

                ///<summary>
                /// Attribute:
                ///     customertypecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Relationship Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the category that best describes the relationship between the account and your organization.
                /// 
                /// Local System  OptionSet account_customertypecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Relationship Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of the account.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Relationship Type")]
                public enum customertypecode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Competitor
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Competitor")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Competitor_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Consultant
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Consultant")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Consultant_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Customer
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Customer")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Customer_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Investor
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Investor")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Investor_4 = 4,

                    ///<summary>
                    /// 5
                    /// DisplayOrder: 5
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Partner
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Partner")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Partner_5 = 5,

                    ///<summary>
                    /// 6
                    /// DisplayOrder: 6
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Influencer
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Influencer")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Influencer_6 = 6,

                    ///<summary>
                    /// 7
                    /// DisplayOrder: 7
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Press
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Press")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Press_7 = 7,

                    ///<summary>
                    /// 8
                    /// DisplayOrder: 8
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Prospect
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Prospect")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Prospect_8 = 8,

                    ///<summary>
                    /// 9
                    /// DisplayOrder: 9
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Reseller
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Reseller")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Reseller_9 = 9,

                    ///<summary>
                    /// 10
                    /// DisplayOrder: 10
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Supplier
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Supplier")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Supplier_10 = 10,

                    ///<summary>
                    /// 11
                    /// DisplayOrder: 11
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Vendor
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Vendor")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Vendor_11 = 11,

                    ///<summary>
                    /// 12
                    /// DisplayOrder: 12
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Other
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Other")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Other_12 = 12,
                }

                ///<summary>
                /// Attribute:
                ///     industrycode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Industry
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the account's primary industry for use in marketing segmentation and demographic analysis.
                /// 
                /// Local System  OptionSet account_industrycode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Industry
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of industry with which the account is associated.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Industry")]
                public enum industrycode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Accounting
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Accounting")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Accounting_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Agriculture and Non-petrol Natural Resource Extraction
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Agriculture and Non-petrol Natural Resource Extraction")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Agriculture_and_Non_petrol_Natural_Resource_Extraction_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Broadcasting Printing and Publishing
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Broadcasting Printing and Publishing")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Broadcasting_Printing_and_Publishing_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Brokers
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Brokers")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Brokers_4 = 4,

                    ///<summary>
                    /// 5
                    /// DisplayOrder: 5
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Building Supply Retail
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Building Supply Retail")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Building_Supply_Retail_5 = 5,

                    ///<summary>
                    /// 6
                    /// DisplayOrder: 6
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Business Services
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Business Services")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Business_Services_6 = 6,

                    ///<summary>
                    /// 7
                    /// DisplayOrder: 7
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Consulting
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Consulting")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Consulting_7 = 7,

                    ///<summary>
                    /// 8
                    /// DisplayOrder: 8
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Consumer Services
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Consumer Services")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Consumer_Services_8 = 8,

                    ///<summary>
                    /// 9
                    /// DisplayOrder: 9
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Design, Direction and Creative Management
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Design, Direction and Creative Management")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Design_Direction_and_Creative_Management_9 = 9,

                    ///<summary>
                    /// 10
                    /// DisplayOrder: 10
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Distributors, Dispatchers and Processors
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Distributors, Dispatchers and Processors")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Distributors_Dispatchers_and_Processors_10 = 10,

                    ///<summary>
                    /// 11
                    /// DisplayOrder: 11
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Doctor's Offices and Clinics
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Doctor's Offices and Clinics")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Doctor_s_Offices_and_Clinics_11 = 11,

                    ///<summary>
                    /// 12
                    /// DisplayOrder: 12
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Durable Manufacturing
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Durable Manufacturing")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Durable_Manufacturing_12 = 12,

                    ///<summary>
                    /// 13
                    /// DisplayOrder: 13
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Eating and Drinking Places
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Eating and Drinking Places")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Eating_and_Drinking_Places_13 = 13,

                    ///<summary>
                    /// 14
                    /// DisplayOrder: 14
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Entertainment Retail
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Entertainment Retail")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Entertainment_Retail_14 = 14,

                    ///<summary>
                    /// 15
                    /// DisplayOrder: 15
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Equipment Rental and Leasing
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Equipment Rental and Leasing")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Equipment_Rental_and_Leasing_15 = 15,

                    ///<summary>
                    /// 16
                    /// DisplayOrder: 16
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Financial
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Financial")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Financial_16 = 16,

                    ///<summary>
                    /// 17
                    /// DisplayOrder: 17
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Food and Tobacco Processing
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Food and Tobacco Processing")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Food_and_Tobacco_Processing_17 = 17,

                    ///<summary>
                    /// 18
                    /// DisplayOrder: 18
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Inbound Capital Intensive Processing
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Inbound Capital Intensive Processing")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Inbound_Capital_Intensive_Processing_18 = 18,

                    ///<summary>
                    /// 19
                    /// DisplayOrder: 19
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Inbound Repair and Services
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Inbound Repair and Services")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Inbound_Repair_and_Services_19 = 19,

                    ///<summary>
                    /// 20
                    /// DisplayOrder: 20
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Insurance
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Insurance")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Insurance_20 = 20,

                    ///<summary>
                    /// 21
                    /// DisplayOrder: 21
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Legal Services
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Legal Services")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Legal_Services_21 = 21,

                    ///<summary>
                    /// 22
                    /// DisplayOrder: 22
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Non-Durable Merchandise Retail
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Non-Durable Merchandise Retail")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Non_Durable_Merchandise_Retail_22 = 22,

                    ///<summary>
                    /// 23
                    /// DisplayOrder: 23
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Outbound Consumer Service
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Outbound Consumer Service")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Outbound_Consumer_Service_23 = 23,

                    ///<summary>
                    /// 24
                    /// DisplayOrder: 24
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Petrochemical Extraction and Distribution
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Petrochemical Extraction and Distribution")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Petrochemical_Extraction_and_Distribution_24 = 24,

                    ///<summary>
                    /// 25
                    /// DisplayOrder: 25
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Service Retail
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Service Retail")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Service_Retail_25 = 25,

                    ///<summary>
                    /// 26
                    /// DisplayOrder: 26
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): SIG Affiliations
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("SIG Affiliations")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    SIG_Affiliations_26 = 26,

                    ///<summary>
                    /// 27
                    /// DisplayOrder: 27
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Social Services
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Social Services")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Social_Services_27 = 27,

                    ///<summary>
                    /// 28
                    /// DisplayOrder: 28
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Special Outbound Trade Contractors
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Special Outbound Trade Contractors")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Special_Outbound_Trade_Contractors_28 = 28,

                    ///<summary>
                    /// 29
                    /// DisplayOrder: 29
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Specialty Realty
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Specialty Realty")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Specialty_Realty_29 = 29,

                    ///<summary>
                    /// 30
                    /// DisplayOrder: 30
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Transportation
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Transportation")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Transportation_30 = 30,

                    ///<summary>
                    /// 31
                    /// DisplayOrder: 31
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Utility Creation and Distribution
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Utility Creation and Distribution")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Utility_Creation_and_Distribution_31 = 31,

                    ///<summary>
                    /// 32
                    /// DisplayOrder: 32
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Vehicle Retail
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Vehicle Retail")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Vehicle_Retail_32 = 32,

                    ///<summary>
                    /// 33
                    /// DisplayOrder: 33
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Wholesale
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Wholesale")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Wholesale_33 = 33,
                }

                ///<summary>
                /// Attribute:
                ///     ownershipcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Ownership
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the account's ownership structure, such as public or private.
                /// 
                /// Local System  OptionSet account_ownershipcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Ownership
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of company ownership, such as public or private.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Ownership")]
                public enum ownershipcode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Public
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Public")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Public_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Private
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Private")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Private_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Subsidiary
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Subsidiary")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Subsidiary_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Other
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Other")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Other_4 = 4,
                }

                ///<summary>
                /// Attribute:
                ///     paymenttermscode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Payment Terms
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the payment terms to indicate when the customer needs to pay the total amount.
                /// 
                /// Local System  OptionSet account_paymenttermscode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Payment Terms
                /// 
                /// Description:
                ///     (English - United States - 1033): Payment terms for the account.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Payment Terms")]
                public enum paymenttermscode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Net 30
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Net 30")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Net_30_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): 2% 10, Net 30
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("2% 10, Net 30")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    V_2_10_Net_30_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Net 45
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Net 45")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Net_45_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Net 60
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Net 60")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Net_60_4 = 4,
                }

                ///<summary>
                /// Attribute:
                ///     preferredappointmentdaycode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Preferred Day
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the preferred day of the week for service appointments.
                /// 
                /// Local System  OptionSet account_preferredappointmentdaycode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Preferred Day
                /// 
                /// Description:
                ///     (English - United States - 1033): Day of the week that the account prefers for scheduling service activities.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Preferred Day")]
                public enum preferredappointmentdaycode
                {
                    ///<summary>
                    /// 0
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Sunday
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Sunday")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Sunday_0 = 0,

                    ///<summary>
                    /// 1
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Monday
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Monday")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Monday_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Tuesday
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Tuesday")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Tuesday_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Wednesday
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Wednesday")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Wednesday_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 5
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Thursday
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Thursday")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Thursday_4 = 4,

                    ///<summary>
                    /// 5
                    /// DisplayOrder: 6
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Friday
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Friday")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Friday_5 = 5,

                    ///<summary>
                    /// 6
                    /// DisplayOrder: 7
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Saturday
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Saturday")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Saturday_6 = 6,
                }

                ///<summary>
                /// Attribute:
                ///     preferredappointmenttimecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Preferred Time
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the preferred time of day for service appointments.
                /// 
                /// Local System  OptionSet account_preferredappointmenttimecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Preferred Time
                /// 
                /// Description:
                ///     (English - United States - 1033): Time of day that the account prefers for scheduling service activities.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Preferred Time")]
                public enum preferredappointmenttimecode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Morning
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Morning")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Morning_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Afternoon
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Afternoon")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Afternoon_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Evening
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Evening")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Evening_3 = 3,
                }

                ///<summary>
                /// Attribute:
                ///     preferredcontactmethodcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Preferred Method of Contact
                /// 
                /// Description:
                ///     (English - United States - 1033): Select the preferred method of contact.
                /// 
                /// Local System  OptionSet account_preferredcontactmethodcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Preferred Method of Contact
                /// 
                /// Description:
                ///     (English - United States - 1033): Preferred contact method for the account.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Preferred Method of Contact")]
                public enum preferredcontactmethodcode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Any
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Any")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Any_1 = 1,

                    ///<summary>
                    /// 2
                    /// DisplayOrder: 2
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Email
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Email")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Email_2 = 2,

                    ///<summary>
                    /// 3
                    /// DisplayOrder: 3
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Phone
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Phone")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Phone_3 = 3,

                    ///<summary>
                    /// 4
                    /// DisplayOrder: 4
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Fax
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Fax")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Fax_4 = 4,

                    ///<summary>
                    /// 5
                    /// DisplayOrder: 5
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Mail
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Mail")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Mail_5 = 5,
                }

                ///<summary>
                /// Attribute:
                ///     shippingmethodcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Shipping Method
                /// 
                /// Description:
                ///     (English - United States - 1033): Select a shipping method for deliveries sent to the account's address to designate the preferred carrier or other delivery option.
                /// 
                /// Local System  OptionSet account_shippingmethodcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Shipping Method
                /// 
                /// Description:
                ///     (English - United States - 1033): Method of shipment for the account.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Shipping Method")]
                public enum shippingmethodcode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Default Value
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Default Value")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Default_Value_1 = 1,
                }

                ///<summary>
                /// Attribute:
                ///     territorycode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Territory Code
                /// 
                /// Description:
                ///     (English - United States - 1033): Select a region or territory for the account for use in segmentation and analysis.
                /// 
                /// Local System  OptionSet account_territorycode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Territory Code
                /// 
                /// Description:
                ///     (English - United States - 1033): Territory to which the account belongs.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("Territory Code")]
                public enum territorycode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Default Value
                    ///</summary>
                    [System.ComponentModel.DescriptionAttribute("Default Value")]
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Default_Value_1 = 1,
                }

                #endregion Picklist OptionSets.
            }

            #endregion OptionSets.

            #region Relationship ManyToOne - N:1.

            public static partial class ManyToOne
            {
                ///<summary>
                /// N:1 - Relationship account_master_account
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_master_account
                /// ReferencingEntityNavigationPropertyName    masterid
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
                /// AttributeMaps:
                ///     SourceEntity                       TargetEntity
                ///     account                      ->    account
                ///     
                ///     SourceAttribute                    TargetAttribute
                ///     accountid                    ->    parentaccountid
                ///     defaultpricelevelid          ->    defaultpricelevelid
                ///     defaultpricelevelidname      ->    defaultpricelevelidname
                ///     name                         ->    parentaccountidname
                ///     transactioncurrencyid        ->    transactioncurrencyid
                ///     transactioncurrencyiddsc     ->    transactioncurrencyiddsc
                ///     transactioncurrencyidname    ->    transactioncurrencyidname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship account_master_account")]
                public static partial class account_master_account
                {
                    public const string Name = "account_master_account";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_masterid = "masterid";
                }

                ///<summary>
                /// N:1 - Relationship account_originating_lead
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_originating_lead
                /// ReferencingEntityNavigationPropertyName    originatingleadid
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
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
                /// ReferencedEntity lead:    PrimaryIdAttribute leadid    PrimaryNameAttribute fullname
                ///     DisplayName:
                ///         (English - United States - 1033): Lead
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Leads
                ///     
                ///     Description:
                ///         (English - United States - 1033): Prospect or potential sales opportunity. Leads are converted into accounts, contacts, or opportunities when they are qualified. Otherwise, they are deleted or archived.
                /// 
                /// AttributeMaps:
                ///     SourceEntity                        TargetEntity
                ///     lead                          ->    account
                ///     
                ///     SourceAttribute                     TargetAttribute
                ///     address1_city                 ->    address1_city
                ///     address1_country              ->    address1_country
                ///     address1_line1                ->    address1_line1
                ///     address1_line2                ->    address1_line2
                ///     address1_line3                ->    address1_line3
                ///     address1_postalcode           ->    address1_postalcode
                ///     address1_stateorprovince      ->    address1_stateorprovince
                ///     companyname                   ->    name
                ///     description                   ->    description
                ///     donotbulkemail                ->    donotbulkemail
                ///     donotemail                    ->    donotemail
                ///     donotfax                      ->    donotfax
                ///     donotphone                    ->    donotphone
                ///     donotpostalmail               ->    donotpostalmail
                ///     donotsendmm                   ->    donotsendmm
                ///     emailaddress1                 ->    emailaddress1
                ///     fax                           ->    fax
                ///     followemail                   ->    followemail
                ///     fullname                      ->    originatingleadidname
                ///     industrycode                  ->    industrycode
                ///     leadid                        ->    originatingleadid
                ///     numberofemployees             ->    numberofemployees
                ///     ownerid                       ->    ownerid
                ///     owneriddsc                    ->    owneriddsc
                ///     owneridname                   ->    owneridname
                ///     owneridtype                   ->    owneridtype
                ///     preferredcontactmethodcode    ->    preferredcontactmethodcode
                ///     revenue                       ->    revenue
                ///     sic                           ->    sic
                ///     telephone1                    ->    telephone1
                ///     telephone3                    ->    telephone2
                ///     transactioncurrencyid         ->    transactioncurrencyid
                ///     transactioncurrencyidname     ->    transactioncurrencyidname
                ///     websiteurl                    ->    websiteurl
                ///     yomicompanyname               ->    yominame
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship account_originating_lead")]
                public static partial class account_originating_lead
                {
                    public const string Name = "account_originating_lead";

                    public const string ReferencedEntity_lead = "lead";

                    public const string ReferencedAttribute_leadid = "leadid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_originatingleadid = "originatingleadid";
                }

                ///<summary>
                /// N:1 - Relationship account_parent_account
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             True
                /// ReferencedEntityNavigationPropertyName     account_parent_account
                /// ReferencingEntityNavigationPropertyName    parentaccountid
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                RemoveLink
                /// CascadeConfiguration.Merge                 Cascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          40
                /// 
                /// AttributeMaps:
                ///     SourceEntity                       TargetEntity
                ///     account                      ->    account
                ///     
                ///     SourceAttribute                    TargetAttribute
                ///     accountid                    ->    parentaccountid
                ///     defaultpricelevelid          ->    defaultpricelevelid
                ///     defaultpricelevelidname      ->    defaultpricelevelidname
                ///     name                         ->    parentaccountidname
                ///     transactioncurrencyid        ->    transactioncurrencyid
                ///     transactioncurrencyiddsc     ->    transactioncurrencyiddsc
                ///     transactioncurrencyidname    ->    transactioncurrencyidname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship account_parent_account")]
                public static partial class account_parent_account
                {
                    public const string Name = "account_parent_account";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_parentaccountid = "parentaccountid";
                }

                ///<summary>
                /// N:1 - Relationship account_primary_contact
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_primary_contact
                /// ReferencingEntityNavigationPropertyName    primarycontactid
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
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
                /// ReferencedEntity contact:    PrimaryIdAttribute contactid    PrimaryNameAttribute fullname
                ///     DisplayName:
                ///         (English - United States - 1033): Contact
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Contacts
                ///     
                ///     Description:
                ///         (English - United States - 1033): Person with whom a business unit has a relationship, such as customer, supplier, and colleague.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship account_primary_contact")]
                public static partial class account_primary_contact
                {
                    public const string Name = "account_primary_contact";

                    public const string ReferencedEntity_contact = "contact";

                    public const string ReferencedAttribute_contactid = "contactid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_primarycontactid = "primarycontactid";
                }

                ///<summary>
                /// N:1 - Relationship business_unit_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_accounts
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// ReferencedEntity businessunit:    PrimaryIdAttribute businessunitid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Business Unit
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Business Units
                ///     
                ///     Description:
                ///         (English - United States - 1033): Business, division, or department in the Microsoft Dynamics 365 database.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship business_unit_accounts")]
                public static partial class business_unit_accounts
                {
                    public const string Name = "business_unit_accounts";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";
                }

                ///<summary>
                /// N:1 - Relationship equipment_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     equipment_accounts
                /// ReferencingEntityNavigationPropertyName    preferredequipmentid
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
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
                /// ReferencedEntity equipment:    PrimaryIdAttribute equipmentid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Facility/Equipment
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Facilities/Equipment
                ///     
                ///     Description:
                ///         (English - United States - 1033): Resource that can be scheduled.
                /// 
                /// AttributeMaps:
                ///     SourceEntity             TargetEntity
                ///     equipment          ->    account
                ///     
                ///     SourceAttribute          TargetAttribute
                ///     equipmentid        ->    preferredequipmentid
                ///     name               ->    preferredequipmentidname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship equipment_accounts")]
                public static partial class equipment_accounts
                {
                    public const string Name = "equipment_accounts";

                    public const string ReferencedEntity_equipment = "equipment";

                    public const string ReferencedAttribute_equipmentid = "equipmentid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_preferredequipmentid = "preferredequipmentid";
                }

                ///<summary>
                /// N:1 - Relationship lk_account_entityimage
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_account_entityimage
                /// ReferencingEntityNavigationPropertyName    entityimageid_imagedescriptor
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
                /// ReferencedEntity imagedescriptor:    PrimaryIdAttribute imagedescriptorid
                ///     DisplayName:
                ///         (English - United States - 1033): Image Descriptor
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Image Descriptors
                ///     
                ///     Description:
                ///         (English - United States - 1033): For internal use only.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship lk_account_entityimage")]
                public static partial class lk_account_entityimage
                {
                    public const string Name = "lk_account_entityimage";

                    public const string ReferencedEntity_imagedescriptor = "imagedescriptor";

                    public const string ReferencedAttribute_imagedescriptorid = "imagedescriptorid";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_entityimageid = "entityimageid";
                }

                ///<summary>
                /// N:1 - Relationship lk_accountbase_createdby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_accountbase_createdby
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
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship lk_accountbase_createdby")]
                public static partial class lk_accountbase_createdby
                {
                    public const string Name = "lk_accountbase_createdby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_createdby = "createdby";
                }

                ///<summary>
                /// N:1 - Relationship lk_accountbase_createdonbehalfby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_accountbase_createdonbehalfby
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
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship lk_accountbase_createdonbehalfby")]
                public static partial class lk_accountbase_createdonbehalfby
                {
                    public const string Name = "lk_accountbase_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_accountbase_modifiedby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_accountbase_modifiedby
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
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship lk_accountbase_modifiedby")]
                public static partial class lk_accountbase_modifiedby
                {
                    public const string Name = "lk_accountbase_modifiedby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";
                }

                ///<summary>
                /// N:1 - Relationship lk_accountbase_modifiedonbehalfby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_accountbase_modifiedonbehalfby
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
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship lk_accountbase_modifiedonbehalfby")]
                public static partial class lk_accountbase_modifiedonbehalfby
                {
                    public const string Name = "lk_accountbase_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_externalparty_account_createdby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_externalparty_account_createdby
                /// ReferencingEntityNavigationPropertyName    CreatedByExternalParty
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
                /// ReferencedEntity externalparty:    PrimaryIdAttribute externalpartyid    PrimaryNameAttribute fullname
                ///     DisplayName:
                ///         (English - United States - 1033): External Party
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): External Parties
                ///     
                ///     Description:
                ///         (English - United States - 1033): Information about external parties that need to access Dynamics 365 from external channels.For internal use only
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship lk_externalparty_account_createdby")]
                public static partial class lk_externalparty_account_createdby
                {
                    public const string Name = "lk_externalparty_account_createdby";

                    public const string ReferencedEntity_externalparty = "externalparty";

                    public const string ReferencedAttribute_externalpartyid = "externalpartyid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_createdbyexternalparty = "createdbyexternalparty";
                }

                ///<summary>
                /// N:1 - Relationship lk_externalparty_account_modifiedby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_externalparty_account_modifiedby
                /// ReferencingEntityNavigationPropertyName    ModifiedByExternalParty
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
                /// ReferencedEntity externalparty:    PrimaryIdAttribute externalpartyid    PrimaryNameAttribute fullname
                ///     DisplayName:
                ///         (English - United States - 1033): External Party
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): External Parties
                ///     
                ///     Description:
                ///         (English - United States - 1033): Information about external parties that need to access Dynamics 365 from external channels.For internal use only
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship lk_externalparty_account_modifiedby")]
                public static partial class lk_externalparty_account_modifiedby
                {
                    public const string Name = "lk_externalparty_account_modifiedby";

                    public const string ReferencedEntity_externalparty = "externalparty";

                    public const string ReferencedAttribute_externalpartyid = "externalpartyid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_modifiedbyexternalparty = "modifiedbyexternalparty";
                }

                ///<summary>
                /// N:1 - Relationship manualsla_account
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     manualsla_account
                /// ReferencingEntityNavigationPropertyName    sla_account_sla
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                RemoveLink
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          10000
                /// 
                /// ReferencedEntity sla:    PrimaryIdAttribute slaid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): SLA
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): SLAs
                ///     
                ///     Description:
                ///         (English - United States - 1033): Contains information about the tracked service-level KPIs for cases that belong to different customers.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship manualsla_account")]
                public static partial class manualsla_account
                {
                    public const string Name = "manualsla_account";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_slaid = "slaid";
                }

                ///<summary>
                /// N:1 - Relationship owner_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     owner_accounts
                /// ReferencingEntityNavigationPropertyName    ownerid
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
                /// ReferencedEntity owner:    PrimaryIdAttribute ownerid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Owner
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Owners
                ///     
                ///     Description:
                ///         (English - United States - 1033): Group of undeleted system users and undeleted teams. Owners can be used to control access to specific objects.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship owner_accounts")]
                public static partial class owner_accounts
                {
                    public const string Name = "owner_accounts";

                    public const string ReferencedEntity_owner = "owner";

                    public const string ReferencedAttribute_ownerid = "ownerid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_ownerid = "ownerid";
                }

                ///<summary>
                /// N:1 - Relationship price_level_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     price_level_accounts
                /// ReferencingEntityNavigationPropertyName    defaultpricelevelid
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity pricelevel:    PrimaryIdAttribute pricelevelid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Price List
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Price Lists
                ///     
                ///     Description:
                ///         (English - United States - 1033): Entity that defines pricing levels.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship price_level_accounts")]
                public static partial class price_level_accounts
                {
                    public const string Name = "price_level_accounts";

                    public const string ReferencedEntity_pricelevel = "pricelevel";

                    public const string ReferencedAttribute_pricelevelid = "pricelevelid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_defaultpricelevelid = "defaultpricelevelid";
                }

                ///<summary>
                /// N:1 - Relationship processstage_account
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     processstage_account
                /// ReferencingEntityNavigationPropertyName    stageid_processstage
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              ParentChild
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
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship processstage_account")]
                public static partial class processstage_account
                {
                    public const string Name = "processstage_account";

                    public const string ReferencedEntity_processstage = "processstage";

                    public const string ReferencedAttribute_processstageid = "processstageid";

                    public const string ReferencedEntity_PrimaryNameAttribute_stagename = "stagename";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_stageid = "stageid";
                }

                ///<summary>
                /// N:1 - Relationship service_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     service_accounts
                /// ReferencingEntityNavigationPropertyName    preferredserviceid
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
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
                /// ReferencedEntity service:    PrimaryIdAttribute serviceid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Service
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Services
                ///     
                ///     Description:
                ///         (English - United States - 1033): Activity that represents work done to satisfy a customer's need.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship service_accounts")]
                public static partial class service_accounts
                {
                    public const string Name = "service_accounts";

                    public const string ReferencedEntity_service = "service";

                    public const string ReferencedAttribute_serviceid = "serviceid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_preferredserviceid = "preferredserviceid";
                }

                ///<summary>
                /// N:1 - Relationship sla_account
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     sla_account
                /// ReferencingEntityNavigationPropertyName    slainvokedid_account_sla
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
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
                /// ReferencedEntity sla:    PrimaryIdAttribute slaid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): SLA
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): SLAs
                ///     
                ///     Description:
                ///         (English - United States - 1033): Contains information about the tracked service-level KPIs for cases that belong to different customers.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship sla_account")]
                public static partial class sla_account
                {
                    public const string Name = "sla_account";

                    public const string ReferencedEntity_sla = "sla";

                    public const string ReferencedAttribute_slaid = "slaid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_slainvokedid = "slainvokedid";
                }

                ///<summary>
                /// N:1 - Relationship system_user_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     system_user_accounts
                /// ReferencingEntityNavigationPropertyName    preferredsystemuserid
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
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship system_user_accounts")]
                public static partial class system_user_accounts
                {
                    public const string Name = "system_user_accounts";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_preferredsystemuserid = "preferredsystemuserid";
                }

                ///<summary>
                /// N:1 - Relationship team_accounts
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
                /// ReferencedEntity team:    PrimaryIdAttribute teamid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Team
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Teams
                ///     
                ///     Description:
                ///         (English - United States - 1033): Collection of system users that routinely collaborate. Teams can be used to simplify record sharing and provide team members with common access to organization data when team members belong to different Business Units.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship team_accounts")]
                public static partial class team_accounts
                {
                    public const string Name = "team_accounts";

                    public const string ReferencedEntity_team = "team";

                    public const string ReferencedAttribute_teamid = "teamid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_owningteam = "owningteam";
                }

                ///<summary>
                /// N:1 - Relationship territory_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     territory_accounts
                /// ReferencingEntityNavigationPropertyName    territoryid
                /// IsCustomizable                             True
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
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity territory:    PrimaryIdAttribute territoryid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Territory
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Territories
                ///     
                ///     Description:
                ///         (English - United States - 1033): Territory represents sales regions.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship territory_accounts")]
                public static partial class territory_accounts
                {
                    public const string Name = "territory_accounts";

                    public const string ReferencedEntity_territory = "territory";

                    public const string ReferencedAttribute_territoryid = "territoryid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_territoryid = "territoryid";
                }

                ///<summary>
                /// N:1 - Relationship transactioncurrency_account
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     transactioncurrency_account
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
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship transactioncurrency_account")]
                public static partial class transactioncurrency_account
                {
                    public const string Name = "transactioncurrency_account";

                    public const string ReferencedEntity_transactioncurrency = "transactioncurrency";

                    public const string ReferencedAttribute_transactioncurrencyid = "transactioncurrencyid";

                    public const string ReferencedEntity_PrimaryNameAttribute_currencyname = "currencyname";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_transactioncurrencyid = "transactioncurrencyid";
                }

                ///<summary>
                /// N:1 - Relationship user_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     user_accounts
                /// ReferencingEntityNavigationPropertyName    owninguser
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
                [System.ComponentModel.DescriptionAttribute("N:1 - Relationship user_accounts")]
                public static partial class user_accounts
                {
                    public const string Name = "user_accounts";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_owninguser = "owninguser";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship account_actioncard
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_actioncard
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account_actioncard
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                Cascade
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
                /// ReferencingEntity actioncard:    PrimaryIdAttribute actioncardid    PrimaryNameAttribute title
                ///     DisplayName:
                ///         (English - United States - 1033): Action Card
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Action Cards
                ///     
                ///     Description:
                ///         (English - United States - 1033): Action card entity to show action cards.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_actioncard")]
                public static partial class account_actioncard
                {
                    public const string Name = "account_actioncard";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_actioncard = "actioncard";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship account_activity_parties
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_activity_parties
                /// ReferencingEntityNavigationPropertyName    partyid_account
                /// IsCustomizable                             False
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
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity activityparty:    PrimaryIdAttribute activitypartyid    PrimaryNameAttribute partyidname
                ///     DisplayName:
                ///         (English - United States - 1033): Activity Party
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Activity Parties
                ///     
                ///     Description:
                ///         (English - United States - 1033): Person or group associated with an activity. An activity can have multiple activity parties.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_activity_parties")]
                public static partial class account_activity_parties
                {
                    public const string Name = "account_activity_parties";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_activityparty = "activityparty";

                    public const string ReferencingAttribute_partyid = "partyid";

                    public const string ReferencingEntity_PrimaryNameAttribute_partyidname = "partyidname";
                }

                ///<summary>
                /// 1:N - Relationship Account_ActivityPointers
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_ActivityPointers
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                RemoveLink
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          20
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_ActivityPointers")]
                public static partial class account_activitypointers
                {
                    public const string Name = "Account_ActivityPointers";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_activitypointer = "activitypointer";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship Account_Annotation
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_Annotation
                /// ReferencingEntityNavigationPropertyName    objectid_account
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_Annotation")]
                public static partial class account_annotation
                {
                    public const string Name = "Account_Annotation";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_annotation = "annotation";

                    public const string ReferencingAttribute_objectid = "objectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship Account_Appointments
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_Appointments
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account_appointment
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_Appointments")]
                public static partial class account_appointments
                {
                    public const string Name = "Account_Appointments";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_appointment = "appointment";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship Account_AsyncOperations
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_AsyncOperations
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_AsyncOperations")]
                public static partial class account_asyncoperations
                {
                    public const string Name = "Account_AsyncOperations";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_asyncoperation = "asyncoperation";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship account_bookableresource_AccountId
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_bookableresource_AccountId
                /// ReferencingEntityNavigationPropertyName    AccountId
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                RemoveLink
                /// CascadeConfiguration.Merge                 Cascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          10000
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_bookableresource_AccountId")]
                public static partial class account_bookableresource_accountid
                {
                    public const string Name = "account_bookableresource_AccountId";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_bookableresource = "bookableresource";

                    public const string ReferencingAttribute_accountid = "accountid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship Account_BulkDeleteFailures
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_BulkDeleteFailures
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_BulkDeleteFailures")]
                public static partial class account_bulkdeletefailures
                {
                    public const string Name = "Account_BulkDeleteFailures";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_bulkdeletefailure = "bulkdeletefailure";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";
                }

                ///<summary>
                /// 1:N - Relationship account_BulkOperations
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_BulkOperations
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account_bulkoperation
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_BulkOperations")]
                public static partial class account_bulkoperations
                {
                    public const string Name = "account_BulkOperations";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_bulkoperation = "bulkoperation";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship account_CampaignResponses
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_CampaignResponses
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account_campaignresponse
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_CampaignResponses")]
                public static partial class account_campaignresponses
                {
                    public const string Name = "account_CampaignResponses";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_campaignresponse = "campaignresponse";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship account_connections1
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_connections1
                /// ReferencingEntityNavigationPropertyName    record1id_account
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 Cascade
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_connections1")]
                public static partial class account_connections1
                {
                    public const string Name = "account_connections1";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_connection = "connection";

                    public const string ReferencingAttribute_record1id = "record1id";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship account_connections2
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_connections2
                /// ReferencingEntityNavigationPropertyName    record2id_account
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 Cascade
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_connections2")]
                public static partial class account_connections2
                {
                    public const string Name = "account_connections2";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_connection = "connection";

                    public const string ReferencingAttribute_record2id = "record2id";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship account_customer_opportunity_roles
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_customer_opportunity_roles
                /// ReferencingEntityNavigationPropertyName    customerid_account
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 Cascade
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
                /// 
                /// AttributeMaps:
                ///     SourceEntity             TargetEntity
                ///     account            ->    customeropportunityrole
                ///     
                ///     SourceAttribute          TargetAttribute
                ///     accountid          ->    customerid
                ///     name               ->    customeridname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_customer_opportunity_roles")]
                public static partial class account_customer_opportunity_roles
                {
                    public const string Name = "account_customer_opportunity_roles";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_customeropportunityrole = "customeropportunityrole";

                    public const string ReferencingAttribute_customerid = "customerid";

                    public const string ReferencingEntity_PrimaryNameAttribute_opportunityroleidname = "opportunityroleidname";
                }

                ///<summary>
                /// 1:N - Relationship account_customer_relationship_customer
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_customer_relationship_customer
                /// ReferencingEntityNavigationPropertyName    customerid_account
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 Cascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          60
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
                /// 
                /// AttributeMaps:
                ///     SourceEntity             TargetEntity
                ///     account            ->    customerrelationship
                ///     
                ///     SourceAttribute          TargetAttribute
                ///     accountid          ->    customerid
                ///     name               ->    customeridname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_customer_relationship_customer")]
                public static partial class account_customer_relationship_customer
                {
                    public const string Name = "account_customer_relationship_customer";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_customerrelationship = "customerrelationship";

                    public const string ReferencingAttribute_customerid = "customerid";

                    public const string ReferencingEntity_PrimaryNameAttribute_customerroleidname = "customerroleidname";
                }

                ///<summary>
                /// 1:N - Relationship account_customer_relationship_partner
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_customer_relationship_partner
                /// ReferencingEntityNavigationPropertyName    partnerid_account
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 Cascade
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
                /// 
                /// AttributeMaps:
                ///     SourceEntity             TargetEntity
                ///     account            ->    customerrelationship
                ///     
                ///     SourceAttribute          TargetAttribute
                ///     accountid          ->    customerid
                ///     name               ->    customeridname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_customer_relationship_partner")]
                public static partial class account_customer_relationship_partner
                {
                    public const string Name = "account_customer_relationship_partner";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_customerrelationship = "customerrelationship";

                    public const string ReferencingAttribute_partnerid = "partnerid";

                    public const string ReferencingEntity_PrimaryNameAttribute_customerroleidname = "customerroleidname";
                }

                ///<summary>
                /// 1:N - Relationship Account_CustomerAddress
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_CustomerAddress
                /// ReferencingEntityNavigationPropertyName    parentid_account
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 Cascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          10
                /// 
                /// ReferencingEntity customeraddress:    PrimaryIdAttribute customeraddressid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Address
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Addresses
                ///     
                ///     Description:
                ///         (English - United States - 1033): Address and shipping information. Used to store additional addresses for an account or contact.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_CustomerAddress")]
                public static partial class account_customeraddress
                {
                    public const string Name = "Account_CustomerAddress";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_customeraddress = "customeraddress";

                    public const string ReferencingAttribute_parentid = "parentid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship Account_DuplicateBaseRecord
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_DuplicateBaseRecord
                /// ReferencingEntityNavigationPropertyName    baserecordid_account
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_DuplicateBaseRecord")]
                public static partial class account_duplicatebaserecord
                {
                    public const string Name = "Account_DuplicateBaseRecord";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_duplicaterecord = "duplicaterecord";

                    public const string ReferencingAttribute_baserecordid = "baserecordid";
                }

                ///<summary>
                /// 1:N - Relationship Account_DuplicateMatchingRecord
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_DuplicateMatchingRecord
                /// ReferencingEntityNavigationPropertyName    duplicaterecordid_account
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_DuplicateMatchingRecord")]
                public static partial class account_duplicatematchingrecord
                {
                    public const string Name = "Account_DuplicateMatchingRecord";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_duplicaterecord = "duplicaterecord";

                    public const string ReferencingAttribute_duplicaterecordid = "duplicaterecordid";
                }

                ///<summary>
                /// 1:N - Relationship Account_Email_EmailSender
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_Email_EmailSender
                /// ReferencingEntityNavigationPropertyName    emailsender_account
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_Email_EmailSender")]
                public static partial class account_email_emailsender
                {
                    public const string Name = "Account_Email_EmailSender";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_email = "email";

                    public const string ReferencingAttribute_emailsender = "emailsender";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship Account_Email_SendersAccount
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_Email_SendersAccount
                /// ReferencingEntityNavigationPropertyName    sendersaccount
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_Email_SendersAccount")]
                public static partial class account_email_sendersaccount
                {
                    public const string Name = "Account_Email_SendersAccount";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_email = "email";

                    public const string ReferencingAttribute_sendersaccount = "sendersaccount";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship Account_Emails
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_Emails
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account_email
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_Emails")]
                public static partial class account_emails
                {
                    public const string Name = "Account_Emails";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_email = "email";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship account_entitlement_Account
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_entitlement_Account
                /// ReferencingEntityNavigationPropertyName    accountid
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                RemoveLink
                /// CascadeConfiguration.Merge                 Cascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          10000
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
                /// 
                /// AttributeMaps:
                ///     SourceEntity             TargetEntity
                ///     account            ->    entitlement
                ///     
                ///     SourceAttribute          TargetAttribute
                ///     accountid          ->    customerid
                ///     name               ->    customeridname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_entitlement_Account")]
                public static partial class account_entitlement_account
                {
                    public const string Name = "account_entitlement_Account";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_entitlement = "entitlement";

                    public const string ReferencingAttribute_accountid = "accountid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship account_entitlement_Customer
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_entitlement_Customer
                /// ReferencingEntityNavigationPropertyName    customerid_account
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Restrict
                /// CascadeConfiguration.Merge                 Cascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          10000
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
                /// 
                /// AttributeMaps:
                ///     SourceEntity             TargetEntity
                ///     account            ->    entitlement
                ///     
                ///     SourceAttribute          TargetAttribute
                ///     accountid          ->    customerid
                ///     name               ->    customeridname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_entitlement_Customer")]
                public static partial class account_entitlement_customer
                {
                    public const string Name = "account_entitlement_Customer";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_entitlement = "entitlement";

                    public const string ReferencingAttribute_customerid = "customerid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship Account_Faxes
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_Faxes
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account_fax
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_Faxes")]
                public static partial class account_faxes
                {
                    public const string Name = "Account_Faxes";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_fax = "fax";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship account_IncidentResolutions
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_IncidentResolutions
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account_incidentresolution
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_IncidentResolutions")]
                public static partial class account_incidentresolutions
                {
                    public const string Name = "account_IncidentResolutions";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_incidentresolution = "incidentresolution";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship Account_Letters
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_Letters
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account_letter
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_Letters")]
                public static partial class account_letters
                {
                    public const string Name = "Account_Letters";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_letter = "letter";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship Account_MailboxTrackingFolder
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_MailboxTrackingFolder
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_MailboxTrackingFolder")]
                public static partial class account_mailboxtrackingfolder
                {
                    public const string Name = "Account_MailboxTrackingFolder";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_mailboxtrackingfolder = "mailboxtrackingfolder";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";
                }

                ///<summary>
                /// 1:N - Relationship account_master_account
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_master_account
                /// ReferencingEntityNavigationPropertyName    masterid
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
                /// AttributeMaps:
                ///     SourceEntity                       TargetEntity
                ///     account                      ->    account
                ///     
                ///     SourceAttribute                    TargetAttribute
                ///     accountid                    ->    parentaccountid
                ///     defaultpricelevelid          ->    defaultpricelevelid
                ///     defaultpricelevelidname      ->    defaultpricelevelidname
                ///     name                         ->    parentaccountidname
                ///     transactioncurrencyid        ->    transactioncurrencyid
                ///     transactioncurrencyiddsc     ->    transactioncurrencyiddsc
                ///     transactioncurrencyidname    ->    transactioncurrencyidname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_master_account")]
                public static partial class account_master_account
                {
                    public const string Name = "account_master_account";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_masterid = "masterid";
                }

                ///<summary>
                /// 1:N - Relationship account_OpportunityCloses
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_OpportunityCloses
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account_opportunityclose
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_OpportunityCloses")]
                public static partial class account_opportunitycloses
                {
                    public const string Name = "account_OpportunityCloses";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_opportunityclose = "opportunityclose";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship account_OrderCloses
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_OrderCloses
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account_orderclose
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_OrderCloses")]
                public static partial class account_ordercloses
                {
                    public const string Name = "account_OrderCloses";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_orderclose = "orderclose";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship account_parent_account
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             True
                /// ReferencedEntityNavigationPropertyName     account_parent_account
                /// ReferencingEntityNavigationPropertyName    parentaccountid
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                RemoveLink
                /// CascadeConfiguration.Merge                 Cascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          40
                /// 
                /// AttributeMaps:
                ///     SourceEntity                       TargetEntity
                ///     account                      ->    account
                ///     
                ///     SourceAttribute                    TargetAttribute
                ///     accountid                    ->    parentaccountid
                ///     defaultpricelevelid          ->    defaultpricelevelid
                ///     defaultpricelevelidname      ->    defaultpricelevelidname
                ///     name                         ->    parentaccountidname
                ///     transactioncurrencyid        ->    transactioncurrencyid
                ///     transactioncurrencyiddsc     ->    transactioncurrencyiddsc
                ///     transactioncurrencyidname    ->    transactioncurrencyidname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_parent_account")]
                public static partial class account_parent_account
                {
                    public const string Name = "account_parent_account";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_parentaccountid = "parentaccountid";
                }

                ///<summary>
                /// 1:N - Relationship Account_Phonecalls
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_Phonecalls
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account_phonecall
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_Phonecalls")]
                public static partial class account_phonecalls
                {
                    public const string Name = "Account_Phonecalls";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_phonecall = "phonecall";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship account_PostFollows
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_PostFollows
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_PostFollows")]
                public static partial class account_postfollows
                {
                    public const string Name = "account_PostFollows";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_postfollow = "postfollow";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_regardingobjectidname = "regardingobjectidname";
                }

                ///<summary>
                /// 1:N - Relationship account_PostRegardings
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_PostRegardings
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account
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
                /// ReferencingEntity postregarding:    PrimaryIdAttribute postregardingid
                ///     DisplayName:
                ///         (English - United States - 1033): Post Regarding
                ///     
                ///     Description:
                ///         (English - United States - 1033): Represents which object an activity feed post is regarding. For internal use only.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_PostRegardings")]
                public static partial class account_postregardings
                {
                    public const string Name = "account_PostRegardings";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_postregarding = "postregarding";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";
                }

                ///<summary>
                /// 1:N - Relationship account_PostRoles
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_PostRoles
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_PostRoles")]
                public static partial class account_postroles
                {
                    public const string Name = "account_PostRoles";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_postrole = "postrole";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";
                }

                ///<summary>
                /// 1:N - Relationship account_principalobjectattributeaccess
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_principalobjectattributeaccess
                /// ReferencingEntityNavigationPropertyName    objectid_account
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_principalobjectattributeaccess")]
                public static partial class account_principalobjectattributeaccess
                {
                    public const string Name = "account_principalobjectattributeaccess";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_principalobjectattributeaccess = "principalobjectattributeaccess";

                    public const string ReferencingAttribute_objectid = "objectid";
                }

                ///<summary>
                /// 1:N - Relationship Account_ProcessSessions
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_ProcessSessions
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_ProcessSessions")]
                public static partial class account_processsessions
                {
                    public const string Name = "Account_ProcessSessions";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_processsession = "processsession";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship account_QuoteCloses
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     account_QuoteCloses
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account_quoteclose
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship account_QuoteCloses")]
                public static partial class account_quotecloses
                {
                    public const string Name = "account_QuoteCloses";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_quoteclose = "quoteclose";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship Account_RecurringAppointmentMasters
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_RecurringAppointmentMasters
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account_recurringappointmentmaster
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_RecurringAppointmentMasters")]
                public static partial class account_recurringappointmentmasters
                {
                    public const string Name = "Account_RecurringAppointmentMasters";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_recurringappointmentmaster = "recurringappointmentmaster";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship Account_ServiceAppointments
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_ServiceAppointments
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account_serviceappointment
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                RemoveLink
                /// CascadeConfiguration.Merge                 Cascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_ServiceAppointments")]
                public static partial class account_serviceappointments
                {
                    public const string Name = "Account_ServiceAppointments";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_serviceappointment = "serviceappointment";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship Account_SharepointDocument
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_SharepointDocument
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 Cascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          60
                /// 
                /// ReferencingEntity sharepointdocument:    PrimaryIdAttribute sharepointdocumentid    PrimaryNameAttribute fullname
                ///     DisplayName:
                ///         (English - United States - 1033): Sharepoint Document
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Documents
                ///     
                ///     Description:
                ///         (English - United States - 1033): Document libraries or folders on a SharePoint server from where documents can be managed in Microsoft Dynamics 365.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_SharepointDocument")]
                public static partial class account_sharepointdocument
                {
                    public const string Name = "Account_SharepointDocument";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_sharepointdocument = "sharepointdocument";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// 1:N - Relationship Account_SharepointDocumentLocation
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_SharepointDocumentLocation
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 Cascade
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_SharepointDocumentLocation")]
                public static partial class account_sharepointdocumentlocation
                {
                    public const string Name = "Account_SharepointDocumentLocation";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_sharepointdocumentlocation = "sharepointdocumentlocation";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship Account_SocialActivities
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_SocialActivities
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account_socialactivity
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_SocialActivities")]
                public static partial class account_socialactivities
                {
                    public const string Name = "Account_SocialActivities";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_socialactivity = "socialactivity";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship Account_SyncErrors
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_SyncErrors
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account_syncerror
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_SyncErrors")]
                public static partial class account_syncerrors
                {
                    public const string Name = "Account_SyncErrors";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_syncerror = "syncerror";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship Account_Tasks
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Account_Tasks
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account_task
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Account_Tasks")]
                public static partial class account_tasks
                {
                    public const string Name = "Account_Tasks";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_task = "task";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship contact_customer_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     contact_customer_accounts
                /// ReferencingEntityNavigationPropertyName    parentcustomerid_account
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          50
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
                /// 
                /// AttributeMaps:
                ///     SourceEntity                         TargetEntity
                ///     account                        ->    contact
                ///     
                ///     SourceAttribute                      TargetAttribute
                ///     accountid                      ->    parentcustomerid
                ///     address1_addresstypecode       ->    address1_addresstypecode
                ///     address1_city                  ->    address1_city
                ///     address1_country               ->    address1_country
                ///     address1_county                ->    address1_county
                ///     address1_freighttermscode      ->    address1_freighttermscode
                ///     address1_line1                 ->    address1_line1
                ///     address1_line2                 ->    address1_line2
                ///     address1_line3                 ->    address1_line3
                ///     address1_name                  ->    address1_name
                ///     address1_postalcode            ->    address1_postalcode
                ///     address1_postofficebox         ->    address1_postofficebox
                ///     address1_shippingmethodcode    ->    address1_shippingmethodcode
                ///     address1_stateorprovince       ->    address1_stateorprovince
                ///     address1_telephone1            ->    address1_telephone1
                ///     defaultpricelevelid            ->    defaultpricelevelid
                ///     defaultpricelevelidname        ->    defaultpricelevelidname
                ///     name                           ->    parentcustomeridname
                ///     paymenttermscode               ->    paymenttermscode
                ///     telephone1                     ->    telephone1
                ///     transactioncurrencyid          ->    transactioncurrencyid
                ///     transactioncurrencyiddsc       ->    transactioncurrencyiddsc
                ///     transactioncurrencyidname      ->    transactioncurrencyidname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship contact_customer_accounts")]
                public static partial class contact_customer_accounts
                {
                    public const string Name = "contact_customer_accounts";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_contact = "contact";

                    public const string ReferencingAttribute_parentcustomerid = "parentcustomerid";

                    public const string ReferencingEntity_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// 1:N - Relationship contract_billingcustomer_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     contract_billingcustomer_accounts
                /// ReferencingEntityNavigationPropertyName    billingcustomerid_account
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Restrict
                /// CascadeConfiguration.Merge                 Cascade
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
                /// 
                /// AttributeMaps:
                ///     SourceEntity                       TargetEntity
                ///     account                      ->    contract
                ///     
                ///     SourceAttribute                    TargetAttribute
                ///     accountid                    ->    customerid
                ///     name                         ->    customeridname
                ///     transactioncurrencyid        ->    transactioncurrencyid
                ///     transactioncurrencyidname    ->    transactioncurrencyidname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship contract_billingcustomer_accounts")]
                public static partial class contract_billingcustomer_accounts
                {
                    public const string Name = "contract_billingcustomer_accounts";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_contract = "contract";

                    public const string ReferencingAttribute_billingcustomerid = "billingcustomerid";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship contract_customer_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     contract_customer_accounts
                /// ReferencingEntityNavigationPropertyName    customerid_account
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Service
                /// AssociatedMenuConfiguration.Order          20
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
                /// 
                /// AttributeMaps:
                ///     SourceEntity                       TargetEntity
                ///     account                      ->    contract
                ///     
                ///     SourceAttribute                    TargetAttribute
                ///     accountid                    ->    customerid
                ///     name                         ->    customeridname
                ///     transactioncurrencyid        ->    transactioncurrencyid
                ///     transactioncurrencyidname    ->    transactioncurrencyidname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship contract_customer_accounts")]
                public static partial class contract_customer_accounts
                {
                    public const string Name = "contract_customer_accounts";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_contract = "contract";

                    public const string ReferencingAttribute_customerid = "customerid";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship contractlineitem_customer_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     contractlineitem_customer_accounts
                /// ReferencingEntityNavigationPropertyName    customerid_account
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                Cascade
                /// CascadeConfiguration.Merge                 Cascade
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship contractlineitem_customer_accounts")]
                public static partial class contractlineitem_customer_accounts
                {
                    public const string Name = "contractlineitem_customer_accounts";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_contractdetail = "contractdetail";

                    public const string ReferencingAttribute_customerid = "customerid";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship CreatedAccount_BulkOperationLogs2
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     CreatedAccount_BulkOperationLogs2
                /// ReferencingEntityNavigationPropertyName    createdobjectid_account
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship CreatedAccount_BulkOperationLogs2")]
                public static partial class createdaccount_bulkoperationlogs2
                {
                    public const string Name = "CreatedAccount_BulkOperationLogs2";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_bulkoperationlog = "bulkoperationlog";

                    public const string ReferencingAttribute_createdobjectid = "createdobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship incident_customer_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     incident_customer_accounts
                /// ReferencingEntityNavigationPropertyName    customerid_account
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Service
                /// AssociatedMenuConfiguration.Order          10
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
                /// 
                /// AttributeMaps:
                ///     SourceEntity             TargetEntity
                ///     account            ->    incident
                ///     
                ///     SourceAttribute          TargetAttribute
                ///     accountid          ->    customerid
                ///     name               ->    customeridname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship incident_customer_accounts")]
                public static partial class incident_customer_accounts
                {
                    public const string Name = "incident_customer_accounts";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_incident = "incident";

                    public const string ReferencingAttribute_customerid = "customerid";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship invoice_customer_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     invoice_customer_accounts
                /// ReferencingEntityNavigationPropertyName    customerid_account
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                Restrict
                /// CascadeConfiguration.Merge                 Cascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Sales
                /// AssociatedMenuConfiguration.Order          40
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
                /// 
                /// AttributeMaps:
                ///     SourceEntity                         TargetEntity
                ///     account                        ->    invoice
                ///     
                ///     SourceAttribute                      TargetAttribute
                ///     accountid                      ->    customerid
                ///     address1_shippingmethodcode    ->    shippingmethodcode
                ///     defaultpricelevelid            ->    pricelevelid
                ///     defaultpricelevelidname        ->    pricelevelidname
                ///     name                           ->    customeridname
                ///     paymenttermscode               ->    paymenttermscode
                ///     transactioncurrencyid          ->    transactioncurrencyid
                ///     transactioncurrencyidname      ->    transactioncurrencyidname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship invoice_customer_accounts")]
                public static partial class invoice_customer_accounts
                {
                    public const string Name = "invoice_customer_accounts";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_invoice = "invoice";

                    public const string ReferencingAttribute_customerid = "customerid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship lead_customer_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lead_customer_accounts
                /// ReferencingEntityNavigationPropertyName    customerid_account
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship lead_customer_accounts")]
                public static partial class lead_customer_accounts
                {
                    public const string Name = "lead_customer_accounts";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_lead = "lead";

                    public const string ReferencingAttribute_customerid = "customerid";

                    public const string ReferencingEntity_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// 1:N - Relationship lead_parent_account
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lead_parent_account
                /// ReferencingEntityNavigationPropertyName    parentaccountid
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                RemoveLink
                /// CascadeConfiguration.Merge                 Cascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Sales
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship lead_parent_account")]
                public static partial class lead_parent_account
                {
                    public const string Name = "lead_parent_account";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_lead = "lead";

                    public const string ReferencingAttribute_parentaccountid = "parentaccountid";

                    public const string ReferencingEntity_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// 1:N - Relationship opportunity_customer_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     opportunity_customer_accounts
                /// ReferencingEntityNavigationPropertyName    customerid_account
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Sales
                /// AssociatedMenuConfiguration.Order          10
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
                /// 
                /// AttributeMaps:
                ///     SourceEntity                       TargetEntity
                ///     account                      ->    opportunity
                ///     
                ///     SourceAttribute                    TargetAttribute
                ///     accountid                    ->    customerid
                ///     accountid                    ->    parentaccountid
                ///     defaultpricelevelid          ->    pricelevelid
                ///     defaultpricelevelidname      ->    pricelevelidname
                ///     name                         ->    customeridname
                ///     name                         ->    parentaccountidname
                ///     transactioncurrencyid        ->    transactioncurrencyid
                ///     transactioncurrencyidname    ->    transactioncurrencyidname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship opportunity_customer_accounts")]
                public static partial class opportunity_customer_accounts
                {
                    public const string Name = "opportunity_customer_accounts";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_opportunity = "opportunity";

                    public const string ReferencingAttribute_customerid = "customerid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship opportunity_parent_account
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     opportunity_parent_account
                /// ReferencingEntityNavigationPropertyName    parentaccountid
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                NoCascade
                /// CascadeConfiguration.Delete                RemoveLink
                /// CascadeConfiguration.Merge                 Cascade
                /// CascadeConfiguration.Reparent              NoCascade
                /// CascadeConfiguration.Share                 NoCascade
                /// CascadeConfiguration.Unshare               NoCascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Sales
                /// AssociatedMenuConfiguration.Order          1
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
                /// 
                /// AttributeMaps:
                ///     SourceEntity                       TargetEntity
                ///     account                      ->    opportunity
                ///     
                ///     SourceAttribute                    TargetAttribute
                ///     accountid                    ->    customerid
                ///     accountid                    ->    parentaccountid
                ///     defaultpricelevelid          ->    pricelevelid
                ///     defaultpricelevelidname      ->    pricelevelidname
                ///     name                         ->    customeridname
                ///     name                         ->    parentaccountidname
                ///     transactioncurrencyid        ->    transactioncurrencyid
                ///     transactioncurrencyidname    ->    transactioncurrencyidname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship opportunity_parent_account")]
                public static partial class opportunity_parent_account
                {
                    public const string Name = "opportunity_parent_account";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_opportunity = "opportunity";

                    public const string ReferencingAttribute_parentaccountid = "parentaccountid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship order_customer_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     order_customer_accounts
                /// ReferencingEntityNavigationPropertyName    customerid_account
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                Restrict
                /// CascadeConfiguration.Merge                 Cascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// CascadeConfiguration.RollupView            NoCascade
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Sales
                /// AssociatedMenuConfiguration.Order          30
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
                /// 
                /// AttributeMaps:
                ///     SourceEntity                         TargetEntity
                ///     account                        ->    salesorder
                ///     
                ///     SourceAttribute                      TargetAttribute
                ///     accountid                      ->    customerid
                ///     address1_freighttermscode      ->    freighttermscode
                ///     address1_shippingmethodcode    ->    shippingmethodcode
                ///     defaultpricelevelid            ->    pricelevelid
                ///     defaultpricelevelidname        ->    pricelevelidname
                ///     name                           ->    customeridname
                ///     paymenttermscode               ->    paymenttermscode
                ///     transactioncurrencyid          ->    transactioncurrencyid
                ///     transactioncurrencyidname      ->    transactioncurrencyidname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship order_customer_accounts")]
                public static partial class order_customer_accounts
                {
                    public const string Name = "order_customer_accounts";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_salesorder = "salesorder";

                    public const string ReferencingAttribute_customerid = "customerid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship quote_customer_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     quote_customer_accounts
                /// ReferencingEntityNavigationPropertyName    customerid_account
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Sales
                /// AssociatedMenuConfiguration.Order          20
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
                /// 
                /// AttributeMaps:
                ///     SourceEntity                         TargetEntity
                ///     account                        ->    quote
                ///     
                ///     SourceAttribute                      TargetAttribute
                ///     accountid                      ->    customerid
                ///     address1_freighttermscode      ->    freighttermscode
                ///     address1_shippingmethodcode    ->    shippingmethodcode
                ///     defaultpricelevelid            ->    pricelevelid
                ///     defaultpricelevelidname        ->    pricelevelidname
                ///     name                           ->    customeridname
                ///     paymenttermscode               ->    paymenttermscode
                ///     transactioncurrencyid          ->    transactioncurrencyid
                ///     transactioncurrencyidname      ->    transactioncurrencyidname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship quote_customer_accounts")]
                public static partial class quote_customer_accounts
                {
                    public const string Name = "quote_customer_accounts";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_quote = "quote";

                    public const string ReferencingAttribute_customerid = "customerid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship slakpiinstance_account
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     slakpiinstance_account
                /// ReferencingEntityNavigationPropertyName    regarding_account
                /// IsCustomizable                             False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
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
                /// ReferencingEntity slakpiinstance:    PrimaryIdAttribute slakpiinstanceid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): SLA KPI Instance
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): SLA KPI Instances
                ///     
                ///     Description:
                ///         (English - United States - 1033): Service level agreement (SLA) key performance indicator (KPI) instance that is tracked for an individual case
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship slakpiinstance_account")]
                public static partial class slakpiinstance_account
                {
                    public const string Name = "slakpiinstance_account";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_slakpiinstance = "slakpiinstance";

                    public const string ReferencingAttribute_regarding = "regarding";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship SocialActivity_PostAuthor_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     SocialActivity_PostAuthor_accounts
                /// ReferencingEntityNavigationPropertyName    postauthor_account
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship SocialActivity_PostAuthor_accounts")]
                public static partial class socialactivity_postauthor_accounts
                {
                    public const string Name = "SocialActivity_PostAuthor_accounts";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_socialactivity = "socialactivity";

                    public const string ReferencingAttribute_postauthor = "postauthor";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship SocialActivity_PostAuthorAccount_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     SocialActivity_PostAuthorAccount_accounts
                /// ReferencingEntityNavigationPropertyName    postauthoraccount_account
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship SocialActivity_PostAuthorAccount_accounts")]
                public static partial class socialactivity_postauthoraccount_accounts
                {
                    public const string Name = "SocialActivity_PostAuthorAccount_accounts";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_socialactivity = "socialactivity";

                    public const string ReferencingAttribute_postauthoraccount = "postauthoraccount";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship Socialprofile_customer_accounts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Socialprofile_customer_accounts
                /// ReferencingEntityNavigationPropertyName    customerid_account
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          50
                /// 
                /// ReferencingEntity socialprofile:    PrimaryIdAttribute socialprofileid    PrimaryNameAttribute profilename
                ///     DisplayName:
                ///         (English - United States - 1033): Social Profile
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Social Profiles
                ///     
                ///     Description:
                ///         (English - United States - 1033): This entity is used to store social profile information of its associated account and contacts on different social channels.
                /// 
                /// AttributeMaps:
                ///     SourceEntity             TargetEntity
                ///     account            ->    socialprofile
                ///     
                ///     SourceAttribute          TargetAttribute
                ///     accountid          ->    customerid
                ///     name               ->    customeridname
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship Socialprofile_customer_accounts")]
                public static partial class socialprofile_customer_accounts
                {
                    public const string Name = "Socialprofile_customer_accounts";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_socialprofile = "socialprofile";

                    public const string ReferencingAttribute_customerid = "customerid";

                    public const string ReferencingEntity_PrimaryNameAttribute_profilename = "profilename";
                }

                ///<summary>
                /// 1:N - Relationship SourceAccount_BulkOperationLogs
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     SourceAccount_BulkOperationLogs
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_account
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship SourceAccount_BulkOperationLogs")]
                public static partial class sourceaccount_bulkoperationlogs
                {
                    public const string Name = "SourceAccount_BulkOperationLogs";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_bulkoperationlog = "bulkoperationlog";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_account
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_account
                /// ReferencingEntityNavigationPropertyName    objectid_account
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
                [System.ComponentModel.DescriptionAttribute("1:N - Relationship userentityinstancedata_account")]
                public static partial class userentityinstancedata_account
                {
                    public const string Name = "userentityinstancedata_account";

                    public const string ReferencedEntity_account = "account";

                    public const string ReferencedAttribute_accountid = "accountid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.

            #region Relationship ManyToMany - N:N.

            public static partial class ManyToMany
            {
                ///<summary>
                /// N:N - Relationship accountleads_association
                /// 
                /// PropertyName                                   Value
                /// Entity1NavigationPropertyName                  accountleads_association
                /// Entity2NavigationPropertyName                  accountleads_association
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
                /// Entity2LogicalName lead:    PrimaryIdAttribute leadid    PrimaryNameAttribute fullname
                ///     DisplayName:
                ///         (English - United States - 1033): Lead
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Leads
                ///     
                ///     Description:
                ///         (English - United States - 1033): Prospect or potential sales opportunity. Leads are converted into accounts, contacts, or opportunities when they are qualified. Otherwise, they are deleted or archived.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:N - Relationship accountleads_association")]
                public static partial class accountleads_association
                {
                    public const string Name = "accountleads_association";

                    public const string IntersectEntity_accountleads = "accountleads";

                    public const string Entity1_account = "account";

                    public const string Entity1Attribute_leadid = "leadid";

                    public const string Entity2_lead = "lead";

                    public const string Entity2Attribute_accountid = "accountid";

                    public const string Entity2LogicalName_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// N:N - Relationship listaccount_association
                /// 
                /// PropertyName                                   Value
                /// Entity1NavigationPropertyName                  listaccount_association
                /// Entity2NavigationPropertyName                  listaccount_association
                /// IsCustomizable                                 False
                /// IsCustomRelationship                           False
                /// IsValidForAdvancedFind                         True
                /// RelationshipType                               ManyToManyRelationship
                /// SecurityTypes                                  None
                /// Entity1AssociatedMenuConfiguration.Behavior    UseCollectionName
                /// Entity1AssociatedMenuConfiguration.Group       Marketing
                /// Entity1AssociatedMenuConfiguration.Order       10
                /// Entity2AssociatedMenuConfiguration.Behavior    DoNotDisplay
                /// Entity2AssociatedMenuConfiguration.Group       Details
                /// Entity2AssociatedMenuConfiguration.Order       null
                /// 
                /// Entity1LogicalName list:    PrimaryIdAttribute listid    PrimaryNameAttribute listname
                ///     DisplayName:
                ///         (English - United States - 1033): Marketing List
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Marketing Lists
                ///     
                ///     Description:
                ///         (English - United States - 1033): Group of existing or potential customers created for a marketing campaign or other sales purposes.
                ///</summary>
                [System.ComponentModel.DescriptionAttribute("N:N - Relationship listaccount_association")]
                public static partial class listaccount_association
                {
                    public const string Name = "listaccount_association";

                    public const string IntersectEntity_listmember = "listmember";

                    public const string Entity1_list = "list";

                    public const string Entity1Attribute_listid = "listid";

                    public const string Entity1LogicalName_PrimaryNameAttribute_listname = "listname";

                    public const string Entity2_account = "account";

                    public const string Entity2Attribute_entityid = "entityid";
                }
            }

            #endregion Relationship ManyToMany - N:N.
        }
    }
}