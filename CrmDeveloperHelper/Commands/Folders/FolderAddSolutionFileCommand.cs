using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Folders
{
    internal sealed class FolderAddSolutionFileCommand : AbstractCommand
    {
        private FolderAddSolutionFileCommand(OleMenuCommandService commandService)
              : base(commandService, PackageIds.guidCommandSet.FolderAddSolutionFileCommandId) { }

        public static FolderAddSolutionFileCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderAddSolutionFileCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleSolutionAddFileToFolder();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerFolderSingle(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FolderAddSolutionFileCommand);
        }
    }
}