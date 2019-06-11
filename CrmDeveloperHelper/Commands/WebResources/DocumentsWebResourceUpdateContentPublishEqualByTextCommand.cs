using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class DocumentsWebResourceUpdateContentPublishEqualByTextCommand : AbstractDynamicCommandByConnectionByGroupWithCurrent
    {
        private DocumentsWebResourceUpdateContentPublishEqualByTextCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.DocumentsWebResourceUpdateContentPublishEqualByTextCommandId
            )
        {

        }

        public static DocumentsWebResourceUpdateContentPublishEqualByTextCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsWebResourceUpdateContentPublishEqualByTextCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (!connectionData.IsReadOnly)
            {
                List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsWebResourceTextType).ToList();

                helper.HandleUpdateContentWebResourcesAndPublishEqualByTextCommand(connectionData, selectedFiles);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            if (connectionData.IsReadOnly)
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
            else
            {
                menuCommand.Enabled = menuCommand.Visible = true;

                CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResourceText(applicationObject, menuCommand);
            }
        }
    }
}