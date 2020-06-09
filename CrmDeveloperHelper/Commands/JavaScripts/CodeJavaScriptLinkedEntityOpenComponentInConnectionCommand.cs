using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeJavaScriptLinkedEntityOpenComponentInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ActionOnComponent _actionOpen;

        private CodeJavaScriptLinkedEntityOpenComponentInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent action)
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
                , ActionOnComponent.OpenInWeb
            );
            
            InstanceOpenListInWebInConnection = new CodeJavaScriptLinkedEntityOpenComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityOpenListInWebInConnectionCommandId
                , ActionOnComponent.OpenListInWeb
            );

            InstanceOpenDependentComponentsInWebInConnection = new CodeJavaScriptLinkedEntityOpenComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityOpenDependentInWebInConnectionCommandId
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceOpenDependentComponentsInExplorerInConnection = new CodeJavaScriptLinkedEntityOpenComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityOpenDependentInExplorerInConnectionCommandId
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceOpenSolutionsContainingComponentInExplorerInConnection = new CodeJavaScriptLinkedEntityOpenComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedEntityOpenSolutionsContainingComponentInExplorerInConnectionCommandId
                , ActionOnComponent.OpenSolutionsContainingComponentInExplorer
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