using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectPluginAssemblyExplorerCommand : AbstractCommand
    {
        private ProjectPluginAssemblyExplorerCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.ProjectPluginAssemblyExplorerCommandId)
        {
        }

        public static ProjectPluginAssemblyExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ProjectPluginAssemblyExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var projects = helper.GetSelectedProjects();

            if (projects.Any())
            {
                helper.HandleOpenPluginAssemblyExplorer(projects.First().Name);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectAny(applicationObject, menuCommand);
        }
    }
}