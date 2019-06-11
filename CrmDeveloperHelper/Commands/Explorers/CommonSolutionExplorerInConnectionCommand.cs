using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonSolutionExplorerInConnectionCommand : AbstractCommandByConnectionWithoutCurrent
    {
        private CommonSolutionExplorerInConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CommonSolutionExplorerInConnectionCommandId
            )
        {

        }

        public static CommonSolutionExplorerInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonSolutionExplorerInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenSolutionExplorerWindow(connectionData);
        }
    }
}