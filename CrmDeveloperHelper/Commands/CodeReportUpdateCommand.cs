using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeReportUpdateCommand : AbstractCommand
    {
        private CodeReportUpdateCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeReportUpdateCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeReportUpdateCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeReportUpdateCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleReportUpdateCommand();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentReport(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CodeReportUpdateCommand);
        }
    }
}