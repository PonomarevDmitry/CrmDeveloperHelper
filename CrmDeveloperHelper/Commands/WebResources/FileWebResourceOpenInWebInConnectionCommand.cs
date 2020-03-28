using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceOpenInWebInConnectionCommand : AbstractDynamicCommandByConnectionByGroupWithCurrent
    {
        private readonly ActionOpenComponent _actionOpen;

        private FileWebResourceOpenInWebInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOpenComponent action)
            : base(commandService, baseIdStart)
        {
            this._actionOpen = action;
        }

        public static FileWebResourceOpenInWebInConnectionCommand InstanceOpenInWebInConnection { get; private set; }

        public static FileWebResourceOpenInWebInConnectionCommand InstanceOpenDependentComponentsInWebInConnection { get; private set; }

        public static FileWebResourceOpenInWebInConnectionCommand InstanceOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static FileWebResourceOpenInWebInConnectionCommand InstanceOpenSolutionsContainingComponentInExplorerInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWebInConnection = new FileWebResourceOpenInWebInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileWebResourceOpenInWebInConnectionCommandId, ActionOpenComponent.OpenInWeb);

            InstanceOpenDependentComponentsInWebInConnection = new FileWebResourceOpenInWebInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileWebResourceOpenDependentInWebInConnectionCommandId, ActionOpenComponent.OpenDependentComponentsInWeb);

            InstanceOpenDependentComponentsInExplorerInConnection = new FileWebResourceOpenInWebInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileWebResourceOpenDependentInExplorerInConnectionCommandId, ActionOpenComponent.OpenDependentComponentsInExplorer);

            InstanceOpenSolutionsContainingComponentInExplorerInConnection = new FileWebResourceOpenInWebInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileWebResourceOpenSolutionsContainingComponentInExplorerInConnectionCommandId, ActionOpenComponent.OpenSolutionsContainingComponentInExplorer);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenWebResource(connectionData, this._actionOpen);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceSingle(applicationObject, menuCommand);
        }
    }
}