using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonCheckManagedElementsCommand : AbstractCommand
    {
        private CommonCheckManagedElementsCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonCheckManagedElementsCommandId, ActionExecute, null) { }

        public static CommonCheckManagedElementsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonCheckManagedElementsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleCheckManagedEntities();
        }
    }
}
