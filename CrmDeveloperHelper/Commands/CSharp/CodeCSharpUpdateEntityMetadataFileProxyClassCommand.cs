using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpUpdateEntityMetadataFileProxyClassCommand : AbstractDynamicCommandByConnectionAll
    {
        private CodeCSharpUpdateEntityMetadataFileProxyClassCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeCSharpUpdateEntityMetadataFileProxyClassCommandId
            )
        {

        }

        public static CodeCSharpUpdateEntityMetadataFileProxyClassCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeCSharpUpdateEntityMetadataFileProxyClassCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsCSharpType).ToList();

            helper.HandleCSharpEntityMetadataFileUpdateProxyClass(connectionData, selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(applicationObject, menuCommand);
        }
    }
}