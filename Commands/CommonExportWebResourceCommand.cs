using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportWebResourceCommand : AbstractCommand
    {
        private CommonExportWebResourceCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportWebResourceCommandId, ActionExecute, null) { }

        public static CommonExportWebResourceCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportWebResourceCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportWebResource();
        }
    }
}
