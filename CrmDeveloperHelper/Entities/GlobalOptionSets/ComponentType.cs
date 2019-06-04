namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [System.ComponentModel.DescriptionAttribute("Component Type")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum ComponentType
    {
        [System.ComponentModel.DescriptionAttribute("Entity")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Entity = 1,

        [System.ComponentModel.DescriptionAttribute("Attribute")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Attribute = 2,

        [System.ComponentModel.DescriptionAttribute("Relationship")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Relationship = 3,

        [System.ComponentModel.DescriptionAttribute("Attribute Picklist Value")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AttributePicklistValue = 4,

        [System.ComponentModel.DescriptionAttribute("Attribute Lookup Value")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AttributeLookupValue = 5,

        [System.ComponentModel.DescriptionAttribute("View Attribute")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ViewAttribute = 6,

        [System.ComponentModel.DescriptionAttribute("Localized Label")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        LocalizedLabel = 7,

        [System.ComponentModel.DescriptionAttribute("Relationship Extra Condition")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        RelationshipExtraCondition = 8,

        [System.ComponentModel.DescriptionAttribute("Option Set")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        OptionSet = 9,

        [System.ComponentModel.DescriptionAttribute("Entity Relationship")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EntityRelationship = 10,

        [System.ComponentModel.DescriptionAttribute("Entity Relationship Role")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EntityRelationshipRole = 11,

        [System.ComponentModel.DescriptionAttribute("Entity Relationship Relationships")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EntityRelationshipRelationships = 12,

        [System.ComponentModel.DescriptionAttribute("Managed Property")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ManagedProperty = 13,

        [System.ComponentModel.DescriptionAttribute("Entity Key")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EntityKey = 14,

        [System.ComponentModel.DescriptionAttribute("Entity Key Attribute")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EntityKeyAttribute = 15,

        [System.ComponentModel.DescriptionAttribute("Privilege")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Privilege = 16,

        [System.ComponentModel.DescriptionAttribute("Privilege ObjectTypeCode")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        PrivilegeObjectTypeCode = 17,

        [System.ComponentModel.DescriptionAttribute("Index")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Index = 18,

        [System.ComponentModel.DescriptionAttribute("Role")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Role = 20,

        [System.ComponentModel.DescriptionAttribute("Role Privilege")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        RolePrivileges = 21,

        [System.ComponentModel.DescriptionAttribute("Display String")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DisplayString = 22,

        [System.ComponentModel.DescriptionAttribute("Display String Map")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DisplayStringMap = 23,

        [System.ComponentModel.DescriptionAttribute("Form")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Form = 24,

        [System.ComponentModel.DescriptionAttribute("Organization")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Organization = 25,

        [System.ComponentModel.DescriptionAttribute("Saved Query")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SavedQuery = 26,

        [System.ComponentModel.DescriptionAttribute("Workflow")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Workflow = 29,

        [System.ComponentModel.DescriptionAttribute("Process Trigger")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ProcessTrigger = 30,

        [System.ComponentModel.DescriptionAttribute("Report")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Report = 31,

        [System.ComponentModel.DescriptionAttribute("Report Entity")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ReportEntity = 32,

        [System.ComponentModel.DescriptionAttribute("Report Category")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ReportCategory = 33,

        [System.ComponentModel.DescriptionAttribute("Report Visibility")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ReportVisibility = 34,

        [System.ComponentModel.DescriptionAttribute("Attachment")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Attachment = 35,

        [System.ComponentModel.DescriptionAttribute("Email Template")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EmailTemplate = 36,

        [System.ComponentModel.DescriptionAttribute("Contract Template")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ContractTemplate = 37,

        [System.ComponentModel.DescriptionAttribute("KB Article Template")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        KbArticleTemplate = 38,

        [System.ComponentModel.DescriptionAttribute("Mail Merge Template")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        MailMergeTemplate = 39,

        [System.ComponentModel.DescriptionAttribute("Duplicate Rule")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DuplicateRule = 44,

        [System.ComponentModel.DescriptionAttribute("Duplicate Rule Condition")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DuplicateRuleCondition = 45,

        [System.ComponentModel.DescriptionAttribute("Entity Map")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EntityMap = 46,

        [System.ComponentModel.DescriptionAttribute("Attribute Map")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AttributeMap = 47,

        [System.ComponentModel.DescriptionAttribute("Ribbon Command")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        RibbonCommand = 48,

        [System.ComponentModel.DescriptionAttribute("Ribbon Context Group")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        RibbonContextGroup = 49,

        [System.ComponentModel.DescriptionAttribute("Ribbon Customization")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        RibbonCustomization = 50,

        [System.ComponentModel.DescriptionAttribute("Ribbon Rule")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        RibbonRule = 52,

        [System.ComponentModel.DescriptionAttribute("Ribbon Tab To Command Map")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        RibbonTabToCommandMap = 53,

        [System.ComponentModel.DescriptionAttribute("Ribbon Diff")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        RibbonDiff = 55,

        [System.ComponentModel.DescriptionAttribute("Saved Query Visualization")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SavedQueryVisualization = 59,

        [System.ComponentModel.DescriptionAttribute("System Form")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SystemForm = 60,

        [System.ComponentModel.DescriptionAttribute("Web Resource")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        WebResource = 61,

        [System.ComponentModel.DescriptionAttribute("Site Map")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SiteMap = 62,

        [System.ComponentModel.DescriptionAttribute("Connection Role")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ConnectionRole = 63,

        [System.ComponentModel.DescriptionAttribute("Complex Control")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ComplexControl = 64,

        [System.ComponentModel.DescriptionAttribute("Field Security Profile")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FieldSecurityProfile = 70,

        [System.ComponentModel.DescriptionAttribute("Field Permission")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FieldPermission = 71,

        [System.ComponentModel.DescriptionAttribute("AppModule")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AppModule = 80,

        [System.ComponentModel.DescriptionAttribute("AppModule Roles")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AppModuleRoles = 88,

        [System.ComponentModel.DescriptionAttribute("Plugin Type")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        PluginType = 90,

        [System.ComponentModel.DescriptionAttribute("Plugin Assembly")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        PluginAssembly = 91,

        [System.ComponentModel.DescriptionAttribute("SDK Message Processing Step")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SdkMessageProcessingStep = 92,

        [System.ComponentModel.DescriptionAttribute("SDK Message Processing Step Image")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SdkMessageProcessingStepImage = 93,

        [System.ComponentModel.DescriptionAttribute("Service Endpoint")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ServiceEndpoint = 95,

        [System.ComponentModel.DescriptionAttribute("Routing Rule")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        RoutingRule = 150,

        [System.ComponentModel.DescriptionAttribute("Routing Rule Item")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        RoutingRuleItem = 151,

        [System.ComponentModel.DescriptionAttribute("SLA")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SLA = 152,

        [System.ComponentModel.DescriptionAttribute("SLA Item")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SLAItem = 153,

        [System.ComponentModel.DescriptionAttribute("Convert Rule")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ConvertRule = 154,

        [System.ComponentModel.DescriptionAttribute("Convert Rule Item")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ConvertRuleItem = 155,

        [System.ComponentModel.DescriptionAttribute("Hierarchy Rule")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        HierarchyRule = 65,

        [System.ComponentModel.DescriptionAttribute("Mobile Offline Profile")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        MobileOfflineProfile = 161,

        [System.ComponentModel.DescriptionAttribute("Mobile Offline Profile Item")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        MobileOfflineProfileItem = 162,

        [System.ComponentModel.DescriptionAttribute("Mobile Offline Profile Item Association")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        MobileOfflineProfileItemAssociation = 163,

        [System.ComponentModel.DescriptionAttribute("Similarity Rule")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SimilarityRule = 165,

        [System.ComponentModel.DescriptionAttribute("Custom Control")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CustomControl = 66,

        [System.ComponentModel.DescriptionAttribute("Custom Control Default Config")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CustomControlDefaultConfig = 68,

        [System.ComponentModel.DescriptionAttribute("Custom Control Resource")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CustomControlResource = 69,

        [System.ComponentModel.DescriptionAttribute("Channel Access Profile Entity Access Level")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ChannelAccessProfileEntityAccessLevel = 171,

        [System.ComponentModel.DescriptionAttribute("Channel Access Profile")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ChannelAccessProfile = 172,

        [System.ComponentModel.DescriptionAttribute("DependencyFeature")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DependencyFeature = 160,

        [System.ComponentModel.DescriptionAttribute("Entity Data Provider")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EntityDataProvider = 181,

        [System.ComponentModel.DescriptionAttribute("Entity Data Source")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EntityDataSource = 183,

        [System.ComponentModel.DescriptionAttribute("SDKMessage")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SdkMessage = 201,

        [System.ComponentModel.DescriptionAttribute("SdkMessageFilter")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SdkMessageFilter = 202,

        [System.ComponentModel.DescriptionAttribute("SdkMessagePair")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SdkMessagePair = 203,

        [System.ComponentModel.DescriptionAttribute("SdkMessageRequest")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SdkMessageRequest = 204,

        [System.ComponentModel.DescriptionAttribute("SdkMessageRequest Field")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SdkMessageRequestField = 205,

        [System.ComponentModel.DescriptionAttribute("SdkMessageResponse")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SdkMessageResponse = 206,

        [System.ComponentModel.DescriptionAttribute("SdkMessageResponse Field")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SdkMessageResponseField = 207,

        [System.ComponentModel.DescriptionAttribute("WebWizard")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        WebWizard = 210,

        [System.ComponentModel.DescriptionAttribute("Import Map")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ImportMap = 208,

        [System.ComponentModel.DescriptionAttribute("Import Entity Mapping")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ImportEntityMapping = 166,
    }
}