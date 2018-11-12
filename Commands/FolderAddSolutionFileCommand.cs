using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FolderAddSolutionFileCommand : AbstractCommand
    {
        private FolderAddSolutionFileCommand(Package package)
              : base(package, PackageGuids.guidCommandSet, PackageIds.FolderAddSolutionFileCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FolderAddSolutionFileCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FolderAddSolutionFileCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportSolution();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerFolderSingle(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FolderAddSolutionFileCommand);
        }
    }
}