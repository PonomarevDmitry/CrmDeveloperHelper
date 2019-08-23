using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputPluginTreeCommand : AbstractOutputWindowCommand
    {
        private OutputPluginTreeCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputPluginTreeCommandId) { }

        public static OutputPluginTreeCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputPluginTreeCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            string selection = helper.GetSelectedText();

            helper.HandleOpenPluginTree(connectionData, selection, string.Empty, string.Empty);
        }
    }
}
