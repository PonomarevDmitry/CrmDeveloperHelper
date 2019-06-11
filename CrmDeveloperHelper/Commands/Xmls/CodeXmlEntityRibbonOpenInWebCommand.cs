using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlEntityRibbonOpenInWebCommand : AbstractCommandByConnectionAll
    {
        private CodeXmlEntityRibbonOpenInWebCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CodeXmlEntityRibbonOpenInWebCommandId
            )
        {

        }

        public static CodeXmlEntityRibbonOpenInWebCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlEntityRibbonOpenInWebCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleEntityRibbonOpenInWeb(connectionData, selectedFiles.FirstOrDefault());
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(
                applicationObject
                , menuCommand
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName
                , out var attribute
                , CommonExportXsdSchemasCommand.RootRibbonDiffXml
                , CommonExportXsdSchemasCommand.RootRibbonDefinitions
            );

            if (attribute != null)
            {
                string entityName = attribute.Value;

                if (string.IsNullOrEmpty(entityName))
                {
                    menuCommand.Enabled = menuCommand.Visible = false;
                }
            }
        }
    }
}
