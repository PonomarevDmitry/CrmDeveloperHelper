﻿using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class CodeReportAddToSolutionLastCommand : IServiceProviderOwner
    {
        private readonly Package _package;

        public IServiceProvider ServiceProvider => this._package;

        private const int _baseIdStart = PackageIds.CodeReportAddToSolutionLastCommandId;

        private CodeReportAddToSolutionLastCommand(Package package)
        {
            this._package = package ?? throw new ArgumentNullException(nameof(package));

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService != null)
            {
                for (int i = 0; i < ConnectionData.CountLastSolutions; i++)
                {
                    var menuCommandID = new CommandID(PackageGuids.guidDynamicCommandSet, _baseIdStart + i);

                    var menuCommand = new OleMenuCommand(this.menuItemCallback, menuCommandID);

                    menuCommand.Enabled = menuCommand.Visible = false;

                    menuCommand.BeforeQueryStatus += menuItem_BeforeQueryStatus;

                    commandService.AddCommand(menuCommand);
                }
            }
        }

        public static CodeReportAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeReportAddToSolutionLastCommand(package);
        }

        private void menuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            try
            {
                if (sender is OleMenuCommand menuCommand)
                {
                    menuCommand.Enabled = menuCommand.Visible = false;

                    var connectionConfig = ConnectionConfiguration.Get();

                    if (connectionConfig.CurrentConnectionData != null)
                    {
                        var connectionData = connectionConfig.CurrentConnectionData;

                        var index = menuCommand.CommandID.ID - _baseIdStart;

                        if (0 <= index && index < connectionData.LastSelectedSolutionsUniqueName.Count)
                        {
                            menuCommand.Text = connectionData.LastSelectedSolutionsUniqueName.ElementAt(index);

                            menuCommand.Enabled = menuCommand.Visible = true;

                            CommonHandlers.ActionBeforeQueryStatusActiveDocumentReport(this, menuCommand);
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

                var applicationObject = this.ServiceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
                if (applicationObject == null)
                {
                    return;
                }

                var index = menuCommand.CommandID.ID - _baseIdStart;

                var connectionConfig = ConnectionConfiguration.Get();

                if (connectionConfig.CurrentConnectionData != null)
                {
                    var connectionData = connectionConfig.CurrentConnectionData;

                    if (0 <= index && index < connectionData.LastSelectedSolutionsUniqueName.Count)
                    {
                        string solutionUniqueName = connectionData.LastSelectedSolutionsUniqueName.ElementAt(index);

                        var helper = DTEHelper.Create(applicationObject);

                        List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsReportType).ToList();

                        helper.HandleAddingReportsToSolutionCommand(null, solutionUniqueName, false, selectedFiles);
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