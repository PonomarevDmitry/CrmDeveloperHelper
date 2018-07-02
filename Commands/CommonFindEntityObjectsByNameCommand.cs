using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonFindEntityObjectsByNameCommand : AbstractCommand
    {
        private CommonFindEntityObjectsByNameCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonFindEntityObjectsByNameCommandId, ActionExecute, null) { }

        public static CommonFindEntityObjectsByNameCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonFindEntityObjectsByNameCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleFindByName();
        }
    }
}
