using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class ListForPublishFilesAddCommand : AbstractCommand
    {
        private ListForPublishFilesAddCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.ListForPublishFilesAddCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static ListForPublishFilesAddCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ListForPublishFilesAddCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var selectedFiles = helper.GetSelectedFilesAll(FileOperations.SupportsWebResourceType, true);

            helper.AddToListForPublish(selectedFiles);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusFilesToAdd(command, menuCommand);
        }
    }
}
