using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonCheckWorkflowsUsedNotExistsEntitiesCommand : AbstractCommand
    {
        private CommonCheckWorkflowsUsedNotExistsEntitiesCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonCheckWorkflowsUsedNotExistsEntitiesCommandId, ActionExecute, null) { }

        public static CommonCheckWorkflowsUsedNotExistsEntitiesCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonCheckWorkflowsUsedNotExistsEntitiesCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleCheckingWorkflowsNotExistingUsedEntities();
        }
    }
}
