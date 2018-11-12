using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileCSharpProjectPluginAssemblyAddIntoSolutionCommand : AbstractCommand
    {
        private FileCSharpProjectPluginAssemblyAddIntoSolutionCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileCSharpProjectPluginAssemblyAddIntoSolutionCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileCSharpProjectPluginAssemblyAddIntoSolutionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileCSharpProjectPluginAssemblyAddIntoSolutionCommand(package);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerAnyItemContainsProject(command, menuCommand, FileOperations.SupportsCSharpType);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileCSharpProjectPluginAssemblyAddIntoSolutionCommand);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var list = helper.GetListSelectedItemInSolutionExplorer(FileOperations.SupportsCSharpType)
                                .Where(i => i.ProjectItem?.ContainingProject != null && !string.IsNullOrEmpty(i.ProjectItem?.ContainingProject?.Name))
                                .Select(i => i.ProjectItem.ContainingProject.Name);

            if (list.Any())
            {
                helper.HandleAddingPluginAssemblyIntoSolutionByProjectCommand(null, true, list.ToArray());
            }
        }
    }
}