using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class ListForPublishAddIntoSolutionCommand : AbstractCommand
    {
        private ListForPublishAddIntoSolutionCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.ListForPublishAddIntoSolutionCommandId, ActionExecute, ActionBeforeQueryStatus)
        {
        }

        public static ListForPublishAddIntoSolutionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ListForPublishAddIntoSolutionCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesFromListForPublish();
            
            if (selectedFiles.Count > 0)
            {
                helper.ShowListForPublish();

                helper.HandleAddingWebResourcesIntoSolutionCommand(selectedFiles, true, null);
            }
            else
            {
                helper.WriteToOutput(Properties.OutputStrings.PublishListIsEmpty);
                helper.ActivateOutputWindow();
            }
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceAny(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.ListForPublishAddIntoSolutionCommand);
        }
    }
}