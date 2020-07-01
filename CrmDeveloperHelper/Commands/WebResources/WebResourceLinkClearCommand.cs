using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceLinkClearCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private WebResourceLinkClearCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static WebResourceLinkClearCommand InstanceCode { get; private set; }

        public static WebResourceLinkClearCommand InstanceDocuments { get; private set; }

        public static WebResourceLinkClearCommand InstanceFile { get; private set; }

        public static WebResourceLinkClearCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new WebResourceLinkClearCommand(commandService, PackageIds.guidCommandSet.CodeWebResourceLinkClearCommandId, sourceCode);

            InstanceDocuments = new WebResourceLinkClearCommand(commandService, PackageIds.guidCommandSet.DocumentsWebResourceLinkClearCommandId, sourceDocuments);

            InstanceFile = new WebResourceLinkClearCommand(commandService, PackageIds.guidCommandSet.FileWebResourceLinkClearCommandId, sourceFile);

            InstanceFolder = new WebResourceLinkClearCommand(commandService, PackageIds.guidCommandSet.FolderWebResourceLinkClearCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResource).ToList();

            helper.HandleFileClearLink(selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);
        }
    }
}