using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonCheckMarkedToDeleteAndShowDependentComponentsCommand : AbstractCommand
    {
        private CommonCheckMarkedToDeleteAndShowDependentComponentsCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonCheckMarkedToDeleteAndShowDependentComponentsCommandId, ActionExecute, null) { }

        public static CommonCheckMarkedToDeleteAndShowDependentComponentsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonCheckMarkedToDeleteAndShowDependentComponentsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleCheckMarkedAndShowDependentComponents();
        }
    }
}
