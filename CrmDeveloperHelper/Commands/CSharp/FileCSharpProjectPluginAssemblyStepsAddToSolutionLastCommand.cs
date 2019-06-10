using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand : AbstractAddObjectToSolutionLastCommand
    {
        private FileCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.FileCSharpProjectPluginAssemblyStepsAddToSolutionLastCommandId
                , ActionExecute
                , ActionBeforeQueryStatus
            )
        {

        }

        public static FileCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand(commandService);
        }

        private static void ActionExecute(DTEHelper helper, ConnectionData connectionData, string solutionUniqueName)
        {
            var list = helper
                .GetSelectedProjectItemsInSolutionExplorer(FileOperations.SupportsCSharpType, false)
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
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerAnyItemContainsProject(applicationObject, menuCommand, false);
        }
    }
}