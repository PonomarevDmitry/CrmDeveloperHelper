using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.FindEdit
{
    internal sealed class OutputFindEntityObjectsByNameCommand : AbstractOutputWindowCommand
    {
        private OutputFindEntityObjectsByNameCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputFindEntityObjectsByNameCommandId) { }

        public static OutputFindEntityObjectsByNameCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputFindEntityObjectsByNameCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleFindEntityObjectsByName(connectionData);
        }
    }
}
