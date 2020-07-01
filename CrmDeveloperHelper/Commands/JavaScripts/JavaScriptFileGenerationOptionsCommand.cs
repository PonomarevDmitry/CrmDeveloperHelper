using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class JavaScriptFileGenerationOptionsCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private JavaScriptFileGenerationOptionsCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static JavaScriptFileGenerationOptionsCommand InstanceCode { get; private set; }

        public static JavaScriptFileGenerationOptionsCommand InstanceDocuments { get; private set; }

        public static JavaScriptFileGenerationOptionsCommand InstanceFile { get; private set; }

        public static JavaScriptFileGenerationOptionsCommand InstanceFileSingle { get; private set; }

        public static JavaScriptFileGenerationOptionsCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFileSingle = FileSourceSelectedFileSingle.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new JavaScriptFileGenerationOptionsCommand(commandService, PackageIds.guidCommandSet.CodeJavaScriptFileGenerationOptionsCommandId, sourceCode);

            InstanceDocuments = new JavaScriptFileGenerationOptionsCommand(commandService, PackageIds.guidCommandSet.DocumentsJavaScriptFileGenerationOptionsCommandId, sourceDocuments);

            InstanceFile = new JavaScriptFileGenerationOptionsCommand(commandService, PackageIds.guidCommandSet.FileJavaScriptFileGenerationOptionsCommandId, sourceFile);

            InstanceFileSingle = new JavaScriptFileGenerationOptionsCommand(commandService, PackageIds.guidCommandSet.FileJavaScriptFileGenerationOptionsSingleCommandId, sourceFileSingle);

            InstanceFolder = new JavaScriptFileGenerationOptionsCommand(commandService, PackageIds.guidCommandSet.FolderJavaScriptFileGenerationOptionsCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleJavaScriptFileGenerationOptions();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceJavaScript);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.JavaScriptFileGenerationOptionsCommand);
        }
    }
}