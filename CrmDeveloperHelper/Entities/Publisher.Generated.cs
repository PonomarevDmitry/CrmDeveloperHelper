
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): Publisher
    /// (Russian - 1049): Издатель
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): Publishers
    /// (Russian - 1049): Издатели
    /// 
    /// Description:
    /// (English - United States - 1033): A publisher of a CRM solution.
    /// (Russian - 1049): Издатель решения по управлению отношениями с клиентами (CRM).
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
    /// CanModifyAdditionalSettings           True                 True
    /// CanTriggerWorkflow                    False
    /// ChangeTrackingEnabled                 False
    /// CollectionSchemaName                  Publishers
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         publishers
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False                True
    /// IsAvailableOffline                    True
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False                False
    /// IsCustomEntity                        False
    /// IsCustomizable                        True                 False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           True                 False
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
    /// IsValidForAdvancedFind                True
    /// IsValidForQueue                       False                False
    /// IsVisibleInMobile                     False                False
    /// IsVisibleInMobileClient               False                False
    /// LogicalCollectionName                 publishers
    /// LogicalName                           publisher
    /// ObjectTypeCode                        7101
    /// OwnershipType                         OrganizationOwned
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredPublisher
    /// SchemaName                            Publisher
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class Publisher
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "publisher";

            public const string EntitySchemaName = "Publisher";

            public const string EntityPrimaryNameAttribute = "friendlyname";

            public const string EntityPrimaryIdAttribute = "publisherid";

            #region Attributes.

            public static partial class Attributes
            {
                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: ID
                ///     (Russian - 1049): Адрес 1: идентификатор
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier for address 1.
                ///     (Russian - 1049): Уникальный идентификатор для адреса 1.
                /// 
                /// SchemaName: Address1_AddressId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string address1_addressid = "address1_addressid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Address Type
                ///     (Russian - 1049): Адрес 1: тип адреса
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of address for address 1, such as billing, shipping, or primary address.
                ///     (Russian - 1049): Тип адреса для адреса 1 (например, адрес для выставления счетов, адрес поставки или основной адрес).
                /// 
                /// SchemaName: Address1_AddressTypeCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet publisher_address1_addresstypecode
                /// DefaultFormValue = 1
                ///</summary>
                public const string address1_addresstypecode = "address1_addresstypecode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): City
                ///     (Russian - 1049): Город
                /// 
                /// Description:
                ///     (English - United States - 1033): City name for address 1.
                ///     (Russian - 1049): Город для адреса 1.
                /// 
                /// SchemaName: Address1_City
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 80
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address1_city = "address1_city";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Country/Region
                ///     (Russian - 1049): Страна или регион
                /// 
                /// Description:
                ///     (English - United States - 1033): Country/region name for address 1.
                ///     (Russian - 1049): Страна или регион для адреса 1.
                /// 
                /// SchemaName: Address1_Country
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 80
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address1_country = "address1_country";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: County
                ///     (Russian - 1049): Адрес 1: округ
                /// 
                /// Description:
                ///     (English - United States - 1033): County name for address 1.
                ///     (Russian - 1049): Округ для адреса 1.
                /// 
                /// SchemaName: Address1_County
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address1_county = "address1_county";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Fax
                ///     (Russian - 1049): Адрес 1: факс
                /// 
                /// Description:
                ///     (English - United States - 1033): Fax number for address 1.
                ///     (Russian - 1049): Номер факса для адреса 1.
                /// 
                /// SchemaName: Address1_Fax
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string address1_fax = "address1_fax";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Latitude
                ///     (Russian - 1049): Адрес 1: широта
                /// 
                /// Description:
                ///     (English - United States - 1033): Latitude for address 1.
                ///     (Russian - 1049): Широта для адреса 1.
                /// 
                /// SchemaName: Address1_Latitude
                /// DoubleAttributeMetadata    AttributeType: Double    AttributeTypeName: DoubleType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -90    MaxValue = 90    Precision = 5
                /// ImeMode = Disabled
                ///</summary>
                public const string address1_latitude = "address1_latitude";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Street 1
                ///     (Russian - 1049): Улица, дом (строка 1)
                /// 
                /// Description:
                ///     (English - United States - 1033): First line for entering address 1 information.
                ///     (Russian - 1049): Первая строка для ввода сведений об адресе 1.
                /// 
                /// SchemaName: Address1_Line1
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address1_line1 = "address1_line1";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Street 2
                ///     (Russian - 1049): Улица, дом (строка 2)
                /// 
                /// Description:
                ///     (English - United States - 1033): Second line for entering address 1 information.
                ///     (Russian - 1049): Вторая строка для ввода сведений об адресе 1.
                /// 
                /// SchemaName: Address1_Line2
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address1_line2 = "address1_line2";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Street 3
                ///     (Russian - 1049): Улица, дом (строка 3)
                /// 
                /// Description:
                ///     (English - United States - 1033): Third line for entering address 1 information.
                ///     (Russian - 1049): Третья строка для ввода сведений об адресе 1.
                /// 
                /// SchemaName: Address1_Line3
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address1_line3 = "address1_line3";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Longitude
                ///     (Russian - 1049): Адрес 1: долгота
                /// 
                /// Description:
                ///     (English - United States - 1033): Longitude for address 1.
                ///     (Russian - 1049): Долгота для адреса 1.
                /// 
                /// SchemaName: Address1_Longitude
                /// DoubleAttributeMetadata    AttributeType: Double    AttributeTypeName: DoubleType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -180    MaxValue = 180    Precision = 5
                /// ImeMode = Disabled
                ///</summary>
                public const string address1_longitude = "address1_longitude";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Name
                ///     (Russian - 1049): Адрес 1: название
                /// 
                /// Description:
                ///     (English - United States - 1033): Name to enter for address 1.
                ///     (Russian - 1049): Название, указываемое в адресе 1.
                /// 
                /// SchemaName: Address1_Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address1_name = "address1_name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): ZIP/Postal Code
                ///     (Russian - 1049): Почтовый индекс
                /// 
                /// Description:
                ///     (English - United States - 1033): ZIP Code or postal code for address 1.
                ///     (Russian - 1049): Почтовый индекс для адреса 1.
                /// 
                /// SchemaName: Address1_PostalCode
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string address1_postalcode = "address1_postalcode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Post Office Box
                ///     (Russian - 1049): Адрес 1: абонентский ящик
                /// 
                /// Description:
                ///     (English - United States - 1033): Post office box number for address 1.
                ///     (Russian - 1049): Номер абонентского ящика для адреса 1.
                /// 
                /// SchemaName: Address1_PostOfficeBox
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string address1_postofficebox = "address1_postofficebox";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Shipping Method
                ///     (Russian - 1049): Адрес 1: способ доставки
                /// 
                /// Description:
                ///     (English - United States - 1033): Method of shipment for address 1.
                ///     (Russian - 1049): Способ поставки для адреса 1.
                /// 
                /// SchemaName: Address1_ShippingMethodCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet publisher_address1_shippingmethodcode
                /// DefaultFormValue = 1
                ///</summary>
                public const string address1_shippingmethodcode = "address1_shippingmethodcode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): State/Province
                ///     (Russian - 1049): Область, край, республика
                /// 
                /// Description:
                ///     (English - United States - 1033): State or province for address 1.
                ///     (Russian - 1049): Область, республика, край, округ для адреса 1.
                /// 
                /// SchemaName: Address1_StateOrProvince
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address1_stateorprovince = "address1_stateorprovince";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Phone
                ///     (Russian - 1049): Телефон
                /// 
                /// Description:
                ///     (English - United States - 1033): First telephone number associated with address 1.
                ///     (Russian - 1049): Первый номер телефона, связанный с адресом 1.
                /// 
                /// SchemaName: Address1_Telephone1
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string address1_telephone1 = "address1_telephone1";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Telephone 2
                ///     (Russian - 1049): Адрес 1: телефон 2
                /// 
                /// Description:
                ///     (English - United States - 1033): Second telephone number associated with address 1.
                ///     (Russian - 1049): Второй номер телефона, связанный с адресом 1.
                /// 
                /// SchemaName: Address1_Telephone2
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string address1_telephone2 = "address1_telephone2";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Telephone 3
                ///     (Russian - 1049): Адрес 1: телефон 3
                /// 
                /// Description:
                ///     (English - United States - 1033): Third telephone number associated with address 1.
                ///     (Russian - 1049): Третий номер телефона, связанный с адресом 1.
                /// 
                /// SchemaName: Address1_Telephone3
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string address1_telephone3 = "address1_telephone3";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: UPS Zone
                ///     (Russian - 1049): Адрес 1: зона UPS
                /// 
                /// Description:
                ///     (English - United States - 1033): United Parcel Service (UPS) zone for address 1.
                ///     (Russian - 1049): Зона службы доставки UPS для адреса 1.
                /// 
                /// SchemaName: Address1_UPSZone
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 4
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string address1_upszone = "address1_upszone";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: UTC Offset
                ///     (Russian - 1049): Адрес 1: часовой пояс
                /// 
                /// Description:
                ///     (English - United States - 1033): UTC offset for address 1. This is the difference between local time and standard Coordinated Universal Time.
                ///     (Russian - 1049): Часовой пояс для адреса 1. Это разница между местным временем и временем в формате UTC.
                /// 
                /// SchemaName: Address1_UTCOffset
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -1500    MaxValue = 1500
                /// Format = TimeZone
                ///</summary>
                public const string address1_utcoffset = "address1_utcoffset";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: ID
                ///     (Russian - 1049): Адрес 2: идентификатор
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier for address 2.
                ///     (Russian - 1049): Уникальный идентификатор для адреса 2.
                /// 
                /// SchemaName: Address2_AddressId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string address2_addressid = "address2_addressid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Address Type
                ///     (Russian - 1049): Адрес 2: тип адреса
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of address for address 2. such as billing, shipping, or primary address.
                ///     (Russian - 1049): Тип адреса для адреса 2 (например, адрес для выставления счетов, адрес поставки или основной адрес).
                /// 
                /// SchemaName: Address2_AddressTypeCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet publisher_address2_addresstypecode
                /// DefaultFormValue = 1
                ///</summary>
                public const string address2_addresstypecode = "address2_addresstypecode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: City
                ///     (Russian - 1049): Адрес 2: город
                /// 
                /// Description:
                ///     (English - United States - 1033): City name for address 2.
                ///     (Russian - 1049): Город для адреса 2.
                /// 
                /// SchemaName: Address2_City
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 80
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address2_city = "address2_city";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Country/Region
                ///     (Russian - 1049): Адрес 2: страна
                /// 
                /// Description:
                ///     (English - United States - 1033): Country/region name for address 2.
                ///     (Russian - 1049): Страна или регион для адреса 2.
                /// 
                /// SchemaName: Address2_Country
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 80
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address2_country = "address2_country";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: County
                ///     (Russian - 1049): Адрес 2: округ
                /// 
                /// Description:
                ///     (English - United States - 1033): County name for address 2.
                ///     (Russian - 1049): Округ для адреса 2.
                /// 
                /// SchemaName: Address2_County
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address2_county = "address2_county";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Fax
                ///     (Russian - 1049): Адрес 2: факс
                /// 
                /// Description:
                ///     (English - United States - 1033): Fax number for address 2.
                ///     (Russian - 1049): Номер факса для адреса 2.
                /// 
                /// SchemaName: Address2_Fax
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string address2_fax = "address2_fax";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Latitude
                ///     (Russian - 1049): Адрес 2: широта
                /// 
                /// Description:
                ///     (English - United States - 1033): Latitude for address 2.
                ///     (Russian - 1049): Широта для адреса 2.
                /// 
                /// SchemaName: Address2_Latitude
                /// DoubleAttributeMetadata    AttributeType: Double    AttributeTypeName: DoubleType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -90    MaxValue = 90    Precision = 5
                /// ImeMode = Disabled
                ///</summary>
                public const string address2_latitude = "address2_latitude";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Street 1
                ///     (Russian - 1049): Адрес 2: улица, дом (строка 1)
                /// 
                /// Description:
                ///     (English - United States - 1033): First line for entering address 2 information.
                ///     (Russian - 1049): Первая строка для ввода сведений об адресе 2.
                /// 
                /// SchemaName: Address2_Line1
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address2_line1 = "address2_line1";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Street 2
                ///     (Russian - 1049): Адрес 2: улица, дом (строка 2)
                /// 
                /// Description:
                ///     (English - United States - 1033): Second line for entering address 2 information.
                ///     (Russian - 1049): Вторая строка для ввода сведений об адресе 2.
                /// 
                /// SchemaName: Address2_Line2
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address2_line2 = "address2_line2";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Street 3
                ///     (Russian - 1049): Адрес 2: улица, дом (строка 3)
                /// 
                /// Description:
                ///     (English - United States - 1033): Third line for entering address 2 information.
                ///     (Russian - 1049): Третья строка для ввода сведений об адресе 2.
                /// 
                /// SchemaName: Address2_Line3
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address2_line3 = "address2_line3";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Longitude
                ///     (Russian - 1049): Адрес 2: долгота
                /// 
                /// Description:
                ///     (English - United States - 1033): Longitude for address 2.
                ///     (Russian - 1049): Долгота для адреса 2.
                /// 
                /// SchemaName: Address2_Longitude
                /// DoubleAttributeMetadata    AttributeType: Double    AttributeTypeName: DoubleType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -180    MaxValue = 180    Precision = 5
                /// ImeMode = Disabled
                ///</summary>
                public const string address2_longitude = "address2_longitude";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Name
                ///     (Russian - 1049): Адрес 2: название
                /// 
                /// Description:
                ///     (English - United States - 1033): Name to enter for address 2.
                ///     (Russian - 1049): Название, указываемое в адресе 2.
                /// 
                /// SchemaName: Address2_Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address2_name = "address2_name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: ZIP/Postal Code
                ///     (Russian - 1049): Адрес 2: почтовый индекс
                /// 
                /// Description:
                ///     (English - United States - 1033): ZIP Code or postal code for address 2.
                ///     (Russian - 1049): Почтовый индекс для адреса 2.
                /// 
                /// SchemaName: Address2_PostalCode
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string address2_postalcode = "address2_postalcode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Post Office Box
                ///     (Russian - 1049): Адрес 2: абонентский ящик
                /// 
                /// Description:
                ///     (English - United States - 1033): Post office box number for address 2.
                ///     (Russian - 1049): Номер абонентского ящика для адреса 2.
                /// 
                /// SchemaName: Address2_PostOfficeBox
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string address2_postofficebox = "address2_postofficebox";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Shipping Method
                ///     (Russian - 1049): Адрес 2: способ поставки
                /// 
                /// Description:
                ///     (English - United States - 1033): Method of shipment for address 2.
                ///     (Russian - 1049): Способ поставки для адреса 2.
                /// 
                /// SchemaName: Address2_ShippingMethodCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet publisher_address2_shippingmethodcode
                /// DefaultFormValue = 1
                ///</summary>
                public const string address2_shippingmethodcode = "address2_shippingmethodcode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: State/Province
                ///     (Russian - 1049): Адрес 2: область, край, республика
                /// 
                /// Description:
                ///     (English - United States - 1033): State or province for address 2.
                ///     (Russian - 1049): Область, республика, край, округ для адреса 2.
                /// 
                /// SchemaName: Address2_StateOrProvince
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address2_stateorprovince = "address2_stateorprovince";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Telephone 1
                ///     (Russian - 1049): Адрес 2: телефон 1
                /// 
                /// Description:
                ///     (English - United States - 1033): First telephone number associated with address 2.
                ///     (Russian - 1049): Первый номер телефона, связанный с адресом 2.
                /// 
                /// SchemaName: Address2_Telephone1
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string address2_telephone1 = "address2_telephone1";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Telephone 2
                ///     (Russian - 1049): Адрес 2: телефон 2
                /// 
                /// Description:
                ///     (English - United States - 1033): Second telephone number associated with address 2.
                ///     (Russian - 1049): Второй номер телефона, связанный с адресом 2.
                /// 
                /// SchemaName: Address2_Telephone2
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string address2_telephone2 = "address2_telephone2";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Telephone 3
                ///     (Russian - 1049): Адрес 2: телефон 3
                /// 
                /// Description:
                ///     (English - United States - 1033): Third telephone number associated with address 2.
                ///     (Russian - 1049): Третий номер телефона, связанный с адресом 2.
                /// 
                /// SchemaName: Address2_Telephone3
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string address2_telephone3 = "address2_telephone3";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: UPS Zone
                ///     (Russian - 1049): Адрес 2: зона UPS
                /// 
                /// Description:
                ///     (English - United States - 1033): United Parcel Service (UPS) zone for address 2.
                ///     (Russian - 1049): Зона службы доставки UPS для адреса 2.
                /// 
                /// SchemaName: Address2_UPSZone
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 4
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string address2_upszone = "address2_upszone";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: UTC Offset
                ///     (Russian - 1049): Адрес 2: часовой пояс
                /// 
                /// Description:
                ///     (English - United States - 1033): UTC offset for address 2. This is the difference between local time and standard Coordinated Universal Time.
                ///     (Russian - 1049): Часовой пояс для адреса 2. Это разница между местным временем и временем в формате UTC.
                /// 
                /// SchemaName: Address2_UTCOffset
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -1500    MaxValue = 1500
                /// Format = TimeZone
                ///</summary>
                public const string address2_utcoffset = "address2_utcoffset";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By
                ///     (Russian - 1049): Создано
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who created the publisher.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, создавшего издателя.
                /// 
                /// SchemaName: CreatedBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///         (English - United States - 1033): User
                ///         (Russian - 1049): Пользователь
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///         (Russian - 1049): Пользователи
                ///         
                ///         Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///         (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public const string createdby = "createdby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created On
                ///     (Russian - 1049): Дата создания
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the publisher was created.
                ///     (Russian - 1049): Дата и время создания издателя.
                /// 
                /// SchemaName: CreatedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateAndTime
                ///</summary>
                public const string createdon = "createdon";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By (Delegate)
                ///     (Russian - 1049): Кем создано (делегат)
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the delegate user who created the publisher.
                ///     (Russian - 1049): Уникальный идентификатор пользователя-делегата, создавшего издателя.
                /// 
                /// SchemaName: CreatedOnBehalfBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///         (English - United States - 1033): User
                ///         (Russian - 1049): Пользователь
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///         (Russian - 1049): Пользователи
                ///         
                ///         Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///         (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public const string createdonbehalfby = "createdonbehalfby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Option Value Prefix
                ///     (Russian - 1049): Префикс значения параметра
                /// 
                /// Description:
                ///     (English - United States - 1033): Default option value prefix used for newly created options for solutions associated with this publisher.
                ///     (Russian - 1049): Префикс по умолчанию значения параметра для параметров, создаваемых для решений, связанных с этим издателем.
                /// 
                /// SchemaName: CustomizationOptionValuePrefix
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 10000    MaxValue = 99999
                /// Format = None
                ///</summary>
                public const string customizationoptionvalueprefix = "customizationoptionvalueprefix";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Prefix
                ///     (Russian - 1049): Префикс
                /// 
                /// Description:
                ///     (English - United States - 1033): Prefix used for new entities, attributes, and entity relationships for solutions associated with this publisher.
                ///     (Russian - 1049): Префикс, используемый для новых сущностей, атрибутов и отношений сущностей, создаваемых для решений, связанных с этим издателем.
                /// 
                /// SchemaName: CustomizationPrefix
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 8
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string customizationprefix = "customizationprefix";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Description
                ///     (Russian - 1049): Описание
                /// 
                /// Description:
                ///     (English - United States - 1033): Description of the solution.
                ///     (Russian - 1049): Описание решения.
                /// 
                /// SchemaName: Description
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 2000
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = True
                ///</summary>
                public const string description = "description";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Email
                ///     (Russian - 1049): Электронная почта
                /// 
                /// Description:
                ///     (English - United States - 1033): Email address for the publisher.
                ///     (Russian - 1049): Адрес электронной почты издателя.
                /// 
                /// SchemaName: EMailAddress
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Email    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string emailaddress = "emailaddress";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Entity Image Id
                ///     (Russian - 1049): Код изображения сущности
                /// 
                /// Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                /// 
                /// SchemaName: EntityImageId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string entityimageid = "entityimageid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Display Name
                ///     (Russian - 1049): Отображаемое имя
                /// 
                /// Description:
                ///     (English - United States - 1033): User display name for this publisher.
                ///     (Russian - 1049): Понятное имя данного издателя.
                /// 
                /// SchemaName: FriendlyName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = True
                ///</summary>
                public const string friendlyname = "friendlyname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Read-Only Publisher
                ///     (Russian - 1049): Является ли издателем только для чтения
                /// 
                /// Description:
                ///     (English - United States - 1033): Indicates whether the publisher was created as part of a managed solution installation.
                ///     (Russian - 1049): Указывает, был ли издатель создан как часть установки управляемого решения.
                /// 
                /// SchemaName: IsReadonly
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
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
                public const string isreadonly = "isreadonly";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By
                ///     (Russian - 1049): Изменено
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who last modified the publisher.
                ///     (Russian - 1049): Уникальный идентификатор последнего пользователя, изменившего издателя.
                /// 
                /// SchemaName: ModifiedBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///         (English - United States - 1033): User
                ///         (Russian - 1049): Пользователь
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///         (Russian - 1049): Пользователи
                ///         
                ///         Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///         (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public const string modifiedby = "modifiedby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified On
                ///     (Russian - 1049): Дата изменения
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time when the publisher was last modified.
                ///     (Russian - 1049): Дата и время последнего изменения издателя.
                /// 
                /// SchemaName: ModifiedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateAndTime
                ///</summary>
                public const string modifiedon = "modifiedon";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By (Delegate)
                ///     (Russian - 1049): Кем изменено (делегат)
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the delegate user who modified the publisher.
                ///     (Russian - 1049): Уникальный идентификатор пользователя-делегата, изменившего издатель.
                /// 
                /// SchemaName: ModifiedOnBehalfBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: systemuser
                /// 
                ///     Target systemuser    PrimaryIdAttribute systemuserid    PrimaryNameAttribute fullname
                ///         DisplayName:
                ///         (English - United States - 1033): User
                ///         (Russian - 1049): Пользователь
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Users
                ///         (Russian - 1049): Пользователи
                ///         
                ///         Description:
                ///         (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///         (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public const string modifiedonbehalfby = "modifiedonbehalfby";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Organization
                ///     (Russian - 1049): Предприятие
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the organization associated with the publisher.
                ///     (Russian - 1049): Уникальный идентификатор организации, связанной с издателем.
                /// 
                /// SchemaName: OrganizationId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: SystemRequired
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: organization
                /// 
                ///     Target organization    PrimaryIdAttribute organizationid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Organization
                ///         (Russian - 1049): Предприятие
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Organizations
                ///         (Russian - 1049): Предприятия
                ///         
                ///         Description:
                ///         (English - United States - 1033): Top level of the Microsoft Dynamics 365 business hierarchy. The organization can be a specific business, holding company, or corporation.
                ///         (Russian - 1049): Верхний уровень бизнес-иерархии Microsoft Dynamics 365. Организация может являться конкретной компанией, холдингом или корпорацией.
                ///</summary>
                public const string organizationid = "organizationid";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Default locale of the publisher in Microsoft Pinpoint.
                ///     (Russian - 1049): Локаль по умолчанию для издателя на сайте Microsoft Pinpoint.
                /// 
                /// SchemaName: PinpointPublisherDefaultLocale
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 16
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string pinpointpublisherdefaultlocale = "pinpointpublisherdefaultlocale";

                ///<summary>
                /// Description:
                ///     (English - United States - 1033): Identifier of the publisher in Microsoft Pinpoint.
                ///     (Russian - 1049): Идентификатор издателя на сайте Microsoft Pinpoint.
                /// 
                /// SchemaName: PinpointPublisherId
                /// BigIntAttributeMetadata    AttributeType: BigInt    AttributeTypeName: BigIntType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -9223372036854775808    MaxValue = 9223372036854775807
                ///</summary>
                public const string pinpointpublisherid = "pinpointpublisherid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Publisher Identifier
                ///     (Russian - 1049): Идентификатор издателя
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the publisher.
                ///     (Russian - 1049): Уникальный идентификатор издателя.
                /// 
                /// SchemaName: PublisherId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string publisherid = "publisherid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Website
                ///     (Russian - 1049): Веб-сайт
                /// 
                /// Description:
                ///     (English - United States - 1033): URL for the supporting website of this publisher.
                ///     (Russian - 1049): URL-адрес вспомогательного сайта для данного издателя.
                /// 
                /// SchemaName: SupportingWebsiteUrl
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Url    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string supportingwebsiteurl = "supportingwebsiteurl";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Name
                ///     (Russian - 1049): Имя
                /// 
                /// Description:
                ///     (English - United States - 1033): The unique name of this publisher.
                ///     (Russian - 1049): Уникальное название издателя.
                /// 
                /// SchemaName: UniqueName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 256
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string uniquename = "uniquename";

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

            #region OptionSets.

            public static partial class OptionSets
            {

                #region Picklist OptionSet OptionSets.
                ///<summary>
                /// Attribute: address1_addresstypecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Address Type
                ///     (Russian - 1049): Адрес 1: тип адреса
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of address for address 1, such as billing, shipping, or primary address.
                ///     (Russian - 1049): Тип адреса для адреса 1 (например, адрес для выставления счетов, адрес поставки или основной адрес).
                /// 
                /// Local System  OptionSet publisher_address1_addresstypecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Address Type
                ///     (Russian - 1049): Адрес 1: тип адреса
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of address for address 1, such as billing, shipping, or primary address.
                ///     (Russian - 1049): Тип адреса для адреса 1 (например, адрес для выставления счетов, адрес поставки или основной адрес).
                ///</summary>
                public enum address1_addresstypecode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Default Value
                    ///     (Russian - 1049): Значение по умолчанию
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Default_Value_1 = 1,
                }

                ///<summary>
                /// Attribute: address1_shippingmethodcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Shipping Method
                ///     (Russian - 1049): Адрес 1: способ доставки
                /// 
                /// Description:
                ///     (English - United States - 1033): Method of shipment for address 1.
                ///     (Russian - 1049): Способ поставки для адреса 1.
                /// 
                /// Local System  OptionSet publisher_address1_shippingmethodcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 1: Shipping Method 
                ///     (Russian - 1049): Адрес 1: способ доставки 
                /// 
                /// Description:
                ///     (English - United States - 1033): Method of shipment for address 1.
                ///     (Russian - 1049): Способ поставки для адреса 1.
                ///</summary>
                public enum address1_shippingmethodcode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Default Value
                    ///     (Russian - 1049): Значение по умолчанию
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Default_Value_1 = 1,
                }

                ///<summary>
                /// Attribute: address2_addresstypecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Address Type
                ///     (Russian - 1049): Адрес 2: тип адреса
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of address for address 2. such as billing, shipping, or primary address.
                ///     (Russian - 1049): Тип адреса для адреса 2 (например, адрес для выставления счетов, адрес поставки или основной адрес).
                /// 
                /// Local System  OptionSet publisher_address2_addresstypecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Address Type
                ///     (Russian - 1049): Адрес 2: тип адреса
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of address for address 2. such as billing, shipping, or primary address.
                ///     (Russian - 1049): Тип адреса для адреса 2 (например, адрес для выставления счетов, адрес поставки или основной адрес).
                ///</summary>
                public enum address2_addresstypecode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Default Value
                    ///     (Russian - 1049): Значение по умолчанию
                    ///</summary>
                    [System.Runtime.Serialization.EnumMemberAttribute()]
                    Default_Value_1 = 1,
                }

                ///<summary>
                /// Attribute: address2_shippingmethodcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Shipping Method
                ///     (Russian - 1049): Адрес 2: способ поставки
                /// 
                /// Description:
                ///     (English - United States - 1033): Method of shipment for address 2.
                ///     (Russian - 1049): Способ поставки для адреса 2.
                /// 
                /// Local System  OptionSet publisher_address2_shippingmethodcode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Shipping Method 
                ///     (Russian - 1049): Адрес 2: способ поставки 
                /// 
                /// Description:
                ///     (English - United States - 1033): Method of shipment for address 2.
                ///     (Russian - 1049): Способ поставки для адреса 2.
                ///</summary>
                public enum address2_shippingmethodcode
                {
                    ///<summary>
                    /// 1
                    /// DisplayOrder: 1
                    /// 
                    /// DisplayName:
                    ///     (English - United States - 1033): Default Value
                    ///     (Russian - 1049): Значение по умолчанию
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
                /// N:1 - Relationship lk_publisher_createdby
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_publisher_createdby
                /// ReferencingEntityNavigationPropertyName    createdby
                /// IsCustomizable                             False                     False
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
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity systemuser:
                ///     DisplayName:
                ///     (English - United States - 1033): User
                ///     (Russian - 1049): Пользователь
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Users
                ///     (Russian - 1049): Пользователи
                ///     
                ///     Description:
                ///     (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///     (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public static partial class lk_publisher_createdby
                {
                    public const string Name = "lk_publisher_createdby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_publisher = "publisher";

                    public const string ReferencingAttribute_createdby = "createdby";
                }

                ///<summary>
                /// N:1 - Relationship lk_publisher_entityimage
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_publisher_entityimage
                /// ReferencingEntityNavigationPropertyName    entityimageid_imagedescriptor
                /// IsCustomizable                             False                            False
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
                /// ReferencedEntity imagedescriptor:
                ///     DisplayName:
                ///     (English - United States - 1033): Image Descriptor
                ///     (Russian - 1049): Дескриптор изображения
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Image Descriptors
                ///     (Russian - 1049): Дескрипторы изображений
                ///     
                ///     Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
                ///</summary>
                public static partial class lk_publisher_entityimage
                {
                    public const string Name = "lk_publisher_entityimage";

                    public const string ReferencedEntity_imagedescriptor = "imagedescriptor";

                    public const string ReferencedAttribute_imagedescriptorid = "imagedescriptorid";

                    public const string ReferencingEntity_publisher = "publisher";

                    public const string ReferencingAttribute_entityimageid = "entityimageid";
                }

                ///<summary>
                /// N:1 - Relationship lk_publisher_modifiedby
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_publisher_modifiedby
                /// ReferencingEntityNavigationPropertyName    modifiedby
                /// IsCustomizable                             False                      False
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
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity systemuser:
                ///     DisplayName:
                ///     (English - United States - 1033): User
                ///     (Russian - 1049): Пользователь
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Users
                ///     (Russian - 1049): Пользователи
                ///     
                ///     Description:
                ///     (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///     (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public static partial class lk_publisher_modifiedby
                {
                    public const string Name = "lk_publisher_modifiedby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_publisher = "publisher";

                    public const string ReferencingAttribute_modifiedby = "modifiedby";
                }

                ///<summary>
                /// N:1 - Relationship lk_publisherbase_createdonbehalfby
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_publisherbase_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    createdonbehalfby
                /// IsCustomizable                             False                                 False
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
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity systemuser:
                ///     DisplayName:
                ///     (English - United States - 1033): User
                ///     (Russian - 1049): Пользователь
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Users
                ///     (Russian - 1049): Пользователи
                ///     
                ///     Description:
                ///     (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///     (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public static partial class lk_publisherbase_createdonbehalfby
                {
                    public const string Name = "lk_publisherbase_createdonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_publisher = "publisher";

                    public const string ReferencingAttribute_createdonbehalfby = "createdonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship lk_publisherbase_modifiedonbehalfby
                /// 
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_publisherbase_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    modifiedonbehalfby
                /// IsCustomizable                             False                                  False
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
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity systemuser:
                ///     DisplayName:
                ///     (English - United States - 1033): User
                ///     (Russian - 1049): Пользователь
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Users
                ///     (Russian - 1049): Пользователи
                ///     
                ///     Description:
                ///     (English - United States - 1033): Person with access to the Microsoft CRM system and who owns objects in the Microsoft CRM database.
                ///     (Russian - 1049): Пользователь, имеющий доступ к системе Microsoft CRM, которому принадлежат объекты в базе данных Microsoft CRM.
                ///</summary>
                public static partial class lk_publisherbase_modifiedonbehalfby
                {
                    public const string Name = "lk_publisherbase_modifiedonbehalfby";

                    public const string ReferencedEntity_systemuser = "systemuser";

                    public const string ReferencedAttribute_systemuserid = "systemuserid";

                    public const string ReferencedEntity_PrimaryNameAttribute_fullname = "fullname";

                    public const string ReferencingEntity_publisher = "publisher";

                    public const string ReferencingAttribute_modifiedonbehalfby = "modifiedonbehalfby";
                }

                ///<summary>
                /// N:1 - Relationship organization_publisher
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_publisher
                /// ReferencingEntityNavigationPropertyName    organizationid
                /// IsCustomizable                             False                     False
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
                /// ReferencedEntity organization:
                ///     DisplayName:
                ///     (English - United States - 1033): Organization
                ///     (Russian - 1049): Предприятие
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Organizations
                ///     (Russian - 1049): Предприятия
                ///     
                ///     Description:
                ///     (English - United States - 1033): Top level of the Microsoft Dynamics 365 business hierarchy. The organization can be a specific business, holding company, or corporation.
                ///     (Russian - 1049): Верхний уровень бизнес-иерархии Microsoft Dynamics 365. Организация может являться конкретной компанией, холдингом или корпорацией.
                ///</summary>
                public static partial class organization_publisher
                {
                    public const string Name = "organization_publisher";

                    public const string ReferencedEntity_organization = "organization";

                    public const string ReferencedAttribute_organizationid = "organizationid";

                    public const string ReferencedEntity_PrimaryNameAttribute_name = "name";

                    public const string ReferencingEntity_publisher = "publisher";

                    public const string ReferencingAttribute_organizationid = "organizationid";
                }
            }

            #endregion Relationship ManyToOne - N:1.

            #region Relationship OneToMany - 1:N.

            public static partial class OneToMany
            {
                ///<summary>
                /// 1:N - Relationship publisher_appmodule
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     publisher_appmodule
                /// ReferencingEntityNavigationPropertyName    publisher_appmodule_appmodule
                /// IsCustomizable                             False                            False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                Restrict
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity appmodule:
                ///     DisplayName:
                ///     (English - United States - 1033): App
                ///     (Russian - 1049): Приложение
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Apps
                ///     (Russian - 1049): Приложения
                ///     
                ///     Description:
                ///     (English - United States - 1033): To provide specific CRM UI context .For internal use only
                ///     (Russian - 1049): Для определения конкретного контекста пользовательского интерфейса CRM. Только для внутреннего использования
                ///</summary>
                public static partial class publisher_appmodule
                {
                    public const string Name = "publisher_appmodule";

                    public const string ReferencedEntity_publisher = "publisher";

                    public const string ReferencedAttribute_publisherid = "publisherid";

                    public const string ReferencingEntity_appmodule = "appmodule";

                    public const string ReferencingAttribute_publisherid = "publisherid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship Publisher_DuplicateBaseRecord
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Publisher_DuplicateBaseRecord
                /// ReferencingEntityNavigationPropertyName    baserecordid_publisher
                /// IsCustomizable                             False                            False
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
                /// ReferencingEntity duplicaterecord:
                ///     DisplayName:
                ///     (English - United States - 1033): Duplicate Record
                ///     (Russian - 1049): Повторяющаяся запись
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Duplicate Records
                ///     (Russian - 1049): Повторные записи
                ///     
                ///     Description:
                ///     (English - United States - 1033): Potential duplicate record.
                ///     (Russian - 1049): Возможная повторная запись.
                ///</summary>
                public static partial class publisher_duplicatebaserecord
                {
                    public const string Name = "Publisher_DuplicateBaseRecord";

                    public const string ReferencedEntity_publisher = "publisher";

                    public const string ReferencedAttribute_publisherid = "publisherid";

                    public const string ReferencingEntity_duplicaterecord = "duplicaterecord";

                    public const string ReferencingAttribute_baserecordid = "baserecordid";
                }

                ///<summary>
                /// 1:N - Relationship Publisher_DuplicateMatchingRecord
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Publisher_DuplicateMatchingRecord
                /// ReferencingEntityNavigationPropertyName    duplicaterecordid_publisher
                /// IsCustomizable                             False                                False
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
                /// ReferencingEntity duplicaterecord:
                ///     DisplayName:
                ///     (English - United States - 1033): Duplicate Record
                ///     (Russian - 1049): Повторяющаяся запись
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Duplicate Records
                ///     (Russian - 1049): Повторные записи
                ///     
                ///     Description:
                ///     (English - United States - 1033): Potential duplicate record.
                ///     (Russian - 1049): Возможная повторная запись.
                ///</summary>
                public static partial class publisher_duplicatematchingrecord
                {
                    public const string Name = "Publisher_DuplicateMatchingRecord";

                    public const string ReferencedEntity_publisher = "publisher";

                    public const string ReferencedAttribute_publisherid = "publisherid";

                    public const string ReferencingEntity_duplicaterecord = "duplicaterecord";

                    public const string ReferencingAttribute_duplicaterecordid = "duplicaterecordid";
                }

                ///<summary>
                /// 1:N - Relationship Publisher_PublisherAddress
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Publisher_PublisherAddress
                /// ReferencingEntityNavigationPropertyName    parentid
                /// IsCustomizable                             False                         False
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
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity publisheraddress:
                ///     DisplayName:
                ///     (English - United States - 1033): Publisher Address
                ///     (Russian - 1049): Адрес издателя
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Publisher Addresses
                ///     (Russian - 1049): Адреса издателя
                ///     
                ///     Description:
                ///     (English - United States - 1033): Address and shipping information. Used to store additional addresses for a publisher.
                ///     (Russian - 1049): Адрес и информация о поставке. Используется для хранения дополнительных адресов издателя.
                ///</summary>
                public static partial class publisher_publisheraddress
                {
                    public const string Name = "Publisher_PublisherAddress";

                    public const string ReferencedEntity_publisher = "publisher";

                    public const string ReferencedAttribute_publisherid = "publisherid";

                    public const string ReferencingEntity_publisheraddress = "publisheraddress";

                    public const string ReferencingAttribute_parentid = "parentid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship publisher_solution
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     publisher_solution
                /// ReferencingEntityNavigationPropertyName    publisherid
                /// IsCustomizable                             False                    False
                /// IsCustomRelationship                       False
                /// IsValidForAdvancedFind                     True
                /// RelationshipType                           OneToManyRelationship
                /// SecurityTypes                              None
                /// CascadeConfiguration.Assign                Cascade
                /// CascadeConfiguration.Delete                Restrict
                /// CascadeConfiguration.Merge                 NoCascade
                /// CascadeConfiguration.Reparent              Cascade
                /// CascadeConfiguration.Share                 Cascade
                /// CascadeConfiguration.Unshare               Cascade
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity solution:
                ///     DisplayName:
                ///     (English - United States - 1033): Solution
                ///     (Russian - 1049): Решение
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Solutions
                ///     (Russian - 1049): Решения
                ///     
                ///     Description:
                ///     (English - United States - 1033): A solution which contains CRM customizations.
                ///     (Russian - 1049): Решение, содержащее настройки CRM.
                ///</summary>
                public static partial class publisher_solution
                {
                    public const string Name = "publisher_solution";

                    public const string ReferencedEntity_publisher = "publisher";

                    public const string ReferencedAttribute_publisherid = "publisherid";

                    public const string ReferencingEntity_solution = "solution";

                    public const string ReferencingAttribute_publisherid = "publisherid";

                    public const string ReferencingEntity_PrimaryNameAttribute_friendlyname = "friendlyname";
                }

                ///<summary>
                /// 1:N - Relationship Publisher_SyncErrors
                /// 
                /// PropertyName                               Value                                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Publisher_SyncErrors
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_publisher_syncerror
                /// IsCustomizable                             True                                     False
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
                public static partial class publisher_syncerrors
                {
                    public const string Name = "Publisher_SyncErrors";

                    public const string ReferencedEntity_publisher = "publisher";

                    public const string ReferencedAttribute_publisherid = "publisherid";

                    public const string ReferencingEntity_syncerror = "syncerror";

                    public const string ReferencingAttribute_regardingobjectid = "regardingobjectid";

                    public const string ReferencingEntity_PrimaryNameAttribute_name = "name";
                }

                ///<summary>
                /// 1:N - Relationship userentityinstancedata_publisher
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_publisher
                /// ReferencingEntityNavigationPropertyName    objectid_publisher
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
                public static partial class userentityinstancedata_publisher
                {
                    public const string Name = "userentityinstancedata_publisher";

                    public const string ReferencedEntity_publisher = "publisher";

                    public const string ReferencedAttribute_publisherid = "publisherid";

                    public const string ReferencingEntity_userentityinstancedata = "userentityinstancedata";

                    public const string ReferencingAttribute_objectid = "objectid";
                }
            }

            #endregion Relationship OneToMany - 1:N.
        }
    }
}
