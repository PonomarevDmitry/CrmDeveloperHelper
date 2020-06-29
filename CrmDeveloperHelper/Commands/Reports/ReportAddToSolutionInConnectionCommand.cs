using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class ReportAddToSolutionInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private ReportAddToSolutionInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static ReportAddToSolutionInConnectionCommand InstanceCode { get; private set; }

        public static ReportAddToSolutionInConnectionCommand InstanceDocuments { get; private set; }

        public static ReportAddToSolutionInConnectionCommand InstanceFile { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            InstanceCode = new ReportAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.CodeReportAddToSolutionInConnectionCommandId, sourceCode);

            InstanceDocuments = new ReportAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsReportAddToSolutionInConnectionCommandId, sourceDocuments);

            InstanceFile = new ReportAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileReportAddToSolutionInConnectionCommandId, sourceFile);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.Report).ToList();

            helper.HandleReportAddingToSolutionCommand(connectionData, null, true, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.Report);
        }
    }
}