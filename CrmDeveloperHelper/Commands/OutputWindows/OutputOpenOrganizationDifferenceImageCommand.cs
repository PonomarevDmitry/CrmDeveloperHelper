using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputOpenOrganizationDifferenceImageCommand : AbstractOutputWindowCommand
    {
        private OutputOpenOrganizationDifferenceImageCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputOpenOrganizationDifferenceImageCommandId) { }

        public static OutputOpenOrganizationDifferenceImageCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputOpenOrganizationDifferenceImageCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenOrganizationDifferenceImageWindow(connectionData);
        }
    }
}