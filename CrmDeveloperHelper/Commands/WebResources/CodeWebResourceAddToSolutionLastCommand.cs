using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class CodeWebResourceAddToSolutionLastCommand : AbstractAddObjectToSolutionLastCommand
    {
        private CodeWebResourceAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CodeWebResourceAddToSolutionLastCommandId
                , ActionExecute
                , CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResource
            )
        {

        }

        public static CodeWebResourceAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeWebResourceAddToSolutionLastCommand(commandService);
        }

        private static void ActionExecute(DTEHelper helper, ConnectionData connectionData, string solutionUniqueName)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsWebResourceType).ToList();

            helper.HandleAddingWebResourcesToSolutionCommand(null, solutionUniqueName, false, selectedFiles);
        }
    }
}