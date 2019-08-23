using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckWorkflowsUsedNotExistsEntitiesCommand : AbstractCommand
    {
        private CommonCheckWorkflowsUsedNotExistsEntitiesCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonCheckWorkflowsUsedNotExistsEntitiesCommandId) { }

        public static CommonCheckWorkflowsUsedNotExistsEntitiesCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckWorkflowsUsedNotExistsEntitiesCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleCheckingWorkflowsNotExistingUsedEntities();
        }
    }
}
