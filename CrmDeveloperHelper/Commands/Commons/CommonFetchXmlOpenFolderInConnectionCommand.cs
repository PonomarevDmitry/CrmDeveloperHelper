using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons
{
    internal sealed class CommonFetchXmlOpenFolderInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private CommonFetchXmlOpenFolderInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CommonFetchXmlOpenFolderInConnectionCommandId)
        {
        }

        public static CommonFetchXmlOpenFolderInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonFetchXmlOpenFolderInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleConnectionOpenFetchXmlFolder(connectionData);
        }
    }
}
