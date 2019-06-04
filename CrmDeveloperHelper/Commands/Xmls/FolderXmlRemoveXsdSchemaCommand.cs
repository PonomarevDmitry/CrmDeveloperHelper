using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class FolderXmlRemoveXsdSchemaCommand : AbstractCommand
    {
        private FolderXmlRemoveXsdSchemaCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FolderXmlRemoveXsdSchemaCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerXmlRecursive) { }

        public static FolderXmlRemoveXsdSchemaCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FolderXmlRemoveXsdSchemaCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var listFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsXmlType, true).ToList();

            if (listFiles.Any())
            {
                foreach (var document in listFiles.Where(s => s.Document != null && s.Document.ActiveWindow != null && s.Document.ActiveWindow.Visible).Select(s => s.Document))
                {
                    ContentCoparerHelper.RemoveXsdSchemaInDocument(document);
                }

                foreach (var filePath in listFiles.Where(s => !(s.Document != null && s.Document.ActiveWindow != null && s.Document.ActiveWindow.Visible)).Select(s => s.FilePath))
                {
                    ContentCoparerHelper.RemoveXsdSchemaInFile(filePath);
                }
            }
        }
    }
}
