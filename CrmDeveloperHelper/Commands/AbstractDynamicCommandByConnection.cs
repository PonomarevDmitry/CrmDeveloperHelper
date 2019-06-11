using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractDynamicCommandByConnection
    {
        protected readonly int _baseIdStart;

        public AbstractDynamicCommandByConnection(
            OleMenuCommandService commandService
            , int baseIdStart
        )
        {
            this._baseIdStart = baseIdStart;

            for (int i = 0; i < ConnectionData.CountConnectionToQuickList; i++)
            {
                var menuCommandID = new CommandID(PackageGuids.guidDynamicCommandSet, _baseIdStart + i);

                var menuCommand = new OleMenuCommand(this.menuItemCallback, menuCommandID);

                menuCommand.Enabled = menuCommand.Visible = false;

                menuCommand.BeforeQueryStatus += menuItem_BeforeQueryStatus;

                commandService.AddCommand(menuCommand);
            }
        }

        private void menuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            try
            {
                if (sender is OleMenuCommand menuCommand)
                {
                    menuCommand.Enabled = menuCommand.Visible = false;

                    var applicationObject = CrmDeveloperHelperPackage.Singleton.ApplicationObject;
                    if (applicationObject == null)
                    {
                        return;
                    }

                    var index = menuCommand.CommandID.ID - _baseIdStart;

                    var connectionConfig = Model.ConnectionConfiguration.Get();

                    var connectionsList = GetConnectionDataSource(connectionConfig);

                    if (0 <= index && index < connectionsList.Count)
                    {
                        var connectionData = connectionsList.ElementAt(index);

                        menuCommand.Text = GetConnectionName(connectionData);

                        menuCommand.Enabled = menuCommand.Visible = true;

                        CommandBeforeQueryStatus(applicationObject, connectionData, menuCommand);
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

                var applicationObject = CrmDeveloperHelperPackage.Singleton.ApplicationObject;
                if (applicationObject == null)
                {
                    return;
                }

                var index = menuCommand.CommandID.ID - _baseIdStart;

                var connectionConfig = ConnectionConfiguration.Get();

                var connectionsList = GetConnectionDataSource(connectionConfig);

                if (0 <= index && index < connectionsList.Count)
                {
                    var connectionData = connectionsList.ElementAt(index);

                    var helper = DTEHelper.Create(applicationObject);

                    CommandAction(helper, connectionData);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected abstract ICollection<ConnectionData> GetConnectionDataSource(ConnectionConfiguration connectionConfig);

        protected abstract string GetConnectionName(ConnectionData connectionData);

        protected virtual void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {

        }

        protected virtual void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {

        }
    }
}
