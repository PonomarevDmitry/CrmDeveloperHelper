using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeReportLinkCreateCommand : AbstractCommand
    {
        private CodeReportLinkCreateCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeReportLinkCreateCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusActiveDocumentReport) { }

        public static CodeReportLinkCreateCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeReportLinkCreateCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsReportType);

            helper.HandleCreateLaskLinkReportCommand(selectedFiles);
        }
    }
}
