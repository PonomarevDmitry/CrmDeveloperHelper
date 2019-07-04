using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class CodeWebResourceOpenInWebCommand : AbstractDynamicCommandByConnectionByGroupWithCurrent
    {
        private readonly ActionOpenComponent _actionOpen;

        private CodeWebResourceOpenInWebCommand(OleMenuCommandService commandService, int baseIdStart, ActionOpenComponent action)
            : base(
                commandService
                , baseIdStart
            )
        {
            this._actionOpen = action;
        }

        public static CodeWebResourceOpenInWebCommand InstanceOpenInWeb { get; private set; }

        public static CodeWebResourceOpenInWebCommand InstanceOpenDependentComponentsInWeb { get; private set; }

        public static CodeWebResourceOpenInWebCommand InstanceOpenDependentComponentsInExplorer { get; private set; }

        public static CodeWebResourceOpenInWebCommand InstanceOpenSolutionsContainingComponentInExplorer { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWeb = new CodeWebResourceOpenInWebCommand(commandService, PackageIds.CodeWebResourceOpenInWebCommandId, ActionOpenComponent.OpenInWeb);

            InstanceOpenDependentComponentsInWeb = new CodeWebResourceOpenInWebCommand(commandService, PackageIds.CodeWebResourceOpenDependentInWebCommandId, ActionOpenComponent.OpenDependentComponentsInWeb);

            InstanceOpenDependentComponentsInExplorer = new CodeWebResourceOpenInWebCommand(commandService, PackageIds.CodeWebResourceOpenDependentInExplorerCommandId, ActionOpenComponent.OpenDependentComponentsInExplorer);

            InstanceOpenSolutionsContainingComponentInExplorer = new CodeWebResourceOpenInWebCommand(commandService, PackageIds.CodeWebResourceOpenSolutionsContainingComponentInExplorerCommandId, ActionOpenComponent.OpenSolutionsContainingComponentInExplorer);
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