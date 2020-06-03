namespace Nav.Common.VSPackages.CrmDeveloperHelper.Entities.GlobalOptionSets
{
    [System.ComponentModel.DescriptionAttribute("Availability")]
    [System.ComponentModel.TypeConverterAttribute("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum AvailabilityEnum
    {
        [System.ComponentModel.DescriptionAttribute("Server")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Server = 0,

        [System.ComponentModel.DescriptionAttribute("Client")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Client = 1,

        [System.ComponentModel.DescriptionAttribute("Both")]
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Both = 2,
    }
}