using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.ComponentModel.Design;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractOutputWindowCommand
    {
        protected AbstractOutputWindowCommand(OleMenuCommandService commandService, int idCommand)
        {
            var menuCommandID = new CommandID(PackageGuids.guidCommandSet, idCommand);

            var menuItem = new OleMenuCommand(this.menuItemCallback, menuCommandID);

            menuItem.BeforeQueryStatus += menuItem_BeforeQueryStatus;

            commandService.AddCommand(menuItem);
        }

        protected void menuItem_BeforeQueryStatus(object sender, EventArgs e)
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

                    var helper = DTEHelper.Create(applicationObject);

                    var connectionData = helper.GetOutputWindowConnection();

                    if (connectionData == null)
                    {
                        return;
                    }

                    menuCommand.Enabled = menuCommand.Visible = true;

                    CommandBeforeQueryStatus(applicationObject, connectionData, menuCommand);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected void menuItemCallback(object sender, EventArgs e)
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

                var helper = DTEHelper.Create(applicationObject);

                var connectionData = helper.GetOutputWindowConnection();

                if (connectionData == null)
                {
                    return;
                }

                CommandAction(helper, connectionData);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected abstract void CommandAction(DTEHelper helper, ConnectionData connectionData);

        protected virtual void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {

        }
    }
}
