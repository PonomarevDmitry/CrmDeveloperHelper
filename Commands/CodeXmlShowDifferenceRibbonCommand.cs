using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeXmlShowDifferenceRibbonCommand : AbstractCommand
    {
        private CodeXmlShowDifferenceRibbonCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeXmlShowDifferenceRibbonCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeXmlShowDifferenceRibbonCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeXmlShowDifferenceRibbonCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType);

            if (selectedFiles.Count == 1)
            {
                helper.HandleRibbonDifferenceCommand(null, selectedFiles.FirstOrDefault());
            }
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(command, menuCommand, "RibbonDefinitions", Intellisense.Model.RibbonIntellisenseData.IntellisenseContextAttributeEntityName, out var attribute);

            if (attribute != null)
            {
                string entityName = attribute.Value;

                string nameCommand = Properties.CommandNames.CodeXmlShowDifferenceApplicationRibbonCommand;

                if (!string.IsNullOrEmpty(entityName))
                {
                    nameCommand = string.Format(Properties.CommandNames.CodeXmlShowDifferenceEntityRibbonCommandFormat1, entityName);
                }

                CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, nameCommand);
            }
        }
    }
}
