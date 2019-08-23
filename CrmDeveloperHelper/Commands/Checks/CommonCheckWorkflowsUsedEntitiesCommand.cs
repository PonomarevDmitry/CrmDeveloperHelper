using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckWorkflowsUsedEntitiesCommand : AbstractCommand
    {
        private CommonCheckWorkflowsUsedEntitiesCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonCheckWorkflowsUsedEntitiesCommandId) { }

        public static CommonCheckWorkflowsUsedEntitiesCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckWorkflowsUsedEntitiesCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleCheckingWorkflowsUsedEntities();
        }
    }
}
