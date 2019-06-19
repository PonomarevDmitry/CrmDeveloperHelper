using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputSolutionExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputSolutionExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputSolutionExplorerCommandId) { }

        public static OutputSolutionExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputSolutionExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenSolutionExplorerWindow(connectionData);
        }
    }
}