using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Projects
{
    internal sealed class FileCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private FileCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommandId
            )
        {

        }

        public static FileCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var projectItem = helper.GetSingleSelectedProjectItemInSolutionExplorer(FileOperations.SupportsCSharpType);

            if (projectItem != null && projectItem.ContainingProject != null)
            {
                helper.HandlePluginAssemblyUpdatingInWindowCommand(connectionData, projectItem.ContainingProject);
                helper.HandlePluginAssemblyBuildProjectUpdateCommand(connectionData, true, projectItem.ContainingProject);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(applicationObject, menuCommand);
        }
    }
}
