using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand : AbstractDynamicCommandOnSolutionLast
    {
        private FileCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginAssemblyStepsAddToSolutionLastCommandId
            )
        {

        }

        public static FileCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
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
                helper.HandlePluginAssemblyAddingProcessingStepsByProjectCommand(null, solutionUniqueName, false, list);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, string solutionUniqueName, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerItemContainsProjectAny(applicationObject, menuCommand);
        }
    }
}