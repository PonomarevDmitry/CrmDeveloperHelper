using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.ComponentModel.Design;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractCommand
    {
        protected AbstractCommand(OleMenuCommandService commandService, int idCommand)
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
                ThreadHelper.ThrowIfNotOnUIThread();

                if (sender is OleMenuCommand menuCommand)
                {
                    menuCommand.Enabled = menuCommand.Visible = false;

                    var applicationObject = CrmDeveloperHelperPackage.Singleton?.ApplicationObject;
                    if (applicationObject == null)
                    {
                        return;
                    }

                    menuCommand.Enabled = menuCommand.Visible = true;

                    CommandBeforeQueryStatus(applicationObject, menuCommand);
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
                ThreadHelper.ThrowIfNotOnUIThread();

                OleMenuCommand menuCommand = sender as OleMenuCommand;
                if (menuCommand == null)
                {
                    return;
                }

                var applicationObject = CrmDeveloperHelperPackage.Singleton?.ApplicationObject;
                if (applicationObject == null)
                {
                    return;
                }

                var helper = DTEHelper.Create(applicationObject);

                CommandAction(helper);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected abstract void CommandAction(DTEHelper helper);

        protected virtual void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {

        }
    }
}
