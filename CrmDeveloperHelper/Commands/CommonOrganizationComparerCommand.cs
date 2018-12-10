using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonOrganizationComparerCommand : AbstractCommand
    {
        private CommonOrganizationComparerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonOrganizationComparerCommandId, ActionExecute, null) { }

        public static CommonOrganizationComparerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonOrganizationComparerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOrganizationComparer();
        }
    }
}