using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class FileXmlXsdSchemaSetCommand : AbstractDynamicCommandXsdSchemas
    {
        private FileXmlXsdSchemaSetCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.FileXmlXsdSchemaSetCommandId)
        {

        }

        public static FileXmlXsdSchemaSetCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileXmlXsdSchemaSetCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, Tuple<string, string[]> schemas)
        {
            var listFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsXmlType, false);

            if (listFiles.Any())
            {
                foreach (var document in listFiles.Where(s => s.Document != null && s.Document.ActiveWindow != null && s.Document.ActiveWindow.Visible).Select(s => s.Document))
                {
                    ContentComparerHelper.ReplaceXsdSchemaInDocument(document, schemas.Item2);
                }

                foreach (var filePath in listFiles.Where(s => !(s.Document != null && s.Document.ActiveWindow != null && s.Document.ActiveWindow.Visible)).Select(s => s.FilePath))
                {
                    ContentComparerHelper.ReplaceXsdSchemaInFile(filePath, schemas.Item2);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, Tuple<string, string[]> schemas, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerXmlAny(applicationObject, menuCommand);
        }
    }
}
