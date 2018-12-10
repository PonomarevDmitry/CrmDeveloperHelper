using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class ListForPublishPerformPublishCommand : AbstractCommand
    {
        private ListForPublishPerformPublishCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.ListForPublishPerformPublishCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static ListForPublishPerformPublishCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ListForPublishPerformPublishCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesFromListForPublish();

            if (selectedFiles.Count > 0)
            {
                helper.ShowListForPublish();

                helper.HandleUpdateContentWebResourcesAndPublishCommand(selectedFiles);
            }
            else
            {
                helper.WriteToOutput("Publish List is empty.");
                helper.ActivateOutputWindow();
            }
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceAny(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, "Publish Files");
        }
    }
}
