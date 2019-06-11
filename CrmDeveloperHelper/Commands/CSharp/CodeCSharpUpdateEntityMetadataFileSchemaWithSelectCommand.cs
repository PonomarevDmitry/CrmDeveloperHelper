using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpUpdateEntityMetadataFileSchemaWithSelectCommand : AbstractCommand
    {
        private CodeCSharpUpdateEntityMetadataFileSchemaWithSelectCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.CodeCSharpUpdateEntityMetadataFileSchemaWithSelectCommandId) { }

        public static CodeCSharpUpdateEntityMetadataFileSchemaWithSelectCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeCSharpUpdateEntityMetadataFileSchemaWithSelectCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsCSharpType).ToList();

            helper.HandleUpdateEntityMetadataFileCSharpSchema(null, selectedFiles, true);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeCSharpUpdateEntityMetadataFileSchemaWithSelectCommand);
        }
    }
}