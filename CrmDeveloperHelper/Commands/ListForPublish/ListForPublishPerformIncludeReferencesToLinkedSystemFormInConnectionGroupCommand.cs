using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class ListForPublishPerformIncludeReferencesToLinkedSystemFormInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithCurrent
    {
        private ListForPublishPerformIncludeReferencesToLinkedSystemFormInConnectionGroupCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.ListForPublishPerformIncludeReferencesToLinkedSystemFormInConnectionGroupCommandId)
        {
        }

        public static ListForPublishPerformIncludeReferencesToLinkedSystemFormInConnectionGroupCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ListForPublishPerformIncludeReferencesToLinkedSystemFormInConnectionGroupCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesFromListForPublish(FileOperations.SupportsJavaScriptType).ToList();

            if (selectedFiles.Count > 0)
            {
                helper.ShowListForPublish(connectionData);

                helper.HandleIncludeReferencesToLinkedSystemFormsLibrariesCommand(connectionData, selectedFiles);
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

                CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceJavaScriptAny(applicationObject, menuCommand);
            }
        }
    }
}