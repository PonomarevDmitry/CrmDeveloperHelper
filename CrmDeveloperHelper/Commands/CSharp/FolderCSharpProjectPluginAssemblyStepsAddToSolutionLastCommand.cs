using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FolderCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand : AbstractDynamicCommandAddObjectToSolutionLast
    {
        private FolderCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.FolderCSharpProjectPluginAssemblyStepsAddToSolutionLastCommandId
                , ActionExecute
                , ActionBeforeQueryStatus
            )
        {

        }

        public static FolderCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand(commandService);
        }

        private static void ActionExecute(DTEHelper helper, ConnectionData connectionData, string solutionUniqueName)
        {
            var list = helper
                .GetSelectedProjectItemsInSolutionExplorer(FileOperations.SupportsCSharpType, true)
                .Where(i => i.ContainingProject != null && !string.IsNullOrEmpty(i.ContainingProject?.Name))
                .Select(i => i.ContainingProject.Name)
                .Distinct(StringComparer.InvariantCultureIgnoreCase)
                .ToArray()
                ;

            if (list.Any())
            {
                helper.HandleAddingPluginAssemblyProcessingStepsByProjectCommand(null, solutionUniqueName, false, list);
            }
        }

        private static void ActionBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerAnyItemContainsProject(applicationObject, menuCommand, true);
        }
    }
}
