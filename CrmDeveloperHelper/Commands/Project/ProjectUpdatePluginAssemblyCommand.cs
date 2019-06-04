using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class ProjectUpdatePluginAssemblyCommand : AbstractCommand
    {
        private ProjectUpdatePluginAssemblyCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.ProjectUpdatePluginAssemblyCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static ProjectUpdatePluginAssemblyCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ProjectUpdatePluginAssemblyCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var project = helper.GetSelectedProject();

            helper.HandleUpdatingPluginAssemblyCommand(null, project);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectSingle(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.ProjectUpdatePluginAssemblyCommand);
        }
    }
}