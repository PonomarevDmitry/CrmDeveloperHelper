using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class FolderXmlOpenXsdSchemaFolderCommand : AbstractCommand
    {
        private FolderXmlOpenXsdSchemaFolderCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.FolderXmlOpenXsdSchemaFolderCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerXmlRecursive) { }

        public static FolderXmlOpenXsdSchemaFolderCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FolderXmlOpenXsdSchemaFolderCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenXsdSchemaFolder();
        }
    }
}
