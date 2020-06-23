using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class FolderJavaScriptUpdateContentIncludeReferencesPublishCommand : AbstractSingleCommand
    {
        private FolderJavaScriptUpdateContentIncludeReferencesPublishCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.FolderJavaScriptUpdateContentIncludeReferencesPublishCommandId)
        {
        }

        public static FolderJavaScriptUpdateContentIncludeReferencesPublishCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderJavaScriptUpdateContentIncludeReferencesPublishCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsJavaScriptType, true).ToList();

            helper.HandleUpdateContentIncludeReferencesToDependencyXmlCommand(null, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerJavaScriptRecursive(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FolderJavaScriptUpdateContentIncludeReferencesPublishCommand);
        }
    }
}