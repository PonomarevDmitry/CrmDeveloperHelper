﻿using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlWebResourceDependencyXmlOpenInWebInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private CodeXmlWebResourceDependencyXmlOpenInWebInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CodeXmlWebResourceDependencyXmlOpenInWebInConnectionCommandId)
        {
        }

        public static CodeXmlWebResourceDependencyXmlOpenInWebInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlWebResourceDependencyXmlOpenInWebInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleWebResourceDependencyXmlOpenInWebCommand(connectionData, selectedFiles[0]);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRootWithAttribute(
                applicationObject
                , menuCommand
                , Intellisense.Model.IntellisenseContext.IntellisenseContextAttributeWebResourceName
                , out var attribute
                , AbstractDynamicCommandXsdSchemas.WebResourceDependencyXmlRoot
            );

            if (attribute == null || string.IsNullOrEmpty(attribute.Value))
            {
                menuCommand.Enabled = menuCommand.Visible = false;
            }
        }
    }
}
