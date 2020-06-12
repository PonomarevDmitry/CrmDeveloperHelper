using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonSdkMessageFilterExplorerCommand : AbstractSingleCommand
    {
        private CommonSdkMessageFilterExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonSdkMessageFilterExplorerCommandId)
        {
        }

        public static CommonSdkMessageFilterExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonSdkMessageFilterExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleSdkMessageFilterExplorer();
        }
    }
}
