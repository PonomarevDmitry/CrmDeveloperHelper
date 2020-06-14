using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class CodeJavaScriptLinkedSystemFormActionOnComponentInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ActionOnComponent _actionOnComponent;

        private CodeJavaScriptLinkedSystemFormActionOnComponentInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static CodeJavaScriptLinkedSystemFormActionOnComponentInConnectionCommand InstanceOpenInWebInConnection { get; private set; }

        public static CodeJavaScriptLinkedSystemFormActionOnComponentInConnectionCommand InstanceOpenDependentComponentsInWebInConnection { get; private set; }

        public static CodeJavaScriptLinkedSystemFormActionOnComponentInConnectionCommand InstanceOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static CodeJavaScriptLinkedSystemFormActionOnComponentInConnectionCommand InstanceOpenSolutionsContainingComponentInExplorerInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWebInConnection = new CodeJavaScriptLinkedSystemFormActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedSystemFormOpenInWebInConnectionCommandId
                , ActionOnComponent.OpenInWeb
            );

            InstanceOpenDependentComponentsInWebInConnection = new CodeJavaScriptLinkedSystemFormActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedSystemFormOpenDependentInWebInConnectionCommandId
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceOpenDependentComponentsInExplorerInConnection = new CodeJavaScriptLinkedSystemFormActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedSystemFormOpenDependentInExplorerInConnectionCommandId
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceOpenSolutionsContainingComponentInExplorerInConnection = new CodeJavaScriptLinkedSystemFormActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedSystemFormOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (helper.TryGetLinkedSystemForm(out string entityName, out Guid formId, out int formType))
            {
                helper.HandleOpenLinkedSystemForm(connectionData, this._actionOnComponent, entityName, formId, formType);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedSystemForm(applicationObject, menuCommand);
        }
    }
}