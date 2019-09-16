using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlFetchXmlPasteFromClipboardCommand : AbstractCommand
    {
        private CodeXmlFetchXmlPasteFromClipboardCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlFetchXmlPasteFromClipboardCommandId) { }

        public static CodeXmlFetchXmlPasteFromClipboardCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlFetchXmlPasteFromClipboardCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            EnvDTE.Document document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsXmlType);

            if (document != null
                && (System.Windows.Clipboard.ContainsText(System.Windows.TextDataFormat.Text) || System.Windows.Clipboard.ContainsText(System.Windows.TextDataFormat.UnicodeText))
            )
            {
                string javaScriptCode = System.Windows.Clipboard.GetText();

                if (!string.IsNullOrEmpty(javaScriptCode))
                {
                    JavaScriptFetchXmlParser parser = new JavaScriptFetchXmlParser(helper, javaScriptCode);

                    ContentCoparerHelper.GetTextViewAndMakeActionAsync(document, "Pasting FetchXml from JavaScript", parser.PasteFetchXml);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentXml(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusClipboardIsText(applicationObject, menuCommand);
        }
    }
}