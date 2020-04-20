using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeJavaScriptLinkedSystemFormExplorerCommand : AbstractCommand
    {
        private CodeJavaScriptLinkedSystemFormExplorerCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeJavaScriptLinkedSystemFormExplorerCommandId)
        {
        }

        public static CodeJavaScriptLinkedSystemFormExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptLinkedSystemFormExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsJavaScriptType);

            if (document == null)
            {
                return;
            }

            var objTextDoc = document.Object("TextDocument");
            if (objTextDoc != null
                && objTextDoc is EnvDTE.TextDocument textDocument
            )
            {
                string fileText = textDocument.StartPoint.CreateEditPoint().GetText(textDocument.EndPoint);

                if (CommonHandlers.GetLinkedSystemForm(fileText, out string entityName, out Guid formId, out int formType))
                {
                    helper.HandleExplorerSystemForm(entityName, formId.ToString());
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedSystemForm(applicationObject, menuCommand);
        }
    }
}