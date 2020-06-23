using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class CodeReportActionOnComponentInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithCurrent
    {
        private readonly ActionOnComponent _actionOnComponent;

        private CodeReportActionOnComponentInConnectionGroupCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static CodeReportActionOnComponentInConnectionGroupCommand InstanceOpenInWebInConnection { get; private set; }

        public static CodeReportActionOnComponentInConnectionGroupCommand InstanceOpenDependentComponentsInWebInConnection { get; private set; }

        public static CodeReportActionOnComponentInConnectionGroupCommand InstanceOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static CodeReportActionOnComponentInConnectionGroupCommand InstanceOpenSolutionsContainingComponentInExplorerInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWebInConnection = new CodeReportActionOnComponentInConnectionGroupCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeReportOpenInWebInConnectionCommandId
                , ActionOnComponent.OpenInWeb
            );

            InstanceOpenDependentComponentsInWebInConnection = new CodeReportActionOnComponentInConnectionGroupCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeReportOpenDependentInWebInConnectionCommandId
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceOpenDependentComponentsInExplorerInConnection = new CodeReportActionOnComponentInConnectionGroupCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeReportOpenDependentInExplorerInConnectionCommandId
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceOpenSolutionsContainingComponentInExplorerInConnection = new CodeReportActionOnComponentInConnectionGroupCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeReportOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenReportCommand(connectionData, this._actionOnComponent);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentReport(applicationObject, menuCommand);
        }
    }
}