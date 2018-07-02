using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportOrganizationCommand : AbstractCommand
    {
        private CommonExportOrganizationCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportOrganizationCommandId, ActionExecute, null) { }

        public static CommonExportOrganizationCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportOrganizationCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportOrganizationInformation();
        }
    }
}
