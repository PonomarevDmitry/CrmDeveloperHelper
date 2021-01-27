using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckWorkflowsUsedEntitiesCommand : AbstractSingleCommand
    {
        private readonly bool _openExplorer;

        private CommonCheckWorkflowsUsedEntitiesCommand(OleMenuCommandService commandService, int idCommand, bool openExplorer)
           : base(commandService, idCommand) 
        {
            this._openExplorer = openExplorer;
        }

        public static CommonCheckWorkflowsUsedEntitiesCommand Instance { get; private set; }

        public static CommonCheckWorkflowsUsedEntitiesCommand InstanceOpenExplorer { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckWorkflowsUsedEntitiesCommand(commandService, PackageIds.guidCommandSet.CommonCheckWorkflowsUsedEntitiesCommandId, false);

            InstanceOpenExplorer = new CommonCheckWorkflowsUsedEntitiesCommand(commandService, PackageIds.guidCommandSet.CommonCheckWorkflowsUsedEntitiesOpenExplorerCommandId, true);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleCheckingWorkflowsUsedEntities(_openExplorer);
        }
    }
}
