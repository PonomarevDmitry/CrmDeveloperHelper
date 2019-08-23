using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class FolderXmlOpenXsdSchemaFolderCommand : AbstractCommand
    {
        private FolderXmlOpenXsdSchemaFolderCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.FolderXmlOpenXsdSchemaFolderCommandId) { }

        public static FolderXmlOpenXsdSchemaFolderCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderXmlOpenXsdSchemaFolderCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleOpenXsdSchemaFolder();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerXmlRecursive(applicationObject, menuCommand);
        }
    }
}
