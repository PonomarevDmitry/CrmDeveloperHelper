using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class DocumentsWebResourceUpdateContentPublishEqualByTextCommand : AbstractCommand
    {
        private DocumentsWebResourceUpdateContentPublishEqualByTextCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.DocumentsWebResourceUpdateContentPublishEqualByTextCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static DocumentsWebResourceUpdateContentPublishEqualByTextCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new DocumentsWebResourceUpdateContentPublishEqualByTextCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsWebResourceTextType);

            helper.HandleUpdateContentWebResourcesAndPublishEqualByTextCommand(selectedFiles);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResourceText(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, "Publish Files equal by Text");
        }
    }
}
