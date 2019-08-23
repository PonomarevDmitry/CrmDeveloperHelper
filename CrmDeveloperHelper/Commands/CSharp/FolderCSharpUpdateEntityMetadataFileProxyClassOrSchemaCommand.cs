using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FolderCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand : AbstractDynamicCommandByConnectionAll
    {
        private FolderCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommandId
            )
        {

        }

        public static FolderCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsCSharpType, true).ToList();

            helper.HandleUpdateEntityMetadataFileCSharpProxyClassOrSchema(connectionData, selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpRecursive(applicationObject, menuCommand);
        }
    }
}