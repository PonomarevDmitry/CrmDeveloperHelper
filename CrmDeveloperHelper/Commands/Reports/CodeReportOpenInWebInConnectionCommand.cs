using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class CodeReportOpenInWebInConnectionCommand : AbstractDynamicCommandByConnectionByGroupWithCurrent
    {
        private readonly ActionOpenComponent _actionOpen;

        private CodeReportOpenInWebInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOpenComponent action)
            : base(commandService, baseIdStart)
        {
            this._actionOpen = action;
        }

        public static CodeReportOpenInWebInConnectionCommand InstanceOpenInWebInConnection { get; private set; }

        public static CodeReportOpenInWebInConnectionCommand InstanceOpenDependentComponentsInWebInConnection { get; private set; }

        public static CodeReportOpenInWebInConnectionCommand InstanceOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static CodeReportOpenInWebInConnectionCommand InstanceOpenSolutionsContainingComponentInExplorerInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWebInConnection = new CodeReportOpenInWebInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.CodeReportOpenInWebInConnectionCommandId, ActionOpenComponent.OpenInWeb);

            InstanceOpenDependentComponentsInWebInConnection = new CodeReportOpenInWebInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.CodeReportOpenDependentInWebInConnectionCommandId, ActionOpenComponent.OpenDependentComponentsInWeb);

            InstanceOpenDependentComponentsInExplorerInConnection = new CodeReportOpenInWebInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.CodeReportOpenDependentInExplorerInConnectionCommandId, ActionOpenComponent.OpenDependentComponentsInExplorer);

            InstanceOpenSolutionsContainingComponentInExplorerInConnection = new CodeReportOpenInWebInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.CodeReportOpenSolutionsContainingComponentInExplorerInConnectionCommandId, ActionOpenComponent.OpenSolutionsContainingComponentInExplorer);
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