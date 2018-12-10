using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FolderWebResourceCheckEncodingCompareWithDetailsFilesCommand : AbstractCommand
    {
        private FolderWebResourceCheckEncodingCompareWithDetailsFilesCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FolderWebResourceCheckEncodingCompareWithDetailsFilesCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceRecursive) { }

        public static FolderWebResourceCheckEncodingCompareWithDetailsFilesCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FolderWebResourceCheckEncodingCompareWithDetailsFilesCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceType, true);

            helper.HandleCompareFilesWithoutUTF8EncodingCommand(selectedFiles, true);
        }
    }
}
