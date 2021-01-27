using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks
{
    internal sealed class OutputCheckWorkflowsWithEntityFieldStringsCommand : AbstractOutputWindowCommand
    {
        private readonly bool _openExplorer;

        private OutputCheckWorkflowsWithEntityFieldStringsCommand(OleMenuCommandService commandService, int idCommand, bool openExplorer)
           : base(commandService, idCommand)
        {
            this._openExplorer = openExplorer;
        }

        public static OutputCheckWorkflowsWithEntityFieldStringsCommand Instance { get; private set; }

        public static OutputCheckWorkflowsWithEntityFieldStringsCommand InstanceOpenExplorer { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCheckWorkflowsWithEntityFieldStringsCommand(commandService, PackageIds.guidCommandSet.OutputCheckWorkflowsWithEntityFieldStringsCommandId, false);

            InstanceOpenExplorer = new OutputCheckWorkflowsWithEntityFieldStringsCommand(commandService, PackageIds.guidCommandSet.OutputCheckWorkflowsWithEntityFieldStringsOpenExplorerCommandId, false);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckingWorkflowsWithEntityFieldStrings(connectionData, _openExplorer);
        }
    }
}
