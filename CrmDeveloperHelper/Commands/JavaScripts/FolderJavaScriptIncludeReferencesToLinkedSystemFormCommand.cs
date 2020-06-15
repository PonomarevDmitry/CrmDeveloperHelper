using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class FolderJavaScriptIncludeReferencesToLinkedSystemFormCommand : AbstractSingleCommand
    {
        private FolderJavaScriptIncludeReferencesToLinkedSystemFormCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.FolderJavaScriptIncludeReferencesToLinkedSystemFormCommandId)
        {
        }

        public static FolderJavaScriptIncludeReferencesToLinkedSystemFormCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderJavaScriptIncludeReferencesToLinkedSystemFormCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsJavaScriptType, true).ToList();

            helper.HandleIncludeReferencesToLinkedSystemFormsLibrariesCommand(null, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerJavaScriptRecursive(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FolderJavaScriptIncludeReferencesToLinkedSystemFormCommand);
        }
    }
}