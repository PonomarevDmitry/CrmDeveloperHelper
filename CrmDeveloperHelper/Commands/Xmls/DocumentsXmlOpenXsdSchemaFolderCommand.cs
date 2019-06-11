using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    class DocumentsXmlOpenXsdSchemaFolderCommand : AbstractCommand
    {
        private DocumentsXmlOpenXsdSchemaFolderCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.DocumentsXmlOpenXsdSchemaFolderCommandId) { }

        public static DocumentsXmlOpenXsdSchemaFolderCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsXmlOpenXsdSchemaFolderCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleOpenXsdSchemaFolder();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsXml(applicationObject, menuCommand);
        }
    }
}
