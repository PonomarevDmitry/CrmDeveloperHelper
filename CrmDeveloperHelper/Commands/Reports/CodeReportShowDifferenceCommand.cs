using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class CodeReportShowDifferenceCommand : AbstractCommand
    {
        private readonly string _fieldName;
        private readonly string _fieldTitle;
        private readonly bool _isCustom;

        private CodeReportShowDifferenceCommand(
            OleMenuCommandService commandService
            , int commandId
            , string fieldName
            , string fieldTitle
            , bool isCustom
        ) : base(
            commandService
            , commandId
        )
        {
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;
            this._isCustom = isCustom;
        }

        public static CodeReportShowDifferenceCommand InstanceOriginalBodyText { get; private set; }

        public static CodeReportShowDifferenceCommand InstanceOriginalBodyTextCustom { get; private set; }

        public static CodeReportShowDifferenceCommand InstanceBodyText { get; private set; }

        public static CodeReportShowDifferenceCommand InstanceBodyTextCustom { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOriginalBodyText = new CodeReportShowDifferenceCommand(
                commandService
                , PackageIds.guidCommandSet.CodeReportShowDifferenceOriginalBodyTextCommandId
                , Report.Schema.Attributes.originalbodytext
                , Report.Schema.Headers.originalbodytext
                , false
            );

            InstanceOriginalBodyTextCustom = new CodeReportShowDifferenceCommand(
                commandService
                , PackageIds.guidCommandSet.CodeReportShowDifferenceOriginalBodyTextCustomCommandId
                , Report.Schema.Attributes.originalbodytext
                , Report.Schema.Headers.originalbodytext
                , true
            );

            InstanceBodyText = new CodeReportShowDifferenceCommand(
                commandService
                , PackageIds.guidCommandSet.CodeReportShowDifferenceBodyTextCommandId
                , Report.Schema.Attributes.bodytext
                , Report.Schema.Headers.bodytext
                , false
            );

            InstanceBodyTextCustom = new CodeReportShowDifferenceCommand(
                commandService
                , PackageIds.guidCommandSet.CodeReportShowDifferenceBodyTextCustomCommandId
                , Report.Schema.Attributes.bodytext
                , Report.Schema.Headers.bodytext
                , true
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleReportDifferenceCommand(null, _fieldName, _fieldTitle, _isCustom);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentReport(applicationObject, menuCommand);

            if (menuCommand.Enabled)
            {
                string custom = this._isCustom ? Properties.CommandNames.CodeReportShowDifferenceCommandCustom : string.Empty;

                string name = string.Format(Properties.CommandNames.CodeReportShowDifferenceCommandFormat2, _fieldTitle, custom);

                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, name);
            }
        }
    }
}