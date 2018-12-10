using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonOpenCurrentConnectionCrmInWebCommand : AbstractCommand
    {
        private CommonOpenCurrentConnectionCrmInWebCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CommonOpenCurrentConnectionCrmInWebCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CommonOpenCurrentConnectionCrmInWebCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonOpenCurrentConnectionCrmInWebCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenCrmInWeb(null, Model.OpenCrmWebSiteType.CrmWebApplication);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CommonOpenCurrentConnectionCrmInWebCommand);
        }
    }
}