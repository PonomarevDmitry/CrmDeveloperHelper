using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class ListForPublishPerformPublishGroupConnectionCommand : IServiceProviderOwner
    {
        private readonly Package _package;

        public IServiceProvider ServiceProvider => this._package;

        private const int _baseIdStart = PackageIds.ListForPublishPerformPublishGroupConnectionCommandId;

        private ListForPublishPerformPublishGroupConnectionCommand(Package package)
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

        public static ListForPublishPerformPublishGroupConnectionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ListForPublishPerformPublishGroupConnectionCommand(package);
        }

        private void menuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            try
            {
                if (sender is OleMenuCommand menuCommand)
                {
                    menuCommand.Enabled = menuCommand.Visible = false;

                    var index = menuCommand.CommandID.ID - _baseIdStart;

                    var connectionConfig = Model.ConnectionConfiguration.Get();

                    var list = connectionConfig.GetConnectionsByGroupWithCurrent();

                    if (0 <= index && index < list.Count)
                    {
                        var connectionData = list[index];

                        menuCommand.Text = connectionData.NameWithCurrentMark;

                        if (!connectionData.IsReadOnly)
                        {
                            menuCommand.Enabled = menuCommand.Visible = true;

                            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceAny(this, menuCommand);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
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

                var list = connectionConfig.GetConnectionsByGroupWithCurrent();

                if (0 <= index && index < list.Count)
                {
                    var connectionData = list[index];

                    if (!connectionData.IsReadOnly)
                    {
                        var helper = DTEHelper.Create(applicationObject);

                        List<SelectedFile> selectedFiles = helper.GetSelectedFilesFromListForPublish();

                        if (selectedFiles.Count > 0)
                        {
                            helper.ShowListForPublish();

                            helper.HandleUpdateContentWebResourcesAndPublishCommand(connectionData, selectedFiles);
                        }
                        else
                        {
                            helper.WriteToOutput(Properties.OutputStrings.PublishListIsEmpty);
                            helper.ActivateOutputWindow();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
        }
    }
}