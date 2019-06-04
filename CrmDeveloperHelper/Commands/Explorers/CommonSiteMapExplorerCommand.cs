using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonSiteMapExplorerCommand : AbstractCommand
    {
        private CommonSiteMapExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonSiteMapExplorerCommandId, ActionExecute, null) { }

        public static CommonSiteMapExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonSiteMapExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportSitemap();
        }
    }
}