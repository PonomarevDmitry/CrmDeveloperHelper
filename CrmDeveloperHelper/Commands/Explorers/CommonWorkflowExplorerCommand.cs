using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonWorkflowExplorerCommand : AbstractCommand
    {
        private CommonWorkflowExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportWorkflowCommandId, ActionExecute, null) { }

        public static CommonWorkflowExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonWorkflowExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportWorkflows();
        }
    }
}
