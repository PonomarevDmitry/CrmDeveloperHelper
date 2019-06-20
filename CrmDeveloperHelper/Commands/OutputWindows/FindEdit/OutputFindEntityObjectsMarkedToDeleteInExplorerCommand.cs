using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.FindEdit
{
    internal sealed class OutputFindEntityObjectsMarkedToDeleteInExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputFindEntityObjectsMarkedToDeleteInExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputFindEntityObjectsMarkedToDeleteInExplorerCommandId) { }

        public static OutputFindEntityObjectsMarkedToDeleteInExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputFindEntityObjectsMarkedToDeleteInExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleFindMarkedToDeleteInExplorer(connectionData);
        }
    }
}
