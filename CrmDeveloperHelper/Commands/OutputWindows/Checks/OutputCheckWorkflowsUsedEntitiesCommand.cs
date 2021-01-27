using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks
{
    internal sealed class OutputCheckWorkflowsUsedEntitiesCommand : AbstractOutputWindowCommand
    {
        private readonly bool _openExplorer;

        private OutputCheckWorkflowsUsedEntitiesCommand(OleMenuCommandService commandService, int idCommand, bool openExplorer)
           : base(commandService, idCommand)
        {
            this._openExplorer = openExplorer;
        }

        public static OutputCheckWorkflowsUsedEntitiesCommand Instance { get; private set; }

        public static OutputCheckWorkflowsUsedEntitiesCommand InstanceOpenExplorer { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCheckWorkflowsUsedEntitiesCommand(commandService, PackageIds.guidCommandSet.OutputCheckWorkflowsUsedEntitiesCommandId, false);

            InstanceOpenExplorer = new OutputCheckWorkflowsUsedEntitiesCommand(commandService, PackageIds.guidCommandSet.OutputCheckWorkflowsUsedEntitiesOpenExplorerCommandId, true);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckingWorkflowsUsedEntities(connectionData, _openExplorer);
        }
    }
}
