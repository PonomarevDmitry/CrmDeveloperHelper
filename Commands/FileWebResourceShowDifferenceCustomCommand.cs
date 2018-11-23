using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileWebResourceShowDifferenceCustomCommand : AbstractCommand
    {
        private FileWebResourceShowDifferenceCustomCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileWebResourceShowDifferenceCustomCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileWebResourceShowDifferenceCustomCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileWebResourceShowDifferenceCustomCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleWebResourceDifferenceCommand(null, true);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextSingle(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileWebResourceShowDifferenceCustromCommand);
        }
    }
}