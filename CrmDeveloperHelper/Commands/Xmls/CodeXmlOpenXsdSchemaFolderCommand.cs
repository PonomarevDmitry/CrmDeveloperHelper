using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlOpenXsdSchemaFolderCommand : AbstractCommand
    {
        private CodeXmlOpenXsdSchemaFolderCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CodeXmlOpenXsdSchemaFolderCommandId) { }
         
        public static CodeXmlOpenXsdSchemaFolderCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlOpenXsdSchemaFolderCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleOpenXsdSchemaFolder();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentXml(applicationObject, menuCommand);
        }
    }
}
