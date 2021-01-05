using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class JavaScriptShowDifferenceReferencesAndDependencyXmlCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly string _commandNameForCorrection;

        private JavaScriptShowDifferenceReferencesAndDependencyXmlCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles, string commandNameForCorrection)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._commandNameForCorrection = commandNameForCorrection;
        }

        public static JavaScriptShowDifferenceReferencesAndDependencyXmlCommand InstanceCode { get; private set; }

        public static JavaScriptShowDifferenceReferencesAndDependencyXmlCommand InstanceFile { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFileSingle.CreateSource();

            InstanceCode = new JavaScriptShowDifferenceReferencesAndDependencyXmlCommand(
                commandService
                , PackageIds.guidCommandSet.CodeJavaScriptShowDifferenceReferencesAndDependencyXmlCommandId
                , sourceCode
                , Properties.CommandNames.CodeJavaScriptShowDifferenceReferencesAndDependencyXmlCommand
            );

            InstanceFile = new JavaScriptShowDifferenceReferencesAndDependencyXmlCommand(
                commandService
                , PackageIds.guidCommandSet.FileJavaScriptShowDifferenceReferencesAndDependencyXmlCommandId
                , sourceFile
                , Properties.CommandNames.FileJavaScriptShowDifferenceReferencesAndDependencyXmlCommand
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFile = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceJavaScript).FirstOrDefault();

            helper.HandleWebResourceDifferenceReferencesAndDependencyXmlCommand(null, selectedFile);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceJavaScript);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}
