using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceLinkCreateCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private WebResourceLinkCreateCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static WebResourceLinkCreateCommand InstanceCode { get; private set; }

        public static WebResourceLinkCreateCommand InstanceDocuments { get; private set; }

        public static WebResourceLinkCreateCommand InstanceFile { get; private set; }

        public static WebResourceLinkCreateCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new WebResourceLinkCreateCommand(commandService, PackageIds.guidCommandSet.CodeWebResourceLinkCreateCommandId, sourceCode);

            InstanceDocuments = new WebResourceLinkCreateCommand(commandService, PackageIds.guidCommandSet.DocumentsWebResourceLinkCreateCommandId, sourceDocuments);

            InstanceFile = new WebResourceLinkCreateCommand(commandService, PackageIds.guidCommandSet.FileWebResourceLinkCreateCommandId, sourceFile);

            InstanceFolder = new WebResourceLinkCreateCommand(commandService, PackageIds.guidCommandSet.FolderWebResourceLinkCreateCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResource).ToList();

            helper.HandleWebResourceCreateLaskLinkMultipleCommand(selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);
        }
    }
}