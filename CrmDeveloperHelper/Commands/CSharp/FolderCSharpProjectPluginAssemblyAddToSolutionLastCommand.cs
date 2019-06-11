using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FolderCSharpProjectPluginAssemblyAddToSolutionLastCommand : AbstractDynamicCommandAddObjectToSolutionLast
    {
        private FolderCSharpProjectPluginAssemblyAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.FolderCSharpProjectPluginAssemblyAddToSolutionLastCommandId
                , ActionExecute
                , ActionBeforeQueryStatus
            )
        {

        }

        public static FolderCSharpProjectPluginAssemblyAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderCSharpProjectPluginAssemblyAddToSolutionLastCommand(commandService);
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
                helper.HandleAddingPluginAssemblyToSolutionByProjectCommand(null, solutionUniqueName, false, list);
            }
        }

        private static void ActionBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerAnyItemContainsProject(applicationObject, menuCommand, true);
        }
    }
}
