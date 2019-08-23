using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class DocumentsWebResourceCheckEncodingCompareWithDetailsFilesCommand : AbstractCommand
    {
        private DocumentsWebResourceCheckEncodingCompareWithDetailsFilesCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.DocumentsWebResourceCheckEncodingCompareWithDetailsFilesCommandId) { }

        public static DocumentsWebResourceCheckEncodingCompareWithDetailsFilesCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsWebResourceCheckEncodingCompareWithDetailsFilesCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsWebResourceTextType).ToList();

            helper.HandleCompareFilesWithoutUTF8EncodingCommand(selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResourceText(applicationObject, menuCommand);
        }
    }
}
