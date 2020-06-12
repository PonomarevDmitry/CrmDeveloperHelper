using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FolderWebResourceCheckEncodingCommand : AbstractSingleCommand
    {
        private FolderWebResourceCheckEncodingCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.FolderWebResourceCheckEncodingCommandId) { }

        public static FolderWebResourceCheckEncodingCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderWebResourceCheckEncodingCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, true).ToList();

            helper.HandleWebResourceCheckFileEncodingCommand(selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursive(applicationObject, menuCommand);
        }
    }
}
