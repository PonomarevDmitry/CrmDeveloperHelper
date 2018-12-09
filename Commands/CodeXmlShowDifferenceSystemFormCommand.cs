using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeXmlShowDifferenceSystemFormCommand : AbstractCommand
    {
        private CodeXmlShowDifferenceSystemFormCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeXmlShowDifferenceSystemFormCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeXmlShowDifferenceSystemFormCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeXmlShowDifferenceSystemFormCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType);

            if (selectedFiles.Count == 1)
            {
                helper.HandleSystemFormDifferenceCommand(null, selectedFiles.FirstOrDefault());
            }
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(command, menuCommand, Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId, out var attribute, "FormXml");

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CodeXmlShowDifferenceSystemFormCommand);

            if (attribute == null
                || !Guid.TryParse(attribute.Value, out _)
                )
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }
    }
}
