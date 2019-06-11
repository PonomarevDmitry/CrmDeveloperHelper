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
        private CodeXmlSystemFormExplorerCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.CodeXmlSystemFormExplorerCommandId) { }

        public static CodeXmlSystemFormExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlSystemFormExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
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

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(applicationObject, menuCommand, Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeFormId, out var attribute, AbstractDynamicCommandXsdSchemas.RootForm);
        }
    }
}
