using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class ReportExplorerCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly string _commandNameForCorrection;

        private ReportExplorerCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles, string commandNameForCorrection)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._commandNameForCorrection = commandNameForCorrection;
        }

        public static ReportExplorerCommand InstanceCode { get; private set; }

        public static ReportExplorerCommand InstanceFile { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFileSingle.CreateSource();

            InstanceCode = new ReportExplorerCommand(
                commandService
                , PackageIds.guidCommandSet.CodeReportExplorerCommandId
                , sourceCode
                , Properties.CommandNames.CodeReportExplorerCommand
            );

            InstanceFile = new ReportExplorerCommand(
                commandService
                , PackageIds.guidCommandSet.FileReportExplorerCommandId
                , sourceFile
                , Properties.CommandNames.FileReportExplorerCommand
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFile = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.Report).FirstOrDefault();

            helper.HandleOpenReportExplorerCommand(selectedFile?.FileName);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.Report);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}