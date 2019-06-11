using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceShowDifferenceCustomCommand : AbstractCommand
    {
        private FileWebResourceShowDifferenceCustomCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.FileWebResourceShowDifferenceCustomCommandId) { }

        public static FileWebResourceShowDifferenceCustomCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileWebResourceShowDifferenceCustomCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleWebResourceDifferenceCommand(null, true);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextSingle(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FileWebResourceShowDifferenceCustromCommand);
        }
    }
}