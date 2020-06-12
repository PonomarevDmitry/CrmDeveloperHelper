using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class FileCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand : AbstractSingleCommand
    {
        private FileCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.FileCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommandId) { }

        public static FileCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var projectItem = helper.GetSingleSelectedProjectItemInSolutionExplorer(FileOperations.SupportsCSharpType);

            if (projectItem != null && projectItem.ContainingProject != null)
            {
                helper.HandlePluginAssemblyBuildProjectUpdateCommand(null, true, projectItem.ContainingProject);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FileCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand);
        }
    }
}
