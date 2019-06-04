using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonCustomControlExplorerCommand : AbstractCommand
    {
        private CommonCustomControlExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportCustomControlCommandId, ActionExecute, null) { }

        public static CommonCustomControlExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonCustomControlExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportCustomControl();
        }
    }
}
