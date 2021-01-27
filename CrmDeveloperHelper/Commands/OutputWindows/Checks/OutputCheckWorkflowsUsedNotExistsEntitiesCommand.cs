using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks
{
    internal sealed class OutputCheckWorkflowsUsedNotExistsEntitiesCommand : AbstractOutputWindowCommand
    {
        private readonly bool _openExplorer;

        private OutputCheckWorkflowsUsedNotExistsEntitiesCommand(OleMenuCommandService commandService, int idCommand, bool openExplorer)
           : base(commandService, idCommand)
        {
            this._openExplorer = openExplorer;
        }

        public static OutputCheckWorkflowsUsedNotExistsEntitiesCommand Instance { get; private set; }

        public static OutputCheckWorkflowsUsedNotExistsEntitiesCommand InstanceOpenExplorer { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCheckWorkflowsUsedNotExistsEntitiesCommand(commandService, PackageIds.guidCommandSet.OutputCheckWorkflowsUsedNotExistsEntitiesCommandId, false);

            InstanceOpenExplorer = new OutputCheckWorkflowsUsedNotExistsEntitiesCommand(commandService, PackageIds.guidCommandSet.OutputCheckWorkflowsUsedNotExistsEntitiesOpenExplorerCommandId, true);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckingWorkflowsNotExistingUsedEntities(connectionData, _openExplorer);
        }
    }
}
