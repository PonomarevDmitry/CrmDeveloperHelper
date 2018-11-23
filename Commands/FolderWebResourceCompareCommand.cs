using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FolderWebResourceCompareCommand : AbstractCommand
    {
        private FolderWebResourceCompareCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FolderWebResourceCompareCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FolderWebResourceCompareCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FolderWebResourceCompareCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, true);

            helper.HandleFileCompareCommand(null, selectedFiles, false);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceRecursive(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FolderWebResourceCompareCommand);
        }
    }
}