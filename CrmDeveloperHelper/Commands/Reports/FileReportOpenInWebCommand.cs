using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class FileReportOpenInWebCommand : AbstractCommandByConnectionByGroupWithCurrent
    {
        private readonly ActionOpenComponent _actionOpen;

        private FileReportOpenInWebCommand(OleMenuCommandService commandService, int baseIdStart, ActionOpenComponent action)
            : base(
                commandService
                , baseIdStart
            )
        {
            this._actionOpen = action;
        }

        public static FileReportOpenInWebCommand InstanceOpenInWeb { get; private set; }

        public static FileReportOpenInWebCommand InstanceOpenDependentComponentsInWeb { get; private set; }

        public static FileReportOpenInWebCommand InstanceOpenDependentComponentsInWindow { get; private set; }

        public static FileReportOpenInWebCommand InstanceOpenSolutionsContainingComponentInWindow { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWeb = new FileReportOpenInWebCommand(commandService, PackageIds.FileReportOpenInWebCommandId, ActionOpenComponent.OpenInWeb);

            InstanceOpenDependentComponentsInWeb = new FileReportOpenInWebCommand(commandService, PackageIds.FileReportOpenDependentInWebCommandId, ActionOpenComponent.OpenDependentComponentsInWeb);

            InstanceOpenDependentComponentsInWindow = new FileReportOpenInWebCommand(commandService, PackageIds.FileReportOpenDependentInWindowCommandId, ActionOpenComponent.OpenDependentComponentsInWindow);

            InstanceOpenSolutionsContainingComponentInWindow = new FileReportOpenInWebCommand(commandService, PackageIds.FileReportOpenSolutionsContainingComponentInWindowInWindowCommandId, ActionOpenComponent.OpenSolutionsContainingComponentInWindow);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenReportCommand(connectionData, this._actionOpen);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerReportSingle(applicationObject, menuCommand);
        }
    }
}