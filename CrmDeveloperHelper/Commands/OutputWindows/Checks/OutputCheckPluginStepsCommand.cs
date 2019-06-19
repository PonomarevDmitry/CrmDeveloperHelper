using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks
{
    internal sealed class OutputCheckPluginStepsCommand : AbstractOutputWindowCommand
    {
        private OutputCheckPluginStepsCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.OutputCheckPluginStepsCommandId
            )
        {

        }

        public static OutputCheckPluginStepsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCheckPluginStepsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckPluginSteps(connectionData);
        }
    }
}
