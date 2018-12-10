using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportOrganizationCommand : AbstractCommand
    {
        private CommonExportOrganizationCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportOrganizationCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CommonExportOrganizationCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportOrganizationCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportOrganizationInformation();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CommonExportOrganizationCommand);
        }
    }
}