using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonPluginConfigurationCreateCommand : AbstractCommand
    {
        private CommonPluginConfigurationCreateCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonPluginConfigurationCreateCommandId, ActionExecute, null) { }

        public static CommonPluginConfigurationCreateCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonPluginConfigurationCreateCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandlePluginConfigurationCreate();
        }
    }
}
