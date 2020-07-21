using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputCrmConnectionTestCommand : AbstractOutputWindowCommand
    {
        private OutputCrmConnectionTestCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.OutputCrmConnectionTestCommandId)
        {
        }

        public static OutputCrmConnectionTestCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCrmConnectionTestCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleConnectionTest(connectionData);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(menuCommand, Properties.CommandNames.OutputTestCrmConnectionCommand, connectionData);
        }
    }
}