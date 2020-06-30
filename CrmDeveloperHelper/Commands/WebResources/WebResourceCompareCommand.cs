using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceCompareCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly bool _withDetails;
        private readonly string _commandNameForCorrection;

        private WebResourceCompareCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles, bool withDetails, string commandNameForCorrection)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._withDetails = withDetails;
            this._commandNameForCorrection = commandNameForCorrection;
        }

        public static WebResourceCompareCommand InstanceDocuments { get; private set; }

        public static WebResourceCompareCommand InstanceFile { get; private set; }

        public static WebResourceCompareCommand InstanceFolder { get; private set; }

        public static WebResourceCompareCommand InstanceWithDetailsCode { get; private set; }

        public static WebResourceCompareCommand InstanceWithDetailsDocuments { get; private set; }

        public static WebResourceCompareCommand InstanceWithDetailsFile { get; private set; }

        public static WebResourceCompareCommand InstanceWithDetailsFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceDocuments = new WebResourceCompareCommand(
                commandService
                , PackageIds.guidCommandSet.DocumentsWebResourceCompareCommandId
                , sourceDocuments
                , false
                , Properties.CommandNames.DocumentsWebResourceCompareCommand
            );

            InstanceFile = new WebResourceCompareCommand(
                commandService
                , PackageIds.guidCommandSet.FileWebResourceCompareCommandId
                , sourceFile
                , false
                , Properties.CommandNames.FileWebResourceCompareCommand
            );

            InstanceFolder = new WebResourceCompareCommand(
                commandService
                , PackageIds.guidCommandSet.FolderWebResourceCompareCommandId
                , sourceFolder
                , false
                , Properties.CommandNames.FolderWebResourceCompareCommand
            );

            InstanceWithDetailsCode = new WebResourceCompareCommand(
                commandService
                , PackageIds.guidCommandSet.CodeWebResourceCompareWithDetailsCommandId
                , sourceCode
                , true
                , Properties.CommandNames.CodeWebResourceCompareWithDetailsCommand
            );

            InstanceWithDetailsDocuments = new WebResourceCompareCommand(
                commandService
                , PackageIds.guidCommandSet.DocumentsWebResourceCompareWithDetailsCommandId
                , sourceDocuments
                , true
                , Properties.CommandNames.DocumentsWebResourceCompareWithDetailsCommand
            );

            InstanceWithDetailsFile = new WebResourceCompareCommand(
                commandService
                , PackageIds.guidCommandSet.FileWebResourceCompareWithDetailsCommandId
                , sourceFile
                , true
                , Properties.CommandNames.FileWebResourceCompareWithDetailsCommand
            );

            InstanceWithDetailsFolder = new WebResourceCompareCommand(
                commandService
                , PackageIds.guidCommandSet.FolderWebResourceCompareWithDetailsCommandId
                , sourceFolder
                , true
                , Properties.CommandNames.FolderWebResourceCompareWithDetailsCommand
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = null;

            if (this._withDetails)
            {
                selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceText).ToList();
            }
            else
            {
                selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResource).ToList();
            }

            if (selectedFiles != null)
            {
                helper.HandleWebResourceCompareCommand(null, selectedFiles, this._withDetails);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            if (this._withDetails)
            {
                _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceText);
            }
            else
            {
                _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);
            }

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}