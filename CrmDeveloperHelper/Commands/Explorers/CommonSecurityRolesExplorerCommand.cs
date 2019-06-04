using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonSecurityRolesExplorerCommand : AbstractCommand
    {
        private CommonSecurityRolesExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonSecurityRolesExplorerCommandId, ActionExecute, null) { }

        public static CommonSecurityRolesExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonSecurityRolesExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenSecurityRolesExplorer();
        }
    }
}
