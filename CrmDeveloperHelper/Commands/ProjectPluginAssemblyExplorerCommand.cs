using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class ProjectPluginAssemblyExplorerCommand : AbstractCommand
    {
        private ProjectPluginAssemblyExplorerCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.ProjectPluginAssemblyExplorerCommandId, ActionExecute, CommonHandlers.ActiveSolutionExplorerProjectAny)
        {
        }

        public static ProjectPluginAssemblyExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ProjectPluginAssemblyExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var projects = helper.GetSelectedProjects();

            if (projects.Any())
            {
                helper.HandleOpenPluginAssemblyExplorer(projects.First().Name);
            }
        }
    }
}