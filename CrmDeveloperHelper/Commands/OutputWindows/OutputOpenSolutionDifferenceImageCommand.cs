using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputOpenSolutionDifferenceImageCommand : AbstractOutputWindowCommand
    {
        private OutputOpenSolutionDifferenceImageCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputOpenSolutionDifferenceImageCommandId) { }

        public static OutputOpenSolutionDifferenceImageCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputOpenSolutionDifferenceImageCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenSolutionDifferenceImageWindow(connectionData);
        }
    }
}