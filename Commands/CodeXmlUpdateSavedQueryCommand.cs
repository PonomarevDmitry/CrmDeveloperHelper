using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeXmlUpdateSavedQueryCommand : AbstractCommand
    {
        private CodeXmlUpdateSavedQueryCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeXmlUpdateSavedQueryCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeXmlUpdateSavedQueryCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeXmlUpdateSavedQueryCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType);

            if (selectedFiles.Count == 1)
            {
                helper.HandleSavedQueryUpdateCommand(null, selectedFiles.FirstOrDefault());
            }
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(command, menuCommand
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId
                , out var attribute
                , "fetch"
                , "grid"
                , "columnset"
                );

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CodeXmlUpdateSavedQueryCommand);

            if (attribute == null
                || !Guid.TryParse(attribute.Value, out _)
                )
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }
    }
}
