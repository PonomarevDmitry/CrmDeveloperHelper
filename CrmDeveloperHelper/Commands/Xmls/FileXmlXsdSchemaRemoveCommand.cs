using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class FileXmlXsdSchemaRemoveCommand : AbstractSingleCommand
    {
        private FileXmlXsdSchemaRemoveCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.FileXmlXsdSchemaRemoveCommandId) { }

        public static FileXmlXsdSchemaRemoveCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileXmlXsdSchemaRemoveCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var listFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsXmlType, false);

            if (listFiles.Any())
            {
                foreach (var document in listFiles.Where(s => s.Document != null && s.Document.ActiveWindow != null && s.Document.ActiveWindow.Visible).Select(s => s.Document))
                {
                    ContentComparerHelper.RemoveXsdSchemaInDocument(document);
                }

                foreach (var filePath in listFiles.Where(s => !(s.Document != null && s.Document.ActiveWindow != null && s.Document.ActiveWindow.Visible)).Select(s => s.FilePath))
                {
                    ContentComparerHelper.RemoveXsdSchemaInFile(filePath);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerXmlAny(applicationObject, menuCommand);
        }
    }
}
