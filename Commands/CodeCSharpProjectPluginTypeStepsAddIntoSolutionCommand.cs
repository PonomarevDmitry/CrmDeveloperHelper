using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeCSharpProjectPluginTypeStepsAddIntoSolutionCommand : AbstractCommand
    {
        private CodeCSharpProjectPluginTypeStepsAddIntoSolutionCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeCSharpProjectPluginTypeStepsAddIntoSolutionCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeCSharpProjectPluginTypeStepsAddIntoSolutionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeCSharpProjectPluginTypeStepsAddIntoSolutionCommand(package);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentContainingProject(command, menuCommand, FileOperations.SupportsCSharpType);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, "Add Steps for PluginType into Crm Solution");
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsCSharpType);

            var file = selectedFiles.FirstOrDefault();

            if (file != null)
            {
                string selection = file.Name.Split('.').FirstOrDefault();

                helper.HandleAddingPluginTypeProcessingStepsByProjectCommand(null, true, selection);
            }
        }
    }
}