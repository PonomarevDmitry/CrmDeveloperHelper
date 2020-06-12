using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpProjectPluginAssemblyCreateDescriptionCommand : AbstractSingleCommand
    {
        private readonly ActionOnComponent _actionOnComponent;

        private CodeCSharpProjectPluginAssemblyCreateDescriptionCommand(OleMenuCommandService commandService, int idCommand, ActionOnComponent actionOnComponent)
            : base(commandService, idCommand)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static CodeCSharpProjectPluginAssemblyCreateDescriptionCommand InstanceEntityDescription { get; private set; }

        public static CodeCSharpProjectPluginAssemblyCreateDescriptionCommand InstanceDescription { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceEntityDescription = new CodeCSharpProjectPluginAssemblyCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.CodeCSharpProjectPluginAssemblyCreateEntityDescriptionCommandId
                , ActionOnComponent.EntityDescription
            );

            InstanceDescription = new CodeCSharpProjectPluginAssemblyCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.CodeCSharpProjectPluginAssemblyCreateDescriptionCommandId
                , ActionOnComponent.Description
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

            if (document != null
                && document.ProjectItem != null
                && document.ProjectItem.ContainingProject != null
            )
            {
                helper.HandleActionOnProjectPluginAssemblyCommand(null, new EnvDTE.Project[] { document.ProjectItem.ContainingProject }, _actionOnComponent);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentContainingProject(applicationObject, menuCommand);

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
