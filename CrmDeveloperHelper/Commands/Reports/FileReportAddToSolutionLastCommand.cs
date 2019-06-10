using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class FileReportAddToSolutionLastCommand : AbstractAddObjectToSolutionLastCommand
    {
        private FileReportAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.FileReportAddToSolutionLastCommandId
                , ActionExecute
                , CommonHandlers.ActionBeforeQueryStatusSolutionExplorerReportAny
            )
        {

        }

        public static FileReportAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileReportAddToSolutionLastCommand(commandService);
        }

        private static void ActionExecute(DTEHelper helper, ConnectionData connectionData, string solutionUniqueName)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsReportType, false).ToList();

            helper.HandleAddingReportsToSolutionCommand(null, solutionUniqueName, false, selectedFiles);
        }
    }
}