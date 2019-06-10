using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class CodeReportAddToSolutionLastCommand : AbstractAddObjectToSolutionLastCommand
    {
        private CodeReportAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CodeReportAddToSolutionLastCommandId
                , ActionExecute
                , CommonHandlers.ActionBeforeQueryStatusActiveDocumentReport
            )
        {

        }

        public static CodeReportAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeReportAddToSolutionLastCommand(commandService);
        }

        private static void ActionExecute(DTEHelper helper, ConnectionData connectionData, string solutionUniqueName)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsReportType).ToList();

            helper.HandleAddingReportsToSolutionCommand(null, solutionUniqueName, false, selectedFiles);
        }
    }
}