using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonSdkMessageRequestTreeCommand : AbstractCommand
    {
        private CommonSdkMessageRequestTreeCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonSdkMessageRequestTreeCommandId) { }

        public static CommonSdkMessageRequestTreeCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonSdkMessageRequestTreeCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleSdkMessageRequestTree();
        }
    }
}
