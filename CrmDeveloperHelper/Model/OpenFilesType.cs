using System.ComponentModel;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [TypeConverter("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum OpenFilesType
    {
        [Description("All")]
        All = 0,

        [Description("Equal by Text")]
        EqualByText,

        [Description("Not Equal by Text")]
        NotEqualByText,

        [Description("With Inserts")]
        WithInserts,

        [Description("With Deletes")]
        WithDeletes,

        [Description("With Complex Changes")]
        WithComplexChanges,

        [Description("Mirror Changes")]
        WithMirrorChanges,

        [Description("Mirror with Inserts")]
        WithMirrorInserts,

        [Description("Mirror with Deletes")]
        WithMirrorDeletes,

        [Description("Mirror with Complex Changes")]
        WithMirrorComplexChanges,

        [Description("Not Exists in Crm without Link")]
        NotExistsInCrmWithoutLink,

        [Description("Not Exists in Crm with Link")]
        NotExistsInCrmWithLink
    }
}