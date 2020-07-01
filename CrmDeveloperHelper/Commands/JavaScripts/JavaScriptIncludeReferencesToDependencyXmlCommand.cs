using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class JavaScriptIncludeReferencesToDependencyXmlCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly string _commandNameForCorrection;

        private JavaScriptIncludeReferencesToDependencyXmlCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles, string commandNameForCorrection)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._commandNameForCorrection = commandNameForCorrection;
        }

        public static JavaScriptIncludeReferencesToDependencyXmlCommand InstanceCode { get; private set; }

        public static JavaScriptIncludeReferencesToDependencyXmlCommand InstanceDocuments { get; private set; }

        public static JavaScriptIncludeReferencesToDependencyXmlCommand InstanceFile { get; private set; }

        public static JavaScriptIncludeReferencesToDependencyXmlCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new JavaScriptIncludeReferencesToDependencyXmlCommand(
                commandService
                , PackageIds.guidCommandSet.CodeJavaScriptIncludeReferencesToDependencyXmlCommandId
                , sourceCode
                , Properties.CommandNames.CodeJavaScriptIncludeReferencesToDependencyXmlCommand
            );

            InstanceDocuments = new JavaScriptIncludeReferencesToDependencyXmlCommand(
                commandService
                , PackageIds.guidCommandSet.DocumentsJavaScriptIncludeReferencesToDependencyXmlCommandId
                , sourceDocuments
                , Properties.CommandNames.DocumentsJavaScriptIncludeReferencesToDependencyXmlCommand
            );

            InstanceFile = new JavaScriptIncludeReferencesToDependencyXmlCommand(
                commandService
                , PackageIds.guidCommandSet.FileJavaScriptIncludeReferencesToDependencyXmlCommandId
                , sourceFile
                , Properties.CommandNames.FileJavaScriptIncludeReferencesToDependencyXmlCommand
            );

            InstanceFolder = new JavaScriptIncludeReferencesToDependencyXmlCommand(
                commandService
                , PackageIds.guidCommandSet.FolderJavaScriptIncludeReferencesToDependencyXmlCommandId
                , sourceFolder
                , Properties.CommandNames.FolderJavaScriptIncludeReferencesToDependencyXmlCommand
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceJavaScript).ToList();

            helper.HandleIncludeReferencesToDependencyXmlCommand(null, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceJavaScript);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}