using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputCrmConnectionListCommand : AbstractOutputWindowCommand
    {
        private OutputCrmConnectionListCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.OutputCrmConnectionListCommandId)
        {
        }

        public static OutputCrmConnectionListCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCrmConnectionListCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleConnectionOpenList();
        }
    }
}
