using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonSystemSavedQueryVisualizationExplorerCommand : AbstractCommand
    {
        private CommonSystemSavedQueryVisualizationExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonSystemSavedQueryVisualizationExplorerCommandId) { }

        public static CommonSystemSavedQueryVisualizationExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonSystemSavedQueryVisualizationExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleExportSystemSavedQueryVisualization();
        }
    }
}
