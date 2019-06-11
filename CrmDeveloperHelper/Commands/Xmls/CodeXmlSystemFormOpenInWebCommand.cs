using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlSystemFormOpenInWebCommand : AbstractDynamicCommandByConnectionAll
    {
        private CodeXmlSystemFormOpenInWebCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CodeXmlSystemFormOpenInWebCommandId
            )
        {

        }

        public static CodeXmlSystemFormOpenInWebCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlSystemFormOpenInWebCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleSystemFormOpenInWebCommand(connectionData, selectedFiles[0]);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(
                applicationObject
                , menuCommand
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId
                , out var attribute
                , CommonExportXsdSchemasCommand.RootForm
            );

            if (attribute == null
                || !Guid.TryParse(attribute.Value, out _)
            )
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }
    }
}
