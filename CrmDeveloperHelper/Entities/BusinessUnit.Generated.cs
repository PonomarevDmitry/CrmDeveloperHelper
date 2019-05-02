
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    public partial class BusinessUnit
    {
        ///<summary>
        /// DisplayName:
        ///     (English - United States - 1033): Business Unit
        /// 
        /// DisplayCollectionName:
        ///     (English - United States - 1033): Business Units
        /// 
        /// Description:
        ///     (English - United States - 1033): Business, division, or department in the Microsoft Dynamics 365 database.
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
        /// CanCreateCharts                       False
        /// CanCreateForms                        True
        /// CanCreateViews                        True
        /// CanEnableSyncToExternalSearchIndex    True
        /// CanModifyAdditionalSettings           True
        /// CanTriggerWorkflow                    True
        /// ChangeTrackingEnabled                 True
        /// CollectionSchemaName                  BusinessUnits
        /// DataProviderId                        null
        /// DataSourceId                          null
        /// EnforceStateTransitions               False
        /// EntityHelpUrlEnabled                  False
        /// EntitySetName                         businessunits
        /// IntroducedVersion                     5.0.0.0
        /// IsAIRUpdated                          False
        /// IsActivity                            False
        /// IsActivityParty                       False
        /// IsAvailableOffline                    True
        /// IsBPFEntity                           False
        /// IsBusinessProcessEnabled              False
        /// IsChildEntity                         False
        /// IsConnectionsEnabled                  False
        /// IsCustomEntity                        False
        /// IsCustomizable                        True
        /// IsDocumentManagementEnabled           False
        /// IsDocumentRecommendationsEnabled      False
        /// IsDuplicateDetectionEnabled           False
        /// IsEnabledForCharts                    False
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
        /// IsReadOnlyInMobileClient              False
        /// IsReadingPaneEnabled                  True
        /// IsRenameable                          True
        /// IsSLAEnabled                          False
        /// IsStateModelAware                     False
        /// IsValidForAdvancedFind                True
        /// IsValidForQueue                       False
        /// IsVisibleInMobile                     False
        /// IsVisibleInMobileClient               True
        /// LogicalCollectionName                 businessunits
        /// LogicalName                           businessunit
        /// MobileOfflineFilters                  <fetch version="1.0" output-format="xml-platform" mapping="logical" distinct="false">
        /// MobileOfflineFilters                        <entity name="businessunit">
        /// MobileOfflineFilters                          <filter type="and">
        /// MobileOfflineFilters                            <condition attribute="modifiedon" operator="on-or-after" value="1900-01-01"/>
        /// MobileOfflineFilters                          </filter>
        /// MobileOfflineFilters                        </entity>
        /// MobileOfflineFilters                      </fetch>
        /// ObjectTypeCode                        10
        /// OwnershipType                         BusinessOwned
        /// PrimaryIdAttribute                    businessunitid
        /// PrimaryNameAttribute                  name
        /// ReportViewName                        FilteredBusinessUnit
        /// SchemaName                            BusinessUnit
        /// SyncToExternalSearchIndex             False
        /// UsesBusinessDataLabelTable            False
        ///</summary>
        public static partial class Schema
        {
            public const string EntityLogicalName = "businessunit";

            public const string EntitySchemaName = "BusinessUnit";

            public const string EntityPrimaryIdAttribute = "businessunitid";

            public const string EntityPrimaryNameAttribute = "name";

            public const int EntityObjectTypeCode = 10;

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Business Unit
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the business unit.
                /// 
                /// SchemaName: BusinessUnitId
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
                public const string businessunitid = "businessunitid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Name
                /// 
                /// Description:
                ///     (English - United States - 1033): Name of the business unit.
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
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string name = "name";

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
                public const string address1_addressid = "address1_addressid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Address Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of address for address 1, such as billing, shipping, or primary address.
                /// 
                /// SchemaName: Address1_AddressTypeCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet businessunit_address1_addresstypecode
                /// DefaultFormValue = 1
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
                ///     (English - United States - 1033): Bill To City
                /// 
                /// Description:
                ///     (English - United States - 1033): City name for address 1.
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
                public const string address1_city = "address1_city";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Bill To Country/Region
                /// 
                /// Description:
                ///     (English - United States - 1033): Country/region name for address 1.
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
                public const string address1_country = "address1_country";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: County
                /// 
                /// Description:
                ///     (English - United States - 1033): County name for address 1.
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
                public const string address1_county = "address1_county";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Fax
                /// 
                /// Description:
                ///     (English - United States - 1033): Fax number for address 1.
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
                public const string address1_fax = "address1_fax";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Latitude
                /// 
                /// Description:
                ///     (English - United States - 1033): Latitude for address 1.
                /// 
                /// SchemaName: Address1_Latitude
                /// DoubleAttributeMetadata    AttributeType: Double    AttributeTypeName: DoubleType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -90    MaxValue = 90    Precision = 5
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
                public const string address1_latitude = "address1_latitude";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Bill To Street 1
                /// 
                /// Description:
                ///     (English - United States - 1033): First line for entering address 1 information.
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
                public const string address1_line1 = "address1_line1";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Bill To Street 2
                /// 
                /// Description:
                ///     (English - United States - 1033): Second line for entering address 1 information.
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
                public const string address1_line2 = "address1_line2";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Bill To Street 3
                /// 
                /// Description:
                ///     (English - United States - 1033): Third line for entering address 1 information.
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
                public const string address1_line3 = "address1_line3";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Longitude
                /// 
                /// Description:
                ///     (English - United States - 1033): Longitude for address 1.
                /// 
                /// SchemaName: Address1_Longitude
                /// DoubleAttributeMetadata    AttributeType: Double    AttributeTypeName: DoubleType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -180    MaxValue = 180    Precision = 5
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
                public const string address1_longitude = "address1_longitude";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Name
                /// 
                /// Description:
                ///     (English - United States - 1033): Name to enter for address 1.
                /// 
                /// SchemaName: Address1_Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
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
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string address1_name = "address1_name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Bill To ZIP/Postal Code
                /// 
                /// Description:
                ///     (English - United States - 1033): ZIP Code or postal code for address 1.
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
                public const string address1_postalcode = "address1_postalcode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Post Office Box
                /// 
                /// Description:
                ///     (English - United States - 1033): Post office box number for address 1.
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
                public const string address1_postofficebox = "address1_postofficebox";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Shipping Method
                /// 
                /// Description:
                ///     (English - United States - 1033): Method of shipment for address 1.
                /// 
                /// SchemaName: Address1_ShippingMethodCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet businessunit_address1_shippingmethodcode
                /// DefaultFormValue = 1
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
                ///     (English - United States - 1033): Bill To State/Province
                /// 
                /// Description:
                ///     (English - United States - 1033): State or province for address 1.
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
                public const string address1_stateorprovince = "address1_stateorprovince";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Main Phone
                /// 
                /// Description:
                ///     (English - United States - 1033): First telephone number associated with address 1.
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
                /// IsRetrievable                  True
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string address1_telephone1 = "address1_telephone1";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Other Phone
                /// 
                /// Description:
                ///     (English - United States - 1033): Second telephone number associated with address 1.
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
                public const string address1_telephone2 = "address1_telephone2";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Telephone 3
                /// 
                /// Description:
                ///     (English - United States - 1033): Third telephone number associated with address 1.
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
                public const string address1_telephone3 = "address1_telephone3";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: UPS Zone
                /// 
                /// Description:
                ///     (English - United States - 1033): United Parcel Service (UPS) zone for address 1.
                /// 
                /// SchemaName: Address1_UPSZone
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 4
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
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string address1_upszone = "address1_upszone";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: UTC Offset
                /// 
                /// Description:
                ///     (English - United States - 1033): UTC offset for address 1. This is the difference between local time and standard Coordinated Universal Time.
                /// 
                /// SchemaName: Address1_UTCOffset
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -1500    MaxValue = 1500
                /// Format = TimeZone
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
                public const string address2_addressid = "address2_addressid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Address Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of address for address 2, such as billing, shipping, or primary address.
                /// 
                /// SchemaName: Address2_AddressTypeCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet businessunit_address2_addresstypecode
                /// DefaultFormValue = 1
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
                ///     (English - United States - 1033): Ship To City
                /// 
                /// Description:
                ///     (English - United States - 1033): City name for address 2.
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
                public const string address2_city = "address2_city";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Ship To Country/Region
                /// 
                /// Description:
                ///     (English - United States - 1033): Country/region name for address 2.
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
                public const string address2_country = "address2_country";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: County
                /// 
                /// Description:
                ///     (English - United States - 1033): County name for address 2.
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
                public const string address2_county = "address2_county";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Fax
                /// 
                /// Description:
                ///     (English - United States - 1033): Fax number for address 2.
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
                public const string address2_fax = "address2_fax";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Latitude
                /// 
                /// Description:
                ///     (English - United States - 1033): Latitude for address 2.
                /// 
                /// SchemaName: Address2_Latitude
                /// DoubleAttributeMetadata    AttributeType: Double    AttributeTypeName: DoubleType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -90    MaxValue = 90    Precision = 5
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
                public const string address2_latitude = "address2_latitude";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Ship To Street 1
                /// 
                /// Description:
                ///     (English - United States - 1033): First line for entering address 2 information.
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
                public const string address2_line1 = "address2_line1";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Ship To Street 2
                /// 
                /// Description:
                ///     (English - United States - 1033): Second line for entering address 2 information.
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
                public const string address2_line2 = "address2_line2";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Ship To Street 3
                /// 
                /// Description:
                ///     (English - United States - 1033): Third line for entering address 2 information.
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
                public const string address2_line3 = "address2_line3";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Longitude
                /// 
                /// Description:
                ///     (English - United States - 1033): Longitude for address 2.
                /// 
                /// SchemaName: Address2_Longitude
                /// DoubleAttributeMetadata    AttributeType: Double    AttributeTypeName: DoubleType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -180    MaxValue = 180    Precision = 5
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
                public const string address2_longitude = "address2_longitude";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Name
                /// 
                /// Description:
                ///     (English - United States - 1033): Name to enter for address 2.
                /// 
                /// SchemaName: Address2_Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
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
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string address2_name = "address2_name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Ship To ZIP/Postal Code
                /// 
                /// Description:
                ///     (English - United States - 1033): ZIP Code or postal code for address 2.
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
                public const string address2_postalcode = "address2_postalcode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Post Office Box
                /// 
                /// Description:
                ///     (English - United States - 1033): Post office box number for address 2.
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
                public const string address2_postofficebox = "address2_postofficebox";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Shipping Method
                /// 
                /// Description:
                ///     (English - United States - 1033): Method of shipment for address 2.
                /// 
                /// SchemaName: Address2_ShippingMethodCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet businessunit_address2_shippingmethodcode
                /// DefaultFormValue = 1
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
                ///     (English - United States - 1033): Ship To State/Province
                /// 
                /// Description:
                ///     (English - United States - 1033): State or province for address 2.
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
                public const string address2_stateorprovince = "address2_stateorprovince";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Telephone 1
                /// 
                /// Description:
                ///     (English - United States - 1033): First telephone number associated with address 2.
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
                public const string address2_telephone1 = "address2_telephone1";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Telephone 2
                /// 
                /// Description:
                ///     (English - United States - 1033): Second telephone number associated with address 2.
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
                public const string address2_telephone2 = "address2_telephone2";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Telephone 3
                /// 
                /// Description:
                ///     (English - United States - 1033): Third telephone number associated with address 2.
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
                public const string address2_telephone3 = "address2_telephone3";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: UPS Zone
                /// 
                /// Description:
                ///     (English - United States - 1033): United Parcel Service (UPS) zone for address 2.
                /// 
                /// SchemaName: Address2_UPSZone
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 4
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
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string address2_upszone = "address2_upszone";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: UTC Offset
                /// 
                /// Description:
                ///     (English - United States - 1033): UTC offset for address 2. This is the difference between local time and standard Coordinated Universal Time.
                /// 
                /// SchemaName: Address2_UTCOffset
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -1500    MaxValue = 1500
                /// Format = TimeZone
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
                public const string address2_utcoffset = "address2_utcoffset";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Calendar
                /// 
                /// Description:
                ///     (English - United States - 1033): Fiscal calendar associated with the business unit.
                /// 
                /// SchemaName: CalendarId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: calendar
                /// 
                ///     Target calendar    PrimaryIdAttribute calendarid    PrimaryNameAttribute name
                ///         DisplayName:
                ///             (English - United States - 1033): Calendar
                ///         
                ///         DisplayCollectionName:
                ///             (English - United States - 1033): Calendars
                ///         
                ///         Description:
                ///             (English - United States - 1033): Calendar used by the scheduling system to define when an appointment or activity is to occur.
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
                public const string calendarid = "calendarid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Cost Center
                /// 
                /// Description:
                ///     (English - United States - 1033): Name of the business unit cost center.
                /// 
                /// SchemaName: CostCenter
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
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
                public const string costcenter = "costcenter";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who created the business unit.
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
                ///     (English - United States - 1033): Date and time when the business unit was created.
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who created the businessunit.
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
                ///     (English - United States - 1033): Credit Limit
                /// 
                /// Description:
                ///     (English - United States - 1033): Credit limit for the business unit.
                /// 
                /// SchemaName: CreditLimit
                /// DoubleAttributeMetadata    AttributeType: Double    AttributeTypeName: DoubleType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = 0    MaxValue = 1000000000    Precision = 2
                /// ImeMode = Disabled
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
                public const string creditlimit = "creditlimit";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Description
                /// 
                /// Description:
                ///     (English - United States - 1033): Description of the business unit.
                /// 
                /// SchemaName: Description
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 2000
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
                /// IsPrimaryName                  False
                /// IsRenameable                   True
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
                ///     (English - United States - 1033): Disable Reason
                /// 
                /// Description:
                ///     (English - United States - 1033): Reason for disabling the business unit.
                /// 
                /// SchemaName: DisabledReason
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 500
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
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
                public const string disabledreason = "disabledreason";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Division
                /// 
                /// Description:
                ///     (English - United States - 1033): Name of the division to which the business unit belongs.
                /// 
                /// SchemaName: DivisionName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
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
                /// IsPrimaryName                  False
                /// IsRenameable                   True
                /// IsRequiredForForm              False
                /// IsRetrievable                  False
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string divisionname = "divisionname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Email
                /// 
                /// Description:
                ///     (English - United States - 1033): Email address for the business unit.
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
                ///     (English - United States - 1033): Exchange rate for the currency associated with the businessunit with respect to the base currency.
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
                ///     (English - United States - 1033): File as Name
                /// 
                /// Description:
                ///     (English - United States - 1033): Alternative name under which the business unit can be filed.
                /// 
                /// SchemaName: FileAsName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
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
                public const string fileasname = "fileasname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): FTP Site
                /// 
                /// Description:
                ///     (English - United States - 1033): FTP site URL for the business unit.
                /// 
                /// SchemaName: FtpSiteUrl
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Url    ImeMode = Inactive    IsLocalizable = False
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
                public const string importsequencenumber = "importsequencenumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Inheritance Mask
                /// 
                /// Description:
                ///     (English - United States - 1033): Inheritance mask for the business unit.
                /// 
                /// SchemaName: InheritanceMask
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: False
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 1000000000
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
                public const string inheritancemask = "inheritancemask";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Disabled
                /// 
                /// Description:
                ///     (English - United States - 1033): Information about whether the business unit is enabled or disabled.
                /// 
                /// SchemaName: IsDisabled
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: True
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
                public const string isdisabled = "isdisabled";

                ///<summary>
                /// SchemaName: IsDisabledName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'isdisabled'
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
                //public const string isdisabledname = "isdisabledname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who last modified the business unit.
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
                ///     (English - United States - 1033): Date and time when the business unit was last modified.
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who last modified the businessunit.
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
                ///     (English - United States - 1033): Unique identifier of the organization associated with the business unit.
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
                ///     (English - United States - 1033): Parent Business
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier for the parent business unit.
                /// 
                /// SchemaName: ParentBusinessUnitId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: ApplicationRequired
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
                public const string parentbusinessunitid = "parentbusinessunitid";

                ///<summary>
                /// SchemaName: ParentBusinessUnitIdName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired    AttributeOf 'parentbusinessunitid'
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
                //public const string parentbusinessunitidname = "parentbusinessunitidname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Picture
                /// 
                /// Description:
                ///     (English - United States - 1033): Picture or diagram of the business unit.
                /// 
                /// SchemaName: Picture
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
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
                public const string picture = "picture";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Stock Exchange
                /// 
                /// Description:
                ///     (English - United States - 1033): Stock exchange on which the business is listed.
                /// 
                /// SchemaName: StockExchange
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
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
                public const string stockexchange = "stockexchange";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Ticker Symbol
                /// 
                /// Description:
                ///     (English - United States - 1033): Stock exchange ticker symbol for the business unit.
                /// 
                /// SchemaName: TickerSymbol
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 10
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
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
                public const string tickersymbol = "tickersymbol";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Currency
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the currency associated with the businessunit.
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
                /// SchemaName: UserGroupId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForUpdate: False
                /// IsValidForRead: False    IsValidForAdvancedFind: False
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
                //public const string usergroupid = "usergroupid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): UTC Offset
                /// 
                /// Description:
                ///     (English - United States - 1033): UTC offset for the business unit. This is the difference between local time and standard Coordinated Universal Time.
                /// 
                /// SchemaName: UTCOffset
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -1500    MaxValue = 1500
                /// Format = TimeZone
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
                public const string utcoffset = "utcoffset";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Version number
                /// 
                /// Description:
                ///     (English - United States - 1033): Version number of the business unit.
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
                ///     (English - United States - 1033): Website
                /// 
                /// Description:
                ///     (English - United States - 1033): Website URL for the business unit.
                /// 
                /// SchemaName: WebSiteUrl
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForUpdate: True
                /// IsValidForRead: True    IsValidForAdvancedFind: True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Url    ImeMode = Inactive    IsLocalizable = False
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
                /// IsRetrievable                  True
                /// IsSearchable                   False
                /// IsSortableEnabled              False
                /// IsValidForForm                 True
                /// IsValidForGrid                 True
                ///</summary>
                public const string websiteurl = "websiteurl";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Workflow Suspended
                /// 
                /// Description:
                ///     (English - United States - 1033): Information about whether workflow or sales process rules have been suspended.
                /// 
                /// SchemaName: WorkflowSuspended
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
                public const string workflowsuspended = "workflowsuspended";

                ///<summary>
                /// SchemaName: WorkflowSuspendedName
                /// AttributeMetadata    AttributeType: Virtual    AttributeTypeName: VirtualType    RequiredLevel: None    AttributeOf 'workflowsuspended'
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
                //public const string workflowsuspendedname = "workflowsuspendedname";
            }

            #endregion Attributes.

            #region OptionSets.

            public static partial class OptionSets
            {

                #region Picklist OptionSet OptionSets.
                ///<summary>
                /// Attribute:
                ///     address1_addresstypecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Address Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of address for address 1, such as billing, shipping, or primary address.
                /// 
                /// Local System  OptionSet businessunit_address1_addresstypecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Address Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of address for address 1, such as billing, shipping, or primary address.
                ///</summary>
                public enum address1_addresstypecode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Default Value
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Default_Value_1 = 1,
                }

                ///<summary>
                /// Attribute:
                ///     address1_shippingmethodcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Shipping Method
                /// 
                /// Description:
                ///     (English - United States - 1033): Method of shipment for address 1.
                /// 
                /// Local System  OptionSet businessunit_address1_shippingmethodcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Shipping Method 
                /// 
                /// Description:
                ///     (English - United States - 1033): Method of shipment for address 1.
                ///</summary>
                public enum address1_shippingmethodcode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Default Value
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Default_Value_1 = 1,
                }

                ///<summary>
                /// Attribute:
                ///     address2_addresstypecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Address Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of address for address 2, such as billing, shipping, or primary address.
                /// 
                /// Local System  OptionSet businessunit_address2_addresstypecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Address Type
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of address for address 2, such as billing, shipping, or primary address.
                ///</summary>
                public enum address2_addresstypecode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Default Value
                    ///</summary>
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
                ///     (English - United States - 1033): Method of shipment for address 2.
                /// 
                /// Local System  OptionSet businessunit_address2_shippingmethodcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Shipping Method 
                /// 
                /// Description:
                ///     (English - United States - 1033): Method of shipment for address 2.
                ///</summary>
                public enum address2_shippingmethodcode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Default Value
                    ///</summary>
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
                /// N:1 - Relationship business_unit_parent_business_unit
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_parent_business_unit
                /// ReferencingEntityNavigationPropertyName    parentbusinessunitid
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
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
                /// AttributeMaps:
                ///     SourceEntity             TargetEntity
                ///     businessunit       ->    businessunit
                ///     
                ///     SourceAttribute          TargetAttribute
                ///     businessunitid     ->    parentbusinessunitid
                ///     name               ->    parentbusinessunitidname
                ///</summary>
                public static partial class business_unit_parent_business_unit
                {
                    public const string Name = "business_unit_parent_business_unit";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_businessunit = "businessunit";

                    public const string ReferencingAttribute_parentbusinessunitid = "parentbusinessunitid";
                }

                ///<summary>
                /// N:1 - Relationship BusinessUnit_Calendar
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_Calendar
                /// ReferencingEntityNavigationPropertyName    calendarid
                /// IsCustomizable                             False
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
                /// ReferencedEntity calendar:    PrimaryIdAttribute calendarid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Calendar
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Calendars
                ///     
                ///     Description:
                ///         (English - United States - 1033): Calendar used by the scheduling system to define when an appointment or activity is to occur.
                ///</summary>
                public static partial class businessunit_calendar
                {
                    public const string Name = "BusinessUnit_Calendar";

                    public const string ReferencedEntity_calendar = "calendar";

                    public const string ReferencedAttribute_calendarid = "calendarid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_businessunit = "businessunit";

                    public const string ReferencingAttribute_calendarid = "calendarid";
                }

                ///<summary>
                /// N:1 - Relationship lk_businessunit_createdonbehalfby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_businessunit_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    createdonbehalfby
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
                public static partial class lk_businessunit_createdonbehalfby
                {
                    public const string Name = "lk_businessunit_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_businessunit = "businessunit";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_businessunit_modifiedonbehalfby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_businessunit_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    modifiedonbehalfby
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
                public static partial class lk_businessunit_modifiedonbehalfby
                {
                    public const string Name = "lk_businessunit_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_businessunit = "businessunit";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_businessunitbase_createdby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_businessunitbase_createdby
                /// ReferencingEntityNavigationPropertyName    createdby
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
                public static partial class lk_businessunitbase_createdby
                {
                    public const string Name = "lk_businessunitbase_createdby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_businessunit = "businessunit";

                    public const string ReferencingAttribute_createdby = "createdby";
                }

                ///<summary>
                /// N:1 - Relationship lk_businessunitbase_modifiedby
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_businessunitbase_modifiedby
                /// ReferencingEntityNavigationPropertyName    modifiedby
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
                public static partial class lk_businessunitbase_modifiedby
                {
                    public const string Name = "lk_businessunitbase_modifiedby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_businessunit = "businessunit";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";
                }

                ///<summary>
                /// N:1 - Relationship organization_business_units
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_business_units
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
                public static partial class organization_business_units
                {
                    public const string Name = "organization_business_units";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_businessunit = "businessunit";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }

                ///<summary>
                /// N:1 - Relationship TransactionCurrency_BusinessUnit
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     TransactionCurrency_BusinessUnit
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
                public static partial class transactioncurrency_businessunit
                {
                    public const string Name = "TransactionCurrency_BusinessUnit";

                    public const string ReferencedEntity_transactioncurrency = "transactioncurrency";

                    public const string ReferencedAttribute_transactioncurrencyid = "transactioncurrencyid";

                    public const string ReferencedEntity_PrimaryNameAttribute_currencyname = "currencyname";

                    public const string ReferencingEntity_businessunit = "businessunit";

                    public const string ReferencingAttribute_transactioncurrencyid = "transactioncurrencyid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship actioncardusersettings_businessunit
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     actioncardusersettings_businessunit
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class actioncardusersettings_businessunit
                {
                    public const string Name = "actioncardusersettings_businessunit";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_actioncardusersettings = "actioncardusersettings";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";
                }

                ///<summary>
                /// 1:N - Relationship bizmap_businessid_businessunit
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     bizmap_businessid_businessunit
                /// ReferencingEntityNavigationPropertyName    businessid_businessunit
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
                /// ReferencingEntity businessunitmap:    PrimaryIdAttribute businessunitmapid
                ///     DisplayName:
                ///         (English - United States - 1033): Business Unit Map
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Business Unit Maps
                ///     
                ///     Description:
                ///         (English - United States - 1033): Stores mapping attributes for business units.
                ///</summary>
                public static partial class bizmap_businessid_businessunit
                {
                    public const string Name = "bizmap_businessid_businessunit";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_businessunitmap = "businessunitmap";

                    public const string ReferencingAttribute_businessid = "businessid";
                }

                ///<summary>
                /// 1:N - Relationship bizmap_subbusinessid_businessunit
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     bizmap_subbusinessid_businessunit
                /// ReferencingEntityNavigationPropertyName    subbusinessid_businessunit
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
                /// ReferencingEntity businessunitmap:    PrimaryIdAttribute businessunitmapid
                ///     DisplayName:
                ///         (English - United States - 1033): Business Unit Map
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Business Unit Maps
                ///     
                ///     Description:
                ///         (English - United States - 1033): Stores mapping attributes for business units.
                ///</summary>
                public static partial class bizmap_subbusinessid_businessunit
                {
                    public const string Name = "bizmap_subbusinessid_businessunit";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_businessunitmap = "businessunitmap";

                    public const string ReferencingAttribute_subbusinessid = "subbusinessid";
                }

                ///<summary>
                /// 1:N - Relationship BulkDeleteOperation_BusinessUnit
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BulkDeleteOperation_BusinessUnit
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// ReferencingEntity bulkdeleteoperation:    PrimaryIdAttribute bulkdeleteoperationid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Bulk Delete Operation
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Bulk Delete Operations
                ///     
                ///     Description:
                ///         (English - United States - 1033): User-submitted bulk deletion job.
                ///</summary>
                public static partial class bulkdeleteoperation_businessunit
                {
                    public const string Name = "BulkDeleteOperation_BusinessUnit";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_bulkdeleteoperation = "bulkdeleteoperation";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_customer_opportunity_roles
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_customer_opportunity_roles
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_customer_opportunity_roles
                {
                    public const string Name = "business_customer_opportunity_roles";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_customeropportunityrole = "customeropportunityrole";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_opportunityroleidname = "opportunityroleidname";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_accounts
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
                public static partial class business_unit_accounts
                {
                    public const string Name = "business_unit_accounts";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_account = "account";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_actioncards
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_actioncards
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_actioncards
                {
                    public const string Name = "business_unit_actioncards";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_actioncard = "actioncard";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_activitypointer
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_activitypointer
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_activitypointer
                {
                    public const string Name = "business_unit_activitypointer";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_activitypointer = "activitypointer";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_annotations
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_annotations
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_annotations
                {
                    public const string Name = "business_unit_annotations";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_annotation = "annotation";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_appointment_activities
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_appointment_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_appointment
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
                public static partial class business_unit_appointment_activities
                {
                    public const string Name = "business_unit_appointment_activities";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_appointment = "appointment";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_asyncoperation
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_asyncoperation
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_asyncoperation
                {
                    public const string Name = "business_unit_asyncoperation";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_asyncoperation = "asyncoperation";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_bookableresource
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_bookableresource
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_bookableresource
                {
                    public const string Name = "business_unit_bookableresource";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_bookableresource = "bookableresource";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_bookableresourcebooking
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_bookableresourcebooking
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_bookableresourcebooking
                {
                    public const string Name = "business_unit_bookableresourcebooking";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_bookableresourcebooking = "bookableresourcebooking";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_bookableresourcebookingexchangesyncidmapping
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_bookableresourcebookingexchangesyncidmapping
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_bookableresourcebookingexchangesyncidmapping
                {
                    public const string Name = "business_unit_bookableresourcebookingexchangesyncidmapping";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_bookableresourcebookingexchangesyncidmapping = "bookableresourcebookingexchangesyncidmapping";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_bookableresourcebookingheader
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_bookableresourcebookingheader
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_bookableresourcebookingheader
                {
                    public const string Name = "business_unit_bookableresourcebookingheader";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_bookableresourcebookingheader = "bookableresourcebookingheader";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_bookableresourcecategory
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_bookableresourcecategory
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_bookableresourcecategory
                {
                    public const string Name = "business_unit_bookableresourcecategory";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_bookableresourcecategory = "bookableresourcecategory";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_bookableresourcecategoryassn
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_bookableresourcecategoryassn
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_bookableresourcecategoryassn
                {
                    public const string Name = "business_unit_bookableresourcecategoryassn";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_bookableresourcecategoryassn = "bookableresourcecategoryassn";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_bookableresourcecharacteristic
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_bookableresourcecharacteristic
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_bookableresourcecharacteristic
                {
                    public const string Name = "business_unit_bookableresourcecharacteristic";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_bookableresourcecharacteristic = "bookableresourcecharacteristic";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_bookableresourcegroup
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_bookableresourcegroup
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_bookableresourcegroup
                {
                    public const string Name = "business_unit_bookableresourcegroup";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_bookableresourcegroup = "bookableresourcegroup";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_bookingstatus
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_bookingstatus
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_bookingstatus
                {
                    public const string Name = "business_unit_bookingstatus";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_bookingstatus = "bookingstatus";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_BulkOperation_activities
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_BulkOperation_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_bulkoperation
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
                public static partial class business_unit_bulkoperation_activities
                {
                    public const string Name = "business_unit_BulkOperation_activities";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_bulkoperation = "bulkoperation";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_calendars
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_calendars
                /// ReferencingEntityNavigationPropertyName    businessunitid
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
                /// ReferencingEntity calendar:    PrimaryIdAttribute calendarid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Calendar
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Calendars
                ///     
                ///     Description:
                ///         (English - United States - 1033): Calendar used by the scheduling system to define when an appointment or activity is to occur.
                ///</summary>
                public static partial class business_unit_calendars
                {
                    public const string Name = "business_unit_calendars";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_calendar = "calendar";

                    public const string ReferencingAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_campaignactivity_activities
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_campaignactivity_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_campaignactivity
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
                public static partial class business_unit_campaignactivity_activities
                {
                    public const string Name = "business_unit_campaignactivity_activities";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_campaignactivity = "campaignactivity";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_campaignresponse_activities
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_campaignresponse_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_campaignresponse
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
                public static partial class business_unit_campaignresponse_activities
                {
                    public const string Name = "business_unit_campaignresponse_activities";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_campaignresponse = "campaignresponse";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_category
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_category
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
                /// ReferencingEntity category:    PrimaryIdAttribute categoryid    PrimaryNameAttribute title
                ///     DisplayName:
                ///         (English - United States - 1033): Category
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Categories
                ///     
                ///     Description:
                ///         (English - United States - 1033): Entity for categorizing records to make it easier for your customers to find them on portals and through search.
                ///</summary>
                public static partial class business_unit_category
                {
                    public const string Name = "business_unit_category";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_category = "category";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_channelaccessprofile
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_channelaccessprofile
                /// ReferencingEntityNavigationPropertyName    business_unit_channelaccessprofile
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
                public static partial class business_unit_channelaccessprofile
                {
                    public const string Name = "business_unit_channelaccessprofile";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_channelaccessprofile = "channelaccessprofile";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_characteristic
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_characteristic
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_characteristic
                {
                    public const string Name = "business_unit_characteristic";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_characteristic = "characteristic";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_connections
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_connections
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_connections
                {
                    public const string Name = "business_unit_connections";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_connection = "connection";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_constraint_based_groups
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_constraint_based_groups
                /// ReferencingEntityNavigationPropertyName    businessunitid_businessunit
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
                /// ReferencingEntity constraintbasedgroup:    PrimaryIdAttribute constraintbasedgroupid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Resource Group
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Resource Groups
                ///     
                ///     Description:
                ///         (English - United States - 1033): Group or collection of people, equipment, and/or facilities that can be scheduled.
                /// 
                /// AttributeMaps:
                ///     SourceEntity             TargetEntity
                ///     businessunit       ->    constraintbasedgroup
                ///     
                ///     SourceAttribute          TargetAttribute
                ///     businessunitid     ->    businessunitid
                ///     name               ->    businessunitidname
                ///</summary>
                public static partial class business_unit_constraint_based_groups
                {
                    public const string Name = "business_unit_constraint_based_groups";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_constraintbasedgroup = "constraintbasedgroup";

                    public const string ReferencingAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_contacts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_contacts
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
                public static partial class business_unit_contacts
                {
                    public const string Name = "business_unit_contacts";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_contact = "contact";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_convertrule
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_convertrule
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_convertrule
                {
                    public const string Name = "business_unit_convertrule";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_convertrule = "convertrule";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_customer_relationship
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_customer_relationship
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_customer_relationship
                {
                    public const string Name = "business_unit_customer_relationship";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_customerrelationship = "customerrelationship";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_customerroleidname = "customerroleidname";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_dynamicproperyinstance
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_dynamicproperyinstance
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_dynamicproperyinstance
                {
                    public const string Name = "business_unit_dynamicproperyinstance";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_dynamicpropertyinstance = "dynamicpropertyinstance";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_email_activities
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_email_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_email
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
                public static partial class business_unit_email_activities
                {
                    public const string Name = "business_unit_email_activities";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_email = "email";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_emailserverprofile
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_emailserverprofile
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_emailserverprofile
                {
                    public const string Name = "business_unit_emailserverprofile";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_emailserverprofile = "emailserverprofile";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_emailsignatures
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_emailsignatures
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// ReferencingEntity emailsignature:    PrimaryIdAttribute emailsignatureid    PrimaryNameAttribute title
                ///     DisplayName:
                ///         (English - United States - 1033): Email Signature
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Email Signatures
                ///     
                ///     Description:
                ///         (English - United States - 1033): Signature for email message
                ///</summary>
                public static partial class business_unit_emailsignatures
                {
                    public const string Name = "business_unit_emailsignatures";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_emailsignature = "emailsignature";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_entitlement
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_entitlement
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_entitlement
                {
                    public const string Name = "business_unit_entitlement";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_entitlement = "entitlement";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_equipment
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_equipment
                /// ReferencingEntityNavigationPropertyName    businessunitid_businessunit
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
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity equipment:    PrimaryIdAttribute equipmentid    PrimaryNameAttribute name
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
                ///     businessunit       ->    equipment
                ///     
                ///     SourceAttribute          TargetAttribute
                ///     businessunitid     ->    businessunitid
                ///     name               ->    businessunitidname
                ///</summary>
                public static partial class business_unit_equipment
                {
                    public const string Name = "business_unit_equipment";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_equipment = "equipment";

                    public const string ReferencingAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_exchangesyncidmapping
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_exchangesyncidmapping
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_exchangesyncidmapping
                {
                    public const string Name = "business_unit_exchangesyncidmapping";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_exchangesyncidmapping = "exchangesyncidmapping";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_externalparty
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_externalparty
                /// ReferencingEntityNavigationPropertyName    business_unit_externalparty_externalparty
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
                public static partial class business_unit_externalparty
                {
                    public const string Name = "business_unit_externalparty";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_externalparty = "externalparty";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_fax_activities
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_fax_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_fax
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
                public static partial class business_unit_fax_activities
                {
                    public const string Name = "business_unit_fax_activities";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_fax = "fax";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_feedback
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_feedback
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
                /// ReferencingEntity feedback:    PrimaryIdAttribute feedbackid    PrimaryNameAttribute title
                ///     DisplayName:
                ///         (English - United States - 1033): Feedback
                ///     
                ///     Description:
                ///         (English - United States - 1033): Container for feedback and ratings for knowledge articles.
                ///</summary>
                public static partial class business_unit_feedback
                {
                    public const string Name = "business_unit_feedback";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_feedback = "feedback";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_gbc_entity_test
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_gbc_entity_test
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// ReferencingEntity gbc_entity_test:    PrimaryIdAttribute gbc_entity_testid    PrimaryNameAttribute gbc_name
                ///     DisplayName:
                ///         (English - United States - 1033): Entity Test
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Entities Test
                ///</summary>
                public static partial class business_unit_gbc_entity_test
                {
                    public const string Name = "business_unit_gbc_entity_test";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_gbc_entity_test = "gbc_entity_test";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_gbc_name = "gbc_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_goal
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_goal
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
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
                public static partial class business_unit_goal
                {
                    public const string Name = "business_unit_goal";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_goal = "goal";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_goalrollupquery
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_goalrollupquery
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_goalrollupquery
                {
                    public const string Name = "business_unit_goalrollupquery";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_goalrollupquery = "goalrollupquery";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_incident_resolution_activities
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_incident_resolution_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_incidentresolution
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
                public static partial class business_unit_incident_resolution_activities
                {
                    public const string Name = "business_unit_incident_resolution_activities";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_incidentresolution = "incidentresolution";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_incidents
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_incidents
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_incidents
                {
                    public const string Name = "business_unit_incidents";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_incident = "incident";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_interactionforemail
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_new_interactionforemail
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_interactionforemail
                {
                    public const string Name = "business_unit_interactionforemail";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_interactionforemail = "interactionforemail";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_invoices
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_invoices
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
                public static partial class business_unit_invoices
                {
                    public const string Name = "business_unit_invoices";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_invoice = "invoice";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_knowledgearticle
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_knowledgearticle
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_knowledgearticle
                {
                    public const string Name = "business_unit_knowledgearticle";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_knowledgearticle = "knowledgearticle";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_leads
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_leads
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
                public static partial class business_unit_leads
                {
                    public const string Name = "business_unit_leads";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_lead = "lead";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_letter_activities
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_letter_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_letter
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
                public static partial class business_unit_letter_activities
                {
                    public const string Name = "business_unit_letter_activities";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_letter = "letter";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_list
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_list
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_list
                {
                    public const string Name = "business_unit_list";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_list = "list";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_listname = "listname";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_mailbox
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_mailbox
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// ReferencingEntity mailbox:    PrimaryIdAttribute mailboxid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Mailbox
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Mailboxes
                ///</summary>
                public static partial class business_unit_mailbox
                {
                    public const string Name = "business_unit_mailbox";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_mailbox = "mailbox";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_mailmergetemplates
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_mailmergetemplates
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
                /// ReferencingEntity mailmergetemplate:    PrimaryIdAttribute mailmergetemplateid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Mail Merge Template
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Mail Merge Templates
                ///     
                ///     Description:
                ///         (English - United States - 1033): Template for a mail merge document that contains the standard attributes of that document.
                ///</summary>
                public static partial class business_unit_mailmergetemplates
                {
                    public const string Name = "business_unit_mailmergetemplates";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_mailmergetemplate = "mailmergetemplate";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_msdyn_postalbum
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_msdyn_postalbum
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_msdyn_postalbum
                {
                    public const string Name = "business_unit_msdyn_postalbum";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_msdyn_postalbum = "msdyn_postalbum";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_msdyn_name = "msdyn_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_msdyn_relationshipinsightsunifiedconfig
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_msdyn_relationshipinsightsunifiedconfig
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// ReferencingEntity msdyn_relationshipinsightsunifiedconfig:    PrimaryIdAttribute msdyn_relationshipinsightsunifiedconfigid    PrimaryNameAttribute new_name
                ///     DisplayName:
                ///         (English - United States - 1033): msdyn_relationshipinsightsunifiedconfig
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): msdyn_relationshipinsightsunifiedconfigs
                ///</summary>
                public static partial class business_unit_msdyn_relationshipinsightsunifiedconfig
                {
                    public const string Name = "business_unit_msdyn_relationshipinsightsunifiedconfig";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_msdyn_relationshipinsightsunifiedconfig = "msdyn_relationshipinsightsunifiedconfig";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_new_name = "new_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_msdyn_siconfig
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_msdyn_siconfig
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// ReferencingEntity msdyn_siconfig:    PrimaryIdAttribute msdyn_siconfigid    PrimaryNameAttribute msdyn_version
                ///     DisplayName:
                ///         (English - United States - 1033): siconfig
                ///</summary>
                public static partial class business_unit_msdyn_siconfig
                {
                    public const string Name = "business_unit_msdyn_siconfig";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_msdyn_siconfig = "msdyn_siconfig";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_msdyn_version = "msdyn_version";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_msdyn_wallsavedqueryusersettings
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_msdyn_wallsavedqueryusersettings
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_msdyn_wallsavedqueryusersettings
                {
                    public const string Name = "business_unit_msdyn_wallsavedqueryusersettings";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_msdyn_wallsavedqueryusersettings = "msdyn_wallsavedqueryusersettings";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_msdyn_entityname = "msdyn_entityname";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_opportunities
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_opportunities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_opportunities
                {
                    public const string Name = "business_unit_opportunities";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_opportunity = "opportunity";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_opportunity_close_activities
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_opportunity_close_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_opportunityclose
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
                public static partial class business_unit_opportunity_close_activities
                {
                    public const string Name = "business_unit_opportunity_close_activities";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_opportunityclose = "opportunityclose";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_order_close_activities
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_order_close_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_orderclose
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
                public static partial class business_unit_order_close_activities
                {
                    public const string Name = "business_unit_order_close_activities";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_orderclose = "orderclose";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_orders
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_orders
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_orders
                {
                    public const string Name = "business_unit_orders";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_salesorder = "salesorder";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_parent_business_unit
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_parent_business_unit
                /// ReferencingEntityNavigationPropertyName    parentbusinessunitid
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     False
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
                /// AttributeMaps:
                ///     SourceEntity             TargetEntity
                ///     businessunit       ->    businessunit
                ///     
                ///     SourceAttribute          TargetAttribute
                ///     businessunitid     ->    parentbusinessunitid
                ///     name               ->    parentbusinessunitidname
                ///</summary>
                public static partial class business_unit_parent_business_unit
                {
                    public const string Name = "business_unit_parent_business_unit";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_businessunit = "businessunit";

                    public const string ReferencingAttribute_parentbusinessunitid = "parentbusinessunitid";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_personaldocumenttemplates
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_personaldocumenttemplates
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
                /// ReferencingEntity personaldocumenttemplate:    PrimaryIdAttribute personaldocumenttemplateid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Personal Document Template
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Personal Document Templates
                ///     
                ///     Description:
                ///         (English - United States - 1033): Used to store Personal Document Templates in database in binary format.
                ///</summary>
                public static partial class business_unit_personaldocumenttemplates
                {
                    public const string Name = "business_unit_personaldocumenttemplates";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_personaldocumenttemplate = "personaldocumenttemplate";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_phone_call_activities
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_phone_call_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_phonecall
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
                public static partial class business_unit_phone_call_activities
                {
                    public const string Name = "business_unit_phone_call_activities";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_phonecall = "phonecall";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_postfollows
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_postfollows
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_postfollows
                {
                    public const string Name = "business_unit_postfollows";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_postfollow = "postfollow";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_regardingobjectidname = "regardingobjectidname";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_PostRegarding
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_PostRegarding
                /// ReferencingEntityNavigationPropertyName    regardingobjectowningbusinessunit
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
                /// ReferencingEntity postregarding:    PrimaryIdAttribute postregardingid
                ///     DisplayName:
                ///         (English - United States - 1033): Post Regarding
                ///     
                ///     Description:
                ///         (English - United States - 1033): Represents which object an activity feed post is regarding. For internal use only.
                ///</summary>
                public static partial class business_unit_postregarding
                {
                    public const string Name = "business_unit_PostRegarding";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_postregarding = "postregarding";

                    public const string ReferencingAttribute_regardingobjectowningbusinessunit = "regardingobjectowningbusinessunit";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_profilerule
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_profilerule
                /// ReferencingEntityNavigationPropertyName    profileruleid5
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
                public static partial class business_unit_profilerule
                {
                    public const string Name = "business_unit_profilerule";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_channelaccessprofilerule = "channelaccessprofilerule";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_queues
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_queues
                /// ReferencingEntityNavigationPropertyName    businessunitid
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
                /// ReferencingEntity queue:    PrimaryIdAttribute queueid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Queue
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Queues
                ///     
                ///     Description:
                ///         (English - United States - 1033): A list of records that require action, such as accounts, activities, and cases.
                ///</summary>
                public static partial class business_unit_queues
                {
                    public const string Name = "business_unit_queues";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_queue = "queue";

                    public const string ReferencingAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_queues2
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_queues2
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// ReferencingEntity queue:    PrimaryIdAttribute queueid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Queue
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Queues
                ///     
                ///     Description:
                ///         (English - United States - 1033): A list of records that require action, such as accounts, activities, and cases.
                ///</summary>
                public static partial class business_unit_queues2
                {
                    public const string Name = "business_unit_queues2";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_queue = "queue";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_quote_close_activities
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_quote_close_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_quoteclose
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
                public static partial class business_unit_quote_close_activities
                {
                    public const string Name = "business_unit_quote_close_activities";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_quoteclose = "quoteclose";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_quotes
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_quotes
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
                public static partial class business_unit_quotes
                {
                    public const string Name = "business_unit_quotes";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_quote = "quote";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_ratingmodel
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_ratingmodel
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_ratingmodel
                {
                    public const string Name = "business_unit_ratingmodel";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_ratingmodel = "ratingmodel";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_ratingvalue
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_ratingvalue
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_ratingvalue
                {
                    public const string Name = "business_unit_ratingvalue";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_ratingvalue = "ratingvalue";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_recurrencerule
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_recurrencerule
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// ReferencingEntity recurrencerule:    PrimaryIdAttribute ruleid
                ///     DisplayName:
                ///         (English - United States - 1033): Recurrence Rule
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Recurrence Rules
                ///     
                ///     Description:
                ///         (English - United States - 1033): Recurrence Rule represents the pattern of incidence of recurring entities.
                ///</summary>
                public static partial class business_unit_recurrencerule
                {
                    public const string Name = "business_unit_recurrencerule";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_recurrencerule = "recurrencerule";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_recurringappointmentmaster_activities
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_recurringappointmentmaster_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_recurringappointmentmaster
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
                public static partial class business_unit_recurringappointmentmaster_activities
                {
                    public const string Name = "business_unit_recurringappointmentmaster_activities";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_recurringappointmentmaster = "recurringappointmentmaster";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_reports
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_reports
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// ReferencingEntity report:    PrimaryIdAttribute reportid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Report
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Reports
                ///     
                ///     Description:
                ///         (English - United States - 1033): Data summary in an easy-to-read layout.
                ///</summary>
                public static partial class business_unit_reports
                {
                    public const string Name = "business_unit_reports";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_report = "report";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_resource_groups
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_resource_groups
                /// ReferencingEntityNavigationPropertyName    businessunitid
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
                public static partial class business_unit_resource_groups
                {
                    public const string Name = "business_unit_resource_groups";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_resourcegroup = "resourcegroup";

                    public const string ReferencingAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_resource_specs
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_resource_specs
                /// ReferencingEntityNavigationPropertyName    businessunitid
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
                public static partial class business_unit_resource_specs
                {
                    public const string Name = "business_unit_resource_specs";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_resourcespec = "resourcespec";

                    public const string ReferencingAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_resources
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_resources
                /// ReferencingEntityNavigationPropertyName    businessunitid
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
                /// ReferencingEntity resource:    PrimaryIdAttribute resourceid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Resource
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Resources
                ///     
                ///     Description:
                ///         (English - United States - 1033): User or facility/equipment that can be scheduled for a service.
                ///</summary>
                public static partial class business_unit_resources
                {
                    public const string Name = "business_unit_resources";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_resource = "resource";

                    public const string ReferencingAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_roles
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_roles
                /// ReferencingEntityNavigationPropertyName    businessunitid
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
                /// ReferencingEntity role:    PrimaryIdAttribute roleid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Security Role
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Security Roles
                ///     
                ///     Description:
                ///         (English - United States - 1033): Grouping of security privileges. Users are assigned roles that authorize their access to the Microsoft CRM system.
                /// 
                /// AttributeMaps:
                ///     SourceEntity             TargetEntity
                ///     businessunit       ->    role
                ///     
                ///     SourceAttribute          TargetAttribute
                ///     businessunitid     ->    businessunitid
                ///     name               ->    businessunitidname
                ///</summary>
                public static partial class business_unit_roles
                {
                    public const string Name = "business_unit_roles";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_role = "role";

                    public const string ReferencingAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_routingrule
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_routingrule
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_routingrule
                {
                    public const string Name = "business_unit_routingrule";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_routingrule = "routingrule";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_salesprocessinstance
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_salesprocessinstance
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// ReferencingEntity salesprocessinstance:    PrimaryIdAttribute salesprocessinstanceid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Sales Process Instance
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Sales Process Instances
                ///     
                ///     Description:
                ///         (English - United States - 1033): Instance of a sales process.
                ///</summary>
                public static partial class business_unit_salesprocessinstance
                {
                    public const string Name = "business_unit_salesprocessinstance";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_salesprocessinstance = "salesprocessinstance";

                    public const string ReferencingAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_service_appointments
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_service_appointments
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_serviceappointment
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
                public static partial class business_unit_service_appointments
                {
                    public const string Name = "business_unit_service_appointments";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_serviceappointment = "serviceappointment";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_service_contracts
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_service_contracts
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
                public static partial class business_unit_service_contracts
                {
                    public const string Name = "business_unit_service_contracts";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_contract = "contract";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_sharepointdocument
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_sharepointdocument
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_sharepointdocument
                {
                    public const string Name = "business_unit_sharepointdocument";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_sharepointdocument = "sharepointdocument";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_sharepointdocument2
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_sharepointdocument2
                /// ReferencingEntityNavigationPropertyName    businessunitid
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
                public static partial class business_unit_sharepointdocument2
                {
                    public const string Name = "business_unit_sharepointdocument2";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_sharepointdocument = "sharepointdocument";

                    public const string ReferencingAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_sharepointdocumentlocation
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_sharepointdocumentlocation
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
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
                public static partial class business_unit_sharepointdocumentlocation
                {
                    public const string Name = "business_unit_sharepointdocumentlocation";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_sharepointdocumentlocation = "sharepointdocumentlocation";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_sharepointsites
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_sharepointsites
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_sharepointsites
                {
                    public const string Name = "business_unit_sharepointsites";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_sharepointsite = "sharepointsite";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_slabase
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_slabase
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_slabase
                {
                    public const string Name = "business_unit_slabase";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_sla = "sla";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_slakpiinstance
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_slakpiinstance
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_slakpiinstance
                {
                    public const string Name = "business_unit_slakpiinstance";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_slakpiinstance = "slakpiinstance";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_socialactivity
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_socialactivity
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_socialactivity
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
                public static partial class business_unit_socialactivity
                {
                    public const string Name = "business_unit_socialactivity";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_socialactivity = "socialactivity";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_socialprofiles
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_socialprofiles
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
                /// ReferencingEntity socialprofile:    PrimaryIdAttribute socialprofileid    PrimaryNameAttribute profilename
                ///     DisplayName:
                ///         (English - United States - 1033): Social Profile
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Social Profiles
                ///     
                ///     Description:
                ///         (English - United States - 1033): This entity is used to store social profile information of its associated account and contacts on different social channels.
                ///</summary>
                public static partial class business_unit_socialprofiles
                {
                    public const string Name = "business_unit_socialprofiles";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_socialprofile = "socialprofile";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_profilename = "profilename";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_system_users
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_system_users
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
                /// ReferencingEntity systemuser:    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///     DisplayName:
                ///         (English - United States - 1033): User
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///     
                ///     Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                /// 
                /// AttributeMaps:
                ///     SourceEntity             TargetEntity
                ///     businessunit       ->    systemuser
                ///     
                ///     SourceAttribute          TargetAttribute
                ///     businessunitid     ->    businessunitid
                ///     name               ->    businessunitidname
                ///</summary>
                public static partial class business_unit_system_users
                {
                    public const string Name = "business_unit_system_users";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_systemuser = "systemuser";

                    public const string ReferencingAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_PrimaryNameAttribute_fullname = "fullname";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_task_activities
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_task_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_task
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
                public static partial class business_unit_task_activities
                {
                    public const string Name = "business_unit_task_activities";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_task = "task";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_teams
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
                /// ReferencingEntity team:    PrimaryIdAttribute teamid    PrimaryNameAttribute name
                ///     DisplayName:
                ///         (English - United States - 1033): Team
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Teams
                ///     
                ///     Description:
                ///         (English - United States - 1033): Collection of system users that routinely collaborate. Teams can be used to simplify record sharing and provide team members with common access to organization data when team members belong to different Business Units.
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

                    public const string ReferencingEntity_team = "team";

                    public const string ReferencingAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_templates
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_templates
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_templates
                {
                    public const string Name = "business_unit_templates";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_template = "template";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_title = "title";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_TraceRegarding
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_TraceRegarding
                /// ReferencingEntityNavigationPropertyName    regardingobjectowningbusinessunit
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
                /// ReferencingEntity traceregarding:    PrimaryIdAttribute traceregardingid
                ///     DisplayName:
                ///         (English - United States - 1033): Trace Regarding
                ///     
                ///     Description:
                ///         (English - United States - 1033): Represents which object a trace record is regarding. For internal use only.
                ///</summary>
                public static partial class business_unit_traceregarding
                {
                    public const string Name = "business_unit_TraceRegarding";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_traceregarding = "traceregarding";

                    public const string ReferencingAttribute_regardingobjectowningbusinessunit = "regardingobjectowningbusinessunit";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_untrackedemail_activities
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_untrackedemail_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_untrackedemail
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
                /// ReferencingEntity untrackedemail:    PrimaryIdAttribute activityid    PrimaryNameAttribute subject
                ///     DisplayName:
                ///         (English - United States - 1033): UntrackedEmail
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): UntrackedEmail Messages
                ///     
                ///     Description:
                ///         (English - United States - 1033): Activity that is delivered using UntrackedEmail protocols.
                ///</summary>
                public static partial class business_unit_untrackedemail_activities
                {
                    public const string Name = "business_unit_untrackedemail_activities";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_untrackedemail = "untrackedemail";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_user_settings
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_user_settings
                /// ReferencingEntityNavigationPropertyName    businessunitid_businessunit
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
                /// ReferencingEntity usersettings:    PrimaryIdAttribute systemuserid
                ///     DisplayName:
                ///         (English - United States - 1033): User Settings
                ///     
                ///     Description:
                ///         (English - United States - 1033): User's preferred settings.
                ///</summary>
                public static partial class business_unit_user_settings
                {
                    public const string Name = "business_unit_user_settings";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_usersettings = "usersettings";

                    public const string ReferencingAttribute_businessunitid = "businessunitid";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_userapplicationmetadata
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_userapplicationmetadata
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// ReferencingEntity userapplicationmetadata:    PrimaryIdAttribute userapplicationmetadataid
                ///     DisplayName:
                ///         (English - United States - 1033): User Application Metadata
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): User Application Metadata Collection
                ///     
                ///     Description:
                ///         (English - United States - 1033): For internal use only.
                ///</summary>
                public static partial class business_unit_userapplicationmetadata
                {
                    public const string Name = "business_unit_userapplicationmetadata";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_userapplicationmetadata = "userapplicationmetadata";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_userform
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_userform
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_userform
                {
                    public const string Name = "business_unit_userform";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_userform = "userform";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_userquery
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_userquery
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_userquery
                {
                    public const string Name = "business_unit_userquery";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_userquery = "userquery";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_userqueryvisualizations
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_userqueryvisualizations
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_userqueryvisualizations
                {
                    public const string Name = "business_unit_userqueryvisualizations";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_userqueryvisualization = "userqueryvisualization";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_workflow
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_workflow
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_workflow
                {
                    public const string Name = "business_unit_workflow";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_workflow = "workflow";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_workflowlogs
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_workflowlogs
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class business_unit_workflowlogs
                {
                    public const string Name = "business_unit_workflowlogs";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_workflowlog = "workflowlog";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";
                }

                ///<summary>
                /// 1:N - Relationship BusinessUnit_AsyncOperations
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_AsyncOperations
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_businessunit
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
                public static partial class businessunit_asyncoperations
                {
                    public const string Name = "BusinessUnit_AsyncOperations";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_asyncoperation = "asyncoperation";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship BusinessUnit_BulkDeleteFailures
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_BulkDeleteFailures
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_businessunit
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
                public static partial class businessunit_bulkdeletefailures
                {
                    public const string Name = "BusinessUnit_BulkDeleteFailures";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_bulkdeletefailure = "bulkdeletefailure";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";
                }

                ///<summary>
                /// 1:N - Relationship BusinessUnit_Campaigns
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_Campaigns
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class businessunit_campaigns
                {
                    public const string Name = "BusinessUnit_Campaigns";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_campaign = "campaign";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship BusinessUnit_DuplicateRules
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_DuplicateRules
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class businessunit_duplicaterules
                {
                    public const string Name = "BusinessUnit_DuplicateRules";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_duplicaterule = "duplicaterule";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship BusinessUnit_ImportData
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_ImportData
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class businessunit_importdata
                {
                    public const string Name = "BusinessUnit_ImportData";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_importdata = "importdata";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_data = "data";
                }

                ///<summary>
                /// 1:N - Relationship BusinessUnit_ImportFiles
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_ImportFiles
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class businessunit_importfiles
                {
                    public const string Name = "BusinessUnit_ImportFiles";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_importfile = "importfile";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship BusinessUnit_ImportLogs
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_ImportLogs
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class businessunit_importlogs
                {
                    public const string Name = "BusinessUnit_ImportLogs";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_importlog = "importlog";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";
                }

                ///<summary>
                /// 1:N - Relationship BusinessUnit_ImportMaps
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_ImportMaps
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class businessunit_importmaps
                {
                    public const string Name = "BusinessUnit_ImportMaps";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_importmap = "importmap";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship BusinessUnit_Imports
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_Imports
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class businessunit_imports
                {
                    public const string Name = "BusinessUnit_Imports";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_import = "import";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship businessunit_internal_addresses
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     businessunit_internal_addresses
                /// ReferencingEntityNavigationPropertyName    parentid_businessunit
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
                /// ReferencingEntity internaladdress:    PrimaryIdAttribute internaladdressid
                ///     DisplayName:
                ///         (English - United States - 1033): Internal Address
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): Internal Addresses
                ///     
                ///     Description:
                ///         (English - United States - 1033): Storage of addresses for a user, business unit, or site.
                ///</summary>
                public static partial class businessunit_internal_addresses
                {
                    public const string Name = "businessunit_internal_addresses";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_internaladdress = "internaladdress";

                    public const string ReferencingAttribute_parentid = "parentid";
                }

                ///<summary>
                /// 1:N - Relationship businessunit_mailboxtrackingcategory
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     businessunit_mailboxtrackingcategory
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class businessunit_mailboxtrackingcategory
                {
                    public const string Name = "businessunit_mailboxtrackingcategory";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_mailboxtrackingcategory = "mailboxtrackingcategory";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";
                }

                ///<summary>
                /// 1:N - Relationship businessunit_mailboxtrackingfolder
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     businessunit_mailboxtrackingfolder
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class businessunit_mailboxtrackingfolder
                {
                    public const string Name = "businessunit_mailboxtrackingfolder";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_mailboxtrackingfolder = "mailboxtrackingfolder";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";
                }

                ///<summary>
                /// 1:N - Relationship businessunit_principalobjectattributeaccess
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     businessunit_principalobjectattributeaccess
                /// ReferencingEntityNavigationPropertyName    objectid_businessunit
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
                public static partial class businessunit_principalobjectattributeaccess
                {
                    public const string Name = "businessunit_principalobjectattributeaccess";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_principalobjectattributeaccess = "principalobjectattributeaccess";

                    public const string ReferencingAttribute_objectid = "objectid";
                }

                ///<summary>
                /// 1:N - Relationship BusinessUnit_ProcessSessions
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_ProcessSessions
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_businessunit
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
                public static partial class businessunit_processsessions
                {
                    public const string Name = "BusinessUnit_ProcessSessions";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_processsession = "processsession";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship BusinessUnit_SyncError
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_SyncError
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class businessunit_syncerror
                {
                    public const string Name = "BusinessUnit_SyncError";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_syncerror = "syncerror";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship BusinessUnit_SyncErrors
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_SyncErrors
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_businessunit_syncerror
                /// IsCustomizable                             True
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              Append
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                NoCascade
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
                public static partial class businessunit_syncerrors
                {
                    public const string Name = "BusinessUnit_SyncErrors";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_syncerror = "syncerror";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship lk_userfiscalcalendar_businessunit
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_userfiscalcalendar_businessunit
                /// ReferencingEntityNavigationPropertyName    businessunitid_businessunit
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
                /// ReferencingEntity userfiscalcalendar:    PrimaryIdAttribute userfiscalcalendarid
                ///     DisplayName:
                ///         (English - United States - 1033): User Fiscal Calendar
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): User Fiscal Calendars
                ///     
                ///     Description:
                ///         (English - United States - 1033): Custom fiscal calendar used for tracking sales quotas.
                ///</summary>
                public static partial class lk_userfiscalcalendar_businessunit
                {
                    public const string Name = "lk_userfiscalcalendar_businessunit";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_userfiscalcalendar = "userfiscalcalendar";

                    public const string ReferencingAttribute_businessunitid = "businessunitid";
                }

                ///<summary>
                /// 1:N - Relationship Owning_businessunit_processsessions
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Owning_businessunit_processsessions
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class owning_businessunit_processsessions
                {
                    public const string Name = "Owning_businessunit_processsessions";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_processsession = "processsession";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship systemuserbusinessunitentitymap_businessunitid_businessunit
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     systemuserbusinessunitentitymap_businessunitid_businessunit
                /// ReferencingEntityNavigationPropertyName    businessunitid_businessunit
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
                /// ReferencingEntity systemuserbusinessunitentitymap:    PrimaryIdAttribute systemuserbusinessunitentitymapid
                ///     DisplayName:
                ///         (English - United States - 1033): SystemUser BusinessUnit Entity Map
                ///     
                ///     DisplayCollectionName:
                ///         (English - United States - 1033): SystemUserBusiness Unit Entity Maps
                ///     
                ///     Description:
                ///         (English - United States - 1033): Stores mapping attributes for business units.
                ///</summary>
                public static partial class systemuserbusinessunitentitymap_businessunitid_businessunit
                {
                    public const string Name = "systemuserbusinessunitentitymap_businessunitid_businessunit";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_systemuserbusinessunitentitymap = "systemuserbusinessunitentitymap";

                    public const string ReferencingAttribute_businessunitid = "businessunitid";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_businessunit
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_businessunit
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class userentityinstancedata_businessunit
                {
                    public const string Name = "userentityinstancedata_businessunit";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";
                }

                ///<summary>
                /// 1:N - Relationship userentityuisettings_businessunit
                /// 
                /// PropertyName                               Value
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityuisettings_businessunit
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                public static partial class userentityuisettings_businessunit
                {
                    public const string Name = "userentityuisettings_businessunit";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_userentityuisettings = "userentityuisettings";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}