using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonOpenOrganizationDifferenceImageCommand : AbstractCommand
    {
        private CommonOpenOrganizationDifferenceImageCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonOpenOrganizationDifferenceImageCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CommonOpenOrganizationDifferenceImageCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonOpenOrganizationDifferenceImageCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenOrganizationDifferenceImageWindow();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CommonOpenOrganizationDifferenceImageCommand);
        }
    }
}