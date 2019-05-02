using System.ComponentModel;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [TypeConverter("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum ProxyClassAttributeEnumsGlobalOptionSetLocation
    {
        [Description("In Global OptionSet Namespace")]
        InGlobalOptionSetNamespace = 0,

        [Description("In Classes Namespace")]
        InClassesNamespace = 1,

        [Description("In Class Schema")]
        InClassSchema = 2,
    }
}