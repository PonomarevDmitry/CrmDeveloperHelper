using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceOpenInWebCommand : AbstractDynamicCommandByConnectionByGroupWithCurrent
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

        public static FileWebResourceOpenInWebCommand InstanceOpenDependentComponentsInExplorer { get; private set; }

        public static FileWebResourceOpenInWebCommand InstanceOpenSolutionsContainingComponentInExplorer { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWeb = new FileWebResourceOpenInWebCommand(commandService, PackageIds.FileWebResourceOpenInWebCommandId, ActionOpenComponent.OpenInWeb);

            InstanceOpenDependentComponentsInWeb = new FileWebResourceOpenInWebCommand(commandService, PackageIds.FileWebResourceOpenDependentInWebCommandId, ActionOpenComponent.OpenDependentComponentsInWeb);

            InstanceOpenDependentComponentsInExplorer = new FileWebResourceOpenInWebCommand(commandService, PackageIds.FileWebResourceOpenDependentInExplorerCommandId, ActionOpenComponent.OpenDependentComponentsInExplorer);

            InstanceOpenSolutionsContainingComponentInExplorer = new FileWebResourceOpenInWebCommand(commandService, PackageIds.FileWebResourceOpenSolutionsContainingComponentInExplorerCommandId, ActionOpenComponent.OpenSolutionsContainingComponentInExplorer);
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