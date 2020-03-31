using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectRegisterPluginAssemblyInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private ProjectRegisterPluginAssemblyInConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.ProjectRegisterPluginAssemblyInConnectionCommandId
            )
        {

        }

        public static ProjectRegisterPluginAssemblyInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ProjectRegisterPluginAssemblyInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var projectList = helper.GetSelectedProjects().ToList();

            helper.HandlePluginAssemblyRegisterCommand(connectionData, projectList);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectAny(applicationObject, menuCommand);
        }
    }
}
