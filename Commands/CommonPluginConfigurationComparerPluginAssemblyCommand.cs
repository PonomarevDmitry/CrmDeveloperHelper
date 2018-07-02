using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonPluginConfigurationComparerPluginAssemblyCommand : AbstractCommand
    {
        private CommonPluginConfigurationComparerPluginAssemblyCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonPluginConfigurationComparerPluginAssemblyCommandId, ActionExecute, null) { }

        public static CommonPluginConfigurationComparerPluginAssemblyCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonPluginConfigurationComparerPluginAssemblyCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandlePluginConfigurationComparerPluginAssembly();
        }
    }
}
