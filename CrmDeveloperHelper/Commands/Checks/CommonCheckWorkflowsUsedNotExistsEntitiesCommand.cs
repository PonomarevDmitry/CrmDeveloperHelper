using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckWorkflowsUsedNotExistsEntitiesCommand : AbstractSingleCommand
    {
        private readonly bool _openExplorer;

        private CommonCheckWorkflowsUsedNotExistsEntitiesCommand(OleMenuCommandService commandService, int idCommand, bool openExplorer)
           : base(commandService, idCommand) 
        {
            this._openExplorer = openExplorer;
        }

        public static CommonCheckWorkflowsUsedNotExistsEntitiesCommand Instance { get; private set; }

        public static CommonCheckWorkflowsUsedNotExistsEntitiesCommand InstanceOpenExplorer { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckWorkflowsUsedNotExistsEntitiesCommand(commandService, PackageIds.guidCommandSet.CommonCheckWorkflowsUsedNotExistsEntitiesCommandId, false);

            InstanceOpenExplorer = new CommonCheckWorkflowsUsedNotExistsEntitiesCommand(commandService, PackageIds.guidCommandSet.CommonCheckWorkflowsUsedNotExistsEntitiesOpenExplorerCommandId, true);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleCheckingWorkflowsNotExistingUsedEntities(_openExplorer);
        }
    }
}
