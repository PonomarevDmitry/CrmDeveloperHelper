using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileReportLinkClearCommand : AbstractCommand
    {
        private FileReportLinkClearCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileReportLinkClearCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerReportAny) { }

        public static FileReportLinkClearCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileReportLinkClearCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsReportType, false);

            helper.HandleFileClearLink(selectedFiles);
        }
    }
}
