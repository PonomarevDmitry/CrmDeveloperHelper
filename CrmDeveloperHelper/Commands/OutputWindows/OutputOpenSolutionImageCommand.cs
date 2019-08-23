using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputOpenSolutionImageCommand : AbstractOutputWindowCommand
    {
        private OutputOpenSolutionImageCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputOpenSolutionImageCommandId) { }

        public static OutputOpenSolutionImageCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputOpenSolutionImageCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenSolutionImageWindow(connectionData);
        }
    }
}