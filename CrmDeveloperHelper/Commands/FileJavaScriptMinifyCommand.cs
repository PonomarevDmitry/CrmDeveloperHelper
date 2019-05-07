using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileJavaScriptMinifyCommand : AbstractCommand
    {
        private FileJavaScriptMinifyCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileJavaScriptMinifyCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSupportsMinificationAny) { }

        public static FileJavaScriptMinifyCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileJavaScriptMinifyCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var selectedFiles = helper.GetSelectedProjectItemsInSolutionExplorer(FileOperations.SupportsMinification, false).ToList();

            helper.MinifyDocuments(selectedFiles);
        }
    }
}