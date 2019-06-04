using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceExplorerCommand : AbstractCommand
    {
        private FileWebResourceExplorerCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileWebResourceExplorerCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileWebResourceExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileWebResourceExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenWebResourceExplorerCommand();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceSingle(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileWebResourceExplorerCommand);
        }
    }
}