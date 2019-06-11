using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonOpenSolutionDifferenceImageCommand : AbstractCommand
    {
        private CommonOpenSolutionDifferenceImageCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.CommonOpenSolutionDifferenceImageCommandId) { }

        public static CommonOpenSolutionDifferenceImageCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonOpenSolutionDifferenceImageCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleOpenSolutionDifferenceImageWindow();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CommonOpenSolutionDifferenceImageCommand);
        }
    }
}
