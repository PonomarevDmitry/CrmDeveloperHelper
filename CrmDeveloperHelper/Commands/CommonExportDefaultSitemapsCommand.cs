using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportDefaultSitemapsCommand : AbstractDynamicCommandDefaultSiteMap
    {
        private CommonExportDefaultSitemapsCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.CommonExportDefaultSitemapsCommandId
            )
        {

        }

        public static CommonExportDefaultSitemapsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonExportDefaultSitemapsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, string selectedSitemap)
        {
            helper.HandleExportDefaultSitemap(selectedSitemap);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, string selectedSitemap, OleMenuCommand menuCommand)
        {
        }
    }
}
