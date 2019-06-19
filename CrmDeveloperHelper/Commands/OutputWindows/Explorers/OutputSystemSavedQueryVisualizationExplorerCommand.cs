using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputSystemSavedQueryVisualizationExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputSystemSavedQueryVisualizationExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputSystemSavedQueryVisualizationExplorerCommandId) { }

        public static OutputSystemSavedQueryVisualizationExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputSystemSavedQueryVisualizationExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleExportSystemSavedQueryVisualization(connectionData);
        }
    }
}
