using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputImportJobExplorerInConnectionCommand : AbstractOutputWindowCommand
    {
        private OutputImportJobExplorerInConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.OutputImportJobExplorerInConnectionCommandId
            )
        {

        }

        public static OutputImportJobExplorerInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputImportJobExplorerInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenImportJobExplorerWindow(connectionData);
        }
    }
}
