using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileWebResourceShowDifferenceCommand : AbstractCommand
    {
        private FileWebResourceShowDifferenceCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileWebResourceShowDifferenceCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileWebResourceShowDifferenceCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileWebResourceShowDifferenceCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleWebResourceDifferenceCommand(null, false);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextSingle(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileWebResourceShowDifferenceCommand);
        }
    }
}