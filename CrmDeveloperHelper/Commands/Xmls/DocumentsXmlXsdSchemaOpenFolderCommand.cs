using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    class DocumentsXmlXsdSchemaOpenFolderCommand : AbstractSingleCommand
    {
        private DocumentsXmlXsdSchemaOpenFolderCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.DocumentsXmlXsdSchemaOpenFolderCommandId) { }

        public static DocumentsXmlXsdSchemaOpenFolderCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsXmlXsdSchemaOpenFolderCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleXsdSchemaOpenFolder();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsXml(applicationObject, menuCommand);
        }
    }
}
