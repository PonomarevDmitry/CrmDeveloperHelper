using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Connections
{
    internal sealed class CommonCrmConnectionTestCommand : AbstractDynamicCommandByConnectionAll
    {
        private CommonCrmConnectionTestCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CommonCrmConnectionTestCommandId
            )
        {

        }

        public static CommonCrmConnectionTestCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCrmConnectionTestCommand(commandService);
        }

        private static void ActionExecute(DTEHelper helper, ConnectionData connectionData)
        {
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.TestConnection(connectionData);
        }
    }
}