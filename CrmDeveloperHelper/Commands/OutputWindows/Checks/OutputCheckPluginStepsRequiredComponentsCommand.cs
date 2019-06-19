using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks
{
    internal sealed class OutputCheckPluginStepsRequiredComponentsCommand : AbstractOutputWindowCommand
    {
        private OutputCheckPluginStepsRequiredComponentsCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.OutputCheckPluginStepsRequiredComponentsCommandId
            )
        {

        }

        public static OutputCheckPluginStepsRequiredComponentsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCheckPluginStepsRequiredComponentsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckPluginStepsRequiredComponents(connectionData);
        }
    }
}
