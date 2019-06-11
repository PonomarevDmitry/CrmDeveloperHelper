using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class ListForPublishPerformPublishGroupConnectionCommand : AbstractDynamicCommandByConnectionByGroupWithCurrent
    {
        private ListForPublishPerformPublishGroupConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.ListForPublishPerformPublishGroupConnectionCommandId
            )
        {

        }

        public static ListForPublishPerformPublishGroupConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ListForPublishPerformPublishGroupConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesFromListForPublish().ToList();

            if (selectedFiles.Count > 0)
            {
                helper.ShowListForPublish(connectionData);

                helper.HandleUpdateContentWebResourcesAndPublishCommand(connectionData, selectedFiles);
            }
            else
            {
                helper.WriteToOutput(connectionData, Properties.OutputStrings.PublishListIsEmpty);
                helper.ActivateOutputWindow(connectionData);
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

                CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceAny(applicationObject, menuCommand);
            }
        }
    }
}