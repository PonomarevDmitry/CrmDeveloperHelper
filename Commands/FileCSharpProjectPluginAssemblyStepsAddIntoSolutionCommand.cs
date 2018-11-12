using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileCSharpProjectPluginAssemblyStepsAddIntoSolutionCommand : AbstractCommand
    {
        private FileCSharpProjectPluginAssemblyStepsAddIntoSolutionCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileCSharpProjectPluginAssemblyStepsAddIntoSolutionCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileCSharpProjectPluginAssemblyStepsAddIntoSolutionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileCSharpProjectPluginAssemblyStepsAddIntoSolutionCommand(package);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerAnyItemContainsProject(command, menuCommand, FileOperations.SupportsCSharpType);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileCSharpProjectPluginAssemblyStepsAddIntoSolutionCommand);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var list = helper.GetListSelectedItemInSolutionExplorer(FileOperations.SupportsCSharpType)
                                 .Where(i => i.ProjectItem?.ContainingProject != null && !string.IsNullOrEmpty(i.ProjectItem?.ContainingProject?.Name))
                                 .Select(i => i.ProjectItem.ContainingProject.Name);

            if (list.Any())
            {
                helper.HandleAddingPluginAssemblyProcessingStepsByProjectCommand(null, true, list.ToArray());
            }
        }
    }
}