using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons
{
    internal sealed class CommonOpenCrmWebSiteEntityMetadataCommand : AbstractDynamicCommandByConnectionAll
    {
        private CommonOpenCrmWebSiteEntityMetadataCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CommonOpenCrmWebSiteEntityMetadataCommandId)
        {
        }

        public static CommonOpenCrmWebSiteEntityMetadataCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonOpenCrmWebSiteEntityMetadataCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleConnectionOpenEntityMetadataInWeb(connectionData);
        }
    }
}