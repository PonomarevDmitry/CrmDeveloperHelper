using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpProjectPluginAssemblyCreateDescriptionInConnectionCommand : AbstractDynamicCommandByConnectionAllWithoutCurrent
    {
        private readonly ActionOnComponent _actionOnComponent;

        private FileCSharpProjectPluginAssemblyCreateDescriptionInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static FileCSharpProjectPluginAssemblyCreateDescriptionInConnectionCommand InstanceEntityDescription { get; private set; }

        public static FileCSharpProjectPluginAssemblyCreateDescriptionInConnectionCommand InstanceDescription { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceEntityDescription = new FileCSharpProjectPluginAssemblyCreateDescriptionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginAssemblyCreateEntityDescriptionInConnectionCommandId
                , ActionOnComponent.EntityDescription
            );

            InstanceDescription = new FileCSharpProjectPluginAssemblyCreateDescriptionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginAssemblyCreateDescriptionInConnectionCommandId
                , ActionOnComponent.Description
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

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData element, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(applicationObject, menuCommand);
        }
    }
}
