using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class ProjectPluginTypeExplorerCommand : AbstractCommand
    {
        private ProjectPluginTypeExplorerCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.ProjectPluginTypeExplorerCommandId, ActionExecute, CommonHandlers.ActiveSolutionExplorerProjectAny)
        {
        }

        public static ProjectPluginTypeExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ProjectPluginTypeExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var projects = helper.GetSelectedProjects();

            if (projects.Any())
            {
                helper.HandleOpenPluginTypeExplorer(projects.First().Name);
            }
        }
    }
}