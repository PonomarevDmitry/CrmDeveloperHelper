using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportWorkflowCommand : AbstractCommand
    {
        private CommonExportWorkflowCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportWorkflowCommandId, ActionExecute, null) { }

        public static CommonExportWorkflowCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportWorkflowCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportWorkflows();
        }
    }
}
