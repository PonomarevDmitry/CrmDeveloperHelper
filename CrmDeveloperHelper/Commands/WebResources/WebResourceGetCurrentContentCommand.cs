using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceGetCurrentContentCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly string _commandNameForCorrection;

        private WebResourceGetCurrentContentCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles, string commandNameForCorrection)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._commandNameForCorrection = commandNameForCorrection;
        }

        public static WebResourceGetCurrentContentCommand InstanceCode { get; private set; }

        public static WebResourceGetCurrentContentCommand InstanceDocuments { get; private set; }

        public static WebResourceGetCurrentContentCommand InstanceFile { get; private set; }

        public static WebResourceGetCurrentContentCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new WebResourceGetCurrentContentCommand(
                commandService
                , PackageIds.guidCommandSet.CodeWebResourceGetCurrentContentCommandId
                , sourceCode
                , Properties.CommandNames.CodeWebResourceGetCurrentContentCommand
            );

            InstanceDocuments = new WebResourceGetCurrentContentCommand(
                commandService
                , PackageIds.guidCommandSet.DocumentsWebResourceGetCurrentContentCommandId
                , sourceDocuments
                , Properties.CommandNames.DocumentsWebResourceGetCurrentContentCommand
            );

            InstanceFile = new WebResourceGetCurrentContentCommand(
                commandService
                , PackageIds.guidCommandSet.FileWebResourceGetCurrentContentCommandId
                , sourceFile
                , Properties.CommandNames.FileWebResourceGetCurrentContentCommand
            );

            InstanceFolder = new WebResourceGetCurrentContentCommand(
                commandService
                , PackageIds.guidCommandSet.FolderWebResourceGetCurrentContentCommandId
                , sourceFolder
                , Properties.CommandNames.FolderWebResourceGetCurrentContentCommand
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResource).ToList();

            helper.HandleWebResourcesGetContentCommand(null, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}