using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportDefaultSiteMapsCommand : AbstractDynamicCommandDefaultSiteMap
    {
        private CommonExportDefaultSiteMapsCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CommonExportDefaultSiteMapsCommandId)
        {
        }

        public static CommonExportDefaultSiteMapsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonExportDefaultSiteMapsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, string selectedSiteMap)
        {
            helper.HandleExportDefaultSiteMap(selectedSiteMap);
        }
    }
}