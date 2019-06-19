using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class OutputFindEntityObjectsContainsStringCommand : AbstractOutputWindowCommand
    {
        private OutputFindEntityObjectsContainsStringCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputFindEntityObjectsContainsStringCommandId) { }

        public static OutputFindEntityObjectsContainsStringCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputFindEntityObjectsContainsStringCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleFindContainsString(connectionData);
        }
    }
}
