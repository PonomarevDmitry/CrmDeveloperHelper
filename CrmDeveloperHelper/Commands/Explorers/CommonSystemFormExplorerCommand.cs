using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonSystemFormExplorerCommand : AbstractCommand
    {
        private CommonSystemFormExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonSystemFormExplorerCommandId, ActionExecute, null) { }

        public static CommonSystemFormExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonSystemFormExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExplorerSystemForm();
        }
    }
}
