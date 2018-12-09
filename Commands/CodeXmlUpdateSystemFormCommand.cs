using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeXmlUpdateSystemFormCommand : AbstractCommand
    {
        private CodeXmlUpdateSystemFormCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeXmlUpdateSystemFormCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeXmlUpdateSystemFormCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeXmlUpdateSystemFormCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType);

            if (selectedFiles.Count == 1)
            {
                helper.HandleSystemFormUpdateCommand(null, selectedFiles.FirstOrDefault());
            }
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(command, menuCommand, Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId, out var attribute, "FormXml");

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CodeXmlUpdateSystemFormCommand);

            if (attribute == null
                || !Guid.TryParse(attribute.Value, out _)
                )
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }
    }
}
