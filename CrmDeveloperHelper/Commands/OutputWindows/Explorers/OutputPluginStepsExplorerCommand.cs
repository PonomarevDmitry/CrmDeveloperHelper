using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputPluginStepsExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputPluginStepsExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputPluginStepsExplorerCommandId)
        {
        }

        public static OutputPluginStepsExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputPluginStepsExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            string selection = helper.GetSelectedText();

            helper.HandleOpenPluginStepsExplorer(selection, string.Empty, string.Empty);
        }
    }
}