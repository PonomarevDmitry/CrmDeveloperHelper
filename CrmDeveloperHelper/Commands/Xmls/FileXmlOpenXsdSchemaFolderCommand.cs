using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class FileXmlOpenXsdSchemaFolderCommand : AbstractCommand
    {
        private FileXmlOpenXsdSchemaFolderCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.FileXmlOpenXsdSchemaFolderCommandId) { }

        public static FileXmlOpenXsdSchemaFolderCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileXmlOpenXsdSchemaFolderCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleOpenXsdSchemaFolder();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerXmlAny(applicationObject, menuCommand);
        }
    }
}
