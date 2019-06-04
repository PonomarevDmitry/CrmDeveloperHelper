using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FolderAddPluginConfigurationFileCommand : AbstractCommand
    {
        private FolderAddPluginConfigurationFileCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FolderAddPluginConfigurationFileCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FolderAddPluginConfigurationFileCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FolderAddPluginConfigurationFileCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportPluginConfigurationInfoFolder();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerFolderSingle(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FolderAddPluginConfigurationFileCommand);
        }
    }
}