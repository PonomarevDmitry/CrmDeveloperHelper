using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.PluginConfigurations
{
    internal sealed class CommonPluginConfigurationCreateCommand : AbstractCommand
    {
        private CommonPluginConfigurationCreateCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.CommonPluginConfigurationCreateCommandId) { }

        public static CommonPluginConfigurationCreateCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonPluginConfigurationCreateCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandlePluginConfigurationCreate();
        }
    }
}
