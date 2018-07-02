using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonFindEntityObjectsByUniqueidentifierCommand : AbstractCommand
    {
        private CommonFindEntityObjectsByUniqueidentifierCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonFindEntityObjectsByUniqueidentifierCommandId, ActionExecute, null) { }

        public static CommonFindEntityObjectsByUniqueidentifierCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonFindEntityObjectsByUniqueidentifierCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleFindEntityByUniqueidentifier();
        }
    }
}
