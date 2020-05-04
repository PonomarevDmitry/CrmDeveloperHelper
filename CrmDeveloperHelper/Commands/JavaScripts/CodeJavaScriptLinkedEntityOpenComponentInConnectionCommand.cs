using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeJavaScriptLinkedEntityOpenComponentInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ActionOpenComponent _actionOpen;

        private CodeJavaScriptLinkedEntityOpenComponentInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOpenComponent action)
            : base(commandService, baseIdStart)
        {
            this._actionOpen = action;
        }

        public static CodeJavaScriptLinkedEntityOpenComponentInConnectionCommand InstanceOpenInWebInConnection { get; private set; }

        public static CodeJavaScriptLinkedEntityOpenComponentInConnectionCommand InstanceOpenListInWebInConnection { get; private set; }

        public static CodeJavaScriptLinkedEntityOpenComponentInConnectionCommand InstanceOpenDependentComponentsInWebInConnection { get; private set; }

        public static CodeJavaScriptLinkedEntityOpenComponentInConnectionCommand InstanceOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static CodeJavaScriptLinkedEntityOpenComponentInConnectionCommand InstanceOpenSolutionsContainingComponentInExplorerInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWebInConnection = new CodeJavaScriptLinkedEntityOpenComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityOpenInWebInConnectionCommandId
                , ActionOpenComponent.OpenInWeb
            );
            
            InstanceOpenListInWebInConnection = new CodeJavaScriptLinkedEntityOpenComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityOpenListInWebInConnectionCommandId
                , ActionOpenComponent.OpenListInWeb
            );

            InstanceOpenDependentComponentsInWebInConnection = new CodeJavaScriptLinkedEntityOpenComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityOpenDependentInWebInConnectionCommandId
                , ActionOpenComponent.OpenDependentComponentsInWeb
            );

            InstanceOpenDependentComponentsInExplorerInConnection = new CodeJavaScriptLinkedEntityOpenComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityOpenDependentInExplorerInConnectionCommandId
                , ActionOpenComponent.OpenDependentComponentsInExplorer
            );

            InstanceOpenSolutionsContainingComponentInExplorerInConnection = new CodeJavaScriptLinkedEntityOpenComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityOpenSolutionsContainingComponentInExplorerInConnectionCommandId
                , ActionOpenComponent.OpenSolutionsContainingComponentInExplorer
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (helper.TryGetLinkedEntityName(out string entityName))
            {
                helper.OpenEntityMetadataCommand(connectionData, entityName, this._actionOpen);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedEntityName(applicationObject, menuCommand);
        }
    }
}