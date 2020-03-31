using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectUpdatePluginAssemblyCommand : AbstractCommand
    {
        private ProjectUpdatePluginAssemblyCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.ProjectUpdatePluginAssemblyCommandId) { }

        public static ProjectUpdatePluginAssemblyCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ProjectUpdatePluginAssemblyCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var projectList = helper.GetSelectedProjects().ToList();

            helper.HandlePluginAssemblyUpdatingInWindowCommand(null, projectList);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectAny(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.ProjectUpdatePluginAssemblyCommand);
        }
    }
}