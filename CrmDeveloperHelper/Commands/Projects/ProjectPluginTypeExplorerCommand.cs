using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectPluginTypeExplorerCommand : AbstractCommand
    {
        private ProjectPluginTypeExplorerCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.ProjectPluginTypeExplorerCommandId)
        {
        }

        public static ProjectPluginTypeExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ProjectPluginTypeExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var projects = helper.GetSelectedProjects();

            if (projects.Any())
            {
                helper.HandleOpenPluginTypeExplorer(projects.First().Name);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectAny(applicationObject, menuCommand);
        }
    }
}