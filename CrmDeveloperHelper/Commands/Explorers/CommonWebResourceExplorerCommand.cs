using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class CommonWebResourceExplorerCommand : AbstractCommand
    {
        private CommonWebResourceExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonWebResourceExplorerCommandId, ActionExecute, null) { }

        public static CommonWebResourceExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonWebResourceExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportWebResource();
        }
    }
}
