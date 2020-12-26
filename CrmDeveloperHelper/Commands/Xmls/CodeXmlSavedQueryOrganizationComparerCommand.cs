using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlSavedQueryOrganizationComparerCommand : AbstractDynamicCommandConnectionPair
    {
        private CodeXmlSavedQueryOrganizationComparerCommand(OleMenuCommandService commandService, int baseIdStart, string formatButtonName)
            : base(commandService, baseIdStart, formatButtonName)
        {
        }

        public static CodeXmlSavedQueryOrganizationComparerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlSavedQueryOrganizationComparerCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeXmlSavedQueryOrganizationComparerCommandId
                , Properties.CommandNames.ShowDifferenceTwoConnectionsCommandFormat2
            );
        }

        protected override void CommandAction(DTEHelper helper, Tuple<ConnectionData, ConnectionData> connectionDataPair)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                string fileText = File.ReadAllText(selectedFiles[0].FilePath);

                if (ContentComparerHelper.TryParseXml(fileText, out var doc))
                {
                    var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSavedQueryId);

                    helper.HandleOpenSavedQueryOrganizationComparerCommand(connectionDataPair.Item1, connectionDataPair.Item2, attribute?.Value);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, Tuple<ConnectionData, ConnectionData> connectionDataPair, OleMenuCommand menuCommand)
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
