using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FolderWebResourceUpdateContentPublishGroupConnectionCommand : AbstractDynamicCommandByConnectionByGroupWithoutCurrent
    {
        private FolderWebResourceUpdateContentPublishGroupConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.FolderWebResourceUpdateContentPublishGroupConnectionCommandId)
        {
        }

        public static FolderWebResourceUpdateContentPublishGroupConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderWebResourceUpdateContentPublishGroupConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (!connectionData.IsReadOnly)
            {
                List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, true).ToList();

                helper.HandleUpdateContentWebResourcesAndPublishCommand(connectionData, selectedFiles);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            if (connectionData.IsReadOnly)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
            else
            {
                menuCommand.Enabled = menuCommand.Visible = true;

                CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursive(applicationObject, menuCommand);
            }
        }
    }
}