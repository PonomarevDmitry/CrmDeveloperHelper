using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class DocumentsWebResourceAddToSolutionInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private DocumentsWebResourceAddToSolutionInConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsWebResourceAddToSolutionInConnectionCommandId
            )
        {

        }

        public static DocumentsWebResourceAddToSolutionInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsWebResourceAddToSolutionInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsWebResourceType).ToList();

            helper.HandleWebResourceAddingToSolutionCommand(connectionData, null, true, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResource(applicationObject, menuCommand);
        }
    }
}
