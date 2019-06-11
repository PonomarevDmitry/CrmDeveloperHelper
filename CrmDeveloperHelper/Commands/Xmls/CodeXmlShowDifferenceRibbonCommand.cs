using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlShowDifferenceRibbonCommand : AbstractCommand
    {
        private CodeXmlShowDifferenceRibbonCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.CodeXmlShowDifferenceRibbonCommandId) { }

        public static CodeXmlShowDifferenceRibbonCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlShowDifferenceRibbonCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleRibbonDifferenceCommand(null, selectedFiles.FirstOrDefault());
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(applicationObject, menuCommand, Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName, out var attribute, AbstractDynamicCommandXsdSchemas.RootRibbonDefinitions);

            if (attribute != null)
            {
                string entityName = attribute.Value;

                string nameCommand = Properties.CommandNames.CodeXmlShowDifferenceApplicationRibbonCommand;

                if (!string.IsNullOrEmpty(entityName))
                {
                    nameCommand = string.Format(Properties.CommandNames.CodeXmlShowDifferenceEntityRibbonCommandFormat1, entityName);
                }

                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, nameCommand);
            }
        }
    }
}