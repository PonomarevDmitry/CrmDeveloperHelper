using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpProjectPluginAssemblyActionOnComponentInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ActionOnComponent _actionOnComponent;

        private FileCSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static FileCSharpProjectPluginAssemblyActionOnComponentInConnectionCommand InstanceOpenDependentComponentsInWebInConnection { get; private set; }

        public static FileCSharpProjectPluginAssemblyActionOnComponentInConnectionCommand InstanceOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static FileCSharpProjectPluginAssemblyActionOnComponentInConnectionCommand InstanceOpenSolutionsListWithComponentInExplorerInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenDependentComponentsInWebInConnection = new FileCSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginAssemblyOpenDependentComponentsInWebInConnectionCommandId
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceOpenDependentComponentsInExplorerInConnection = new FileCSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginAssemblyOpenDependentComponentsInExplorerInConnectionCommandId
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceOpenSolutionsListWithComponentInExplorerInConnection = new FileCSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginAssemblyOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var projectItem = helper.GetSingleSelectedProjectItemInSolutionExplorer(FileOperations.SupportsCSharpType);

            if (projectItem != null && projectItem.ContainingProject != null)
            {
                helper.HandleActionOnProjectPluginAssemblyCommand(connectionData, new EnvDTE.Project[] { projectItem.ContainingProject }, this._actionOnComponent);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(applicationObject, menuCommand);
        }
    }
}
