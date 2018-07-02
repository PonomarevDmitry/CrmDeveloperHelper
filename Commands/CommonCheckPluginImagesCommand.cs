using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonCheckPluginImagesCommand : AbstractCommand
    {
        private CommonCheckPluginImagesCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonCheckPluginImagesCommandId, ActionExecute, null) { }

        public static CommonCheckPluginImagesCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonCheckPluginImagesCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleCheckPluginImages();
        }
    }
}
