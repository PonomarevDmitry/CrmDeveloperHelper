using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputCrmConnectionSelectAndPublishEntityCommand : AbstractOutputWindowCommand
    {
        private OutputCrmConnectionSelectAndPublishEntityCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.OutputCrmConnectionSelectAndPublishEntityCommandId)
        {
        }

        public static OutputCrmConnectionSelectAndPublishEntityCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCrmConnectionSelectAndPublishEntityCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleSelectAndPublishEntityCommand(connectionData);
        }
    }
}