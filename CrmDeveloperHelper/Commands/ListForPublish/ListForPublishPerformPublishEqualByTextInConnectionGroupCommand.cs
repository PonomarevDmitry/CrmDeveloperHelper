using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class ListForPublishPerformPublishEqualByTextInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithCurrent
    {
        private ListForPublishPerformPublishEqualByTextInConnectionGroupCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.ListForPublishPerformPublishEqualByTextInConnectionGroupCommandId)
        {
        }

        public static ListForPublishPerformPublishEqualByTextInConnectionGroupCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ListForPublishPerformPublishEqualByTextInConnectionGroupCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (connectionData.IsReadOnly)
            {
                helper.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsReadOnlyFormat1, connectionData.Name);
                return;
            }

            List<SelectedFile> selectedFiles = helper.GetSelectedFilesFromListForPublish(FileOperations.SupportsWebResourceTextType).ToList();

            if (selectedFiles.Count > 0)
            {
                helper.ShowListForPublish(connectionData);

                helper.HandleUpdateContentWebResourcesAndPublishEqualByTextCommand(connectionData, selectedFiles);
            }
            else
            {
                helper.WriteToOutput(connectionData, Properties.OutputStrings.PublishListIsEmpty);
                helper.ActivateOutputWindow(connectionData);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceTextAny(applicationObject, menuCommand);
        }
    }
}