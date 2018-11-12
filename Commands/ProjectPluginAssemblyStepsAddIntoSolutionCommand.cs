using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class ProjectPluginAssemblyStepsAddIntoSolutionCommand : AbstractCommand
    {
        private ProjectPluginAssemblyStepsAddIntoSolutionCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.ProjectPluginAssemblyStepsAddIntoSolutionCommandId, ActionExecute, ActionBeforeQueryStatus)
        {
        }

        public static ProjectPluginAssemblyStepsAddIntoSolutionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ProjectPluginAssemblyStepsAddIntoSolutionCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var projects = helper.GetSelectedProjects();

            if (projects.Any())
            {
                helper.HandleAddingPluginAssemblyProcessingStepsByProjectCommand(null, true, projects.Select(p => p.Name).ToArray());
            }
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectAny(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.ProjectPluginAssemblyStepsAddIntoSolutionCommand);
        }
    }
}