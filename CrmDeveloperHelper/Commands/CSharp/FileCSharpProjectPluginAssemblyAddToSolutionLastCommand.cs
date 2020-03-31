using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpProjectPluginAssemblyAddToSolutionLastCommand : AbstractDynamicCommandOnSolutionLast
    {
        private FileCSharpProjectPluginAssemblyAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginAssemblyAddToSolutionLastCommandId
            )
        {

        }

        public static FileCSharpProjectPluginAssemblyAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileCSharpProjectPluginAssemblyAddToSolutionLastCommand(commandService);
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
                helper.HandlePluginAssemblyAddingToSolutionByProjectCommand(null, solutionUniqueName, false, list);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, string solutionUniqueName, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerItemContainsProjectAny(applicationObject, menuCommand);
        }
    }
}