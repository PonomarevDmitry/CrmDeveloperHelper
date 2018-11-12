using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class DocumentsReportAddIntoSolutionCommand : AbstractCommand
    {
        private DocumentsReportAddIntoSolutionCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.DocumentsReportAddIntoSolutionCommandId, ActionExecute, ActionBeforeQueryStatus)
        {
        }

        public static DocumentsReportAddIntoSolutionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new DocumentsReportAddIntoSolutionCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsReportType);

            helper.HandleAddingReportsIntoSolutionCommand(selectedFiles, true, null);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsReport(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.DocumentsReportAddIntoSolutionCommand);
        }
    }
}