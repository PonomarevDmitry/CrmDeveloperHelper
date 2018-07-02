using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonCheckPluginStepsRequiredComponentsCommand : AbstractCommand
    {
        private CommonCheckPluginStepsRequiredComponentsCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonCheckPluginStepsRequiredComponentsCommandId, ActionExecute, null) { }

        public static CommonCheckPluginStepsRequiredComponentsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonCheckPluginStepsRequiredComponentsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleCheckPluginStepsRequiredComponents();
        }
    }
}
