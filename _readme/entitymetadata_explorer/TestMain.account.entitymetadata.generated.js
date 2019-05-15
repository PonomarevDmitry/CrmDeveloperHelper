if (typeof (account) == 'undefined') {
    account = { __namespace: true };
}

account.Schema = {

    'Attributes': {
        'accountcategorycode': 'accountcategorycode',
        'accountclassificationcode': 'accountclassificationcode',
        'accountid': 'accountid',
        'accountnumber': 'accountnumber',
        'accountratingcode': 'accountratingcode',
        'address1_addressid': 'address1_addressid',
        'address1_addresstypecode': 'address1_addresstypecode',
        'address1_city': 'address1_city',
        'address1_composite': 'address1_composite',
        'address1_country': 'address1_country',
        'address1_county': 'address1_county',
        'address1_fax': 'address1_fax',
        'address1_freighttermscode': 'address1_freighttermscode',
        'address1_latitude': 'address1_latitude',
        'address1_line1': 'address1_line1',
        'address1_line2': 'address1_line2',
        'address1_line3': 'address1_line3',
        'address1_longitude': 'address1_longitude',
        'address1_name': 'address1_name',
        'address1_postalcode': 'address1_postalcode',
        'address1_postofficebox': 'address1_postofficebox',
        'address1_primarycontactname': 'address1_primarycontactname',
        'address1_shippingmethodcode': 'address1_shippingmethodcode',
        'address1_stateorprovince': 'address1_stateorprovince',
        'address1_telephone1': 'address1_telephone1',
        'address1_telephone2': 'address1_telephone2',
        'address1_telephone3': 'address1_telephone3',
        'address1_upszone': 'address1_upszone',
        'address1_utcoffset': 'address1_utcoffset',
        'address2_addressid': 'address2_addressid',
        'address2_addresstypecode': 'address2_addresstypecode',
        'address2_city': 'address2_city',
        'address2_composite': 'address2_composite',
        'address2_country': 'address2_country',
        'address2_county': 'address2_county',
        'address2_fax': 'address2_fax',
        'address2_freighttermscode': 'address2_freighttermscode',
        'address2_latitude': 'address2_latitude',
        'address2_line1': 'address2_line1',
        'address2_line2': 'address2_line2',
        'address2_line3': 'address2_line3',
        'address2_longitude': 'address2_longitude',
        'address2_name': 'address2_name',
        'address2_postalcode': 'address2_postalcode',
        'address2_postofficebox': 'address2_postofficebox',
        'address2_primarycontactname': 'address2_primarycontactname',
        'address2_shippingmethodcode': 'address2_shippingmethodcode',
        'address2_stateorprovince': 'address2_stateorprovince',
        'address2_telephone1': 'address2_telephone1',
        'address2_telephone2': 'address2_telephone2',
        'address2_telephone3': 'address2_telephone3',
        'address2_upszone': 'address2_upszone',
        'address2_utcoffset': 'address2_utcoffset',
        'aging30': 'aging30',
        'aging30_base': 'aging30_base',
        'aging60': 'aging60',
        'aging60_base': 'aging60_base',
        'aging90': 'aging90',
        'aging90_base': 'aging90_base',
        'businesstypecode': 'businesstypecode',
        'createdby': 'createdby',
        'createdbyexternalparty': 'createdbyexternalparty',
        'createdon': 'createdon',
        'createdonbehalfby': 'createdonbehalfby',
        'creditlimit': 'creditlimit',
        'creditlimit_base': 'creditlimit_base',
        'creditonhold': 'creditonhold',
        'customersizecode': 'customersizecode',
        'customertypecode': 'customertypecode',
        'defaultpricelevelid': 'defaultpricelevelid',
        'description': 'description',
        'donotbulkemail': 'donotbulkemail',
        'donotbulkpostalmail': 'donotbulkpostalmail',
        'donotemail': 'donotemail',
        'donotfax': 'donotfax',
        'donotphone': 'donotphone',
        'donotpostalmail': 'donotpostalmail',
        'donotsendmm': 'donotsendmm',
        'emailaddress1': 'emailaddress1',
        'emailaddress2': 'emailaddress2',
        'emailaddress3': 'emailaddress3',
        'entityimageid': 'entityimageid',
        'exchangerate': 'exchangerate',
        'fax': 'fax',
        'followemail': 'followemail',
        'ftpsiteurl': 'ftpsiteurl',
        'importsequencenumber': 'importsequencenumber',
        'industrycode': 'industrycode',
        'isprivate': 'isprivate',
        'lastonholdtime': 'lastonholdtime',
        'lastusedincampaign': 'lastusedincampaign',
        'marketcap': 'marketcap',
        'marketcap_base': 'marketcap_base',
        'marketingonly': 'marketingonly',
        'masterid': 'masterid',
        'merged': 'merged',
        'modifiedby': 'modifiedby',
        'modifiedbyexternalparty': 'modifiedbyexternalparty',
        'modifiedon': 'modifiedon',
        'modifiedonbehalfby': 'modifiedonbehalfby',
        'name': 'name',
        'numberofemployees': 'numberofemployees',
        'onholdtime': 'onholdtime',
        'opendeals': 'opendeals',
        'opendeals_date': 'opendeals_date',
        'opendeals_state': 'opendeals_state',
        'openrevenue': 'openrevenue',
        'openrevenue_base': 'openrevenue_base',
        'openrevenue_date': 'openrevenue_date',
        'openrevenue_state': 'openrevenue_state',
        'originatingleadid': 'originatingleadid',
        'overriddencreatedon': 'overriddencreatedon',
        'ownerid': 'ownerid',
        'ownershipcode': 'ownershipcode',
        'owningbusinessunit': 'owningbusinessunit',
        'owningteam': 'owningteam',
        'owninguser': 'owninguser',
        'parentaccountid': 'parentaccountid',
        'participatesinworkflow': 'participatesinworkflow',
        'paymenttermscode': 'paymenttermscode',
        'preferredappointmentdaycode': 'preferredappointmentdaycode',
        'preferredappointmenttimecode': 'preferredappointmenttimecode',
        'preferredcontactmethodcode': 'preferredcontactmethodcode',
        'preferredequipmentid': 'preferredequipmentid',
        'preferredserviceid': 'preferredserviceid',
        'preferredsystemuserid': 'preferredsystemuserid',
        'primarycontactid': 'primarycontactid',
        'primarysatoriid': 'primarysatoriid',
        'primarytwitterid': 'primarytwitterid',
        'processid': 'processid',
        'revenue': 'revenue',
        'revenue_base': 'revenue_base',
        'sharesoutstanding': 'sharesoutstanding',
        'shippingmethodcode': 'shippingmethodcode',
        'sic': 'sic',
        'slaid': 'slaid',
        'slainvokedid': 'slainvokedid',
        'stageid': 'stageid',
        'statecode': 'statecode',
        'statuscode': 'statuscode',
        'stockexchange': 'stockexchange',
        'telephone1': 'telephone1',
        'telephone2': 'telephone2',
        'telephone3': 'telephone3',
        'territorycode': 'territorycode',
        'territoryid': 'territoryid',
        'tickersymbol': 'tickersymbol',
        'timespentbymeonemailandmeetings': 'timespentbymeonemailandmeetings',
        'timezoneruleversionnumber': 'timezoneruleversionnumber',
        'transactioncurrencyid': 'transactioncurrencyid',
        'traversedpath': 'traversedpath',
        'utcconversiontimezonecode': 'utcconversiontimezonecode',
        'versionnumber': 'versionnumber',
        'websiteurl': 'websiteurl',
        'yominame': 'yominame',
    },

    'StateCodes': {

        'Active_0': { 'value': 0, 'text': 'Active', 'displayOrder': 1, 'name1033': 'Active' },
        'Inactive_1': { 'value': 1, 'text': 'Inactive', 'displayOrder': 2, 'name1033': 'Inactive' },
    },

    'StatusCodes': {

        'Active_0_Active_1': { 'value': 1, 'text': 'Active', 'displayOrder': 1, 'name1033': 'Active', 'linkedstatecode': 0 },
        'Inactive_1_Inactive_2': { 'value': 2, 'text': 'Inactive', 'displayOrder': 2, 'name1033': 'Inactive', 'linkedstatecode': 1 },
    },

    // Attribute:
    //     accountcategorycode
    'accountcategorycodeEnum': {

        'Preferred_Customer_1': { 'value': 1, 'text': 'Preferred Customer', 'displayOrder': 1, 'name1033': 'Preferred Customer' },
        'Standard_2': { 'value': 2, 'text': 'Standard', 'displayOrder': 2, 'name1033': 'Standard' },
    },

    // Attribute:
    //     accountclassificationcode
    'accountclassificationcodeEnum': {

        'Default_Value_1': { 'value': 1, 'text': 'Default Value', 'displayOrder': 1, 'name1033': 'Default Value' },
    },

    // Attribute:
    //     accountratingcode
    'accountratingcodeEnum': {

        'Default_Value_1': { 'value': 1, 'text': 'Default Value', 'displayOrder': 1, 'name1033': 'Default Value' },
    },

    // Attribute:
    //     address1_addresstypecode
    'address1_addresstypecodeEnum': {

        'Bill_To_1': { 'value': 1, 'text': 'Bill To', 'displayOrder': 1, 'name1033': 'Bill To' },
        'Ship_To_2': { 'value': 2, 'text': 'Ship To', 'displayOrder': 2, 'name1033': 'Ship To' },
        'Primary_3': { 'value': 3, 'text': 'Primary', 'displayOrder': 3, 'name1033': 'Primary' },
        'Other_4': { 'value': 4, 'text': 'Other', 'displayOrder': 4, 'name1033': 'Other' },
    },

    // Attribute:
    //     address1_freighttermscode
    'address1_freighttermscodeEnum': {

        'FOB_1': { 'value': 1, 'text': 'FOB', 'displayOrder': 1, 'name1033': 'FOB' },
        'No_Charge_2': { 'value': 2, 'text': 'No Charge', 'displayOrder': 2, 'name1033': 'No Charge' },
    },

    // Attribute:
    //     address1_shippingmethodcode
    'address1_shippingmethodcodeEnum': {

        'Airborne_1': { 'value': 1, 'text': 'Airborne', 'displayOrder': 1, 'name1033': 'Airborne' },
        'DHL_2': { 'value': 2, 'text': 'DHL', 'displayOrder': 2, 'name1033': 'DHL' },
        'FedEx_3': { 'value': 3, 'text': 'FedEx', 'displayOrder': 3, 'name1033': 'FedEx' },
        'UPS_4': { 'value': 4, 'text': 'UPS', 'displayOrder': 4, 'name1033': 'UPS' },
        'Postal_Mail_5': { 'value': 5, 'text': 'Postal Mail', 'displayOrder': 5, 'name1033': 'Postal Mail' },
        'Full_Load_6': { 'value': 6, 'text': 'Full Load', 'displayOrder': 6, 'name1033': 'Full Load' },
        'Will_Call_7': { 'value': 7, 'text': 'Will Call', 'displayOrder': 7, 'name1033': 'Will Call' },
    },

    // Attribute:
    //     address2_addresstypecode
    'address2_addresstypecodeEnum': {

        'Default_Value_1': { 'value': 1, 'text': 'Default Value', 'displayOrder': 1, 'name1033': 'Default Value' },
    },

    // Attribute:
    //     address2_freighttermscode
    'address2_freighttermscodeEnum': {

        'Default_Value_1': { 'value': 1, 'text': 'Default Value', 'displayOrder': 1, 'name1033': 'Default Value' },
    },

    // Attribute:
    //     address2_shippingmethodcode
    'address2_shippingmethodcodeEnum': {

        'Default_Value_1': { 'value': 1, 'text': 'Default Value', 'displayOrder': 1, 'name1033': 'Default Value' },
    },

    // Attribute:
    //     businesstypecode
    'businesstypecodeEnum': {

        'Default_Value_1': { 'value': 1, 'text': 'Default Value', 'displayOrder': 1, 'name1033': 'Default Value' },
    },

    // Attribute:
    //     customersizecode
    'customersizecodeEnum': {

        'Default_Value_1': { 'value': 1, 'text': 'Default Value', 'displayOrder': 1, 'name1033': 'Default Value' },
    },

    // Attribute:
    //     customertypecode
    'customertypecodeEnum': {

        'Competitor_1': { 'value': 1, 'text': 'Competitor', 'displayOrder': 1, 'name1033': 'Competitor' },
        'Consultant_2': { 'value': 2, 'text': 'Consultant', 'displayOrder': 2, 'name1033': 'Consultant' },
        'Customer_3': { 'value': 3, 'text': 'Customer', 'displayOrder': 3, 'name1033': 'Customer' },
        'Investor_4': { 'value': 4, 'text': 'Investor', 'displayOrder': 4, 'name1033': 'Investor' },
        'Partner_5': { 'value': 5, 'text': 'Partner', 'displayOrder': 5, 'name1033': 'Partner' },
        'Influencer_6': { 'value': 6, 'text': 'Influencer', 'displayOrder': 6, 'name1033': 'Influencer' },
        'Press_7': { 'value': 7, 'text': 'Press', 'displayOrder': 7, 'name1033': 'Press' },
        'Prospect_8': { 'value': 8, 'text': 'Prospect', 'displayOrder': 8, 'name1033': 'Prospect' },
        'Reseller_9': { 'value': 9, 'text': 'Reseller', 'displayOrder': 9, 'name1033': 'Reseller' },
        'Supplier_10': { 'value': 10, 'text': 'Supplier', 'displayOrder': 10, 'name1033': 'Supplier' },
        'Vendor_11': { 'value': 11, 'text': 'Vendor', 'displayOrder': 11, 'name1033': 'Vendor' },
        'Other_12': { 'value': 12, 'text': 'Other', 'displayOrder': 12, 'name1033': 'Other' },
    },

    // Attribute:
    //     industrycode
    'industrycodeEnum': {

        'Accounting_1': { 'value': 1, 'text': 'Accounting', 'displayOrder': 1, 'name1033': 'Accounting' },
        'Agriculture_and_Non_petrol_Natural_Resource_Extraction_2': { 'value': 2, 'text': 'Agriculture and Non-petrol Natural Resource Extraction', 'displayOrder': 2, 'name1033': 'Agriculture and Non-petrol Natural Resource Extraction' },
        'Broadcasting_Printing_and_Publishing_3': { 'value': 3, 'text': 'Broadcasting Printing and Publishing', 'displayOrder': 3, 'name1033': 'Broadcasting Printing and Publishing' },
        'Brokers_4': { 'value': 4, 'text': 'Brokers', 'displayOrder': 4, 'name1033': 'Brokers' },
        'Building_Supply_Retail_5': { 'value': 5, 'text': 'Building Supply Retail', 'displayOrder': 5, 'name1033': 'Building Supply Retail' },
        'Business_Services_6': { 'value': 6, 'text': 'Business Services', 'displayOrder': 6, 'name1033': 'Business Services' },
        'Consulting_7': { 'value': 7, 'text': 'Consulting', 'displayOrder': 7, 'name1033': 'Consulting' },
        'Consumer_Services_8': { 'value': 8, 'text': 'Consumer Services', 'displayOrder': 8, 'name1033': 'Consumer Services' },
        'Design_Direction_and_Creative_Management_9': { 'value': 9, 'text': 'Design, Direction and Creative Management', 'displayOrder': 9, 'name1033': 'Design, Direction and Creative Management' },
        'Distributors_Dispatchers_and_Processors_10': { 'value': 10, 'text': 'Distributors, Dispatchers and Processors', 'displayOrder': 10, 'name1033': 'Distributors, Dispatchers and Processors' },
        'Doctor_s_Offices_and_Clinics_11': { 'value': 11, 'text': 'Doctor\'s Offices and Clinics', 'displayOrder': 11, 'name1033': 'Doctor\'s Offices and Clinics' },
        'Durable_Manufacturing_12': { 'value': 12, 'text': 'Durable Manufacturing', 'displayOrder': 12, 'name1033': 'Durable Manufacturing' },
        'Eating_and_Drinking_Places_13': { 'value': 13, 'text': 'Eating and Drinking Places', 'displayOrder': 13, 'name1033': 'Eating and Drinking Places' },
        'Entertainment_Retail_14': { 'value': 14, 'text': 'Entertainment Retail', 'displayOrder': 14, 'name1033': 'Entertainment Retail' },
        'Equipment_Rental_and_Leasing_15': { 'value': 15, 'text': 'Equipment Rental and Leasing', 'displayOrder': 15, 'name1033': 'Equipment Rental and Leasing' },
        'Financial_16': { 'value': 16, 'text': 'Financial', 'displayOrder': 16, 'name1033': 'Financial' },
        'Food_and_Tobacco_Processing_17': { 'value': 17, 'text': 'Food and Tobacco Processing', 'displayOrder': 17, 'name1033': 'Food and Tobacco Processing' },
        'Inbound_Capital_Intensive_Processing_18': { 'value': 18, 'text': 'Inbound Capital Intensive Processing', 'displayOrder': 18, 'name1033': 'Inbound Capital Intensive Processing' },
        'Inbound_Repair_and_Services_19': { 'value': 19, 'text': 'Inbound Repair and Services', 'displayOrder': 19, 'name1033': 'Inbound Repair and Services' },
        'Insurance_20': { 'value': 20, 'text': 'Insurance', 'displayOrder': 20, 'name1033': 'Insurance' },
        'Legal_Services_21': { 'value': 21, 'text': 'Legal Services', 'displayOrder': 21, 'name1033': 'Legal Services' },
        'Non_Durable_Merchandise_Retail_22': { 'value': 22, 'text': 'Non-Durable Merchandise Retail', 'displayOrder': 22, 'name1033': 'Non-Durable Merchandise Retail' },
        'Outbound_Consumer_Service_23': { 'value': 23, 'text': 'Outbound Consumer Service', 'displayOrder': 23, 'name1033': 'Outbound Consumer Service' },
        'Petrochemical_Extraction_and_Distribution_24': { 'value': 24, 'text': 'Petrochemical Extraction and Distribution', 'displayOrder': 24, 'name1033': 'Petrochemical Extraction and Distribution' },
        'Service_Retail_25': { 'value': 25, 'text': 'Service Retail', 'displayOrder': 25, 'name1033': 'Service Retail' },
        'SIG_Affiliations_26': { 'value': 26, 'text': 'SIG Affiliations', 'displayOrder': 26, 'name1033': 'SIG Affiliations' },
        'Social_Services_27': { 'value': 27, 'text': 'Social Services', 'displayOrder': 27, 'name1033': 'Social Services' },
        'Special_Outbound_Trade_Contractors_28': { 'value': 28, 'text': 'Special Outbound Trade Contractors', 'displayOrder': 28, 'name1033': 'Special Outbound Trade Contractors' },
        'Specialty_Realty_29': { 'value': 29, 'text': 'Specialty Realty', 'displayOrder': 29, 'name1033': 'Specialty Realty' },
        'Transportation_30': { 'value': 30, 'text': 'Transportation', 'displayOrder': 30, 'name1033': 'Transportation' },
        'Utility_Creation_and_Distribution_31': { 'value': 31, 'text': 'Utility Creation and Distribution', 'displayOrder': 31, 'name1033': 'Utility Creation and Distribution' },
        'Vehicle_Retail_32': { 'value': 32, 'text': 'Vehicle Retail', 'displayOrder': 32, 'name1033': 'Vehicle Retail' },
        'Wholesale_33': { 'value': 33, 'text': 'Wholesale', 'displayOrder': 33, 'name1033': 'Wholesale' },
    },

    // Attribute:
    //     ownershipcode
    'ownershipcodeEnum': {

        'Public_1': { 'value': 1, 'text': 'Public', 'displayOrder': 1, 'name1033': 'Public' },
        'Private_2': { 'value': 2, 'text': 'Private', 'displayOrder': 2, 'name1033': 'Private' },
        'Subsidiary_3': { 'value': 3, 'text': 'Subsidiary', 'displayOrder': 3, 'name1033': 'Subsidiary' },
        'Other_4': { 'value': 4, 'text': 'Other', 'displayOrder': 4, 'name1033': 'Other' },
    },

    // Attribute:
    //     paymenttermscode
    'paymenttermscodeEnum': {

        'Net_30_1': { 'value': 1, 'text': 'Net 30', 'displayOrder': 1, 'name1033': 'Net 30' },
        'V_2_10_Net_30_2': { 'value': 2, 'text': '2% 10, Net 30', 'displayOrder': 2, 'name1033': '2% 10, Net 30' },
        'Net_45_3': { 'value': 3, 'text': 'Net 45', 'displayOrder': 3, 'name1033': 'Net 45' },
        'Net_60_4': { 'value': 4, 'text': 'Net 60', 'displayOrder': 4, 'name1033': 'Net 60' },
    },

    // Attribute:
    //     preferredappointmentdaycode
    'preferredappointmentdaycodeEnum': {

        'Sunday_0': { 'value': 0, 'text': 'Sunday', 'displayOrder': 1, 'name1033': 'Sunday' },
        'Monday_1': { 'value': 1, 'text': 'Monday', 'displayOrder': 2, 'name1033': 'Monday' },
        'Tuesday_2': { 'value': 2, 'text': 'Tuesday', 'displayOrder': 3, 'name1033': 'Tuesday' },
        'Wednesday_3': { 'value': 3, 'text': 'Wednesday', 'displayOrder': 4, 'name1033': 'Wednesday' },
        'Thursday_4': { 'value': 4, 'text': 'Thursday', 'displayOrder': 5, 'name1033': 'Thursday' },
        'Friday_5': { 'value': 5, 'text': 'Friday', 'displayOrder': 6, 'name1033': 'Friday' },
        'Saturday_6': { 'value': 6, 'text': 'Saturday', 'displayOrder': 7, 'name1033': 'Saturday' },
    },

    // Attribute:
    //     preferredappointmenttimecode
    'preferredappointmenttimecodeEnum': {

        'Morning_1': { 'value': 1, 'text': 'Morning', 'displayOrder': 1, 'name1033': 'Morning' },
        'Afternoon_2': { 'value': 2, 'text': 'Afternoon', 'displayOrder': 2, 'name1033': 'Afternoon' },
        'Evening_3': { 'value': 3, 'text': 'Evening', 'displayOrder': 3, 'name1033': 'Evening' },
    },

    // Attribute:
    //     preferredcontactmethodcode
    'preferredcontactmethodcodeEnum': {

        'Any_1': { 'value': 1, 'text': 'Any', 'displayOrder': 1, 'name1033': 'Any' },
        'Email_2': { 'value': 2, 'text': 'Email', 'displayOrder': 2, 'name1033': 'Email' },
        'Phone_3': { 'value': 3, 'text': 'Phone', 'displayOrder': 3, 'name1033': 'Phone' },
        'Fax_4': { 'value': 4, 'text': 'Fax', 'displayOrder': 4, 'name1033': 'Fax' },
        'Mail_5': { 'value': 5, 'text': 'Mail', 'displayOrder': 5, 'name1033': 'Mail' },
    },

    // Attribute:
    //     shippingmethodcode
    'shippingmethodcodeEnum': {

        'Default_Value_1': { 'value': 1, 'text': 'Default Value', 'displayOrder': 1, 'name1033': 'Default Value' },
    },

    // Attribute:
    //     territorycode
    'territorycodeEnum': {

        'Default_Value_1': { 'value': 1, 'text': 'Default Value', 'displayOrder': 1, 'name1033': 'Default Value' },
    },
}