using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlCommonConvertToJavaScriptCodeCommand : AbstractCommand
    {
        private CodeXmlCommonConvertToJavaScriptCodeCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlCommonConvertToJavaScriptCodeCommandId) { }

        public static CodeXmlCommonConvertToJavaScriptCodeCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlCommonConvertToJavaScriptCodeCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            EnvDTE.Document document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsXmlType);

            if (document != null)
            {
                var objTextDoc = document.Object(nameof(EnvDTE.TextDocument));
                if (objTextDoc != null
                    && objTextDoc is EnvDTE.TextDocument textDocument
                    )
                {
                    string text = textDocument.StartPoint.CreateEditPoint().GetText(textDocument.EndPoint);

                    string jsCode = ContentComparerHelper.FormatToJavaScript(Entities.SavedQuery.Schema.Variables.fetchxml, text);

                    ClipboardHelper.SetText(jsCode);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentXml(applicationObject, menuCommand);
        }
    }
}