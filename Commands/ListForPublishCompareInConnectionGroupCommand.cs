using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class ListForPublishCompareInConnectionGroupCommand : IServiceProviderOwner
    {
        private readonly Package _package;

        public IServiceProvider ServiceProvider => this._package;

        private readonly bool _withDetails;

        private readonly int _baseIdStart;

        private ListForPublishCompareInConnectionGroupCommand(Package package, int baseIdStart, bool withDetails)
        {
            this._withDetails = withDetails;
            this._baseIdStart = baseIdStart;

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

        public static ListForPublishCompareInConnectionGroupCommand Instance { get; private set; }

        public static ListForPublishCompareInConnectionGroupCommand InstanceWithDetails { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ListForPublishCompareInConnectionGroupCommand(package, PackageIds.ListForPublishCompareInConnectionGroupCommandId, false);

            InstanceWithDetails = new ListForPublishCompareInConnectionGroupCommand(package, PackageIds.ListForPublishCompareWithDetailsInConnectionGroupCommandId, true);
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

                    var list = connectionConfig.GetConnectionsByGroupWithoutCurrent();

                    if (0 <= index && index < list.Count)
                    {
                        var connectionData = list[index];

                        menuCommand.Text = connectionData.NameWithCurrentMark;

                        menuCommand.Enabled = menuCommand.Visible = true;

                        CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(this, menuCommand);

                        CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceAny(this, menuCommand);
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

                var connectionConfig = ConnectionConfiguration.Get();

                var list = connectionConfig.GetConnectionsByGroupWithoutCurrent();

                if (0 <= index && index < list.Count)
                {
                    var connectionData = list[index];

                    var helper = DTEHelper.Create(applicationObject);

                    helper.HandleFileCompareListForPublishCommand(connectionData, this._withDetails);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
        }
    }
}