using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonSdkMessageTreeCommand : AbstractCommand
    {
        private CommonSdkMessageTreeCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonSdkMessageTreeCommandId, ActionExecute, null) { }

        public static CommonSdkMessageTreeCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonSdkMessageTreeCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleSdkMessageTree();
        }
    }
}
