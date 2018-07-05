using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class ProjectPluginTypeDescriptionCommand : AbstractCommand
    {
        private ProjectPluginTypeDescriptionCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.ProjectPluginTypeDescriptionCommandId, ActionExecute, CommonHandlers.ActiveSolutionExplorerProjectAny)
        {
        }

        public static ProjectPluginTypeDescriptionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ProjectPluginTypeDescriptionCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var projects = helper.GetSelectedProjects();

            if (projects.Any())
            {
                helper.HandleExportPluginTypeDescription(projects.First().Name);
            }
        }
    }
}