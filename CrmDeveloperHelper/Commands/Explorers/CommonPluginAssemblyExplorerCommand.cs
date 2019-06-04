using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonPluginAssemblyExplorerCommand : AbstractCommand
    {
        private CommonPluginAssemblyExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportPluginAssemblyExplorerCommandId, ActionExecute, null) { }

        public static CommonPluginAssemblyExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonPluginAssemblyExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenPluginAssemblyExplorer();
        }
    }
}
