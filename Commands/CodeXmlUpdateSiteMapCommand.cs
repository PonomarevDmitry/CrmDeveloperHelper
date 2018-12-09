using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeXmlUpdateSiteMapCommand : AbstractCommand
    {
        private CodeXmlUpdateSiteMapCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeXmlUpdateSiteMapCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeXmlUpdateSiteMapCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeXmlUpdateSiteMapCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType);

            if (selectedFiles.Count == 1)
            {
                helper.HandleSiteMapUpdateCommand(null, selectedFiles.FirstOrDefault());
            }
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(command, menuCommand, out var doc, "SiteMap");

            if (doc != null)
            {
                string nameCommand = Properties.CommandNames.CodeXmlUpdateSiteMapCommand;

                var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSiteMapNameUnique);

                if (attribute != null && !string.IsNullOrEmpty(attribute.Value))
                {
                    nameCommand = string.Format(Properties.CommandNames.CodeXmlUpdateSiteMapByNameCommandFormat1, attribute.Value);
                }

                CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, nameCommand);
            }
        }
    }
}
