using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class JavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithoutCurrent
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private JavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static JavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommand InstanceCode { get; private set; }

        public static JavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommand InstanceDocuments { get; private set; }

        public static JavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommand InstanceFile { get; private set; }

        public static JavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new JavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.CodeJavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommandId, sourceCode);

            InstanceDocuments = new JavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsJavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommandId, sourceDocuments);

            InstanceFile = new JavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.FileJavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommandId, sourceFile);

            InstanceFolder = new JavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.FolderJavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (connectionData.IsReadOnly)
            {
                helper.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsReadOnlyFormat1, connectionData.Name);
                return;
            }

            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceJavaScript).ToList();

            helper.HandleIncludeReferencesToDependencyXmlCommand(connectionData, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);

            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceJavaScript);
        }
    }
}