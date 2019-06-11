using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckGlobalOptionSetDuplicateCommand : AbstractDynamicCommandByConnectionAll
    {
        private CommonCheckGlobalOptionSetDuplicateCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CommonCheckGlobalOptionSetDuplicateCommandId
            )
        {

        }

        public static CommonCheckGlobalOptionSetDuplicateCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckGlobalOptionSetDuplicateCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckGlobalOptionSetDuplicates(connectionData);
        }
    }
}
