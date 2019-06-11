using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlUpdateRibbonDiffXmlCommand : AbstractCommand
    {
        private CodeXmlUpdateRibbonDiffXmlCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.CodeXmlUpdateRibbonDiffXmlCommandId) { }

        public static CodeXmlUpdateRibbonDiffXmlCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlUpdateRibbonDiffXmlCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleRibbonDiffXmlUpdateCommand(null, selectedFiles.FirstOrDefault());
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(applicationObject, menuCommand, Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName, out var attribute, AbstractDynamicCommandXsdSchemas.RootRibbonDiffXml);

            if (attribute != null)
            {
                string entityName = attribute.Value;

                string nameCommand = Properties.CommandNames.CodeXmlUpdateApplicationRibbonDiffXmlCommand;

                if (!string.IsNullOrEmpty(entityName))
                {
                    nameCommand = string.Format(Properties.CommandNames.CodeXmlUpdateEntityRibbonDiffXmlCommandFormat1, entityName);
                }

                CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);

                CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, nameCommand);
            }
        }
    }
}
