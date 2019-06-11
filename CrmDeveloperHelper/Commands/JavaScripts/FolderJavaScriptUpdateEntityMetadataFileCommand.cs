using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class FolderJavaScriptUpdateEntityMetadataFileCommand : AbstractDynamicCommandByConnectionAll
    {
        private FolderJavaScriptUpdateEntityMetadataFileCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.FolderJavaScriptUpdateEntityMetadataFileCommandId
            )
        {

        }

        public static FolderJavaScriptUpdateEntityMetadataFileCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderJavaScriptUpdateEntityMetadataFileCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsJavaScriptType, true).ToList();

            helper.HandleUpdateEntityMetadataFileJavaScript(connectionData, selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerJavaScriptRecursive(applicationObject, menuCommand);
        }
    }
}