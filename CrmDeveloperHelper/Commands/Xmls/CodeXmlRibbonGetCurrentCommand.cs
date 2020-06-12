using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlRibbonGetCurrentCommand : AbstractSingleCommand
    {
        private CodeXmlRibbonGetCurrentCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlRibbonGetCurrentCommandId)
        {
        }

        public static CodeXmlRibbonGetCurrentCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlRibbonGetCurrentCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleRibbonGetCurrentCommand(null, selectedFiles.FirstOrDefault());
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(
                applicationObject
                , menuCommand
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName
                , out var attribute
                , AbstractDynamicCommandXsdSchemas.RootRibbonDiffXml
                , AbstractDynamicCommandXsdSchemas.RootRibbonDefinitions
            );

            if (attribute != null)
            {
                string entityName = attribute.Value;

                string nameCommand = Properties.CommandNames.CodeXmlRibbonApplicationGetCurrentCommand;

                if (!string.IsNullOrEmpty(entityName))
                {
                    nameCommand = string.Format(Properties.CommandNames.CodeXmlRibbonEntityGetCurrentCommandFormat1, entityName);
                }

                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, nameCommand);
            }
        }
    }
}