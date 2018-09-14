using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.ComponentModel.Design;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeReportShowDifferenceInConnectionGroupCommand : IServiceProviderOwner
    {
        private readonly Package _package;
        private readonly string _fieldName;
        private readonly string _fieldTitle;

        public IServiceProvider ServiceProvider => this._package;

        private readonly int _baseIdStart;

        private CodeReportShowDifferenceInConnectionGroupCommand(Package package, int baseIdStart, string fieldName, string fieldTitle)
        {
            this._package = package ?? throw new ArgumentNullException(nameof(package));
            this._baseIdStart = baseIdStart;
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;

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

        public static CodeReportShowDifferenceInConnectionGroupCommand InstanceOriginalBodyText { get; private set; }

        public static CodeReportShowDifferenceInConnectionGroupCommand InstanceBodyText { get; private set; }

        public static void Initialize(Package package)
        {
            InstanceOriginalBodyText = new CodeReportShowDifferenceInConnectionGroupCommand(
                package
                , PackageIds.CodeReportShowDifferenceOriginalBodyTextInConnectionGroupCommandId
                , Report.Schema.Attributes.originalbodytext
                , "OriginalBodyText"
                );

            InstanceBodyText = new CodeReportShowDifferenceInConnectionGroupCommand(
               package
               , PackageIds.CodeReportShowDifferenceBodyTextInConnectionGroupCommandId
               , Report.Schema.Attributes.bodytext
               , "BodyText"
               );
        }

        private void menuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            try
            {
                if (sender is OleMenuCommand menuCommand)
                {
                    menuCommand.Enabled = menuCommand.Visible = false;

                    var index = menuCommand.CommandID.ID - _baseIdStart;

                    var connectionConfig = Model.ConnectionConfiguration.Get();

                    var list = connectionConfig.GetConnectionsByGroupWithoutCurrent();

                    if (0 <= index && index < list.Count)
                    {
                        var connectionData = list[index];

                        menuCommand.Text = connectionData.NameWithCurrentMark;

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

                var index = menuCommand.CommandID.ID - _baseIdStart;

                var connectionConfig = Model.ConnectionConfiguration.Get();

                var list = connectionConfig.GetConnectionsByGroupWithoutCurrent();

                if (0 <= index && index < list.Count)
                {
                    var connectionData = list[index];

                    var applicationObject = this.ServiceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
                    if (applicationObject != null)
                    {
                        var helper = DTEHelper.Create(applicationObject);
                        helper.HandleReportDifferenceCommand(connectionData, this._fieldName, this._fieldTitle, false);
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
        }
    }
}