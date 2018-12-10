using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonCheckEntitiesNamesAndShowDependentComponentsCommand : AbstractCommand
    {
        private CommonCheckEntitiesNamesAndShowDependentComponentsCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonCheckEntitiesNamesAndShowDependentComponentsCommandId, ActionExecute, null) { }

        public static CommonCheckEntitiesNamesAndShowDependentComponentsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonCheckEntitiesNamesAndShowDependentComponentsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleCheckEntitiesNamesAndShowDependentComponents();
        }
    }
}
