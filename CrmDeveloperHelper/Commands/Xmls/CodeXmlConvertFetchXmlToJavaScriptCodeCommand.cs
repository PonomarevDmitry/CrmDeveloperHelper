using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.IO;
using System.Linq;
using System.Windows;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlConvertFetchXmlToJavaScriptCodeCommand : AbstractCommand
    {
        private CodeXmlConvertFetchXmlToJavaScriptCodeCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.CodeXmlConvertFetchXmlToJavaScriptCodeCommandId) { }

        public static CodeXmlConvertFetchXmlToJavaScriptCodeCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlConvertFetchXmlToJavaScriptCodeCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            EnvDTE.Document document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsXmlType);

            if (document != null)
            {
                var objTextDoc = document.Object("TextDocument");
                if (objTextDoc != null
                    && objTextDoc is EnvDTE.TextDocument textDocument
                    )
                {
                    string text = textDocument.StartPoint.CreateEditPoint().GetText(textDocument.EndPoint);

                    string jsCode = ContentCoparerHelper.FormatToJavaScript("fetchXml", text);

                    Clipboard.SetText(jsCode);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentXml(applicationObject, menuCommand);
        }
    }
}
