using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.PluginConfigurations
{
    internal sealed class OutputPluginConfigurationPluginTypeCommand : AbstractCommand
    {
        private OutputPluginConfigurationPluginTypeCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputPluginConfigurationPluginTypeCommandId) { }

        public static OutputPluginConfigurationPluginTypeCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputPluginConfigurationPluginTypeCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandlePluginConfigurationPluginTypeDescription();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusIsConnectionOutput(applicationObject, menuCommand);
        }
    }
}
