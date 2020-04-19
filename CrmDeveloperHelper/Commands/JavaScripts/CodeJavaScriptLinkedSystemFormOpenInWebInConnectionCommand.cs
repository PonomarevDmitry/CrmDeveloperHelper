using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeJavaScriptLinkedSystemFormOpenInWebInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private CodeJavaScriptLinkedSystemFormOpenInWebInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedSystemFormOpenInWebInConnectionCommandId)
        {
        }

        public static CodeJavaScriptLinkedSystemFormOpenInWebInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptLinkedSystemFormOpenInWebInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsJavaScriptType).Take(2).ToList();

            if (selectedFiles.Count != 1)
            {
                return;
            }

            string fileText = File.ReadAllText(selectedFiles[0].FilePath);

            if (CommonHandlers.GetLinkedSystemForm(fileText, out string entityName, out Guid formId, out int formType))
            {
                connectionData.OpenSystemFormInWeb(entityName, formId, formType);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScript(applicationObject, menuCommand);

            //CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(
            //    applicationObject
            //    , menuCommand
            //    , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId
            //    , out var attribute
            //    , AbstractDynamicCommandXsdSchemas.RootForm
            //);

            //if (attribute == null || !Guid.TryParse(attribute.Value, out _))
            //{
            //    menuCommand.Enabled = menuCommand.Visible = false;
            //}
        }
    }
}
