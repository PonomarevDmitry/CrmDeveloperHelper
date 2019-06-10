using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectPluginAssemblyAddToSolutionLastCommand : AbstractAddObjectToSolutionLastCommand
    {
        private ProjectPluginAssemblyAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.ProjectPluginAssemblyAddToSolutionLastCommandId
                , ActionExecute
                , CommonHandlers.ActiveSolutionExplorerProjectAny
            )
        {

        }

        public static ProjectPluginAssemblyAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ProjectPluginAssemblyAddToSolutionLastCommand(commandService);
        }

        private static void ActionExecute(DTEHelper helper, ConnectionData connectionData, string solutionUniqueName)
        {
            var projects = helper.GetSelectedProjects().ToList();

            if (projects.Any())
            {
                helper.HandleAddingPluginAssemblyToSolutionByProjectCommand(null, solutionUniqueName, false, projects.Select(p => p.Name).ToArray());
            }
        }
    }
}