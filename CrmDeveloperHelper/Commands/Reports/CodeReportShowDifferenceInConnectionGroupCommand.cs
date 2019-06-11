using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class CodeReportShowDifferenceInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithoutCurrent
    {
        private readonly string _fieldName;
        private readonly string _fieldTitle;

        private CodeReportShowDifferenceInConnectionGroupCommand(OleMenuCommandService commandService, int baseIdStart, string fieldName, string fieldTitle)
            : base(
                commandService
                , baseIdStart
            )
        {
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;
        }

        public static CodeReportShowDifferenceInConnectionGroupCommand InstanceOriginalBodyText { get; private set; }

        public static CodeReportShowDifferenceInConnectionGroupCommand InstanceBodyText { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOriginalBodyText = new CodeReportShowDifferenceInConnectionGroupCommand(
                commandService
                , PackageIds.CodeReportShowDifferenceOriginalBodyTextInConnectionGroupCommandId
                , Report.Schema.Attributes.originalbodytext
                , Report.Schema.Headers.originalbodytext
            );

            InstanceBodyText = new CodeReportShowDifferenceInConnectionGroupCommand(
               commandService
               , PackageIds.CodeReportShowDifferenceBodyTextInConnectionGroupCommandId
               , Report.Schema.Attributes.bodytext
               , Report.Schema.Headers.bodytext
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleReportDifferenceCommand(connectionData, this._fieldName, this._fieldTitle, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentReport(applicationObject, menuCommand);
        }
    }
}