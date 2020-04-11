using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class CodeWebResourceUpdateContentPublishInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithoutCurrent
    {
        private CodeWebResourceUpdateContentPublishInConnectionGroupCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CodeWebResourceUpdateContentPublishInConnectionGroupCommandId)
        {
        }

        public static CodeWebResourceUpdateContentPublishInConnectionGroupCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeWebResourceUpdateContentPublishInConnectionGroupCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (!connectionData.IsReadOnly)
            {
                List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsWebResourceType).ToList();

                helper.HandleUpdateContentWebResourcesAndPublishCommand(connectionData, selectedFiles);
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

                CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResource(applicationObject, menuCommand);
            }
        }
    }
}