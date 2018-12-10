using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class DocumentsReportLinkClearCommand : AbstractCommand
    {
        private DocumentsReportLinkClearCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.DocumentsReportLinkClearCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsReport) { }

        public static DocumentsReportLinkClearCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new DocumentsReportLinkClearCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsReportType);

            helper.HandleFileClearLink(selectedFiles);
        }
    }
}
