using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonPluginTypeExplorerCommand : AbstractCommand
    {
        private CommonPluginTypeExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonPluginTypeExplorerCommandId, ActionExecute, null) { }

        public static CommonPluginTypeExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonPluginTypeExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            string selection = helper.GetSelectedText();

            helper.HandleOpenPluginTypeExplorer(selection);
        }
    }
}
