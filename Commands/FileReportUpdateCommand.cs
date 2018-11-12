using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileReportUpdateCommand : AbstractCommand
    {
        private FileReportUpdateCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileReportUpdateCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerReportSingle) { }

        public static FileReportUpdateCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileReportUpdateCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleReportUpdateCommand();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerReportSingle(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileReportUpdateCommand);
        }
    }
}