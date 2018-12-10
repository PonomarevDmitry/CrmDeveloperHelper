
namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    ///<summary>
    /// DisplayName:
    /// (English - United States - 1033): Business Unit
    /// (Russian - 1049): Подразделение
    /// 
    /// DisplayCollectionName:
    /// (English - United States - 1033): Business Units
    /// (Russian - 1049): Подразделения
    /// 
    /// Description:
    /// (English - United States - 1033): Business, division, or department in the Microsoft Dynamics 365 database.
    /// (Russian - 1049): Компания, подразделение или отдел в базе данных Microsoft Dynamics 365.
    /// 
    /// PropertyName                          Value                   CanBeChanged
    /// ActivityTypeMask                      0
    /// AutoCreateAccessTeams                 False
    /// AutoRouteToOwnerQueue                 False
    /// CanBeInManyToMany                     True                    False
    /// CanBePrimaryEntityInRelationship      True                    False
    /// CanBeRelatedEntityInRelationship      True                    False
    /// CanChangeHierarchicalRelationship     False                   False
    /// CanChangeTrackingBeEnabled            True                    True
    /// CanCreateAttributes                   True                    False
    /// CanCreateCharts                       False                   False
    /// CanCreateForms                        True                    False
    /// CanCreateViews                        True                    False
    /// CanEnableSyncToExternalSearchIndex    True                    True
    /// CanModifyAdditionalSettings           True                    True
    /// CanTriggerWorkflow                    True
    /// ChangeTrackingEnabled                 False
    /// CollectionSchemaName                  BusinessUnits
    /// DaysSinceRecordLastModified           0
    /// EnforceStateTransitions               False
    /// EntityColor
    /// EntityHelpUrl
    /// EntityHelpUrlEnabled                  False
    /// EntitySetName                         businessunits
    /// IconLargeName
    /// IconMediumName
    /// IconSmallName
    /// IsActivity                            False
    /// IsActivityParty                       False
    /// IsAIRUpdated                          False
    /// IsAuditEnabled                        False                   True
    /// IsAvailableOffline                    True
    /// IsBusinessProcessEnabled              False
    /// IsChildEntity                         False
    /// IsConnectionsEnabled                  False                   True
    /// IsCustomEntity                        False
    /// IsCustomizable                        True                    False
    /// IsDocumentManagementEnabled           False
    /// IsDuplicateDetectionEnabled           False                   True
    /// IsEnabledForCharts                    False
    /// IsEnabledForExternalChannels          False
    /// IsEnabledForTrace                     False
    /// IsImportable                          True
    /// IsInteractionCentricEnabled           False
    /// IsIntersect                           False
    /// IsKnowledgeManagementEnabled          False
    /// IsMailMergeEnabled                    False                   True
    /// IsMappable                            True                    False
    /// IsOfflineInMobileClient               False                   True
    /// IsOneNoteIntegrationEnabled           False
    /// IsOptimisticConcurrencyEnabled        True
    /// IsPrivate                             False
    /// IsQuickCreateEnabled                  False
    /// IsReadingPaneEnabled                  True
    /// IsReadOnlyInMobileClient              False                   False
    /// IsRenameable                          True                    False
    /// IsStateModelAware                     False
    /// IsValidForAdvancedFind                True
    /// IsValidForQueue                       False                   True
    /// IsVisibleInMobile                     False                   False
    /// IsVisibleInMobileClient               False                   False
    /// LogicalCollectionName                 businessunits
    /// LogicalName                           businessunit
    /// ObjectTypeCode                        10
    /// OwnershipType                         BusinessOwned
    /// RecurrenceBaseEntityLogicalName
    /// ReportViewName                        FilteredBusinessUnit
    /// SchemaName                            BusinessUnit
    /// SyncToExternalSearchIndex             False
    ///</summary>
    public partial class BusinessUnit
    {
        public static partial class Schema
        {
            public const string EntityLogicalName = "businessunit";

            public const string EntitySchemaName = "BusinessUnit";

            public const string EntityPrimaryNameAttribute = "name";

            public const string EntityPrimaryIdAttribute = "businessunitid";

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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet businessunit_address1_addresstypecode
                /// DefaultFormValue = 1
                ///</summary>
                public const string address1_addresstypecode = "address1_addresstypecode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Bill To City
                ///     (Russian - 1049): Город адреса для счета
                /// 
                /// Description:
                ///     (English - United States - 1033): City name for address 1.
                ///     (Russian - 1049): Город для адреса 1.
                /// 
                /// SchemaName: Address1_City
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 80
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address1_city = "address1_city";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Bill To Country/Region
                ///     (Russian - 1049): Страна адреса для счета
                /// 
                /// Description:
                ///     (English - United States - 1033): Country/region name for address 1.
                ///     (Russian - 1049): Страна или регион для адреса 1.
                /// 
                /// SchemaName: Address1_Country
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -90    MaxValue = 90    Precision = 5
                /// ImeMode = Disabled
                ///</summary>
                public const string address1_latitude = "address1_latitude";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Bill To Street 1
                ///     (Russian - 1049): Улица, дом адреса для счета (строка 1)
                /// 
                /// Description:
                ///     (English - United States - 1033): First line for entering address 1 information.
                ///     (Russian - 1049): Первая строка для ввода сведений об адресе 1.
                /// 
                /// SchemaName: Address1_Line1
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 250
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address1_line1 = "address1_line1";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Bill To Street 2
                ///     (Russian - 1049): Улица, дом адреса для счета (строка 2)
                /// 
                /// Description:
                ///     (English - United States - 1033): Second line for entering address 1 information.
                ///     (Russian - 1049): Вторая строка для ввода сведений об адресе 1.
                /// 
                /// SchemaName: Address1_Line2
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 250
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address1_line2 = "address1_line2";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Bill To Street 3
                ///     (Russian - 1049): Улица, дом адреса для счета (строка 3)
                /// 
                /// Description:
                ///     (English - United States - 1033): Third line for entering address 1 information.
                ///     (Russian - 1049): Третья строка для ввода сведений об адресе 1.
                /// 
                /// SchemaName: Address1_Line3
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 250
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address1_name = "address1_name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Bill To ZIP/Postal Code
                ///     (Russian - 1049): Почтовый индекс адреса для счета
                /// 
                /// Description:
                ///     (English - United States - 1033): ZIP Code or postal code for address 1.
                ///     (Russian - 1049): Почтовый индекс для адреса 1.
                /// 
                /// SchemaName: Address1_PostalCode
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet businessunit_address1_shippingmethodcode
                /// DefaultFormValue = 1
                ///</summary>
                public const string address1_shippingmethodcode = "address1_shippingmethodcode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Bill To State/Province
                ///     (Russian - 1049): Область, край, республика адреса для счета
                /// 
                /// Description:
                ///     (English - United States - 1033): State or province for address 1.
                ///     (Russian - 1049): Область, республика, край, округ для адреса 1.
                /// 
                /// SchemaName: Address1_StateOrProvince
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address1_stateorprovince = "address1_stateorprovince";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Main Phone
                ///     (Russian - 1049): Основной телефон
                /// 
                /// Description:
                ///     (English - United States - 1033): First telephone number associated with address 1.
                ///     (Russian - 1049): Первый номер телефона, связанный с адресом 1.
                /// 
                /// SchemaName: Address1_Telephone1
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 50
                /// Format = Text    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string address1_telephone1 = "address1_telephone1";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Other Phone
                ///     (Russian - 1049): Другой телефон
                /// 
                /// Description:
                ///     (English - United States - 1033): Second telephone number associated with address 1.
                ///     (Russian - 1049): Второй номер телефона, связанный с адресом 1.
                /// 
                /// SchemaName: Address1_Telephone2
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                ///     (English - United States - 1033): Type of address for address 2, such as billing, shipping, or primary address.
                ///     (Russian - 1049): Тип адреса для адреса 2 (например, адрес для выставления счетов, адрес поставки или основной адрес).
                /// 
                /// SchemaName: Address2_AddressTypeCode
                /// PicklistAttributeMetadata    AttributeType: Picklist    AttributeTypeName: PicklistType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet businessunit_address2_addresstypecode
                /// DefaultFormValue = 1
                ///</summary>
                public const string address2_addresstypecode = "address2_addresstypecode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Ship To City
                ///     (Russian - 1049): Город адреса доставки
                /// 
                /// Description:
                ///     (English - United States - 1033): City name for address 2.
                ///     (Russian - 1049): Город для адреса 2.
                /// 
                /// SchemaName: Address2_City
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 80
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address2_city = "address2_city";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Ship To Country/Region
                ///     (Russian - 1049): Страна адреса доставки
                /// 
                /// Description:
                ///     (English - United States - 1033): Country/region name for address 2.
                ///     (Russian - 1049): Страна или регион для адреса 2.
                /// 
                /// SchemaName: Address2_Country
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -90    MaxValue = 90    Precision = 5
                /// ImeMode = Disabled
                ///</summary>
                public const string address2_latitude = "address2_latitude";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Ship To Street 1
                ///     (Russian - 1049): Улица, дом адреса доставки (строка 1)
                /// 
                /// Description:
                ///     (English - United States - 1033): First line for entering address 2 information.
                ///     (Russian - 1049): Первая строка для ввода сведений об адресе 2.
                /// 
                /// SchemaName: Address2_Line1
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 250
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address2_line1 = "address2_line1";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Ship To Street 2
                ///     (Russian - 1049): Улица, дом адреса доставки (строка 2)
                /// 
                /// Description:
                ///     (English - United States - 1033): Second line for entering address 2 information.
                ///     (Russian - 1049): Вторая строка для ввода сведений об адресе 2.
                /// 
                /// SchemaName: Address2_Line2
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 250
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address2_line2 = "address2_line2";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Ship To Street 3
                ///     (Russian - 1049): Улица, дом адреса доставки (строка 3)
                /// 
                /// Description:
                ///     (English - United States - 1033): Third line for entering address 2 information.
                ///     (Russian - 1049): Третья строка для ввода сведений об адресе 2.
                /// 
                /// SchemaName: Address2_Line3
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 250
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Active    IsLocalizable = False
                ///</summary>
                public const string address2_name = "address2_name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Ship To ZIP/Postal Code
                ///     (Russian - 1049): Почтовый индекс адреса доставки
                /// 
                /// Description:
                ///     (English - United States - 1033): ZIP Code or postal code for address 2.
                ///     (Russian - 1049): Почтовый индекс для адреса 2.
                /// 
                /// SchemaName: Address2_PostalCode
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// Local System  OptionSet businessunit_address2_shippingmethodcode
                /// DefaultFormValue = 1
                ///</summary>
                public const string address2_shippingmethodcode = "address2_shippingmethodcode";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Ship To State/Province
                ///     (Russian - 1049): Область, край, республика адреса доставки
                /// 
                /// Description:
                ///     (English - United States - 1033): State or province for address 2.
                ///     (Russian - 1049): Область, республика, край, округ для адреса 2.
                /// 
                /// SchemaName: Address2_StateOrProvince
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
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
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: True    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -1500    MaxValue = 1500
                /// Format = TimeZone
                ///</summary>
                public const string address2_utcoffset = "address2_utcoffset";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Business Unit
                ///     (Russian - 1049): Подразделение
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the business unit.
                ///     (Russian - 1049): Уникальный идентификатор подразделения.
                /// 
                /// SchemaName: BusinessUnitId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string businessunitid = "businessunitid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Calendar
                ///     (Russian - 1049): Календарь
                /// 
                /// Description:
                ///     (English - United States - 1033): Fiscal calendar associated with the business unit.
                ///     (Russian - 1049): Финансовый календарь, связанный с подразделением.
                /// 
                /// SchemaName: CalendarId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: calendar
                /// 
                ///     Target calendar    PrimaryIdAttribute calendarid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Calendar
                ///         (Russian - 1049): Календарь
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Calendars
                ///         (Russian - 1049): Календари
                ///         
                ///         Description:
                ///         (English - United States - 1033): Calendar used by the scheduling system to define when an appointment or activity is to occur.
                ///         (Russian - 1049): Календарь, используемый системой планирования для определения времени проведения встречи или выполнения действия.
                ///</summary>
                public const string calendarid = "calendarid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Cost Center
                ///     (Russian - 1049): Центр затрат
                /// 
                /// Description:
                ///     (English - United States - 1033): Name of the business unit cost center.
                ///     (Russian - 1049): Название центра затрат подразделения.
                /// 
                /// SchemaName: CostCenter
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string costcenter = "costcenter";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Created By
                ///     (Russian - 1049): Создано
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who created the business unit.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, создавшего подразделение.
                /// 
                /// SchemaName: CreatedBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
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
                ///     (English - United States - 1033): Date and time when the business unit was created.
                ///     (Russian - 1049): Дата и время создания подразделения.
                /// 
                /// SchemaName: CreatedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who created the businessunit.
                ///     (Russian - 1049): Уникальный идентификатор делегата, создавшего businessunit.
                /// 
                /// SchemaName: CreatedOnBehalfBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
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
                ///     (English - United States - 1033): Credit Limit
                ///     (Russian - 1049): Кредитный лимит
                /// 
                /// Description:
                ///     (English - United States - 1033): Credit limit for the business unit.
                ///     (Russian - 1049): Лимит кредита подразделения.
                /// 
                /// SchemaName: CreditLimit
                /// DoubleAttributeMetadata    AttributeType: Double    AttributeTypeName: DoubleType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = 0    MaxValue = 1000000000    Precision = 2
                /// ImeMode = Disabled
                ///</summary>
                public const string creditlimit = "creditlimit";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Description
                ///     (Russian - 1049): Описание
                /// 
                /// Description:
                ///     (English - United States - 1033): Description of the business unit.
                ///     (Russian - 1049): Описание подразделения.
                /// 
                /// SchemaName: Description
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 2000
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string description = "description";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Disable Reason
                ///     (Russian - 1049): Причина отключения
                /// 
                /// Description:
                ///     (English - United States - 1033): Reason for disabling the business unit.
                ///     (Russian - 1049): Причина отключения подразделения.
                /// 
                /// SchemaName: DisabledReason
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 500
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string disabledreason = "disabledreason";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Division
                ///     (Russian - 1049): Отдел
                /// 
                /// Description:
                ///     (English - United States - 1033): Name of the division to which the business unit belongs.
                ///     (Russian - 1049): Название отделения, которому принадлежит подразделение.
                /// 
                /// SchemaName: DivisionName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string divisionname = "divisionname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Email
                ///     (Russian - 1049): Электронная почта
                /// 
                /// Description:
                ///     (English - United States - 1033): Email address for the business unit.
                ///     (Russian - 1049): Адрес электронной почты подразделения.
                /// 
                /// SchemaName: EMailAddress
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Email    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string emailaddress = "emailaddress";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Exchange Rate
                ///     (Russian - 1049): Валютный курс
                /// 
                /// Description:
                ///     (English - United States - 1033): Exchange rate for the currency associated with the businessunit with respect to the base currency.
                ///     (Russian - 1049): Валютный курс денежной единицы, связанной с businessunit, по отношению к базовой валюте.
                /// 
                /// SchemaName: ExchangeRate
                /// DecimalAttributeMetadata    AttributeType: Decimal    AttributeTypeName: DecimalType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0,0000000001    MaxValue = 100000000000    Precision = 10
                /// ImeMode = Disabled
                ///</summary>
                public const string exchangerate = "exchangerate";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): File as Name
                ///     (Russian - 1049): Сохранить как имя
                /// 
                /// Description:
                ///     (English - United States - 1033): Alternative name under which the business unit can be filed.
                ///     (Russian - 1049): Альтернативное название подразделения, под которым оно может проходить.
                /// 
                /// SchemaName: FileAsName
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 100
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string fileasname = "fileasname";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): FTP Site
                ///     (Russian - 1049): FTP-сайт
                /// 
                /// Description:
                ///     (English - United States - 1033): FTP site URL for the business unit.
                ///     (Russian - 1049): URL-адрес FTP-сайта подразделения.
                /// 
                /// SchemaName: FtpSiteUrl
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Url    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string ftpsiteurl = "ftpsiteurl";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Import Sequence Number
                ///     (Russian - 1049): Порядковый номер импорта
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the data import or data migration that created this record.
                ///     (Russian - 1049): Уникальный идентификатор импорта или переноса данных, создавшего эту запись.
                /// 
                /// SchemaName: ImportSequenceNumber
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -2147483648    MaxValue = 2147483647
                /// Format = None
                ///</summary>
                public const string importsequencenumber = "importsequencenumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Inheritance Mask
                ///     (Russian - 1049): Маска наследования
                /// 
                /// Description:
                ///     (English - United States - 1033): Inheritance mask for the business unit.
                ///     (Russian - 1049): Маска наследования подразделения.
                /// 
                /// SchemaName: InheritanceMask
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = 0    MaxValue = 1000000000
                /// Format = None
                ///</summary>
                public const string inheritancemask = "inheritancemask";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Is Disabled
                ///     (Russian - 1049): Отключен
                /// 
                /// Description:
                ///     (English - United States - 1033): Information about whether the business unit is enabled or disabled.
                ///     (Russian - 1049): Сведения о том, включено или отключено подразделение.
                /// 
                /// SchemaName: IsDisabled
                /// BooleanAttributeMetadata    AttributeType: Boolean    AttributeTypeName: BooleanType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
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
                public const string isdisabled = "isdisabled";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Modified By
                ///     (Russian - 1049): Изменено
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the user who last modified the business unit.
                ///     (Russian - 1049): Уникальный идентификатор пользователя, который последним изменил подразделение.
                /// 
                /// SchemaName: ModifiedBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
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
                ///     (English - United States - 1033): Date and time when the business unit was last modified.
                ///     (Russian - 1049): Дата и время последнего изменения подразделения.
                /// 
                /// SchemaName: ModifiedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
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
                ///     (English - United States - 1033): Unique identifier of the delegate user who last modified the businessunit.
                ///     (Russian - 1049): Уникальный идентификатор делегата, который последним изменил businessunit.
                /// 
                /// SchemaName: ModifiedOnBehalfBy
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
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
                ///     (English - United States - 1033): Name
                ///     (Russian - 1049): Имя
                /// 
                /// Description:
                ///     (English - United States - 1033): Name of the business unit.
                ///     (Russian - 1049): Название подразделения.
                /// 
                /// SchemaName: Name
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: SystemRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 160
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string name = "name";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Organization
                ///     (Russian - 1049): Предприятие
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the organization associated with the business unit.
                ///     (Russian - 1049): Уникальный идентификатор организации, связанной с этим подразделением.
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
                /// DisplayName:
                ///     (English - United States - 1033): Record Created On
                ///     (Russian - 1049): Дата создания записи
                /// 
                /// Description:
                ///     (English - United States - 1033): Date and time that the record was migrated.
                ///     (Russian - 1049): Дата и время переноса записи.
                /// 
                /// SchemaName: OverriddenCreatedOn
                /// DateTimeAttributeMetadata    AttributeType: DateTime    AttributeTypeName: DateTimeType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// DateTimeBehavior = UserLocal    CanChangeDateTimeBehavior = False
                /// ImeMode = Inactive    Format = DateOnly
                ///</summary>
                public const string overriddencreatedon = "overriddencreatedon";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Parent Business
                ///     (Russian - 1049): Головное подразделение
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier for the parent business unit.
                ///     (Russian - 1049): Уникальный идентификатор головного подразделения.
                /// 
                /// SchemaName: ParentBusinessUnitId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: ApplicationRequired
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: businessunit
                /// 
                ///     Target businessunit    PrimaryIdAttribute businessunitid    PrimaryNameAttribute name
                ///         DisplayName:
                ///         (English - United States - 1033): Business Unit
                ///         (Russian - 1049): Подразделение
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Business Units
                ///         (Russian - 1049): Подразделения
                ///         
                ///         Description:
                ///         (English - United States - 1033): Business, division, or department in the Microsoft Dynamics 365 database.
                ///         (Russian - 1049): Компания, подразделение или отдел в базе данных Microsoft Dynamics 365.
                ///</summary>
                public const string parentbusinessunitid = "parentbusinessunitid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Picture
                ///     (Russian - 1049): Рисунок
                /// 
                /// Description:
                ///     (English - United States - 1033): Picture or diagram of the business unit.
                ///     (Russian - 1049): Схема или диаграмма подразделения.
                /// 
                /// SchemaName: Picture
                /// MemoAttributeMetadata    AttributeType: Memo    AttributeTypeName: MemoType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MaxLength = 1073741823
                /// Format = TextArea    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string picture = "picture";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Stock Exchange
                ///     (Russian - 1049): Фондовая биржа
                /// 
                /// Description:
                ///     (English - United States - 1033): Stock exchange on which the business is listed.
                ///     (Russian - 1049): Фондовая биржа, на которой зарегистрирована компания.
                /// 
                /// SchemaName: StockExchange
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 20
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string stockexchange = "stockexchange";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Ticker Symbol
                ///     (Russian - 1049): Тикер
                /// 
                /// Description:
                ///     (English - United States - 1033): Stock exchange ticker symbol for the business unit.
                ///     (Russian - 1049): Символ подразделения на фондовой бирже.
                /// 
                /// SchemaName: TickerSymbol
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 10
                /// Format = Text    ImeMode = Auto    IsLocalizable = False
                ///</summary>
                public const string tickersymbol = "tickersymbol";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Currency
                ///     (Russian - 1049): Валюта
                /// 
                /// Description:
                ///     (English - United States - 1033): Unique identifier of the currency associated with the businessunit.
                ///     (Russian - 1049): Уникальный идентификатор валюты, связанной с businessunit.
                /// 
                /// SchemaName: TransactionCurrencyId
                /// LookupAttributeMetadata    AttributeType: Lookup    AttributeTypeName: LookupType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// Targets: transactioncurrency
                /// 
                ///     Target transactioncurrency    PrimaryIdAttribute transactioncurrencyid    PrimaryNameAttribute currencyname
                ///         DisplayName:
                ///         (English - United States - 1033): Currency
                ///         (Russian - 1049): Валюта
                ///         
                ///         DisplayCollectionName:
                ///         (English - United States - 1033): Currencies
                ///         (Russian - 1049): Валюты
                ///         
                ///         Description:
                ///         (English - United States - 1033): Currency in which a financial transaction is carried out.
                ///         (Russian - 1049): Валюта, в которой выполняется финансовая операция.
                ///</summary>
                public const string transactioncurrencyid = "transactioncurrencyid";

                ///<summary>
                /// SchemaName: UserGroupId
                /// AttributeMetadata    AttributeType: Uniqueidentifier    AttributeTypeName: UniqueidentifierType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: False    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                ///</summary>
                public const string usergroupid = "usergroupid";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): UTC Offset
                ///     (Russian - 1049): Часовой пояс
                /// 
                /// Description:
                ///     (English - United States - 1033): UTC offset for the business unit. This is the difference between local time and standard Coordinated Universal Time.
                ///     (Russian - 1049): Часовой пояс для подразделения. Это разница между местным временем и временем в формате UTC.
                /// 
                /// SchemaName: UTCOffset
                /// IntegerAttributeMetadata    AttributeType: Integer    AttributeTypeName: IntegerType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MinValue = -1500    MaxValue = 1500
                /// Format = TimeZone
                ///</summary>
                public const string utcoffset = "utcoffset";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Version number
                ///     (Russian - 1049): Номер версии
                /// 
                /// Description:
                ///     (English - United States - 1033): Version number of the business unit.
                ///     (Russian - 1049): Номер версии подразделения.
                /// 
                /// SchemaName: VersionNumber
                /// BigIntAttributeMetadata    AttributeType: BigInt    AttributeTypeName: BigIntType    RequiredLevel: None
                /// IsValidForCreate: False    IsValidForRead: True    IsValidForUpdate: False    IsValidForAdvancedFind: False    CanBeChanged = False
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: Simple
                /// MinValue = -9223372036854775808    MaxValue = 9223372036854775807
                ///</summary>
                public const string versionnumber = "versionnumber";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Website
                ///     (Russian - 1049): Веб-сайт
                /// 
                /// Description:
                ///     (English - United States - 1033): Website URL for the business unit.
                ///     (Russian - 1049): URL-адрес веб-сайта подразделения.
                /// 
                /// SchemaName: WebSiteUrl
                /// StringAttributeMetadata    AttributeType: String    AttributeTypeName: StringType    RequiredLevel: None
                /// IsValidForCreate: True    IsValidForRead: True    IsValidForUpdate: True    IsValidForAdvancedFind: True    CanBeChanged = True
                /// IsLogical: False    IsSecured: False    IsCustomAttribute: False    SourceType: 0
                /// MaxLength = 200
                /// Format = Url    ImeMode = Inactive    IsLocalizable = False
                ///</summary>
                public const string websiteurl = "websiteurl";

                ///<summary>
                /// DisplayName:
                ///     (English - United States - 1033): Workflow Suspended
                ///     (Russian - 1049): Бизнес-процесс приостановлен
                /// 
                /// Description:
                ///     (English - United States - 1033): Information about whether workflow or sales process rules have been suspended.
                ///     (Russian - 1049): Сведения о том, приостановлено ли действие правил бизнес-процессов или правил процесса продаж.
                /// 
                /// SchemaName: WorkflowSuspended
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
                public const string workflowsuspended = "workflowsuspended";
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
                /// Local System  OptionSet businessunit_address1_addresstypecode
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
                /// Local System  OptionSet businessunit_address1_shippingmethodcode
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
                ///     (English - United States - 1033): Type of address for address 2, such as billing, shipping, or primary address.
                ///     (Russian - 1049): Тип адреса для адреса 2 (например, адрес для выставления счетов, адрес поставки или основной адрес).
                /// 
                /// Local System  OptionSet businessunit_address2_addresstypecode
                /// 
                /// DisplayName:
                ///     (English - United States - 1033): Address 2: Address Type
                ///     (Russian - 1049): Адрес 2: тип адреса
                /// 
                /// Description:
                ///     (English - United States - 1033): Type of address for address 2, such as billing, shipping, or primary address.
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
                /// Local System  OptionSet businessunit_address2_shippingmethodcode
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
                /// N:1 - Relationship business_unit_parent_business_unit
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_parent_business_unit
                /// ReferencingEntityNavigationPropertyName    parentbusinessunitid
                /// IsCustomizable                             True                                  False
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
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_Calendar
                /// ReferencingEntityNavigationPropertyName    calendarid
                /// IsCustomizable                             False                    False
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
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity calendar:
                ///     DisplayName:
                ///     (English - United States - 1033): Calendar
                ///     (Russian - 1049): Календарь
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Calendars
                ///     (Russian - 1049): Календари
                ///     
                ///     Description:
                ///     (English - United States - 1033): Calendar used by the scheduling system to define when an appointment or activity is to occur.
                ///     (Russian - 1049): Календарь, используемый системой планирования для определения времени проведения встречи или выполнения действия.
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
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_businessunit_createdonbehalfby
                /// ReferencingEntityNavigationPropertyName    createdonbehalfby
                /// IsCustomizable                             True                                 False
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
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_businessunit_modifiedonbehalfby
                /// ReferencingEntityNavigationPropertyName    modifiedonbehalfby
                /// IsCustomizable                             True                                  False
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
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_businessunitbase_createdby
                /// ReferencingEntityNavigationPropertyName    createdby
                /// IsCustomizable                             True                             False
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
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_businessunitbase_modifiedby
                /// ReferencingEntityNavigationPropertyName    modifiedby
                /// IsCustomizable                             True                              False
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
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     organization_business_units
                /// ReferencingEntityNavigationPropertyName    organizationid
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
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     TransactionCurrency_BusinessUnit
                /// ReferencingEntityNavigationPropertyName    transactioncurrencyid
                /// IsCustomizable                             False                               False
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
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencedEntity transactioncurrency:
                ///     DisplayName:
                ///     (English - United States - 1033): Currency
                ///     (Russian - 1049): Валюта
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Currencies
                ///     (Russian - 1049): Валюты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Currency in which a financial transaction is carried out.
                ///     (Russian - 1049): Валюта, в которой выполняется финансовая операция.
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
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     actioncardusersettings_businessunit
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                                  False
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
                /// ReferencingEntity actioncardusersettings:
                ///     DisplayName:
                ///     (English - United States - 1033): Action Card User Settings
                ///     (Russian - 1049): Параметры пользователя карточки действия
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Stores user settings for action cards
                ///     (Russian - 1049): Хранит параметры пользователя карточек действий
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
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     bizmap_businessid_businessunit
                /// ReferencingEntityNavigationPropertyName    businessid_businessunit
                /// IsCustomizable                             False                             False
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
                /// ReferencingEntity businessunitmap:
                ///     DisplayName:
                ///     (English - United States - 1033): Business Unit Map
                ///     (Russian - 1049): Сопоставление подразделения
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Business Unit Maps
                ///     (Russian - 1049): Схемы подразделений
                ///     
                ///     Description:
                ///     (English - United States - 1033): Stores mapping attributes for business units.
                ///     (Russian - 1049): Хранит атрибуты сопоставления для подразделений.
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
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     bizmap_subbusinessid_businessunit
                /// ReferencingEntityNavigationPropertyName    subbusinessid_businessunit
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
                /// ReferencingEntity businessunitmap:
                ///     DisplayName:
                ///     (English - United States - 1033): Business Unit Map
                ///     (Russian - 1049): Сопоставление подразделения
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Business Unit Maps
                ///     (Russian - 1049): Схемы подразделений
                ///     
                ///     Description:
                ///     (English - United States - 1033): Stores mapping attributes for business units.
                ///     (Russian - 1049): Хранит атрибуты сопоставления для подразделений.
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
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BulkDeleteOperation_BusinessUnit
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                               False
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
                /// ReferencingEntity bulkdeleteoperation:
                ///     DisplayName:
                ///     (English - United States - 1033): Bulk Delete Operation
                ///     (Russian - 1049): Операция группового удаления
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Bulk Delete Operations
                ///     (Russian - 1049): Операции группового удаления
                ///     
                ///     Description:
                ///     (English - United States - 1033): User-submitted bulk deletion job.
                ///     (Russian - 1049): Задание группового удаления, отправленное пользователем.
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
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_customer_opportunity_roles
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                                  False
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
                /// ReferencingEntity customeropportunityrole:
                ///     DisplayName:
                ///     (English - United States - 1033): Opportunity Relationship
                ///     (Russian - 1049): Отношение возможной сделки
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Opportunity Relationships
                ///     (Russian - 1049): Отношения возможных сделок
                ///     
                ///     Description:
                ///     (English - United States - 1033): Relationship between an account or contact and an opportunity.
                ///     (Russian - 1049): Отношение между организацией или контактом и возможной сделкой.
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
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_accounts
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                      False
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
                /// ReferencingEntity account:
                ///     DisplayName:
                ///     (English - United States - 1033): Account
                ///     (Russian - 1049): Организация
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Accounts
                ///     (Russian - 1049): Организации
                ///     
                ///     Description:
                ///     (English - United States - 1033): Business that represents a customer or potential customer. The company that is billed in business transactions.
                ///     (Russian - 1049): Компания, представляющая существующего или потенциального клиента. Компания, которой выставляется счет в деловых транзакциях.
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
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_actioncards
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                        False
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
                /// ReferencingEntity actioncard:
                ///     DisplayName:
                ///     (English - United States - 1033): Action Card
                ///     (Russian - 1049): Карточка действия
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Action Cards
                ///     (Russian - 1049): Карточки действий
                ///     
                ///     Description:
                ///     (English - United States - 1033): Action card entity to show action cards.
                ///     (Russian - 1049): Сущность карточки действия для отображения карточек действий.
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
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_activitypointer
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// ReferencingEntity activitypointer:
                ///     DisplayName:
                ///     (English - United States - 1033): Activity
                ///     (Russian - 1049): Действие
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Activities
                ///     (Russian - 1049): Действия
                ///     
                ///     Description:
                ///     (English - United States - 1033): Task performed, or to be performed, by a user. An activity is any action for which an entry can be made on a calendar.
                ///     (Russian - 1049): Выполненная пользователем задача или задача, которую пользователь должен выполнить. "Действие" — это любое действие, для которого в календаре можно создать запись.
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
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_annotations
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                        False
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
                /// ReferencingEntity annotation:
                ///     DisplayName:
                ///     (English - United States - 1033): Note
                ///     (Russian - 1049): Примечание
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Notes
                ///     (Russian - 1049): Примечания
                ///     
                ///     Description:
                ///     (English - United States - 1033): Note that is attached to one or more objects, including other notes.
                ///     (Russian - 1049): Примечание, которое прикреплено к одному или нескольким объектам, в том числе другим примечаниям.
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
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_appointment_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_appointment
                /// IsCustomizable                             True                                    False
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
                /// ReferencingEntity appointment:
                ///     DisplayName:
                ///     (English - United States - 1033): Appointment
                ///     (Russian - 1049): Встреча
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Appointments
                ///     (Russian - 1049): Встречи
                ///     
                ///     Description:
                ///     (English - United States - 1033): Commitment representing a time interval with start/end times and duration.
                ///     (Russian - 1049): Обязательство, представляющее временной интервал с временем начала и окончания, а также длительностью.
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
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_asyncoperation
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                           False
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
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_bookableresource
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                              False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity bookableresource:
                ///     DisplayName:
                ///     (English - United States - 1033): Bookable Resource
                ///     (Russian - 1049): Резервируемый ресурс
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Bookable Resources
                ///     (Russian - 1049): Резервируемые ресурсы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Resource that has capacity which can be allocated to work.
                ///     (Russian - 1049): Ресурс, имеющий производительность, которую можно назначить работе.
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
                /// PropertyName                               Value                                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_bookableresourcebooking
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                     False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity bookableresourcebooking:
                ///     DisplayName:
                ///     (English - United States - 1033): Bookable Resource Booking
                ///     (Russian - 1049): Резервирование резервируемого ресурса
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Bookable Resource Bookings
                ///     (Russian - 1049): Резервирования резервируемого ресурса
                ///     
                ///     Description:
                ///     (English - United States - 1033): Represents the line details of a resource booking.
                ///     (Russian - 1049): Представляет сведения строки в резервировании ресурса.
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
                /// PropertyName                               Value                                                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_bookableresourcebookingexchangesyncidmapping
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                                                         True
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity bookableresourcebookingexchangesyncidmapping:
                ///     DisplayName:
                ///     (English - United States - 1033): BookableResourceBooking to Exchange Id Mapping
                ///     (Russian - 1049): Сопоставление BookableResourceBooking с идентификатором Exchange
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): BookableResourceBooking to Exchange Id Mappings
                ///     (Russian - 1049): Сопоставления BookableResourceBooking с идентификатором Exchange
                ///     
                ///     Description:
                ///     (English - United States - 1033): The mapping used to keep track of the IDs for items synced between CRM BookableResourceBooking and Exchange.
                ///     (Russian - 1049): Сопоставление, используемое для отслеживания идентификаторов элементов, синхронизируемых между BookableResourceBooking CRM и Exchange.
                ///</summary>
                public static partial class business_unit_bookableresourcebookingexchangesyncidmapping
                {
                    public const string Name = "business_unit_bookableresourcebookingexchangesyncidmapping";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_bookableresourcebookingexchangesyncidmapping = "bookableresourcebookingexchangesyncidmapping";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_bookableresourcebookingheader
                /// 
                /// PropertyName                               Value                                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_bookableresourcebookingheader
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                           False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity bookableresourcebookingheader:
                ///     DisplayName:
                ///     (English - United States - 1033): Bookable Resource Booking Header
                ///     (Russian - 1049): Заголовок резервирования резервируемого ресурса
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Bookable Resource Booking Headers
                ///     (Russian - 1049): Заголовки резервирования резервируемого ресурса
                ///     
                ///     Description:
                ///     (English - United States - 1033): Reservation entity representing the summary of the associated resource bookings.
                ///     (Russian - 1049): Сущность резервирования, представляющая сводку связанных резервирований ресурсов.
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
                /// PropertyName                               Value                                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_bookableresourcecategory
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                      False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity bookableresourcecategory:
                ///     DisplayName:
                ///     (English - United States - 1033): Bookable Resource Category
                ///     (Russian - 1049): Категория резервируемого ресурса
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Bookable Resource Categories
                ///     (Russian - 1049): Категории резервируемого ресурса
                ///     
                ///     Description:
                ///     (English - United States - 1033): Categorize resources that have capacity into categories such as roles.
                ///     (Russian - 1049): Распределите ресурсы, имеющие производительность по категориям, например по ролям.
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
                /// PropertyName                               Value                                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_bookableresourcecategoryassn
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                          False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity bookableresourcecategoryassn:
                ///     DisplayName:
                ///     (English - United States - 1033): Bookable Resource Category Assn
                ///     (Russian - 1049): Связь категории резервируемого ресурса
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Bookable Resource Category Assns
                ///     (Russian - 1049): Связи категории резервируемого ресурса
                ///     
                ///     Description:
                ///     (English - United States - 1033): Association entity to model the categorization of resources.
                ///     (Russian - 1049): Сущность связей для моделирования категоризации ресурсов.
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
                /// PropertyName                               Value                                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_bookableresourcecharacteristic
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                            False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity bookableresourcecharacteristic:
                ///     DisplayName:
                ///     (English - United States - 1033): Bookable Resource Characteristic
                ///     (Russian - 1049): Характеристика резервируемого ресурса
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Bookable Resource Characteristics
                ///     (Russian - 1049): Характеристики резервируемого ресурса
                ///     
                ///     Description:
                ///     (English - United States - 1033): Associates resources with their characteristics and specifies the proficiency level of a resource for that characteristic.
                ///     (Russian - 1049): Связывает ресурсы с их характеристиками и задает уровень квалификации ресурса для этой характеристики.
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
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_bookableresourcegroup
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                   False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity bookableresourcegroup:
                ///     DisplayName:
                ///     (English - United States - 1033): Bookable Resource Group
                ///     (Russian - 1049): Группа резервируемых ресурсов
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Bookable Resource Groups
                ///     (Russian - 1049): Группы резервируемых ресурсов
                ///     
                ///     Description:
                ///     (English - United States - 1033): Associates resources with resource groups that they are a member of.
                ///     (Russian - 1049): Связывает ресурсы c группами ресурсов, в которые они входят.
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
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_bookingstatus
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                           False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity bookingstatus:
                ///     DisplayName:
                ///     (English - United States - 1033): Booking Status
                ///     (Russian - 1049): Состояние резервирования
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Booking Statuses
                ///     (Russian - 1049): Состояния резервирования
                ///     
                ///     Description:
                ///     (English - United States - 1033): Allows creation of multiple sub statuses mapped to a booking status option.
                ///     (Russian - 1049): Позволяет создавать несколько вложенных состояний, сопоставляемых варианту состояния резервирования.
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
                /// PropertyName                               Value                                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_BulkOperation_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_bulkoperation
                /// IsCustomizable                             False                                     False
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
                /// ReferencingEntity bulkoperation:
                ///     DisplayName:
                ///     (English - United States - 1033): Quick Campaign
                ///     (Russian - 1049): Быстрая кампания
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Quick Campaigns
                ///     (Russian - 1049): Быстрые кампании
                ///     
                ///     Description:
                ///     (English - United States - 1033): System operation used to perform lengthy and asynchronous operations on large data sets, such as distributing a campaign activity or quick campaign.
                ///     (Russian - 1049): Системная операция, которая выполняла длительные и асинхронные операции над крупными наборами данных (например, распространение действия кампании и быстрой кампании).
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
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_calendars
                /// ReferencingEntityNavigationPropertyName    businessunitid
                /// IsCustomizable                             False                      False
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
                /// ReferencingEntity calendar:
                ///     DisplayName:
                ///     (English - United States - 1033): Calendar
                ///     (Russian - 1049): Календарь
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Calendars
                ///     (Russian - 1049): Календари
                ///     
                ///     Description:
                ///     (English - United States - 1033): Calendar used by the scheduling system to define when an appointment or activity is to occur.
                ///     (Russian - 1049): Календарь, используемый системой планирования для определения времени проведения встречи или выполнения действия.
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
                /// PropertyName                               Value                                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_campaignactivity_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_campaignactivity
                /// IsCustomizable                             True                                         False
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
                /// ReferencingEntity campaignactivity:
                ///     DisplayName:
                ///     (English - United States - 1033): Campaign Activity
                ///     (Russian - 1049): Действие кампании
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Campaign Activities
                ///     (Russian - 1049): Действия кампаний
                ///     
                ///     Description:
                ///     (English - United States - 1033): Task performed, or to be performed, by a user for planning or running a campaign.
                ///     (Russian - 1049): Выполненная пользователем задача или задача, которую пользователь должен выполнить для планирования или проведения кампании.
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
                /// PropertyName                               Value                                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_campaignresponse_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_campaignresponse
                /// IsCustomizable                             True                                         False
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
                /// ReferencingEntity campaignresponse:
                ///     DisplayName:
                ///     (English - United States - 1033): Campaign Response
                ///     (Russian - 1049): Отклик от кампании
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Campaign Responses
                ///     (Russian - 1049): Отклики от кампании
                ///     
                ///     Description:
                ///     (English - United States - 1033): Response from an existing or a potential new customer for a campaign.
                ///     (Russian - 1049): Отклик существующего или потенциального клиента от кампании.
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
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_category
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                      False
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
                /// ReferencingEntity category:
                ///     DisplayName:
                ///     (English - United States - 1033): Category
                ///     (Russian - 1049): Категория
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Categories
                ///     (Russian - 1049): Категории
                ///     
                ///     Description:
                ///     (English - United States - 1033): Entity for categorizing records to make it easier for your customers to find them on portals and through search.
                ///     (Russian - 1049): Сущность для распределения записей по категориям, чтобы клиентам было проще находить записи на порталах и с помощью функции поиска.
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
                /// 1:N - Relationship business_unit_cdi_anonymousvisitor
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_anonymousvisitor
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                  True
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
                /// ReferencingEntity cdi_anonymousvisitor:
                ///     DisplayName:
                ///     (English - United States - 1033): Anonymous Visitor
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Anonymous Visitors
                ///</summary>
                public static partial class business_unit_cdi_anonymousvisitor
                {
                    public const string Name = "business_unit_cdi_anonymousvisitor";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_anonymousvisitor = "cdi_anonymousvisitor";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_automation
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_automation
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                            True
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
                /// ReferencingEntity cdi_automation:
                ///     DisplayName:
                ///     (English - United States - 1033): Campaign Automation
                ///     
                ///     DisplayCollectionName:
                ///</summary>
                public static partial class business_unit_cdi_automation
                {
                    public const string Name = "business_unit_cdi_automation";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_automation = "cdi_automation";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_bulktxtmessage
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_bulktxtmessage
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                True
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
                /// ReferencingEntity cdi_bulktxtmessage:
                ///     DisplayName:
                ///     (English - United States - 1033): Bulk Text Message
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Bulk Text Messages
                ///</summary>
                public static partial class business_unit_cdi_bulktxtmessage
                {
                    public const string Name = "business_unit_cdi_bulktxtmessage";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_bulktxtmessage = "cdi_bulktxtmessage";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_subject = "cdi_subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_category
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_category
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                          True
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
                /// ReferencingEntity cdi_category:
                ///     DisplayName:
                ///     (English - United States - 1033): Content Category
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Content Categories
                ///</summary>
                public static partial class business_unit_cdi_category
                {
                    public const string Name = "business_unit_cdi_category";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_category = "cdi_category";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_datasync
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_datasync
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                          True
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
                /// ReferencingEntity cdi_datasync:
                ///     DisplayName:
                ///     (English - United States - 1033): Profile Management Data Sync
                ///     
                ///     DisplayCollectionName:
                ///</summary>
                public static partial class business_unit_cdi_datasync
                {
                    public const string Name = "business_unit_cdi_datasync";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_datasync = "cdi_datasync";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_emailcname
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_emailcname
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                            True
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
                /// ReferencingEntity cdi_emailcname:
                ///     DisplayName:
                ///     (English - United States - 1033): Email CNAME
                ///     
                ///     DisplayCollectionName:
                ///</summary>
                public static partial class business_unit_cdi_emailcname
                {
                    public const string Name = "business_unit_cdi_emailcname";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_emailcname = "cdi_emailcname";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_emailevent
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_emailevent
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                            True
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
                /// ReferencingEntity cdi_emailevent:
                ///     DisplayName:
                ///     (English - United States - 1033): Email Event
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Email Events
                ///</summary>
                public static partial class business_unit_cdi_emailevent
                {
                    public const string Name = "business_unit_cdi_emailevent";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_emailevent = "cdi_emailevent";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_time = "cdi_time";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_emailsend
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_emailsend
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                           True
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
                /// ReferencingEntity cdi_emailsend:
                ///     DisplayName:
                ///     (English - United States - 1033): Email Send
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Email Sends
                ///</summary>
                public static partial class business_unit_cdi_emailsend
                {
                    public const string Name = "business_unit_cdi_emailsend";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_emailsend = "cdi_emailsend";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_subject = "cdi_subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_emailstatistics
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_emailstatistics
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                 True
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
                /// ReferencingEntity cdi_emailstatistics:
                ///     DisplayName:
                ///     (English - United States - 1033): Email Statistics
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Used to give permissions on the Email Statistics button on the Email Send entity.
                ///</summary>
                public static partial class business_unit_cdi_emailstatistics
                {
                    public const string Name = "business_unit_cdi_emailstatistics";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_emailstatistics = "cdi_emailstatistics";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_emailtemplate
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_emailtemplate
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                               True
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
                /// ReferencingEntity cdi_emailtemplate:
                ///     DisplayName:
                ///     (English - United States - 1033): Email Template
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Email Templates
                ///</summary>
                public static partial class business_unit_cdi_emailtemplate
                {
                    public const string Name = "business_unit_cdi_emailtemplate";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_emailtemplate = "cdi_emailtemplate";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_event
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_event
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                       True
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
                /// ReferencingEntity cdi_event:
                ///     DisplayName:
                ///     (English - United States - 1033): Event
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Events
                ///</summary>
                public static partial class business_unit_cdi_event
                {
                    public const string Name = "business_unit_cdi_event";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_event = "cdi_event";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_eventparticipation
                /// 
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_eventparticipation
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                    True
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
                /// ReferencingEntity cdi_eventparticipation:
                ///     DisplayName:
                ///     (English - United States - 1033): Event Participation
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Event Participations
                ///</summary>
                public static partial class business_unit_cdi_eventparticipation
                {
                    public const string Name = "business_unit_cdi_eventparticipation";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_eventparticipation = "cdi_eventparticipation";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_firstname = "cdi_firstname";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_excludedemail
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_excludedemail
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                               True
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
                /// ReferencingEntity cdi_excludedemail:
                ///     DisplayName:
                ///     (English - United States - 1033): Excluded Email
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Excluded Emails
                ///</summary>
                public static partial class business_unit_cdi_excludedemail
                {
                    public const string Name = "business_unit_cdi_excludedemail";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_excludedemail = "cdi_excludedemail";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_email = "cdi_email";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_executesend
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_executesend
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                             True
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
                /// ReferencingEntity cdi_executesend:
                ///     DisplayName:
                ///     (English - United States - 1033): Execute Send
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Execute Sends
                ///</summary>
                public static partial class business_unit_cdi_executesend
                {
                    public const string Name = "business_unit_cdi_executesend";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_executesend = "cdi_executesend";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_from = "cdi_from";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_executesocialpost
                /// 
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_executesocialpost
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                   True
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
                /// ReferencingEntity cdi_executesocialpost:
                ///     DisplayName:
                ///     (English - United States - 1033): Execute Social Post
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Execute Social Posts
                ///</summary>
                public static partial class business_unit_cdi_executesocialpost
                {
                    public const string Name = "business_unit_cdi_executesocialpost";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_executesocialpost = "cdi_executesocialpost";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_import
                /// 
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_import
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                        True
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
                /// ReferencingEntity cdi_import:
                ///     DisplayName:
                ///     (English - United States - 1033): ClickDimensions Import
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): ClickDimensions Imports
                ///</summary>
                public static partial class business_unit_cdi_import
                {
                    public const string Name = "business_unit_cdi_import";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_import = "cdi_import";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_importlog
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_importlog
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                           True
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
                /// ReferencingEntity cdi_importlog:
                ///     DisplayName:
                ///     (English - United States - 1033): Import Log
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Import Logs
                ///</summary>
                public static partial class business_unit_cdi_importlog
                {
                    public const string Name = "business_unit_cdi_importlog";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_importlog = "cdi_importlog";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_iporganization
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_iporganization
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                True
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
                /// ReferencingEntity cdi_iporganization:
                ///     DisplayName:
                ///     (English - United States - 1033): IP Organization
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): IP Organizations
                ///</summary>
                public static partial class business_unit_cdi_iporganization
                {
                    public const string Name = "business_unit_cdi_iporganization";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_iporganization = "cdi_iporganization";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_nurturebuilder
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_nurturebuilder
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                True
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
                /// ReferencingEntity cdi_nurturebuilder:
                ///     DisplayName:
                ///     (Russian - 1049): Nurture Program
                ///     
                ///     DisplayCollectionName:
                ///     (Russian - 1049): Nurture Programs
                ///</summary>
                public static partial class business_unit_cdi_nurturebuilder
                {
                    public const string Name = "business_unit_cdi_nurturebuilder";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_nurturebuilder = "cdi_nurturebuilder";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_pageview
                /// 
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_pageview
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                          True
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
                /// ReferencingEntity cdi_pageview:
                ///     DisplayName:
                ///     (English - United States - 1033): Page View
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Page Views
                ///</summary>
                public static partial class business_unit_cdi_pageview
                {
                    public const string Name = "business_unit_cdi_pageview";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_pageview = "cdi_pageview";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_time = "cdi_time";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_postedfield
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_postedfield
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                             True
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
                /// ReferencingEntity cdi_postedfield:
                ///     DisplayName:
                ///     (English - United States - 1033): Posted Field
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Posted Fields
                ///</summary>
                public static partial class business_unit_cdi_postedfield
                {
                    public const string Name = "business_unit_cdi_postedfield";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_postedfield = "cdi_postedfield";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_label = "cdi_label";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_postedform
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_postedform
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                            True
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
                /// ReferencingEntity cdi_postedform:
                ///     DisplayName:
                ///     (English - United States - 1033): Posted Form
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Posted Forms
                ///</summary>
                public static partial class business_unit_cdi_postedform
                {
                    public const string Name = "business_unit_cdi_postedform";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_postedform = "cdi_postedform";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_postedsubscription
                /// 
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_postedsubscription
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                    True
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
                /// ReferencingEntity cdi_postedsubscription:
                ///     DisplayName:
                ///     (English - United States - 1033): Posted Subscription
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Posted Subscriptions
                ///</summary>
                public static partial class business_unit_cdi_postedsubscription
                {
                    public const string Name = "business_unit_cdi_postedsubscription";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_postedsubscription = "cdi_postedsubscription";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_postedsurvey
                /// 
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_postedsurvey
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                              True
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
                /// ReferencingEntity cdi_postedsurvey:
                ///     DisplayName:
                ///     (English - United States - 1033): Posted Survey
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Posted Surveys
                ///</summary>
                public static partial class business_unit_cdi_postedsurvey
                {
                    public const string Name = "business_unit_cdi_postedsurvey";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_postedsurvey = "cdi_postedsurvey";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_quicksendprivilege
                /// 
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_quicksendprivilege
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                    True
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
                /// ReferencingEntity cdi_quicksendprivilege:
                ///     DisplayName:
                ///     (English - United States - 1033): Quick Send Privilege
                ///     
                ///     DisplayCollectionName:
                ///</summary>
                public static partial class business_unit_cdi_quicksendprivilege
                {
                    public const string Name = "business_unit_cdi_quicksendprivilege";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_quicksendprivilege = "cdi_quicksendprivilege";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_runnurture
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_runnurture
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                            True
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
                /// ReferencingEntity cdi_runnurture:
                ///     DisplayName:
                ///     (Russian - 1049): Run Nurture
                ///     
                ///     DisplayCollectionName:
                ///</summary>
                public static partial class business_unit_cdi_runnurture
                {
                    public const string Name = "business_unit_cdi_runnurture";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_runnurture = "cdi_runnurture";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_securitysession
                /// 
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_securitysession
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                 True
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
                /// ReferencingEntity cdi_securitysession:
                ///     DisplayName:
                ///     (English - United States - 1033): Security Session
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Security Sessions
                ///</summary>
                public static partial class business_unit_cdi_securitysession
                {
                    public const string Name = "business_unit_cdi_securitysession";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_securitysession = "cdi_securitysession";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_sendemail
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_sendemail
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                           True
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
                /// ReferencingEntity cdi_sendemail:
                ///     DisplayName:
                ///     (English - United States - 1033): Send ClickDimensions Email
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Send ClickDimensions Emails
                ///</summary>
                public static partial class business_unit_cdi_sendemail
                {
                    public const string Name = "business_unit_cdi_sendemail";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_sendemail = "cdi_sendemail";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_subject = "cdi_subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_sentemail
                /// 
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_sentemail
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                           True
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
                /// ReferencingEntity cdi_sentemail:
                ///     DisplayName:
                ///     (English - United States - 1033): Sent Email
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Sent Emails
                ///</summary>
                public static partial class business_unit_cdi_sentemail
                {
                    public const string Name = "business_unit_cdi_sentemail";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_sentemail = "cdi_sentemail";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_subject = "cdi_subject";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_socialclick
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_socialclick
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                             True
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
                /// ReferencingEntity cdi_socialclick:
                ///     DisplayName:
                ///     (English - United States - 1033): Social Click
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Social Clicks
                ///     
                ///     Description:
                ///     (English - United States - 1033): ClickDimensions entity used to show click data from social posts.
                ///</summary>
                public static partial class business_unit_cdi_socialclick
                {
                    public const string Name = "business_unit_cdi_socialclick";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_socialclick = "cdi_socialclick";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_socialpost
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_socialpost
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                            True
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
                /// ReferencingEntity cdi_socialpost:
                ///     DisplayName:
                ///     (English - United States - 1033): Social Post
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Social Posts
                ///     
                ///     Description:
                ///     (English - United States - 1033): ClickDimensions entity used to post information to social sites.
                ///</summary>
                public static partial class business_unit_cdi_socialpost
                {
                    public const string Name = "business_unit_cdi_socialpost";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_socialpost = "cdi_socialpost";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_subscriptionlist
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_subscriptionlist
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                  True
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
                /// ReferencingEntity cdi_subscriptionlist:
                ///     DisplayName:
                ///     (English - United States - 1033): Subscription List
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Subscription Lists
                ///</summary>
                public static partial class business_unit_cdi_subscriptionlist
                {
                    public const string Name = "business_unit_cdi_subscriptionlist";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_subscriptionlist = "cdi_subscriptionlist";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_subscriptionpreference
                /// 
                /// PropertyName                               Value                                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_subscriptionpreference
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                        True
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
                /// ReferencingEntity cdi_subscriptionpreference:
                ///     DisplayName:
                ///     (English - United States - 1033): Subscription Preference
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Subscription Preferences
                ///</summary>
                public static partial class business_unit_cdi_subscriptionpreference
                {
                    public const string Name = "business_unit_cdi_subscriptionpreference";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_subscriptionpreference = "cdi_subscriptionpreference";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_surveyanswer
                /// 
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_surveyanswer
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                              True
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
                /// ReferencingEntity cdi_surveyanswer:
                ///     DisplayName:
                ///     (English - United States - 1033): Survey Answer
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Survey Answers
                ///</summary>
                public static partial class business_unit_cdi_surveyanswer
                {
                    public const string Name = "business_unit_cdi_surveyanswer";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_surveyanswer = "cdi_surveyanswer";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_question = "cdi_question";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_surveyquestion
                /// 
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_surveyquestion
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                True
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
                /// ReferencingEntity cdi_surveyquestion:
                ///     DisplayName:
                ///     (English - United States - 1033): Survey Question
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Survey Questions
                ///</summary>
                public static partial class business_unit_cdi_surveyquestion
                {
                    public const string Name = "business_unit_cdi_surveyquestion";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_surveyquestion = "cdi_surveyquestion";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_unsubscribe
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_unsubscribe
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                             True
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
                /// ReferencingEntity cdi_unsubscribe:
                ///     DisplayName:
                ///     (English - United States - 1033): Unsubscribe
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Unsubscribes
                ///</summary>
                public static partial class business_unit_cdi_unsubscribe
                {
                    public const string Name = "business_unit_cdi_unsubscribe";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_unsubscribe = "cdi_unsubscribe";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_email = "cdi_email";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_usersession
                /// 
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_usersession
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                             True
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
                /// ReferencingEntity cdi_usersession:
                ///     DisplayName:
                ///     (English - United States - 1033): User Session
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): User Sessions
                ///</summary>
                public static partial class business_unit_cdi_usersession
                {
                    public const string Name = "business_unit_cdi_usersession";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_usersession = "cdi_usersession";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_visit
                /// 
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_visit
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                       True
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
                /// ReferencingEntity cdi_visit:
                ///     DisplayName:
                ///     (English - United States - 1033): Visit
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Visits
                ///</summary>
                public static partial class business_unit_cdi_visit
                {
                    public const string Name = "business_unit_cdi_visit";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_visit = "cdi_visit";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_time = "cdi_time";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_cdi_webcontent
                /// 
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_cdi_webcontent
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                            True
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
                /// ReferencingEntity cdi_webcontent:
                ///     DisplayName:
                ///     (English - United States - 1033): Web Content
                ///     
                ///     DisplayCollectionName:
                ///</summary>
                public static partial class business_unit_cdi_webcontent
                {
                    public const string Name = "business_unit_cdi_webcontent";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_webcontent = "cdi_webcontent";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_cdi_name = "cdi_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_channelaccessprofile
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_channelaccessprofile
                /// ReferencingEntityNavigationPropertyName    business_unit_channelaccessprofile
                /// IsCustomizable                             True                                  False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity channelaccessprofile:
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
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_characteristic
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                            False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity characteristic:
                ///     DisplayName:
                ///     (English - United States - 1033): Characteristic
                ///     (Russian - 1049): Характеристика
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Characteristics
                ///     (Russian - 1049): Характеристики
                ///     
                ///     Description:
                ///     (English - United States - 1033): Skills, education and certifications of resources.
                ///     (Russian - 1049): Навыки, образование и сертификация ресурсов.
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
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_connections
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                        False
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
                /// ReferencingEntity connection:
                ///     DisplayName:
                ///     (English - United States - 1033): Connection
                ///     (Russian - 1049): Подключение
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Connections
                ///     (Russian - 1049): Подключения
                ///     
                ///     Description:
                ///     (English - United States - 1033): Relationship between two entities.
                ///     (Russian - 1049): Отношение между двумя сущностями.
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
                /// PropertyName                               Value                                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_constraint_based_groups
                /// ReferencingEntityNavigationPropertyName    businessunitid_businessunit
                /// IsCustomizable                             False                                    False
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
                /// ReferencingEntity constraintbasedgroup:
                ///     DisplayName:
                ///     (English - United States - 1033): Resource Group
                ///     (Russian - 1049): Группа ресурсов
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Resource Groups
                ///     (Russian - 1049): Группы ресурсов
                ///     
                ///     Description:
                ///     (English - United States - 1033): Group or collection of people, equipment, and/or facilities that can be scheduled.
                ///     (Russian - 1049): Группа людей, оборудования и (или) помещений, которые могут быть запланированы.
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
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_contacts
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                      False
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
                /// ReferencingEntity contact:
                ///     DisplayName:
                ///     (English - United States - 1033): Person
                ///     (Russian - 1049): Контакт
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Persons
                ///     (Russian - 1049): Контакты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Person with whom a business unit has a relationship, such as customer, supplier, and colleague.
                ///     (Russian - 1049): Лицо, с которым подразделение состоит в отношениях (например, клиент, поставщик, коллега).
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
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_convertrule
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                        False
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
                /// ReferencingEntity convertrule:
                ///     DisplayName:
                ///     (English - United States - 1033): Record Creation and Update Rule
                ///     (Russian - 1049): Правило создания и обновления записей
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Record Creation and Update Rules
                ///     (Russian - 1049): Правила создания и обновления записей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Defines the settings for automatic record creation.
                ///     (Russian - 1049): Определяет параметры автоматического создания записей.
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
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_customer_relationship
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                                  False
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
                /// ReferencingEntity customerrelationship:
                ///     DisplayName:
                ///     (English - United States - 1033): Customer Relationship
                ///     (Russian - 1049): Взаимоотношения с клиентами
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Customer Relationships
                ///     (Russian - 1049): Отношение с клиентами
                ///     
                ///     Description:
                ///     (English - United States - 1033): Relationship between a customer and a partner in which either can be an account or contact.
                ///     (Russian - 1049): Отношение между клиентом и партнером, каждый из которых может быть организацией или контактом.
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
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_dynamicproperyinstance
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                                   False
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
                /// ReferencingEntity dynamicpropertyinstance:
                ///     DisplayName:
                ///     (English - United States - 1033): Property Instance
                ///     (Russian - 1049): Экземпляр свойства
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Property Instances
                ///     (Russian - 1049): Экземпляры свойства
                ///     
                ///     Description:
                ///     (English - United States - 1033): Instance of a property with its value.
                ///     (Russian - 1049): Экземпляр свойства и его значение.
                ///</summary>
                public static partial class business_unit_dynamicproperyinstance
                {
                    public const string Name = "business_unit_dynamicproperyinstance";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_dynamicpropertyinstance = "dynamicpropertyinstance";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_email_activities
                /// 
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_email_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_email
                /// IsCustomizable                             True                              False
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
                /// ReferencingEntity email:
                ///     DisplayName:
                ///     (English - United States - 1033): Email
                ///     (Russian - 1049): Электронная почта
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Email Messages
                ///     (Russian - 1049): Сообщения электронной почты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity that is delivered using email protocols.
                ///     (Russian - 1049): Действие, передаваемое с помощью протоколов электронной почты.
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
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_emailserverprofile
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                False
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
                /// ReferencingEntity emailserverprofile:
                ///     DisplayName:
                ///     (English - United States - 1033): Email Server Profile
                ///     (Russian - 1049): Профиль сервера электронной почты
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Email Server Profiles
                ///     (Russian - 1049): Профили серверов электронной почты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Holds the Email Server Profiles of an organization
                ///     (Russian - 1049): Содержит профили серверов электронной почты организации
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
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_emailsignatures
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// ReferencingEntity emailsignature:
                ///     DisplayName:
                ///     (English - United States - 1033): Email Signature
                ///     (Russian - 1049): Подпись электронной почты
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Email Signatures
                ///     (Russian - 1049): Подписи электронной почты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Signature for email message
                ///     (Russian - 1049): Подпись для сообщения электронной почты
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
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_entitlement
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                        False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity entitlement:
                ///     DisplayName:
                ///     (English - United States - 1033): Entitlement
                ///     (Russian - 1049): Объем обслуживания
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Entitlements
                ///     (Russian - 1049): Объемы обслуживания
                ///     
                ///     Description:
                ///     (English - United States - 1033): Defines the amount and type of support a customer should receive.
                ///     (Russian - 1049): Определяет объем и тип поддержки, которую должен получать клиент.
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
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_equipment
                /// ReferencingEntityNavigationPropertyName    businessunitid_businessunit
                /// IsCustomizable                             True                           False
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
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity equipment:
                ///     DisplayName:
                ///     (English - United States - 1033): Facility/Equipment
                ///     (Russian - 1049): Помещения / оборудование
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Facilities/Equipment
                ///     
                ///     Description:
                ///     (English - United States - 1033): Resource that can be scheduled.
                ///     (Russian - 1049): Ресурс, который может быть запланирован.
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
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_exchangesyncidmapping
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                                  False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity exchangesyncidmapping:
                ///     DisplayName:
                ///     (English - United States - 1033): Exchange Sync Id Mapping
                ///     (Russian - 1049): Сопоставление идентификаторов синхронизации с Exchange
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Exchange Sync Id Mappings
                ///     (Russian - 1049): Сопоставления идентификаторов синхронизации с Exchange
                ///     
                ///     Description:
                ///     (English - United States - 1033): The mapping used to keep track of the IDs for items synced between CRM and Exchange.
                ///     (Russian - 1049): Сопоставление, используемое для отслеживания идентификаторов элементов, синхронизируемых между CRM и Exchange.
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
                /// PropertyName                               Value                                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_externalparty
                /// ReferencingEntityNavigationPropertyName    business_unit_externalparty_externalparty
                /// IsCustomizable                             True                                         False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity externalparty:
                ///     DisplayName:
                ///     (English - United States - 1033): External Party
                ///     (Russian - 1049): Внешняя сторона
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): External Parties
                ///     (Russian - 1049): Внешние стороны
                ///     
                ///     Description:
                ///     (English - United States - 1033): Information about external parties that need to access Dynamics 365 from external channels.For internal use only
                ///     (Russian - 1049): Информация о внешних сторонах, которым необходим доступ к Dynamics 365 через внешние каналы. Только для внутреннего использования.
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
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_fax_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_fax
                /// IsCustomizable                             True                            False
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
                /// ReferencingEntity fax:
                ///     DisplayName:
                ///     (English - United States - 1033): Fax
                ///     (Russian - 1049): Факс
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Faxes
                ///     (Russian - 1049): Факсы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity that tracks call outcome and number of pages for a fax and optionally stores an electronic copy of the document.
                ///     (Russian - 1049): Действие, отслеживающее результаты звонков и число страниц в факсе и дополнительно хранящее электронную копию документа.
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
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_feedback
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                      False
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
                /// ReferencingEntity feedback:
                ///     DisplayName:
                ///     (English - United States - 1033): Feedback
                ///     (Russian - 1049): Отзывы
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Container for feedback and ratings for knowledge articles.
                ///     (Russian - 1049): Контейнер для отзывов и оценок к статьям базы знаний.
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
                /// 1:N - Relationship business_unit_goal
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_goal
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                     False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity goal:
                ///     DisplayName:
                ///     (English - United States - 1033): Goal
                ///     (Russian - 1049): Цель
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Goals
                ///     (Russian - 1049): Цели
                ///     
                ///     Description:
                ///     (English - United States - 1033): Target objective for a user or a team for a specified time period.
                ///     (Russian - 1049): Целевое значение для пользователя или рабочей группы за указанный период времени.
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
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_goalrollupquery
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// ReferencingEntity goalrollupquery:
                ///     DisplayName:
                ///     (English - United States - 1033): Rollup Query
                ///     (Russian - 1049): Запрос сведения
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Rollup Queries
                ///     (Russian - 1049): Запросы сведения
                ///     
                ///     Description:
                ///     (English - United States - 1033): Query that is used to filter the results of the goal rollup.
                ///     (Russian - 1049): Запрос, используемый для фильтрации результатов сведения цели.
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
                /// PropertyName                               Value                                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_incident_resolution_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_incidentresolution
                /// IsCustomizable                             False                                           False
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
                /// ReferencingEntity incidentresolution:
                ///     DisplayName:
                ///     (English - United States - 1033): Case Resolution
                ///     (Russian - 1049): Разрешение обращения
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Case Resolutions
                ///     (Russian - 1049): Разрешение обращение
                ///     
                ///     Description:
                ///     (English - United States - 1033): Special type of activity that includes description of the resolution, billing status, and the duration of the case.
                ///     (Russian - 1049): Специальный тип действия, включающий такие сведения, как описание разрешения, состояние выставления счета и длительность обращения.
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
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_incidents
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                       False
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
                /// ReferencingEntity incident:
                ///     DisplayName:
                ///     (English - United States - 1033): Case
                ///     (Russian - 1049): Обращение
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Cases
                ///     (Russian - 1049): Обращения
                ///     
                ///     Description:
                ///     (English - United States - 1033): Service request case associated with a contract.
                ///     (Russian - 1049): Обращение по запросу на обслуживание, связанному с контрактом.
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
                /// PropertyName                               Value                                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_new_interactionforemail
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                     False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity interactionforemail:
                ///     DisplayName:
                ///     (English - United States - 1033): Interaction for Email
                ///     (Russian - 1049): Взаимодействие для электронной почты
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Interactions for Email
                ///     (Russian - 1049): Взаимодействия для электронной почты
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
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_invoices
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                      False
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
                /// ReferencingEntity invoice:
                ///     DisplayName:
                ///     (English - United States - 1033): Invoice
                ///     (Russian - 1049): Счет
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Invoices
                ///     (Russian - 1049): Счета
                ///     
                ///     Description:
                ///     (English - United States - 1033): Order that has been billed.
                ///     (Russian - 1049): Заказ, по которому был выставлен счет.
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
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_knowledgearticle
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                              False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity knowledgearticle:
                ///     DisplayName:
                ///     (English - United States - 1033): Knowledge Article
                ///     (Russian - 1049): Статья базы знаний
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Knowledge Articles
                ///     (Russian - 1049): Статьи базы знаний
                ///     
                ///     Description:
                ///     (English - United States - 1033): Organizational knowledge for internal and external use.
                ///     (Russian - 1049): Знания организации для внутреннего и внешнего пользования.
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
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_leads
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity lead:
                ///     DisplayName:
                ///     (English - United States - 1033): Lead
                ///     (Russian - 1049): Интерес
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Leads
                ///     (Russian - 1049): Интересы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Prospect or potential sales opportunity. Leads are converted into accounts, contacts, or opportunities when they are qualified. Otherwise, they are deleted or archived.
                ///     (Russian - 1049): Перспективный клиент или потенциальная сделка. Интересы преобразуются в организации, контакты или возможные сделки после их оценки. В противном случае они удаляются или архивируются.
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
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_letter_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_letter
                /// IsCustomizable                             True                               False
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
                /// ReferencingEntity letter:
                ///     DisplayName:
                ///     (English - United States - 1033): Letter
                ///     (Russian - 1049): Письмо
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Letters
                ///     (Russian - 1049): Письма
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity that tracks the delivery of a letter. The activity can contain the electronic copy of the letter.
                ///     (Russian - 1049): Действие, отслеживающее доставку письма. Действие может содержать электронную копию письма.
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
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_list
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity list:
                ///     DisplayName:
                ///     (English - United States - 1033): Marketing List
                ///     (Russian - 1049): Маркетинговый список
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Marketing Lists
                ///     (Russian - 1049): Маркетинговые списки
                ///     
                ///     Description:
                ///     (English - United States - 1033): Group of existing or potential customers created for a marketing campaign or other sales purposes.
                ///     (Russian - 1049): Группа существующих или потенциальных клиентов, созданная для проведения маркетинговой кампании или для других целей.
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
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_mailbox
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity mailbox:
                ///     DisplayName:
                ///     (English - United States - 1033): Mailbox
                ///     (Russian - 1049): Почтовый ящик
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Mailboxes
                ///     (Russian - 1049): Почтовые ящики
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
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_mailmergetemplates
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                False
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
                /// ReferencingEntity mailmergetemplate:
                ///     DisplayName:
                ///     (English - United States - 1033): Mail Merge Template
                ///     (Russian - 1049): Шаблон слияния
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Mail Merge Templates
                ///     (Russian - 1049): Шаблоны слияния почты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Template for a mail merge document that contains the standard attributes of that document.
                ///     (Russian - 1049): Шаблон документа слияния, содержащий стандартные атрибуты этого документа.
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
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_msdyn_postalbum
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                             True
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
                /// ReferencingEntity msdyn_postalbum:
                ///     DisplayName:
                ///     (English - United States - 1033): Profile Album
                ///     (Russian - 1049): Альбом профиля
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Profile Albums
                ///     (Russian - 1049): Альбомы профилей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Contains user profile images that are stored as attachments and displayed in posts.
                ///     (Russian - 1049): Содержит образы профилей пользователей в том виде, в котором они хранятся как вложения и отображаются в записях.
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
                /// 1:N - Relationship business_unit_msdyn_wallsavedqueryusersettings
                /// 
                /// PropertyName                               Value                                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_msdyn_wallsavedqueryusersettings
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                              True
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
                /// ReferencingEntity msdyn_wallsavedqueryusersettings:
                ///     DisplayName:
                ///     (English - United States - 1033): Filter
                ///     (Russian - 1049): Фильтр
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Filters
                ///     (Russian - 1049): Фильтры
                ///     
                ///     Description:
                ///     (English - United States - 1033): Contains user personalization information regarding which of the administrator’s selected views to display on a user’s personal wall.
                ///     (Russian - 1049): Содержит данные персонализации, касающиеся того, как представления, выбранные администратором, будут отображаться на личной стене пользователя.
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
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_opportunities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                           False
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
                /// ReferencingEntity opportunity:
                ///     DisplayName:
                ///     (English - United States - 1033): Opportunity
                ///     (Russian - 1049): Возможная сделка
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Opportunities
                ///     (Russian - 1049): Возможные сделки
                ///     
                ///     Description:
                ///     (English - United States - 1033): Potential revenue-generating event, or sale to an account, which needs to be tracked through a sales process to completion.
                ///     (Russian - 1049): Событие, потенциально создающее прибыль, или продажа организации, которая должна отслеживаться в процессе продажи до завершения.
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
                /// PropertyName                               Value                                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_opportunity_close_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_opportunityclose
                /// IsCustomizable                             False                                         False
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
                /// ReferencingEntity opportunityclose:
                ///     DisplayName:
                ///     (English - United States - 1033): Opportunity Close
                ///     (Russian - 1049): Закрытие возможной сделки
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Opportunity Close Activities
                ///     (Russian - 1049): Действия по закрытию возможных сделок
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity that is created automatically when an opportunity is closed, containing information such as the description of the closing and actual revenue.
                ///     (Russian - 1049): Действие, которое создается автоматически при закрытии возможной сделки, содержащее такие сведения, как описание закрытия и фактический доход.
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
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_order_close_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_orderclose
                /// IsCustomizable                             False                                   False
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
                /// ReferencingEntity orderclose:
                ///     DisplayName:
                ///     (English - United States - 1033): Order Close
                ///     (Russian - 1049): Закрытие заказа
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Order Close Activities
                ///     (Russian - 1049): Действия по закрытию заказов
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity generated automatically when an order is closed.
                ///     (Russian - 1049): Действие создается автоматически при закрытии заказа.
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
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_orders
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity salesorder:
                ///     DisplayName:
                ///     (English - United States - 1033): Order
                ///     (Russian - 1049): Заказ
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Orders
                ///     (Russian - 1049): Заказы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Quote that has been accepted.
                ///     (Russian - 1049): Предложение с расценками принято.
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
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_parent_business_unit
                /// ReferencingEntityNavigationPropertyName    parentbusinessunitid
                /// IsCustomizable                             True                                  False
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
                /// PropertyName                               Value                                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_personaldocumenttemplates
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                       False
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
                /// ReferencingEntity personaldocumenttemplate:
                ///     DisplayName:
                ///     (English - United States - 1033): Personal Document Template
                ///     (Russian - 1049): Личный шаблон документа
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Personal Document Templates
                ///     (Russian - 1049): Личные шаблоны документов
                ///     
                ///     Description:
                ///     (English - United States - 1033): Used to store Personal Document Templates in database in binary format.
                ///     (Russian - 1049): Используется для хранения личных шаблонов документов в базе данных в двоичном формате.
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
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_phone_call_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_phonecall
                /// IsCustomizable                             True                                   False
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
                /// ReferencingEntity phonecall:
                ///     DisplayName:
                ///     (English - United States - 1033): Phone Call
                ///     (Russian - 1049): Звонок
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Phone Calls
                ///     (Russian - 1049): Звонки
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity to track a telephone call.
                ///     (Russian - 1049): Действие для отслеживания телефонного звонка.
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
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_postfollows
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                        False
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
                /// ReferencingEntity postfollow:
                ///     DisplayName:
                ///     (English - United States - 1033): Follow
                ///     (Russian - 1049): Подписаться
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Follows
                ///     (Russian - 1049): Подписан
                ///     
                ///     Description:
                ///     (English - United States - 1033): Represents a user following the activity feed of an object.
                ///     (Russian - 1049): Представляет пользователя, подписанного на ленту новостей объекта.
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
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_PostRegarding
                /// ReferencingEntityNavigationPropertyName    regardingobjectowningbusinessunit
                /// IsCustomizable                             False                                False
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
                /// ReferencingEntity postregarding:
                ///     DisplayName:
                ///     (English - United States - 1033): Post Regarding
                ///     (Russian - 1049): Запись "В отношении"
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Represents which object an activity feed post is regarding. For internal use only.
                ///     (Russian - 1049): Представляет, к какому объекту относится запись в ленте новостей. Только для внутреннего использования.
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
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_profilerule
                /// ReferencingEntityNavigationPropertyName    profileruleid5
                /// IsCustomizable                             True                         False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity channelaccessprofilerule:
                ///     DisplayName:
                ///     (English - United States - 1033): Channel Access Profile Rule
                ///     (Russian - 1049): Правило профиля доступа к каналам
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Channel Access Profile Rules
                ///     (Russian - 1049): Правила профиля доступа к каналам
                ///     
                ///     Description:
                ///     (English - United States - 1033): Defines the rules for automatically associating channel access profiles to external party records.For internal use only
                ///     (Russian - 1049): Определяет правила для автоматической связи профилей доступа к каналам с записями внешней стороны. Только для внутреннего использования.
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
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_queues
                /// ReferencingEntityNavigationPropertyName    businessunitid
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity queue:
                ///     DisplayName:
                ///     (English - United States - 1033): Queue
                ///     (Russian - 1049): Очередь
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Queues
                ///     (Russian - 1049): Очереди
                ///     
                ///     Description:
                ///     (English - United States - 1033): A list of records that require action, such as accounts, activities, and cases.
                ///     (Russian - 1049): Список записей, требующих действий от пользователя, например, организаций, действий и обращений.
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
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_queues2
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity queue:
                ///     DisplayName:
                ///     (English - United States - 1033): Queue
                ///     (Russian - 1049): Очередь
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Queues
                ///     (Russian - 1049): Очереди
                ///     
                ///     Description:
                ///     (English - United States - 1033): A list of records that require action, such as accounts, activities, and cases.
                ///     (Russian - 1049): Список записей, требующих действий от пользователя, например, организаций, действий и обращений.
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
                /// PropertyName                               Value                                   CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_quote_close_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_quoteclose
                /// IsCustomizable                             False                                   False
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
                /// ReferencingEntity quoteclose:
                ///     DisplayName:
                ///     (English - United States - 1033): Quote Close
                ///     (Russian - 1049): Закрытие предложения
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Quote Close Activities
                ///     (Russian - 1049): Действия по закрытию предложений
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity generated when a quote is closed.
                ///     (Russian - 1049): Действие, создаваемое при закрытии предложения с расценками.
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
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_quotes
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity quote:
                ///     DisplayName:
                ///     (English - United States - 1033): Quote
                ///     (Russian - 1049): Предложение с расценками
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Quotes
                ///     (Russian - 1049): Предложения с расценками
                ///     
                ///     Description:
                ///     (English - United States - 1033): Formal offer for products and/or services, proposed at specific prices and related payment terms, which is sent to a prospective customer.
                ///     (Russian - 1049): Формальное предложение продуктов и (или) услуг с конкретными ценами и условиями оплаты, отправляемое потенциальным клиентам.
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
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_ratingmodel
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                         False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity ratingmodel:
                ///     DisplayName:
                ///     (English - United States - 1033): Rating Model
                ///     (Russian - 1049): Модель оценки
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Rating Models
                ///     (Russian - 1049): Модели оценки
                ///     
                ///     Description:
                ///     (English - United States - 1033): Represents a model to evaluate skills or other related entities.
                ///     (Russian - 1049): Представляет модель для оценки навыков и других связанных сущностей.
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
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_ratingvalue
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                         False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity ratingvalue:
                ///     DisplayName:
                ///     (English - United States - 1033): Rating Value
                ///     (Russian - 1049): Значение оценки
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Rating Values
                ///     (Russian - 1049): Значения оценок
                ///     
                ///     Description:
                ///     (English - United States - 1033): A unique value associated with a rating model that allows providing a user friendly rating value.
                ///     (Russian - 1049): Уникальное значение, связанное с моделью оценки и содержащее понятное для пользователя значение оценки.
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
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_recurrencerule
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                           False
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
                /// ReferencingEntity recurrencerule:
                ///     DisplayName:
                ///     (English - United States - 1033): Recurrence Rule
                ///     (Russian - 1049): Правило повторения
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Recurrence Rules
                ///     (Russian - 1049): Правила повторения
                ///     
                ///     Description:
                ///     (English - United States - 1033): Recurrence Rule represents the pattern of incidence of recurring entities.
                ///     (Russian - 1049): Правило повторения представляет собой закономерность возникновения повторяющихся сущностей.
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
                /// PropertyName                               Value                                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_recurringappointmentmaster_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_recurringappointmentmaster
                /// IsCustomizable                             True                                                   False
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
                /// ReferencingEntity recurringappointmentmaster:
                ///     DisplayName:
                ///     (English - United States - 1033): Recurring Appointment
                ///     (Russian - 1049): Повторяющаяся встреча
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Recurring Appointments
                ///     (Russian - 1049): Повторяющиеся встречи
                ///     
                ///     Description:
                ///     (English - United States - 1033): The Master appointment of a recurring appointment series.
                ///     (Russian - 1049): Главная встреча ряда повторяющейся встречи.
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
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_reports
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                    False
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
                /// ReferencingEntity report:
                ///     DisplayName:
                ///     (English - United States - 1033): Report
                ///     (Russian - 1049): Отчет
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Reports
                ///     (Russian - 1049): Отчеты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Data summary in an easy-to-read layout.
                ///     (Russian - 1049): Сводные данные в легкочитаемом формате.
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
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_resource_groups
                /// ReferencingEntityNavigationPropertyName    businessunitid
                /// IsCustomizable                             False                            False
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
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity resourcegroup:
                ///     DisplayName:
                ///     (English - United States - 1033): Scheduling Group
                ///     (Russian - 1049): Группа планирования
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Scheduling Groups
                ///     (Russian - 1049): Группы планирования
                ///     
                ///     Description:
                ///     (English - United States - 1033): Resource group or team whose members can be scheduled for a service.
                ///     (Russian - 1049): Группа ресурсов или рабочая группа, участники которой могут быть запланированы для сервиса.
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
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_resource_specs
                /// ReferencingEntityNavigationPropertyName    businessunitid
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
                /// ReferencingEntity resourcespec:
                ///     DisplayName:
                ///     (English - United States - 1033): Resource Specification
                ///     (Russian - 1049): Спецификация ресурсов
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Resource Specifications
                ///     (Russian - 1049): Спецификации ресурсов
                ///     
                ///     Description:
                ///     (English - United States - 1033): Selection rule that allows the scheduling engine to select a number of resources from a pool of resources. The rules can be associated with a service.
                ///     (Russian - 1049): Правило выбора, позволяющее ядру планирования выбирать определенное количество ресурсов из пула ресурсов. Правила могут быть связаны с сервисом.
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
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_resources
                /// ReferencingEntityNavigationPropertyName    businessunitid
                /// IsCustomizable                             False                      False
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
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity resource:
                ///     DisplayName:
                ///     (English - United States - 1033): Resource
                ///     (Russian - 1049): Ресурс
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Resources
                ///     (Russian - 1049): Ресурсы
                ///     
                ///     Description:
                ///     (English - United States - 1033): User or facility/equipment that can be scheduled for a service.
                ///     (Russian - 1049): Пользователь или оборудование, которые могут быть запланированы для сервиса.
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
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_roles
                /// ReferencingEntityNavigationPropertyName    businessunitid
                /// IsCustomizable                             False                    False
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
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity role:
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
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_routingrule
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                        False
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
                /// ReferencingEntity routingrule:
                ///     DisplayName:
                ///     (English - United States - 1033): Routing Rule Set
                ///     (Russian - 1049): Набор правил маршрутизации
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Routing Rule Sets
                ///     (Russian - 1049): Наборы правил маршрутизации
                ///     
                ///     Description:
                ///     (English - United States - 1033): Define Routing Rule to route cases to right people at the right time
                ///     (Russian - 1049): Определите правило маршрутизации для своевременного направления обращений нужным лицам
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
                /// 1:N - Relationship business_unit_service_appointments
                /// 
                /// PropertyName                               Value                                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_service_appointments
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_serviceappointment
                /// IsCustomizable                             True                                     False
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
                /// ReferencingEntity serviceappointment:
                ///     DisplayName:
                ///     (English - United States - 1033): Service Activity
                ///     (Russian - 1049): Действие сервиса
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Service Activities
                ///     (Russian - 1049): Действия сервиса
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity offered by the organization to satisfy its customer's needs. Each service activity includes date, time, duration, and required resources.
                ///     (Russian - 1049): Действие, предлагаемое организацией с целью удовлетворить потребности клиента. Каждое действие сервиса включает дату, время, продолжительность и необходимые ресурсы.
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
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_service_contracts
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                               False
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
                /// ReferencingEntity contract:
                ///     DisplayName:
                ///     (English - United States - 1033): Contract
                ///     (Russian - 1049): Контракт
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Contracts
                ///     (Russian - 1049): Контракты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Agreement to provide customer service during a specified amount of time or number of cases.
                ///     (Russian - 1049): Соглашение об обслуживании клиентов в течение определенного периода времени или количества обращений.
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
                /// PropertyName                               Value                               CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_sharepointdocument
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                False
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
                /// ReferencingEntity sharepointdocument:
                ///     DisplayName:
                ///     (English - United States - 1033): Sharepoint Document
                ///     (Russian - 1049): Документ SharePoint
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Documents
                ///     (Russian - 1049): Документы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Document libraries or folders on a SharePoint server from where documents can be managed in Microsoft Dynamics 365.
                ///     (Russian - 1049): Библиотеки документов или папки на сервере SharePoint, документами из которых можно управлять с помощью Microsoft Dynamics 365.
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
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_sharepointdocument2
                /// ReferencingEntityNavigationPropertyName    businessunitid
                /// IsCustomizable                             True                                 False
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
                /// ReferencingEntity sharepointdocument:
                ///     DisplayName:
                ///     (English - United States - 1033): Sharepoint Document
                ///     (Russian - 1049): Документ SharePoint
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Documents
                ///     (Russian - 1049): Документы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Document libraries or folders on a SharePoint server from where documents can be managed in Microsoft Dynamics 365.
                ///     (Russian - 1049): Библиотеки документов или папки на сервере SharePoint, документами из которых можно управлять с помощью Microsoft Dynamics 365.
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
                /// PropertyName                               Value                                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_sharepointdocumentlocation
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                                        False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity sharepointdocumentlocation:
                ///     DisplayName:
                ///     (English - United States - 1033): Document Location
                ///     (Russian - 1049): Расположение документа
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Document Locations
                ///     (Russian - 1049): Расположения документов
                ///     
                ///     Description:
                ///     (English - United States - 1033): Document libraries or folders on a SharePoint server from where documents can be managed in Microsoft Dynamics 365.
                ///     (Russian - 1049): Библиотеки документов или папки на сервере SharePoint, документами из которых можно управлять с помощью Microsoft Dynamics 365.
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
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_sharepointsites
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                             False
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
                /// ReferencingEntity sharepointsite:
                ///     DisplayName:
                ///     (English - United States - 1033): SharePoint Site
                ///     (Russian - 1049): Сайт SharePoint
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): SharePoint Sites
                ///     (Russian - 1049): Сайты SharePoint
                ///     
                ///     Description:
                ///     (English - United States - 1033): SharePoint site from where documents can be managed in Microsoft Dynamics 365.
                ///     (Russian - 1049): Сайт SharePoint, документами которого можно управлять в Microsoft Dynamics 365.
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
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_slabase
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                     False
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
                /// ReferencingEntity sla:
                ///     DisplayName:
                ///     (English - United States - 1033): SLA
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): SLAs
                ///     
                ///     Description:
                ///     (English - United States - 1033): Contains information about the tracked service-level KPIs for cases that belong to different customers.
                ///     (Russian - 1049): Содержит информацию об отслеживаемых ключевых индикаторах уровня обслуживания (KPI) для обращений, принадлежащих разным клиентам.
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
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_slakpiinstance
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                           False
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
                /// ReferencingEntity slakpiinstance:
                ///     DisplayName:
                ///     (English - United States - 1033): SLA KPI Instance
                ///     (Russian - 1049): Экземпляр KPI по SLA
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): SLA KPI Instances
                ///     (Russian - 1049): Экземпляры KPI по SLA
                ///     
                ///     Description:
                ///     (English - United States - 1033): Service level agreement (SLA) key performance indicator (KPI) instance that is tracked for an individual case
                ///     (Russian - 1049): Экземпляр ключевого показателя эффективности (KPI) соглашения об уровнях обслуживания (SLA), отслеживаемый для отдельного обращения
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
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_socialactivity
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_socialactivity
                /// IsCustomizable                             False                                False
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
                /// ReferencingEntity socialactivity:
                ///     DisplayName:
                ///     (English - United States - 1033): Social Activity
                ///     (Russian - 1049): Действие социальной сети
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Social Activities
                ///     (Russian - 1049): Действия социальной сети
                ///     
                ///     Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
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
                /// PropertyName                               Value                           CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_socialprofiles
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                            False
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
                /// ReferencingEntity socialprofile:
                ///     DisplayName:
                ///     (English - United States - 1033): Social Profile
                ///     (Russian - 1049): Профиль социальной сети
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Social Profiles
                ///     (Russian - 1049): Профили социальной сети
                ///     
                ///     Description:
                ///     (English - United States - 1033): This entity is used to store social profile information of its associated account and contacts on different social channels.
                ///     (Russian - 1049): Эта сущность используется для хранения сведений профиля социальной сети о связанной организации и контактах в различных каналах социальных сетей.
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
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_system_users
                /// ReferencingEntityNavigationPropertyName    businessunitid
                /// IsCustomizable                             True                          False
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
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity systemuser:
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
                /// PropertyName                               Value                            CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_task_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_task
                /// IsCustomizable                             True                             False
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
                /// ReferencingEntity task:
                ///     DisplayName:
                ///     (English - United States - 1033): Task
                ///     (Russian - 1049): Задача
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Tasks
                ///     (Russian - 1049): Задачи
                ///     
                ///     Description:
                ///     (English - United States - 1033): Generic activity representing work needed to be done.
                ///     (Russian - 1049): Универсальное действие, представляющее работу, которую необходимо выполнить.
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
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_teams
                /// ReferencingEntityNavigationPropertyName    businessunitid
                /// IsCustomizable                             True                     False
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
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity team:
                ///     DisplayName:
                ///     (English - United States - 1033): Team
                ///     (Russian - 1049): Рабочая группа
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Teams
                ///     (Russian - 1049): Рабочие группы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Collection of system users that routinely collaborate. Teams can be used to simplify record sharing and provide team members with common access to organization data when team members belong to different Business Units.
                ///     (Russian - 1049): Набор системных пользователей, которые обычно участвуют в совместной работе. Рабочие группы можно использовать для упрощения предоставления участникам группы общего доступа к записям и данным организации, если участники принадлежат к разным подразделениям.
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
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_templates
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                      False
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
                /// ReferencingEntity template:
                ///     DisplayName:
                ///     (English - United States - 1033): Email Template
                ///     (Russian - 1049): Шаблон электронной почты
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Email Templates
                ///     (Russian - 1049): Шаблоны электронной почты
                ///     
                ///     Description:
                ///     (English - United States - 1033): Template for an email message that contains the standard attributes of an email message.
                ///     (Russian - 1049): Шаблон сообщения электронной почты, содержащий стандартные атрибуты сообщения электронной почты.
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
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_TraceRegarding
                /// ReferencingEntityNavigationPropertyName    regardingobjectowningbusinessunit
                /// IsCustomizable                             False                                False
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
                /// ReferencingEntity traceregarding:
                ///     DisplayName:
                ///     (English - United States - 1033): Trace Regarding
                ///     (Russian - 1049): Трассировка в отношении
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Represents which object a trace record is regarding. For internal use only.
                ///     (Russian - 1049): Представляет объект, к которому относится запись трассировки. Только для внутреннего использования.
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
                /// PropertyName                               Value                                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_untrackedemail_activities
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_untrackedemail
                /// IsCustomizable                             True                                       False
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
                /// ReferencingEntity untrackedemail:
                ///     DisplayName:
                ///     (English - United States - 1033): UntrackedEmail
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): UntrackedEmail Messages
                ///     (Russian - 1049): Сообщения UntrackedEmail
                ///     
                ///     Description:
                ///     (English - United States - 1033): Activity that is delivered using UntrackedEmail protocols.
                ///     (Russian - 1049): Действие, передаваемое с помощью протоколов UntrackedEmail.
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
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_user_settings
                /// ReferencingEntityNavigationPropertyName    businessunitid_businessunit
                /// IsCustomizable                             False                          False
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
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity usersettings:
                ///     DisplayName:
                ///     (English - United States - 1033): User Settings
                ///     (Russian - 1049): Параметры пользователя
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): User's preferred settings.
                ///     (Russian - 1049): Предпочитаемые параметры пользователя.
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
                /// PropertyName                               Value                                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_userapplicationmetadata
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                                    False
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
                /// ReferencingEntity userapplicationmetadata:
                ///     DisplayName:
                ///     (English - United States - 1033): User Application Metadata
                ///     (Russian - 1049): Метаданные пользовательского приложения
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): User Application Metadata Collection
                ///     (Russian - 1049): Сбор метаданных пользовательского приложения
                ///     
                ///     Description:
                ///     (English - United States - 1033): For internal use only.
                ///     (Russian - 1049): Только для внутреннего использования.
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
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_userform
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// ReferencingEntity userform:
                ///     DisplayName:
                ///     (English - United States - 1033): User Dashboard
                ///     (Russian - 1049): Панель мониторинга пользователя
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): User Dashboards
                ///     (Russian - 1049): Панели мониторинга пользователя
                ///     
                ///     Description:
                ///     (English - United States - 1033): User-owned dashboards.
                ///     (Russian - 1049): Панели мониторинга, принадлежащие пользователю.
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
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_userquery
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                      False
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
                /// ReferencingEntity userquery:
                ///     DisplayName:
                ///     (English - United States - 1033): Saved View
                ///     (Russian - 1049): Сохраненное представление
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Saved Views
                ///     (Russian - 1049): Сохраненные представления
                ///     
                ///     Description:
                ///     (English - United States - 1033): Saved database query that is owned by a user.
                ///     (Russian - 1049): Сохраненный запрос базы данных, который принадлежит пользователю.
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
                /// PropertyName                               Value                                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_userqueryvisualizations
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                                    False
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
                /// ReferencingEntity userqueryvisualization:
                ///     DisplayName:
                ///     (English - United States - 1033): User Chart
                ///     (Russian - 1049): Диаграмма пользователя
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): User Charts
                ///     (Russian - 1049): Диаграммы пользователя
                ///     
                ///     Description:
                ///     (English - United States - 1033): Chart attached to an entity.
                ///     (Russian - 1049): Диаграмма, присоединенная к сущности.
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
                /// 1:N - Relationship business_unit_vw_car
                /// 
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_vw_car
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                     True
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
                /// ReferencingEntity vw_car:
                ///     DisplayName:
                ///     (English - United States - 1033): Car
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Cars
                ///</summary>
                public static partial class business_unit_vw_car
                {
                    public const string Name = "business_unit_vw_car";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_vw_car = "vw_car";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_vw_vin = "vw_vin";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_vw_contract
                /// 
                /// PropertyName                               Value                        CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_vw_contract
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                         True
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
                /// ReferencingEntity vw_contract:
                ///     DisplayName:
                ///     (English - United States - 1033): Contract
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Contracts
                ///</summary>
                public static partial class business_unit_vw_contract
                {
                    public const string Name = "business_unit_vw_contract";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_vw_contract = "vw_contract";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_vw_contractnumber = "vw_contractnumber";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_vw_dealersaccount
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_vw_dealersaccount
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                               True
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
                /// ReferencingEntity vw_dealersaccount:
                ///     DisplayName:
                ///     (English - United States - 1033): Dealer`s Account
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Dealer`s Accounts
                ///</summary>
                public static partial class business_unit_vw_dealersaccount
                {
                    public const string Name = "business_unit_vw_dealersaccount";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_vw_dealersaccount = "vw_dealersaccount";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_vw_name = "vw_name";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_vw_dealerscontact
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_vw_dealerscontact
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                               True
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
                /// ReferencingEntity vw_dealerscontact:
                ///     DisplayName:
                ///     (English - United States - 1033): Dealer`s Contact
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Dealer`s Contacts
                ///</summary>
                public static partial class business_unit_vw_dealerscontact
                {
                    public const string Name = "business_unit_vw_dealerscontact";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_vw_dealerscontact = "vw_dealerscontact";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_vw_fullname = "vw_fullname";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_vw_servicerequest
                /// 
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_vw_servicerequest
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                               True
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
                /// ReferencingEntity vw_servicerequest:
                ///     DisplayName:
                ///     (English - United States - 1033): Service Request
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Service Requests
                ///</summary>
                public static partial class business_unit_vw_servicerequest
                {
                    public const string Name = "business_unit_vw_servicerequest";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_vw_servicerequest = "vw_servicerequest";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_vw_requestnumber = "vw_requestnumber";
                }

                ///<summary>
                /// 1:N - Relationship business_unit_workflow
                /// 
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_workflow
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// ReferencingEntity workflow:
                ///     DisplayName:
                ///     (English - United States - 1033): Process
                ///     (Russian - 1049): Процесс
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Processes
                ///     (Russian - 1049): Процессы
                ///     
                ///     Description:
                ///     (English - United States - 1033): Set of logical rules that define the steps necessary to automate a specific business process, task, or set of actions to be performed.
                ///     (Russian - 1049): Задайте логические правила, определяющие необходимые действия для автоматизации конкретных бизнес-процессов, заданий или наборов действий.
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
                /// PropertyName                               Value                         CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     business_unit_workflowlogs
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                         False
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
                /// ReferencingEntity workflowlog:
                ///     DisplayName:
                ///     (English - United States - 1033): Process Log
                ///     (Russian - 1049): Журнал процесса
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Process Logs
                ///     (Russian - 1049): Журналы процессов
                ///     
                ///     Description:
                ///     (English - United States - 1033): Log used to track process execution.
                ///     (Russian - 1049): Журнал для отслеживания хода выполнения процесса.
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
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_AsyncOperations
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_businessunit
                /// IsCustomizable                             False                             False
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
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_BulkDeleteFailures
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_businessunit
                /// IsCustomizable                             False                              False
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
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_Campaigns
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             True                      False
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
                /// ReferencingEntity campaign:
                ///     DisplayName:
                ///     (English - United States - 1033): Campaign
                ///     (Russian - 1049): Кампания
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Campaigns
                ///     (Russian - 1049): Кампании
                ///     
                ///     Description:
                ///     (English - United States - 1033): Container for campaign activities and responses, sales literature, products, and lists to create, plan, execute, and track the results of a specific marketing campaign through its life.
                ///     (Russian - 1049): Контейнер для действий и откликов, литературы, продуктов и списков для создания, планирования, выполнения и отслеживания результатов определенной маркетинговой кампании в течение срока ее действия.
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
                /// PropertyName                               Value                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_DuplicateRules
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// ReferencingEntity duplicaterule:
                ///     DisplayName:
                ///     (English - United States - 1033): Duplicate Detection Rule
                ///     (Russian - 1049): Правило обнаружения повторяющихся записей
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Duplicate Detection Rules
                ///     (Russian - 1049): Правила обнаружения повторяющихся записей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Rule used to identify potential duplicates.
                ///     (Russian - 1049): Правило, используемое для определения возможных повторов.
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
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_ImportData
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                      False
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
                /// ReferencingEntity importdata:
                ///     DisplayName:
                ///     (English - United States - 1033): Import Data
                ///     (Russian - 1049): Данные импорта
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Unprocessed data from imported files.
                ///     (Russian - 1049): Необработанные данные из импортированных файлов.
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
                /// PropertyName                               Value                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_ImportFiles
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                       False
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
                /// ReferencingEntity importfile:
                ///     DisplayName:
                ///     (English - United States - 1033): Import Source File
                ///     (Russian - 1049): Файл источника импорта
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Imports
                ///     (Russian - 1049): Импорт
                ///     
                ///     Description:
                ///     (English - United States - 1033): File name of file used for import.
                ///     (Russian - 1049): Имя файла, используемого для импорта.
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
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_ImportLogs
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                      False
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
                /// ReferencingEntity importlog:
                ///     DisplayName:
                ///     (English - United States - 1033): Import Log
                ///     (Russian - 1049): Журнал импорта
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): ImportLogs
                ///     
                ///     Description:
                ///     (English - United States - 1033): Failure reason and other detailed information for a record that failed to import.
                ///     (Russian - 1049): Причина сбоя и другие подробные сведения о записи, при импорте которой произошла ошибка.
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
                /// PropertyName                               Value                      CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_ImportMaps
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                      False
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
                /// ReferencingEntity importmap:
                ///     DisplayName:
                ///     (English - United States - 1033): Data Map
                ///     (Russian - 1049): Сопоставление данных
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Data Maps
                ///     (Russian - 1049): Сопоставления данных
                ///     
                ///     Description:
                ///     (English - United States - 1033): Data map used in import.
                ///     (Russian - 1049): Карта данных, использованная в импорте.
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
                /// PropertyName                               Value                    CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_Imports
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                    False
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
                /// ReferencingEntity import:
                ///     DisplayName:
                ///     (English - United States - 1033): Data Import
                ///     (Russian - 1049): Импорт данных
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Data Imports
                ///     (Russian - 1049): Импорты данных
                ///     
                ///     Description:
                ///     (English - United States - 1033): Status and ownership information for an import job.
                ///     (Russian - 1049): Сведения о состоянии и ответственном для задания импорта.
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
                /// PropertyName                               Value                              CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     businessunit_internal_addresses
                /// ReferencingEntityNavigationPropertyName    parentid_businessunit
                /// IsCustomizable                             False                              False
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
                /// ReferencingEntity internaladdress:
                ///     DisplayName:
                ///     (English - United States - 1033): Internal Address
                ///     (Russian - 1049): Внутренний адрес
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Internal Addresses
                ///     (Russian - 1049): Внутренние адреса
                ///     
                ///     Description:
                ///     (English - United States - 1033): Storage of addresses for a user, business unit, or site.
                ///     (Russian - 1049): Хранилище адресов для пользователя, подразделения или сайта.
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
                /// 1:N - Relationship businessunit_mailboxtrackingfolder
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     businessunit_mailboxtrackingfolder
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                                 False
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
                /// ReferencingEntity mailboxtrackingfolder:
                ///     DisplayName:
                ///     (English - United States - 1033): Mailbox Auto Tracking Folder
                ///     (Russian - 1049): Папка автоматического отслеживания почтового ящика
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Mailbox Auto Tracking Folders
                ///     (Russian - 1049): Папки автоматического отслеживания почтового ящика
                ///     
                ///     Description:
                ///     (English - United States - 1033): Stores data about what folders for a mailbox are auto tracked
                ///     (Russian - 1049): Хранит данные о том, какие папки для почтового ящика отслеживаются автоматически
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
                /// PropertyName                               Value                                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     businessunit_principalobjectattributeaccess
                /// ReferencingEntityNavigationPropertyName    objectid_businessunit
                /// IsCustomizable                             False                                          False
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
                /// AssociatedMenuConfiguration.Behavior       DoNotDisplay
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          null
                /// 
                /// ReferencingEntity principalobjectattributeaccess:
                ///     DisplayName:
                ///     (English - United States - 1033): Field Sharing
                ///     (Russian - 1049): Общий доступ к полям
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Defines CRM security principals (users and teams) access rights to secured field for an entity instance.
                ///     (Russian - 1049): Определяет права на доступ субъектов безопасности CRM (пользователей и рабочих группы) к защищенному полю экземпляра сущности.
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
                /// PropertyName                               Value                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_ProcessSessions
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_businessunit
                /// IsCustomizable                             False                             False
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
                /// AssociatedMenuConfiguration.Behavior       UseCollectionName
                /// AssociatedMenuConfiguration.Group          Details
                /// AssociatedMenuConfiguration.Order          110
                /// 
                /// ReferencingEntity processsession:
                ///     DisplayName:
                ///     (English - United States - 1033): Process Session
                ///     (Russian - 1049): Сеанс процесса
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Process Sessions
                ///     (Russian - 1049): Сеансы процесса
                ///     
                ///     Description:
                ///     (English - United States - 1033): Information that is generated when a dialog is run. Every time that you run a dialog, a dialog session is created.
                ///     (Russian - 1049): Информация, созданная после запуска диалогового окна. При каждом запуске диалогового окна создается сеанс диалогового окна.
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
                /// PropertyName                               Value                     CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_SyncError
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
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
                /// PropertyName                               Value                                       CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     BusinessUnit_SyncErrors
                /// ReferencingEntityNavigationPropertyName    regardingobjectid_businessunit_syncerror
                /// IsCustomizable                             True                                        False
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
                /// 1:N - Relationship cdi_txtmessage_businessunit_owningbusinessunit
                /// 
                /// PropertyName                               Value                                             CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     cdi_txtmessage_businessunit_owningbusinessunit
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit_cdi_txtmessage
                /// IsCustomizable                             True                                              True
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
                /// ReferencingEntity cdi_txtmessage:
                ///     DisplayName:
                ///     (English - United States - 1033): Text Message
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Text Messages
                ///</summary>
                public static partial class cdi_txtmessage_businessunit_owningbusinessunit
                {
                    public const string Name = "cdi_txtmessage_businessunit_owningbusinessunit";

                    public const string ReferencedEntity_businessunit = "businessunit";

                    public const string ReferencedAttribute_businessunitid = "businessunitid";

                    public const string ReferencingEntity_cdi_txtmessage = "cdi_txtmessage";

                    public const string ReferencingAttribute_owningbusinessunit = "owningbusinessunit";

                    public const string ReferencingEntity_PrimaryNameAttribute_subject = "subject";
                }

                ///<summary>
                /// 1:N - Relationship lk_userfiscalcalendar_businessunit
                /// 
                /// PropertyName                               Value                                 CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     lk_userfiscalcalendar_businessunit
                /// ReferencingEntityNavigationPropertyName    businessunitid_businessunit
                /// IsCustomizable                             False                                 False
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
                /// ReferencingEntity userfiscalcalendar:
                ///     DisplayName:
                ///     (English - United States - 1033): User Fiscal Calendar
                ///     (Russian - 1049): Финансовый календарь пользователя
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): User Fiscal Calendars
                ///     (Russian - 1049): Финансовые календари пользователей
                ///     
                ///     Description:
                ///     (English - United States - 1033): Custom fiscal calendar used for tracking sales quotas.
                ///     (Russian - 1049): Настраиваемый финансовый календарь, используемый для отслеживания квот продаж.
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
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     Owning_businessunit_processsessions
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                                  False
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
                /// ReferencingEntity processsession:
                ///     DisplayName:
                ///     (English - United States - 1033): Process Session
                ///     (Russian - 1049): Сеанс процесса
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): Process Sessions
                ///     (Russian - 1049): Сеансы процесса
                ///     
                ///     Description:
                ///     (English - United States - 1033): Information that is generated when a dialog is run. Every time that you run a dialog, a dialog session is created.
                ///     (Russian - 1049): Информация, созданная после запуска диалогового окна. При каждом запуске диалогового окна создается сеанс диалогового окна.
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
                /// PropertyName                               Value                                                          CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     systemuserbusinessunitentitymap_businessunitid_businessunit
                /// ReferencingEntityNavigationPropertyName    businessunitid_businessunit
                /// IsCustomizable                             False                                                          False
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
                /// ReferencingEntity systemuserbusinessunitentitymap:
                ///     DisplayName:
                ///     (English - United States - 1033): SystemUser BusinessUnit Entity Map
                ///     (Russian - 1049): Сопоставление сущностей подразделения системного пользователя
                ///     
                ///     DisplayCollectionName:
                ///     (English - United States - 1033): SystemUserBusiness Unit Entity Maps
                ///     (Russian - 1049): Сопоставления сущностей подразделения системного пользователя
                ///     
                ///     Description:
                ///     (English - United States - 1033): Stores mapping attributes for business units.
                ///     (Russian - 1049): Хранит атрибуты сопоставления для подразделений.
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
                /// PropertyName                               Value                                  CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityinstancedata_businessunit
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                                  False
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
                /// PropertyName                               Value                                CanBeChanged
                /// IsHierarchical                             False
                /// ReferencedEntityNavigationPropertyName     userentityuisettings_businessunit
                /// ReferencingEntityNavigationPropertyName    owningbusinessunit
                /// IsCustomizable                             False                                False
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
                /// ReferencingEntity userentityuisettings:
                ///     DisplayName:
                ///     (English - United States - 1033): User Entity UI Settings
                ///     (Russian - 1049): Параметры интерфейса сущности пользователя
                ///     
                ///     DisplayCollectionName:
                ///     
                ///     Description:
                ///     (English - United States - 1033): Stores user settings for entity views.
                ///     (Russian - 1049): Хранит параметры пользователя для представлений сущности.
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
