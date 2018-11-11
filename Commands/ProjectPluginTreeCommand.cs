using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
  internal sealed  class ProjectPluginTreeCommand : AbstractCommand
    {
        private ProjectPluginTreeCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.ProjectPluginTreeCommandId, ActionExecute, CommonHandlers.ActiveSolutionExplorerProjectAny)
        {
        }

        public static ProjectPluginTreeCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ProjectPluginTreeCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var projects = helper.GetSelectedProjects();

            if (projects.Any())
            {
                helper.HandleOpenPluginTree(string.Empty, projects.First().Name, string.Empty);
            }
        }
    }
}
