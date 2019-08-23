using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks
{
    internal sealed class OutputCheckGlobalOptionSetDuplicateCommand : AbstractOutputWindowCommand
    {
        private OutputCheckGlobalOptionSetDuplicateCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidCommandSet.OutputCheckGlobalOptionSetDuplicateCommandId
            )
        {

        }

        public static OutputCheckGlobalOptionSetDuplicateCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCheckGlobalOptionSetDuplicateCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckGlobalOptionSetDuplicates(connectionData);
        }
    }
}
