using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceCompareWithDetailsCommand : AbstractCommand
    {
        private FileWebResourceCompareWithDetailsCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileWebResourceCompareWithDetailsCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileWebResourceCompareWithDetailsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileWebResourceCompareWithDetailsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, false).ToList();

            helper.HandleFileCompareCommand(null, selectedFiles, true);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextAny(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileWebResourceCompareWithDetailsCommand);
        }
    }
}