using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Connections
{
    internal sealed class CommonSelectCrmConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private CommonSelectCrmConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CommonSelectCrmConnectionCommandId
            )
        {

        }

        public static CommonSelectCrmConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonSelectCrmConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            connectionData.ConnectionConfiguration.SetCurrentConnection(connectionData.ConnectionId);

            connectionData.ConnectionConfiguration.Save();

            helper.WriteToOutput(null, Properties.OutputStrings.CurrentConnectionFormat1, connectionData.Name);
            helper.ActivateOutputWindow(null);
        }
    }
}