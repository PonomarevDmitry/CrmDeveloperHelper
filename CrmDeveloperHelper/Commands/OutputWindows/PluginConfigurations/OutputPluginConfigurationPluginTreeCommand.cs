using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.PluginConfigurations
{
    internal sealed class OutputPluginConfigurationPluginTreeCommand : AbstractCommand
    {
        private OutputPluginConfigurationPluginTreeCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputPluginConfigurationPluginTreeCommandId) { }

        public static OutputPluginConfigurationPluginTreeCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputPluginConfigurationPluginTreeCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var connectionData = helper.GetOutputWindowConnection();

            if (connectionData == null)
            {
                return;
            }

            helper.HandlePluginConfigurationTree(connectionData);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusIsConnectionOutput(applicationObject, menuCommand);
        }
    }
}
