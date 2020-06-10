using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectPluginAssemblyCreateDescriptionCommand : AbstractCommand
    {
        private readonly ActionOnComponent _actionOnComponent;

        private ProjectPluginAssemblyCreateDescriptionCommand(OleMenuCommandService commandService, int idCommand, ActionOnComponent actionOnComponent)
            : base(commandService, idCommand)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static ProjectPluginAssemblyCreateDescriptionCommand InstanceEntityDescription { get; private set; }

        public static ProjectPluginAssemblyCreateDescriptionCommand InstanceDescription { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceEntityDescription = new ProjectPluginAssemblyCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.ProjectPluginAssemblyCreateEntityDescriptionCommandId
                , ActionOnComponent.EntityDescription
            );

            InstanceDescription = new ProjectPluginAssemblyCreateDescriptionCommand(
                commandService
                , PackageIds.guidCommandSet.ProjectPluginAssemblyCreateDescriptionCommandId
                , ActionOnComponent.Description
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var projectList = helper.GetSelectedProjects().ToList();

            helper.HandleActionOnProjectPluginAssemblyCommand(null, projectList, _actionOnComponent);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectAny(applicationObject, menuCommand);

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
