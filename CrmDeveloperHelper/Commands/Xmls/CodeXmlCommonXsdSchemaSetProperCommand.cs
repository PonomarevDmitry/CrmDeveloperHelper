using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlCommonXsdSchemaSetProperCommand : AbstractSingleCommand
    {
        private CodeXmlCommonXsdSchemaSetProperCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlCommonXsdSchemaSetProperCommandId)
        {
        }

        public static CodeXmlCommonXsdSchemaSetProperCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlCommonXsdSchemaSetProperCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            EnvDTE.Document document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsXmlType);

            if (document == null)
            {
                return;
            }

            var objTextDoc = document.Object(nameof(EnvDTE.TextDocument));

            if (objTextDoc != null && objTextDoc is EnvDTE.TextDocument textDocument)
            {
                string text = textDocument.StartPoint.CreateEditPoint().GetText(textDocument.EndPoint);

                if (!string.IsNullOrEmpty(text))
                {
                    if (ContentComparerHelper.TryParseXmlDocument(text, out var doc))
                    {
                        string docRootName = doc.Root.Name.ToString();

                        var schemasResources = AbstractDynamicCommandXsdSchemas.GetXsdSchemasByRootName(docRootName);

                        if (schemasResources != null)
                        {
                            ContentComparerHelper.ReplaceXsdSchemaInDocument(document, schemasResources);
                        }
                    }
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXml(applicationObject, menuCommand, out var doc);

            if (doc == null)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
                return;
            }

            string docRootName = doc.Name.ToString();

            var schemas = AbstractDynamicCommandXsdSchemas.GetSchemaByRootName(docRootName);

            if (!string.IsNullOrEmpty(schemas))
            {
                menuCommand.Text = string.Format(Properties.CommandNames.CodeXmlCommonXsdSchemaSetProperCommandFormat1, schemas);
            }
            else
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }
    }
}