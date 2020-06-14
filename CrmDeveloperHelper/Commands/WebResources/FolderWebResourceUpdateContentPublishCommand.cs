using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FolderWebResourceUpdateContentPublishCommand : AbstractSingleCommand
    {
        private FolderWebResourceUpdateContentPublishCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.FolderWebResourceUpdateContentPublishCommandId)
        {
        }

        public static FolderWebResourceUpdateContentPublishCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderWebResourceUpdateContentPublishCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, true).ToList();

            helper.HandleUpdateContentWebResourcesAndPublishCommand(null, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceRecursive(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FolderWebResourceUpdateContentPublishCommand);
        }
    }
}