using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonCurrentConnectionCommand : AbstractCommand
    {
        private CommonCurrentConnectionCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CommonCurrentConnectionCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CommonCurrentConnectionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonCurrentConnectionCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenCrmInWeb(null, Model.OpenCrmWebSiteType.CrmWebApplication);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CommonCurrentConnectionCommand);
        }
    }
}