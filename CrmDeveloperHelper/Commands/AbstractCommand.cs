using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.ComponentModel.Design;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractCommand : IServiceProviderOwner
    {
        protected readonly Package _package;
        protected readonly Guid _guidCommandset;
        protected readonly int _idCommand;
        protected readonly Action<DTEHelper> _action;
        private readonly Action<IServiceProviderOwner, OleMenuCommand> _actionBeforeQueryStatus;

        public IServiceProvider ServiceProvider => this._package;

        protected AbstractCommand(Package package, Guid guidCommandset, int idCommand, Action<DTEHelper> action, Action<IServiceProviderOwner, OleMenuCommand> actionBeforeQueryStatus)
        {
            this._package = package ?? throw new ArgumentNullException(nameof(package));
            this._action = action ?? throw new ArgumentNullException(nameof(action));
            this._actionBeforeQueryStatus = actionBeforeQueryStatus;
            this._guidCommandset = guidCommandset;
            this._idCommand = idCommand;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(this._guidCommandset, this._idCommand);
                var menuItem = new OleMenuCommand(this.menuItemCallback, menuCommandID);
                if (actionBeforeQueryStatus != null)
                {
                    menuItem.BeforeQueryStatus += menuItem_BeforeQueryStatus;
                }

                commandService.AddCommand(menuItem);
            }
        }

        protected void menuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            try
            {
                if (sender is OleMenuCommand menuCommand)
                {
                    menuCommand.Enabled = menuCommand.Visible = true;

                    _actionBeforeQueryStatus?.Invoke(this, menuCommand);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
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

                var applicationObject = this.ServiceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
                if (applicationObject == null)
                {
                    return;
                }

                var helper = DTEHelper.Create(applicationObject);

                _action?.Invoke(helper);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
           
        }
    }
}
