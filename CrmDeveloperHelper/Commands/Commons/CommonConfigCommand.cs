using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons
{
    internal sealed class CommonConfigCommand : AbstractCommand
    {
        private CommonConfigCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonConfigCommandId) { }

        public static CommonConfigCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonConfigCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.OpenCommonConfiguration();
        }
    }
}
