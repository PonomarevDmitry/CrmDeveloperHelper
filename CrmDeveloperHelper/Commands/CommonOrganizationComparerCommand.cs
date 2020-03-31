using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonOrganizationComparerCommand : AbstractCommand
    {
        private CommonOrganizationComparerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonOrganizationComparerCommandId) { }

        public static CommonOrganizationComparerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonOrganizationComparerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleConnectionOrganizationComparer();
        }
    }
}