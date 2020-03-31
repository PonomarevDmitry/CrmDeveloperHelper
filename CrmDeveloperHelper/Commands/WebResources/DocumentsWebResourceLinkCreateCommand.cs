using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class DocumentsWebResourceLinkCreateCommand : AbstractCommand
    {
        private DocumentsWebResourceLinkCreateCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.DocumentsWebResourceLinkCreateCommandId) { }

        public static DocumentsWebResourceLinkCreateCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsWebResourceLinkCreateCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsWebResourceType).ToList();

            helper.HandleWebResourceCreateLaskLinkMultipleCommand(selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResource(applicationObject, menuCommand);
        }
    }
}
