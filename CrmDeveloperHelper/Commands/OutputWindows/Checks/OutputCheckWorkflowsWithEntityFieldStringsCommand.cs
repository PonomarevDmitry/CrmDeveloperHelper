using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks
{
    internal sealed class OutputCheckWorkflowsWithEntityFieldStringsCommand : AbstractOutputWindowCommand
    {
        private OutputCheckWorkflowsWithEntityFieldStringsCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputCheckWorkflowsWithEntityFieldStringsCommandId)
        {
        }

        public static OutputCheckWorkflowsWithEntityFieldStringsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCheckWorkflowsWithEntityFieldStringsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckingWorkflowsWithEntityFieldStrings(connectionData);
        }
    }
}
