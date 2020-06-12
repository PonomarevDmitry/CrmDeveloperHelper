using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlCommonXsdSchemaOpenFolderCommand : AbstractSingleCommand
    {
        private CodeXmlCommonXsdSchemaOpenFolderCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CodeXmlCommonXsdSchemaOpenFolderCommandId) { }
         
        public static CodeXmlCommonXsdSchemaOpenFolderCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlCommonXsdSchemaOpenFolderCommand(commandService);
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
