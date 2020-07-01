using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceCheckEncodingOpenFilesCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private WebResourceCheckEncodingOpenFilesCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static WebResourceCheckEncodingOpenFilesCommand InstanceFile { get; private set; }

        public static WebResourceCheckEncodingOpenFilesCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceFile = new WebResourceCheckEncodingOpenFilesCommand(commandService, PackageIds.guidCommandSet.FileWebResourceCheckEncodingOpenFilesCommandId, sourceFile);

            InstanceFolder = new WebResourceCheckEncodingOpenFilesCommand(commandService, PackageIds.guidCommandSet.FolderWebResourceCheckEncodingOpenFilesCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceText).ToList();

            helper.HandleWebResourceCheckOpenFilesWithoutUTF8EncodingCommand(selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceText);
        }
    }
}