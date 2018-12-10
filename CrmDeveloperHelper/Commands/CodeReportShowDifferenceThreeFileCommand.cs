using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.ComponentModel.Design;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    public sealed class CodeReportShowDifferenceThreeFileCommand : IServiceProviderOwner
    {
        private readonly Package _package;
        private readonly string _fieldName;
        private readonly string _fieldTitle;

        public IServiceProvider ServiceProvider => this._package;

        private readonly ShowDifferenceThreeFileType _differenceType;

        private readonly int _baseIdStart;
        private readonly string _formatButtonName;

        private CodeReportShowDifferenceThreeFileCommand(Package package, int baseIdStart, string fieldName, string fieldTitle, ShowDifferenceThreeFileType differenceType, string formatButtonName)
        {
            this._differenceType = differenceType;
            this._baseIdStart = baseIdStart;
            this._formatButtonName = formatButtonName;
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;

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

        public static CodeReportShowDifferenceThreeFileCommand InstanceOneByOneOriginalBodyText { get; private set; }

        public static CodeReportShowDifferenceThreeFileCommand InstanceTwoConnectionsOriginalBodyText { get; private set; }

        public static CodeReportShowDifferenceThreeFileCommand InstanceThreeWayOriginalBodyText { get; private set; }

        public static CodeReportShowDifferenceThreeFileCommand InstanceOneByOneBodyText { get; private set; }

        public static CodeReportShowDifferenceThreeFileCommand InstanceTwoConnectionsBodyText { get; private set; }

        public static CodeReportShowDifferenceThreeFileCommand InstanceThreeWayBodyText { get; private set; }

        public static void Initialize(Package package)
        {
            InstanceOneByOneOriginalBodyText = new CodeReportShowDifferenceThreeFileCommand(
                package
                , PackageIds.CodeReportShowDifferenceOriginalBodyTextOneByOneCommandId
                , Report.Schema.Attributes.originalbodytext
                , "OriginalBodyText"
                , ShowDifferenceThreeFileType.OneByOne
                , "Local File <-> {0}      Local File <-> {1}      {0} <-> {1}"
                );

            InstanceTwoConnectionsOriginalBodyText = new CodeReportShowDifferenceThreeFileCommand(
                package
                , PackageIds.CodeReportShowDifferenceOriginalBodyTextTwoConnectionsCommandId
                , Report.Schema.Attributes.originalbodytext
                , "OriginalBodyText"
                , ShowDifferenceThreeFileType.TwoConnections
                , "{0} <-> {1}"
                );

            InstanceThreeWayOriginalBodyText = new CodeReportShowDifferenceThreeFileCommand(
                package
                , PackageIds.CodeReportShowDifferenceOriginalBodyTextThreeWayCommandId
                , Report.Schema.Attributes.originalbodytext
                , "OriginalBodyText"
                , ShowDifferenceThreeFileType.ThreeWay
                , "{0} <-> Local File <-> {1}"
                );

            InstanceOneByOneBodyText = new CodeReportShowDifferenceThreeFileCommand(
                package
                , PackageIds.CodeReportShowDifferenceBodyTextOneByOneCommandId
                , Report.Schema.Attributes.bodytext
                , "BodyText"
                , ShowDifferenceThreeFileType.OneByOne
                , "Local File <-> {0}      Local File <-> {1}      {0} <-> {1}"
                );

            InstanceTwoConnectionsBodyText = new CodeReportShowDifferenceThreeFileCommand(
                package
                , PackageIds.CodeReportShowDifferenceBodyTextTwoConnectionsCommandId
                , Report.Schema.Attributes.bodytext
                , "BodyText"
                , ShowDifferenceThreeFileType.TwoConnections
                , "{0} <-> {1}"
                );

            InstanceThreeWayBodyText = new CodeReportShowDifferenceThreeFileCommand(
                package
                , PackageIds.CodeReportShowDifferenceBodyTextThreeWayCommandId
                , Report.Schema.Attributes.bodytext
                , "BodyText"
                , ShowDifferenceThreeFileType.ThreeWay
                , "{0} <-> Local File <-> {1}"
                );
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

                        CommonHandlers.ActionBeforeQueryStatusActiveDocumentReport(this, menuCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
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

                    helper.HandleReportThreeFileDifferenceCommand(connectionPair.Item1, connectionPair.Item2, _fieldName, _fieldTitle, _differenceType);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
        }
    }
}