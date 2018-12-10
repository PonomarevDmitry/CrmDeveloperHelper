using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonCrmConnectionCommand : AbstractCommand
    {
        private CommonCrmConnectionCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CommonCrmConnectionCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CommonCrmConnectionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonCrmConnectionCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.OpenConnectionList();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CommonCrmConnectionCommand);
        }
    }
}