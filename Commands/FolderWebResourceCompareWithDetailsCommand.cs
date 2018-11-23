using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FolderWebResourceCompareWithDetailsCommand : AbstractCommand
    {
        private FolderWebResourceCompareWithDetailsCommand(Package package)
              : base(package, PackageGuids.guidCommandSet, PackageIds.FolderWebResourceCompareWithDetailsCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FolderWebResourceCompareWithDetailsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FolderWebResourceCompareWithDetailsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceType, true);

            helper.HandleFileCompareCommand(null, selectedFiles, true);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceRecursive(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FolderWebResourceCompareWithDetailsCommand);
        }
    }
}