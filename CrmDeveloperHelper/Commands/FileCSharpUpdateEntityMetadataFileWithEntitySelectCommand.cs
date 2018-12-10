using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileCSharpUpdateEntityMetadataFileWithEntitySelectCommand : AbstractCommand
    {
        private FileCSharpUpdateEntityMetadataFileWithEntitySelectCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileCSharpUpdateEntityMetadataFileWithEntitySelectCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle) { }

        public static FileCSharpUpdateEntityMetadataFileWithEntitySelectCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileCSharpUpdateEntityMetadataFileWithEntitySelectCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsCSharpType, false);

            helper.HandleUpdateEntityMetadataFile(selectedFiles, true);
        }
    }
}
