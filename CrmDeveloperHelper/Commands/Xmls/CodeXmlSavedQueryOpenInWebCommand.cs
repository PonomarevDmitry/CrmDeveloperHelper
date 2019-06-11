using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlSavedQueryOpenInWebCommand : AbstractDynamicCommandByConnectionAll
    {
        private CodeXmlSavedQueryOpenInWebCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CodeXmlSavedQueryOpenInWebCommandId
            )
        {

        }

        public static CodeXmlSavedQueryOpenInWebCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlSavedQueryOpenInWebCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleSavedQueryOpenInWebCommand(connectionData, selectedFiles[0]);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(
                applicationObject
                , menuCommand
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId
                , out var attribute
                , CommonExportXsdSchemasCommand.RootFetch
                , CommonExportXsdSchemasCommand.RootGrid
                , CommonExportXsdSchemasCommand.RootColumnSet
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
