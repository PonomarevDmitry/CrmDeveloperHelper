using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectPluginAssemblyOpenInWebInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ActionOnComponent _actionOnComponent;

        private ProjectPluginAssemblyOpenInWebInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._actionOnComponent = actionOnComponent;
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

            helper.HandleActionOnProjectPluginAssemblyCommand(connectionData,  projectList, this._actionOnComponent);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectAny(applicationObject, menuCommand);
        }
    }
}