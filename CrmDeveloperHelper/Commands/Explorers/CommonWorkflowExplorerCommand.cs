using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonWorkflowExplorerCommand : AbstractSingleCommand
    {
        private CommonWorkflowExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonWorkflowExplorerCommandId) { }

        public static CommonWorkflowExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonWorkflowExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleExplorerWorkflows();
        }
    }
}
