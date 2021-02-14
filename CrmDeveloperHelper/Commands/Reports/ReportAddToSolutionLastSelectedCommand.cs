using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class ReportAddToSolutionLastSelectedCommand : AbstractDynamicCommandOnSolutionLastSelected
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private ReportAddToSolutionLastSelectedCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static ReportAddToSolutionLastSelectedCommand InstanceCode { get; private set; }

        public static ReportAddToSolutionLastSelectedCommand InstanceDocuments { get; private set; }

        public static ReportAddToSolutionLastSelectedCommand InstanceFile { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            InstanceCode = new ReportAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.CodeReportAddToSolutionLastSelectedCommandId, sourceCode);

            InstanceDocuments = new ReportAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.DocumentsReportAddToSolutionLastSelectedCommandId, sourceDocuments);

            InstanceFile = new ReportAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.FileReportAddToSolutionLastSelectedCommandId, sourceFile);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.Report).ToList();

            helper.HandleReportAddingToSolutionCommand(null, solutionUniqueName, false, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, string element, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.Report);
        }
    }
}