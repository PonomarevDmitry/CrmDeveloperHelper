using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportAttributesAndDependentComponentsCommand : AbstractCommand
    {
        private CommonExportAttributesAndDependentComponentsCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportAttributesAndDependentComponentsCommandId, ActionExecute, null) { }

        public static CommonExportAttributesAndDependentComponentsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportAttributesAndDependentComponentsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportEntityAttributesDependentComponents();
        }
    }
}
