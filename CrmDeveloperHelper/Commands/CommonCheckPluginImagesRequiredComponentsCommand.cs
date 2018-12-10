using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonCheckPluginImagesRequiredComponentsCommand : AbstractCommand
    {
        private CommonCheckPluginImagesRequiredComponentsCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonCheckPluginImagesRequiredComponentsCommandId, ActionExecute, null) { }

        public static CommonCheckPluginImagesRequiredComponentsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonCheckPluginImagesRequiredComponentsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleCheckPluginImagesRequiredComponents();
        }
    }
}
