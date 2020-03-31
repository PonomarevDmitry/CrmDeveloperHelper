using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class CodeJavaScriptUpdateGlobalOptionSetSingleFileCommand : AbstractDynamicCommandByConnectionAll
    {
        private CodeJavaScriptUpdateGlobalOptionSetSingleFileCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptUpdateGlobalOptionSetSingleFileCommandId
            )
        {

        }

        public static CodeJavaScriptUpdateGlobalOptionSetSingleFileCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptUpdateGlobalOptionSetSingleFileCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsJavaScriptType).ToList();

            helper.HandleJavaScriptGlobalOptionSetFileUpdateSingle(connectionData, selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScript(applicationObject, menuCommand);
        }
    }
}