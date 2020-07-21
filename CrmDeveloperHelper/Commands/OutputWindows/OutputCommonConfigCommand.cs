using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputCommonConfigCommand : AbstractOutputWindowCommand
    {
        private OutputCommonConfigCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.OutputCommonConfigCommandId)
        {
        }

        public static OutputCommonConfigCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCommonConfigCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.OpenCommonConfiguration();
        }
    }
}