using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputSdkMessageFilterTreeCommand : AbstractOutputWindowCommand
    {
        private OutputSdkMessageFilterTreeCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputSdkMessageFilterTreeCommandId)
        {
        }

        public static OutputSdkMessageFilterTreeCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputSdkMessageFilterTreeCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleSdkMessageFilterTree(connectionData);
        }
    }
}