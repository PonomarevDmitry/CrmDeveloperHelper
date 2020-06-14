using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class CodeJavaScriptLinkedSystemFormChangeInEditorInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private CodeJavaScriptLinkedSystemFormChangeInEditorInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedSystemFormChangeInEditorInConnectionCommandId)
        {
        }

        public static CodeJavaScriptLinkedSystemFormChangeInEditorInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptLinkedSystemFormChangeInEditorInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (helper.TryGetLinkedSystemForm(out string entityName, out Guid formId, out int formType))
            {
                helper.HandleLinkedSystemFormChangeInEntityEditorCommand(connectionData, entityName, formId, formType);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedSystemForm(applicationObject, menuCommand);
        }
    }
}