﻿using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.ComponentModel.Design;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    public sealed class FileWebResourceShowDifferenceThreeFileCommand : IServiceProviderOwner
    {
        private readonly Package _package;

        public IServiceProvider ServiceProvider => this._package;

        private readonly ShowDifferenceThreeFileType _differenceType;

        private readonly int _baseIdStart;
        private readonly string _formatButtonName;

        private FileWebResourceShowDifferenceThreeFileCommand(Package package, int baseIdStart, ShowDifferenceThreeFileType differenceType, string formatButtonName)
        {
            this._differenceType = differenceType;
            this._baseIdStart = baseIdStart;
            this._formatButtonName = formatButtonName;

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

        public static FileWebResourceShowDifferenceThreeFileCommand InstanceOneByOne { get; private set; }

        public static FileWebResourceShowDifferenceThreeFileCommand InstanceTwoConnections { get; private set; }

        public static FileWebResourceShowDifferenceThreeFileCommand InstanceThreeWay { get; private set; }

        public static void Initialize(Package package)
        {
            InstanceOneByOne = new FileWebResourceShowDifferenceThreeFileCommand(package, PackageIds.FileWebResourceShowDifferenceOneByOneCommandId, ShowDifferenceThreeFileType.OneByOne, "Local File <-> {0}      Local File <-> {1}      {0} <-> {1}");

            InstanceTwoConnections = new FileWebResourceShowDifferenceThreeFileCommand(package, PackageIds.FileWebResourceShowDifferenceTwoConnectionsCommandId, ShowDifferenceThreeFileType.TwoConnections, "{0} <-> {1}");

            InstanceThreeWay = new FileWebResourceShowDifferenceThreeFileCommand(package, PackageIds.FileWebResourceShowDifferenceThreeWayCommandId, ShowDifferenceThreeFileType.ThreeWay, "{0} <-> Local File <-> {1}");
        }

        private void menuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            try
            {
                if (sender is OleMenuCommand menuCommand)
                {
                    menuCommand.Enabled = menuCommand.Visible = false;

                    if (this._differenceType == ShowDifferenceThreeFileType.ThreeWay)
                    {
                        var commonConfig = Model.CommonConfiguration.Get();

                        if (!commonConfig.DifferenceThreeWayAvaliable())
                        {
                            return;
                        }
                    }

                    var index = menuCommand.CommandID.ID - _baseIdStart;

                    var connectionConfig = Model.ConnectionConfiguration.Get();

                    var list = connectionConfig.GetConnectionPairsByGroup();

                    if (0 <= index && index < list.Count)
                    {
                        var connectionDataPair = list[index];

                        menuCommand.Text = string.Format(_formatButtonName, connectionDataPair.Item1.Name, connectionDataPair.Item2.Name);

                        menuCommand.Enabled = menuCommand.Visible = true;

                        CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextSingle(this, menuCommand);
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

                if (this._differenceType == ShowDifferenceThreeFileType.ThreeWay)
                {
                    var commonConfig = Model.CommonConfiguration.Get();

                    if (!commonConfig.DifferenceThreeWayAvaliable())
                    {
                        return;
                    }
                }

                var index = menuCommand.CommandID.ID - _baseIdStart;

                var connectionConfig = Model.ConnectionConfiguration.Get();

                var list = connectionConfig.GetConnectionPairsByGroup();

                if (0 <= index && index < list.Count)
                {
                    var connectionPair = list[index];

                    var helper = DTEHelper.Create(applicationObject);

                    helper.HandleWebResourceThreeFileDifferenceCommand(connectionPair.Item1, connectionPair.Item2, _differenceType);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }
    }
}