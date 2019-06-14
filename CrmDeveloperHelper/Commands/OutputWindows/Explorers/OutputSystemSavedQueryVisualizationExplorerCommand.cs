using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputSystemSavedQueryVisualizationExplorerCommand : AbstractCommand
    {
        private OutputSystemSavedQueryVisualizationExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputSystemSavedQueryVisualizationExplorerCommandId) { }

        public static OutputSystemSavedQueryVisualizationExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputSystemSavedQueryVisualizationExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var connectionData = helper.GetOutputWindowConnection();

            if (connectionData == null)
            {
                return;
            }

            helper.HandleExportSystemSavedQueryVisualization(connectionData);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusIsConnectionOutput(applicationObject, menuCommand);
        }
    }
}
