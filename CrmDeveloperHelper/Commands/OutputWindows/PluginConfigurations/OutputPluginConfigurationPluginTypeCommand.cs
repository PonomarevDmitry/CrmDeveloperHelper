using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.PluginConfigurations
{
    internal sealed class OutputPluginConfigurationPluginTypeCommand : AbstractOutputWindowCommand
    {
        private OutputPluginConfigurationPluginTypeCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputPluginConfigurationPluginTypeCommandId) { }

        public static OutputPluginConfigurationPluginTypeCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputPluginConfigurationPluginTypeCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandlePluginConfigurationPluginTypeDescription();
        }
    }
}
