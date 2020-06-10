using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class FileReportActionOnComponentInConnectionCommand : AbstractDynamicCommandByConnectionByGroupWithCurrent
    {
        private readonly ActionOnComponent _actionOnComponent;

        private FileReportActionOnComponentInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static FileReportActionOnComponentInConnectionCommand InstanceOpenInWebInConnection { get; private set; }

        public static FileReportActionOnComponentInConnectionCommand InstanceOpenDependentComponentsInWebInConnection { get; private set; }

        public static FileReportActionOnComponentInConnectionCommand InstanceOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static FileReportActionOnComponentInConnectionCommand InstanceOpenSolutionsContainingComponentInExplorerInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWebInConnection = new FileReportActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileReportOpenInWebInConnectionCommandId
                , ActionOnComponent.OpenInWeb
            );

            InstanceOpenDependentComponentsInWebInConnection = new FileReportActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileReportOpenDependentInWebInConnectionCommandId
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceOpenDependentComponentsInExplorerInConnection = new FileReportActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileReportOpenDependentInExplorerInConnectionCommandId
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceOpenSolutionsContainingComponentInExplorerInConnection = new FileReportActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileReportOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenReportCommand(connectionData, this._actionOnComponent);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerReportSingle(applicationObject, menuCommand);
        }
    }
}