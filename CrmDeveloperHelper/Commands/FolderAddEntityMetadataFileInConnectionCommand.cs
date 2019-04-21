using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FolderAddEntityMetadataFileInConnectionCommand : IServiceProviderOwner
    {
        private readonly Package _package;

        public IServiceProvider ServiceProvider => this._package;

        private const int _baseIdStart = PackageIds.FolderAddEntityMetadataFileInConnectionCommandId;

        private FolderAddEntityMetadataFileInConnectionCommand(Package package)
        {
            this._package = package ?? throw new ArgumentNullException(nameof(package));

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService != null)
            {
                for (int i = 0; i < ConnectionData.CountConnectionToQuickList; i++)
                {
                    var menuCommandID = new CommandID(PackageGuids.guidDynamicCommandSet, _baseIdStart + i);

                    var menuCommand = new OleMenuCommand(this.menuItemCallback, menuCommandID);

                    menuCommand.Enabled = menuCommand.Visible = false;

                    menuCommand.BeforeQueryStatus += menuItem_BeforeQueryStatus;

                    commandService.AddCommand(menuCommand);
                }
            }
        }

        public static FolderAddEntityMetadataFileInConnectionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FolderAddEntityMetadataFileInConnectionCommand(package);
        }

        private void menuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            try
            {
                if (sender is OleMenuCommand menuCommand)
                {
                    menuCommand.Enabled = menuCommand.Visible = false;

                    var index = menuCommand.CommandID.ID - _baseIdStart;

                    var connectionConfig = ConnectionConfiguration.Get();

                    var connectionsList = connectionConfig.Connections;

                    if (0 <= index && index < connectionsList.Count)
                    {
                        var connectionData = connectionsList[index];

                        menuCommand.Text = connectionData.NameWithCurrentMark;

                        menuCommand.Enabled = menuCommand.Visible = true;

                        CommonHandlers.ActiveSolutionExplorerFolderSingle(this, menuCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        private void menuItemCallback(object sender, EventArgs e)
        {
            try
            {
                OleMenuCommand menuCommand = sender as OleMenuCommand;
                if (menuCommand == null)
                {
                    return;
                }

                var applicationObject = this.ServiceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
                if (applicationObject == null)
                {
                    return;
                }

                var index = menuCommand.CommandID.ID - _baseIdStart;

                var connectionConfig = Model.ConnectionConfiguration.Get();

                var connectionsList = connectionConfig.Connections;

                if (0 <= index && index < connectionsList.Count)
                {
                    var connectionData = connectionsList[index];

                    var helper = DTEHelper.Create(applicationObject);

                    SelectedItem selectedItem = helper.GetSelectedProjectItem();

                    if (selectedItem != null)
                    {
                        helper.HandleExportFileWithEntityMetadata(connectionData, selectedItem);
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }
    }
}
