using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.PluginConfigurations
{
    internal sealed class CommonPluginConfigurationPluginAssemblyCommand : AbstractSingleCommand
    {
        private CommonPluginConfigurationPluginAssemblyCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonPluginConfigurationPluginAssemblyCommandId) { }

        public static CommonPluginConfigurationPluginAssemblyCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonPluginConfigurationPluginAssemblyCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandlePluginConfigurationPluginAssemblyDescription();
        }
    }
}
