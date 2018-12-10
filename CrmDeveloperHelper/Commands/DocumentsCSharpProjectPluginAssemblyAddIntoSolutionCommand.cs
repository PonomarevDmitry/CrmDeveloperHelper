using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class DocumentsCSharpProjectPluginAssemblyAddIntoSolutionCommand : AbstractCommand
    {
        private DocumentsCSharpProjectPluginAssemblyAddIntoSolutionCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.DocumentsCSharpProjectPluginAssemblyAddIntoSolutionCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static DocumentsCSharpProjectPluginAssemblyAddIntoSolutionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new DocumentsCSharpProjectPluginAssemblyAddIntoSolutionCommand(package);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsCSharp(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.DocumentsCSharpProjectPluginAssemblyAddIntoSolutionCommand);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var list = helper.GetOpenedDocumentsAsDocument(FileOperations.SupportsCSharpType)
                                .Where(i => i.ProjectItem?.ContainingProject != null && !string.IsNullOrEmpty(i.ProjectItem?.ContainingProject?.Name))
                                .Select(i => i.ProjectItem.ContainingProject.Name);

            if (list.Any())
            {
                helper.HandleAddingPluginAssemblyIntoSolutionByProjectCommand(null, true, list.ToArray());
            }
        }
    }
}