using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodePublishListRemoveCommand : AbstractCommand
    {
        private CodePublishListRemoveCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodePublishListRemoveCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodePublishListRemoveCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodePublishListRemoveCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsWebResourceType);

            helper.RemoveFromListForPublish(selectedFiles);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResource(command, menuCommand);
        }
    }
}
