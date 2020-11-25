using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons
{
    internal sealed class CommonOpenCrmWebSiteEntityListCommand : AbstractSingleCommand
    {
        private CommonOpenCrmWebSiteEntityListCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CommonOpenCrmWebSiteEntityListCommandId)
        {
        }

        public static CommonOpenCrmWebSiteEntityListCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonOpenCrmWebSiteEntityListCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleConnectionOpenEntityListInWeb();
        }
    }
}
