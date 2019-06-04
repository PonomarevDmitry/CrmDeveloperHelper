using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonFindEntityObjectsByIdCommand : AbstractCommand
    {
        private CommonFindEntityObjectsByIdCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonFindEntityObjectsByIdCommandId, ActionExecute, null) { }

        public static CommonFindEntityObjectsByIdCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonFindEntityObjectsByIdCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleFindEntityById();
        }
    }
}
