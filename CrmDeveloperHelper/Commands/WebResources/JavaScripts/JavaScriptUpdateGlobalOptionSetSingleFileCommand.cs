using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class JavaScriptUpdateGlobalOptionSetSingleFileCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private JavaScriptUpdateGlobalOptionSetSingleFileCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static JavaScriptUpdateGlobalOptionSetSingleFileCommand InstanceCode { get; private set; }

        public static JavaScriptUpdateGlobalOptionSetSingleFileCommand InstanceDocuments { get; private set; }

        public static JavaScriptUpdateGlobalOptionSetSingleFileCommand InstanceFile { get; private set; }

        public static JavaScriptUpdateGlobalOptionSetSingleFileCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new JavaScriptUpdateGlobalOptionSetSingleFileCommand(commandService, PackageIds.guidDynamicCommandSet.CodeJavaScriptUpdateGlobalOptionSetSingleFileCommandId, sourceCode);

            InstanceDocuments = new JavaScriptUpdateGlobalOptionSetSingleFileCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsJavaScriptUpdateGlobalOptionSetSingleFileCommandId, sourceDocuments);

            InstanceFile = new JavaScriptUpdateGlobalOptionSetSingleFileCommand(commandService, PackageIds.guidDynamicCommandSet.FileJavaScriptUpdateGlobalOptionSetSingleFileCommandId, sourceFile);

            InstanceFolder = new JavaScriptUpdateGlobalOptionSetSingleFileCommand(commandService, PackageIds.guidDynamicCommandSet.FolderJavaScriptUpdateGlobalOptionSetSingleFileCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceJavaScript).ToList();

            helper.HandleJavaScriptGlobalOptionSetFileUpdateSingle(connectionData, selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceJavaScript);
        }
    }
}
