using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class DocumentsWebResouceLinkClearCommand : AbstractCommand
    {
        private DocumentsWebResouceLinkClearCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.DocumentsWebResourceLinkClearCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResource) { }

        public static DocumentsWebResouceLinkClearCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new DocumentsWebResouceLinkClearCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsWebResourceType);

            helper.HandleFileClearLink(selectedFiles);
        }
    }
}
