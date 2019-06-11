using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class CodeReportOpenInWebCommand : AbstractCommandByConnectionByGroupWithCurrent
    {
        private readonly ActionOpenComponent _actionOpen;

        private CodeReportOpenInWebCommand(OleMenuCommandService commandService, int baseIdStart, ActionOpenComponent action)
            : base(
                commandService
                , baseIdStart
            )
        {
            this._actionOpen = action;
        }

        public static CodeReportOpenInWebCommand InstanceOpenInWeb { get; private set; }

        public static CodeReportOpenInWebCommand InstanceOpenDependentComponentsInWeb { get; private set; }

        public static CodeReportOpenInWebCommand InstanceOpenDependentComponentsInWindow { get; private set; }

        public static CodeReportOpenInWebCommand InstanceOpenSolutionsContainingComponentInWindow { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWeb = new CodeReportOpenInWebCommand(commandService, PackageIds.CodeReportOpenInWebCommandId, ActionOpenComponent.OpenInWeb);

            InstanceOpenDependentComponentsInWeb = new CodeReportOpenInWebCommand(commandService, PackageIds.CodeReportOpenDependentInWebCommandId, ActionOpenComponent.OpenDependentComponentsInWeb);

            InstanceOpenDependentComponentsInWindow = new CodeReportOpenInWebCommand(commandService, PackageIds.CodeReportOpenDependentInWindowCommandId, ActionOpenComponent.OpenDependentComponentsInWindow);

            InstanceOpenSolutionsContainingComponentInWindow = new CodeReportOpenInWebCommand(commandService, PackageIds.CodeReportOpenSolutionsContainingComponentInWindowInWindowCommandId, ActionOpenComponent.OpenSolutionsContainingComponentInWindow);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenReportCommand(connectionData, this._actionOpen);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentReport(applicationObject, menuCommand);
        }
    }
}