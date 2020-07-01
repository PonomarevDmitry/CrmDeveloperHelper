using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class ReportActionOnComponentInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly ActionOnComponent _actionOnComponent;

        private ReportActionOnComponentInConnectionCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles, ActionOnComponent actionOnComponent)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._actionOnComponent = actionOnComponent;
        }

        public static ReportActionOnComponentInConnectionCommand InstanceCodeOpenInWebInConnection { get; private set; }

        public static ReportActionOnComponentInConnectionCommand InstanceCodeOpenDependentComponentsInWebInConnection { get; private set; }

        public static ReportActionOnComponentInConnectionCommand InstanceCodeOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static ReportActionOnComponentInConnectionCommand InstanceCodeOpenSolutionsContainingComponentInExplorerInConnection { get; private set; }

        public static ReportActionOnComponentInConnectionCommand InstanceFileOpenInWebInConnection { get; private set; }

        public static ReportActionOnComponentInConnectionCommand InstanceFileOpenDependentComponentsInWebInConnection { get; private set; }

        public static ReportActionOnComponentInConnectionCommand InstanceFileOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static ReportActionOnComponentInConnectionCommand InstanceFileOpenSolutionsContainingComponentInExplorerInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFileSingle.CreateSource();

            InstanceCodeOpenInWebInConnection = new ReportActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeReportOpenInWebInConnectionCommandId
                , sourceCode
                , ActionOnComponent.OpenInWeb
            );

            InstanceCodeOpenDependentComponentsInWebInConnection = new ReportActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeReportOpenDependentInWebInConnectionCommandId
                , sourceCode
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceCodeOpenDependentComponentsInExplorerInConnection = new ReportActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeReportOpenDependentInExplorerInConnectionCommandId
                , sourceCode
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceCodeOpenSolutionsContainingComponentInExplorerInConnection = new ReportActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeReportOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , sourceCode
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );




            InstanceFileOpenInWebInConnection = new ReportActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileReportOpenInWebInConnectionCommandId
                , sourceFile
                , ActionOnComponent.OpenInWeb
            );

            InstanceFileOpenDependentComponentsInWebInConnection = new ReportActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileReportOpenDependentInWebInConnectionCommandId
                , sourceFile
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceFileOpenDependentComponentsInExplorerInConnection = new ReportActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileReportOpenDependentInExplorerInConnectionCommandId
                , sourceFile
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceFileOpenSolutionsContainingComponentInExplorerInConnection = new ReportActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileReportOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , sourceFile
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (connectionData.IsReadOnly)
            {
                helper.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsReadOnlyFormat1, connectionData.Name);
                return;
            }

            List<SelectedFile> selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.Report).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleOpenReportCommand(connectionData, selectedFiles.FirstOrDefault(), this._actionOnComponent);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.Report);
        }
    }
}