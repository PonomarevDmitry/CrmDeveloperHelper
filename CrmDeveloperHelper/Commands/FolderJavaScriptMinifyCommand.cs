using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FolderJavaScriptMinifyCommand : AbstractCommand
    {
        private FolderJavaScriptMinifyCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FolderJavaScriptMinifyCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSupportsMinificationRecursive) { }

        public static FolderJavaScriptMinifyCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FolderJavaScriptMinifyCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var selectedFiles = helper.GetSelectedProjectItemsInSolutionExplorer(FileOperations.SupportsMinification, true).ToList();

            helper.MinifyDocuments(selectedFiles);
        }
    }
}