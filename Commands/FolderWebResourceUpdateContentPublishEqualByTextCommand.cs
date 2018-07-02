using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FolderWebResourceUpdateContentPublishEqualByTextCommand : AbstractCommand
    {
        private FolderWebResourceUpdateContentPublishEqualByTextCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FolderWebResourceUpdateContentPublishEqualByTextCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FolderWebResourceUpdateContentPublishEqualByTextCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FolderWebResourceUpdateContentPublishEqualByTextCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, true);

            helper.HandleUpdateContentWebResourcesAndPublishEqualByTextCommand(selectedFiles);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextRecursive(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, "Publish Files equal by Text (Recursive)");
        }
    }
}
