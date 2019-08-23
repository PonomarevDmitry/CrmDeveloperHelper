using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputSdkMessageTreeCommand : AbstractOutputWindowCommand
    {
        private OutputSdkMessageTreeCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputSdkMessageTreeCommandId) { }

        public static OutputSdkMessageTreeCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputSdkMessageTreeCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleSdkMessageTree(connectionData);
        }
    }
}
