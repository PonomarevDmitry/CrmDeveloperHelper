using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckPluginStepsCommand : AbstractDynamicCommandByConnectionAll
    {
        private CommonCheckPluginStepsCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CommonCheckPluginStepsCommandId
            )
        {

        }

        public static CommonCheckPluginStepsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckPluginStepsCommand(commandService);
        }

        private static void ActionExecute(DTEHelper helper, ConnectionData connectionData)
        {
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckPluginSteps(connectionData);
        }
    }
}
