using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportCustomControlCommand : AbstractCommand
    {
        private CommonExportCustomControlCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportCustomControlCommandId, ActionExecute, null) { }

        public static CommonExportCustomControlCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportCustomControlCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportCustomControl();
        }
    }
}
