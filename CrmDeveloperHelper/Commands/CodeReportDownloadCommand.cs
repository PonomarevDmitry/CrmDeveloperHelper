using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeReportDownloadCommand : AbstractCommand
    {
        private CodeReportDownloadCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeReportDownloadCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeReportDownloadCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeReportDownloadCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleReportDownloadCommand();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentReport(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CodeReportDownloadCommand);
        }
    }
}