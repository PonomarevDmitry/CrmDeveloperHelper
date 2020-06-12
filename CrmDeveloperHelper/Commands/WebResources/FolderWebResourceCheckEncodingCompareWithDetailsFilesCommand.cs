using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FolderWebResourceCheckEncodingCompareWithDetailsFilesCommand : AbstractSingleCommand
    {
        private FolderWebResourceCheckEncodingCompareWithDetailsFilesCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.FolderWebResourceCheckEncodingCompareWithDetailsFilesCommandId) { }

        public static FolderWebResourceCheckEncodingCompareWithDetailsFilesCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderWebResourceCheckEncodingCompareWithDetailsFilesCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceType, true).ToList();

            helper.HandleWebResourceCompareFilesWithoutUTF8EncodingCommand(selectedFiles, true);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceRecursive(applicationObject, menuCommand);
        }
    }
}
