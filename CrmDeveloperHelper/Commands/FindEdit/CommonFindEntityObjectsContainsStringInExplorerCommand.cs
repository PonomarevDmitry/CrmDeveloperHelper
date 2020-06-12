using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
 internal sealed   class CommonFindEntityObjectsContainsStringInExplorerCommand : AbstractSingleCommand
    {
        private CommonFindEntityObjectsContainsStringInExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonFindEntityObjectsContainsStringInExplorerCommandId) { }

        public static CommonFindEntityObjectsContainsStringInExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonFindEntityObjectsContainsStringInExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleFindEntityObjectsContainsStringInExplorer();
        }
    }
}
