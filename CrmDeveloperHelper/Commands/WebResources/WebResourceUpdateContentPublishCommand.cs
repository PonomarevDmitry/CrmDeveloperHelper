using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceUpdateContentPublishCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly string _commandNameForCorrection;

        private WebResourceUpdateContentPublishCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles, string commandNameForCorrection)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._commandNameForCorrection = commandNameForCorrection;
        }

        public static WebResourceUpdateContentPublishCommand InstanceCode { get; private set; }

        public static WebResourceUpdateContentPublishCommand InstanceFile { get; private set; }

        public static WebResourceUpdateContentPublishCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new WebResourceUpdateContentPublishCommand(
                commandService
                , PackageIds.guidCommandSet.CodeWebResourceUpdateContentPublishCommandId
                , sourceCode
                , Properties.CommandNames.CodeWebResourceUpdateContentPublishCommand
            );

            InstanceFile = new WebResourceUpdateContentPublishCommand(
                commandService
                , PackageIds.guidCommandSet.FileWebResourceUpdateContentPublishCommandId
                , sourceFile
                , Properties.CommandNames.FileWebResourceUpdateContentPublishCommand
            );

            InstanceFolder = new WebResourceUpdateContentPublishCommand(
                commandService
                , PackageIds.guidCommandSet.FolderWebResourceUpdateContentPublishCommandId
                , sourceFolder
                , Properties.CommandNames.FolderWebResourceUpdateContentPublishCommand
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResource).ToList();

            helper.HandleUpdateContentWebResourcesAndPublishCommand(null, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}