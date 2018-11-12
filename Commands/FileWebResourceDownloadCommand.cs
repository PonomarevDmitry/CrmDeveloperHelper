using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileWebResourceDownloadCommand : AbstractCommand
    {
        private FileWebResourceDownloadCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileWebResourceDownloadCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileWebResourceDownloadCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileWebResourceDownloadCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleWebResourceDownloadCommand();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceSingle(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileWebResourceDownloadCommand);
        }
    }
}