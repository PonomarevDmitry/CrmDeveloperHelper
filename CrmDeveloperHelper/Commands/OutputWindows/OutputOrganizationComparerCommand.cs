using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputOrganizationComparerCommand : AbstractOutputWindowCommand
    {
        private OutputOrganizationComparerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputOrganizationComparerCommandId) { }

        public static OutputOrganizationComparerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputOrganizationComparerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleConnectionOrganizationComparer();
        }
    }
}