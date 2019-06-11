using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlRibbonExplorerCommand : AbstractCommand
    {
        private CodeXmlRibbonExplorerCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeXmlRibbonExplorerCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeXmlRibbonExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeXmlRibbonExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleRibbonExplorerCommand(null, selectedFiles.FirstOrDefault());
            }
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(command
                , menuCommand
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName
                , out var attribute
                , AbstractDynamicCommandXsdSchemas.RootRibbonDefinitions
                , AbstractDynamicCommandXsdSchemas.RootRibbonDiffXml
            );

            if (attribute != null)
            {
                string entityName = attribute.Value;

                string nameCommand = Properties.CommandNames.CodeXmlApplicationRibbonExplorerCommand;

                if (!string.IsNullOrEmpty(entityName))
                {
                    nameCommand = Properties.CommandNames.CodeXmlEntityRibbonExplorerCommand;
                }

                CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, nameCommand);
            }
        }
    }
}
