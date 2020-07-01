using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceShowDependentComponentsCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private WebResourceShowDependentComponentsCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static WebResourceShowDependentComponentsCommand InstanceDocuments { get; private set; }

        public static WebResourceShowDependentComponentsCommand InstanceFile { get; private set; }

        public static WebResourceShowDependentComponentsCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceDocuments = new WebResourceShowDependentComponentsCommand(commandService, PackageIds.guidCommandSet.DocumentsWebResourceShowDependentComponentsCommandId, sourceDocuments);

            InstanceFile = new WebResourceShowDependentComponentsCommand(commandService, PackageIds.guidCommandSet.FileWebResourceShowDependentComponentsCommandId, sourceFile);

            InstanceFolder = new WebResourceShowDependentComponentsCommand(commandService, PackageIds.guidCommandSet.FolderWebResourceShowDependentComponentsCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResource).ToList();

            helper.HandleShowingWebResourcesDependentComponents(selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);
        }
    }
}