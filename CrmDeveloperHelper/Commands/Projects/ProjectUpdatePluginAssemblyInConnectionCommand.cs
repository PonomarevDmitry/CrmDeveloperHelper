using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectUpdatePluginAssemblyInConnectionCommand : AbstractDynamicCommandByConnectionWithoutCurrent
    {
        private ProjectUpdatePluginAssemblyInConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.ProjectCompareToCrmAssemblyInConnectionCommandId
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
            var project = helper.GetSelectedProject();

            helper.HandleUpdatingPluginAssemblyCommand(connectionData, project);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectSingle(applicationObject, menuCommand);
        }
    }
}