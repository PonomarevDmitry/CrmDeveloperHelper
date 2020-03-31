using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class DocumentsCSharpUpdateEntityMetadataFileSchemaCommand : AbstractDynamicCommandByConnectionAll
    {
        private DocumentsCSharpUpdateEntityMetadataFileSchemaCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsCSharpUpdateEntityMetadataFileSchemaCommandId
            )
        {

        }

        public static DocumentsCSharpUpdateEntityMetadataFileSchemaCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsCSharpUpdateEntityMetadataFileSchemaCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsCSharpType).ToList();

            helper.HandleCSharpEntityMetadataFileUpdateSchema(connectionData, selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsCSharp(applicationObject, menuCommand);
        }
    }
}