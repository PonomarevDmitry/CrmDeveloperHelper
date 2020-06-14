using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class FolderJavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithoutCurrent
    {
        private FolderJavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.FolderJavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommandId)
        {
        }

        public static FolderJavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderJavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsJavaScriptType, true).ToList();

            helper.HandleIncludeReferencesToDependencyXmlCommand(connectionData, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerJavaScriptRecursive(applicationObject, menuCommand);
        }
    }
}
