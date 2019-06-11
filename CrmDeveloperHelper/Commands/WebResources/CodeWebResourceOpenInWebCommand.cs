using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class CodeWebResourceOpenInWebCommand : AbstractCommandByConnectionByGroupWithCurrent
    {
        private readonly ActionOpenComponent _actionOpen;

        private CodeWebResourceOpenInWebCommand(OleMenuCommandService commandService, int baseIdStart, ActionOpenComponent action)
            : base(
                commandService
                , baseIdStart
            )
        {

        }

        public static CodeWebResourceOpenInWebCommand InstanceOpenInWeb { get; private set; }

        public static CodeWebResourceOpenInWebCommand InstanceOpenDependentComponentsInWeb { get; private set; }

        public static CodeWebResourceOpenInWebCommand InstanceOpenDependentComponentsInWindow { get; private set; }

        public static CodeWebResourceOpenInWebCommand InstanceOpenSolutionsContainingComponentInWindow { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWeb = new CodeWebResourceOpenInWebCommand(commandService, PackageIds.CodeWebResourceOpenInWebCommandId, ActionOpenComponent.OpenInWeb);

            InstanceOpenDependentComponentsInWeb = new CodeWebResourceOpenInWebCommand(commandService, PackageIds.CodeWebResourceOpenDependentInWebCommandId, ActionOpenComponent.OpenDependentComponentsInWeb);

            InstanceOpenDependentComponentsInWindow = new CodeWebResourceOpenInWebCommand(commandService, PackageIds.CodeWebResourceOpenDependentInWindowCommandId, ActionOpenComponent.OpenDependentComponentsInWindow);

            InstanceOpenSolutionsContainingComponentInWindow = new CodeWebResourceOpenInWebCommand(commandService, PackageIds.CodeWebResourceOpenSolutionsContainingComponentInWindowCommandId, ActionOpenComponent.OpenSolutionsContainingComponentInWindow);
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