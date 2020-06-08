using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand : AbstractDynamicCommandByConnectionWithoutCurrent
    {
        private ProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.ProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommandId
            )
        {

        }

        public static ProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var projectList = helper.GetSelectedProjects().ToList();

            helper.HandlePluginAssemblyBuildProjectUpdateCommand(connectionData, true, projectList);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData element, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectAny(applicationObject, menuCommand);
        }
    }
}
