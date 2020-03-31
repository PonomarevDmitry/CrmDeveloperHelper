using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputApplicationRibbonExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputApplicationRibbonExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputApplicationRibbonExplorerCommandId) { }

        public static OutputApplicationRibbonExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputApplicationRibbonExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenApplicationRibbonExplorer(connectionData);
        }
    }
}
