using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonCheckEntitiesOwnerShipsCommand : AbstractCommand
    {
        private CommonCheckEntitiesOwnerShipsCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonCheckEntitiesOwnerShipsCommandId, ActionExecute, null) { }

        public static CommonCheckEntitiesOwnerShipsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonCheckEntitiesOwnerShipsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleCheckEntitiesOwnership();
        }
    }
}
