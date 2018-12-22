using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonSolutionExplorerCommand : AbstractCommand
    {
        private CommonSolutionExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonSolutionExplorerCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CommonSolutionExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonSolutionExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenSolutionExplorerWindow(null);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CommonExportSolutionComponentsCommand);
        }
    }
}