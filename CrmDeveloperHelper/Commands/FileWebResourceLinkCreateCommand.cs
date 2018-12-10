using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileWebResourceLinkCreateCommand : AbstractCommand
    {
        private FileWebResourceLinkCreateCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileWebResourceLinkCreateCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceAny) { }

        public static FileWebResourceLinkCreateCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileWebResourceLinkCreateCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceType, false);

            helper.HandleCreateLaskLinkWebResourcesMultipleCommand(selectedFiles);
        }
    }
}
