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
    internal sealed class CodeXmlSavedQueryExplorerCommand : AbstractSingleCommand
    {
        private CodeXmlSavedQueryExplorerCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlSavedQueryExplorerCommandId) { }

        public static CodeXmlSavedQueryExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlSavedQueryExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                string fileText = File.ReadAllText(selectedFiles[0].FilePath);

                if (ContentComparerHelper.TryParseXml(fileText, out var doc))
                {
                    var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId);

                    helper.HandleExplorerSystemSavedQuery(attribute?.Value);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(applicationObject, menuCommand
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId
                , out var attribute
                , AbstractDynamicCommandXsdSchemas.FetchRoot
                , AbstractDynamicCommandXsdSchemas.GridRoot
                , AbstractDynamicCommandXsdSchemas.ColumnSetRoot
            );
        }
    }
}
