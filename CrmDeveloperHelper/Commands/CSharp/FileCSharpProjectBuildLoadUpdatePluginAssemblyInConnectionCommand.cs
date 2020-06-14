using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommand : AbstractDynamicCommandByConnectionAllWithoutCurrent
    {
        private FileCSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.FileCSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommandId)
        {
        }

        public static FileCSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileCSharpProjectBuildLoadUpdatePluginAssemblyInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var projectItem = helper.GetSingleSelectedProjectItemInSolutionExplorer(FileOperations.SupportsCSharpType);

            if (projectItem != null && projectItem.ContainingProject != null)
            {
                helper.HandlePluginAssemblyUpdatingInWindowCommand(connectionData, projectItem.ContainingProject);
                helper.HandlePluginAssemblyBuildProjectUpdateCommand(connectionData, false, projectItem.ContainingProject);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(applicationObject, menuCommand);
        }
    }
}