using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlXsdSchemaOpenFolderCommand : AbstractCommand
    {
        private CodeXmlXsdSchemaOpenFolderCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CodeXmlXsdSchemaOpenFolderCommandId) { }
         
        public static CodeXmlXsdSchemaOpenFolderCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlXsdSchemaOpenFolderCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleXsdSchemaOpenFolder();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentXml(applicationObject, menuCommand);
        }
    }
}
