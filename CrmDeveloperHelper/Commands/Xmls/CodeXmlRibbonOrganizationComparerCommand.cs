using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlRibbonOrganizationComparerCommand : AbstractDynamicCommandConnectionPair
    {
        private CodeXmlRibbonOrganizationComparerCommand(OleMenuCommandService commandService, int baseIdStart, string formatButtonName)
            : base(commandService, baseIdStart, formatButtonName)
        {
        }

        public static CodeXmlRibbonOrganizationComparerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlRibbonOrganizationComparerCommand(
                commandService
                , PackageIds.guidDynamicConnectionPairCommandSet.CodeXmlRibbonOrganizationComparerCommandId
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
                    var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName);

                    string entityName = attribute?.Value;

                    if (!string.IsNullOrEmpty(entityName))
                    {
                        helper.HandleOpenEntityMetadataOrganizationComparerCommand(connectionDataPair.Item1, connectionDataPair.Item2, entityName);
                    }
                    else
                    {
                        helper.HandleOpenApplicationRibbonOrganizationComparerCommand(connectionDataPair.Item1, connectionDataPair.Item2);
                    }
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, Tuple<ConnectionData, ConnectionData> connectionDataPair, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(applicationObject
                , menuCommand
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeEntityName
                , out var attribute
                , AbstractDynamicCommandXsdSchemas.RibbonXmlRoot
                , AbstractDynamicCommandXsdSchemas.RibbonDiffXmlRoot
            );
        }
    }
}
