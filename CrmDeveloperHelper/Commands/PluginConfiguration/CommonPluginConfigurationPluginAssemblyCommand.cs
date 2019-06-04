using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonPluginConfigurationPluginAssemblyCommand : AbstractCommand
    {
        private CommonPluginConfigurationPluginAssemblyCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonPluginConfigurationPluginAssemblyCommandId, ActionExecute, null) { }

        public static CommonPluginConfigurationPluginAssemblyCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonPluginConfigurationPluginAssemblyCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandlePluginConfigurationPluginAssemblyDescription();
        }
    }
}
