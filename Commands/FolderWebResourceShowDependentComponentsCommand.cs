using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FolderWebResourceShowDependentComponentsCommand : AbstractCommand
    {
        private FolderWebResourceShowDependentComponentsCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FolderWebResourceShowDependentComponentsCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceRecursive) { }

        public static FolderWebResourceShowDependentComponentsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FolderWebResourceShowDependentComponentsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceType, true);

            helper.HandleShowingWebResourcesDependentComponents(selectedFiles);
        }
    }
}
