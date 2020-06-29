using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class JavaScriptIncludeReferencesToLinkedSystemFormCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly string _commandNameForCorrection;

        private JavaScriptIncludeReferencesToLinkedSystemFormCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles, string commandNameForCorrection)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._commandNameForCorrection = commandNameForCorrection;
        }

        public static JavaScriptIncludeReferencesToLinkedSystemFormCommand InstanceCode { get; private set; }

        public static JavaScriptIncludeReferencesToLinkedSystemFormCommand InstanceDocuments { get; private set; }

        public static JavaScriptIncludeReferencesToLinkedSystemFormCommand InstanceFile { get; private set; }

        public static JavaScriptIncludeReferencesToLinkedSystemFormCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new JavaScriptIncludeReferencesToLinkedSystemFormCommand(
                commandService
                , PackageIds.guidCommandSet.CodeJavaScriptIncludeReferencesToLinkedSystemFormCommandId
                , sourceCode
                , Properties.CommandNames.CodeJavaScriptIncludeReferencesToLinkedSystemFormCommand
            );

            InstanceDocuments = new JavaScriptIncludeReferencesToLinkedSystemFormCommand(
                commandService
                , PackageIds.guidCommandSet.DocumentsJavaScriptIncludeReferencesToLinkedSystemFormCommandId
                , sourceDocuments
                , Properties.CommandNames.DocumentsJavaScriptIncludeReferencesToLinkedSystemFormCommand
            );

            InstanceFile = new JavaScriptIncludeReferencesToLinkedSystemFormCommand(
                commandService
                , PackageIds.guidCommandSet.FileJavaScriptIncludeReferencesToLinkedSystemFormCommandId
                , sourceFile
                , Properties.CommandNames.FileJavaScriptIncludeReferencesToLinkedSystemFormCommand
            );

            InstanceFolder = new JavaScriptIncludeReferencesToLinkedSystemFormCommand(
                commandService
                , PackageIds.guidCommandSet.FolderJavaScriptIncludeReferencesToLinkedSystemFormCommandId
                , sourceFolder
                , Properties.CommandNames.FolderJavaScriptIncludeReferencesToLinkedSystemFormCommand
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceJavaScriptHasLinkedSystemForm).ToList();

            helper.HandleIncludeReferencesToLinkedSystemFormsLibrariesCommand(null, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceJavaScriptHasLinkedSystemForm);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}