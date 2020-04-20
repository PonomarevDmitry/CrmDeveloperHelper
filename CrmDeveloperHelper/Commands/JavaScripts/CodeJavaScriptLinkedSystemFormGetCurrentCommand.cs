using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeJavaScriptLinkedSystemFormGetCurrentCommand : AbstractCommand
    {
        private CodeJavaScriptLinkedSystemFormGetCurrentCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeJavaScriptLinkedSystemFormGetCurrentCommandId)
        {
        }

        public static CodeJavaScriptLinkedSystemFormGetCurrentCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptLinkedSystemFormGetCurrentCommand(commandService);
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
                    helper.HandleSystemFormGetCurrentCommand(null, formId);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedSystemForm(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeJavaScriptLinkedSystemFormGetCurrentCommand);
        }
    }
}