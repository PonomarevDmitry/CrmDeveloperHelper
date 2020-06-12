using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlRibbonShowDifferenceCommand : AbstractSingleCommand
    {
        private CodeXmlRibbonShowDifferenceCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlRibbonShowDifferenceCommandId) { }

        public static CodeXmlRibbonShowDifferenceCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlRibbonShowDifferenceCommand(commandService);
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

                string nameCommand = Properties.CommandNames.CodeXmlRibbonApplicationShowDifferenceCommand;

                if (!string.IsNullOrEmpty(entityName))
                {
                    nameCommand = string.Format(Properties.CommandNames.CodeXmlRibbonEntityShowDifferenceCommandFormat1, entityName);
                }

                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, nameCommand);
            }
        }
    }
}