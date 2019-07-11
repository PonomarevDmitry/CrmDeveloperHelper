using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Connections
{
    internal sealed class CommonCrmConnectionSelectFileCommand : AbstractDynamicCommandByConnectionAll
    {
        private CommonCrmConnectionSelectFileCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.CommonCrmConnectionSelectFileCommandId)
        {

        }

        public static CommonCrmConnectionSelectFileCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCrmConnectionSelectFileCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.SelectFileInFolder(connectionData, connectionData.Path);
        }
    }
}
