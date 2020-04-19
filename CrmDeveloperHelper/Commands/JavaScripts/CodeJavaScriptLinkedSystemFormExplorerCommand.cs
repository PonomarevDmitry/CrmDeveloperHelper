using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeJavaScriptLinkedSystemFormExplorerCommand : AbstractCommand
    {
        private CodeJavaScriptLinkedSystemFormExplorerCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeJavaScriptLinkedSystemFormExplorerCommandId)
        {
        }

        public static CodeJavaScriptLinkedSystemFormExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptLinkedSystemFormExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsJavaScriptType).Take(2).ToList();

            if (selectedFiles.Count != 1)
            {
                return;
            }

            string fileText = File.ReadAllText(selectedFiles[0].FilePath);

            if (CommonHandlers.GetLinkedSystemForm(fileText, out string entityName, out Guid formId, out int formType))
            {
                helper.HandleExplorerSystemForm(entityName, formId.ToString());
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScript(applicationObject, menuCommand);

            //CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(
            //    applicationObject
            //    , menuCommand
            //    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId
            //    , out var attribute
            //    , AbstractDynamicCommandXsdSchemas.RootForm
            //);
        }
    }
}
