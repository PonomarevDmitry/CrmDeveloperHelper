using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportFormEventsCommand : AbstractCommand
    {
        private CommonExportFormEventsCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportFormEventsCommandId, ActionExecute, null) { }

        public static CommonExportFormEventsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportFormEventsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportFormEvents();
        }
    }
}
