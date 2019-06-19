using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputSystemUsersExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputSystemUsersExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputSystemUsersExplorerCommandId) { }

        public static OutputSystemUsersExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputSystemUsersExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenSystemUsersExplorer(connectionData);
        }
    }
}
