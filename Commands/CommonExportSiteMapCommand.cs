using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportSiteMapCommand : AbstractCommand
    {
        private CommonExportSiteMapCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportSiteMapCommandId, ActionExecute, null) { }

        public static CommonExportSiteMapCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportSiteMapCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportSitemap();
        }
    }
}