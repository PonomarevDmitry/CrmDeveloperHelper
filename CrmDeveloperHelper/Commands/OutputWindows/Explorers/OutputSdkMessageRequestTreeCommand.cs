using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputSdkMessageRequestTreeCommand : AbstractOutputWindowCommand
    {
        private OutputSdkMessageRequestTreeCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputSdkMessageRequestTreeCommandId) { }

        public static OutputSdkMessageRequestTreeCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputSdkMessageRequestTreeCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleSdkMessageRequestTree(connectionData);
        }
    }
}
