using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class CommonEditEntityObjectsByIdCommand : AbstractCommand
    {
        private CommonEditEntityObjectsByIdCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonEditEntityObjectsByIdCommandId, ActionExecute, null) { }

        public static CommonEditEntityObjectsByIdCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonEditEntityObjectsByIdCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleEditEntityById();
        }
    }
}
