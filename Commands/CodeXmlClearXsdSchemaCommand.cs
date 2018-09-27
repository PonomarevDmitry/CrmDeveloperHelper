using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Windows;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeXmlClearXsdSchemaCommand : AbstractCommand
    {
        private CodeXmlClearXsdSchemaCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeXmlClearXsdSchemaCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusActiveDocumentXml) { }

        public static CodeXmlClearXsdSchemaCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeXmlClearXsdSchemaCommand(package);
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
                    var editPoint = textDocument.StartPoint.CreateEditPoint();

                    string text = editPoint.GetText(textDocument.EndPoint);

                    if (ContentCoparerHelper.ClearXsdSchema(text, out var newText))
                    {
                        editPoint.ReplaceText(textDocument.EndPoint, newText, 0);
                    }
                }
            }
        }
    }
}