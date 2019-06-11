using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class ListForPublishAddToSolutionInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private ListForPublishAddToSolutionInConnectionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.ListForPublishAddToSolutionInConnectionCommandId
            )
        {

        }

        public static ListForPublishAddToSolutionInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ListForPublishAddToSolutionInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesFromListForPublish().ToList();

            if (selectedFiles.Count > 0)
            {
                helper.ShowListForPublish(connectionData);

                helper.HandleAddingWebResourcesToSolutionCommand(connectionData, null, true, selectedFiles);
            }
            else
            {
                helper.WriteToOutput(connectionData, Properties.OutputStrings.PublishListIsEmpty);
                helper.ActivateOutputWindow(connectionData);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceAny(applicationObject, menuCommand);
        }
    }
}
