using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class ListForPublishFilesRemoveCommand : AbstractCommand
    {
        private ListForPublishFilesRemoveCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.ListForPublishFilesRemoveCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static ListForPublishFilesRemoveCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ListForPublishFilesRemoveCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsWebResourceType);

            helper.RemoveFromListForPublish(selectedFiles);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusFilesToAdd(command, menuCommand);
        }
    }
}
