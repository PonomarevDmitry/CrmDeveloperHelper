using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectPluginAssemblyStepsAddToSolutionLastCommand : AbstractAddObjectToSolutionLastCommand
    {
        private ProjectPluginAssemblyStepsAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.ProjectPluginAssemblyStepsAddToSolutionLastCommandId
                , ActionExecute
                , CommonHandlers.ActiveSolutionExplorerProjectAny
            )
        {

        }

        public static ProjectPluginAssemblyStepsAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ProjectPluginAssemblyStepsAddToSolutionLastCommand(commandService);
        }

        private static void ActionExecute(DTEHelper helper, ConnectionData connectionData, string solutionUniqueName)
        {
            var projects = helper.GetSelectedProjects().ToList();

            if (projects.Any())
            {
                helper.HandleAddingPluginAssemblyProcessingStepsByProjectCommand(null, solutionUniqueName, false, projects.Select(p => p.Name).ToArray());
            }
        }
    }
}