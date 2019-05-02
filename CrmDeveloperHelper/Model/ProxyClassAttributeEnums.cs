using System.ComponentModel;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [TypeConverter("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum ProxyClassAttributeEnums
    {
        [Description("Not Needed")]
        NotNeeded = 0,

        [Description("Add with name {Attribute}Enum")]
        AddWithNameAttributeEnum = 1,

        [Description("Instead of OptionSetValue")]
        InsteadOfOptionSet = 2,
    }
}