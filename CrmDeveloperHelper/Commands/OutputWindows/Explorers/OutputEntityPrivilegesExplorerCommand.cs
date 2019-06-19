using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputEntityPrivilegesExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputEntityPrivilegesExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputEntityPrivilegesExplorerCommandId) { }

        public static OutputEntityPrivilegesExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputEntityPrivilegesExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenEntityPrivilegesExplorer(connectionData);
        }
    }
}
