using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckWorkflowsWithEntityFieldStringsCommand : AbstractSingleCommand
    {
        private CommonCheckWorkflowsWithEntityFieldStringsCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonCheckWorkflowsWithEntityFieldStringsCommandId)
        {
        }

        public static CommonCheckWorkflowsWithEntityFieldStringsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckWorkflowsWithEntityFieldStringsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleCheckingWorkflowsWithEntityFieldStrings();
        }
    }
}
