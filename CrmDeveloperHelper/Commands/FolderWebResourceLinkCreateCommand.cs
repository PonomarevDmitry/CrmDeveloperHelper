using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FolderWebResourceLinkCreateCommand : AbstractCommand
    {
        private FolderWebResourceLinkCreateCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FolderWebResourceLinkCreateCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceRecursive) { }

        public static FolderWebResourceLinkCreateCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FolderWebResourceLinkCreateCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceType, true);

            helper.HandleCreateLaskLinkWebResourcesMultipleCommand(selectedFiles);
        }
    }
}
