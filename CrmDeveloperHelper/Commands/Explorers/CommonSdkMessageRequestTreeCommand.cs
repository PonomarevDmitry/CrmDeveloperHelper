using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonSdkMessageRequestTreeCommand : AbstractCommand
    {
        private CommonSdkMessageRequestTreeCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonSdkMessageRequestTreeCommandId, ActionExecute, null) { }

        public static CommonSdkMessageRequestTreeCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonSdkMessageRequestTreeCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleSdkMessageRequestTree();
        }
    }
}
