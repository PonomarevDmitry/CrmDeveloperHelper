using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class JavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithoutCurrent
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private JavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static JavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommand InstanceCode { get; private set; }

        public static JavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommand InstanceDocuments { get; private set; }

        public static JavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommand InstanceFile { get; private set; }

        public static JavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new JavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.CodeJavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommandId, sourceCode);

            InstanceDocuments = new JavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsJavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommandId, sourceDocuments);

            InstanceFile = new JavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.FileJavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommandId, sourceFile);

            InstanceFolder = new JavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.FolderJavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (connectionData.IsReadOnly)
            {
                helper.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsReadOnlyFormat1, connectionData.Name);
                return;
            }

            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceJavaScriptHasLinkedSystemForm).ToList();

            helper.HandleIncludeReferencesToLinkedSystemFormsLibrariesCommand(connectionData, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);

            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceJavaScriptHasLinkedSystemForm);
        }
    }
}
