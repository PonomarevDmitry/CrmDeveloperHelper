using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class ProjectCompareToCrmAssemblyCommand : AbstractSingleCommand
    {
        private ProjectCompareToCrmAssemblyCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.ProjectCompareToCrmAssemblyCommandId) { }

        public static ProjectCompareToCrmAssemblyCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ProjectCompareToCrmAssemblyCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var project = helper.GetSelectedProject();

            helper.HandlePluginAssemblyComparingWithLocalAssemblyCommand(null, project);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActiveSolutionExplorerProjectSingle(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.ProjectCompareToCrmAssemblyCommand);
        }
    }
}