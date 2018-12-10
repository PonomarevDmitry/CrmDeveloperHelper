using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeReportLinkClearCommand : AbstractCommand
    {
        private CodeReportLinkClearCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeReportLinkClearCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusActiveDocumentReport) { }

        public static CodeReportLinkClearCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeReportLinkClearCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsReportType);

            helper.HandleFileClearLink(selectedFiles);
        }
    }
}
