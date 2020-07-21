using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputCrmConnectionSelectFileCommand : AbstractOutputWindowCommand
    {
        private OutputCrmConnectionSelectFileCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.OutputCrmConnectionSelectFileCommandId)
        {
        }

        public static OutputCrmConnectionSelectFileCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCrmConnectionSelectFileCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.SelectFileInFolder(connectionData, connectionData.Path);
        }
    }
}