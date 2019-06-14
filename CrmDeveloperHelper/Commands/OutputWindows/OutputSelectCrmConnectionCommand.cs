using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputSelectCrmConnectionCommand : AbstractCommand
    {
        private OutputSelectCrmConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.OutputSelectCrmConnectionCommandId
            )
        {

        }

        public static OutputSelectCrmConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputSelectCrmConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var connectionData = helper.GetOutputWindowConnection();

            if (connectionData == null)
            {
                return;
            }

            connectionData.ConnectionConfiguration.SetCurrentConnection(connectionData.ConnectionId);

            connectionData.ConnectionConfiguration.Save();

            helper.WriteToOutput(null, Properties.OutputStrings.CurrentConnectionFormat1, connectionData.Name);
            helper.ActivateOutputWindow(null);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusIsConnectionOutput(applicationObject, menuCommand);
        }
    }
}