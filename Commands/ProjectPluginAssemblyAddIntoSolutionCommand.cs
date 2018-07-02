using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class ProjectPluginAssemblyAddIntoSolutionCommand : AbstractCommand
    {
        private ProjectPluginAssemblyAddIntoSolutionCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.ProjectPluginAssemblyAddIntoSolutionCommandId, ActionExecute, ActionBeforeQueryStatus)
        {
        }

        public static ProjectPluginAssemblyAddIntoSolutionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ProjectPluginAssemblyAddIntoSolutionCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var project = helper.GetSelectedProject();

            helper.HandleAddingPluginAssemblyIntoSolutionByProjectCommand(project, true, null);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectSingle(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, "Add PluginAssembly into Crm Solution");
        }
    }
}