using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand : AbstractCommand
    {
        private ProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.ProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommandId) { }

        public static ProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var projectList = helper.GetSelectedProjects().ToList();

            helper.HandlePluginAssemblyBuildProjectUpdateCommand(null, true, projectList);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectAny(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.ProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand);
        }
    }
}
