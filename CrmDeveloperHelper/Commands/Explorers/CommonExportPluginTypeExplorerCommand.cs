using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportPluginTypeExplorerCommand : AbstractCommand
    {
        private CommonExportPluginTypeExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportPluginTypeExplorerCommandId, ActionExecute, null) { }

        public static CommonExportPluginTypeExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportPluginTypeExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            string selection = helper.GetSelectedText();

            helper.HandleOpenPluginTypeExplorer(selection);
        }
    }
}
