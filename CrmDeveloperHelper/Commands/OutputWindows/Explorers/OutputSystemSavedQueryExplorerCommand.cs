using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputSystemSavedQueryExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputSystemSavedQueryExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputSystemSavedQueryExplorerCommandId) { }

        public static OutputSystemSavedQueryExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputSystemSavedQueryExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleExplorerSystemSavedQuery(connectionData);
        }
    }
}
