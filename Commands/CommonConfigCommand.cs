using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonConfigCommand : AbstractCommand
    {
        private CommonConfigCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonConfigCommandId, ActionExecute, null) { }

        public static CommonConfigCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonConfigCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.OpenCommonConfiguration();
        }
    }
}
