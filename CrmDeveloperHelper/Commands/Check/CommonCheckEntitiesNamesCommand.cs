using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonCheckEntitiesNamesCommand : AbstractCommand
    {
        private CommonCheckEntitiesNamesCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonCheckEntitiesNamesCommandId, ActionExecute, null) { }

        public static CommonCheckEntitiesNamesCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonCheckEntitiesNamesCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleCheckEntitiesNames();
        }
    }
}
