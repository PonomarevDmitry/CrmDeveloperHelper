using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpProjectBuildLoadUpdatePluginAssemblyCommand : AbstractSingleCommand
    {
        private FileCSharpProjectBuildLoadUpdatePluginAssemblyCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.FileCSharpProjectBuildLoadUpdatePluginAssemblyCommandId) { }

        public static FileCSharpProjectBuildLoadUpdatePluginAssemblyCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileCSharpProjectBuildLoadUpdatePluginAssemblyCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var projectItem = helper.GetSingleSelectedProjectItemInSolutionExplorer(FileOperations.SupportsCSharpType);

            if (projectItem != null && projectItem.ContainingProject != null)
            {
                helper.HandlePluginAssemblyBuildProjectUpdateCommand(null, false, projectItem.ContainingProject);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FileCSharpProjectBuildLoadUpdatePluginAssemblyCommand);
        }
    }
}
