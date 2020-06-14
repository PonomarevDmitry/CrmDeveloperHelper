using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class FolderJavaScriptIncludeReferencesToDependencyXmlCommand : AbstractSingleCommand
    {
        private FolderJavaScriptIncludeReferencesToDependencyXmlCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.FolderJavaScriptIncludeReferencesToDependencyXmlCommandId)
        {
        }

        public static FolderJavaScriptIncludeReferencesToDependencyXmlCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderJavaScriptIncludeReferencesToDependencyXmlCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsJavaScriptType, true).ToList();

            helper.HandleIncludeReferencesToDependencyXmlCommand(null, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerJavaScriptRecursive(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FolderJavaScriptIncludeReferencesToDependencyXmlCommand);
        }
    }
}
