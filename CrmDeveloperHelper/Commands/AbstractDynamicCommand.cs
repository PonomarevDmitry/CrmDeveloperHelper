﻿using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractDynamicCommand<T> : AbstractCommand
    {
        protected readonly int _baseIdStart;
        protected readonly int _commandsCount;

        protected AbstractDynamicCommand(
            OleMenuCommandService commandService
            , Guid commandGroupId
            , int baseIdStart
            , int commandsCount
        )
        {
            this._baseIdStart = baseIdStart;
            this._commandsCount = commandsCount;

            for (int i = 0; i < commandsCount; i++)
            {
                var menuCommandID = new CommandID(commandGroupId, _baseIdStart + i);

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
                ThreadHelper.ThrowIfNotOnUIThread();

                if (sender is OleMenuCommand menuCommand)
                {
                    menuCommand.Enabled = menuCommand.Visible = false;

                    var applicationObject = CrmDeveloperHelperPackage.Singleton?.ApplicationObject;
                    if (applicationObject == null)
                    {
                        return;
                    }

                    var index = menuCommand.CommandID.ID - _baseIdStart;

                    var elementsList = GetElementSourceCollection();

                    if (0 <= index && index < elementsList.Count)
                    {
                        var element = elementsList[index];

                        menuCommand.Text = GetElementName(element);

                        menuCommand.Enabled = menuCommand.Visible = true;

                        CommandBeforeQueryStatus(applicationObject, element, menuCommand);
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

                var index = menuCommand.CommandID.ID - _baseIdStart;

                var elementsList = GetElementSourceCollection();

                if (0 <= index && index < elementsList.Count)
                {
                    var element = elementsList[index];

                    var helper = DTEHelper.Create(applicationObject);

                    CommandAction(helper, element);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }

        protected abstract IList<T> GetElementSourceCollection();

        protected abstract string GetElementName(T element);

        protected abstract void CommandAction(DTEHelper helper, T element);

        protected virtual void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, T element, OleMenuCommand menuCommand)
        {

        }
    }
}
