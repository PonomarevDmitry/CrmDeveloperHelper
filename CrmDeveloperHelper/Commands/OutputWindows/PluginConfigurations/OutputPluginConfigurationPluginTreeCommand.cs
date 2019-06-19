using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.PluginConfigurations
{
    internal sealed class OutputPluginConfigurationPluginTreeCommand : AbstractOutputWindowCommand
    {
        private OutputPluginConfigurationPluginTreeCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputPluginConfigurationPluginTreeCommandId) { }

        public static OutputPluginConfigurationPluginTreeCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputPluginConfigurationPluginTreeCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandlePluginConfigurationTree(connectionData);
        }
    }
}
