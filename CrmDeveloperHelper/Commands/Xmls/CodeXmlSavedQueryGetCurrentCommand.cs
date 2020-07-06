using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlSavedQueryGetCurrentCommand : AbstractSingleCommand
    {
        private CodeXmlSavedQueryGetCurrentCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlSavedQueryGetCurrentCommandId)
        {
        }

        public static CodeXmlSavedQueryGetCurrentCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlSavedQueryGetCurrentCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleSavedQueryGetCurrentCommand(null, selectedFiles.FirstOrDefault());
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(
                applicationObject
                , menuCommand
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId
                , out var attribute
                , AbstractDynamicCommandXsdSchemas.FetchRoot
                , AbstractDynamicCommandXsdSchemas.GridRoot
                , AbstractDynamicCommandXsdSchemas.ColumnSetRoot
            );

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeXmlSavedQueryGetCurrentCommand);

            if (attribute == null || !Guid.TryParse(attribute.Value, out _))
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }
    }
}