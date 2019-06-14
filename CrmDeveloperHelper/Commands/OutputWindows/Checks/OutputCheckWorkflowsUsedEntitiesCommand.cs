using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks
{
    internal sealed class OutputCheckWorkflowsUsedEntitiesCommand : AbstractCommand
    {
        private OutputCheckWorkflowsUsedEntitiesCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputCheckWorkflowsUsedEntitiesCommandId) { }

        public static OutputCheckWorkflowsUsedEntitiesCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCheckWorkflowsUsedEntitiesCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var connectionData = helper.GetOutputWindowConnection();

            if (connectionData == null)
            {
                return;
            }

            helper.HandleCheckingWorkflowsUsedEntities(connectionData);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusIsConnectionOutput(applicationObject, menuCommand);
        }
    }
}
