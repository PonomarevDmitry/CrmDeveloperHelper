using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeJavaScriptLinkedSystemFormOpenInWebInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ActionOpenComponent _actionOpen;

        private CodeJavaScriptLinkedSystemFormOpenInWebInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOpenComponent action)
            : base(commandService, baseIdStart)
        {
            this._actionOpen = action;
        }

        public static CodeJavaScriptLinkedSystemFormOpenInWebInConnectionCommand InstanceOpenInWebInConnection { get; private set; }

        public static CodeJavaScriptLinkedSystemFormOpenInWebInConnectionCommand InstanceOpenDependentComponentsInWebInConnection { get; private set; }

        public static CodeJavaScriptLinkedSystemFormOpenInWebInConnectionCommand InstanceOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static CodeJavaScriptLinkedSystemFormOpenInWebInConnectionCommand InstanceOpenSolutionsContainingComponentInExplorerInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWebInConnection = new CodeJavaScriptLinkedSystemFormOpenInWebInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedSystemFormOpenInWebInConnectionCommandId
                , ActionOpenComponent.OpenInWeb
            );

            InstanceOpenDependentComponentsInWebInConnection = new CodeJavaScriptLinkedSystemFormOpenInWebInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedSystemFormOpenDependentInWebInConnectionCommandId
                , ActionOpenComponent.OpenDependentComponentsInWeb
            );

            InstanceOpenDependentComponentsInExplorerInConnection = new CodeJavaScriptLinkedSystemFormOpenInWebInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedSystemFormOpenDependentInExplorerInConnectionCommandId
                , ActionOpenComponent.OpenDependentComponentsInExplorer
            );

            InstanceOpenSolutionsContainingComponentInExplorerInConnection = new CodeJavaScriptLinkedSystemFormOpenInWebInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedSystemFormOpenSolutionsContainingComponentInExplorerInConnectionCommandId
                , ActionOpenComponent.OpenSolutionsContainingComponentInExplorer
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (helper.TryGetLinkedSystemForm(out string entityName, out Guid formId, out int formType))
            {
                helper.HandleOpenLinkedSystemForm(connectionData, this._actionOpen, entityName, formId, formType);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedSystemForm(applicationObject, menuCommand);
        }
    }
}