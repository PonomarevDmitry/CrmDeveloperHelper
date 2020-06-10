using System.ComponentModel;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [TypeConverter("Nav.Common.VSPackages.CrmDeveloperHelper.Helpers.EnumDescriptionTypeConverter")]
    public enum ActionOnComponent
    {
        [Description("None")]
        None = 0,

        [Description("Opening in Web")]
        OpenInWeb = 1,

        [Description("Opening in Explorer")]
        OpenInExplorer = 2,

        [Description("Opening Dependent Components in Web")]
        OpenDependentComponentsInWeb = 3,

        [Description("Opening Dependent Components in Explorer")]
        OpenDependentComponentsInExplorer = 4,

        [Description("Opening Solutions List with Component in Explorer")]
        OpenSolutionsListWithComponentInExplorer = 5,

        [Description("Opening List in Web")]
        OpenListInWeb = 6,

        [Description("Getting Single Field")]
        SingleField = 7,

        [Description("Getting EntityDescription")]
        EntityDescription = 8,

        [Description("Getting Description")]
        Description = 9,

        [Description("Getting Single Xml Field")]
        SingleXmlField = 10,
    }
}