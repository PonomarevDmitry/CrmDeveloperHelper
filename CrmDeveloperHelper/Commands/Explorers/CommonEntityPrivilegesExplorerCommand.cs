using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonEntityPrivilegesExplorerCommand : AbstractCommand
    {
        private CommonEntityPrivilegesExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonEntityPrivilegesExplorerCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CommonEntityPrivilegesExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonEntityPrivilegesExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenEntityPrivilegesExplorer();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CommonEntityPrivilegesExplorerCommand);
        }
    }
}
