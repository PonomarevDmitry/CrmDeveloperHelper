using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileWebResourceCompareCommand : AbstractCommand
    {
        private FileWebResourceCompareCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileWebResourceCompareCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileWebResourceCompareCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileWebResourceCompareCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceType, false).ToList();

            helper.HandleFileCompareCommand(null, selectedFiles, false);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceAny(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileWebResourceCompareCommand);
        }
    }
}