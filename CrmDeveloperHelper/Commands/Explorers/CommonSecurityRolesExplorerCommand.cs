using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonSecurityRolesExplorerCommand : AbstractCommand
    {
        private CommonSecurityRolesExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.CommonSecurityRolesExplorerCommandId) { }

        public static CommonSecurityRolesExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonSecurityRolesExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleOpenSecurityRolesExplorer();
        }
    }
}
