using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class JavaScriptUpdateEntityMetadataFileCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private JavaScriptUpdateEntityMetadataFileCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static JavaScriptUpdateEntityMetadataFileCommand InstanceCode { get; private set; }

        public static JavaScriptUpdateEntityMetadataFileCommand InstanceDocuments { get; private set; }

        public static JavaScriptUpdateEntityMetadataFileCommand InstanceFile { get; private set; }

        public static JavaScriptUpdateEntityMetadataFileCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new JavaScriptUpdateEntityMetadataFileCommand(commandService, PackageIds.guidDynamicCommandSet.CodeJavaScriptUpdateEntityMetadataFileCommandId, sourceCode);

            InstanceDocuments = new JavaScriptUpdateEntityMetadataFileCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsJavaScriptUpdateEntityMetadataFileCommandId, sourceDocuments);

            InstanceFile = new JavaScriptUpdateEntityMetadataFileCommand(commandService, PackageIds.guidDynamicCommandSet.FileJavaScriptUpdateEntityMetadataFileCommandId, sourceFile);

            InstanceFolder = new JavaScriptUpdateEntityMetadataFileCommand(commandService, PackageIds.guidDynamicCommandSet.FolderJavaScriptUpdateEntityMetadataFileCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceJavaScript).ToList();

            helper.HandleJavaScriptEntityMetadataFileUpdate(connectionData, selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceJavaScript);
        }
    }
}