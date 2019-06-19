using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.PluginConfigurations
{
    internal sealed class OutputPluginConfigurationComparerPluginAssemblyCommand : AbstractOutputWindowCommand
    {
        private OutputPluginConfigurationComparerPluginAssemblyCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputPluginConfigurationComparerPluginAssemblyCommandId) { }

        public static OutputPluginConfigurationComparerPluginAssemblyCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputPluginConfigurationComparerPluginAssemblyCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandlePluginConfigurationComparerPluginAssembly();
        }
    }
}
