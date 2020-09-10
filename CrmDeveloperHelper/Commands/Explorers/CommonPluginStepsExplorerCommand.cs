using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonPluginStepsExplorerCommand : AbstractSingleCommand
    {
        private CommonPluginStepsExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonPluginStepsExplorerCommandId)
        {
        }

        public static CommonPluginStepsExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonPluginStepsExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            string selection = helper.GetSelectedText();

            helper.HandleOpenPluginStepsExplorer(selection, string.Empty, string.Empty);
        }
    }
}