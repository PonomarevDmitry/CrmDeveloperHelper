using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class CommonFindEntityObjectsByNameInExplorerCommand : AbstractSingleCommand
    {
        private CommonFindEntityObjectsByNameInExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonFindEntityObjectsByNameInExplorerCommandId) { }

        public static CommonFindEntityObjectsByNameInExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonFindEntityObjectsByNameInExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleFindEntityObjectsByNameInExplorer();
        }
    }
}
