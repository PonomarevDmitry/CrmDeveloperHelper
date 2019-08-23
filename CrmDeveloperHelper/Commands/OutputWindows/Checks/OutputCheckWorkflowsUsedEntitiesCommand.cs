using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks
{
    internal sealed class OutputCheckWorkflowsUsedEntitiesCommand : AbstractOutputWindowCommand
    {
        private OutputCheckWorkflowsUsedEntitiesCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputCheckWorkflowsUsedEntitiesCommandId) { }

        public static OutputCheckWorkflowsUsedEntitiesCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCheckWorkflowsUsedEntitiesCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckingWorkflowsUsedEntities(connectionData);
        }
    }
}
