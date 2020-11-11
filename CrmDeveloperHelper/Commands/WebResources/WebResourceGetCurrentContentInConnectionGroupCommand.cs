using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceGetCurrentContentInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithoutCurrent
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private WebResourceGetCurrentContentInConnectionGroupCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static WebResourceGetCurrentContentInConnectionGroupCommand InstanceCode { get; private set; }

        public static WebResourceGetCurrentContentInConnectionGroupCommand InstanceDocuments { get; private set; }

        public static WebResourceGetCurrentContentInConnectionGroupCommand InstanceFile { get; private set; }

        public static WebResourceGetCurrentContentInConnectionGroupCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new WebResourceGetCurrentContentInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.CodeWebResourceGetCurrentContentInConnectionGroupCommandId, sourceCode);

            InstanceDocuments = new WebResourceGetCurrentContentInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsWebResourceGetCurrentContentInConnectionGroupCommandId, sourceDocuments);

            InstanceFile = new WebResourceGetCurrentContentInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.FileWebResourceGetCurrentContentInConnectionGroupCommandId, sourceFile);

            InstanceFolder = new WebResourceGetCurrentContentInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.FolderWebResourceGetCurrentContentInConnectionGroupCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResource).ToList();

            helper.HandleWebResourcesGetContentCommand(connectionData, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);
        }
    }
}