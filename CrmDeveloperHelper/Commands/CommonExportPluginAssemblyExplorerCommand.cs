using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportPluginAssemblyExplorerCommand : AbstractCommand
    {
        private CommonExportPluginAssemblyExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportPluginAssemblyExplorerCommandId, ActionExecute, null) { }

        public static CommonExportPluginAssemblyExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportPluginAssemblyExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenPluginAssemblyExplorer();
        }
    }
}
