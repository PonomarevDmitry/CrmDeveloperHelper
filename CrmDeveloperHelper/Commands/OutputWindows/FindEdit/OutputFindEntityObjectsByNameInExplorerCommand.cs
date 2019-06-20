using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.FindEdit
{
    internal sealed class OutputFindEntityObjectsByNameInExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputFindEntityObjectsByNameInExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputFindEntityObjectsByNameInExplorerCommandId) { }

        public static OutputFindEntityObjectsByNameInExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputFindEntityObjectsByNameInExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleFindEntityObjectsByNameInExplorer(connectionData);
        }
    }
}
