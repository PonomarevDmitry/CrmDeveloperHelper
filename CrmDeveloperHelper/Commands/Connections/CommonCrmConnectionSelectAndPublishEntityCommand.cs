using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Connections
{
    internal sealed class CommonCrmConnectionSelectAndPublishEntityCommand : AbstractDynamicCommandByConnectionAll
    {
        private CommonCrmConnectionSelectAndPublishEntityCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CommonCrmConnectionSelectAndPublishEntityCommandId)
        {
        }

        public static CommonCrmConnectionSelectAndPublishEntityCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCrmConnectionSelectAndPublishEntityCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleSelectAndPublishEntityCommand(connectionData);
        }
    }
}