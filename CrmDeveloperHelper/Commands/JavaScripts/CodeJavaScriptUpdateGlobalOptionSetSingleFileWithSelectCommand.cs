using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class CodeJavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand : AbstractCommand
    {
        private CodeJavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.CodeJavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommandId) { }

        public static CodeJavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsJavaScriptType).ToList();

            helper.HandleUpdateGlobalOptionSetSingleFileJavaScript(null, selectedFiles, true);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScript(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeJavaScriptUpdateGlobalOptionSetSingleFileWithSelectCommand);
        }
    }
}