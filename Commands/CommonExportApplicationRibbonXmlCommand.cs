using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportApplicationRibbonXmlCommand : AbstractCommand
    {
        private CommonExportApplicationRibbonXmlCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportApplicationRibbonXmlCommandId, ActionExecute, null) { }

        public static CommonExportApplicationRibbonXmlCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportApplicationRibbonXmlCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportRibbon();
        }
    }
}
