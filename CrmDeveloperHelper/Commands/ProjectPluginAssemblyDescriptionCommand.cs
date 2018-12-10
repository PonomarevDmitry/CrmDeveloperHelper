using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class ProjectPluginAssemblyDescriptionCommand : AbstractCommand
    {
        private ProjectPluginAssemblyDescriptionCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.ProjectPluginAssemblyDescriptionCommandId, ActionExecute, CommonHandlers.ActiveSolutionExplorerProjectAny)
        {
        }

        public static ProjectPluginAssemblyDescriptionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ProjectPluginAssemblyDescriptionCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var projects = helper.GetSelectedProjects();

            if (projects.Any())
            {
                helper.HandleExportPluginAssembly(projects.First().Name);
            }
        }
    }
}