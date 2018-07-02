using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileWebResourceCheckEncodingCommand : AbstractCommand
    {
        private FileWebResourceCheckEncodingCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileWebResourceCheckEncodingCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextAny) { }

        public static FileWebResourceCheckEncodingCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileWebResourceCheckEncodingCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, false);

            helper.HandleCheckFileEncodingCommand(selectedFiles);
        }
    }
}
