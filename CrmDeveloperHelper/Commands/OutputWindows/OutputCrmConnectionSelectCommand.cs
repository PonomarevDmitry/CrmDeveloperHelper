using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputCrmConnectionSelectCommand : AbstractOutputWindowCommand
    {
        private OutputCrmConnectionSelectCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.OutputCrmConnectionSelectCommandId)
        {
        }

        public static OutputCrmConnectionSelectCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCrmConnectionSelectCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            connectionData.ConnectionConfiguration.SetCurrentConnection(connectionData.ConnectionId);

            connectionData.ConnectionConfiguration.Save();

            helper.WriteToOutput(null, Properties.OutputStrings.CurrentConnectionFormat1, connectionData.Name);
            helper.ActivateOutputWindow(null);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(menuCommand, Properties.CommandNames.OutputSelectCrmConnectionCommand, connectionData);
        }
    }
}