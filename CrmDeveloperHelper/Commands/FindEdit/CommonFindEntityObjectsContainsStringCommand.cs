using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class CommonFindEntityObjectsContainsStringCommand : AbstractCommand
    {
        private CommonFindEntityObjectsContainsStringCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.CommonFindEntityObjectsContainsStringCommandId) { }

        public static CommonFindEntityObjectsContainsStringCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonFindEntityObjectsContainsStringCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleFindContainsString();
        }
    }
}
