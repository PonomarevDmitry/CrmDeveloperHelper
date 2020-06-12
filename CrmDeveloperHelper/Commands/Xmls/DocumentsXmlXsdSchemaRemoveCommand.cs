using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class DocumentsXmlXsdSchemaRemoveCommand : AbstractSingleCommand
    {
        private DocumentsXmlXsdSchemaRemoveCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.DocumentsXmlXsdSchemaRemoveCommandId) { }

        public static DocumentsXmlXsdSchemaRemoveCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsXmlXsdSchemaRemoveCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<EnvDTE.Document> listDocuments = helper.GetOpenedDocumentsAsDocument(FileOperations.SupportsXmlType).ToList();

            if (listDocuments.Any())
            {
                foreach (var document in listDocuments)
                {
                    ContentComparerHelper.RemoveXsdSchemaInDocument(document);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsXml(applicationObject, menuCommand);
        }
    }
}
