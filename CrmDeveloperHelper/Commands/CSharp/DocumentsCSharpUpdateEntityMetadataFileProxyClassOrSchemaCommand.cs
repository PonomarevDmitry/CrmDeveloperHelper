using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class DocumentsCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand : AbstractDynamicCommandByConnectionAll
    {
        private DocumentsCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.DocumentsCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommandId
            )
        {

        }

        public static DocumentsCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsCSharpType).ToList();

            helper.HandleUpdateEntityMetadataFileCSharpProxyClassOrSchema(connectionData, selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsCSharp(applicationObject, menuCommand);
        }
    }
}