using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class CodeJavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithoutCurrent
    {
        private CodeJavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CodeJavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommandId)
        {
        }

        public static CodeJavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptIncludeReferencesToLinkedSystemFormInConnectionGroupCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsJavaScriptType).ToList();

            helper.HandleIncludeReferencesToLinkedSystemFormsLibrariesCommand(connectionData, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedSystemForm(applicationObject, menuCommand);
        }
    }
}
