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
                , PackageIds.OutputPublishAllInCrmConnectionCommandId
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
            helper.HandlePublishAll(connectionData);
        }
    }
}
