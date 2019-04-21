using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileXmlRemoveXsdSchemaCommand : AbstractCommand
    {
        private FileXmlRemoveXsdSchemaCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileXmlRemoveXsdSchemaCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusSolutionExplorerXmlAny) { }

        public static FileXmlRemoveXsdSchemaCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileXmlRemoveXsdSchemaCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var listFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsXmlType, false);

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
