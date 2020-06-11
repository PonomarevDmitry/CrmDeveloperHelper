using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlCommonCopyToClipboardWithoutSchemaCommand : AbstractCommand
    {
        private CodeXmlCommonCopyToClipboardWithoutSchemaCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlCommonCopyToClipboardWithoutSchemaCommandId)
        {
        }

        public static CodeXmlCommonCopyToClipboardWithoutSchemaCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlCommonCopyToClipboardWithoutSchemaCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            EnvDTE.Document document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsXmlType);

            if (document != null)
            {
                var objTextDoc = document.Object(nameof(EnvDTE.TextDocument));

                if (objTextDoc != null && objTextDoc is EnvDTE.TextDocument textDocument)
                {
                    string text = textDocument.StartPoint.CreateEditPoint().GetText(textDocument.EndPoint);

                    text = ContentComparerHelper.RemoveInTextAllCustomXmlAttributesAndNamespaces(text);

                    ClipboardHelper.SetText(text);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentXml(applicationObject, menuCommand);
        }
    }
}