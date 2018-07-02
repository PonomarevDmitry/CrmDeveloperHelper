using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportGlobalOptionSetsCommand : AbstractCommand
    {
        private CommonExportGlobalOptionSetsCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportGlobalOptionSetsCommandId, ActionExecute, null) { }

        public static CommonExportGlobalOptionSetsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportGlobalOptionSetsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportGlobalOptionSets();
        }
    }
}
