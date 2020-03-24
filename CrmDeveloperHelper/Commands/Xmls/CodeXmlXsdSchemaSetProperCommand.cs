using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlXsdSchemaSetProperCommand : AbstractCommand
    {
        private CodeXmlXsdSchemaSetProperCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlXsdSchemaSetProperCommandId) { }

        public static CodeXmlXsdSchemaSetProperCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlXsdSchemaSetProperCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            EnvDTE.Document document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsXmlType);

            if (document == null)
            {
                return;
            }

            var objTextDoc = document.Object("TextDocument");

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
                menuCommand.Text = string.Format(Properties.CommandNames.CodeXmlXsdSchemaSetProperCommandFormat1, schemas);
            }
            else
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }
    }
}