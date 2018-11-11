using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonPluginTreeCommand : AbstractCommand
    {
        private CommonPluginTreeCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonPluginTreeCommandId, ActionExecute, null) { }

        public static CommonPluginTreeCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonPluginTreeCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            string selection = helper.GetSelectedText();

            helper.HandleOpenPluginTree(selection, string.Empty, string.Empty);
        }
    }
}
