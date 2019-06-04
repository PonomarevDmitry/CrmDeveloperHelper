using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonGlobalOptionSetsExplorerCommand : AbstractCommand
    {
        private CommonGlobalOptionSetsExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportGlobalOptionSetsCommandId, ActionExecute, null) { }

        public static CommonGlobalOptionSetsExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonGlobalOptionSetsExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportGlobalOptionSets();
        }
    }
}
