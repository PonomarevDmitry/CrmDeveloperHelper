using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Connections
{
    internal sealed class CommonCurrentConnectionCommand : AbstractCommand
    {
        private CommonCurrentConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.CommonCurrentConnectionCommandId) { }

        public static CommonCurrentConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCurrentConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleOpenCrmInWeb(null, Model.OpenCrmWebSiteType.CrmWebApplication);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CommonCurrentConnectionCommand);
        }
    }
}