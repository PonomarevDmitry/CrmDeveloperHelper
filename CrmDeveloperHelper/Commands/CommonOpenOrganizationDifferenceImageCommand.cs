using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonOpenOrganizationDifferenceImageCommand : AbstractCommand
    {
        private CommonOpenOrganizationDifferenceImageCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.CommonOpenOrganizationDifferenceImageCommandId) { }

        public static CommonOpenOrganizationDifferenceImageCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonOpenOrganizationDifferenceImageCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleOpenOrganizationDifferenceImageWindow();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CommonOpenOrganizationDifferenceImageCommand);
        }
    }
}