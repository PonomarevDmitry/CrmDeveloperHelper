using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.PluginConfigurations
{
    internal sealed class CommonPluginConfigurationComparerPluginAssemblyCommand : AbstractCommand
    {
        private CommonPluginConfigurationComparerPluginAssemblyCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonPluginConfigurationComparerPluginAssemblyCommandId) { }

        public static CommonPluginConfigurationComparerPluginAssemblyCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonPluginConfigurationComparerPluginAssemblyCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandlePluginConfigurationComparerPluginAssembly();
        }
    }
}
