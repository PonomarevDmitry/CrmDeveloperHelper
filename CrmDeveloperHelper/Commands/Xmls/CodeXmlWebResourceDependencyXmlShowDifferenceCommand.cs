using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlWebResourceDependencyXmlShowDifferenceCommand : AbstractSingleCommand
    {
        private CodeXmlWebResourceDependencyXmlShowDifferenceCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlWebResourceDependencyXmlShowDifferenceCommandId)
        {
        }

        public static CodeXmlWebResourceDependencyXmlShowDifferenceCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlWebResourceDependencyXmlShowDifferenceCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleWebResourceDependencyXmlDifferenceCommand(null, selectedFiles.FirstOrDefault());
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(
                applicationObject
                , menuCommand
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeWebResourceName
                , out var attribute
                , AbstractDynamicCommandXsdSchemas.WebResourceDependencyXmlRoot
            );

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeXmlWebResourceDependencyXmlShowDifferenceCommand);

            if (attribute == null || string.IsNullOrEmpty(attribute.Value))
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }
    }
}
