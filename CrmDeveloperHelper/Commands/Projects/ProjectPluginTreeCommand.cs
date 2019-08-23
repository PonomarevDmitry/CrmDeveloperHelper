using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
  internal sealed  class ProjectPluginTreeCommand : AbstractCommand
    {
        private ProjectPluginTreeCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.ProjectPluginTreeCommandId)
        {
        }

        public static ProjectPluginTreeCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ProjectPluginTreeCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var projects = helper.GetSelectedProjects();

            if (projects.Any())
            {
                helper.HandleOpenPluginTree(string.Empty, projects.First().Name, string.Empty);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectAny(applicationObject, menuCommand);
        }
    }
}
