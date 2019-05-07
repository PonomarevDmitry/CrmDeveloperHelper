using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileJavaScriptMinifyCommand : AbstractCommand
    {
        private FileJavaScriptMinifyCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileJavaScriptMinifyCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileJavaScriptMinifyCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileJavaScriptMinifyCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsJavaScriptType, false).ToList();

            helper.HandleUpdateEntityMetadataFileJavaScript(null, selectedFiles, true);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerJavaScriptSingle(command, menuCommand);
        }
    }
}
