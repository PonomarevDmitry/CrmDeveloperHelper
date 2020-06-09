using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectPluginAssemblyOpenInWebInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ActionOnComponent _actionOpen;

        private ProjectPluginAssemblyOpenInWebInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent action)
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
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceOpenDependentComponentsInExplorerInConnection = new ProjectPluginAssemblyOpenInWebInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.ProjectPluginAssemblyOpenDependentInExplorerInConnectionCommandId
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceOpenSolutionsContainingComponentInExplorerInConnection = new ProjectPluginAssemblyOpenInWebInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.ProjectPluginAssemblyOpenSolutionsContainingComponentInExplorerInConnectionCommandId
                , ActionOnComponent.OpenSolutionsContainingComponentInExplorer
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