using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputOpenCrmWebSiteEntityListCommand : AbstractOutputWindowCommand
    {
        private OutputOpenCrmWebSiteEntityListCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.OutputOpenCrmWebSiteEntityListCommandId)
        {
        }

        public static OutputOpenCrmWebSiteEntityListCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputOpenCrmWebSiteEntityListCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleConnectionOpenEntityListInWeb(connectionData);
        }
    }
}
