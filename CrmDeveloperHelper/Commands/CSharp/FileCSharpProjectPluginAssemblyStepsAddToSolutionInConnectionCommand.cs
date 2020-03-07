using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private FileCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommandId
            )
        {

        }

        public static FileCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileCSharpProjectPluginAssemblyStepsAddToSolutionInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var listProjects = helper
                   .GetSelectedProjectItemsInSolutionExplorer(FileOperations.SupportsCSharpType, false)
                   .Where(i => i.ContainingProject != null && !string.IsNullOrEmpty(i.ContainingProject?.Name))
                   .Select(i => i.ContainingProject.Name)
                   .Distinct(StringComparer.InvariantCultureIgnoreCase)
                   .ToArray()
                   ;

            if (listProjects.Any())
            {
                helper.HandleAddingPluginAssemblyProcessingStepsByProjectCommand(connectionData, null, true, listProjects);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerItemContainsProjectAny(applicationObject, menuCommand);
        }
    }
}