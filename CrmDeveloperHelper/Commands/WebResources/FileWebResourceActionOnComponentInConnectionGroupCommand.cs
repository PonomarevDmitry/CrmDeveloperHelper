using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceActionOnComponentInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithCurrent
    {
        private readonly ActionOnComponent _actionOnComponent;

        private FileWebResourceActionOnComponentInConnectionGroupCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static FileWebResourceActionOnComponentInConnectionGroupCommand InstanceOpenInWebInConnection { get; private set; }

        public static FileWebResourceActionOnComponentInConnectionGroupCommand InstanceOpenDependentComponentsInWebInConnection { get; private set; }

        public static FileWebResourceActionOnComponentInConnectionGroupCommand InstanceOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static FileWebResourceActionOnComponentInConnectionGroupCommand InstanceOpenSolutionsContainingComponentInExplorerInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWebInConnection = new FileWebResourceActionOnComponentInConnectionGroupCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileWebResourceOpenInWebInConnectionCommandId
                , ActionOnComponent.OpenInWeb
            );

            InstanceOpenDependentComponentsInWebInConnection = new FileWebResourceActionOnComponentInConnectionGroupCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileWebResourceOpenDependentInWebInConnectionCommandId
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceOpenDependentComponentsInExplorerInConnection = new FileWebResourceActionOnComponentInConnectionGroupCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileWebResourceOpenDependentInExplorerInConnectionCommandId
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceOpenSolutionsContainingComponentInExplorerInConnection = new FileWebResourceActionOnComponentInConnectionGroupCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileWebResourceOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenWebResource(connectionData, this._actionOnComponent);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceAny(applicationObject, menuCommand);
        }
    }
}