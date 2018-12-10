using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonOpenXsdSchemaFolderCommand : AbstractCommand
    {
        private CommonOpenXsdSchemaFolderCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonOpenXsdSchemaFolderCommandId, ActionExecute, null) { }

        public static CommonOpenXsdSchemaFolderCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonOpenXsdSchemaFolderCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenXsdSchemaFolder();
        }
    }
}
