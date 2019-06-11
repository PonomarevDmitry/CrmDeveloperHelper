using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.ComponentModel.Design;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Folders
{
    internal sealed class FolderAddEntityMetadataFileInConnectionCommand : AbstractCommandByConnectionAll
    {
        private FolderAddEntityMetadataFileInConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.FolderAddEntityMetadataFileInConnectionCommandId
            )
        {

        }

        public static FolderAddEntityMetadataFileInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderAddEntityMetadataFileInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            SelectedItem selectedItem = helper.GetSelectedProjectItem();

            if (selectedItem != null)
            {
                helper.HandleExportFileWithEntityMetadata(connectionData, selectedItem);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerFolderSingle(applicationObject, menuCommand);
        }
    }
}
