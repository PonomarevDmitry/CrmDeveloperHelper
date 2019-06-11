using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class DocumentsCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand : AbstractDynamicCommandAddObjectToSolutionLast
    {
        private DocumentsCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.DocumentsCSharpProjectPluginAssemblyStepsAddToSolutionLastCommandId
                , ActionExecute
                , CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsCSharp
            )
        {

        }

        public static DocumentsCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsCSharpProjectPluginAssemblyStepsAddToSolutionLastCommand(commandService);
        }

        private static void ActionExecute(DTEHelper helper, ConnectionData connectionData, string solutionUniqueName)
        {
            var list = helper
                .GetOpenedDocumentsAsDocument(FileOperations.SupportsCSharpType)
                .Where(i => i.ProjectItem?.ContainingProject != null && !string.IsNullOrEmpty(i.ProjectItem?.ContainingProject?.Name))
                .Select(i => i.ProjectItem.ContainingProject.Name)
                .ToArray()
                ;

            if (list.Any())
            {
                helper.HandleAddingPluginAssemblyProcessingStepsByProjectCommand(null, solutionUniqueName, false, list);
            }
        }
    }
}