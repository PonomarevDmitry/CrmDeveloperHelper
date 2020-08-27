using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class JavaScriptUpdateContentIncludeReferencesPublishCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly string _commandNameForCorrection;

        private JavaScriptUpdateContentIncludeReferencesPublishCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles, string commandNameForCorrection)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._commandNameForCorrection = commandNameForCorrection;
        }

        public static JavaScriptUpdateContentIncludeReferencesPublishCommand InstanceCode { get; private set; }

        public static JavaScriptUpdateContentIncludeReferencesPublishCommand InstanceFile { get; private set; }

        public static JavaScriptUpdateContentIncludeReferencesPublishCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new JavaScriptUpdateContentIncludeReferencesPublishCommand(
                commandService
                , PackageIds.guidCommandSet.CodeJavaScriptUpdateContentIncludeReferencesPublishCommandId
                , sourceCode
                , Properties.CommandNames.CodeJavaScriptUpdateContentIncludeReferencesPublishCommand
            );

            InstanceFile = new JavaScriptUpdateContentIncludeReferencesPublishCommand(
                commandService
                , PackageIds.guidCommandSet.FileJavaScriptUpdateContentIncludeReferencesPublishCommandId
                , sourceFile
                , Properties.CommandNames.FileJavaScriptUpdateContentIncludeReferencesPublishCommand
            );

            InstanceFolder = new JavaScriptUpdateContentIncludeReferencesPublishCommand(
                commandService
                , PackageIds.guidCommandSet.FolderJavaScriptUpdateContentIncludeReferencesPublishCommandId
                , sourceFolder
                , Properties.CommandNames.FolderJavaScriptUpdateContentIncludeReferencesPublishCommand
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResource).ToList();

            helper.HandleUpdateContentIncludeReferencesToDependencyXmlCommand(null, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceJavaScript);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}
