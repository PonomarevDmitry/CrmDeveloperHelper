using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodePublishListAddCommand : AbstractCommand
    {
        private CodePublishListAddCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodePublishListAddCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodePublishListAddCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodePublishListAddCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsWebResourceType);

            helper.AddToListForPublish(selectedFiles);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResource(command, menuCommand);
        }
    }
}
