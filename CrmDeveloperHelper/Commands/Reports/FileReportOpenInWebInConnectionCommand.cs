using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class FileReportOpenInWebInConnectionCommand : AbstractDynamicCommandByConnectionByGroupWithCurrent
    {
        private readonly ActionOpenComponent _actionOpen;

        private FileReportOpenInWebInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOpenComponent action)
            : base(commandService, baseIdStart)
        {
            this._actionOpen = action;
        }

        public static FileReportOpenInWebInConnectionCommand InstanceOpenInWebInConnection { get; private set; }

        public static FileReportOpenInWebInConnectionCommand InstanceOpenDependentComponentsInWebInConnection { get; private set; }

        public static FileReportOpenInWebInConnectionCommand InstanceOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static FileReportOpenInWebInConnectionCommand InstanceOpenSolutionsContainingComponentInExplorerInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWebInConnection = new FileReportOpenInWebInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileReportOpenInWebInConnectionCommandId, ActionOpenComponent.OpenInWeb);

            InstanceOpenDependentComponentsInWebInConnection = new FileReportOpenInWebInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileReportOpenDependentInWebInConnectionCommandId, ActionOpenComponent.OpenDependentComponentsInWeb);

            InstanceOpenDependentComponentsInExplorerInConnection = new FileReportOpenInWebInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileReportOpenDependentInExplorerInConnectionCommandId, ActionOpenComponent.OpenDependentComponentsInExplorer);

            InstanceOpenSolutionsContainingComponentInExplorerInConnection = new FileReportOpenInWebInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileReportOpenSolutionsContainingComponentInExplorerInConnectionCommandId, ActionOpenComponent.OpenSolutionsContainingComponentInExplorer);
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