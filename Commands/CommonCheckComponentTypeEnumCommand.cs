using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonCheckComponentTypeEnumCommand : AbstractCommand
    {
        private CommonCheckComponentTypeEnumCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonCheckComponentTypeEnumCommandId, ActionExecute, null) { }

        public static CommonCheckComponentTypeEnumCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonCheckComponentTypeEnumCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleCheckComponentTypeEnum();
        }
    }
}
