using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileWebResourceCheckEncodingOpenFilesCommand : AbstractCommand
    {
        private FileWebResourceCheckEncodingOpenFilesCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileWebResourceCheckEncodingOpenFilesCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextAny) { }

        public static FileWebResourceCheckEncodingOpenFilesCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileWebResourceCheckEncodingOpenFilesCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, false);

            helper.HandleCheckOpenFilesWithoutUTF8EncodingCommand(selectedFiles);
        }
    }
}
