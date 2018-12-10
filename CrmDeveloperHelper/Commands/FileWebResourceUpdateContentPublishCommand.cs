using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileWebResourceUpdateContentPublishCommand : AbstractCommand
    {
        private FileWebResourceUpdateContentPublishCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileWebResourceUpdateContentPublishCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileWebResourceUpdateContentPublishCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileWebResourceUpdateContentPublishCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceType, false);

            helper.HandleUpdateContentWebResourcesAndPublishCommand(null, selectedFiles);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceAny(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileWebResourceUpdateContentPublishCommand);
        }
    }
}