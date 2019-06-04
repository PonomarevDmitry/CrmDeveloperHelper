using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class CommonReportExplorerCommand : AbstractCommand
    {
        private CommonReportExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportReportCommandId, ActionExecute, null) { }

        public static CommonReportExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonReportExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportReport();
        }
    }
}
