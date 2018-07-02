using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportPluginAssemblyDescriptionCommand : AbstractCommand
    {
        private CommonExportPluginAssemblyDescriptionCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportPluginAssemblyDescriptionCommandId, ActionExecute, null) { }

        public static CommonExportPluginAssemblyDescriptionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportPluginAssemblyDescriptionCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportPluginAssembly();
        }
    }
}
