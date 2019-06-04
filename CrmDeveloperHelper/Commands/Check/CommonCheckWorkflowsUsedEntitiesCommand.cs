using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonCheckWorkflowsUsedEntitiesCommand : AbstractCommand
    {
        private CommonCheckWorkflowsUsedEntitiesCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonCheckWorkflowsUsedEntitiesCommandId, ActionExecute, null) { }

        public static CommonCheckWorkflowsUsedEntitiesCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonCheckWorkflowsUsedEntitiesCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleCheckingWorkflowsUsedEntities();
        }
    }
}
