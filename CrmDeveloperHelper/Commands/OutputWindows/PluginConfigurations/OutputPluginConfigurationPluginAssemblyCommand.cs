using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.PluginConfigurations
{
    internal sealed class OutputPluginConfigurationPluginAssemblyCommand : AbstractOutputWindowCommand
    {
        private OutputPluginConfigurationPluginAssemblyCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputPluginConfigurationPluginAssemblyCommandId) { }

        public static OutputPluginConfigurationPluginAssemblyCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputPluginConfigurationPluginAssemblyCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandlePluginConfigurationPluginAssemblyDescription();
        }
    }
}
