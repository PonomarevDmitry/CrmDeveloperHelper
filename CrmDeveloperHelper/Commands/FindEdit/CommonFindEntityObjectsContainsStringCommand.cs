using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonFindEntityObjectsContainsStringCommand : AbstractCommand
    {
        private CommonFindEntityObjectsContainsStringCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonFindEntityObjectsContainsStringCommandId, ActionExecute, null) { }

        public static CommonFindEntityObjectsContainsStringCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonFindEntityObjectsContainsStringCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleFindContainsString();
        }
    }
}
