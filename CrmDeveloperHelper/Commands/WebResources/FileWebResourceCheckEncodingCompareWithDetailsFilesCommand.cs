using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceCheckEncodingCompareWithDetailsFilesCommand : AbstractSingleCommand
    {
        private FileWebResourceCheckEncodingCompareWithDetailsFilesCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.FileWebResourceCheckEncodingCompareWithDetailsFilesCommandId) { }

        public static FileWebResourceCheckEncodingCompareWithDetailsFilesCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileWebResourceCheckEncodingCompareWithDetailsFilesCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, false).ToList();

            helper.HandleWebResourceCompareFilesWithoutUTF8EncodingCommand(selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextAny(applicationObject, menuCommand);
        }
    }
}
