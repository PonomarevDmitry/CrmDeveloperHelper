using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileCSharpUpdateProxyClassesCommand : AbstractCommand
    {
        private FileCSharpUpdateProxyClassesCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileCSharpUpdateProxyClassesCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileCSharpUpdateProxyClassesCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileCSharpUpdateProxyClassesCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleUpdateProxyClasses();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileCSharpUpdateProxyClassesCommand);
        }
    }
}