using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.PluginConfigurations
{
    internal sealed class OutputPluginConfigurationComparerPluginAssemblyCommand : AbstractCommand
    {
        private OutputPluginConfigurationComparerPluginAssemblyCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputPluginConfigurationComparerPluginAssemblyCommandId) { }

        public static OutputPluginConfigurationComparerPluginAssemblyCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputPluginConfigurationComparerPluginAssemblyCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var connectionData = helper.GetOutputWindowConnection();

            if (connectionData == null)
            {
                return;
            }

            helper.HandlePluginConfigurationComparerPluginAssembly();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusIsConnectionOutput(applicationObject, menuCommand);
        }
    }
}
