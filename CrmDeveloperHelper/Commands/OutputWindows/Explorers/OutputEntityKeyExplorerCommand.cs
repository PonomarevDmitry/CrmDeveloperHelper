using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputEntityKeyExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputEntityKeyExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputEntityKeyExplorerCommandId) { }

        public static OutputEntityKeyExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputEntityKeyExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenEntityKeyExplorer(connectionData);
        }
    }
}