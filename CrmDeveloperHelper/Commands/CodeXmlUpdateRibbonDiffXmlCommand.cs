using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeXmlUpdateRibbonDiffXmlCommand : AbstractCommand
    {
        private CodeXmlUpdateRibbonDiffXmlCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeXmlUpdateRibbonDiffXmlCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeXmlUpdateRibbonDiffXmlCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeXmlUpdateRibbonDiffXmlCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType);

            if (selectedFiles.Count == 1)
            {
                helper.HandleRibbonDiffXmlUpdateCommand(null, selectedFiles.FirstOrDefault());
            }
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(command, menuCommand, Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName, out var attribute, "RibbonDiffXml");

            if (attribute != null)
            {
                string entityName = attribute.Value;

                string nameCommand = Properties.CommandNames.CodeXmlUpdateApplicationRibbonDiffXmlCommand;

                if (!string.IsNullOrEmpty(entityName))
                {
                    nameCommand = string.Format(Properties.CommandNames.CodeXmlUpdateEntityRibbonDiffXmlCommandFormat1, entityName);
                }

                CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(command, menuCommand);

                CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, nameCommand);
            }
        }
    }
}
