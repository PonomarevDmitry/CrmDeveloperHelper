using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlSiteMapExplorerCommand : AbstractCommand
    {
        private CodeXmlSiteMapExplorerCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.CodeXmlSiteMapExplorerCommandId) { }

        public static CodeXmlSiteMapExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlSiteMapExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
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

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(applicationObject, menuCommand, out var doc, AbstractDynamicCommandXsdSchemas.RootSiteMap);
        }
    }
}
