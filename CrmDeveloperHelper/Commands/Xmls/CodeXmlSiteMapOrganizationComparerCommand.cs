using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlSiteMapOrganizationComparerCommand : AbstractDynamicCommandConnectionPair
    {
        private CodeXmlSiteMapOrganizationComparerCommand(OleMenuCommandService commandService, int baseIdStart, string formatButtonName)
            : base(commandService, baseIdStart, formatButtonName)
        {
        }

        public static CodeXmlSiteMapOrganizationComparerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlSiteMapOrganizationComparerCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeXmlSiteMapOrganizationComparerCommandId
                , Properties.CommandNames.ShowDifferenceTwoConnectionsCommandFormat2
            );
        }

        protected override void CommandAction(DTEHelper helper, Tuple<ConnectionData, ConnectionData> connectionDataPair)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                string siteMapNameUnique = string.Empty;

                string fileText = File.ReadAllText(selectedFiles[0].FilePath);

                if (ContentComparerHelper.TryParseXml(fileText, out var doc))
                {
                    var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSiteMapNameUnique);

                    if (attribute != null && !string.IsNullOrEmpty(attribute.Value))
                    {
                        siteMapNameUnique = attribute.Value;
                    }
                }

                helper.HandleOpenSiteMapOrganizationComparerCommand(connectionDataPair.Item1, connectionDataPair.Item2, siteMapNameUnique);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, Tuple<ConnectionData, ConnectionData> connectionDataPair, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(
                applicationObject
                , menuCommand
                , out var doc
                , AbstractDynamicCommandXsdSchemas.SiteMapXmlRoot
            );
        }
    }
}
