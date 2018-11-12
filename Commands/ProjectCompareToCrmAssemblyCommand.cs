using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class ProjectCompareToCrmAssemblyCommand : AbstractCommand
    {
        private ProjectCompareToCrmAssemblyCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.ProjectCompareToCrmAssemblyCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static ProjectCompareToCrmAssemblyCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ProjectCompareToCrmAssemblyCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var project = helper.GetSelectedProject();

            helper.HandleComparingPluginAssemblyAndLocalAssemblyCommand(null, project);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectSingle(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.ProjectCompareToCrmAssemblyCommand);
        }
    }
}