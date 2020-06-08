using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons
{
    internal sealed class CommonOpenCrmWebSiteEntityListCommand : AbstractDynamicCommandByConnectionAll
    {
        private CommonOpenCrmWebSiteEntityListCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CommonOpenCrmWebSiteEntityListCommandId)
        {
        }

        public static CommonOpenCrmWebSiteEntityListCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonOpenCrmWebSiteEntityListCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleConnectionOpenEntityListInWeb(connectionData);
        }
    }
}