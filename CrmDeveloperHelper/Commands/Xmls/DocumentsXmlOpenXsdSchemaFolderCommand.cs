using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    class DocumentsXmlOpenXsdSchemaFolderCommand : AbstractCommand
    {
        private DocumentsXmlOpenXsdSchemaFolderCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.DocumentsXmlOpenXsdSchemaFolderCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsXml) { }

        public static DocumentsXmlOpenXsdSchemaFolderCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new DocumentsXmlOpenXsdSchemaFolderCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenXsdSchemaFolder();
        }
    }
}
