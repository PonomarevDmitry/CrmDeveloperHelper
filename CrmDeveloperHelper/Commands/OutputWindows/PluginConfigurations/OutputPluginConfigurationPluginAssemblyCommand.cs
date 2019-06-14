using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.PluginConfigurations
{
    internal sealed class OutputPluginConfigurationPluginAssemblyCommand : AbstractCommand
    {
        private OutputPluginConfigurationPluginAssemblyCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputPluginConfigurationPluginAssemblyCommandId) { }

        public static OutputPluginConfigurationPluginAssemblyCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputPluginConfigurationPluginAssemblyCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandlePluginConfigurationPluginAssemblyDescription();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusIsConnectionOutput(applicationObject, menuCommand);
        }
    }
}
