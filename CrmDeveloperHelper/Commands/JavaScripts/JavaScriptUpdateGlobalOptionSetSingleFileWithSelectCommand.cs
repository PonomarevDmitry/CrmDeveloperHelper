using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class JavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly string _commandNameForCorrection;

        private JavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles, string commandNameForCorrection)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._commandNameForCorrection = commandNameForCorrection;
        }

        public static JavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand InstanceCode { get; private set; }

        public static JavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand InstanceFile { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFileSingle.CreateSource();

            InstanceCode = new JavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand(
                commandService
                , PackageIds.guidCommandSet.CodeJavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommandId
                , sourceCode
                , Properties.CommandNames.CodeJavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand
            );

            InstanceFile = new JavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand(
                commandService
                , PackageIds.guidCommandSet.FileJavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommandId
                , sourceFile
                , Properties.CommandNames.FileJavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceJavaScript).ToList();

            helper.HandleJavaScriptGlobalOptionSetFileUpdateSingle(null, selectedFiles, true);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceJavaScript);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}
