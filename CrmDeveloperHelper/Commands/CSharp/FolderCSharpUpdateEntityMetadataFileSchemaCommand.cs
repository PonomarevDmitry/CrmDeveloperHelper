using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FolderCSharpUpdateEntityMetadataFileSchemaCommand : AbstractCommandByConnectionAll
    {
        private FolderCSharpUpdateEntityMetadataFileSchemaCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.FolderCSharpUpdateEntityMetadataFileSchemaCommandId
            )
        {

        }

        public static FolderCSharpUpdateEntityMetadataFileSchemaCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderCSharpUpdateEntityMetadataFileSchemaCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsCSharpType, true).ToList();

            helper.HandleUpdateEntityMetadataFileCSharpSchema(connectionData, selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpRecursive(applicationObject, menuCommand);
        }
    }
}