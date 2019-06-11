using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlShowDifferenceSiteMapCommand : AbstractCommand
    {
        private CodeXmlShowDifferenceSiteMapCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.CodeXmlShowDifferenceSiteMapCommandId) { }

        public static CodeXmlShowDifferenceSiteMapCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlShowDifferenceSiteMapCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleSiteMapDifferenceCommand(null, selectedFiles.FirstOrDefault());
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(applicationObject, menuCommand, out var doc, AbstractDynamicCommandXsdSchemas.RootSiteMap);

            if (doc != null)
            {
                string nameCommand = Properties.CommandNames.CodeXmlShowDifferenceSiteMapCommand;

                var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSiteMapNameUnique);

                if (attribute != null && !string.IsNullOrEmpty(attribute.Value))
                {
                    nameCommand = string.Format(Properties.CommandNames.CodeXmlShowDifferenceSiteMapByNameCommandFormat1, attribute.Value);
                }

                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, nameCommand);
            }
        }
    }
}