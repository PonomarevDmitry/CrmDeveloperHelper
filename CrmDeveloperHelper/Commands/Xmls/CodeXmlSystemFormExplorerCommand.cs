using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlSystemFormExplorerCommand : AbstractCommand
    {
        private CodeXmlSystemFormExplorerCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeXmlSystemFormExplorerCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeXmlSystemFormExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeXmlSystemFormExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                string fileText = File.ReadAllText(selectedFiles[0].FilePath);

                if (ContentCoparerHelper.TryParseXml(fileText, out var doc))
                {
                    var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId);

                    helper.HandleExplorerSystemForm(attribute?.Value);
                }
            }
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(command, menuCommand, Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId, out var attribute, CommonExportXsdSchemasCommand.RootForm);
        }
    }
}
