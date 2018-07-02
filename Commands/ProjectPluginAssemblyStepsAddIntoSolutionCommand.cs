using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class ProjectPluginAssemblyStepsAddIntoSolutionCommand : AbstractCommand
    {
        private ProjectPluginAssemblyStepsAddIntoSolutionCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.ProjectPluginAssemblyStepsAddIntoSolutionCommandId, ActionExecute, ActionBeforeQueryStatus)
        {
        }

        public static ProjectPluginAssemblyStepsAddIntoSolutionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ProjectPluginAssemblyStepsAddIntoSolutionCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var project = helper.GetSelectedProject();

            helper.HandleAddingPluginAssemblyProcessingStepsByProjectCommand(project, true, null);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectSingle(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, "Add Steps for PluginAssembly into Crm Solution");
        }
    }
}