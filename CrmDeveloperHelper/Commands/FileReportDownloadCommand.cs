using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileReportDownloadCommand : AbstractCommand
    {
        private FileReportDownloadCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileReportDownloadCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileReportDownloadCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileReportDownloadCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleReportDownloadCommand();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerReportSingle(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileReportDownloadCommand);
        }
    }
}