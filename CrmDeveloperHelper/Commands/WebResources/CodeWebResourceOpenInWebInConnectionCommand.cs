using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class CodeWebResourceOpenInWebInConnectionCommand : AbstractDynamicCommandByConnectionByGroupWithCurrent
    {
        private readonly ActionOpenComponent _actionOpen;

        private CodeWebResourceOpenInWebInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOpenComponent action)
            : base(commandService, baseIdStart)
        {
            this._actionOpen = action;
        }

        public static CodeWebResourceOpenInWebInConnectionCommand InstanceOpenInWebInConnection { get; private set; }

        public static CodeWebResourceOpenInWebInConnectionCommand InstanceOpenDependentComponentsInWebInConnection { get; private set; }

        public static CodeWebResourceOpenInWebInConnectionCommand InstanceOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static CodeWebResourceOpenInWebInConnectionCommand InstanceOpenSolutionsContainingComponentInExplorerInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWebInConnection = new CodeWebResourceOpenInWebInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.CodeWebResourceOpenInWebInConnectionCommandId, ActionOpenComponent.OpenInWeb);

            InstanceOpenDependentComponentsInWebInConnection = new CodeWebResourceOpenInWebInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.CodeWebResourceOpenDependentInWebInConnectionCommandId, ActionOpenComponent.OpenDependentComponentsInWeb);

            InstanceOpenDependentComponentsInExplorerInConnection = new CodeWebResourceOpenInWebInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.CodeWebResourceOpenDependentInExplorerInConnectionCommandId, ActionOpenComponent.OpenDependentComponentsInExplorer);

            InstanceOpenSolutionsContainingComponentInExplorerInConnection = new CodeWebResourceOpenInWebInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.CodeWebResourceOpenSolutionsContainingComponentInExplorerInConnectionCommandId, ActionOpenComponent.OpenSolutionsContainingComponentInExplorer);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenWebResource(connectionData, this._actionOpen);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResource(applicationObject, menuCommand);
        }
    }
}