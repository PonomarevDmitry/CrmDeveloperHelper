using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceAddToSolutionLastCommand
        : AbstractAddObjectToSolutionLastCommand
    {
        private FileWebResourceAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.FileWebResourceAddToSolutionLastCommandId
                , ActionExecute
                , CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceAny
            )
        {

        }

        public static FileWebResourceAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileWebResourceAddToSolutionLastCommand(commandService);
        }

        private static void ActionExecute(DTEHelper helper, ConnectionData connectionData, string solutionUniqueName)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceType, false).ToList();

            helper.HandleAddingWebResourcesToSolutionCommand(null, solutionUniqueName, false, selectedFiles);
        }
    }
}