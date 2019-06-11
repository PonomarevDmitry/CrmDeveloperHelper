using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlUpdateSiteMapCommand : AbstractCommand
    {
        private CodeXmlUpdateSiteMapCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.CodeXmlUpdateSiteMapCommandId) { }

        public static CodeXmlUpdateSiteMapCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlUpdateSiteMapCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleSiteMapUpdateCommand(null, selectedFiles.FirstOrDefault());
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(applicationObject, menuCommand, out var doc, AbstractDynamicCommandXsdSchemas.RootSiteMap);

            if (doc != null)
            {
                string nameCommand = Properties.CommandNames.CodeXmlUpdateSiteMapCommand;

                var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSiteMapNameUnique);

                if (attribute != null && !string.IsNullOrEmpty(attribute.Value))
                {
                    nameCommand = string.Format(Properties.CommandNames.CodeXmlUpdateSiteMapByNameCommandFormat1, attribute.Value);
                }

                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, nameCommand);
            }
        }
    }
}
