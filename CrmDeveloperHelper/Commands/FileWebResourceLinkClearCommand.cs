using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileWebResourceLinkClearCommand : AbstractCommand
    {
        private FileWebResourceLinkClearCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileWebResourceLinkClearCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceAny) { }

        public static FileWebResourceLinkClearCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileWebResourceLinkClearCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceType, false);

            helper.HandleFileClearLink(selectedFiles);
        }
    }
}
