using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceShowDependentComponentsCommand : AbstractCommand
    {
        private FileWebResourceShowDependentComponentsCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileWebResourceShowDependentComponentsCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceAny) { }

        public static FileWebResourceShowDependentComponentsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileWebResourceShowDependentComponentsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceType, false).ToList();

            helper.HandleShowingWebResourcesDependentComponents(selectedFiles);
        }
    }
}
