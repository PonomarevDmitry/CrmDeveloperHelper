using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class DocumentsJavaScriptUpdateEntityMetadataFileCommand : AbstractDynamicCommandByConnectionAll
    {
        private DocumentsJavaScriptUpdateEntityMetadataFileCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.DocumentsJavaScriptUpdateEntityMetadataFileCommandId)
        {
        }

        public static DocumentsJavaScriptUpdateEntityMetadataFileCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsJavaScriptUpdateEntityMetadataFileCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsJavaScriptType).ToList();

            helper.HandleJavaScriptEntityMetadataFileUpdate(connectionData, selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsJavaScript(applicationObject, menuCommand);
        }
    }
}