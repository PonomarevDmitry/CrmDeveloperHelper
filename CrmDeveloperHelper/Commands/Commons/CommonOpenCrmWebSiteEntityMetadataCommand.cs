using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons
{
    internal sealed class CommonOpenCrmWebSiteEntityMetadataCommand : AbstractSingleCommand
    {
        private CommonOpenCrmWebSiteEntityMetadataCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CommonOpenCrmWebSiteEntityMetadataCommandId)
        {
        }

        public static CommonOpenCrmWebSiteEntityMetadataCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonOpenCrmWebSiteEntityMetadataCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleConnectionOpenEntityMetadataInWeb();
        }
    }
}
