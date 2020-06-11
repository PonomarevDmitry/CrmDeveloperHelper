using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputFetchXmlOpenFolderCommand : AbstractOutputWindowCommand
    {
        private OutputFetchXmlOpenFolderCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.OutputFetchXmlOpenFolderCommandId)
        {
        }

        public static OutputFetchXmlOpenFolderCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputFetchXmlOpenFolderCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleConnectionOpenFetchXmlFolder(connectionData);
        }
    }
}