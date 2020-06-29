using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class DocumentsWebResourceCompareInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithCurrent
    {
        private readonly bool _withDetails;

        private DocumentsWebResourceCompareInConnectionGroupCommand(OleMenuCommandService commandService, int baseIdStart, bool withDetails)
            : base(commandService, baseIdStart)
        {
            this._withDetails = withDetails;
        }

        public static DocumentsWebResourceCompareInConnectionGroupCommand Instance { get; private set; }

        public static DocumentsWebResourceCompareInConnectionGroupCommand InstanceWithDetails { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsWebResourceCompareInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsWebResourceCompareInConnectionGroupCommandId, false);

            InstanceWithDetails = new DocumentsWebResourceCompareInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsWebResourceCompareWithDetailsInConnectionGroupCommandId, true);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsWebResourceTextType).ToList();

            helper.HandleWebResourceCompareCommand(connectionData, selectedFiles, this._withDetails);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResourceText(applicationObject, menuCommand);
        }
    }
}
