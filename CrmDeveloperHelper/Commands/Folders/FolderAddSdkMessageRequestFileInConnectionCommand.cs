using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;


namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Folders
{
    internal sealed class FolderAddSdkMessageRequestFileInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private FolderAddSdkMessageRequestFileInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.FolderAddSdkMessageRequestFileInConnectionCommandId)
        {
        }

        public static FolderAddSdkMessageRequestFileInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderAddSdkMessageRequestFileInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            SelectedItem selectedItem = helper.GetSelectedProjectItem();

            if (selectedItem != null)
            {
                helper.HandleSdkMessageRequestTree(connectionData, selectedItem);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerFolderSingle(applicationObject, menuCommand);
        }
    }
}