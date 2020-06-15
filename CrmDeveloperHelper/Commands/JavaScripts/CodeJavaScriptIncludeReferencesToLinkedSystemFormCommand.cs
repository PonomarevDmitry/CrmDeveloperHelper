using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class CodeJavaScriptIncludeReferencesToLinkedSystemFormCommand : AbstractSingleCommand
    {
        private CodeJavaScriptIncludeReferencesToLinkedSystemFormCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeJavaScriptIncludeReferencesToLinkedSystemFormCommandId)
        {
        }

        public static CodeJavaScriptIncludeReferencesToLinkedSystemFormCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptIncludeReferencesToLinkedSystemFormCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsJavaScriptType).ToList();

            helper.HandleIncludeReferencesToLinkedSystemFormsLibrariesCommand(null, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedSystemForm(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeJavaScriptIncludeReferencesToLinkedSystemFormCommand);
        }
    }
}