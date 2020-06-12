using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlWebResourceDependencyXmlUpdateCommand : AbstractSingleCommand
    {
        private CodeXmlWebResourceDependencyXmlUpdateCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlWebResourceDependencyXmlUpdateCommandId)
        {
        }

        public static CodeXmlWebResourceDependencyXmlUpdateCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlWebResourceDependencyXmlUpdateCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleWebResourceDependencyXmlUpdateCommand(null, selectedFiles.FirstOrDefault());
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(
                applicationObject
                , menuCommand
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeWebResourceName
                , out var attribute
                , AbstractDynamicCommandXsdSchemas.RootWebResourceDependencies
            );

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeXmlWebResourceDependencyXmlUpdateCommand);

            if (attribute == null || string.IsNullOrEmpty(attribute.Value))
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }
    }
}