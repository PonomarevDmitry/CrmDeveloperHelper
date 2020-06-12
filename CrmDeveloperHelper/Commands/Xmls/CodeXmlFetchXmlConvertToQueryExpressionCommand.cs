using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlFetchXmlConvertToQueryExpressionCommand : AbstractSingleCommand
    {
        private CodeXmlFetchXmlConvertToQueryExpressionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlFetchXmlConvertToQueryExpressionCommandId)
        {
        }

        public static CodeXmlFetchXmlConvertToQueryExpressionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlFetchXmlConvertToQueryExpressionCommand(commandService);
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
                    string fetchXml = textDocument.StartPoint.CreateEditPoint().GetText(textDocument.EndPoint);

                    string codeCSharp = ContentComparerHelper.ConvertFetchXmlToQueryExpression(fetchXml);

                    ClipboardHelper.SetText(codeCSharp);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(applicationObject, menuCommand, out _, AbstractDynamicCommandXsdSchemas.RootFetch);
        }
    }
}