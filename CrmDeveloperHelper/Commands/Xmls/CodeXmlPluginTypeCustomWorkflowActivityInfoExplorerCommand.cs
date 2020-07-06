using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.XPath;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlPluginTypeCustomWorkflowActivityInfoExplorerCommand : AbstractSingleCommand
    {
        private CodeXmlPluginTypeCustomWorkflowActivityInfoExplorerCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlPluginTypeCustomWorkflowActivityInfoExplorerCommandId)
        {
        }

        public static CodeXmlPluginTypeCustomWorkflowActivityInfoExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlPluginTypeCustomWorkflowActivityInfoExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                string fileText = File.ReadAllText(selectedFiles[0].FilePath);

                if (ContentComparerHelper.TryParseXml(fileText, out var doc))
                {
                    var pluginTypeName = doc.XPathSelectElements("./CustomActivityInfo/TypeName").Where(e => !string.IsNullOrEmpty(e.Value)).Select(e => e.Value).FirstOrDefault();

                    helper.HandleOpenPluginTypeExplorer(pluginTypeName);
                }
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(
                applicationObject
                , menuCommand
                , out var doc
                , AbstractDynamicCommandXsdSchemas.PluginTypeCustomWorkflowActivityInfoRoot
            );

            if (doc != null && menuCommand.Enabled & menuCommand.Visible)
            {
                var pluginTypeName = doc.XPathSelectElements("./CustomActivityInfo/TypeName").Where(e => !string.IsNullOrEmpty(e.Value)).Select(e => e.Value).FirstOrDefault();

                if (string.IsNullOrEmpty(pluginTypeName))
                {
                    menuCommand.Enabled = menuCommand.Visible = false;
                }
            }

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeXmlPluginTypeCustomWorkflowActivityInfoExplorerCommand);
        }
    }
}