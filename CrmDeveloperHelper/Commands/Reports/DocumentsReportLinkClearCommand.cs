using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class DocumentsReportLinkClearCommand : AbstractSingleCommand
    {
        private DocumentsReportLinkClearCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.DocumentsReportLinkClearCommandId) { }

        public static DocumentsReportLinkClearCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsReportLinkClearCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsReportType).ToList();

            helper.HandleFileClearLink(selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsReport(applicationObject, menuCommand);
        }
    }
}
