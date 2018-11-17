using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.IO;
using System.Linq;
using System.Windows;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeXmlConvertFetchXmlToJavaScriptCodeCommand : AbstractCommand
    {
        private CodeXmlConvertFetchXmlToJavaScriptCodeCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeXmlConvertFetchXmlToJavaScriptCodeCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusActiveDocumentXml) { }

        public static CodeXmlConvertFetchXmlToJavaScriptCodeCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeXmlConvertFetchXmlToJavaScriptCodeCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
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
    }
}
