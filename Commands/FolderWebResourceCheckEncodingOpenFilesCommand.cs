using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FolderWebResourceCheckEncodingOpenFilesCommand : AbstractCommand
    {
        private FolderWebResourceCheckEncodingOpenFilesCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FolderWebResourceCheckEncodingOpenFilesCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursive) { }

        public static FolderWebResourceCheckEncodingOpenFilesCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FolderWebResourceCheckEncodingOpenFilesCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, true);

            helper.HandleCheckOpenFilesWithoutUTF8EncodingCommand(selectedFiles);
        }
    }
}
