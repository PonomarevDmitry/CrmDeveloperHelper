using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputSystemFormExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputSystemFormExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputSystemFormExplorerCommandId) { }

        public static OutputSystemFormExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputSystemFormExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleExplorerSystemForm(connectionData);
        }
    }
}
