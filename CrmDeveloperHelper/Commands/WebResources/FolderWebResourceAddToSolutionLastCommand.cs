using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FolderWebResourceAddToSolutionLastCommand : AbstractDynamicCommandAddObjectToSolutionLast
    {
        private FolderWebResourceAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.FolderWebResourceAddToSolutionLastCommandId
                , ActionExecute
                , CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceRecursive
            )
        {

        }

        public static FolderWebResourceAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderWebResourceAddToSolutionLastCommand(commandService);
        }

        private static void ActionExecute(DTEHelper helper, ConnectionData connectionData, string solutionUniqueName)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, true).ToList();

            helper.HandleAddingWebResourcesToSolutionCommand(null, solutionUniqueName, false, selectedFiles);
        }
    }
}