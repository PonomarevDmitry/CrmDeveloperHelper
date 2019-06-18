using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class CommonFindEntityObjectsByPrefixCommand : AbstractCommand
    {
        private CommonFindEntityObjectsByPrefixCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.CommonFindEntityObjectsByPrefixCommandId) { }

        public static CommonFindEntityObjectsByPrefixCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonFindEntityObjectsByPrefixCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleFindEntityObjectsByPrefix();
        }
    }
}
