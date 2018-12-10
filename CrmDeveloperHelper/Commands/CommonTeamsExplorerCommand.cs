using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonTeamsExplorerCommand : AbstractCommand
    {
        private CommonTeamsExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonTeamsExplorerCommandId, ActionExecute, null) { }

        public static CommonTeamsExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonTeamsExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenTeamsExplorer();
        }
    }
}
