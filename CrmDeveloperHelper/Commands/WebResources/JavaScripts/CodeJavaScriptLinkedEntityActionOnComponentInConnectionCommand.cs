using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class CodeJavaScriptLinkedEntityActionOnComponentInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ActionOnComponent _actionOnComponent;

        private CodeJavaScriptLinkedEntityActionOnComponentInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static CodeJavaScriptLinkedEntityActionOnComponentInConnectionCommand InstanceOpenInWebInConnection { get; private set; }

        public static CodeJavaScriptLinkedEntityActionOnComponentInConnectionCommand InstanceOpenListInWebInConnection { get; private set; }

        public static CodeJavaScriptLinkedEntityActionOnComponentInConnectionCommand InstanceOpenDependentComponentsInWebInConnection { get; private set; }

        public static CodeJavaScriptLinkedEntityActionOnComponentInConnectionCommand InstanceOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static CodeJavaScriptLinkedEntityActionOnComponentInConnectionCommand InstanceOpenSolutionsListWithComponentInExplorerInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWebInConnection = new CodeJavaScriptLinkedEntityActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityOpenInWebInConnectionCommandId
                , ActionOnComponent.OpenInWeb
            );

            InstanceOpenListInWebInConnection = new CodeJavaScriptLinkedEntityActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityOpenListInWebInConnectionCommandId
                , ActionOnComponent.OpenListInWeb
            );

            InstanceOpenDependentComponentsInWebInConnection = new CodeJavaScriptLinkedEntityActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityOpenDependentInWebInConnectionCommandId
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceOpenDependentComponentsInExplorerInConnection = new CodeJavaScriptLinkedEntityActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityOpenDependentInExplorerInConnectionCommandId
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceOpenSolutionsListWithComponentInExplorerInConnection = new CodeJavaScriptLinkedEntityActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (helper.TryGetLinkedEntityName(out string entityName))
            {
                helper.OpenEntityMetadataCommand(connectionData, entityName, this._actionOnComponent);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedEntityName(applicationObject, menuCommand);
        }
    }
}