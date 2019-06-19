using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputTestCrmConnectionCommand : AbstractOutputWindowCommand
    {
        private OutputTestCrmConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.OutputTestCrmConnectionCommandId
            )
        {

        }

        public static OutputTestCrmConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputTestCrmConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.TestConnection(connectionData);
        }
    }
}