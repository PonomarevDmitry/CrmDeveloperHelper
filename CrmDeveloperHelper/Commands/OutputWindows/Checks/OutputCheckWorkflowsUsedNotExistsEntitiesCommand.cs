using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks
{
    internal sealed class OutputCheckWorkflowsUsedNotExistsEntitiesCommand : AbstractOutputWindowCommand
    {
        private OutputCheckWorkflowsUsedNotExistsEntitiesCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputCheckWorkflowsUsedNotExistsEntitiesCommandId) { }

        public static OutputCheckWorkflowsUsedNotExistsEntitiesCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCheckWorkflowsUsedNotExistsEntitiesCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckingWorkflowsNotExistingUsedEntities(connectionData);
        }
    }
}
