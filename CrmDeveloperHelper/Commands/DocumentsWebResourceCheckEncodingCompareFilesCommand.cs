using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class DocumentsWebResourceCheckEncodingCompareFilesCommand : AbstractCommand
    {
        private DocumentsWebResourceCheckEncodingCompareFilesCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.DocumentsWebResourceCheckEncodingCompareFilesCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResource) { }

        public static DocumentsWebResourceCheckEncodingCompareFilesCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new DocumentsWebResourceCheckEncodingCompareFilesCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsWebResourceTextType);

            helper.HandleCompareFilesWithoutUTF8EncodingCommand(selectedFiles, false);
        }
    }
}
