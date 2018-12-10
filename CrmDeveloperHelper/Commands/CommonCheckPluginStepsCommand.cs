using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonCheckPluginStepsCommand : AbstractCommand
    {
        private CommonCheckPluginStepsCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonCheckPluginStepsCommandId, ActionExecute, null) { }

        public static CommonCheckPluginStepsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonCheckPluginStepsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleCheckPluginSteps();
        }
    }
}
