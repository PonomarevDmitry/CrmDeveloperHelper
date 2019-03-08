﻿using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class DocumentsWebResourceCompareInConnectionGroupCommand : IServiceProviderOwner
    {
        private readonly Package _package;

        public IServiceProvider ServiceProvider => this._package;

        private readonly bool _withDetails;

        private readonly int _baseIdStart;

        private DocumentsWebResourceCompareInConnectionGroupCommand(Package package, int baseIdStart, bool withDetails)
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

        public static DocumentsWebResourceCompareInConnectionGroupCommand Instance { get; private set; }

        public static DocumentsWebResourceCompareInConnectionGroupCommand InstanceWithDetails { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new DocumentsWebResourceCompareInConnectionGroupCommand(package, PackageIds.DocumentsWebResourceCompareInConnectionGroupCommandId, false);

            InstanceWithDetails = new DocumentsWebResourceCompareInConnectionGroupCommand(package, PackageIds.DocumentsWebResourceCompareWithDetailsInConnectionGroupCommandId, true);
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

                    var list = connectionConfig.GetConnectionsByGroupWithCurrent();

                    if (0 <= index && index < list.Count)
                    {
                        var connectionData = list[index];

                        menuCommand.Text = connectionData.NameWithCurrentMark;

                        menuCommand.Enabled = menuCommand.Visible = true;

                        CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResourceText(this, menuCommand);
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

                var connectionConfig = ConnectionConfiguration.Get();

                var list = connectionConfig.GetConnectionsByGroupWithCurrent();

                if (0 <= index && index < list.Count)
                {
                    var connectionData = list[index];

                    var helper = DTEHelper.Create(applicationObject);

                    List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsWebResourceTextType);

                    helper.HandleFileCompareCommand(connectionData, selectedFiles, this._withDetails);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }
    }
}
