using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class ReportAddToSolutionLastCommand : AbstractDynamicCommandOnSolutionLast
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private ReportAddToSolutionLastCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static ReportAddToSolutionLastCommand InstanceCode { get; private set; }

        public static ReportAddToSolutionLastCommand InstanceDocuments { get; private set; }

        public static ReportAddToSolutionLastCommand InstanceFile { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            InstanceCode = new ReportAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.CodeReportAddToSolutionLastCommandId, sourceCode);

            InstanceDocuments = new ReportAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsReportAddToSolutionLastCommandId, sourceDocuments);

            InstanceFile = new ReportAddToSolutionLastCommand(commandService, PackageIds.guidDynamicCommandSet.FileReportAddToSolutionLastCommandId, sourceFile);
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