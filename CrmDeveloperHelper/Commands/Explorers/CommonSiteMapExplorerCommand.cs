using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonSiteMapExplorerCommand : AbstractCommand
    {
        private CommonSiteMapExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonSiteMapExplorerCommandId) { }

        public static CommonSiteMapExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonSiteMapExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleExplorerSiteMap();
        }
    }
}