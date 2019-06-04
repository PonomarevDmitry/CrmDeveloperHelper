using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FolderWebResourceCheckEncodingCompareFilesCommand : AbstractCommand
    {
        private FolderWebResourceCheckEncodingCompareFilesCommand(Package package)
              : base(package, PackageGuids.guidCommandSet, PackageIds.FolderWebResourceCheckEncodingCompareFilesCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursive) { }

        public static FolderWebResourceCheckEncodingCompareFilesCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FolderWebResourceCheckEncodingCompareFilesCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, true).ToList();

            helper.HandleCompareFilesWithoutUTF8EncodingCommand(selectedFiles, false);
        }
    }
}
