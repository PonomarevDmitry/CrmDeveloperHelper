using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonSystemSavedQueryExplorerCommand : AbstractCommand
    {
        private CommonSystemSavedQueryExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonSystemSavedQueryExplorerCommandId, ActionExecute, null) { }

        public static CommonSystemSavedQueryExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonSystemSavedQueryExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportSystemSavedQuery();
        }
    }
}
