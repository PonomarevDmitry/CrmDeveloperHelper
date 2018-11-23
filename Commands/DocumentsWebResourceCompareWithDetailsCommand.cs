using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class DocumentsWebResourceCompareWithDetailsCommand : AbstractCommand
    {
        private DocumentsWebResourceCompareWithDetailsCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.DocumentsWebResourceCompareWithDetailsCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static DocumentsWebResourceCompareWithDetailsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new DocumentsWebResourceCompareWithDetailsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsWebResourceTextType);

            helper.HandleFileCompareCommand(null, selectedFiles, true);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResourceText(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.DocumentsWebResourceCompareWithDetailsCommand);
        }
    }
}