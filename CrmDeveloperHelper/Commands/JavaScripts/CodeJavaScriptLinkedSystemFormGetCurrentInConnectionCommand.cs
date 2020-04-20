using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeJavaScriptLinkedSystemFormGetCurrentInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private CodeJavaScriptLinkedSystemFormGetCurrentInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedSystemFormGetCurrentInConnectionCommandId)
        {
        }

        public static CodeJavaScriptLinkedSystemFormGetCurrentInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptLinkedSystemFormGetCurrentInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsJavaScriptType).Take(2).ToList();

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
                    helper.HandleSystemFormGetCurrentCommand(connectionData, formId);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedSystemForm(applicationObject, menuCommand);
        }
    }
}