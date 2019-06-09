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
    internal sealed class CodeXmlSavedQueryExplorerCommand : AbstractCommand
    {
        private CodeXmlSavedQueryExplorerCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeXmlSavedQueryExplorerCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeXmlSavedQueryExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeXmlSavedQueryExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                string fileText = File.ReadAllText(selectedFiles[0].FilePath);

                if (ContentCoparerHelper.TryParseXml(fileText, out var doc))
                {
                    var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId);

                    helper.HandleExplorerSystemSavedQuery(attribute?.Value);
                }
            }
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(command, menuCommand
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId
                , out var attribute
                , CommonExportXsdSchemasCommand.RootFetch
                , CommonExportXsdSchemasCommand.RootGrid
                , CommonExportXsdSchemasCommand.RootColumnSet
            );
        }
    }
}
