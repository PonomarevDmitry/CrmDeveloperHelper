using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonCheckGlobalOptionSetDuplicateCommand : AbstractCommand
    {
        private CommonCheckGlobalOptionSetDuplicateCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonCheckGlobalOptionSetDuplicateCommandId, ActionExecute, null) { }

        public static CommonCheckGlobalOptionSetDuplicateCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonCheckGlobalOptionSetDuplicateCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleCheckGlobalOptionSetDuplicates();
        }
    }
}
