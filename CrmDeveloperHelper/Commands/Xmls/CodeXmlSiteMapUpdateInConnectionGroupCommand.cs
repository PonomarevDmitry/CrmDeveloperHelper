using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlSiteMapUpdateInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithoutCurrent
    {
        private CodeXmlSiteMapUpdateInConnectionGroupCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CodeXmlSiteMapUpdateInConnectionGroupCommandId
            )
        {

        }

        public static CodeXmlSiteMapUpdateInConnectionGroupCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlSiteMapUpdateInConnectionGroupCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleSiteMapUpdateCommand(connectionData, selectedFiles.FirstOrDefault());
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(applicationObject
                , menuCommand
                , out var doc
                , AbstractDynamicCommandXsdSchemas.RootSiteMap
            );

            if (doc != null)
            {
                string siteMapUniqueName = "Default";

                var attribute = doc.Attribute(Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeSiteMapNameUnique);

                if (attribute != null && !string.IsNullOrEmpty(attribute.Value))
                {
                    siteMapUniqueName = attribute.Value;
                }

                menuCommand.Text = string.Format(Properties.CommandNames.CommandNameWithConnectionFormat2, siteMapUniqueName, connectionData.Name);
            }
        }
    }
}
