using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputOpenCrmWebSiteEntityMetadataCommand : AbstractOutputWindowCommand
    {
        private OutputOpenCrmWebSiteEntityMetadataCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.OutputOpenCrmWebSiteEntityMetadataCommandId)
        {
        }

        public static OutputOpenCrmWebSiteEntityMetadataCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputOpenCrmWebSiteEntityMetadataCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleConnectionOpenEntityMetadataInWeb(connectionData);
        }
    }
}