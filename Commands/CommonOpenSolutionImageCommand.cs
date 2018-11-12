using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonOpenSolutionImageCommand : AbstractCommand
    {
        private CommonOpenSolutionImageCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonOpenSolutionImageCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CommonOpenSolutionImageCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonOpenSolutionImageCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenSolutionImageWindow();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CommonOpenSolutionImageCommand);
        }
    }
}