using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceCheckEncodingOpenFilesCommand : AbstractSingleCommand
    {
        private FileWebResourceCheckEncodingOpenFilesCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.FileWebResourceCheckEncodingOpenFilesCommandId) { }

        public static FileWebResourceCheckEncodingOpenFilesCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileWebResourceCheckEncodingOpenFilesCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, false).ToList();

            helper.HandleWebResourceCheckOpenFilesWithoutUTF8EncodingCommand(selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextAny(applicationObject, menuCommand);
        }
    }
}
