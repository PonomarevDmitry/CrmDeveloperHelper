using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileReportLinkCreateCommand : AbstractCommand
    {
        private FileReportLinkCreateCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileReportLinkCreateCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerReportSingle) { }

        public static FileReportLinkCreateCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileReportLinkCreateCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsReportType, false);

            helper.HandleCreateLaskLinkReportCommand(selectedFiles);
        }
    }
}
