using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class DocumentsCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand : AbstractDynamicCommandOnSolutionLast
    {
        private DocumentsCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginAssemblyStepsAddToSolutionLastCommandId
            )
        {

        }

        public static DocumentsCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            var list = helper
                .GetOpenedDocumentsAsDocument(FileOperations.SupportsCSharpType)
                .Where(i => i.ProjectItem?.ContainingProject != null && !string.IsNullOrEmpty(i.ProjectItem?.ContainingProject?.Name))
                .Select(i => i.ProjectItem.ContainingProject.Name)
                .ToArray()
                ;

            if (list.Any())
            {
                helper.HandlePluginAssemblyAddingProcessingStepsByProjectCommand(null, solutionUniqueName, false, list);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, string solutionUniqueName, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsCSharp(applicationObject, menuCommand);
        }
    }
}