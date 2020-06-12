using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class FileXmlXsdSchemaOpenFolderCommand : AbstractSingleCommand
    {
        private FileXmlXsdSchemaOpenFolderCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.FileXmlXsdSchemaOpenFolderCommandId) { }

        public static FileXmlXsdSchemaOpenFolderCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileXmlXsdSchemaOpenFolderCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleXsdSchemaOpenFolder();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerXmlAny(applicationObject, menuCommand);
        }
    }
}
