using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class CodeJavaScriptUpdateEntityMetadataFileWithSelectCommand : AbstractCommand
    {
        private CodeJavaScriptUpdateEntityMetadataFileWithSelectCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeJavaScriptUpdateEntityMetadataFileWithSelectCommandId) { }

        public static CodeJavaScriptUpdateEntityMetadataFileWithSelectCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptUpdateEntityMetadataFileWithSelectCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsJavaScriptType).ToList();

            helper.HandleJavaScriptEntityMetadataFileUpdate(null, selectedFiles, true);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScript(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeJavaScriptUpdateEntityMetadataFileWithSelectCommand);
        }
    }
}