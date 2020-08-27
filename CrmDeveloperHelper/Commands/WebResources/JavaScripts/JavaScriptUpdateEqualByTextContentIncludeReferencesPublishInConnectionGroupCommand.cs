using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class JavaScriptUpdateEqualByTextContentIncludeReferencesPublishInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithCurrent
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private JavaScriptUpdateEqualByTextContentIncludeReferencesPublishInConnectionGroupCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static JavaScriptUpdateEqualByTextContentIncludeReferencesPublishInConnectionGroupCommand InstanceDocuments { get; private set; }

        public static JavaScriptUpdateEqualByTextContentIncludeReferencesPublishInConnectionGroupCommand InstanceFile { get; private set; }

        public static JavaScriptUpdateEqualByTextContentIncludeReferencesPublishInConnectionGroupCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceDocuments = new JavaScriptUpdateEqualByTextContentIncludeReferencesPublishInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsJavaScriptUpdateEqualByTextContentIncludeReferencesPublishInConnectionGroupCommandId, sourceDocuments);

            InstanceFile = new JavaScriptUpdateEqualByTextContentIncludeReferencesPublishInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.FileJavaScriptUpdateEqualByTextContentIncludeReferencesPublishInConnectionGroupCommandId, sourceFile);

            InstanceFolder = new JavaScriptUpdateEqualByTextContentIncludeReferencesPublishInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.FolderJavaScriptUpdateEqualByTextContentIncludeReferencesPublishInConnectionGroupCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (connectionData.IsReadOnly)
            {
                helper.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsReadOnlyFormat1, connectionData.Name);
                return;
            }

            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceJavaScript).ToList();

            helper.HandleUpdateEqualByTextContentIncludeReferencesToDependencyXmlCommand(connectionData, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);

            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceJavaScript);
        }
    }
}
