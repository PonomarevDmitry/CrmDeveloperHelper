using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputSelectFileCrmConnectionCommand : AbstractOutputWindowCommand
    {
        private OutputSelectFileCrmConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidCommandSet.OutputSelectFileCrmConnectionCommandId
            )
        {

        }

        public static OutputSelectFileCrmConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputSelectFileCrmConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.SelectFileInFolder(connectionData, connectionData.Path);
        }
    }
}
