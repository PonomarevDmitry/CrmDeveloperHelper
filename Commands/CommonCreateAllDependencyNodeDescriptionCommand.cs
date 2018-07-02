using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonCreateAllDependencyNodeDescriptionCommand : AbstractCommand
    {
        private CommonCreateAllDependencyNodeDescriptionCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonCreateAllDependencyNodeDescriptionCommandId, ActionExecute, null) { }

        public static CommonCreateAllDependencyNodeDescriptionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonCreateAllDependencyNodeDescriptionCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleCreateAllDependencyNodesDescription();
        }
    }
}