using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputCrmConnectionOpenFetchXmlFolderCommand : AbstractOutputWindowCommand
    {
        private OutputCrmConnectionOpenFetchXmlFolderCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.OutputCrmConnectionOpenFetchXmlFolderCommandId)
        {
        }

        public static OutputCrmConnectionOpenFetchXmlFolderCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCrmConnectionOpenFetchXmlFolderCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleConnectionOpenFetchXmlFolder(connectionData);
        }
    }
}