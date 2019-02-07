using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal class CommonTraceExportFileCommand : AbstractCommand
    {
        private CommonTraceExportFileCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CommonTraceExportFileCommandId, ActionExecute, null) { }

        public static CommonTraceExportFileCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonTraceExportFileCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportTraceEnableFile();
        }
    }
}