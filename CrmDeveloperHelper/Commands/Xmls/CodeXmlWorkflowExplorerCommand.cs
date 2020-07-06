using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlWorkflowExplorerCommand : AbstractSingleCommand
    {
        private CodeXmlWorkflowExplorerCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlWorkflowExplorerCommandId) { }

        public static CodeXmlWorkflowExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlWorkflowExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                string fileText = File.ReadAllText(selectedFiles[0].FilePath);

                if (ContentComparerHelper.TryParseXml(fileText, out var doc))
                {
                    var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeWorkflowId);

                    helper.HandleExplorerWorkflows(attribute?.Value);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(applicationObject, menuCommand
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeWorkflowId
                , out var attribute
                , AbstractDynamicCommandXsdSchemas.WorkflowActivityRoot
            );
        }
    }
}
