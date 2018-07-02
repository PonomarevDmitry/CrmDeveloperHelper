using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class ListForPublishPerformPublishEqualByTextCommand : AbstractCommand
    {
        private ListForPublishPerformPublishEqualByTextCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.ListForPublishPerformPublishEqualByTextCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static ListForPublishPerformPublishEqualByTextCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ListForPublishPerformPublishEqualByTextCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesFromListForPublish();

            if (selectedFiles.Count > 0)
            {
                helper.ShowListForPublish();

                helper.HandleUpdateContentWebResourcesAndPublishEqualByTextCommand(selectedFiles);
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

            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceTextAny(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, "Publish Files equal by Text");
        }
    }
}
