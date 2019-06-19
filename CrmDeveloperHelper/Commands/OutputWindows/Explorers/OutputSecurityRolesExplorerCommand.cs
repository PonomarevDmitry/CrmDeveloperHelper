using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputSecurityRolesExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputSecurityRolesExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputSecurityRolesExplorerCommandId) { }

        public static OutputSecurityRolesExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputSecurityRolesExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenSecurityRolesExplorer(connectionData);
        }
    }
}
