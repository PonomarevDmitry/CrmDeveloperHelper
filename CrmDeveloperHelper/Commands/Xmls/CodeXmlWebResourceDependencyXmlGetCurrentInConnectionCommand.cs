using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlWebResourceDependencyXmlGetCurrentInConnectionCommand : AbstractDynamicCommandByConnectionWithoutCurrent
    {
        private CodeXmlWebResourceDependencyXmlGetCurrentInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CodeXmlWebResourceDependencyXmlGetCurrentInConnectionCommandId)
        {
        }

        public static CodeXmlWebResourceDependencyXmlGetCurrentInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlWebResourceDependencyXmlGetCurrentInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleWebResourceDependencyXmlGetCurrentCommand(connectionData, selectedFiles.FirstOrDefault());
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(applicationObject
                , menuCommand
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeWebResourceName
                , out var attribute
                , AbstractDynamicCommandXsdSchemas.RootWebResourceDependencies
            );

            if (attribute == null || string.IsNullOrEmpty(attribute.Value))
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }
    }
}