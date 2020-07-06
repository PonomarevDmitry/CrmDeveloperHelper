using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlPluginTypeCustomWorkflowActivityInfoGetCurrentInConnectionCommand : AbstractDynamicCommandByConnectionAllWithoutCurrent
    {
        private CodeXmlPluginTypeCustomWorkflowActivityInfoGetCurrentInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CodeXmlPluginTypeCustomWorkflowActivityInfoGetCurrentInConnectionCommandId)
        {
        }

        public static CodeXmlPluginTypeCustomWorkflowActivityInfoGetCurrentInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlPluginTypeCustomWorkflowActivityInfoGetCurrentInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandlePluginTypeCustomWorkflowActivityInfoGetCurrentCommand(connectionData, selectedFiles[0]);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
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
        }
    }
}