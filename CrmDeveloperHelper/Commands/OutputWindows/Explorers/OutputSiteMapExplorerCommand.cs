using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputSiteMapExplorerCommand : AbstractCommand
    {
        private OutputSiteMapExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputSiteMapExplorerCommandId) { }

        public static OutputSiteMapExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputSiteMapExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var connectionData = helper.GetOutputWindowConnection();

            if (connectionData == null)
            {
                return;
            }

            helper.HandleExplorerSitemap(connectionData);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusIsConnectionOutput(applicationObject, menuCommand);
        }
    }
}