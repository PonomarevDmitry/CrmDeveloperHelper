using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.PluginConfigurations
{
    internal sealed class CommonPluginConfigurationPluginTypeCommand : AbstractSingleCommand
    {
        private CommonPluginConfigurationPluginTypeCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonPluginConfigurationPluginTypeCommandId) { }

        public static CommonPluginConfigurationPluginTypeCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonPluginConfigurationPluginTypeCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandlePluginConfigurationPluginTypeDescription();
        }
    }
}
