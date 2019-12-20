using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class DocumentsXmlSetXsdSchemaCommand : AbstractDynamicCommandXsdSchemas
    {
        private DocumentsXmlSetXsdSchemaCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsXmlSetXsdSchemaCommandId
            )
        {

        }

        public static DocumentsXmlSetXsdSchemaCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsXmlSetXsdSchemaCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, Tuple<string, string[]> schemas)
        {
            var listDocuments = helper.GetOpenedDocumentsAsDocument(FileOperations.SupportsXmlType).ToList();

            if (listDocuments.Any())
            {
                foreach (var document in listDocuments)
                {
                    ContentComparerHelper.ReplaceXsdSchemaInDocument(document, schemas.Item2);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, Tuple<string, string[]> schemas, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsXml(applicationObject, menuCommand);
        }
    }
}
