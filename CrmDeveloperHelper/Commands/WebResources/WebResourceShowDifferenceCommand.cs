using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceShowDifferenceCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly bool _withSelect;
        private readonly string _commandNameForCorrection;

        private WebResourceShowDifferenceCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles, bool withSelect, string commandNameForCorrection)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._withSelect = withSelect;
            this._commandNameForCorrection = commandNameForCorrection;
        }

        public static WebResourceShowDifferenceCommand InstanceCode { get; private set; }

        public static WebResourceShowDifferenceCommand InstanceFile { get; private set; }

        public static WebResourceShowDifferenceCommand InstanceWithSelectCode { get; private set; }

        public static WebResourceShowDifferenceCommand InstanceWithSelectFile { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFileSingle.CreateSource();

            InstanceCode = new WebResourceShowDifferenceCommand(
                commandService
                , PackageIds.guidCommandSet.CodeWebResourceShowDifferenceCommandId
                , sourceCode
                , false
                , Properties.CommandNames.CodeWebResourceShowDifferenceCommand
            );

            InstanceFile = new WebResourceShowDifferenceCommand(
                commandService
                , PackageIds.guidCommandSet.FileWebResourceShowDifferenceCommandId
                , sourceFile
                , false
                , Properties.CommandNames.FileWebResourceShowDifferenceCommand
            );

            InstanceWithSelectCode = new WebResourceShowDifferenceCommand(
                commandService
                , PackageIds.guidCommandSet.CodeWebResourceShowDifferenceWithSelectCommandId
                , sourceCode
                , true
                , Properties.CommandNames.CodeWebResourceShowDifferenceWithSelectCommand
            );

            InstanceWithSelectFile = new WebResourceShowDifferenceCommand(
                commandService
                , PackageIds.guidCommandSet.FileWebResourceShowDifferenceWithSelectCommandId
                , sourceFile
                , true
                , Properties.CommandNames.FileWebResourceShowDifferenceWithSelectCommand
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFile = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceText).FirstOrDefault();

            helper.HandleWebResourceDifferenceCommand(null, selectedFile, _withSelect);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceText);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}
