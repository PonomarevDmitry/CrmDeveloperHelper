using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.ComponentModel.Design;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractDynamicCommandAddObjectToSolutionLast
    {
        protected readonly int _baseIdStart;

        protected readonly Action<DTEHelper, ConnectionData, string> _action;
        private readonly Action<EnvDTE80.DTE2, OleMenuCommand> _actionBeforeQueryStatus;

        public AbstractDynamicCommandAddObjectToSolutionLast(
            OleMenuCommandService commandService
            , int baseIdStart
            , Action<DTEHelper, ConnectionData, string> action
            , Action<EnvDTE80.DTE2, OleMenuCommand> actionBeforeQueryStatus
        )
        {
            this._baseIdStart = baseIdStart;

            this._action = action ?? throw new ArgumentNullException(nameof(action));
            this._actionBeforeQueryStatus = actionBeforeQueryStatus ?? throw new ArgumentNullException(nameof(actionBeforeQueryStatus));

            for (int i = 0; i < ConnectionData.CountLastSolutions; i++)
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

                    var connectionConfig = ConnectionConfiguration.Get();

                    if (connectionConfig.CurrentConnectionData != null)
                    {
                        var index = menuCommand.CommandID.ID - _baseIdStart;

                        if (0 <= index && index < connectionConfig.CurrentConnectionData.LastSelectedSolutionsUniqueName.Count)
                        {
                            menuCommand.Text = connectionConfig.CurrentConnectionData.LastSelectedSolutionsUniqueName.ElementAt(index);

                            menuCommand.Enabled = menuCommand.Visible = true;

                            this._actionBeforeQueryStatus?.Invoke(applicationObject, menuCommand);
                        }
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

                var connectionConfig = ConnectionConfiguration.Get();

                if (connectionConfig.CurrentConnectionData != null)
                {
                    var index = menuCommand.CommandID.ID - _baseIdStart;

                    if (0 <= index && index < connectionConfig.CurrentConnectionData.LastSelectedSolutionsUniqueName.Count)
                    {
                        string solutionUniqueName = connectionConfig.CurrentConnectionData.LastSelectedSolutionsUniqueName.ElementAt(index);

                        var helper = DTEHelper.Create(applicationObject);

                        this._action.Invoke(helper, connectionConfig.CurrentConnectionData, solutionUniqueName);
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
