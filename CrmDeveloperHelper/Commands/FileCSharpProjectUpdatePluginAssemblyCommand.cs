using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileCSharpProjectUpdatePluginAssemblyCommand : AbstractCommand
    {
        private FileCSharpProjectUpdatePluginAssemblyCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileCSharpProjectUpdatePluginAssemblyCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileCSharpProjectUpdatePluginAssemblyCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileCSharpProjectUpdatePluginAssemblyCommand(package);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(command, menuCommand, FileOperations.SupportsCSharpType);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileCSharpProjectUpdatePluginAssemblyCommand);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var projectItem = helper.GetSingleSelectedProjectItemInSolutionExplorer(FileOperations.SupportsCSharpType);

            if (projectItem != null && projectItem.ContainingProject != null)
            {
                helper.HandleUpdatingPluginAssemblyCommand(null, projectItem.ContainingProject);
            }
        }
    }
}