using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectPluginAssemblyCreateDescriptionInConnectionCommand : AbstractDynamicCommandByConnectionWithoutCurrent
    {
        private ProjectPluginAssemblyCreateDescriptionInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.ProjectPluginAssemblyCreateDescriptionInConnectionCommandId)
        {
        }

        public static ProjectPluginAssemblyCreateDescriptionInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ProjectPluginAssemblyCreateDescriptionInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var projectList = helper.GetSelectedProjects().ToList();

            helper.HandlePluginAssemblyCreateDescriptionCommand(connectionData, projectList);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData element, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectAny(applicationObject, menuCommand);
        }
    }
}
