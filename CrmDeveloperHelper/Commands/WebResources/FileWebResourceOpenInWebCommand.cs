using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceOpenInWebCommand : AbstractCommandByConnectionByGroupWithCurrent
    {
        private readonly ActionOpenComponent _actionOpen;

        private FileWebResourceOpenInWebCommand(OleMenuCommandService commandService, int baseIdStart, ActionOpenComponent action)
            : base(
                commandService
                , baseIdStart
            )
        {
            this._actionOpen = action;
        }

        public static FileWebResourceOpenInWebCommand InstanceOpenInWeb { get; private set; }

        public static FileWebResourceOpenInWebCommand InstanceOpenDependentComponentsInWeb { get; private set; }

        public static FileWebResourceOpenInWebCommand InstanceOpenDependentComponentsInWindow { get; private set; }

        public static FileWebResourceOpenInWebCommand InstanceOpenSolutionsContainingComponentInWindow { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWeb = new FileWebResourceOpenInWebCommand(commandService, PackageIds.FileWebResourceOpenInWebCommandId, ActionOpenComponent.OpenInWeb);

            InstanceOpenDependentComponentsInWeb = new FileWebResourceOpenInWebCommand(commandService, PackageIds.FileWebResourceOpenDependentInWebCommandId, ActionOpenComponent.OpenDependentComponentsInWeb);

            InstanceOpenDependentComponentsInWindow = new FileWebResourceOpenInWebCommand(commandService, PackageIds.FileWebResourceOpenDependentInWindowCommandId, ActionOpenComponent.OpenDependentComponentsInWindow);

            InstanceOpenSolutionsContainingComponentInWindow = new FileWebResourceOpenInWebCommand(commandService, PackageIds.FileWebResourceOpenSolutionsContainingComponentInWindowInWindowCommandId, ActionOpenComponent.OpenSolutionsContainingComponentInWindow);
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