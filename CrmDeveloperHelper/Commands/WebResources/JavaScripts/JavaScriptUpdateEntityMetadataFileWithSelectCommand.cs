using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class JavaScriptUpdateEntityMetadataFileWithSelectCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly string _commandNameForCorrection;

        private JavaScriptUpdateEntityMetadataFileWithSelectCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles, string commandNameForCorrection)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._commandNameForCorrection = commandNameForCorrection;
        }

        public static JavaScriptUpdateEntityMetadataFileWithSelectCommand InstanceCode { get; private set; }

        public static JavaScriptUpdateEntityMetadataFileWithSelectCommand InstanceFile { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFileSingle.CreateSource();

            InstanceCode = new JavaScriptUpdateEntityMetadataFileWithSelectCommand(
                commandService
                , PackageIds.guidCommandSet.CodeJavaScriptUpdateEntityMetadataFileWithSelectCommandId
                , sourceCode
                , Properties.CommandNames.CodeJavaScriptUpdateEntityMetadataFileWithSelectCommand
            );

            InstanceFile = new JavaScriptUpdateEntityMetadataFileWithSelectCommand(
                commandService
                , PackageIds.guidCommandSet.FileJavaScriptUpdateEntityMetadataFileWithSelectCommandId
                , sourceFile
                , Properties.CommandNames.FileJavaScriptUpdateEntityMetadataFileWithSelectCommand
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceJavaScript).ToList();

            helper.HandleJavaScriptEntityMetadataFileUpdate(null, selectedFiles, true);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceJavaScript);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}