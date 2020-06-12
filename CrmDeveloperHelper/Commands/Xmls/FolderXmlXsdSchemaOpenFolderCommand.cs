using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class FolderXmlXsdSchemaOpenFolderCommand : AbstractSingleCommand
    {
        private FolderXmlXsdSchemaOpenFolderCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.FolderXmlXsdSchemaOpenFolderCommandId) { }

        public static FolderXmlXsdSchemaOpenFolderCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderXmlXsdSchemaOpenFolderCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleXsdSchemaOpenFolder();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerXmlRecursive(applicationObject, menuCommand);
        }
    }
}
