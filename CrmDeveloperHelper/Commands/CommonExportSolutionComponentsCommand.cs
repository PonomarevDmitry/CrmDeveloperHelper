using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportSolutionComponentsCommand : AbstractCommand
    {
        private CommonExportSolutionComponentsCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportSolutionComponentsCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CommonExportSolutionComponentsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportSolutionComponentsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenSolutionComponentExplorerWindow();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CommonExportSolutionComponentsCommand);
        }
    }
}