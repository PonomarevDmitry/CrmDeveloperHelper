using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpUpdateEntityMetadataFileSchemaCommand : AbstractCommandByConnectionAll
    {
        private CodeCSharpUpdateEntityMetadataFileSchemaCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CodeCSharpUpdateEntityMetadataFileSchemaCommandId
            )
        {

        }

        public static CodeCSharpUpdateEntityMetadataFileSchemaCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeCSharpUpdateEntityMetadataFileSchemaCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsCSharpType).ToList();

            helper.HandleUpdateEntityMetadataFileCSharpSchema(connectionData, selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(applicationObject, menuCommand);
        }
    }
}