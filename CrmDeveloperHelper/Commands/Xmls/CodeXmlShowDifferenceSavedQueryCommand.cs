using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlShowDifferenceSavedQueryCommand : AbstractCommand
    {
        private CodeXmlShowDifferenceSavedQueryCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.CodeXmlShowDifferenceSavedQueryCommandId) { }

        public static CodeXmlShowDifferenceSavedQueryCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlShowDifferenceSavedQueryCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleSavedQueryDifferenceCommand(null, selectedFiles.FirstOrDefault());
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(applicationObject, menuCommand
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId
                , out var attribute
                , AbstractDynamicCommandXsdSchemas.RootFetch
                , AbstractDynamicCommandXsdSchemas.RootGrid
                , AbstractDynamicCommandXsdSchemas.RootColumnSet
            );

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeXmlShowDifferenceSavedQueryCommand);

            if (attribute == null
                || !Guid.TryParse(attribute.Value, out _)
                )
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }
    }
}
