using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons
{
    internal sealed class CommonOpenCrmWebSiteEntityInstanceByIdCommand : AbstractSingleCommand
    {
        private CommonOpenCrmWebSiteEntityInstanceByIdCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CommonOpenCrmWebSiteEntityInstanceByIdCommandId)
        {
        }

        public static CommonOpenCrmWebSiteEntityInstanceByIdCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonOpenCrmWebSiteEntityInstanceByIdCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleConnectionOpenEntityInstanceByIdInWeb();
        }
    }
}
