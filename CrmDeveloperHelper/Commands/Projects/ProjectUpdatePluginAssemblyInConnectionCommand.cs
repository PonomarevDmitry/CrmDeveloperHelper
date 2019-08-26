using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectUpdatePluginAssemblyInConnectionCommand : AbstractDynamicCommandByConnectionWithoutCurrent
    {
        private ProjectUpdatePluginAssemblyInConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.ProjectUpdatePluginAssemblyInConnectionCommandId
            )
        {

        }

        public static ProjectUpdatePluginAssemblyInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ProjectUpdatePluginAssemblyInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var projectList = helper.GetSelectedProjects().ToList();

            helper.HandleUpdatingPluginAssemblyCommand(connectionData, projectList);
        }

        //protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        //{
        //    CommonHandlers.ActiveSolutionExplorerProjectSingle(applicationObject, menuCommand);
        //}
    }
}