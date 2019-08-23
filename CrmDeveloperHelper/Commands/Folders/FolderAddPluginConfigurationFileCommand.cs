using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Folders
{
    internal sealed class FolderAddPluginConfigurationFileCommand : AbstractCommand
    {
        private FolderAddPluginConfigurationFileCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.FolderAddPluginConfigurationFileCommandId) { }

        public static FolderAddPluginConfigurationFileCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderAddPluginConfigurationFileCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleExportPluginConfigurationInfoFolder();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerFolderSingle(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FolderAddPluginConfigurationFileCommand);
        }
    }
}