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
    internal abstract class AbstractCommandByConnection
    {
        protected readonly int _baseIdStart;

        private readonly Func<ConnectionConfiguration, ICollection<ConnectionData>> _connectionDataSource;
        private readonly Func<ConnectionData, string> _connectionDataName;

        public AbstractCommandByConnection(
            OleMenuCommandService commandService
            , int baseIdStart
            , Func<ConnectionConfiguration, ICollection<ConnectionData>> connectionDataSource
            , Func<ConnectionData, string> connectionDataName
        )
        {
            this._baseIdStart = baseIdStart;

            this._connectionDataSource = connectionDataSource ?? throw new ArgumentNullException(nameof(connectionDataSource));
            this._connectionDataName = connectionDataName ?? throw new ArgumentNullException(nameof(connectionDataName));

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

                    var connectionsList = _connectionDataSource(connectionConfig);

                    if (0 <= index && index < connectionsList.Count)
                    {
                        var connectionData = connectionsList.ElementAt(index);

                        menuCommand.Text = _connectionDataName(connectionData);

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

                var connectionsList = _connectionDataSource(connectionConfig);

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

        protected virtual void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {

        }

        protected virtual void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {

        }
    }
}
