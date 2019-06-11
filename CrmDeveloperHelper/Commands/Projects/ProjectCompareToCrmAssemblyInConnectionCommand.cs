using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectCompareToCrmAssemblyInConnectionCommand : AbstractDynamicCommandByConnectionWithoutCurrent
    {
        private ProjectCompareToCrmAssemblyInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.ProjectCompareToCrmAssemblyInConnectionCommandId)
        {

        }

        public static ProjectCompareToCrmAssemblyInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ProjectCompareToCrmAssemblyInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var project = helper.GetSelectedProject();

            helper.HandleComparingPluginAssemblyAndLocalAssemblyCommand(connectionData, project);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectSingle(applicationObject, menuCommand);
        }
    }
}