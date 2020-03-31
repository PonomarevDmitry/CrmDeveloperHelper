using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectPluginAssemblyStepsAddToSolutionLastCommand : AbstractDynamicCommandOnSolutionLast
    {
        private ProjectPluginAssemblyStepsAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.ProjectPluginAssemblyStepsAddToSolutionLastCommandId
            )
        {

        }

        public static ProjectPluginAssemblyStepsAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ProjectPluginAssemblyStepsAddToSolutionLastCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            var projects = helper.GetSelectedProjects().ToList();

            if (projects.Any())
            {
                helper.HandlePluginAssemblyAddingProcessingStepsByProjectCommand(null, solutionUniqueName, false, projects.Select(p => p.Name).ToArray());
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, string solutionUniqueName, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectAny(applicationObject, menuCommand);
        }
    }
}