using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlSiteMapExplorerCommand : AbstractCommand
    {
        private CodeXmlSiteMapExplorerCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeXmlSiteMapExplorerCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeXmlSiteMapExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeXmlSiteMapExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                string siteMapNameUnique = string.Empty;

                string fileText = File.ReadAllText(selectedFiles[0].FilePath);

                if (ContentCoparerHelper.TryParseXml(fileText, out var doc))
                {
                    var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSiteMapNameUnique);

                    if (attribute != null && !string.IsNullOrEmpty(attribute.Value))
                    {
                        siteMapNameUnique = attribute.Value;
                    }
                }

                helper.HandleExplorerSitemap(siteMapNameUnique);
            }
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(command, menuCommand, out var doc, CommonExportXsdSchemasCommand.RootSiteMap);
        }
    }
}
