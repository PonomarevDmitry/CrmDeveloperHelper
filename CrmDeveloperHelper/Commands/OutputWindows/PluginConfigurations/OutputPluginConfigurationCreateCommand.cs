using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.PluginConfigurations
{
    internal sealed class OutputPluginConfigurationCreateCommand : AbstractOutputWindowCommand
    {
        private OutputPluginConfigurationCreateCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputPluginConfigurationCreateCommandId) { }

        public static OutputPluginConfigurationCreateCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputPluginConfigurationCreateCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandlePluginConfigurationCreate(connectionData);
        }
    }
}
