using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class DocumentsWebResourceCheckEncodingCompareFilesCommand : AbstractSingleCommand
    {
        private DocumentsWebResourceCheckEncodingCompareFilesCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.DocumentsWebResourceCheckEncodingCompareFilesCommandId) { }

        public static DocumentsWebResourceCheckEncodingCompareFilesCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsWebResourceCheckEncodingCompareFilesCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsWebResourceTextType).ToList();

            helper.HandleWebResourceCompareFilesWithoutUTF8EncodingCommand(selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResource(applicationObject, menuCommand);
        }
    }
}
