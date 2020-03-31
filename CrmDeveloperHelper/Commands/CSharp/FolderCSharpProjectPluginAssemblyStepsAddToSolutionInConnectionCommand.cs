using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FolderCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private FolderCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommandId
            )
        {

        }

        public static FolderCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var listProjects = helper
                .GetSelectedProjectItemsInSolutionExplorer(FileOperations.SupportsCSharpType, true)
                .Where(i => i.ContainingProject != null && !string.IsNullOrEmpty(i.ContainingProject?.Name))
                .Select(i => i.ContainingProject.Name)
                .Distinct(StringComparer.InvariantCultureIgnoreCase)
                .ToArray()
                ;

            if (listProjects.Any())
            {
                helper.HandlePluginAssemblyAddingProcessingStepsByProjectCommand(connectionData, null, true, listProjects);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerItemContainsProjectRecursive(applicationObject, menuCommand);
        }
    }
}
