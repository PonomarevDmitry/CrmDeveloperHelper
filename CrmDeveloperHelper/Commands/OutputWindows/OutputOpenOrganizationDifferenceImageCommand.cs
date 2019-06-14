using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputOpenOrganizationDifferenceImageCommand : AbstractCommand
    {
        private OutputOpenOrganizationDifferenceImageCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputOpenOrganizationDifferenceImageCommandId) { }

        public static OutputOpenOrganizationDifferenceImageCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputOpenOrganizationDifferenceImageCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var connectionData = helper.GetOutputWindowConnection();

            if (connectionData == null)
            {
                return;
            }

            helper.HandleOpenOrganizationDifferenceImageWindow(connectionData);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusIsConnectionOutput(applicationObject, menuCommand);
        }
    }
}