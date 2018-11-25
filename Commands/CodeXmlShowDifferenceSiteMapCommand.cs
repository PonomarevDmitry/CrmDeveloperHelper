using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeXmlShowDifferenceSiteMapCommand : AbstractCommand
    {
        private CodeXmlShowDifferenceSiteMapCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeXmlShowDifferenceSiteMapCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeXmlShowDifferenceSiteMapCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeXmlShowDifferenceSiteMapCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType);

            if (selectedFiles.Count == 1)
            {
                helper.HandleSiteMapDifferenceCommand(null, selectedFiles.FirstOrDefault());
            }
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(command, menuCommand, "SiteMap");
            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CodeXmlShowDifferenceSiteMapCommand);

            //CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(command, menuCommand, "SiteMap", Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName, out var attribute);

            //if (attribute != null)
            //{
            //    string entityName = attribute.Value;

            //    string nameCommand = Properties.CommandNames.CodeXmlShowDifferenceApplicationRibbonCommand;

            //    if (!string.IsNullOrEmpty(entityName))
            //    {
            //        nameCommand = string.Format(Properties.CommandNames.CodeXmlShowDifferenceEntityRibbonCommandFormat1, entityName);
            //    }

            //    CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, nameCommand);
            //}
        }
    }
}
