using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Connections
{
    internal sealed class CommonCrmConnectionOpenFetchXmlFolderCommand : AbstractDynamicCommandByConnectionAll
    {
        private CommonCrmConnectionOpenFetchXmlFolderCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CommonCrmConnectionOpenFetchXmlFolderCommandId)
        {
        }

        public static CommonCrmConnectionOpenFetchXmlFolderCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCrmConnectionOpenFetchXmlFolderCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleConnectionOpenFetchXmlFolder(connectionData);
        }
    }
}