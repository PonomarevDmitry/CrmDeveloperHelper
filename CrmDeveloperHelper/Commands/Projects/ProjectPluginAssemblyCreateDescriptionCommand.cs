using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectPluginAssemblyCreateDescriptionCommand : AbstractCommand
    {
        private ProjectPluginAssemblyCreateDescriptionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.ProjectPluginAssemblyCreateDescriptionCommandId)
        {
        }

        public static ProjectPluginAssemblyCreateDescriptionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ProjectPluginAssemblyCreateDescriptionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var projectList = helper.GetSelectedProjects().ToList();

            helper.HandleActionOnProjectPluginAssemblyCommand(null, projectList, Model.ActionOnComponent.Description);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectAny(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.ProjectPluginAssemblyCreateDescriptionCommand);
        }
    }
}
