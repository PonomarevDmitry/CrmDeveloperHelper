using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class FileReportOpenInWebCommand : AbstractDynamicCommandByConnectionByGroupWithCurrent
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

        public static FileReportOpenInWebCommand InstanceOpenDependentComponentsInExplorer { get; private set; }

        public static FileReportOpenInWebCommand InstanceOpenSolutionsContainingComponentInExplorer { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWeb = new FileReportOpenInWebCommand(commandService, PackageIds.guidDynamicCommandSet.FileReportOpenInWebCommandId, ActionOpenComponent.OpenInWeb);

            InstanceOpenDependentComponentsInWeb = new FileReportOpenInWebCommand(commandService, PackageIds.guidDynamicCommandSet.FileReportOpenDependentInWebCommandId, ActionOpenComponent.OpenDependentComponentsInWeb);

            InstanceOpenDependentComponentsInExplorer = new FileReportOpenInWebCommand(commandService, PackageIds.guidDynamicCommandSet.FileReportOpenDependentInExplorerCommandId, ActionOpenComponent.OpenDependentComponentsInExplorer);

            InstanceOpenSolutionsContainingComponentInExplorer = new FileReportOpenInWebCommand(commandService, PackageIds.guidDynamicCommandSet.FileReportOpenSolutionsContainingComponentInExplorerCommandId, ActionOpenComponent.OpenSolutionsContainingComponentInExplorer);
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