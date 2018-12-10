using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileCSharpUpdateGlobalOptionSetsFileWithSelectCommand : AbstractCommand
    {
        private FileCSharpUpdateGlobalOptionSetsFileWithSelectCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileCSharpUpdateGlobalOptionSetsFileWithSelectCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle) { }

        public static FileCSharpUpdateGlobalOptionSetsFileWithSelectCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileCSharpUpdateGlobalOptionSetsFileWithSelectCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsCSharpType, false);

            helper.HandleUpdateGlobalOptionSetsFile(selectedFiles, true);
        }
    }
}
