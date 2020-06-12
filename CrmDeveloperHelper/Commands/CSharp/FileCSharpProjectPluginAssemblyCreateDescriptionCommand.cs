using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpProjectPluginAssemblyCreateDescriptionCommand : AbstractSingleCommand
    {
        private readonly ActionOnComponent _actionOnComponent;

        private FileCSharpProjectPluginAssemblyCreateDescriptionCommand(OleMenuCommandService commandService, int idCommand, ActionOnComponent actionOnComponent)
            : base(commandService, idCommand)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static FileCSharpProjectPluginAssemblyCreateDescriptionCommand InstanceEntityDescription { get; private set; }

        public static FileCSharpProjectPluginAssemblyCreateDescriptionCommand InstanceDescription { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceEntityDescription = new FileCSharpProjectPluginAssemblyCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.FileCSharpProjectPluginAssemblyCreateEntityDescriptionCommandId
                , ActionOnComponent.EntityDescription
            );

            InstanceDescription = new FileCSharpProjectPluginAssemblyCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.FileCSharpProjectPluginAssemblyCreateDescriptionCommandId
                , ActionOnComponent.Description
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var projectItem = helper.GetSingleSelectedProjectItemInSolutionExplorer(FileOperations.SupportsCSharpType);

            if (projectItem != null && projectItem.ContainingProject != null)
            {
                helper.HandleActionOnProjectPluginAssemblyCommand(null, new EnvDTE.Project[] { projectItem.ContainingProject }, this._actionOnComponent);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(applicationObject, menuCommand);

            if (_actionOnComponent == ActionOnComponent.EntityDescription)
            {
                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.ProjectPluginAssemblyCreateEntityDescriptionCommand);
            }
            else if (_actionOnComponent == ActionOnComponent.Description)
            {
                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.ProjectPluginAssemblyCreateDescriptionCommand);
            }
        }
    }
}
