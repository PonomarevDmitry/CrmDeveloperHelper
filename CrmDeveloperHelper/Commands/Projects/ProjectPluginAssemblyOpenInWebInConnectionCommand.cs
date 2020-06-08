using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectPluginAssemblyOpenInWebInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ActionOpenComponent _actionOpen;

        private ProjectPluginAssemblyOpenInWebInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOpenComponent action)
            : base(commandService, baseIdStart)
        {
            this._actionOpen = action;
        }

        public static ProjectPluginAssemblyOpenInWebInConnectionCommand InstanceOpenDependentComponentsInWebInConnection { get; private set; }

        public static ProjectPluginAssemblyOpenInWebInConnectionCommand InstanceOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static ProjectPluginAssemblyOpenInWebInConnectionCommand InstanceOpenSolutionsContainingComponentInExplorerInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenDependentComponentsInWebInConnection = new ProjectPluginAssemblyOpenInWebInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.ProjectPluginAssemblyOpenDependentInWebInConnectionCommandId
                , ActionOpenComponent.OpenDependentComponentsInWeb
            );

            InstanceOpenDependentComponentsInExplorerInConnection = new ProjectPluginAssemblyOpenInWebInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.ProjectPluginAssemblyOpenDependentInExplorerInConnectionCommandId
                , ActionOpenComponent.OpenDependentComponentsInExplorer
            );

            InstanceOpenSolutionsContainingComponentInExplorerInConnection = new ProjectPluginAssemblyOpenInWebInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.ProjectPluginAssemblyOpenSolutionsContainingComponentInExplorerInConnectionCommandId
                , ActionOpenComponent.OpenSolutionsContainingComponentInExplorer
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var projectList = helper.GetSelectedProjects().ToList();

            helper.HandlePluginAssemblyProjectOpenCommand(connectionData,  projectList, this._actionOpen);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectAny(applicationObject, menuCommand);
        }
    }
}