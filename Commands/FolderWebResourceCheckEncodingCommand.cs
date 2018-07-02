using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FolderWebResourceCheckEncodingCommand : AbstractCommand
    {
        private FolderWebResourceCheckEncodingCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FolderWebResourceCheckEncodingCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursive) { }

        public static FolderWebResourceCheckEncodingCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FolderWebResourceCheckEncodingCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, true);

            helper.HandleCheckFileEncodingCommand(selectedFiles);
        }
    }
}
