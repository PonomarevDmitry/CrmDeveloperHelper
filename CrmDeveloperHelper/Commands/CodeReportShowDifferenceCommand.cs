using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.ComponentModel.Design;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeReportShowDifferenceCommand : IServiceProviderOwner
    {
        private readonly Package _package;
        private readonly string _fieldName;
        private readonly string _fieldTitle;
        private readonly bool _isCustom;

        public IServiceProvider ServiceProvider => this._package;

        private CodeReportShowDifferenceCommand(Package package, int commandId, string fieldName, string fieldTitle, bool isCustom)
        {
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;
            this._isCustom = isCustom;

            this._package = package ?? throw new ArgumentNullException(nameof(package));

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService != null)
            {
                var menuCommandID = new CommandID(PackageGuids.guidCommandSet, commandId);

                var menuCommand = new OleMenuCommand(this.menuItemCallback, menuCommandID);

                menuCommand.Enabled = menuCommand.Visible = false;

                menuCommand.BeforeQueryStatus += menuItem_BeforeQueryStatus;

                commandService.AddCommand(menuCommand);
            }
        }

        public static CodeReportShowDifferenceCommand InstanceOriginalBodyText { get; private set; }

        public static CodeReportShowDifferenceCommand InstanceOriginalBodyTextCustom { get; private set; }

        public static CodeReportShowDifferenceCommand InstanceBodyText { get; private set; }

        public static CodeReportShowDifferenceCommand InstanceBodyTextCustom { get; private set; }

        public static void Initialize(Package package)
        {
            InstanceOriginalBodyText = new CodeReportShowDifferenceCommand(
                package
                , PackageIds.CodeReportShowDifferenceOriginalBodyTextCommandId
                , Report.Schema.Attributes.originalbodytext
                , "OriginalBodyText"
                , false
                );

            InstanceOriginalBodyTextCustom = new CodeReportShowDifferenceCommand(
                package
                , PackageIds.CodeReportShowDifferenceOriginalBodyTextCustomCommandId
                , Report.Schema.Attributes.originalbodytext
                , "OriginalBodyText"
                , true
                );

            InstanceBodyText = new CodeReportShowDifferenceCommand(
                package
                , PackageIds.CodeReportShowDifferenceBodyTextCommandId
                , Report.Schema.Attributes.bodytext
                , "BodyText"
                , false
                );

            InstanceBodyTextCustom = new CodeReportShowDifferenceCommand(
                package
                , PackageIds.CodeReportShowDifferenceBodyTextCustomCommandId
                , Report.Schema.Attributes.bodytext
                , "BodyText"
                , true
                );
        }

        private void menuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            try
            {
                if (sender is OleMenuCommand menuCommand)
                {
                    menuCommand.Enabled = menuCommand.Visible = true;

                    string custom = this._isCustom ? " " + Properties.CommandNames.CodeReportShowDifferenceCommandCustom : string.Empty;

                    string name = string.Format(Properties.CommandNames.CodeReportShowDifferenceCommandFormat2, _fieldTitle, custom);
                    
                    CommonHandlers.ActionBeforeQueryStatusActiveDocumentReport(this, menuCommand);

                    CommonHandlers.CorrectCommandNameForConnectionName(this, menuCommand, name);
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

                var helper = DTEHelper.Create(applicationObject);

                helper.HandleReportDifferenceCommand(null, _fieldName, _fieldTitle, _isCustom);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
        }
    }
}