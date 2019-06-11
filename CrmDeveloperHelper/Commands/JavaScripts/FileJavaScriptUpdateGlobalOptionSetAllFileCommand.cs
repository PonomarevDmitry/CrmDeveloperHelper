using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class FileJavaScriptUpdateGlobalOptionSetAllFileCommand : AbstractCommandByConnectionAll
    {
        private FileJavaScriptUpdateGlobalOptionSetAllFileCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.FileJavaScriptUpdateGlobalOptionSetAllFileCommandId
            )
        {

        }

        public static FileJavaScriptUpdateGlobalOptionSetAllFileCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileJavaScriptUpdateGlobalOptionSetAllFileCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsJavaScriptType, false).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleUpdateGlobalOptionSetAllFileJavaScript(connectionData, selectedFiles[0]);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerJavaScriptSingle(applicationObject, menuCommand);
        }
    }
}