using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Reports
{
    internal sealed class FileReportExplorerCommand : AbstractCommand
    {
        private FileReportExplorerCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileReportExplorerCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileReportExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileReportExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenReportExplorerCommand();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerReportSingle(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileReportExplorerCommand);
        }
    }
}