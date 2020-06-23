using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class FileJavaScriptUpdateContentIncludeReferencesPublishCommand : AbstractSingleCommand
    {
        private FileJavaScriptUpdateContentIncludeReferencesPublishCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.FileJavaScriptUpdateContentIncludeReferencesPublishCommandId)
        {
        }

        public static FileJavaScriptUpdateContentIncludeReferencesPublishCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileJavaScriptUpdateContentIncludeReferencesPublishCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsJavaScriptType, false).ToList();

            helper.HandleUpdateContentIncludeReferencesToDependencyXmlCommand(null, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerJavaScriptAny(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FileJavaScriptUpdateContentIncludeReferencesPublishCommand);
        }
    }
}