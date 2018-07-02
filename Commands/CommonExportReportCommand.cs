using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportReportCommand : AbstractCommand
    {
        private CommonExportReportCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportReportCommandId, ActionExecute, null) { }

        public static CommonExportReportCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportReportCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportReport();
        }
    }
}
