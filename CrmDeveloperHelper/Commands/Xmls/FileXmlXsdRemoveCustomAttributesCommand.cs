using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class FileXmlXsdRemoveCustomAttributesCommand : AbstractSingleCommand
    {
        private FileXmlXsdRemoveCustomAttributesCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.FileXmlRemoveCustomAttributesCommandId)
        {
        }

        public static FileXmlXsdRemoveCustomAttributesCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileXmlXsdRemoveCustomAttributesCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var listFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsXmlType, false);

            if (listFiles.Any())
            {
                foreach (var document in listFiles.Where(s => s.Document != null && s.Document.ActiveWindow != null && s.Document.ActiveWindow.Visible).Select(s => s.Document))
                {
                    ContentComparerHelper.RemoveAllCustomAttributesInDocument(document);
                }

                foreach (var filePath in listFiles.Where(s => !(s.Document != null && s.Document.ActiveWindow != null && s.Document.ActiveWindow.Visible)).Select(s => s.FilePath))
                {
                    ContentComparerHelper.RemoveAllCustomAttributesInFile(filePath);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerXmlAny(applicationObject, menuCommand);
        }
    }
}
