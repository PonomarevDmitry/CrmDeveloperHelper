using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.PluginConfigurations
{
    internal sealed class CommonPluginConfigurationPluginTreeCommand : AbstractCommand
    {
        private CommonPluginConfigurationPluginTreeCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonPluginConfigurationPluginTreeCommandId) { }

        public static CommonPluginConfigurationPluginTreeCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonPluginConfigurationPluginTreeCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandlePluginConfigurationTree();
        }
    }
}
