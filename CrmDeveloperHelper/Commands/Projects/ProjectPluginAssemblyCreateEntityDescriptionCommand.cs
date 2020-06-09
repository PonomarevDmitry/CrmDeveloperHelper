using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectPluginAssemblyCreateEntityDescriptionCommand : AbstractCommand
    {
        private ProjectPluginAssemblyCreateEntityDescriptionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.ProjectPluginAssemblyCreateEntityDescriptionCommandId)
        {
        }

        public static ProjectPluginAssemblyCreateEntityDescriptionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ProjectPluginAssemblyCreateEntityDescriptionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var projectList = helper.GetSelectedProjects().ToList();

            helper.HandleActionOnProjectPluginAssemblyCommand(null, projectList, Model.ActionOnComponent.EntityDescription);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectAny(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.ProjectPluginAssemblyCreateEntityDescriptionCommand);
        }
    }
}
