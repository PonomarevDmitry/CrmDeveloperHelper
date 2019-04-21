using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class DocumentsXmlRemoveXsdSchemaCommand : AbstractCommand
    {
        private DocumentsXmlRemoveXsdSchemaCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.DocumentsXmlRemoveXsdSchemaCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsXml) { }

        public static DocumentsXmlRemoveXsdSchemaCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new DocumentsXmlRemoveXsdSchemaCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<EnvDTE.Document> listDocuments = helper.GetOpenedDocumentsAsDocument(FileOperations.SupportsXmlType);

            if (listDocuments.Any())
            {
                foreach (var document in listDocuments)
                {
                    ContentCoparerHelper.RemoveXsdSchemaInDocument(document);
                }
            }
        }
    }
}
