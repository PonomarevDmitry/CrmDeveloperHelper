using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputPublishAllInCrmConnectionCommand : AbstractOutputWindowCommand
    {
        private OutputPublishAllInCrmConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidCommandSet.OutputPublishAllInCrmConnectionCommandId
            )
        {

        }

        public static OutputPublishAllInCrmConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputPublishAllInCrmConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleConnectionPublishAll(connectionData);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(menuCommand, Properties.CommandNames.OutputPublishAllInCrmConnectionCommand, connectionData);
        }
    }
}
