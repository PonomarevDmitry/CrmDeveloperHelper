using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckWorkflowsWithEntityFieldStringsCommand : AbstractSingleCommand
    {
        private readonly bool _openExplorer;

        private CommonCheckWorkflowsWithEntityFieldStringsCommand(OleMenuCommandService commandService, int idCommand, bool openExplorer)
           : base(commandService, idCommand)
        {
            this._openExplorer = openExplorer;
        }

        public static CommonCheckWorkflowsWithEntityFieldStringsCommand Instance { get; private set; }

        public static CommonCheckWorkflowsWithEntityFieldStringsCommand InstanceOpenExplorer { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckWorkflowsWithEntityFieldStringsCommand(commandService, PackageIds.guidCommandSet.CommonCheckWorkflowsWithEntityFieldStringsCommandId, false);

            InstanceOpenExplorer = new CommonCheckWorkflowsWithEntityFieldStringsCommand(commandService, PackageIds.guidCommandSet.CommonCheckWorkflowsWithEntityFieldStringsOpenExplorerCommandId, true);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleCheckingWorkflowsWithEntityFieldStrings(_openExplorer);
        }
    }
}
