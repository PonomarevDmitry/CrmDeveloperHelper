using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonTeamsExplorerCommand : AbstractCommand
    {
        private CommonTeamsExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.CommonTeamsExplorerCommandId) { }

        public static CommonTeamsExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonTeamsExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleOpenTeamsExplorer();
        }
    }
}
